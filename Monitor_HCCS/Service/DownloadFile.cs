using Monitor_HCCS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Monitor_HCCS.Service
{
    public class DownloadFile
    {

        public delegate void ValueChangedHandler(object sender, EventArgs e);
        public event ValueChangedHandler ProgressSer;
        public event ValueChangedHandler StateTextChanged;

        private static Encoding encode = Encoding.ASCII;
        FileStream fs;
        public Socket client;
        public int whileCount = 0;
        private bool isUpdata;

        public bool IsUpdata
        {
            get { return isUpdata; }
            set { isUpdata = value; }
        }
        
        private string stateText;
        public string StateText
        {
            get { return stateText; }
            set
            {
                if (value != stateText)
                {
                    stateText = value;
                    StateTextChanged.Invoke(this, null);
                }
            }
        }
        public int percentage = 0;
        public int Percentage
        {
            get { return percentage; }
            set
            {
                if (value != percentage)
                {
                    percentage = value;
                    ProgressSer.Invoke(this, null);
                }
            }
        }

        //建立socket连接
        public bool SocketConnet(string host, int port)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ipe);
                //超时设置为2分钟
                //client.ReceiveTimeout = 1000 * 2 * 60;

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }

        //接收消息
        private string Receive(Socket socket, int timeout)
        {
            string result = string.Empty;
            socket.ReceiveTimeout = timeout;
            List<byte> data = new List<byte>();
            byte[] buffer = new byte[2048];
            int length = 0;
            try
            {
                while ((length = socket.Receive(buffer)) > 0)
                {
                    for (int j = 0; j < length; j++)
                        data.Add(buffer[j]);

                    if (length < buffer.Length)
                        break;
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
            }

            if (data.Count > 0)
                result = encode.GetString(data.ToArray(), 0, data.Count);

            return result;
        }

        //销毁Socket对象
        private void DestroySocket(Socket socket)
        {
            if (socket.Connected)
                socket.Shutdown(SocketShutdown.Both);

            socket.Close();
        }

        //按照固定格式从服务器下载
        public string DownloadBin()
        {
                        
            String PUT1 = "PUT1";
            String PUT3 = "PUT3";
            
            string file = null;
            Log4netHelper.Info("开始下载固件升级包..");
            int allCRC = 0xffff;
            bool downloadDone = false; //文件是否下载完成
            try
            {                

                byte[] buff = new byte[2048];
                int read;

                #region 循环读取

                while ((read = client.Receive(buff)) != 0)
                {

                    whileCount++;

                    //app3
                    if (read == 10)
                    {
                        string fn = encode.GetString(buff).Replace('\0', ' ').Trim() + ".bin";
                        string[] fns = fn.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        file = fns[1];
                        XmlHelper.setVer(file);
                        if (File.Exists(file))
                            File.Delete(file);

                        //fs = new FileStream(fns[1], FileMode.Append, FileAccess.Write);
                        fs = File.Open(fns[1], FileMode.Append, FileAccess.Write, FileShare.Write);
                        continue;
                    }
                        
                    
                    int pos = 0;//数据文件的字节位置
                    int k = 0;

                    //查找数据头
                    for (int i = 0; i < read; i++)
                    {
                        if (whileCount > 2)
                        {
                            if (buff[i] == ',')
                                k++;

                            if (k == 4)
                            {
                                pos = i + 1;
                                break;
                            }
                        }
                        else
                        {
                            if (buff[i] == ',')
                                pos = i + 1;
                        }
                    }

                    if (pos <= 0)
                    {
                        Log4netHelper.Info("无法找到数据头");
                        string login = encode.GetString(buff).Replace('\0', ' ').Trim();

                        switch (login)
                        {
                            case "INVALID":
                                return "访问无效！";

                            case "BUSY":
                                return "服务器忙！";

                            case "WELCOME":
                                if (whileCount > 1)
                                {
                                    sendData(encode.GetBytes("APP2," + StateText));
                                    continue;
                                }
                                    
                                sendData(encode.GetBytes("APP0"));
                                continue;
                        }

                    }

                    //读取数据头数据转字符串
                    String str = new String(encode.GetChars(buff), 0, pos);
                    Log4netHelper.Info("接收数据:" + str);
                    //按照逗号截取数据
                    //"PUT0,total_send,curr_send,curr_crc,"+数据内容
                    string[] strArray = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (strArray[0].Equals("APP1"))
                    {
                        StateText = FindMaxVersion(strArray);
                        IsUpdata = true;
                        DestroySocket(client);
                        return "";
                    }

                    string put;
                    long total_send;
                    int curr_send;
                    int curr_crc;
                    put = strArray[0];
                    // 创建FileOutputStream对象
                    if (fs == null)
                        //fs = new FileStream(file, FileMode.Append, FileAccess.Write);
                        fs = File.Open(file, FileMode.Append, FileAccess.Write, FileShare.Write);
                    total_send = Convert.ToInt32(strArray[1]);
                    curr_send = Convert.ToInt32(strArray[2]);
                    curr_crc = Convert.ToInt32(strArray[3]);

                    Percentage += 2;

                    if (put.Equals("PUT2"))
                    {

                        int crc = getDispersetAllCRC16(allCRC);
                        if (curr_crc != crc)
                        {
                            Log4netHelper.Info("所有数据CRC16验证失败!" + crc + ":" + curr_crc);
                            IsUpdata = false;
                            return "所有数据CRC16验证失败!";
                        }
                        downloadDone = true;
                        sendData(encode.GetBytes(PUT3));

                        whileCount = 0;
                        Percentage = 0;
                        IsUpdata = false;
                        return "下载成功";
                    }

                    //完整的一个文件数据
                    byte[] bytes = new byte[curr_send];

                    //当前已读取的数据中是否有完整的一个数据包
                    //当前数据中的，图片文件数据段
                    int currDataLength = read - pos;

                    Array.Copy(buff, pos, bytes, 0, currDataLength);//把数据字节存起来

                    if (curr_send > currDataLength)
                    {
                        //不完整，则读取完整的数据包
                        //剩余缺失的数据大小
                        int need = curr_send - currDataLength;
                        //读取剩余数据
                        do
                        {
                            if ((read = fs.Read(bytes, currDataLength, need)) != 0)
                            {
                                if (read != need)
                                {
                                    need = need - read;
                                    currDataLength += read;
                                }
                                else
                                {
                                    break;
                                }
                                //System.arraycopy(buff, 0, bytes, currDataLength, read);//把数据字节存起来
                            }
                        } while (true);

                    }
                    allCRC = dispersetCRC16(allCRC, bytes, bytes.Length);
                    int crc1 = getCRC16(bytes, bytes.Length);
                    if (crc1 == curr_crc)
                    {
                        if (fs == null)
                            return "文件流已关闭";
                        // 往文件所在的缓冲输出流中写byte数据
                        fs.Write(bytes, 0, bytes.Length);
                        // 刷出缓冲输出流
                        fs.Flush();
                        Log4netHelper.Info("发送 PUT1");
                        if (!sendData(encode.GetBytes(PUT1)))
                        {
                            return "接收数据失败";
                        }
                    }
                    else
                    {
                        Log4netHelper.Info("接收的数据CRC16验证失败!" + crc1 + ":" + curr_crc);
                        return "接收的数据CRC16验证失败!";
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return ex.Message;
            }
            finally
            {
                try
                {
                    if (!downloadDone && file != null)
                    {
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                    if (fs != null)
                        fs.Close();
                }
                catch (IOException ex)
                {
                    Log4netHelper.Error(ex);
                }

            }
            return "下载更新文件失败";
        }


        //发送数据  参数byte[]
        public bool sendData(byte[] bytes)
        {
            try
            {
                client.Send(bytes);

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }
        //发送数据重载方法 参数string
        public bool Send(string Data)
        {
            try
            {
                client.Send(encode.GetBytes(Data));

                return true;
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(ex);
                return false;
            }
        }
        //CRC校验
        public int getCRC16(byte[] bytes, int length)
        {
            int Reg_CRC = 0xffff;
            int temp;
            int i, j;
            for (i = 0; i < length; i++)
            {
                temp = bytes[i];
                if (temp < 0) temp += 256;
                temp &= 0xff;
                Reg_CRC ^= temp;
                for (j = 0; j < 8; j++)
                {
                    if ((Reg_CRC & 0x0001) == 0x0001)
                        Reg_CRC = (Reg_CRC >> 1) ^ 0xA001;
                    else
                        Reg_CRC >>= 1;
                }
            }
            Reg_CRC &= 0xFFFF;
            int crc = ((Reg_CRC << 8) & 0xFF00) | ((Reg_CRC >> 8) & 0x00FF);
            return crc;
        }

        public int dispersetCRC16(int Reg_CRC, byte[] bytes, int length)
        {
            //Reg_CRC = Reg_CRC==0?0xffff:Reg_CRC;
            int temp;
            int i, j;
            for (i = 0; i < length; i++)
            {
                temp = bytes[i];
                if (temp < 0) temp += 256;
                temp &= 0xff;
                Reg_CRC ^= temp;
                for (j = 0; j < 8; j++)
                {
                    if ((Reg_CRC & 0x0001) == 0x0001)
                        Reg_CRC = (Reg_CRC >> 1) ^ 0xA001;
                    else
                        Reg_CRC >>= 1;
                }
            }

            return Reg_CRC;
        }

        //全部数据的CRC校验
        public int getDispersetAllCRC16(int Reg_CRC)
        {
            Reg_CRC &= 0xFFFF;
            int crc = ((Reg_CRC << 8) & 0xFF00) | ((Reg_CRC >> 8) & 0x00FF);
            return crc;
        }

        //返回一组序列中的最大版本号
        public string FindMaxVersion(string[] str)
        {
            List<string> list = new List<string>();
            string ver = XmlHelper.getValue("flag");
            string suffix = "";

            for (int i = 0; i < str.Length; i++)
            {                
                if (str[i].Contains(ver))
                {
                    string[] str2 = str[i].Split(new char[] { Convert.ToChar(ver) });
                    list.Add(str2[0]);
                    suffix = str2[1];
                }
            }

            str = list.ToArray();
            float[] array = new float[str.Length];

            for (int i = 0; i < str.Length; i++)
            {
                array[i] = Convert.ToSingle(str[i]);
            }

            string version = array.Max() + ver + suffix;
            return version;
        }
    }
}

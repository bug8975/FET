using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace BenNHControl
{
    public class PicHelper
    {
        /// <summary>
        /// 水印图
        /// </summary>
        protected static readonly string WaterImage = ConfigurationManager.AppSettings["WaterImage"];

        /// <summary>
        ///     缩放图像
        /// </summary>
        /// <param name="stream">图片流</param>
        /// <param name="thumNailPath">保存路径</param>
        /// <param name="width">缩放图的宽</param>
        /// <param name="height">缩放图的高</param>
        /// <param name="model">缩放模式</param>
        /// <param name="IsWaterMark">是否加水印</param>
        public static void MakeThumNail(Stream stream, string thumNailPath, int width, int height, string model,
            bool IsWaterMark = true)
        {
            var originalImage = Image.FromStream(stream);

            var thumWidth = width; //缩略图的宽度
            var thumHeight = height; //缩略图的高度

            var x = 0;
            var y = 0;

            var originalWidth = originalImage.Width; //原始图片的宽度
            var originalHeight = originalImage.Height; //原始图片的高度

            switch (model)
            {
                case "HW": //指定高宽缩放,可能变形
                    break;
                case "W": //指定宽度,高度按照比例缩放
                    thumHeight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H": //指定高度,宽度按照等比例缩放
                    thumWidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":
                    if (originalImage.Width / (double)originalImage.Height > thumWidth / (double)thumHeight)
                    {
                        originalHeight = originalImage.Height;
                        originalWidth = originalImage.Height * thumWidth / thumHeight;
                        y = 0;
                        x = (originalImage.Width - originalWidth) / 2;
                    }
                    else
                    {
                        originalWidth = originalImage.Width;
                        originalHeight = originalWidth * height / thumWidth;
                        x = 0;
                        y = (originalImage.Height - originalHeight) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new Bitmap(thumWidth, thumHeight);

            //新建一个画板
            var graphic = Graphics.FromImage(bitmap);

            //设置高质量查值法
            graphic.InterpolationMode = InterpolationMode.High;

            //设置高质量，低速度呈现平滑程度
            graphic.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            graphic.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            graphic.DrawImage(originalImage, new Rectangle(0, 0, thumWidth, thumHeight),
                new Rectangle(x, y, originalWidth, originalHeight), GraphicsUnit.Pixel);

            try
            {
                if (IsWaterMark)
                {
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        if (!string.IsNullOrEmpty(WaterImage))
                        {
                            using (var watermark = Image.FromFile(WaterImage))
                            {
                                float[][] nArray =
                                {
                                    new float[] {1, 0, 0, 0, 0},
                                    new float[] {0, 1, 0, 0, 0},
                                    new float[] {0, 0, 1, 0, 0},
                                    new[] {0, 0, 0, 0.6f, 0},
                                    new float[] {0, 0, 0, 0, 1}
                                };
                                var colormatrix = new ColorMatrix(nArray);
                                var attributes = new ImageAttributes();
                                attributes.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                g.DrawImage(watermark,
                                    new Rectangle(Convert.ToInt32(bitmap.Width * 0.7), Convert.ToInt32(bitmap.Height * 0.7),
                                        Convert.ToInt32(bitmap.Width * 0.3), Convert.ToInt32(bitmap.Height * 0.3)), 0, 0,
                                    watermark.Width, watermark.Height, GraphicsUnit.Pixel, attributes);
                                //在图片指定坐标处放入一个矩形图片内容为水印图片
                            }
                        }
                    }
                }
                bitmap.Save(thumNailPath, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                graphic.Dispose();
            }
        }


        //public static void WaterMark(HttpPostedFileBase item, string filePath)
        //{
        //    var ext = item.FileName.Substring(item.FileName.LastIndexOf('.')).ToLower();
        //    if (ext == ".gif" || ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp")
        //    {
        //        using (Image image = new Bitmap(item.InputStream))
        //        {
        //            try
        //            {
        //                using (var g = Graphics.FromImage(image))
        //                {
        //                    if (!string.IsNullOrEmpty(WaterImage))
        //                    {
        //                        using (var watermark = Image.FromFile(HttpContext.Current.Server.MapPath(WaterImage)))
        //                        {
        //                            float[][] nArray =
        //                            {
        //                                new float[] {1, 0, 0, 0, 0},
        //                                new float[] {0, 1, 0, 0, 0},
        //                                new float[] {0, 0, 1, 0, 0},
        //                                new[] {0, 0, 0, 0.6f, 0},
        //                                new float[] {0, 0, 0, 0, 1}
        //                            };
        //                            var colormatrix = new ColorMatrix(nArray);
        //                            var attributes = new ImageAttributes();
        //                            attributes.SetColorMatrix(colormatrix, ColorMatrixFlag.Default,
        //                                ColorAdjustType.Bitmap);
        //                            g.DrawImage(watermark,
        //                                new Rectangle(Convert.ToInt32(image.Width * 0.7),
        //                                    Convert.ToInt32(image.Height * 0.7),
        //                                    Convert.ToInt32(image.Width * 0.3), Convert.ToInt32(image.Height * 0.3)), 0, 0,
        //                                watermark.Width, watermark.Height, GraphicsUnit.Pixel, attributes);
        //                            //在图片指定坐标处放入一个矩形图片内容为水印图片
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                LoggerManager.Error(e.Message);
        //            }
        //            finally
        //            {
        //                image.Save(filePath);
        //            }
        //        }
        //    }
        //}


        public static void WaterMark(byte[] buffer, string filePath)
        {
            var ms = new MemoryStream(buffer);
            using (var image = Image.FromStream(ms))
            {
                using (var g = Graphics.FromImage(image))
                {
                    if (!string.IsNullOrEmpty(WaterImage))
                    {
                        using (var watermark = Image.FromFile(WaterImage))
                        {
                            float[][] nArray =
                            {
                                new float[] {1, 0, 0, 0, 0},
                                new float[] {0, 1, 0, 0, 0},
                                new float[] {0, 0, 1, 0, 0},
                                new[] {0, 0, 0, 0.6f, 0},
                                new float[] {0, 0, 0, 0, 1}
                            };
                            var colormatrix = new ColorMatrix(nArray);
                            var attributes = new ImageAttributes();
                            attributes.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            g.DrawImage(watermark,
                                new Rectangle(Convert.ToInt32(image.Width * 0.7), Convert.ToInt32(image.Height * 0.7),
                                    Convert.ToInt32(image.Width * 0.3), Convert.ToInt32(image.Height * 0.3)), 0, 0,
                                watermark.Width, watermark.Height, GraphicsUnit.Pixel, attributes);
                            //在图片指定坐标处放入一个矩形图片内容为水印图片
                        }
                    }
                }
                image.Save(filePath);
            }
        }

        #region 根据原图片生成等比缩略图

        /// <summary>
        ///     根据源图片生成缩略图
        /// </summary>
        /// <param name = "imgPathOld" > 源图(大图)物理路径</param>
        /// <param name = "imgPathNew" > 缩略图物理路径(生成的缩略图将保存到该物理位置) </ param >
        /// < param name="width">缩略图宽度</param>
        /// <param name = "height" > 缩略图高度 </ param >
        /// < param name="mode">缩略图缩放模式(取值"HW":指定高宽缩放, 可能变形；取值"W":按指定宽度, 高度按比例缩放；取值"H":按指定高度, 宽度按比例缩放；取值"Cut":按指定高度和宽度裁剪, 不变形)；取值"DB":等比缩放,以值较大的作为标准进行等比缩放</param>
        /// <param name = "imageType" > 即将生成缩略图的文件的扩展名(仅限：JPG、GIF、PNG、BMP) </ param >
        /// < param name="xx"></param>
        /// <param name = "yy" ></ param >
        public static void MakeThumbnail(string imgPathOld, string imgPathNew, int width, int height, string mode,
            string imageType, int xx, int yy)
        {
            var img = Image.FromFile(imgPathOld);
            var towidth = width;
            var toheight = height;
            var x = 0;
            var y = 0;
            var ow = img.Width;
            var oh = img.Height;
            switch (mode)
            {
                case "HW": //指定高宽压缩
                    //    if (img.Width/(double) img.Height > width/(double) height) //判断图形是什么形状
                    //    {
                    //        towidth = width;
                    //        toheight = img.Height*width/img.Width;
                    //    }
                    //    else if (img.Width/(double) img.Height == width/(double) height)
                    //    {
                    towidth = width;
                    toheight = height;
                    //}
                    //else
                    //{
                    //    toheight = height;
                    //    towidth = img.Width * height / img.Height;
                    //}
                    break;
                case "hw":
                    if (img.Width / (double)img.Height > width / (double)height) //判断图形是什么形状
                    {
                        towidth = width;
                        toheight = img.Height * width / img.Width;
                    }
                    else if (img.Width / (double)img.Height == width / (double)height)
                    {
                        towidth = width;
                        toheight = height;
                    }
                    else
                    {
                        toheight = height;
                        towidth = img.Width * height / img.Height;
                    }
                    break;

                case "W": //指定宽，高按比例   
                    toheight = img.Height * width / img.Width;
                    break;
                case "H": //指定高，宽按比例  
                    towidth = img.Width * height / img.Height;
                    break;
                case "Cut": //指定高宽裁减（不变形）   
                    if (img.Width / (double)img.Height > towidth / (double)toheight)
                    {
                        oh = img.Height;
                        ow = img.Height * towidth / toheight;
                        y = yy;
                        x = (img.Width - ow) / 2;
                    }
                    else
                    {
                        ow = img.Width;
                        oh = img.Width * height / towidth;
                        x = xx;
                        y = (img.Height - oh) / 2;
                    }
                    break;
                case "DB": // 按值较大的进行等比缩放（不变形）   
                    if (img.Width / (double)towidth < img.Height / (double)toheight)
                    {
                        toheight = height;
                        towidth = img.Width * height / img.Height;
                    }
                    else
                    {
                        towidth = width;
                        toheight = img.Height * width / img.Width;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片  
            Image bitmap = new Bitmap(towidth, toheight);
            //Image bitmap = new Bitmap(180, 145);

            //新建一个画板  
            var g = Graphics.FromImage(bitmap);
            //设置高质量插值法  
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度  
            g.SmoothingMode = SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充  
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分  
            g.DrawImage(img, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图 
                switch (imageType.ToLower())
                {
                    case "gif":
                        img.Save(imgPathNew, ImageFormat.Gif); //生成缩略图
                        break;
                    case "jpg":
                        bitmap.Save(imgPathNew, ImageFormat.Jpeg);
                        break;
                    case "bmp":
                        bitmap.Save(imgPathNew, ImageFormat.Bmp);
                        break;
                    case "png":
                        bitmap.Save(imgPathNew, ImageFormat.Png);
                        break;
                    default:
                        bitmap.Save(imgPathNew, ImageFormat.Jpeg);
                        break;
                }
                ////保存缩略图  
                // bitmap.Save(imgPath_new);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                img.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        #endregion

        public static Image GetNewPic(Image img, int width, int height)
        {
            return new Bitmap(img, width, height);

        }


        #region 图片缩略图测试方法

        public static void MakeThumbnail(string imgPathOld, string imgPathNew, int _width, int _height, string mode,
            string imageType)
        {
            try
            {
                //原图加载
                using (var sourceImage = Image.FromFile(imgPathOld))
                {
                    //原图宽度和高度
                    var width = sourceImage.Width;
                    var height = sourceImage.Height;
                    int smallWidth;
                    int smallHeight;
                    //获取第一张绘制图的大小,(比较 原图的宽/缩略图的宽  和 原图的高/缩略图的高)
                    if (((decimal)width) / height <= ((decimal)_width) / _height) //这给地方可能是出错的地方，也就是图片不清晰
                    {
                        smallWidth = _width;
                        smallHeight = _width * height / width;
                    }
                    else
                    {
                        smallWidth = _height * width / height;
                        smallHeight = _height;
                    }

                    //要保存的缩略图路径
                    var smallImagePath = imgPathNew;

                    //新建一个图板,以最小等比例压缩大小绘制原图
                    using (Image bitmap = new Bitmap(smallWidth, smallHeight))
                    {
                        //绘制中间图
                        using (var g = Graphics.FromImage(bitmap))
                        {
                            //高清,平滑
                            g.InterpolationMode = InterpolationMode.High;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear(Color.Black);
                            g.DrawImage(
                                sourceImage,
                                new Rectangle(0, 0, smallWidth, smallHeight),
                                new Rectangle(0, 0, width, height),
                                GraphicsUnit.Pixel
                                );
                        }
                        //新建一个图板,以缩略图大小绘制中间图
                        using (Image bitmap1 = new Bitmap(_width, _height))
                        {
                            //绘制缩略图
                            using (var g = Graphics.FromImage(bitmap1))
                            {
                                //高清,平滑
                                g.InterpolationMode = InterpolationMode.High;
                                g.SmoothingMode = SmoothingMode.HighQuality;
                                g.Clear(Color.Black);
                                var lwidth = (smallWidth - _width) / 2;
                                var bheight = (smallHeight - _height) / 2;
                                //g.DrawImage(bitmap, new Rectangle(0, 0, _width, _height), lwidth, bheight,
                                g.DrawImage(bitmap, new Rectangle(0, 0, _width, _height), 0, 0, smallWidth, smallHeight, GraphicsUnit.Pixel);
                                g.Dispose();
                                bitmap1.Save(smallImagePath, ImageFormat.Jpeg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ////出错则删除
                //System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(imgPathOld));
                //return "图片格式不正确";
                throw ex;
            }
        }

        #endregion

        //#region 裁剪头像
        ///// <summary>
        ///// 裁剪头像图片
        ///// </summary>
        ///// <param name="pointX">X坐标</param>
        ///// <param name="pointY">Y坐标</param>
        ///// <param name="imgUrl">被截图图片地址</param>
        ///// <param name="width">截图矩形的宽</param>
        ///// <param name="height">截图矩形的高</param>
        ///// <returns>裁剪后图片的url</returns>
        //public static string CutAvatar(string imgUrl, int pointX = 0, int pointY = 0, int width = 0, int height = 0)
        //{
        //    Bitmap bitmap = null;   //按截图区域生成Bitmap
        //    Image thumbImg = null;  //被截图 
        //    Graphics gps = null;    //存绘图对象   
        //    Image finalImg = null;  //最终图片
        //    try
        //    {
        //        int finalWidth = 180;
        //        int finalHeight = 180;
        //        if (!string.IsNullOrEmpty(imgUrl))
        //        {
        //            bitmap = new System.Drawing.Bitmap(width, height);
        //            thumbImg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgUrl));
        //            gps = System.Drawing.Graphics.FromImage(bitmap);      //读到绘图对象
        //            gps.DrawImage(thumbImg, new Rectangle(0, 0, width, height), new Rectangle(pointX, pointY, width, height), GraphicsUnit.Pixel);

        //            finalImg = GetThumbNailImage(bitmap, finalWidth, finalHeight);

        //            //以下代码为保存图片时，设置压缩质量  
        //            EncoderParameters ep = new EncoderParameters();
        //            long[] qy = new long[1];
        //            qy[0] = 80;//设置压缩的比例1-100  
        //            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
        //            ep.Param[0] = eParam;

        //            ImageCodecInfo[] arrayIci = ImageCodecInfo.GetImageEncoders();
        //            ImageCodecInfo jpegIcIinfo = null;
        //            for (int x = 0; x < arrayIci.Length; x++)
        //            {
        //                if (arrayIci[x].FormatDescription.Equals("JPEG"))
        //                {
        //                    jpegIcIinfo = arrayIci[x];
        //                    break;
        //                }
        //            }

        //            string finalUrl = imgUrl.Replace("TemporaryFiles", "FinalUserAvatar");
        //            string finalPath = HttpContext.Current.Server.MapPath(finalUrl);//最终图片保存的地址（绝对地址）
        //            string finalPathDir = finalPath.Substring(0, finalPath.LastIndexOf("\\", StringComparison.Ordinal));//最终图片保存地址的文件夹

        //            if (!Directory.Exists(finalPathDir))
        //            {
        //                Directory.CreateDirectory(finalPathDir);
        //            }

        //            if (jpegIcIinfo != null)
        //            {
        //                finalImg.Save(finalPath, jpegIcIinfo, ep);
        //            }
        //            else
        //            {
        //                finalImg.Save(finalPath);
        //            }

        //            return finalUrl;

        //        }
        //        return "";
        //    }
        //    catch (Exception)
        //    {
        //        return "";
        //    }
        //    finally
        //    {
        //        if (bitmap != null) bitmap.Dispose();
        //        if (thumbImg != null) thumbImg.Dispose();
        //        if (gps != null) gps.Dispose();
        //        if (finalImg != null) finalImg.Dispose();

        //        //删除被截取的图片

        //        GC.Collect();
        //        System.IO.File.Delete(HttpContext.Current.Server.MapPath(imgUrl));

        //    }
        //}

        ///// <summary>
        ///// 对指定的图片对象生成缩略图
        ///// </summary>
        /////<param name="originalImage">原始图片</param>
        /////<param name="thumMaxWidth">缩略图的宽度</param>
        /////<param name="thumMaxHeight">缩略图的高度</param>
        /////<returns>返回缩略图的Image对象</returns>
        //private static Image GetThumbNailImage(Image originalImage, int thumMaxWidth, int thumMaxHeight)
        //{
        //    Image newImage = originalImage;
        //    Graphics graphics = null;
        //    try
        //    {
        //        var thumRealSize = GetNewSize(thumMaxWidth, thumMaxHeight, originalImage.Width, originalImage.Height);
        //        newImage = new System.Drawing.Bitmap(thumRealSize.Width, thumRealSize.Height);
        //        graphics = Graphics.FromImage(newImage);
        //        graphics.DrawImage(originalImage, new Rectangle(0, 0, thumRealSize.Width, thumRealSize.Height), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);
        //    }
        //    catch
        //    {
        //        // ignored
        //    }
        //    finally
        //    {
        //        if (graphics != null)
        //        {
        //            graphics.Dispose();
        //            graphics = null;
        //        }
        //    }
        //    return newImage;
        //}

        ///// <summary>
        ///// 获取一个图片按等比例缩小后的大小
        ///// </summary>
        /////<param name="maxWidth">需要缩小到的宽度</param>
        /////<param name="maxHeight">需要缩小到的高度</param>
        /////<param name="imageOriginalWidth">图片的原始宽度</param>
        /////<param name="imageOriginalHeight">图片的原始高度</param>
        /////<returns>返回图片按等比例缩小后的实际大小</returns>
        //public static System.Drawing.Size GetNewSize(int maxWidth, int maxHeight, int imageOriginalWidth, int imageOriginalHeight)
        //{
        //    double w = 0.0;
        //    double h = 0.0;
        //    double sw = Convert.ToDouble(imageOriginalWidth);
        //    double sh = Convert.ToDouble(imageOriginalHeight);
        //    double mw = Convert.ToDouble(maxWidth);
        //    double mh = Convert.ToDouble(maxHeight);
        //    if (sw < mw && sh < mh)
        //    {
        //        w = sw;
        //        h = sh;
        //    }
        //    else if ((sw / sh) > (mw / mh))
        //    {
        //        w = maxWidth;
        //        h = (w * sh) / sw;
        //    }
        //    else
        //    {
        //        h = maxHeight;
        //        w = (h * sw) / sh;
        //    }
        //    return new System.Drawing.Size(Convert.ToInt32(w), Convert.ToInt32(h));
        //}

        //#endregion

        public static Bitmap PercentImage(Image srcImage)
        {
            int newW = srcImage.Width < 1130 ? srcImage.Width : 1130;
            int newH = int.Parse(Math.Round(srcImage.Height * (double)newW / srcImage.Width).ToString());
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                g.DrawImage(srcImage, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //将图片按百分比压缩，flag取值1到100，越小压缩比越大

        public static bool YaSuo(Image iSource, string outPath, int flag)
        {
            ImageFormat tFormat = iSource.RawFormat;
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageDecoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                    iSource.Save(outPath, jpegICIinfo, ep);
                else
                    iSource.Save(outPath, tFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 画点
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static void DrawPoint(Image image, int x, int y, Color color)
        {
            Bitmap bit = image as Bitmap;
            if (bit != null)
            {
                bit.SetPixel(x, y, color);
            }
        }

    }
}
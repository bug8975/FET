using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Monitor_HCCS.Common.GeoProspector
{
    class SurferHandler
    {
        public static bool SurferKriging(string normalizationsFileName, int width, int height, string outPutFileName, int pointNum, string fet, string gears)
        {
            if (!File.Exists(normalizationsFileName))
            {
                return false;
            }

            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            string colorFileName = filePath + "fill.clr";
            string dataFileName = filePath + "data.grd";

            if (!File.Exists(colorFileName))
            {
                return false;
            }

            Surfer.Application app = new Surfer.Application();
            app.WindowState = Surfer.SrfWindowState.srfWindowStateMinimized;
            app.Visible = false;
            app.Caption = "kriging";
            app.GridData(normalizationsFileName, Algorithm: Surfer.SrfGridAlgorithm.srfKriging, DupMethod: Surfer.SrfDupMethod.srfDupNone, ShowReport: false, OutGrid: dataFileName);

            //此处改动，强制转换Surfer.IPlotDocument
            Surfer.IPlotDocument plot = (Surfer.IPlotDocument)app.Documents.Add(Surfer.SrfDocTypes.srfDocPlot);
            Surfer.IMapFrame map = plot.Shapes.AddContourMap(dataFileName);

            map.Axes.Item(1).Visible = false;
            map.Axes.Item(2).Visible = false;
            map.Axes.Item(3).Visible = false;
            map.Axes.Item(4).Visible = false;

            Surfer.IContourMap contour = (Surfer.IContourMap)map.Overlays.Item(1);
            contour.FillContours = true;
            contour.FillForegroundColorMap.LoadFile(colorFileName);
            contour.ApplyFillToLevels(1, 1, 0);

            contour.ShowColorScale = true;
            contour.ColorScale.Height = 3.0;
            //if (pointNum <= 50)
            //    contour.ColorScale.Height = 3.0;
            //else if (pointNum > 50 && pointNum <= 140)
            //    contour.ColorScale.Height = 2.0;
            //else if (pointNum > 140 && pointNum <= 200)
            //    contour.ColorScale.Height = 1.0;
            //else if (pointNum > 200 && pointNum <= 300)
            //    contour.ColorScale.Height = 0.5;
            //else
            //    contour.ColorScale.Height = 0.25;

            #region 修改

            #region 依据设备型号调整色柱图宽度和左边距

            //色柱图基点

            //EX系列
            if (fet.Contains("M"))
            {
                if (pointNum >= 50 && pointNum < 140)
                {
                    contour.ColorScale.Width = 0.006 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.001 * pointNum;
                    contour.ColorScale.Height = 1.2;
                }
                else if (pointNum >= 140 && pointNum < 210)
                {
                    contour.ColorScale.Width = 0.004 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0006 * pointNum;
                    contour.ColorScale.Height = 0.6;
                }
                else if (pointNum >= 210)
                {
                    contour.ColorScale.Width = 0.0025 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0004 * pointNum;
                    contour.ColorScale.Height = 0.4;
                }
                else
                {
                    contour.ColorScale.Width = 0.015 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.002 * pointNum;
                    contour.ColorScale.Height = 3.0;
                }
            }
            else if (fet.Contains("N"))
            {
                if (pointNum >= 67 && pointNum < 140)
                {
                    contour.ColorScale.Width = 0.006 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.001 * pointNum;
                    contour.ColorScale.Height = 2.0;
                }
                else if (pointNum >= 140 && pointNum < 210)
                {
                    contour.ColorScale.Width = 0.004 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0006 * pointNum;
                    contour.ColorScale.Height = 1.0;
                }
                else if (pointNum >= 210)
                {
                    contour.ColorScale.Width = 0.0025 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0004 * pointNum;
                    contour.ColorScale.Height = 0.5;
                }
                else
                {
                    contour.ColorScale.Width = 0.009 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0012 * pointNum;
                }
                
            }
            else if (fet.Contains("O"))
            {
                if (pointNum > 50 && pointNum < 140)
                {
                    contour.ColorScale.Width = 0.006 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.001 * pointNum;
                    contour.ColorScale.Height = 2.0;
                }
                else if (pointNum >= 140 && pointNum < 210)
                {
                    contour.ColorScale.Width = 0.004 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0006 * pointNum;
                    contour.ColorScale.Height = 1.0;
                }
                else if (pointNum >= 210)
                {
                    contour.ColorScale.Width = 0.0025 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0004 * pointNum;
                    contour.ColorScale.Height = 0.5;
                }
                else
                {
                    contour.ColorScale.Width = 0.009 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0012 * pointNum;
                }
            }
            else if (fet.Contains("P"))
            {
                if (pointNum > 50 && pointNum < 140)
                {
                    contour.ColorScale.Width = 0.006 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.001 * pointNum;
                    contour.ColorScale.Height = 2.0;
                }
                else if (pointNum >= 140 && pointNum < 210)
                {
                    contour.ColorScale.Width = 0.004 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0006 * pointNum;
                    contour.ColorScale.Height = 1.0;
                }
                else if (pointNum >= 210)
                {
                    contour.ColorScale.Width = 0.0025 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0004 * pointNum;
                    contour.ColorScale.Height = 1.0;
                }
                else
                {
                    contour.ColorScale.Width = 0.009 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0012 * pointNum;
                }
            }
            else if (fet.Contains("Q"))
            {
                if (pointNum >= 50 && pointNum < 140)
                {
                    contour.ColorScale.Width = 0.006 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.001 * pointNum;
                    contour.ColorScale.Height = 2.0;
                }
                else if (pointNum >= 140 && pointNum < 210)
                {
                    contour.ColorScale.Width = 0.004 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0006 * pointNum;
                    contour.ColorScale.Height = 1.0;
                }
                else if (pointNum >= 210)
                {
                    contour.ColorScale.Width = 0.0025 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0004 * pointNum;
                    contour.ColorScale.Height = 1.0;
                }
                else
                {
                    contour.ColorScale.Width = 0.005 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.001 * pointNum;
                }
            }
            //2000A
            else if (fet.Contains("S"))
            {
                if (pointNum >= 31 && pointNum < 61)
                {
                    contour.ColorScale.Width = 0.0006 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
                    contour.ColorScale.Height = 2.0;
                }
                else if (pointNum >= 61)
                {
                    contour.ColorScale.Width = 0.0003 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.00015 * pointNum;
                    contour.ColorScale.Height = 1.5;
                }
                else
                {
                    contour.ColorScale.Width = 0.0012 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
                }
            }
            //3000A
            else if (fet.Contains("R"))
            {
                if (pointNum >= 31 && pointNum < 61)
                {
                    contour.ColorScale.Width = 0.0004 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
                    contour.ColorScale.Height = 2.0;
                }
                else if (pointNum >= 61)
                {
                    contour.ColorScale.Width = 0.0003 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.00015 * pointNum;
                    contour.ColorScale.Height = 2.0;
                }
                else
                {
                    contour.ColorScale.Width = 0.0012 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
                }
            }
            //300A
            else if (fet.Contains("F"))
            {
                if(gears.Equals("100"))
                {
                    contour.ColorScale.Width = 0.005 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.001 * pointNum;
                }
                else if (gears.Equals("200"))
                {
                    contour.ColorScale.Width = 0.0025 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
                }
                else
                {
                    contour.ColorScale.Width = 0.0015 * pointNum;
                    contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
                }
            }
            else if (fet.Contains("G") && gears.Equals("200"))
            {
                contour.ColorScale.Width = 0.0025 * pointNum;
                contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
            }
            else if (fet.Contains("H") && gears.Equals("200"))
            {
                contour.ColorScale.Width = 0.0025 * pointNum;
                contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
            }
            else if (fet.Contains("I") && gears.Equals("200"))
            {
                contour.ColorScale.Width = 0.0025 * pointNum;
                contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
            }
            else if (fet.Contains("J") && gears.Equals("200"))
            {
                contour.ColorScale.Width = 0.0025 * pointNum;
                contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
            }
            else
            {
                contour.ColorScale.Width = 0.0015 * pointNum;
                contour.ColorScale.Left = contour.Left + contour.Width + 0.0002 * pointNum;
            }
            #endregion

            
            
            contour.ColorScale.Top = 0.0;
            contour.ColorScale.Top = contour.Top - (contour.Height - contour.ColorScale.Height);
            //if (contour.ColorScale.Top == 0.0)
            //    contour.ColorScale.Top = contour.Top - (contour.Height - contour.ColorScale.Height);
            //else
            //    contour.ColorScale.Top = contour.Top + contour.ColorScale.Top;
            
            #endregion

            #region 修改色柱图2
            //contour.ColorScale.Height = 2;
            //contour.ColorScale.Width = 0.03;
            //contour.ColorScale.Top = contour.Top - (contour.Height - contour.ColorScale.Height);
            //contour.ColorScale.Left = contour.Left + contour.Width + 0.01;
            //System.Diagnostics.Trace.WriteLine(contour.Top + "," + contour.Height + "|" + contour.ColorScale.Top + "," + contour.ColorScale.Height);
            #endregion


            string optionsInfo = "Defaults=1";
            if (width > 0 && height > 0)
            {
                optionsInfo = string.Format("Defaults=0,Width={0},Height={1},ColorDepth=32", width, height);
            }

            plot.Export(outPutFileName, Options: optionsInfo);
            app.Documents.CloseAll(Surfer.SrfSaveTypes.srfSaveChangesNo);
            app.Quit();
            System.GC.Collect(System.GC.GetGeneration(app));

            File.Delete(dataFileName);

            return true;
        }
    }
}

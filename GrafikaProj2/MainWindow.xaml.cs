﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes; 
using System.Windows.Threading;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace GrafikaProj2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Figure figure;
        double xDeg = 20;
        double yDeg = 30;
        double zDeg = 40;
        double size = 100;
        bool zmiana = true;

        ZBuffer zBuffer;
        Bitmap bitmap;
        double[,] mt;
        private void fillTopFlatTriangle(double[] v1, double[] v2, double[] v3, byte[] color = null)
        {
            double invslope1 = (v3[0] - v1[0]) / (v3[1] - v1[1]);
            double invslope2 = (v3[0] - v2[0]) / (v3[1] - v2[1]);

            double curx1 = v3[0];
            double curx2 = v3[0];
            Random rand = new Random();
            for (int scanlineY = (int)v3[1]; scanlineY > v1[1]; scanlineY--)
            {
                for (double xstart = curx1; xstart < curx2; xstart++)
                {
                    //this.mt[(int)xstart, scanlineY] = BitConverter.ToInt32(new byte[] { (byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255), 255 }, 0);
                    //this.colorRGB[(int)xstart, scanlineY] = color;
                    try
                    {
                        this.mt[(int)xstart, scanlineY] = 2;

                    }
                    catch (IndexOutOfRangeException) { }
                }
                curx1 -= invslope1;
                curx2 -= invslope2;
            }
        }

        private void fillBottomFlatTriangle(double[] v1, double[] v2, double[] v3, byte[] color = null)
        {
            double invslope1 = (v2[0] - v1[0]) / (v2[1] - v1[1]);
            double invslope2 = (v3[0] - v1[0]) / (v3[1] - v1[1]);

            double curx1 = v1[0];
            double curx2 = v1[0];

            Random rand = new Random();
            for (int scanlineY = (int)v1[1]; scanlineY <= v2[1]; scanlineY++)
            {
                for (double xstart = curx1; xstart < curx2; xstart++)
                {
                    try
                    {
                        this.mt[(int)xstart, scanlineY] = 2;

                    }
                    catch (IndexOutOfRangeException) { }
                }
                curx1 += invslope1;
                curx2 += invslope2;
            }
        }

        public void test(object sender, EventArgs asd)
        {
            mt = new double[(int)Width, (int)Height];
            mainCanvas.Children.Clear();
            Random r1 = new Random();
            fillBottomFlatTriangle(new double[] { r1.Next(0,400), r1.Next(1,100) }, new double[] { r1.Next(100), 200 }, new double[] { r1.Next(101,300), 200 });
            fillTopFlatTriangle(new double[] { 5, 5 }, new double[] { 100, 5 }, new double[] { 10, 100 });
            DrawLineByLine(mt);
        }
        public MainWindow()
        {
            InitializeComponent();
            figure = new Figure(350, 150, 0);
            mt = new double[(int)Width,(int)Height];

            //zBuffer = new ZBuffer((int)Width, (int)Height,  new byte[] { 0, 0, 0 });
            //bitmap = new Bitmap((int)Width, (int)Height);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(100000);
            timer.Tick += test;
            timer.Start();
        }


        private void DrawLineByLine(double[,] depthMatrix)
        {
            int[] actualStartDepth = new int[] { 0, 0 };
            bool newBeg = false;
            for (int i = 0; i < mt.GetLength(0); i++)
            {
                for (int j = 1; j < mt.GetLength(1); j++)
                {
                    if (depthMatrix[i, j] == 0)
                    {
                        if (newBeg)
                        {
                            newBeg = !newBeg;
                            DrawLine(actualStartDepth, new int[] { i, j }, new byte[] { 0, 0, 0 });
                        }
                        continue;
                    }
                    if (!newBeg && depthMatrix[i,j-1]== depthMatrix[i, j] )
                    {
                        actualStartDepth = new int[] { i , j-1 };
                        newBeg = true;
                    }
                    if(newBeg && (depthMatrix[i,j-1]!= depthMatrix[i, j]))
                    {
                        DrawLine(actualStartDepth, new int[] { i, j }, new byte[] { 0, 0, 0 });
                        actualStartDepth = new int[] { i, j };
                    }
                }
            }
        }


        private void DrawSquare(object sender, EventArgs args)
        {
            if (xDeg >= 360) xDeg -= 360;
            if (yDeg >= 360) yDeg -= 360;
            if (zDeg >= 360) zDeg -= 360;
            if (size < 200 && zmiana) size += 1; else zmiana = false;
            if (size > 100 && !zmiana) size -= 1; else zmiana = true;
            Random r = new Random();
            int rand = r.Next(3);
            if (rand == 0) xDeg += 0.5;
            else if (rand == 1) yDeg += 0.5;
            else zDeg += 0.5;
            mainCanvas.Children.Clear();

            figure.Transform(xDeg,yDeg, zDeg, size);
            zBuffer.CalculateDepth(figure.triangles);
            foreach (var triangle in figure.triangles)
            {
                byte[] clr = new byte[] { 0, 0, 0 };//new byte[] { (byte)randd.Next(255), (byte)randd.Next(255), (byte)randd.Next(255) };
                double[] p1 = Figure.actualListOfPoints[triangle.Point1];
                double[] p2 = Figure.actualListOfPoints[triangle.Point2];
                double[] p3 = Figure.actualListOfPoints[triangle.Point3];
                DrawLine(p1, p2, clr);
                DrawLine(p1, p3, clr);
                DrawLine(p2, p3, clr);
            }
            for (int i = 0; i < zBuffer.Surface.GetLength(0); i++)
            {
                for (int j = 0; j < zBuffer.Surface.GetLength(1); j++)
                {
                    if (zBuffer.Surface[i, j] == int.MaxValue) continue;
                    SetPixel(i, j, zBuffer.colorRGB[i, j]);
                    //SetPixel(i, j, zBuffer.colorRGB[i, j]);
                    //if (isSameRGB(lastColor, zBuffer.colorRGB[i,j]) || colorChanged==false)
                    //{
                    //    if (colorChanged == false) { lastI = i; }
                    //    scope += 1;
                    //    colorChanged = true;
                    //    lastColor = zBuffer.colorRGB[i, j];

                    //}
                    //else if (scope > 0)
                    //{
                    //    DrawLine(new int[] { i - scope, j }, new int[] { i, j }, lastColor);
                    //    scope = 0;
                    //    colorChanged = false;
                    //}
                }
            }
            
            
        }
        private bool isSameRGB(byte[] a, byte[] b)
        {
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }

        private void SetPixel(double x, double y, byte[] color)
        {
            Rectangle rec = new Rectangle();
            Canvas.SetTop(rec, y);
            Canvas.SetLeft(rec, x);
            rec.Width = 1;
            rec.Height = 1;
            rec.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color[0], color[1], color[2]));
            mainCanvas.Children.Add(rec);
            //bitmap.SetPixel((int)x, (int)y, System.Drawing.Color.FromArgb(color[0], color[1], color[2]));
            //System.Drawing.Point p = new System.Drawing.Point((int)x, (int)y);
            //mainCanvas.Children.Add();
            //Rectangle rec = new Rectangle();
            //rec.Width = 1; rec.Height = 1;
            //rec.Fill = new SolidColorBrush(Color.FromRgb(color[0], color[1], color[2]));
            //mainCanvas.Children.Add(rec);
        }

        private void DrawLine(double[] start, double[] stop, byte[] color)
        {
            Line l = new Line();
            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color[0], color[1], color[2]));
            //= new SolidColorBrush(Color.FromRgb(color[0], color[1], color[2]));
            l.StrokeThickness = 1;
            l.Stroke = brush;

            l.X1 = (int)start[0];
            l.X2 = (int)stop[0];
            l.Y1 = (int)start[1];
            l.Y2 = (int)stop[1];
            mainCanvas.Children.Add(l);
        }
        private void DrawLine(int[] start, int[] stop, byte[] color)
        {
            Line l = new Line();
            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color[0], color[1], color[2]));
            //= new SolidColorBrush(Color.FromRgb(color[0], color[1], color[2]));
            l.StrokeThickness = 2;
            l.Stroke = brush;

            l.X1 = (int)start[0];
            l.X2 = (int)stop[0];
            l.Y1 = (int)start[1];
            l.Y2 = (int)stop[1];
            mainCanvas.Children.Add(l);
        }

        private void mainWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

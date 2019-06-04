using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            figure = new Figure(350, 150, 0);
            
            zBuffer = new ZBuffer((int)Width, (int)Height,  new byte[] { 0, 0, 0 });

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(100000);
            timer.Tick += DrawSquare;
            timer.Start();
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
            //zBuffer.CalculateDepth(figure.triangles);


            
            for (int i = 0; i < zBuffer.Surface.GetLength(0); i++)
            {
                for (int j = 0; j < zBuffer.Surface.GetLength(1); j++)
                {
                    //if (zBuffer.Surface[i, j] == int.MaxValue) continue;
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
            Brush aBrush = (Brush)Brushes.Black;
            System.Drawing.Graphics g = mainCanvas.CreateGraphics();

            g.FillRectangle(aBrush, x, y, 1, 1);
            //Rectangle rec = new Rectangle();
            //rec.Width = 1; rec.Height = 1;
            //rec.Fill = new SolidColorBrush(Color.FromRgb(color[0], color[1], color[2]));
            //mainCanvas.Children.Add(rec);
        }

        private void DrawLine(double[] start, double[] stop, byte[] color)
        {
            Line l = new Line();
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(color[0], color[1], color[2]));
            l.StrokeThickness = 1;
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

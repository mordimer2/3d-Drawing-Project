using System;
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

namespace GrafikaProj2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Figure figure;

        public static double[] LightSource1 = new double[3];
        public static double[] LightSource2 = new double[3];
        double xDeg = 10;
        double yDeg = 170;
        double zDeg = 180;
        double size = 70;
        ZBuffer zBuffer;


        double[] refToActualLight;
        public MainWindow()
        {
            int startX = 350, startY = 220, startZ = 0;
            InitializeComponent();
            LightSource1[0] = -1 * size + startX;
            LightSource1[1] = 0.5 * size + startY;
            LightSource1[2] = 3 * size + startZ;
            LightSource2[0] = 0.5 * size + startX;
            LightSource2[1] = 1 * size + startY;
            LightSource2[2] = -10 * size + startZ;

            refToActualLight = LightSource1;
            figure = new Figure(startX, startY, startZ);


            zBuffer = new ZBuffer((int)Width, (int)Height, new byte[] { 0, 0, 0 });

            DrawNew(null, null);
        }

        public void DrawNew(object sender, EventArgs args)
        {
            figure.Transform(xDeg, yDeg, zDeg, size);
            zBuffer.CalculateDepth(figure.triangles, refToActualLight);
            DrawItFinally(zBuffer.colorRGB);
        }

        public void DrawItFinally(byte[] byteArray)
        {
            BitmapSource bitmapSource = BitmapSource.Create((int)Width, (int)Height,
                                                            1, 1,
                                                            PixelFormats.Indexed8, BitmapPalettes.Gray256,
                                                            byteArray, (int)(Width * 1 + (Width % 2)));

            imageHolder.Source = bitmapSource;
        }

        private void MainGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.O)
                refToActualLight = LightSource1;
            else if (e.Key == Key.P)
                refToActualLight = LightSource2;
            else if (e.Key == Key.E)
                size += 5;
            else if (e.Key == Key.Q)
            {
                if (size > 50)
                    size -= 5;
            }
            else if (e.Key == Key.A)
            {
                if (yDeg >= 360) yDeg -= 360;
                yDeg += 1;
            }
            else if (e.Key == Key.D)
            {
                if (yDeg <= 0) yDeg += 360;
                yDeg -= 1;
            }
            else if (e.Key == Key.W)
            {
                if (xDeg >= 360) xDeg -= 360;
                xDeg += 1;
            }
            else if (e.Key == Key.S)
            {
                if (xDeg <= 0) xDeg += 360;
                xDeg -= 1;
            }
            else if (e.Key == Key.Z)
            {
                if (zDeg >= 360) zDeg -= 360;
                zDeg += 1;
            }
            else if (e.Key == Key.C)
            {
                if (zDeg <= 0) zDeg += 360;
                zDeg -= 1;
            }
            figure.Transform(xDeg, yDeg, zDeg, size);
            zBuffer.CalculateDepth(figure.triangles, refToActualLight);
            DrawItFinally(zBuffer.colorRGB);
        }
    }
}



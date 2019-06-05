using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Threading.Tasks;

namespace GrafikaProj2
{
    class ZBuffer
    {
        public double[,] Surface { get; private set; }

        //public Color[,] ColorsTab { get; private set; }

        //private Color baseColor { get; set; }

        public byte[,][] colorRGB { get; private set; }
        private byte[] baseColor { get; set; }


        private int width { get; set; }
        private int height { get; set; }
        public ZBuffer(int width, int height, byte[] baseRGB)
        {
            this.width = width;
            this.height = height;
            Surface = new double[width, height];
            //ColorsTab = new Color[width, height];
            colorRGB = new byte[width, height][];

            baseColor = baseRGB;
            ResetBoard();

        }
        public void ResetBoard()
        {
            Surface = new double[width, height];
            colorRGB = new byte[width, height][];
            //for (int i = 0; i < Surface.GetLength(0); i++)
            //    for (int j = 0; j < Surface.GetLength(1); j++)
            //    {
            //        Surface[i, j] = int.MaxValue;
            //        //ColorsTab[i, j] = baseColor;
            //        colorRGB[i, j] = new byte[] { baseColor[0], baseColor[1], baseColor[2] };
            //    }
        }

        public void fillBottomFlatTriangle(double[] v1, double[] v2, double[] v3, Triangle t, byte[] color)
        {
            double invslope1 = (v2[0] - v1[0]) / (v2[1] - v1[1]);
            double invslope2 = (v3[0] - v1[0]) / (v3[1] - v1[1]);

            double curx1 = v1[0];
            double curx2 = v1[0];

            for (int scanlineY = (int)v1[1]; scanlineY <= v2[1]; scanlineY++)
            {
                for (double xstart = curx1; xstart < curx2; xstart++)
                {
                    double tmp = Math.Abs(t.ZValue(xstart, scanlineY));
                    try
                    {
                        if (tmp > this.Surface[(int)xstart, scanlineY])
                        {
                            this.Surface[(int)xstart, scanlineY] = tmp;
                            this.colorRGB[(int)xstart, scanlineY] = color;
                        }
                    }
                    catch (Exception) { }


                }
                curx1 += invslope1;
                curx2 += invslope2;
            }
        }

        private void fillTopFlatTriangle(double[] v1, double[] v2, double[] v3, Triangle t, byte[] color)
        {
            double invslope1 = (v3[0] - v1[0]) / (v3[1] - v1[1]);
            double invslope2 = (v3[0] - v2[0]) / (v3[1] - v2[1]);

            double curx1 = v3[0];
            double curx2 = v3[0];

            for (int scanlineY = (int)v3[1]; scanlineY > v1[1]; scanlineY--)
            {
                for (double xstart = curx1; xstart < curx2; xstart++)
                {
                    double tmp = Math.Abs(t.ZValue(xstart, scanlineY));
                    try
                    {
                        if (tmp > this.Surface[(int)xstart, scanlineY])
                        {
                            this.Surface[(int)xstart, scanlineY] = tmp;
                            this.colorRGB[(int)xstart, scanlineY] = color;
                        }
                    }

                    catch (Exception) { }

                }
                curx1 -= invslope1;
                curx2 -= invslope2;
            }
        }

        public void CalculateDepth(List<Triangle> triangles)
        {
            ResetBoard();
            Random rnd = new Random();

            foreach (var triangle in triangles)
            {

                triangle.SortPointsByYAxis();
                triangle.CalculateCoefficients();

                byte[] randomColor = new byte[] { (byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256) };

                double[] v1 = Figure.actualListOfPoints[triangle.Point1];
                double[] v2 = Figure.actualListOfPoints[triangle.Point2];
                double[] v3 = Figure.actualListOfPoints[triangle.Point3];

                if (v2[1] == v3[1])
                    fillBottomFlatTriangle(v1, v2, v3, triangle, randomColor);
                else if (v1[1] == v2[1])
                    fillTopFlatTriangle(v1, v2, v3, triangle, randomColor);
                else
                {
                    double[] tmpVert = new double[] {v1[0]+ (( (v2[1] - v1[1]) / (v3[1] - v1[1])) * (v3[0] - v1[0])), v2[1] };
                    fillBottomFlatTriangle(v1, v2, tmpVert, triangle, randomColor);
                    fillTopFlatTriangle(v2, tmpVert, v3, triangle, randomColor);
                }

            }
        }


    }
}

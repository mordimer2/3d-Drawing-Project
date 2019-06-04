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
            for (int i = 0; i < Surface.GetLength(0); i++)
                for (int j = 0; j < Surface.GetLength(1); j++)
                {
                    Surface[i, j] = int.MaxValue;
                    //ColorsTab[i, j] = baseColor;
                    colorRGB[i, j] = new byte[] { baseColor[0], baseColor[1], baseColor[2] };
                }
        }

        public void CalculateDepth23(List<Triangle> triangles)
        {
            ResetBoard();

            Random rnd = new Random();
            foreach (var triangle in triangles)
            {
                byte[] randomColor = new byte[] { (byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256) };


                for (int i = (triangle.MinX >= 0 ? (int)triangle.MinX : 0); i < (triangle.MaxX < Surface.GetLength(0) ? triangle.MaxX : Surface.GetLength(0)); i++)
                {
                    for (int j = (triangle.MinY >= 0 ? (int)triangle.MinY : 0); j < (triangle.MaxY < Surface.GetLength(1) ? triangle.MaxY : Surface.GetLength(1)); j++)
                    {
                        double tmp = triangle.ZValue(i, j);
                        if (tmp < this.Surface[i, j])
                        {
                            this.Surface[i, j] = tmp;
                            //this.ColorsTab[i, j] = randomColor;
                            this.colorRGB[i, j] = randomColor;
                        }
                    }
                }

            }
        }

        private void fillBottomFlatTriangle(double[] v1, double[] v2, double[] v3, Triangle t, byte[] color)
        {
            double invslope1 = (v2[0] - v1[0]) / (v2[1] - v1[1]);
            double invslope2 = (v3[0] - v1[0]) / (v3[1] - v1[1]);

            double curx1 = v1[0];
            double curx2 = v1[0];

            for (int scanlineY = (int)v1[1]; scanlineY <= v2[1]; scanlineY++)
            {
                for (double xstart = curx1; xstart < curx2; xstart++)
                {
                    double tmp = t.ZValue(xstart, scanlineY);
                    try
                    {
                        if (tmp < this.Surface[(int)xstart, scanlineY])
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
                    double tmp = t.ZValue(xstart, scanlineY);
                    try
                    {
                        if (tmp < this.Surface[(int)xstart, scanlineY])
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
                    //Vertice v4 = new Vertice(
                    //    (int)(vt1.x + ((float)(vt2.y - vt1.y) / (float)(vt3.y - vt1.y)) * (vt3.x - vt1.x)), vt2.y);
                    fillBottomFlatTriangle(v1, v2, tmpVert, triangle, randomColor);
                    fillTopFlatTriangle(v2, tmpVert, v3, triangle, randomColor);
                }

            }
        }


    }
}

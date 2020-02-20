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
        public byte[] colorRGB { get; private set; }
        private byte[] baseColor { get; set; }


        private int width { get; set; }
        private int height { get; set; }
        public ZBuffer(int width, int height, byte[] baseRGB)
        {
            this.width = width;
            this.height = height;
            Surface = new double[width, height];
            colorRGB = new byte[width * height];

            baseColor = baseRGB;
            ResetBoard();
        }
        public void ResetBoard()
        {
            colorRGB = new byte[width * height];
            for (int i = 0; i < Surface.GetLength(0); i++)
                for (int j = 0; j < Surface.GetLength(1); j++)
                    Surface[i, j] = 99999;

        }

        private void replace(ref double a, ref double b)
        {
            double tmp = a; a = b; b = tmp;
        }
        public void fillBottomFlatTriangle(double[] v1, double[] v2, double[] v3, Triangle t, byte color)
        {
            double invslope1 = (v2[0] - v1[0]) / (v2[1] - v1[1]);
            double invslope2 = (v3[0] - v1[0]) / (v3[1] - v1[1]);

            double curx1 = v1[0];
            double curx2 = v1[0];

            if (invslope1 > invslope2) replace(ref invslope1, ref invslope2);

            for (int scanlineY = (int)Math.Round(v1[1]); scanlineY <= v2[1]; scanlineY++)
            {
                for (double xstart = curx1; xstart < curx2; xstart++)
                {
                    if (scanlineY < 0) continue;
                    double tmp = t.ZValue(xstart, scanlineY);
                    try
                    {
                        if (tmp < this.Surface[(int)Math.Round(xstart), scanlineY])
                        {
                            this.Surface[(int)Math.Round(xstart), scanlineY] = tmp;
                            this.colorRGB[(int)Math.Round((xstart + scanlineY * width))] = color;
                        }
                    }
                    catch (Exception) { }


                }
                curx1 += invslope1;
                curx2 += invslope2;
            }
        }

        private void fillTopFlatTriangle(double[] v1, double[] v2, double[] v3, Triangle t, byte color)
        {
            double invslope1 = (v3[0] - v1[0]) / (v3[1] - v1[1]);
            double invslope2 = (v3[0] - v2[0]) / (v3[1] - v2[1]);
            if (invslope1 < invslope2) replace(ref invslope1, ref invslope2);

            double curx1 = v3[0];
            double curx2 = v3[0];

            for (int scanlineY = (int)Math.Round(v3[1]); scanlineY > v1[1]; scanlineY--)
            {
                for (double xstart = curx1; xstart < curx2; xstart++)
                {
                    if (scanlineY < 0) continue;
                    double tmp = t.ZValue(xstart, scanlineY);
                    try
                    {
                        if (tmp < this.Surface[(int)Math.Round(xstart), scanlineY])
                        {
                            this.Surface[(int)Math.Round(xstart), scanlineY] = tmp;
                            this.colorRGB[(int)Math.Round((xstart + scanlineY * width))] = color;
                        }
                    }

                    catch (Exception) { }

                }
                curx1 -= invslope1;
                curx2 -= invslope2;
            }
        }

        /// <summary>
        /// PL: Funkcja kalkulująca jak daleko od źródła światła znajdują się poszczególne trójkąty. Dzieli potem trójkąt na górną i dolną połowę.
        /// Każdą z nich następnie wypełnia
        /// EN: Function that calculates how far is from light source from triangles. Then it splits triangles into 2 groups: a bottom baseline and a top  baseline.
        /// for each baseline location there is separate algorithm, that fills triangles with choosen color
        ///                     ^       example of bottom flat baseline
        ///                    / \
        ///                   /   \
        ///                  /     \
        ///                 /_______\
        ///                 
        ///                 ----------  example of top flat baseline triangle
        ///                 \        |
        ///                  \       |
        ///                   \      |
        ///                    \     |
        ///                     \    |
        ///                      \   |
        ///                       \  |
        ///                        \ |
        ///                         \|
        ///                         
        /// ZBuffer source and algorithm references:
        /// 1. https://www.geeksforgeeks.org/z-buffer-depth-buffer-method/
        /// 2. https://en.wikipedia.org/wiki/Z-buffering
        /// 3. https://www.ques10.com/p/22067/write-and-explain-the-depth-bufferz-buffer-algorit/
        /// </summary>
        /// <param name="triangles">trójkąty w 3d / list of triangles that make the shape</param>
        /// <param name="lightSource">punkt 3d ze źródłem światła / 3d light source points </param>
        public void CalculateDepth(List<Triangle> triangles, double[] lightSource)
        {
            ResetBoard();
            byte color = 0;
            foreach (var triangle in triangles)
            {
                triangle.SortPointsByYAxis();
                color = Shading.GetColor(triangle, lightSource);

                double[] v1 = Figure.currentListOfPoints[triangle.Point1];
                double[] v2 = Figure.currentListOfPoints[triangle.Point2];
                double[] v3 = Figure.currentListOfPoints[triangle.Point3];

                if (v1[1] > v2[1] || v2[1] > v3[1]) throw new Exception("Y nie są w kolejności rosnącej");

                if (v2[1] == v3[1])
                    fillBottomFlatTriangle(v1, v2, v3, triangle, color);
                else if (v1[1] == v2[1])
                    fillTopFlatTriangle(v1, v2, v3, triangle, color);
                else
                {
                    double[] tmpVert = new double[] { v1[0] + (((v2[1] - v1[1]) / (v3[1] - v1[1])) * (v3[0] - v1[0])), v2[1] };
                    fillBottomFlatTriangle(v1, v2, tmpVert, triangle, color);
                    fillTopFlatTriangle(v2, tmpVert, v3, triangle, color);
                }

            }
        }


    }
}

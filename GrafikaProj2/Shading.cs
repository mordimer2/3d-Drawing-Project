using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaProj2
{
    static class Shading
    {
        /// <summary>
        /// Fill plane with appriopriate color. Whole plane is shaded with one color
        /// </summary>
        /// <param name="plane">plane (trinagle) </param>
        /// <param name="l1">source of light </param>
        /// <returns></returns>
        public static byte GetColor(Triangle plane, double[] l1)
        {
            double[] normal = new double[] { plane.A, plane.B, plane.C };
            l1 = MatrixOperations.Normalize(l1);
            normal = MatrixOperations.Normalize(normal);

            double scalar = MatrixOperations.VectorScalar(l1, normal);
            double color = Math.Max(20, 255 * Math.Max(0, scalar));
            if (color == 20)
            {
                plane.FlipNormal();
                normal = new double[] { plane.A, plane.B, plane.C };
                normal = MatrixOperations.Normalize(normal);
                scalar = MatrixOperations.VectorScalar(l1, normal);
                color = Math.Max(20, 255 * Math.Max(0, scalar));
                plane.FlipNormal();

            }
            return (byte)color;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaProj2
{
    static class Shading
    {
        
        public static byte GetColor(Triangle plane)
        {
            double[] l1 = MainWindow.LightSource1;
            double[] l2 = MainWindow.LightSource2;

            //double[] center = plane.CenterPoint;
            //double[] normalAvg = plane.AverageNormalVector;

            //double[] lightDirection = new double[3];

            //for (int i = 0; i < lightDirection.Length; i++)
            //    lightDirection[i] = l1[i] - center[i];

            double[] normal = new double[] { plane.A, plane.B, plane.C };
            l1 = MatrixOperations.Normalize(l1);
            l2 = MatrixOperations.Normalize(l2);
            normal  = MatrixOperations.Normalize(normal);

            double scalar = MatrixOperations.VectorScalar(l1, normal);
            double scalar2 = MatrixOperations.VectorScalar(l2, normal);
            double color = Math.Max(20, 255 * Math.Max(0, scalar+scalar2));
            return (byte) color;
        }
    }
}

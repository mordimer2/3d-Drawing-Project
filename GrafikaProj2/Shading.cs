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

            double[] center = plane.CenterPoint;
            double[] normalAvg = plane.AverageNormalVector;

            double[] lightDirection = new double[3];

            for (int i = 0; i < lightDirection.Length; i++)
                lightDirection[i] =  center[i]- l1[i];

            //double[] normal = new double[] { plane.A, plane.B, plane.C };
            lightDirection = MatrixOperations.Normalize(lightDirection);
            normalAvg = MatrixOperations.Normalize(normalAvg);

            double scalar = MatrixOperations.VectorScalar(lightDirection, normalAvg);
            double color = Math.Max(20, 255 * Math.Max(0, scalar));
            return (byte) color;
        }
    }
}

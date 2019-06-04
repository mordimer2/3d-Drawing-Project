using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaProj2
{
    public static class MatrixOperations
    {
        public static double[] MultipleMatrixAndVector(double[,] matrix, double[] vector)
        {
            double[] vec = new double[matrix.GetLength(0)];
            if (matrix.GetLength(1) != vector.Length) throw new FormatException("nie można przemnożyć podanych macierzy");
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    vec[i] += (matrix[i, j] * vector[j]);
            return vec;
        }

        public static double[,] MakeHomogeneousConsistency(double [,] matrix)
        {
            double last = matrix[3, 3];
            if (last == 1) return matrix;
            double division = 1 / last;
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] *= division;
            return matrix;
        }

        public static double[] MakeVectorConsistent(double[] vector)
        { 
            if (vector[3] == 1) return vector;
            double div = 1 / vector[3];
            for (int i = 0; i < vector.Length-1; i++)
                vector[i] *= div;
            return new double[] { vector[0], vector[1], vector[2] };
        }

        private static double DegreesToRadians (double degree)
        {
            if (degree >= 360) degree -= 360;
            return (degree * Math.PI) / 180;
        }

        private static double[] CutLast(double[] vector)
        {
            if (vector.Length == 3) return vector;
            return new double[] { vector[0], vector[1], vector[2] };
        }

        public static double[] RotateMatrix(double[] vector, double xDegree, double yDegree, double zDegree)
        {
            xDegree = DegreesToRadians(xDegree);
            yDegree = DegreesToRadians(yDegree);
            zDegree = DegreesToRadians(zDegree);
            double cosX = Math.Cos(xDegree); double cosY = Math.Cos(yDegree); double cosZ = Math.Cos(zDegree);
            double sinX = Math.Sin(xDegree); double sinY = Math.Sin(yDegree); double sinZ = Math.Sin(zDegree);
            double[,] rotationMatrix =
                {{ cosY*cosZ,                    -1*cosY*sinZ,                   sinY ,          0},
                {sinX*sinY*cosZ + cosX*sinZ,    -1*sinX*sinY*sinZ+cosX*cosZ,    -1*sinX*cosY,   0 },
                {-1*cosX*sinY*cosZ+sinX*sinZ,   cosX*sinY*sinZ+sinX*cosZ,       cosX*cosY,      0},
                {0,0,0,1 }};
            double[] vectorPrim = new double[] { vector[0], vector[1], vector[2], 1 };
            return CutLast(MakeVectorConsistent( MultipleMatrixAndVector( rotationMatrix, vectorPrim)));
        }

        public static double[,] MultipleSameSizeMatrices(double[,] matrice1, double[,] matrice2)
        {
            double[,] output = new double[4, 4];
            for (int i = 0; i < matrice1.GetLength(0); i++)
                for (int j = 0; j < matrice1.GetLength(1); j++)
                    for (int k = 0; k < matrice1.Length; k++)
                        output[i, j] += matrice1[i, k] + matrice2[k, j];
            return output;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaProj2
{
    class Figure
    {
        public List<double[]> baseListOfPoints { get; private set; }

        public static List<double[]> currentListOfPoints { get; private set; }

        public List<Triangle> triangles { get; private set; }


        public List<int[]> listOfEdges { get; private set; }

        private int[] canvStart;

        public Figure(int x, int y, int z)
        {
            canvStart = new int[] { x, y, z };
            listOfEdges = new List<int[]>();
            baseListOfPoints = new List<double[]>();
            triangles = new List<Triangle>();

            baseListOfPoints.Add(new double[] { 0, 1, 0 }); // 0
            baseListOfPoints.Add(new double[] { 1, 1, 0 }); // 1
            baseListOfPoints.Add(new double[] { 0, 2, 0 }); // 2
            baseListOfPoints.Add(new double[] { 1, 2, 0 }); // 3
            baseListOfPoints.Add(new double[] { 0, 1, 1 }); // 4
            baseListOfPoints.Add(new double[] { 1, 1, 1 }); // 5
            baseListOfPoints.Add(new double[] { 0, 2, 1 }); // 6
            baseListOfPoints.Add(new double[] { 1, 2, 1 }); // 7


            triangles.Add(new Triangle(0, 2, 4));
            triangles.Add(new Triangle(0, 1, 4));
            triangles.Add(new Triangle(0, 2, 3));
            triangles.Add(new Triangle(0, 1, 3));
            triangles.Add(new Triangle(1, 4, 5));
            triangles.Add(new Triangle(1, 3, 7));
            triangles.Add(new Triangle(1, 5, 7));
            triangles.Add(new Triangle(2, 3, 7));
            triangles.Add(new Triangle(2, 6, 7));
            triangles.Add(new Triangle(2, 4, 6));
            triangles.Add(new Triangle(4, 5, 6));
            triangles.Add(new Triangle(5, 6, 7));

            baseListOfPoints.Add(new double[] { 0, 0, 0 }); // 0
            baseListOfPoints.Add(new double[] { 1, 0, 0 }); // 1
            baseListOfPoints.Add(new double[] { 0, 1, 0 }); // 2
            baseListOfPoints.Add(new double[] { 1, 1, 0 }); // 3
            baseListOfPoints.Add(new double[] { 0, 0, 2 }); // 4
            baseListOfPoints.Add(new double[] { 1, 0, 2 }); // 5
            baseListOfPoints.Add(new double[] { 0, 1, 2 }); // 6
            baseListOfPoints.Add(new double[] { 1, 1, 2 }); // 7

            triangles.Add(new Triangle(8, 10, 12));
            triangles.Add(new Triangle(8, 9, 12));
            triangles.Add(new Triangle(8, 10, 11));
            triangles.Add(new Triangle(8, 9, 11));
            triangles.Add(new Triangle(9, 12, 13));
            triangles.Add(new Triangle(9, 11, 15));
            triangles.Add(new Triangle(9, 13, 15));
            triangles.Add(new Triangle(10, 11, 15));
            triangles.Add(new Triangle(10, 14, 15));
            triangles.Add(new Triangle(10, 12, 14));
            triangles.Add(new Triangle(12, 13, 14));
            triangles.Add(new Triangle(13, 14, 15));

            baseListOfPoints.Add(new double[] { 0.3, 2, 0 }); // 0
            baseListOfPoints.Add(new double[] { 0.7, 2, 0 }); // 1
            baseListOfPoints.Add(new double[] { 0.3, 2.25, 0 }); // 2
            baseListOfPoints.Add(new double[] { 0.7, 2.25, 0 }); // 3
            baseListOfPoints.Add(new double[] { 0.3, 2, 2 }); // 4
            baseListOfPoints.Add(new double[] { 0.7, 2, 2 }); // 5
            baseListOfPoints.Add(new double[] { 0.3, 2.25, 2 }); // 6
            baseListOfPoints.Add(new double[] { 0.7, 2.25, 2 }); // 7

            Figure.currentListOfPoints = new List<double[]>(baseListOfPoints);

            triangles.Add(new Triangle(16, 18, 20));
            triangles.Add(new Triangle(16, 17, 20));
            triangles.Add(new Triangle(16, 18, 19));
            triangles.Add(new Triangle(16, 17, 19));
            triangles.Add(new Triangle(17, 20, 21));
            triangles.Add(new Triangle(17, 19, 23));
            triangles.Add(new Triangle(17, 21, 23));
            triangles.Add(new Triangle(18, 19, 23));
            triangles.Add(new Triangle(18, 22, 23));
            triangles.Add(new Triangle(18, 20, 22));
            triangles.Add(new Triangle(20, 21, 22));
            triangles.Add(new Triangle(21, 22, 23));
        }

        public void Transform(double xDeg, double yDeg, double zDeg, double size)
        {
            Rotate(xDeg, yDeg, zDeg);
            Scale(size);
            Transact();
        }

        public void Rotate(double xDeg, double yDeg, double zDeg)
        {
            for (int i = 0; i < baseListOfPoints.Count; i++)
                Figure.currentListOfPoints[i] = MatrixOperations.RotateMatrix(baseListOfPoints[i], xDeg, yDeg, zDeg);
        }

        public void Scale(double scale = 100)
        {
            for (int i = 0; i < Figure.currentListOfPoints.Count; i++)
                for (int j = 0; j < Figure.currentListOfPoints[i].Length; j++)
                    Figure.currentListOfPoints[i][j] = currentListOfPoints[i][j] * (scale <= 0 ? 1 : scale);
        }

        public void Transact()
        {
            foreach (var vertice in Figure.currentListOfPoints)
            {
                vertice[0] += canvStart[0];
                vertice[1] += canvStart[1];
                vertice[2] += canvStart[2];
            }
        }

        private static double[,] projectionMatrix = new double[,] {  {1,0,0,0},
                                                                {0,1,0,0},
                                                                {0,0,0,0},
                                                                {0,0,0,1}};

        public static double[] ProjectionPoints(double[] input) // inpu len = 3
        {
            double[] consis = new double[input.Length + 1];
            for (int i = 0; i < input.Length; i++) consis[i] = input[i];
            consis[3] = 1;
            double[] output = MatrixOperations.MultipleMatrixAndVector(projectionMatrix, consis);
            output = MatrixOperations.MakeVectorConsistent(output);
            return new double[] { output[0], output[1] };
        }



    }
}

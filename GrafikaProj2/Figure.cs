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

        public static List<double[]> actualListOfPoints { get; private set; }

        public List<Triangle> triangles { get; private set; }

        /// <summary> DEPRECATEF
        /// Przechowuje indeksy listOfPoints wskazując na wierzchołki, które powinny 
        /// zostać połączone
        /// </summary>
        public List<int[]> listOfEdges { get; private set; }

        private int[] canvStart;

        public Figure(int x, int y, int z)
        {
            canvStart = new int[] {x, y , z};
            listOfEdges = new List<int[]>();
            baseListOfPoints = new List<double[]>();
            triangles = new List<Triangle>();

            baseListOfPoints.Add(new double[] { 0, 0, 0 }); // 0
            baseListOfPoints.Add(new double[] { 1, 0, 0 }); // 1
            baseListOfPoints.Add(new double[] { 0, 1, 0 }); // 2
            baseListOfPoints.Add(new double[] { 1, 1, 0 }); // 3
            baseListOfPoints.Add(new double[] { 0, 0, 1 }); // 4
            baseListOfPoints.Add(new double[] { 1, 0, 1 }); // 5
            baseListOfPoints.Add(new double[] { 0, 1, 1 }); // 6
            baseListOfPoints.Add(new double[] { 1, 1, 1 }); // 7

            Figure.actualListOfPoints = new List<double[]>(baseListOfPoints);

            triangles.Add(new Triangle(0,2,4));
            triangles.Add(new Triangle(0,1,4));
            triangles.Add(new Triangle(0,2,3));
            triangles.Add(new Triangle(0,1,3));
            triangles.Add(new Triangle(1,4,5));
            triangles.Add(new Triangle(1,3,7));
            triangles.Add(new Triangle(1,5,7));
            triangles.Add(new Triangle(2,3,7));
            triangles.Add(new Triangle(2,6,7));
            triangles.Add(new Triangle(2,4,6));
            triangles.Add(new Triangle(4,5,6));
            triangles.Add(new Triangle(5,6,7));


            //listOfEdges.Add(new int[] { 0,2 });
            //listOfEdges.Add(new int[] { 0,1 });
            //listOfEdges.Add(new int[] { 0,4 });
            //listOfEdges.Add(new int[] { 2,3 });
            //listOfEdges.Add(new int[] { 1,3 });
            //listOfEdges.Add(new int[] { 3,7 });
            //listOfEdges.Add(new int[] { 4,5 });
            //listOfEdges.Add(new int[] { 1,5 });
            //listOfEdges.Add(new int[] { 5,7 });
            //listOfEdges.Add(new int[] { 4,6 });
            //listOfEdges.Add(new int[] { 2,6 });
            //listOfEdges.Add(new int[] { 6,7 });


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
                Figure.actualListOfPoints[i] = MatrixOperations.RotateMatrix(baseListOfPoints[i], xDeg, yDeg, zDeg);
        }

        public void Scale(double scale=100)
        {
            for (int i = 0; i < Figure.actualListOfPoints.Count; i++)
                for (int j = 0; j < Figure.actualListOfPoints[i].Length; j++)
                    Figure.actualListOfPoints[i][j] = actualListOfPoints[i][j] * (scale<=0?1:scale) ;
        }

        public void Transact()
        {
            foreach (var vertice in Figure.actualListOfPoints)
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


        //private void TrianglesPoints()
        //{
        //    List<int> binarizedSum = new List<int>();
        //    for (int i = 0; i < baseListOfPoints.Count; i++)
        //    {
        //        binarizedSum.Add(0);
        //        for (int j = 0; j < baseListOfPoints[i].Length; j++)
        //            if (baseListOfPoints[i][j] != 0) binarizedSum[i] += 1;
        //    }

        //    for (int i = 0; i < binarizedSum.Count; i++)
        //        for (int j = i+1; j < binarizedSum.Count; j++)
        //            if (Math.Abs(binarizedSum[i] - binarizedSum[j]) == 1)
        //                for (int k = j+1; k < binarizedSum.Count; k++) 
        //                    if (Math.Abs(binarizedSum[i] - binarizedSum[k]) == 1 && j != k)
        //                        this.triangles.Add(new Triangle(i, j, k));
        //}

        //public int[] Transaction(double[] point)
        //{
        //    int[] outp = new int[2];
        //    for (int i = 0; i < point.Length; i++) outp[i] = this.canvStart[i] + (int)point[i];
        //    return outp;
        //}
    }
}

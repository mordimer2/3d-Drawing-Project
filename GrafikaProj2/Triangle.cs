using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaProj2
{
    class Triangle
    {

        public double A { get; private set; }
        public double B { get; private set; }
        public double C { get; private set; }
        public double D { get; private set; }

        //public double[] Point1 { get; private set; }
        //public double[] Point2 { get; private set; }
        //public double[] Point3 { get; private set; }

        public int Point1 { get; private set; }
        public int Point2 { get; private set; }
        public int Point3 { get; private set; }


        public double MaxX { get { return findMax(Figure.actualListOfPoints[Point1][0], Figure.actualListOfPoints[Point2][0], Figure.actualListOfPoints[Point3][0]);} }
        public double MaxY { get { return findMax(Figure.actualListOfPoints[Point1][1], Figure.actualListOfPoints[Point2][1], Figure.actualListOfPoints[Point3][1]); } }
        public double MaxZ { get { return findMax(Figure.actualListOfPoints[Point1][2], Figure.actualListOfPoints[Point2][2], Figure.actualListOfPoints[Point3][2]); } }


        public double MinX { get { return findMin(Figure.actualListOfPoints[Point1][0], Figure.actualListOfPoints[Point2][0], Figure.actualListOfPoints[Point3][0]); } }
        public double MinY { get { return findMin(Figure.actualListOfPoints[Point1][1], Figure.actualListOfPoints[Point2][1], Figure.actualListOfPoints[Point3][1]); } }
        public double MinZ { get { return findMin(Figure.actualListOfPoints[Point1][2], Figure.actualListOfPoints[Point2][2], Figure.actualListOfPoints[Point3][2]); } }



        public Triangle(int point1, int point2, int point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            CalculateCoefficients();

        }

        public void SortPointsByYAxis()
        {
            double p1Y = Figure.actualListOfPoints[Point1][1];
            double p2Y = Figure.actualListOfPoints[Point2][1];
            double p3Y = Figure.actualListOfPoints[Point3][1];

            if (p1Y > p2Y){
                int tmp = Point1; Point1 = Point2; Point2 = tmp;
                double tmp2 = p1Y;p1Y = p2Y;p2Y = tmp2;
            }
            if (p1Y > p3Y){
                int tmp = Point1; Point1 = Point3; Point3 = tmp;
                double tmp2 = p1Y;p1Y = p3Y;p3Y = tmp2;
            }
            if (p2Y> p3Y){int tmp = Point2; Point2 = Point3; Point3 = tmp;}
        }

        private double Scalar(double[] a, double[] b)
        {
            double output = 0;
            for (int i = 0; i < a.Length; i++)
                output += a[i] * b[i];
            return output;
        }
        private double VectorLen(double[] a)
        {
            double output = 0;
            for (int i = 0; i < a.Length; i++)
                output += Math.Pow(a[i], 2);
            return Math.Sqrt(output);
        }

        private double[] VectorSum(double[] vector1, double[] vector)
        {
            double[] output = new double[vector.Length];
            for (int i = 0; i < vector1.Length; i++)
                output[i] = vector[i] - vector1[i];
            return output;
        }
        private double MatrixDet(double[,] matrix)
        {
            // assuming matrice is 2x2
            return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
        }
        private void CalculateCoefficients()
        {
            SortPointsByYAxis();
            double[] point1 = Figure.actualListOfPoints[Point1];
            double[] point2 = Figure.actualListOfPoints[Point2];
            double[] point3 = Figure.actualListOfPoints[Point3];
            //A = (point2[2] - point3[2]) * (point1[1] - point2[1]) - (point1[2] - point2[2]) * (point2[1] - point3[1]);
            //B = (point2[0] - point3[0]) * (point1[2] - point2[2]) - (point1[0] - point2[0]) * (point2[2] - point3[2]);
            //C = (point2[1] - point3[1]) * (point1[0] - point2[0]) - (point1[1] - point2[1]) * (point2[0] - point3[0]);
            //D = -point1[0] * (point2[1] * point3[2] - point2[2] * point3[1]) + point1[1] * (point2[0] * point3[2] - point2[2] * point3[0]) - point1[2] * (point2[0] * point3[1] - point2[1] * Figure.actualListOfPoints[Point3][0]);
            double[] p1p2 = VectorSum(point1, point2);
            double[] p1p3 = VectorSum(point1, point3);

            A= MatrixDet(new double[,] { { p1p2[1], p1p2[2] },  {p1p3[1], p1p3[2] } });
            B= MatrixDet(new double[,] { { p1p2[0], p1p2[2] },  {p1p3[0], p1p3[2] } });
            C= MatrixDet(new double[,] { { p1p2[0], p1p2[1] },  {p1p3[0], p1p3[1] } });
            D = (-1) * (A * point1[0] + B * point1[1] + C * point1[2]);
            // cross product vec1*vec2 =||a||*||b||*sin(angle)

        }

        private double findMax(double a1, double a2, double a3) { return Math.Max(Math.Max(a1, a2), a3); }
        private double findMin(double a1, double a2, double a3) { return Math.Min(Math.Min(a1, a2), a3); }

        public double ZValue(double x, double y)
        {
            CalculateCoefficients();
            double z = int.MaxValue;
            if (C != 0)
                z = (-A * x - B * y - D) / C;
            return z;
        }
    }
}

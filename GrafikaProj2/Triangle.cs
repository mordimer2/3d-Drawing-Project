using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaProj2
{
    class Triangle
    {

        public double A
        {
            get
            {
                SortPointsByYAxis();
                double[] p1 = Figure.currentListOfPoints[Point1];
                double[] p2 = Figure.currentListOfPoints[Point2];
                double[] p3 = Figure.currentListOfPoints[Point3];
                if (flipNormal == false)
                    return (p2[1] - p1[1]) * (p3[2] - p1[2]) - (p2[2] - p1[2]) * (p3[1] - p1[1]);
                else return (-1) * ((p2[1] - p1[1]) * (p3[2] - p1[2]) - (p2[2] - p1[2]) * (p3[1] - p1[1]));
            }
        }
        public double B
        {
            get
            {
                SortPointsByYAxis();
                double[] p1 = Figure.currentListOfPoints[Point1];
                double[] p2 = Figure.currentListOfPoints[Point2];
                double[] p3 = Figure.currentListOfPoints[Point3];
                if (flipNormal == false)
                    return (-1) * ((p2[0] - p1[0]) * (p3[2] - p1[2]) - (p3[0] - p1[0]) * (p2[2] - p1[2]));
                else return (-1) * ((-1) * ((p2[0] - p1[0]) * (p3[2] - p1[2]) - (p3[0] - p1[0]) * (p2[2] - p1[2])));
            }
        }
        public double C
        {
            get
            {
                SortPointsByYAxis();
                double[] p1 = Figure.currentListOfPoints[Point1];
                double[] p2 = Figure.currentListOfPoints[Point2];
                double[] p3 = Figure.currentListOfPoints[Point3];
                if (flipNormal == false)
                    return (p2[0] - p1[0]) * (p3[1] - p1[1]) - (p3[0] - p1[0]) * (p2[1] - p1[1]);
                else return (-1) * ((p2[0] - p1[0]) * (p3[1] - p1[1]) - (p3[0] - p1[0]) * (p2[1] - p1[1]));

            }
        }
        public double D
        {
            get
            {
                SortPointsByYAxis();
                double[] p1 = Figure.currentListOfPoints[Point1];
                double[] p2 = Figure.currentListOfPoints[Point2];
                double[] p3 = Figure.currentListOfPoints[Point3];
                if (flipNormal == false)
                    return (-1) * (A * p1[0] + B * p1[1] + C * p1[2]);
                else return (A * p1[0] + B * p1[1] + C * p1[2]);
            }

        }

        public int Point1 { get; private set; }
        public int Point2 { get; private set; }
        public int Point3 { get; private set; }

        public double MaxX { get { return findMax(Figure.currentListOfPoints[Point1][0], Figure.currentListOfPoints[Point2][0], Figure.currentListOfPoints[Point3][0]); } }
        public double MaxY { get { return findMax(Figure.currentListOfPoints[Point1][1], Figure.currentListOfPoints[Point2][1], Figure.currentListOfPoints[Point3][1]); } }
        public double MaxZ { get { return findMax(Figure.currentListOfPoints[Point1][2], Figure.currentListOfPoints[Point2][2], Figure.currentListOfPoints[Point3][2]); } }

        public double MinX { get { return findMin(Figure.currentListOfPoints[Point1][0], Figure.currentListOfPoints[Point2][0], Figure.currentListOfPoints[Point3][0]); } }
        public double MinY { get { return findMin(Figure.currentListOfPoints[Point1][1], Figure.currentListOfPoints[Point2][1], Figure.currentListOfPoints[Point3][1]); } }
        public double MinZ { get { return findMin(Figure.currentListOfPoints[Point1][2], Figure.currentListOfPoints[Point2][2], Figure.currentListOfPoints[Point3][2]); } }

        public double[] AverageNormalVector
        {
            get
            {
                double[] n1 = MatrixOperations.Normalize(Figure.currentListOfPoints[Point1]);
                double[] n2 = MatrixOperations.Normalize(Figure.currentListOfPoints[Point2]);
                double[] n3 = MatrixOperations.Normalize(Figure.currentListOfPoints[Point3]);
                double[] avgNorm = new double[3];
                for (int i = 0; i < n1.Length; i++)
                    avgNorm[i] = (n1[i] + n2[i] + n3[i]) / 3;
                return avgNorm;
            }
        }

        bool flipNormal;
        public Triangle(int point1, int point2, int point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            flipNormal = false;
        }

        public void FlipNormal()
        {
            flipNormal = !flipNormal;
        }

        public void SortPointsByYAxis()
        {
            double p1Y = Figure.currentListOfPoints[Point1][1];
            double p2Y = Figure.currentListOfPoints[Point2][1];
            double p3Y = Figure.currentListOfPoints[Point3][1];

            if (p1Y > p2Y)
            {
                int tmp = Point1; Point1 = Point2; Point2 = tmp;
                double tmp2 = p1Y; p1Y = p2Y; p2Y = tmp2;
            }
            if (p1Y > p3Y)
            {
                int tmp = Point1; Point1 = Point3; Point3 = tmp;
                double tmp2 = p1Y; p1Y = p3Y; p3Y = tmp2;
            }
            if (p2Y > p3Y) { int tmp = Point2; Point2 = Point3; Point3 = tmp; }
        }
        private double findMax(double a1, double a2, double a3) { return Math.Max(Math.Max(a1, a2), a3); }
        private double findMin(double a1, double a2, double a3) { return Math.Min(Math.Min(a1, a2), a3); }

        public double ZValue(double x, double y)
        {
            double z = int.MaxValue;
            if (C != 0)
                z = ((-1) * (A * x + B * y + D)) / C;
            if (MaxZ == MinZ) return MaxZ;
            return z;
        }
    }
}

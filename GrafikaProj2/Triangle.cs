using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaProj2
{
    class Triangle
    {

        public double A {
            get {
                SortPointsByYAxis();
                double[] p1 = Figure.actualListOfPoints[Point1];
                double[] p2 = Figure.actualListOfPoints[Point2];
                double[] p3 = Figure.actualListOfPoints[Point3];
                if(flipNormal==false)
                    return (p2[1] - p1[1]) * (p3[2] - p1[2]) - (p2[2] - p1[2]) * (p3[1] - p1[1]);
                else return (-1)*((p2[1] - p1[1]) * (p3[2] - p1[2]) - (p2[2] - p1[2]) * (p3[1] - p1[1]));
            }
        }
        public double B { get
            {
                SortPointsByYAxis();
                double[] p1 = Figure.actualListOfPoints[Point1];
                double[] p2 = Figure.actualListOfPoints[Point2];
                double[] p3 = Figure.actualListOfPoints[Point3];
                if(flipNormal==false)
                    return (-1) * ((p2[0] - p1[0]) * (p3[2] - p1[2]) - (p3[0] - p1[0]) * (p2[2] - p1[2]));
                else return (-1)*((-1) * ((p2[0] - p1[0]) * (p3[2] - p1[2]) - (p3[0] - p1[0]) * (p2[2] - p1[2])));
            } }
        public double C { get
            {
                SortPointsByYAxis();
                double[] p1 = Figure.actualListOfPoints[Point1];
                double[] p2 = Figure.actualListOfPoints[Point2];
                double[] p3 = Figure.actualListOfPoints[Point3];
                if(flipNormal==false)
                return (p2[0] - p1[0]) * (p3[1] - p1[1]) - (p3[0] - p1[0]) * (p2[1] - p1[1]);
                else return (-1)*((p2[0] - p1[0]) * (p3[1] - p1[1]) - (p3[0] - p1[0]) * (p2[1] - p1[1]));

            }
        }
        public double D { get {
                SortPointsByYAxis();
                double[] p1 = Figure.actualListOfPoints[Point1];
                double[] p2 = Figure.actualListOfPoints[Point2];
                double[] p3 = Figure.actualListOfPoints[Point3];
                if(flipNormal==false)
                    return (-1) * (A * p1[0] + B * p1[1] + C * p1[2]);
                else return (A * p1[0] + B * p1[1] + C * p1[2]);
            }

        }

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

        public double[] AverageNormalVector
        {
            get {
                double[] n1 = MatrixOperations.Normalize(Figure.actualListOfPoints[Point1]);
                double[] n2 = MatrixOperations.Normalize(Figure.actualListOfPoints[Point2]);
                double[] n3 = MatrixOperations.Normalize(Figure.actualListOfPoints[Point3]);
                double[] avgNorm = new double[3];
                for (int i = 0; i < n1.Length; i++)
                    avgNorm[i] = (n1[i] + n2[i] + n3[i]) / 3;
                return avgNorm;
            }
        }

        public double[] CenterPoint
        {
            get
            {
                double[] n1 = Figure.actualListOfPoints[Point1];
                double[] n2 = Figure.actualListOfPoints[Point2];
                double[] n3 = Figure.actualListOfPoints[Point3];
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
            CalculateCoefficients();
            flipNormal = false;
        }

        public void FlipNormal()
        {
            flipNormal = !flipNormal;
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
        public void CalculateCoefficients()
        {
            
            //A = (p2[2] - p3[2]) * (p1[1] - p2[1]) - (p1[2] - p2[2]) * (p2[1] - p3[1]);
            //B = (p2[0] - p3[0]) * (p1[2] - p2[2]) - (p1[0] - p2[0]) * (p2[2] - p3[2]);
            //C = (p2[1] - p3[1]) * (p1[0] - p2[0]) - (p1[1] - p2[1]) * (p2[0] - p3[0]);
            //D = -p1[0] * (p2[1] * p3[2] - p2[2] * p3[1]) + p1[1] * (p2[0] * p3[2] - p2[2] * p3[0]) - p1[2] * (p2[0] * p3[1] - p2[1] * Figure.actualListOfPoints[Point3][0]);
            //double[] p1p2 = VectorSum(p1, p2);
            //double[] p1p3 = VectorSum(p1, p3);

            //A = MatrixDet(new double[,] { { p1p2[1], p1p2[2] }, { p1p3[1], p1p3[2] } });
            //B = MatrixDet(new double[,] { { p1p2[0], p1p2[2] }, { p1p3[0], p1p3[2] } });
            //C = MatrixDet(new double[,] { { p1p2[0], p1p2[1] }, { p1p3[0], p1p3[1] } });
            //D = (-1) * (A * p1[0] + B * p1[1] + C * p1[2]);
            // cross product vec1*vec2 =||a||*||b||*sin(angle)
            //A = (p2[1] - p1[1]) * (p3[2] - p1[2]) - (p3[1] - p1[1]) * (p2[2] - p1[2]);
            //B = (-1) * ((p2[2] - p1[2]) * (p3[0] - p1[0]) - (p3[2] - p1[2]) * (p2[0] - p1[0]));
            //C = (p2[0] - p1[0]) * (p3[1] - p1[1]) - (p3[0] - p1[0]) * (p2[1] - p1[1]);
            //D = (-1) * (A * p1[0] + B * p1[1] + C * p1[2]);
            
        }

        private double findMax(double a1, double a2, double a3) { return Math.Max(Math.Max(a1, a2), a3); }
        private double findMin(double a1, double a2, double a3) { return Math.Min(Math.Min(a1, a2), a3); }

        public double ZValue(double x, double y)
        {
            //SortPointsByYAxis();
            //double[] point1 = Figure.actualListOfPoints[Point1];
            //double[] point2 = Figure.actualListOfPoints[Point2];
            //double[] point3 = Figure.actualListOfPoints[Point3];
            //CalculateCoefficients();

            double z = int.MaxValue;
            if (C != 0)
                z = ((-1)*(A * x + B * y + D)) / C;
            if (MaxZ == MinZ) return MaxZ;
            //double asd=((2*((z-MinZ)/(MaxZ-MinZ))-1)+1)/2;
            //if (z < MinZ || z > MaxZ) throw new Exception("Coś idzie nie tak, i to bardzooo");
            return z;
            //return 2 * ((z-MinZ)/(MaxZ-MinZ))-1;
        }
    }
}

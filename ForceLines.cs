﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    public class ForceLines
    {
        private List<TrianglePotential> triangles;
        private List<LevelLines> forceLines;

        List<Potential> magnet1;
        List<Potential> magnet2;

        public ForceLines(List<Triangle> p_triangles, List<Potential> p_magnet1, List<Potential> p_magnet2)
        {
            triangles = new List<TrianglePotential>();
            forceLines = new List<LevelLines>();

            magnet1 = new List<Potential>();
            magnet1.AddRange(p_magnet1);

            magnet2 = new List<Potential>();
            magnet2.AddRange(p_magnet2);

        }
        #region Производные для градиента
        float DpDy(Potential pot1, Potential pot2, Potential pot3)
        {
            float B = -((pot2.point.X - pot1.point.X) * (pot3.value - pot1.value) -
                (pot3.point.X - pot1.point.X) * (pot2.value - pot1.value));
            return B;
        }

        float DpDx(Potential pot1, Potential pot2, Potential pot3)
        {
            float A = (pot2.point.Y - pot1.point.Y) * (pot3.value - pot1.value) -
                (pot3.point.Y - pot1.point.Y) * (pot2.value - pot1.value);
            return A;
        }

        float DpDz(Potential pot1, Potential pot2, Potential pot3)
        {
            float C = (pot2.point.X - pot1.point.X) * (pot3.point.Y - pot1.point.Y) -
                (pot3.point.X - pot1.point.X) * (pot2.point.Y - pot1.point.Y);
            return C;
        }
        #endregion

        public List<LevelLines> GetForceLines()
        {
            int size_triangles = triangles.Count;
            int size_magnet1 = magnet1.Count;

            for (int i = 0; i < size_magnet1; ++i)
            {
                Potential pot = magnet1[i];
                int num_triangle = 0;

                /*Поиск любого треугольника на границе магнита содержащего точку pot*/
                for (int j = 0; j < size_triangles; j++)
                {
                    if (pot.point == triangles[j].point1.point ||
                        pot.point == triangles[j].point2.point ||
                        pot.point == triangles[j].point3.point)
                        num_triangle = j;
                }

                float A = -DpDx(triangles[num_triangle].point1,
                    triangles[num_triangle].point2,
                    triangles[num_triangle].point3);
                float B = -DpDy(triangles[num_triangle].point1,
                    triangles[num_triangle].point2,
                    triangles[num_triangle].point3);

                PointF pointIntersect = new PointF();

                /*Проверияю есть ли пересечение*/
                if(pot.point==triangles[num_triangle].point1.point)
                {

                }


            }

            return forceLines;
        }

        private bool IsPointOnLine(PointF p1, PointF p2, PointF point_test)
        {
            float scalar = (p2.X - p1.X) * (point_test.X - p1.X) + (p2.Y - p1.Y) * (point_test.Y - p1.Y);
            if (scalar > 1e-7) return false;
            else return false;
        }

        /*Угловой коэффициент прямой одной из прямых*/
        private float Ua(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float result = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) /
                            ((p4.Y-p3.Y)*(p2.X-p1.X)-(p4.X-p3.X)*(p2.Y-p1.Y));
            return result;
        }
        private float Ub(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float result = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) /
                            ((p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y));
            return result;
        }
    }
}

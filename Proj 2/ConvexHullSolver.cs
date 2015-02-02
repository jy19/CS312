using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {

        public List<PointF> hullPoints = new List<PointF>();

        //find the right-most point in given list
        public PointF findMaxPoint(List<PointF> pointList) 
        {
            PointF maxPoint = new PointF(float.MinValue, float.MinValue);
            float max = float.MinValue;

            foreach (PointF point in pointList)
            {
                if (point.X > max)
                {
                    maxPoint = point;
                    max = point.X;
                }
            }
            return maxPoint;
        }

        //find the left-most point in given list
        public PointF findMinPoint(List<PointF> pointList) 
        {
            PointF minPoint = new PointF(float.MaxValue, float.MaxValue);
            float min = float.MaxValue;

            foreach (PointF point in pointList)
            {

                if (point.X < min)
                {
                    minPoint = point;
                    min = point.X;
                }
            }

            return minPoint;
        }

        //turn two points into 'vector'
        public PointF getVector(PointF a, PointF b)
        {
            PointF vector = new PointF(0, 0);

            vector.X = b.X - a.X;
            vector.Y = b.Y - a.Y;

            return vector;
        }

        //find the cross product of two 'vectors'
        public float getCrossProduct(PointF ab, PointF ac)
        {
            float crossProduct = 0;

            crossProduct = (ab.X * ac.Y) - (ab.Y * ac.X);

            return crossProduct;
        }

        //find point furthest away from vector
        public PointF findFurthestPoint(List<PointF> pointList, PointF minPoint, PointF maxPoint)
        {
            PointF farthest = new PointF(0, 0);
            float maxCrossProduct = float.MinValue;
            PointF vectorPoint = getVector(minPoint, maxPoint);

            foreach (PointF point in pointList)
            {
                PointF tempVector = getVector(minPoint, point);
                float tempCrossProduct = getCrossProduct(vectorPoint, tempVector);
                if(tempCrossProduct > maxCrossProduct) {
                    maxCrossProduct = tempCrossProduct;
                    farthest = point;
                }
            }

            return farthest;
        }

        //solve using divide and conquer, recursively
        public void dcSolve(List<PointF> pointList, PointF minPoint, PointF maxPoint, Boolean firstPass)
        {
            if(pointList.Count == 0) 
            {
                return;
            }

            List<PointF> resultList1 = new List<PointF>();
            List<PointF> resultList2 = new List<PointF>();

            PointF vectorPoint = getVector(minPoint, maxPoint);

            foreach (PointF point in pointList)
            {
                PointF tempVector = getVector(minPoint, point);

                //if cross product of vector and points is positive
                if(getCrossProduct(vectorPoint, tempVector) > 0) 
                {
                    resultList1.Add(point);
                }
                //else if first pass through list, add points to other list
                else if(firstPass)
                {
                    resultList2.Add(point);
                }
                //else just ignore the points as they are in the hull
                else {}

            }

            //find the furthest point if there are points in result list 1
            if (resultList1.Count > 0)
            {
                PointF furthestPoint = findFurthestPoint(resultList1, minPoint, maxPoint);
                //insert furthest point of top half to list of hull points
                int minPointIndex = hullPoints.IndexOf(minPoint);
                hullPoints.Insert(minPointIndex, furthestPoint);
                //recursively solve with split list and new points
                dcSolve(resultList1, minPoint, furthestPoint, false);
            }
            
            if(firstPass && resultList2.Count > 0) {  //if first time through the list, find bottom half 
                //go from max->min to reverse vector so furthest point is still positive
                PointF vectorPoint2 = getVector(maxPoint, minPoint);
                PointF furthestPoint2 = findFurthestPoint(resultList2, maxPoint, minPoint);
                //insert furthest point of bottom half to list of hull points
                int maxPointIndex = hullPoints.IndexOf(maxPoint);
                hullPoints.Insert(maxPointIndex, furthestPoint2);
                //recursively solve bottom half
                dcSolve(resultList2, maxPoint, furthestPoint2, false);
            } //else those points are in hull

        }

        public void Solve(System.Drawing.Graphics g, List<System.Drawing.PointF> pointList)
        {
            // Insert your code here.
            // find min and max points
            PointF maxPoint = findMaxPoint(pointList);
            PointF minPoint = findMinPoint(pointList);
            //add points to hull
            hullPoints.Add(minPoint);
            hullPoints.Add(maxPoint);

            //recursively solve for points on the hull, first time through
            dcSolve(pointList, minPoint, maxPoint, true);

            //Console.Write(String.Join("\n", pointList));

            Pen blackPen = new Pen(Color.Black, 3);


            //draw lines
            for (int i = 0; i < hullPoints.Count - 1; i++)
            {
                g.DrawLine(blackPen, hullPoints[i], hullPoints[i+1]);
            }
            //draws line between first and last points
            g.DrawLine(blackPen, hullPoints[hullPoints.Count - 1], hullPoints[0]);



               // throw new Exception("The method or operation is not implemented.");
        }
    }
}


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

        //find the cross product of three ponits
        public float getCrossProduct(PointF a, PointF b, PointF c)
        {
            float result = ((a.X - b.X) * (c.Y - b.Y)) - ((a.Y - b.Y) * (c.X - b.X));

            return result;
        }

        //calculates the distance from a point c to the line formed by points a and b. Not doing full formula 
        //because denominator will always be the same
        public float distanceFromLine(PointF a, PointF b, PointF c)
        {
            float yDiff = b.Y - a.Y;
            float xDiff = b.X - a.X;

            float distance = ((yDiff*c.X) - (xDiff*c.Y)) + b.X*a.Y - b.Y*a.X;

            if (distance < 0) distance = -distance; //make sure distance returned is positive

            return distance;
        }

        //finds the furthest point from a line
        public PointF findFurthestPoint(PointF minPoint, PointF maxPoint, List<PointF> points)
        {
            PointF farthest = new PointF();
            float distance = float.MinValue;

            foreach (PointF point in points)
            {
                float currDistance = distanceFromLine(minPoint, maxPoint, point);
                if (currDistance > distance)
                {
                    distance = currDistance;
                    farthest = point;
                }
            }

            return farthest;
        }

        public void dcSolve(List<PointF> points, PointF minPoint, PointF maxPoint)
        {
            //base case: no more points
            if (points.Count == 0) return;

            //find the furthest point
            PointF furthestPoint = findFurthestPoint(minPoint, maxPoint, points);
            //insert the furthest point to list of points on hull
            int insertIndex = hullPoints.IndexOf(minPoint);
            hullPoints.Insert(insertIndex, furthestPoint);
            //remove furthest point from list of points so program doesn't check that point again
            points.Remove(furthestPoint);
            
            List<PointF> resultList1 = new List<PointF>();
            List<PointF> resultList2 = new List<PointF>();

            foreach (PointF point in points)
            {
                //check which side to put the rest of the points on based on furthest point
                //if cross product is positive, the point lies outside so might be on hull
                if(getCrossProduct(minPoint, furthestPoint, point) > 0) {
                    resultList1.Add(point);
                }
                if (getCrossProduct(furthestPoint, maxPoint, point) > 0)
                {
                    resultList2.Add(point);
                }
            }

            //recursively solve new lists
            dcSolve(resultList1, minPoint, furthestPoint);
            dcSolve(resultList2, furthestPoint, maxPoint);

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

            //remove min and max points so they don't go through check again
            pointList.Remove(minPoint);
            pointList.Remove(maxPoint);

            List<PointF> upperHull = new List<PointF>();
            List<PointF> lowerHull = new List<PointF>();

            //split pointList into two hulls (upper, lower) to solve 
            foreach (PointF point in pointList)
            {
                if(getCrossProduct(minPoint, maxPoint, point) > 0) 
                {
                    //positive, put in upper hull
                    upperHull.Add(point);
                }
                else //negative, put in lower hull
                {
                    lowerHull.Add(point);
                }
            }
            
            //solve recursively
            dcSolve(upperHull, minPoint, maxPoint);
            dcSolve(lowerHull, maxPoint, minPoint);


            Pen blackPen = new Pen(Color.Black, 3);


            //draw lines
            for (int i = 0; i < hullPoints.Count - 1; i++)
            {
                g.DrawLine(blackPen, hullPoints[i], hullPoints[i+1]);
            }
            //draws line between first and last points
            g.DrawLine(blackPen, hullPoints[hullPoints.Count - 1], hullPoints[0]);

        }
    }
}


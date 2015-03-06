using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;


namespace VisualIntelligentScissors
{
	public class SimpleScissors : Scissors
	{
		public SimpleScissors() { }

        /// <summary>
        /// constructor for SimpleScissors. 
        /// </summary>
        /// <param name="image">the image you are going to segment including methods for getting gradients.</param>
        /// <param name="overlay">a bitmap on which you can draw stuff.</param>
		public SimpleScissors(GrayBitmap image, Bitmap overlay) : base(image, overlay) { }

        //list of points that have been settled
        HashSet<Point> settled = new HashSet<Point>();

        //list of points that have been visited
        List<Point> visited = new List<Point>();

        // this is a class you need to implement in CS 312. 

        /// <summary>
        ///  this is the class to implement for CS 312. 
        /// </summary>
        /// <param name="points">the list of segmentation points parsed from the pgm file</param>
        /// <param name="pen">a pen for writing on the overlay if you want to use it.</param>
		public override void FindSegmentation(IList<Point> points, Pen pen)
		{
            // this is the entry point for this class when the button is clicked for 
            // segmenting the image using the simple greedy algorithm. 
            // the points
            
			if (Image == null) throw new InvalidOperationException("Set Image property first.");

            colorPoints(points);
            //go point by point
            for (int i = 0; i < points.Count - 1; i++)
            {
                //dijkstraScissors(points[i], points[(i + 1) % points.Count]);
                simpleScissors(points[i], points[i + 1]);
                Program.MainForm.RefreshImage();
            }
            //go from last to first point
            simpleScissors(points[points.Count - 1], points[0]);
            Program.MainForm.RefreshImage();
			
		}

        //color the points on image, code taken from straightscissors.cs
        public void colorPoints(IList<Point> points)
        {
            Pen yellowpen = new Pen(Color.Yellow);
            using (Graphics g = Graphics.FromImage(Overlay))
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Point start = points[i];
                    Point end = points[(i + 1) % points.Count];
                    g.DrawEllipse(yellowpen, start.X, start.Y, 5, 5);
                }
            }
            
        }

        //greedy algorithm for finding the next point
        public void simpleScissors(Point start, Point goal)
        {
            Boolean foundGoal = false;
            Point currPoint = start;
            

            while (!foundGoal)
            {
                //set pixel in overlay
                Overlay.SetPixel(currPoint.X, currPoint.Y, Color.Red);

                //add the current point into settled
                settled.Add(currPoint);

                //end point is found, return
                //if(currPoint.Equals(goal))
                if(Point.Equals(currPoint, goal))
                {
                    foundGoal = true;
                    break;
                }

                //find the smallest point of surrounding points

                int smallestDist = int.MaxValue;

                //add the surrounding points to a list for easier comparing
                List<Point> neighborPoints = new List<Point>();

                //add points in clockwise order
                neighborPoints.Add(new Point(currPoint.X, currPoint.Y + 1)); //N
                neighborPoints.Add(new Point(currPoint.X + 1, currPoint.Y)); //E
                neighborPoints.Add(new Point(currPoint.X, currPoint.Y - 1)); //S
                neighborPoints.Add(new Point(currPoint.X - 1, currPoint.Y)); //W

                //for loop through points to find smallest (going through 'edges')
                foreach (Point p in neighborPoints)
                {
                    //pixel weight treated as 'distance' in a weighted graph
                    int distance = GetPixelWeight(p);

                    if(isInOverlay(p) && !settled.Contains(p) && distance < smallestDist)
                    {
                        currPoint = p;
                        smallestDist = distance;
                    }
                }

                //if smallestDist isn't changed, that means therhe's a dead end
                //all the points are already settled
                if (smallestDist == int.MaxValue)
                {
                    break;
                }

            }

        }

        //check that the point is in bounds
        public Boolean isInOverlay(Point point)
        {
            //checks that point is in overlay
            return (point.X > 1  && point.X < Overlay.Width - 2
                && point.Y > 1 && point.Y < Overlay.Height - 2);
            
        }

	}
}

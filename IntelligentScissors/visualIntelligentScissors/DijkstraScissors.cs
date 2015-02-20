using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using System.Linq;

namespace VisualIntelligentScissors
{
	public class DijkstraScissors : Scissors
	{
		public DijkstraScissors() { }
        /// <summary>
        /// constructor for intelligent scissors. 
        /// </summary>
        /// <param name="image">the image you are oging to segment.  has methods for getting gradient information</param>
        /// <param name="overlay">an overlay on which you can draw stuff by setting pixels.</param>
		public DijkstraScissors(GrayBitmap image, Bitmap overlay) : base(image, overlay) { }

        // this is the class you need to implement in CS 312
        /// <summary>
        /// this is the class you implement in CS 312. 
        /// </summary>
        /// <param name="points">these are the segmentation points from the pgm file.</param>
        /// <param name="pen">this is a pen you can use to draw on the overlay</param>
		public override void FindSegmentation(IList<Point> points, Pen pen)
		{
			if (Image == null) throw new InvalidOperationException("Set Image property first.");
            // this is the entry point for this class when the button is clicked
            // to do the image segmentation with intelligent scissors.

            colorPoints(points);

            //go point by point
            for (int i = 0; i < points.Count - 1; i++)
            {
                dijkstraScissors(points[i], points[i + 1]);
                Program.MainForm.RefreshImage();
            }
            //go from last to first point
            dijkstraScissors(points[points.Count - 1], points[0]);
            Program.MainForm.RefreshImage();
			//throw new NotImplementedException();
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

        public void dijkstraScissors(Point startPoint, Point goalPoint)
        {
            HashSet<Point> settled = new HashSet<Point>(); //set pf settled points
            //priority queue, priority is the weight/distance
            PriorityQueue<int, Node> pq = new PriorityQueue<int, Node>();
            //create a hashset that keeps track of all points in priority queue
            HashSet<Point> parallelSet = new HashSet<Point>();

            Node start = new Node(null, startPoint, GetPixelWeight(startPoint));
            pq.Enqueue(0, start);
            parallelSet.Add(start.point);

            //insert start into priority queue
            Boolean foundGoal = false;

            //declare this node outside of while so can use later
            Node smallest = null;

            while (!pq.IsEmpty && !foundGoal)
            {
               
                //go to next point with min distance, and delete from pq
                Node u = pq.DequeueValue();
                
                parallelSet.Remove(u.point);
                
                //put u in settled set
                settled.Add(u.point);

                //add the surrounding points to a list for easier comparing
                List<Point> neighborPoints = getNeighbors(u.point);
                
                //go through all of u's neighbors
                foreach (Point neighbor in neighborPoints)
                {
                    //if neighbor is same as goal point, goal is found
                    if(Point.Equals(neighbor, goalPoint)) {
                        foundGoal = true;
                        int currWeight = u.weight + GetPixelWeight(neighbor);
                        //set earlier node new found node
                        smallest = new Node(u, neighbor, currWeight);
                    }
                    //check that neighbor is not settled and is not in priority queue
                    else if(!settled.Contains(neighbor) && !parallelSet.Contains(neighbor))
                    {
                        //new weight of smallest node, or distance from start to neighbor
                        int weight = u.weight + GetPixelWeight(neighbor);
                        //create node of smallest distance
                        Node currNode = new Node(u, neighbor, weight);
                        pq.Enqueue(weight, currNode);
                        parallelSet.Add(currNode.point);
                        //insert new node into settled
                        settled.Add(currNode.point);
                    }
                }

            }

            //use the last node found to trace a path backwards to start using prev pointers
            while(smallest != null) {
                Overlay.SetPixel(smallest.point.X, smallest.point.Y, Color.Red);
                smallest = smallest.prev;
            }

        }

        public List<Point> getNeighbors(Point point)
        {
            List<Point> neighbors = new List<Point>();
            //add points in clockwise order
            Point north = new Point(point.X, point.Y + 1);
            if (isInOverlay(north))
            {
                neighbors.Add(north);
            }
            Point east = new Point(point.X + 1, point.Y);
            if (isInOverlay(east))
            {
                neighbors.Add(east);
            }
            Point south = new Point(point.X, point.Y - 1);
            if (isInOverlay(south))
            {
                neighbors.Add(south);
            }
            Point west = new Point(point.X - 1, point.Y);
            if (isInOverlay(west))
            {
                neighbors.Add(west);
            }

            return neighbors;
        }

        //check that the point is in bounds
        public Boolean isInOverlay(Point point)
        {
            //checks that point is in overlay
            return (point.X > 1 && point.X < Overlay.Width - 2
                && point.Y > 1 && point.Y < Overlay.Height - 2);

        }

	}

    //treat each point like a node
    public class Node
    {
        public Node prev; //previous point
        public Point point; //the node's point(x, y values)
        public int weight; //weight/distance

        public Node(Node prev, Point point, int weight)
        {
            this.prev = prev;
            this.point = point;
            this.weight = weight;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Node other = obj as Node;
            return this.point.Equals(other.point) && this.weight == other.weight;
        }

        public override int GetHashCode()
        {
            return this.point.GetHashCode() + this.weight.GetHashCode();
        }

    }

    //----------------------------------------------
    //priority queue based on a heap structure
    //source: http://www.codeproject.com/Articles/126751/Priority-queue-in-C-with-the-help-of-heap-data-str
    //----------------------------------------------

    public class PriorityQueue<TPriority, TValue>
    {
        private List<KeyValuePair<TPriority, TValue>> _baseHeap;
        private IComparer<TPriority> _comparer;

        public PriorityQueue()
            : this(Comparer<TPriority>.Default)
        {
        }

        public PriorityQueue(IComparer<TPriority> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException();

            _baseHeap = new List<KeyValuePair<TPriority, TValue>>();
            _comparer = comparer;
        }

        public void Enqueue(TPriority priority, TValue value)
        {
            Insert(priority, value);
        }

        private void Insert(TPriority priority, TValue value)
        {
            KeyValuePair<TPriority, TValue> val =
                new KeyValuePair<TPriority, TValue>(priority, value);
            _baseHeap.Add(val);

            // heapify after insert, from end to beginning
            HeapifyFromEndToBeginning(_baseHeap.Count - 1);
        }

        private int HeapifyFromEndToBeginning(int pos)
        {
            if (pos >= _baseHeap.Count) return -1;

            // heap[i] have children heap[2*i + 1] and heap[2*i + 2] and parent heap[(i-1)/ 2];

            while (pos > 0)
            {
                int parentPos = (pos - 1) / 2;
                if (_comparer.Compare(_baseHeap[parentPos].Key, _baseHeap[pos].Key) > 0)
                {
                    ExchangeElements(parentPos, pos);
                    pos = parentPos;
                }
                else break;
            }
            return pos;
        }

        private void ExchangeElements(int pos1, int pos2)
        {
            KeyValuePair<TPriority, TValue> val = _baseHeap[pos1];
            _baseHeap[pos1] = _baseHeap[pos2];
            _baseHeap[pos2] = val;
        }

        public TValue DequeueValue()
        {
            return Dequeue().Value;
        }

        public KeyValuePair<TPriority, TValue> Dequeue()
        {
            if (!IsEmpty)
            {
                KeyValuePair<TPriority, TValue> result = _baseHeap[0];
                DeleteRoot();
                return result;
            }
            else
                throw new InvalidOperationException("Priority queue is empty");
        }

        private void DeleteRoot()
        {
            if (_baseHeap.Count <= 1)
            {
                _baseHeap.Clear();
                return;
            }

            _baseHeap[0] = _baseHeap[_baseHeap.Count - 1];
            _baseHeap.RemoveAt(_baseHeap.Count - 1);

            // heapify
            HeapifyFromBeginningToEnd(0);
        }

        private void HeapifyFromBeginningToEnd(int pos)
        {
            if (pos >= _baseHeap.Count) return;

            // heap[i] have children heap[2*i + 1] and heap[2*i + 2] and parent heap[(i-1)/ 2];

            while (true)
            {
                // on each iteration exchange element with its smallest child
                int smallest = pos;
                int left = 2 * pos + 1;
                int right = 2 * pos + 2;
                if (left < _baseHeap.Count &&
                    _comparer.Compare(_baseHeap[smallest].Key, _baseHeap[left].Key) > 0)
                    smallest = left;
                if (right < _baseHeap.Count &&
                    _comparer.Compare(_baseHeap[smallest].Key, _baseHeap[right].Key) > 0)
                    smallest = right;

                if (smallest != pos)
                {
                    ExchangeElements(smallest, pos);
                    pos = smallest;
                }
                else break;
            }
        }

        public KeyValuePair<TPriority, TValue> Peek()
        {
            if (!IsEmpty)
                return _baseHeap[0];
            else
                throw new InvalidOperationException("Priority queue is empty");
        }


        public TValue PeekValue()
        {
            return Peek().Value;
        }

        public bool IsEmpty
        {
            get { return _baseHeap.Count == 0; }
        }

        
    }
}

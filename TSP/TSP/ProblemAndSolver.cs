using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Linq;

namespace TSP
{
    class ProblemAndSolver
    {
        private class TSPSolution
        {
            /// <summary>
            /// we use the representation [cityB,cityA,cityC] 
            /// to mean that cityB is the first city in the solution, cityA is the second, cityC is the third 
            /// and the edge from cityC to cityB is the final edge in the path.  
            /// you are, of course, free to use a different representation if it would be more convenient or efficient 
            /// for your node data structure and search algorithm. 
            /// </summary>
            public ArrayList 
                Route;

            public TSPSolution(ArrayList iroute)
            {
                Route = new ArrayList(iroute);
            }


            /// <summary>
            ///  compute the cost of the current route.  does not check that the route is complete, btw.
            /// assumes that the route passes from the last city back to the first city. 
            /// </summary>
            /// <returns></returns>
            public double costOfRoute()
            {
                // go through each edge in the route and add up the cost. 
                int x;
                City here; 
                double cost = 0D;
                
                for (x = 0; x < Route.Count-1; x++)
                {
                    here = Route[x] as City;
                    cost += here.costToGetTo(Route[x + 1] as City);
                }
                // go from the last city to the first. 
                here = Route[Route.Count - 1] as City;
                cost += here.costToGetTo(Route[0] as City);
                return cost; 
            }
        }

        #region private members
        private const int DEFAULT_SIZE = 25;
        
        private const int CITY_ICON_SIZE = 5;

        /// <summary>
        /// the cities in the current problem.
        /// </summary>
        private City[] Cities;
        /// <summary>
        /// a route through the current problem, useful as a temporary variable. 
        /// </summary>
        private ArrayList Route;
        /// <summary>
        /// best solution so far. 
        /// </summary>
        private TSPSolution bssf; 

        /// <summary>
        /// how to color various things. 
        /// </summary>
        private Brush cityBrushStartStyle;
        private Brush cityBrushStyle;
        private Pen routePenStyle;


        /// <summary>
        /// keep track of the seed value so that the same sequence of problems can be 
        /// regenerated next time the generator is run. 
        /// </summary>
        private int _seed;
        /// <summary>
        /// number of cities to include in a problem. 
        /// </summary>
        private int _size;

        /// <summary>
        /// random number generator. 
        /// </summary>
        private Random rnd;
        #endregion

        #region public members.
        public int Size
        {
            get { return _size; }
        }

        public int Seed
        {
            get { return _seed; }
        }
        #endregion

        public const int DEFAULT_SEED = -1;

        #region Constructors
        public ProblemAndSolver()
        {
            initialize(DEFAULT_SEED, DEFAULT_SIZE);
        }

        public ProblemAndSolver(int seed)
        {
            initialize(seed, DEFAULT_SIZE);
        }

        public ProblemAndSolver(int seed, int size)
        {
            initialize(seed, size);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// reset the problem instance. 
        /// </summary>
        private void resetData()
        {
            Cities = new City[_size];
            Route = new ArrayList(_size);
            bssf = null; 

            for (int i = 0; i < _size; i++)
                Cities[i] = new City(rnd.NextDouble(), rnd.NextDouble());

            cityBrushStyle = new SolidBrush(Color.Black);
            cityBrushStartStyle = new SolidBrush(Color.Red);
            routePenStyle = new Pen(Color.LightGray,1);
            routePenStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        }

        private void initialize(int seed, int size)
        {
            this._seed = seed;
            this._size = size;
            if (seed != DEFAULT_SEED)
                this.rnd = new Random(seed);
            else
                this.rnd = new Random();
            this.resetData();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// make a new problem with the given size.
        /// </summary>
        /// <param name="size">number of cities</param>
        public void GenerateProblem(int size)
        {
            this._size = size;
            resetData(); 
        }

        /// <summary>
        /// return a copy of the cities in this problem. 
        /// </summary>
        /// <returns>array of cities</returns>
        public City[] GetCities()
        {
            City[] retCities = new City[Cities.Length];
            Array.Copy(Cities, retCities, Cities.Length);
            return retCities;
        }

        /// <summary>
        /// draw the cities in the problem.  if the bssf member is defined, then
        /// draw that too. 
        /// </summary>
        /// <param name="g">where to draw the stuff</param>
        public void Draw(Graphics g)
        {
            float width  = g.VisibleClipBounds.Width-45F;
            float height = g.VisibleClipBounds.Height-15F;
            Font labelFont = new Font("Arial", 10);

            g.DrawString("n(c) means this node is the nth node in the current solution and incurs cost c to travel to the next node.", labelFont, cityBrushStartStyle, new PointF(0F, 0F)); 

            // Draw lines
            if (bssf != null)
            {
                // make a list of points. 
                Point[] ps = new Point[bssf.Route.Count];
                int index = 0;
                foreach (City c in bssf.Route)
                {
                    if (index < bssf.Route.Count -1)
                        g.DrawString(" " + index +"("+c.costToGetTo(bssf.Route[index+1]as City)+")", labelFont, cityBrushStartStyle, new PointF((float)c.X * width + 3F, (float)c.Y * height));
                    else 
                        g.DrawString(" " + index +"("+c.costToGetTo(bssf.Route[0]as City)+")", labelFont, cityBrushStartStyle, new PointF((float)c.X * width + 3F, (float)c.Y * height));
                    ps[index++] = new Point((int)(c.X * width) + CITY_ICON_SIZE / 2, (int)(c.Y * height) + CITY_ICON_SIZE / 2);
                }

                if (ps.Length > 0)
                {
                    g.DrawLines(routePenStyle, ps);
                    g.FillEllipse(cityBrushStartStyle, (float)Cities[0].X * width - 1, (float)Cities[0].Y * height - 1, CITY_ICON_SIZE + 2, CITY_ICON_SIZE + 2);
                }

                // draw the last line. 
                g.DrawLine(routePenStyle, ps[0], ps[ps.Length - 1]);
            }

            // Draw city dots
            foreach (City c in Cities)
            {
                g.FillEllipse(cityBrushStyle, (float)c.X * width, (float)c.Y * height, CITY_ICON_SIZE, CITY_ICON_SIZE);
            }

        }

        /// <summary>
        ///  return the cost of the best solution so far. 
        /// </summary>
        /// <returns></returns>
        public double costOfBssf ()
        {
            if (bssf != null)
                return (bssf.costOfRoute());
            else
                return -1D; 
        }

        /// <summary>
        ///  solve the problem.  This is the entry point for the solver when the run button is clicked
        /// right now it just picks a simple solution. 
        /// </summary>
        /// 

        //priority queue of states for the agenda, based on lower bound
        PriorityQueue<double, TSPState> agenda;
        double bssfCost; //bssf that's not an object
        //double lowerBound; //the curr lower bound

        //some functions to help split an array
        //source: http://stackoverflow.com/questions/1841246/c-sharp-splitting-an-array/1841276#1841276
        public void Split<T>(T[] array, int index, out T[] first, out T[] second)
        {
            //first = array.Take(index).ToArray();
            //second = array.Skip(index).ToArray();
            int len2 = array.Length - index;
            first = new T[index];
            second = new T[len2];
            Array.Copy(array, 0, first, 0, index);
            Array.Copy(array, index, second, 0, len2);
        }

        public void SplitMidPoint<T>(T[] array, out T[] first, out T[] second)
        {
            Split(array, array.Length / 2, out first, out second);
        }


        //public void recSplit(City[] currCities, City[] cities1, City[] cities2)
        //{
        //    //split by half
        //    SplitMidPoint(currCities, out cities1, out cities2);
        //    //sort by y
        //    Array.Sort(cities1, delegate(City x, City y) { return x.Y.CompareTo(y.Y); });
        //    Array.Sort(cities2, delegate(City x, City y) { return x.Y.CompareTo(y.Y); });
        //    Array.Reverse(cities2);
        //}

        public City[] sortByX(City[] cities)
        {
            Array.Sort(cities, delegate(City x, City y) { return x.X.CompareTo(y.X); });
            return cities;
        }

        public City[] sortByY(City[] cities)
        {
            Array.Sort(cities, delegate(City x, City y) { return x.Y.CompareTo(y.Y); });
            return cities;
        }

        public void greedySolution() {
            //greedy solution: some sorts by x and y to get path around all points
            //copy array to make changes to
            City[] copyCities = new City[Cities.Length];
            Array.Copy(Cities, copyCities, Cities.Length);
            //Array.Sort(copyCities, delegate(City x, City y) { return x.X.CompareTo(y.X); });
            copyCities = sortByX(copyCities);
            City[] cities1 = new City[Cities.Length / 2];
            City[] cities2 = new City[Cities.Length - cities1.Length];
            SplitMidPoint(copyCities, out cities1, out cities2);
            //sort by y
            //Array.Sort(cities1, delegate(City x, City y) { return x.Y.CompareTo(y.Y); });
            //Array.Sort(cities2, delegate(City x, City y) { return x.Y.CompareTo(y.Y); });
            cities1 = sortByY(cities1);
            cities2 = sortByY(cities2);
            //split those again
            City[] cities1_1 = new City[cities1.Length / 2];
            City[] cities1_2 = new City[cities1.Length - cities1_1.Length];
            City[] cities2_1 = new City[cities2.Length / 2];
            City[] cities2_2 = new City[cities2.Length - cities2_1.Length];
            
            SplitMidPoint(cities1, out cities1_1, out cities1_2);
            SplitMidPoint(cities2, out cities2_1, out cities2_2);

            cities1_1 = sortByX(cities1_1);
            cities1_2 = sortByX(cities1_2);
            cities2_1 = sortByX(cities2_1);
            cities2_2 = sortByX(cities2_2);
            Array.Reverse(cities1_2);
            Array.Reverse(cities2_2);
            

            //concat all the arrays
            var allCities = new City[Cities.Length];
            //cities1.CopyTo(allCities, 0);
            //cities2.CopyTo(allCities, cities1.Length);
            cities1_1.CopyTo(allCities, 0);
            cities2_1.CopyTo(allCities, cities1_1.Length);
            cities2_2.CopyTo(allCities, (cities1_1.Length + cities2_1.Length));
            cities1_2.CopyTo(allCities, (cities1_1.Length + cities2_1.Length + cities2_2.Length));

            //add cities to route
            for (int i = 0; i < allCities.Length; i++ )
            {
                Route.Add(allCities[i]);
            }

            //set the current bssf
            bssf = new TSPSolution(Route);
            //set the current bssf cost
            bssfCost = bssf.costOfRoute();
        }

        public void solveProblem()
        {
            Route = new ArrayList(); 
            // use the greedy algo
            greedySolution();

            // call this the best solution so far.  bssf is the route that will be drawn by the Draw method. 
            //save the solution
            bssf = new TSPSolution(Route);
            bssfCost = bssf.costOfRoute();

            //initialize stuff
            agenda = new PriorityQueue<double,TSPState>();
            //lowerBound = 0;

            //run branch and bound
            branchAndBound();
        }

        //--------------------------------------
        //branch and bound with lazy pruning
        //--------------------------------------
        public void branchAndBound()
        {
            if(!agenda.IsEmpty)
            {
                //if agenda is not empty, clear it
                agenda.clearPQ();
            }
            //variable to keep track of number of states
            double statesCount = 1;
            //build initial state
            double[][] initCostMatrix = generateInitMatrix();
            TSPState initialState = new TSPState(initCostMatrix, 0, new List<int>());
            //reduce the cost matrix/initialize lower bound
            //calcLowerBound(initialState);
            Tuple<double[][], double> matrixInfo = calcReducedCostMatix(initialState);
            initialState.costMatrix = matrixInfo.Item1;
            initialState.lowerBound = matrixInfo.Item2;

            //add the initial state to agenda with its bound
            agenda.Enqueue(initialState.lowerBound, initialState);

            //use stopwatch for time
            var stopWatch = Stopwatch.StartNew();
            var maxTime = 60000;
            //while pq is not empty, bssf>lb, time is less than 60s, keep running
            while(!agenda.IsEmpty && bssfCost != agenda.PeekValue().lowerBound && stopWatch.ElapsedMilliseconds < maxTime) 
            {
                if(statesCount < agenda.getSize()) {
                    statesCount = agenda.getSize();
                }
                //curr state is first on agenda, also remove first on agenda
                TSPState currState = agenda.DequeueValue();
                //lowerBound = currState.lowerBound;
                if(currState.lowerBound < bssfCost)
                {
                    //children = successors of curr state
                    List<TSPState> children = generateChildrenStates(currState);
                    foreach (TSPState child in children)
                    {
                        //if no time left, break
                        if (stopWatch.ElapsedMilliseconds > maxTime) { break; }
                        
                        //if child.bound is better than bssf
                        if (child.lowerBound < bssfCost)
                        {
                            //if child has all the cities
                            if (child.pathSoFar.Count == Cities.Length)
                            {
                                //possible new bssf
                                List<City> citiesList = new List<City>();
                                for (int i = 0; i < child.pathSoFar.Count; i++ )
                                {
                                    citiesList.Add(Cities[child.pathSoFar[i]]);
                                }
                                TSPSolution possibleSolution = new TSPSolution(new ArrayList(citiesList));
                                double newCost = possibleSolution.costOfRoute();
                                if (newCost < bssfCost)
                                {
                                    bssf = possibleSolution;
                                    bssfCost = newCost;
                                }
                            }
                            else
                            {
                                //add child to agenda
                                agenda.Enqueue(child.lowerBound, child);
                            }

                        }

                    }
                }
                
            }
            
            // update the cost of the tour. 
            Program.MainForm.tbCostOfTour.Text = " " + bssf.costOfRoute();
            //update the time
            Program.MainForm.tbElapsedTime.Text = " " + (stopWatch.ElapsedMilliseconds)/1000.0 + "s";
            // do a refresh. 
            Program.MainForm.Invalidate();
            Console.WriteLine("---------------most states at once? " + statesCount);
        }


        //--------------------------------------
        //method to generate the 2d array ('matrix') that represents a state
        //--------------------------------------
        public double[][] generateInitMatrix()
        {
            double[][] matrix = new double[Cities.Length][];
            //initialize the array
            for (int k = 0; k < Cities.Length; k++ )
            {
                matrix[k] = new double[Cities.Length];
            }
            for (int i = 0; i < Cities.Length; i++ )
            {
                for (int j = 0; j < Cities.Length; j++)
                {
                    if (i == j)
                    {
                        //if a city goes to itself, assign the distance as infinite (impossible)
                        matrix[i][j] = double.MaxValue;
                    }
                    else
                    {
                        //else assign it the cost of getting there from i to j
                        matrix[i][j] = Cities[i].costToGetTo(Cities[j]);
                    }
                }
            }
            return matrix;
        }

        //--------------------------------------
        //method to calculate the reduced cost matrix of a state
        //calculate lower bound then recude the cost matrix
        //--------------------------------------
        //public void calcLowerBound(TSPState state)
        public Tuple<double[][], double> calcReducedCostMatix(TSPState state)
        {
            double[][] costMatrix = state.costMatrix;
            //create a copy of the cost matrix to modify
            //double[][] copyCost = CopyArray(state.costMatrix);
            
            double lb = state.lowerBound;

            //rows
            for (int i = 0; i < costMatrix.Length; i++ )
            {
                double rowMin = double.MaxValue;
                for (int j = 0; j < costMatrix[i].Length; j++ )
                {
                    double currCost = costMatrix[i][j];
                    if(currCost < rowMin) {
                        rowMin = currCost;
                    }
                }
                //if the whole row is infinite, can't reduce this row
                if(rowMin == double.MaxValue) {
                    continue;
                }
                for (int j = 0; j < costMatrix[i].Length; j++ )
                {
                    if(costMatrix[i][j] != double.MaxValue) 
                    {
                        costMatrix[i][j] = costMatrix[i][j] - rowMin;
                    }
                }
                lb += rowMin;
            }

            //columns
            for (int i = 0; i < costMatrix.Length; i++ )
            {
                double colMin = double.MaxValue;
                for (int j = 0; j < costMatrix[i].Length; j++ )
                {
                    double currCost = costMatrix[j][i];
                    if(currCost < colMin) 
                    {
                        colMin = currCost;
                    }
                }
                //if the whole col is infinite, can't reduce this col
                if (colMin == double.MaxValue)
                {
                    continue;
                }
                for (int j = 0; j < costMatrix[i].Length; j++)
                {
                    if(costMatrix[j][i] != double.MaxValue)
                    {
                        costMatrix[j][i] = costMatrix[j][i] - colMin;
                    }
                }
                lb += colMin;
            }

            return Tuple.Create(costMatrix, lb);

            //state.lowerBound = lb;
            //state.costMatrix = costMatrix;

        }

        

        //--------------------------------------
        //method to generate a list of children states from a parent state
        //--------------------------------------
        public List<TSPState> generateChildrenStates(TSPState parentState)
        {
            List<TSPState> children = new List<TSPState>();
            //keep track of best child states
            TSPState includeState = null;
            TSPState excludeState = null;
            //keep track of best include bound
            double boundDifference = 0; // want to maximize this difference

            double[][] costMatrix = parentState.costMatrix;

            //go through array for 0s
            for (int i = 0; i < costMatrix.Length; i++ )
            {
                for (int j = 0; j < costMatrix[i].Length; j++ )
                {
                    if(costMatrix[i][j] == 0)
                    {
                        //if this city is already in the path, skip it
                        if(parentState.pathSoFar.Contains(i)) {
                            continue;
                        }
                        //include
                        TSPState currIncludeState = calcInclude(i, j, parentState);
                        //exclude
                        TSPState currExcludeState = calcExclude(i, j, parentState);
                        //calc bound difference
                        double currDifference = currExcludeState.lowerBound - currIncludeState.lowerBound;
                        if(currDifference >= boundDifference) 
                        {
                            //set curr best states
                            boundDifference = currDifference;
                            includeState = currIncludeState;
                            excludeState = currExcludeState;
                        }
                    }
                }
                
            }

            //check that the state is not null and that it passes criterion
            if(includeState != null && includeState.lowerBound < bssfCost) { children.Add(includeState); }
            if(excludeState != null && excludeState.lowerBound < bssfCost) { children.Add(excludeState); }
            
            return children;
        }

        // helper function for include
        public TSPState calcInclude(int row, int col, TSPState parentState)
        {
            double[][] costMatrix = CopyArray(parentState.costMatrix);
            //replace specified row with infinities
            for (int i = 0; i < costMatrix.Length; i++ )
            {
                costMatrix[row][i] = double.MaxValue;
            }
            //replace specified col with infinities
            for (int i = 0; i < costMatrix.Length; i++ )
            {
                costMatrix[i][col] = double.MaxValue;
            }

            //List<City> pathSoFar = parentState.pathSoFar;
            //create new list so it's not referencing old one
            List<int> pathSoFar = new List<int>(parentState.pathSoFar);
            //add currently on city to path
            //pathSoFar.Add(Cities[row]);
            pathSoFar.Add(row);

            double parentLowerBound = parentState.lowerBound;

            TSPState childState = new TSPState(costMatrix, parentLowerBound, pathSoFar);

            //reduce
            Tuple<double[][], double> newMatrixInfo = calcReducedCostMatix(childState);
            childState.costMatrix = newMatrixInfo.Item1;
            childState.lowerBound = newMatrixInfo.Item2;

            return childState;
        }

        //helper function for exclude
        public TSPState calcExclude(int row, int col, TSPState parentState)
        {
            double[][] costMatrix = CopyArray(parentState.costMatrix);
            //List<City> pathSoFar = parentState.pathSoFar;
            //create new list so it's not referencing old one
            List<int> pathSoFar = new List<int>(parentState.pathSoFar);

            //replace the excluded edge with infinite
            costMatrix[row][col] = double.MaxValue;

            double parentLowerBound = parentState.lowerBound;

            TSPState childState = new TSPState(costMatrix, parentLowerBound, pathSoFar);
            //reduce new matrix
            Tuple<double[][], double> newMatrixInfo = calcReducedCostMatix(childState);
            childState.costMatrix = newMatrixInfo.Item1;
            childState.lowerBound = newMatrixInfo.Item2;

            return childState;
        }

        //--------------------------------------
        //method to copy an array of array
        //source: http://stackoverflow.com/questions/4670720/extremely-fast-way-to-clone-the-values-of-a-jagged-array-into-a-second-array/4671179#4671179
        //--------------------------------------
        public double[][] CopyArray(double[][] source)
        {
            var len = source.Length;
            var dest = new double[len][];

            for (var x = 0; x < len; x++)
            {
                var inner = source[x];
                var ilen = inner.Length;
                var newer = new double[ilen];
                Array.Copy(inner, newer, ilen);
                dest[x] = newer;
            }

            return dest;
        }

        #endregion
    }
}

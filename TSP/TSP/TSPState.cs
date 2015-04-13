using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Priority_Queue;

namespace TSP
{
    class TSPState : PriorityQueueNode
    {
        //the cost matrix of the state
        public double[][] costMatrix {get; set;} 
        //the lower bound of this state
        public double lowerBound { get; set; }
        //the cost of this state (the bssf at this state)?
        //public double cost { get; set; }
        //the path so far
        //public List<City> pathSoFar { get; set; }
        //path so far in terms of city nums for easier debugging
        public List<int> pathSoFar { get; set; }
        //list of paths of which cities it came from and has gone to
        //public List<Tuple<int, int>> paths { get; set; }
        public Dictionary<int, int> paths { get; set; }

        //public TSPState(double[][] costMatrix, double lowerBound, double cost, List<City>pathSoFar)
        public TSPState(double[][] costMatrix, double lowerBound, List<int> pathSoFar)
        {
            this.costMatrix = costMatrix;
            this.lowerBound = lowerBound;
            //this.cost = cost;
            this.pathSoFar = pathSoFar;
            this.paths = new Dictionary<int, int>();
        }

        //some constant value to 'reward' states with for having more depth/more explored cities
        private const int SOME_CONST = 200;

        public double getPriority() {
            return this.lowerBound - this.pathSoFar.Count*SOME_CONST;
        }
    }
}

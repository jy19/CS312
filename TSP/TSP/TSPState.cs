using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSP
{
    class TSPState
    {
        //the cost matrix of the state
        public double[][] costMatrix {get; set;} 
        //the lower bound of this state
        public double lowerBound { get; set; }
        //the cost of this state (the bssf at this state)?
        //public double cost { get; set; }
        //the path so far
        public List<City> pathSoFar { get; set; }

        //public TSPState(double[][] costMatrix, double lowerBound, double cost, List<City>pathSoFar)
        public TSPState(double[][] costMatrix, double lowerBound, List<City> pathSoFar)
        {
            this.costMatrix = costMatrix;
            this.lowerBound = lowerBound;
            //this.cost = cost;
            this.pathSoFar = pathSoFar;
        }
    }
}

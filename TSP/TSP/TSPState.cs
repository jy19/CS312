using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSP
{
    class TSPState
    {
        //the matrix of the state
        private double[,] state {get; set;} 
        //the lower bound of this state
        private double lowerBound {get; set;}
        //the cost of this state (the bssf at this state)?
        private double cost {get; set;}
        //the path so far
        private List<City> pathSoFar { get; set; }

        public TSPState(double[,] state, double lowerBound, double cost, List<City>pathSoFar)
        {
            this.state = state;
            this.lowerBound = lowerBound;
            this.cost = cost;
            this.pathSoFar = pathSoFar;
        }
    }
}

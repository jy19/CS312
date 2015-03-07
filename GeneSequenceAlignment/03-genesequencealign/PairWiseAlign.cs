using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticsLab
{
    class PairWiseAlign
    {
        
        /// <summary>
        /// Align only 5000 characters in each sequence.
        /// </summary>
        private int MaxCharactersToAlign = 5000;
        
        private int C_INDEL = 5;
        private int C_SUB = 1;
        private int C_MATCH = -3;

        //store already calculated values so don't need to calculate again for scoring 
        private Dictionary<Tuple<int, int>, int> results = new Dictionary<Tuple<int, int>, int>();

        //two algos, one extraction, one scoring
        //scoring just fills out the entire table

        /// <summary>
        /// this is the function you implement.
        /// </summary>
        /// <param name="sequenceA">the first sequence</param>
        /// <param name="sequenceB">the second sequence, may have length not equal to the length of the first seq.</param>
        /// <param name="resultTableSoFar">the table of alignment results that has been generated so far using pair-wise alignment</param>
        /// <param name="rowInTable">this particular alignment problem will occupy a cell in this row the result table.</param>
        /// <param name="columnInTable">this particular alignment will occupy a cell in this column of the result table.</param>
        /// <returns>the alignment score for sequenceA and sequenceB.  The calling function places the result in entry rowInTable,columnInTable
        /// of the ResultTable</returns>
        public int Align(GeneSequence sequenceA, GeneSequence sequenceB, ResultTable resultTableSoFar, int rowInTable, int columnInTable)
        {
            // a place holder computation.  You'll want to implement your code here. 
            //align two sequences with array for dp table 

            //if on diagonal, the two values are the same
            if(rowInTable == columnInTable) {
                return 0;
            }

            //initialize the sequences with place holder at front for indel
            string seqA = "-" + sequenceA.Sequence;
            string seqB = "-" + sequenceB.Sequence;

            int score = 0;

            //tuple for current position in result table
            var currPos = Tuple.Create(rowInTable, columnInTable);
            //results.ContainsKey(new Tuple<int, int>(rowInTable, columnInTable)
            //if there already exists something, use that
            if(results.ContainsKey(currPos)) {
                score = results[new Tuple<int, int>(rowInTable, columnInTable)];
            }
            else{ //else calculate and add to results
                score = scoring(seqA, seqB);
                results.Add(currPos, score);
                //also add to the other side of diagonal so don't have to calculate again
                var currPosRev = Tuple.Create(columnInTable, rowInTable);
                results.Add(currPosRev, score);
                //results[currPos] = score;
            }

            return score;

            //return (Math.Abs(sequenceA.Sequence.Length - sequenceB.Sequence.Length));             
        }

        //find the smallest cost between 3 costs
        public int smallestCost(int cost1, int cost2, int cost3)
        {
            int min = Math.Min(cost1, cost2);
            return Math.Min(cost3, min);
        }

        public int scoring(string sequenceA, string sequenceB)
        {

            List<int> prevRow = new List<int>();
            List<int> currRow = new List<int>();
            //int [] prevRow = new int [5001];
            //int [] currRow = new int[5001];

            //align only the first 5000 chars
            //if sequence A or B is longer than 5001 chars (extra one because added "-" at front)
            int aLength = (sequenceA.Length > MaxCharactersToAlign + 1) ? MaxCharactersToAlign + 1 : sequenceA.Length;
            int bLength = (sequenceB.Length > MaxCharactersToAlign + 1) ? MaxCharactersToAlign + 1: sequenceB.Length;

            for(int i = 0; i < aLength; i++) 
            {
                for (int j = 0; j < bLength; j++ )
                {
                    //calculating cost
                    int cost = int.MaxValue;
                    if (i <= 0 && j <= 0)
                    {
                        cost = 0;
                    }
                    else if (i <= 0 && j > 0)
                    {
                        cost = currRow[j-1] + C_INDEL;
                    }
                    else if (i > 0 && j <= 0)
                    {
                        cost = prevRow[j] + C_INDEL;
                    }
                    else //i > 0 and j > 0
                    {
                        //when sequenceA[i] == sequenceB[j]
                        if (sequenceA[i] == sequenceB[j])
                        {
                            //match
                            int matchCost = prevRow[j-1] + C_MATCH;
                            //indel
                            int indelVertCost = prevRow[j] + C_INDEL;
                            int indelHorizCost = prevRow[j - 1] + C_INDEL;

                            cost = smallestCost(matchCost, indelVertCost, indelHorizCost);
                        }
                        //check other possibilities, where sequenceA[i] != sequenceB[j]
                        else
                        {
                            //sub
                            int subCost = prevRow[j - 1] + C_SUB;
                            //indel
                            int indelVertCost = prevRow[j] + C_INDEL;
                            int indelHorizCost = prevRow[j - 1] + C_INDEL;

                            cost = smallestCost(subCost, indelVertCost, indelHorizCost);
                        }
                    }
                    //add cost to currRow
                    currRow.Add(cost);
                    //currRow[j] = cost;
                }

                //make prev row curr row
                prevRow = currRow;
                //currRow.CopyTo(prevRow, 0);

                //curr row is now a new row
                currRow = new List<int>();
                //currRow = new int[5001];
            }

            //the optimal cost
             int optCost = prevRow[prevRow.Count - 1];
            //int optCost = prevRow[prevRow.Length - 1];

            return optCost;

        }

        public List<string> extractSolution(GeneSequence sequenceA, GeneSequence sequenceB, int row, int col) {

            List<string> solutions = new List<string>();

            string seqA = sequenceA.Sequence;
            string seqB = sequenceB.Sequence;

            Console.WriteLine(seqA);
            Console.WriteLine(seqB);

            //for final strings
            StringBuilder aString = new StringBuilder();
            StringBuilder bString = new StringBuilder();

            seqA = "-" + seqA;
            seqB = "-" + seqB;

            if(seqA.Length > 101) {
                seqA = seqA.Substring(0, 101);
            }

            if(seqB.Length > 101) {
                seqB = seqB.Substring(0, 101);
            }
            //same sequence
            if(row == col) 
            {
                //return the same thing
                solutions.Add(formatString(seqA));
                solutions.Add(formatString(seqB));

                return solutions;
            }
            else //calculate and extract 
            {
                //List<List<Tuple<int, int>>> backPointers = new List<List<Tuple<int, int>>>();
                Tuple<int, int>[,] backPointers = new Tuple<int, int>[101, 101];
                int[,] dpCost = new int[101, 101];
                //List<List<int>> dpCost = new List<List<int>>();

                calcBackPointers(seqA, seqB, backPointers, dpCost);

                //start off at end point
                var currPos = Tuple.Create(seqA.Length - 1, seqB.Length - 1);
                //while (currPos != null) { 
                while((currPos.Item1  != 0 && currPos.Item2 != 0) && currPos != null) {
                    int x = currPos.Item1;
                    int y = currPos.Item2;

                    //Console.WriteLine(x + ", " + y);

                    if(dpCost[x, y] == dpCost[x, y -1] + C_INDEL) 
                    {
                        aString.Append('-');
                        bString.Append(seqB[y - 1]);
                        
                    }
                    else if(dpCost[x, y] == dpCost[x -1, y] + C_INDEL)
                    {
                        aString.Append(seqA[x - 1]);
                        bString.Append('-');
                    }
                    else if(dpCost[x, y] == dpCost[x-1, y-1] + C_MATCH ||
                            dpCost[x, y] == dpCost[x - 1, y - 1] + C_SUB) 
                    {
                        aString.Append(seqA[x - 1]);
                        bString.Append(seqA[y - 1]);
                    }
                    else
                    {
                        //error
                    }

                    currPos = backPointers[x, y];
                }

                string a = formatString(aString.ToString());
                string b = formatString(bString.ToString());

                solutions.Add(a);
                solutions.Add(b);
                return solutions;
            }

            
         }

        public string formatString(string seq) {
            //remove initial '-'
            seq = seq.Substring(0, seq.Length-1);
            char[] charArray = seq.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public void calcBackPointers(String seqA, String seqB, Tuple<int, int>[,] backPointers, int[,] dpCost)
        {
            

            int charactersToExtract = 100;

            int aLength = (seqA.Length > charactersToExtract + 1) ? charactersToExtract + 1 : seqA.Length;
            int bLength = (seqB.Length > charactersToExtract + 1) ? charactersToExtract + 1 : seqB.Length;


            for (int i = 0; i < aLength; i++)
            {
                for (int j = 0; j < bLength; j++)
                {
                    if (i <= 0 && j <= 0)
                    {
                        dpCost[i,j] = 0;
                        backPointers[i, j] = null; //no back pointers at (0,0)
                    }
                    else if (i <= 0 && j > 0)
                    {
                        dpCost[i,j] = dpCost[i,j - 1] + C_INDEL;
                        backPointers[i, j] = Tuple.Create(i, j - 1);
                    }
                    else if (i > 0 && j <= 0)
                    {
                        dpCost[i,j] = dpCost[i - 1, j] + C_INDEL;
                        backPointers[i, j] = Tuple.Create(i - 1, j);
                    }
                    else //i > 0 and j > 0
                    {
                        //when sequenceA[i] == sequenceB[j]
                        if (seqA[i] == seqB[j])
                        {

                            //start off with match
                            int min = int.MaxValue;
                            if (min > dpCost[i - 1, j - 1] + C_MATCH)
                            {
                                min = dpCost[i - 1, j - 1] + C_MATCH;
                                backPointers[i, j] = Tuple.Create(i - 1, j - 1);
                            }
                            if (min > dpCost[i - 1, j] + C_INDEL)
                            {
                                min = dpCost[i - 1, j] + C_INDEL;
                                backPointers[i, j] = Tuple.Create(i - 1, j);
                            }
                            if (min > dpCost[i, j - 1] + C_INDEL)
                            {
                                min = dpCost[i, j - 1] + C_INDEL;
                                backPointers[i, j] = Tuple.Create(i, j - 1);
                            }
                            dpCost[i, j] = min;
                        }
                        //check other possibilities, where sequenceA[i] != sequenceB[j]
                        else
                        {

                            //start off with sub
                            int min = int.MaxValue;
                            if (min > dpCost[i - 1, j - 1] + C_SUB)
                            {
                                min = dpCost[i - 1, j - 1] + C_SUB;
                                backPointers[i, j] = Tuple.Create(i - 1, j - 1);
                            }
                            if (min > dpCost[i - 1, j] + C_INDEL)
                            {
                                min = dpCost[i - 1, j] + C_INDEL;
                                backPointers[i, j] = Tuple.Create(i - 1, j);
                            }
                            if (min > dpCost[i, j - 1] + C_INDEL)
                            {
                                min = dpCost[i, j - 1] + C_INDEL;
                                backPointers[i, j] = Tuple.Create(i, j - 1);
                            }
                            dpCost[i, j] = min;
                        }
                    }
                }
            }
        }
        
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Simplex : Form
    {
        public Simplex()
        {
            InitializeComponent();
        }

        private void solveBtn_Click(object sender, EventArgs e)
        {
            initialize();
            solvewSimplex();
            
            CopperText.Text = b[0].ToString();
            GoldText.Text = b[1].ToString();
            SilverText.Text = b[2].ToString();
            PlatText.Text = b[3].ToString();
            ResultTxt.Text = v.ToString();
        }

        //private int[] N; //positions of non-basic variables
        private List<int> N;
        //private int[] B; //positions of basic variables
        private List<int> B;
        private double[,] A; //padded matrix
        private double[] b; //solutions
        private double[] c; //objective function
        private double v = 0; //result

        private Tuple<double[], double> solvewSimplex()
        {
            int e; //e is the entering var

            do{
                //if there exists an e in N such that c_e > 0
                if (getPosE() != -1)
                {
                    e = getPosE();
                    double dMin = double.MaxValue; //dMin is set to infinite
                    int l = -1; //l is the leaving var
                    foreach (int index in B)
                    {
                        if (A[index, e] > 0)
                        {
                            double d = b[index] / A[index, e]; //d is check ratio for current constraint
                            if(d < dMin) {
                                //update the min
                                dMin = d;
                                //update the leaving var index
                                l = index;
                            }
                        }
                    }
                    //l in B is now index of smallest check ratio : dMin
                    if(dMin == double.MaxValue) {
                        //unbounded
                        //return something...(currently just returning an empty array + 'infinite')
                        return Tuple.Create(new double[0], double.MaxValue);
                    }
                    else
                    {
                        //else pivot with new entering var
                        pivot(e, l);
                    }
                }
                else
                {
                    e = -1;
                }
            } while(e != -1); //while there are still entering variables
            
            //set non-basic variables to 0 and everything else to optimal solution
            for (int i = 0; i < b.Length; i++ )
            {   
                //if i is in B
                if(B.Contains(i)) {
                    c[i] = b[i];
                }
                else
                {
                    c[i] = 0;
                }
            }

            return Tuple.Create(c, v);
        }

        //e is entering var
        //l is leaving var
        private void pivot(int e, int l)
        {
            //compute coefficients for contraint for new basic var x_e
            b[e] = b[l]/A[l, e];

            //for each j in N besides e
            foreach (int j in N)
            {
                if(j == e) {
                    continue; //skip e
                }
                A[e, j] = A[l, j] / A[l, e];
            }

            A[e, l] = 1 / A[l, e];
            //compute the new coefficients for the other constraints
            //for each i in B besidse l
            foreach (int i in B)
            {
                if(i == l) {
                    continue; //skip l
                }
                b[i] = b[i] - (A[i, e] * b[e]);
                foreach (int j in N)
                {
                    if(j == e) {
                        continue;
                    }
                    A[i, j] = A[i, j] - (A[i, e] * A[e, j]);
                }
                A[i, l] = (A[i, e] * A[e, l]) * -1;
            }

            //compute the new objective function
            v = v + (c[e] * b[e]);
            foreach (int j in N)
            {
                if(j == e) {
                    continue;
                }
                c[j] = c[j] - (c[e] * A[e, j]);
            }

            c[l] = (c[e]*A[e, l])*-1; 

            //compute new basic and non-basic variable index sets
            N.Remove(e);
            N.Add(l);

            B.Remove(l);
            B.Add(e);

        }

        private void initialize()
        {
            //initialize the padded matrix with constraints
            A = new double[9, 9] 
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0.1, 0.5, 0.333, 1, 0, 0, 0, 0, 0},
                {30, 15, 19, 12, 0, 0, 0, 0, 0},
                {1000, 6000, 4100, 9100, 0, 0, 0, 0, 0},
                {50, 20, 21, 10, 0, 0, 0, 0, 0},
                {4, 6, 19, 30, 0, 0, 0, 0, 0}
            };

            //initialize c
            c = new double[9] { 10.2, 422.3, 6.91, 853, 0, 0, 0, 0, 0};

            //initialize b
            b = new double[9] { 0, 0, 0, 0, 2000, 1000, 1000000, 640, 432};

            //initialize where the basic and non-basic vars are
            //N = new int[4] { 0, 1, 2, 3 };
            //B = new int[5] { 4, 5, 6, 7, 8};
            N = new List<int>(new int[] { 0, 1, 2, 3});
            B = new List<int>(new int[] { 4, 5, 6, 7, 8 });
        }

        //gets the index of positive value in N for e
        private int getPosE()
        {
            foreach (int index in N)
            {
                if (c[index] > 0) 
                    return index;
            }
            return -1; //if no positive value found in N
        }

    }
}

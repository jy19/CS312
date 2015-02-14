using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace Lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void solve_Click(object sender, EventArgs e)
        {
            BigInteger inputNum;
            BigInteger.TryParse(this.input.Text, out inputNum);
            output.Text = primality2(inputNum);
        }

        //Modular Exponentiation
        private BigInteger modexp(BigInteger basenum, BigInteger exponent, BigInteger mod)
        {
            if (exponent == 0)
            {
                return 1;
            }
            BigInteger ans = modexp(basenum, BigInteger.Divide(exponent, (BigInteger)2), mod);
            if (exponent % 2 == 0)
            {
                return BigInteger.Pow(ans, 2) % mod;
            }
            else
            {
                return basenum * BigInteger.Pow(ans, 2) % mod;
            }
        }

        //generates a random value A from 1 to input
        private BigInteger generateUniformA(BigInteger input)
        {
            Random rand = new System.Random();

            BigInteger length = (BigInteger) BigInteger.Log(input, 2.0) + 1;

            BigInteger randomNum = 0;
            for (int i = 0; i < length / 32; i++)
            {
                randomNum = (randomNum << 32) + rand.Next();
            }
            randomNum %= input;
            if(1 < randomNum && randomNum < input) 
            {
                return randomNum;
            }
            else
            {
                return generateUniformA(input);
            }
            
        }

        //Fermat's primality test, with low probability 
        private String primality2(BigInteger input)
        {
            int k = 20;
            //System.Diagnostics.Debug.Write("primality2\n");
            for (int i = 0; i < k; i++ )
            {

                BigInteger a = generateUniformA(input);
                if(modexp(a, input-1, input) == 1)
                {
                    //possibly prime
                }
                else
                {
                    return "Not prime.";
                }

            }

            double p = 1 - (1/Math.Pow(2, k));

            return "Yes, with correctness " +  p;
           
        }
    }
}

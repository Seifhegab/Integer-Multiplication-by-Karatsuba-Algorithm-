using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class IntegerMultiplication
    {
        #region YOUR CODE IS HERE

        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 large integers of N digits in an efficient way [Karatsuba's Method]
        /// </summary>
        /// <param name="X">First large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="Y">Second large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="N">Number of digits (power of 2)</param>
        /// <returns>Resulting large integer of 2xN digits (left padded with 0's if necessarily) [0: least signif., 2xN-1: most signif.]</returns>
        static public byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {
            //REMOVE THIS LINE BEFORE START CODING
            // throw new NotImplementedException();

            //base case
            if (148 >= N)
            {
                int length_X = X.Length;
                int length_Y = Y.Length;
                short[] result_arr = new short[length_X + length_Y];
                int i = 0;

                while (i < length_X)
                {
                    for (int i2 = 0; i2 < length_Y; i2++)
                    {
                        int p = X[i] * Y[i2];
                        result_arr[i + i2] += (short)(p % 10);
                        result_arr[i + i2 + 1] += (short)(p / 10);
                    }
                    i++;
                }

                int i3 = 0;
                while (i3 < result_arr.Length - 1)
                {
                    if (10 <= result_arr[i3])
                    {
                        result_arr[i3 + 1] += (short)(result_arr[i3] / 10);
                        result_arr[i3] %= 10;
                    }
                    i3++;
                }
                return result_arr.Select(x => (byte)x).ToArray();
            }
            else if (N == 1)
            {
                int i = 0;
                byte temp = 0;
                byte[] multiply = new byte[2];
                temp = (byte)(X[0] * Y[0]);
                for (i = 0; i < 2; i++)
                {
                    multiply[i] = (byte)(temp % 10);
                    temp = (byte)(temp / 10);
                }
                return multiply;
            }

            // setup Padding of odd arrays
            int n_divideby2;
            if (N % 2 != 0)
            {
                int countX = X.Length + 1;
                byte[] pad_left1 = new byte[countX];
                int countY = Y.Length + 1;
                byte[] pad_left2 = new byte[countY];
                for (int i = 0; i < countX; i++)
                {
                    if (i >= countX - 1)
                    {
                        pad_left1[i] = 0;
                        pad_left2[i] = 0;
                    }
                    else
                    {
                        pad_left1[i] = X[i];
                        pad_left2[i] = Y[i];
                    }
                }
                X = pad_left1;
                Y = pad_left2;

                N = N + 1;
                n_divideby2 = N / 2;
            }
            else
            {
                n_divideby2 = N / 2;
            }

            // splitting arrays
            byte[] split_array_a = new byte[n_divideby2];
            byte[] split_array_b = new byte[n_divideby2];
            byte[] split_array_c = new byte[n_divideby2];
            byte[] split_array_d = new byte[n_divideby2];
            int j = n_divideby2;
            int k = 0;
            for (int i = 0; i < N; i++)
            {
                if(i < j)
                {
                    split_array_a[i] = X[i];
                    split_array_c[i] = Y[i];
                }
                else
                {
                    split_array_b[k] = X[i];
                    split_array_d[k] = Y[i];
                    k++;
                }
            }

            //Multiplication of ac
            
            byte[] a_mult_c = IntegerMultiply(split_array_a, split_array_c, n_divideby2);
            //Multiplication of bd
            
            byte[] b_mult_d = IntegerMultiply(split_array_b, split_array_d, n_divideby2);

            // Adding Arrays
            int carry1 = 0;
            int carry2 = 0;
            int temp1 = 0;
            int temp2 = 0;
            byte[] sum1 = new byte[split_array_a.Length + 1];
            byte[] sum2 = new byte[split_array_c.Length + 1];
            for (int i = 0; i < split_array_a.Length; i++)
            {
                temp1 = (byte)(split_array_a[i] + split_array_b[i] + carry1);
                sum1[i] = (byte)(temp1 % 10);
                carry1 = (byte)(temp1 / 10); ;

                temp2 = (byte)(split_array_c[i] + split_array_d[i] + carry2);
                sum2[i] = (byte)(temp2 % 10);
                carry2 = (byte)(temp2 / 10); ;
            }
            sum1[split_array_a.Length] = (byte)carry1;
            sum2[split_array_c.Length] = (byte)carry2;

            //Multiplication of (a+b)(c+d)
            byte[] mult_sum_arrays;
            if (sum1[sum1.Length - 1] == 0 && sum2[sum2.Length - 1] == 0)
            {
                mult_sum_arrays = IntegerMultiply(sum1, sum2, n_divideby2);
            }
            else
            {
                if (sum1.Length % 2 != 0 && sum2.Length % 2 != 0)
                {
                    int count1 = sum1.Length + 1;
                    byte[] pad_left1 = new byte[count1];
                    int count2 = sum2.Length + 1;
                    byte[] pad_left2 = new byte[count2];
                    for (int i = 0; i < count1; i++)
                    {
                        if (i >= count1 - 1)
                        {
                            pad_left1[i] = 0;
                            pad_left2[i] = 0;
                        }
                        else
                        {
                            pad_left1[i] = sum1[i];
                            pad_left2[i] = sum2[i];
                        }
                    }
                    sum1 = pad_left1;
                    sum2 = pad_left2;
                }
                mult_sum_arrays = IntegerMultiply(sum1, sum2, sum1.Length);
            }
            // padding before Subtracting 
            if (mult_sum_arrays.Length != a_mult_c.Length)
            {
                int count_ac = a_mult_c.Length + N;
                byte[] pad_left_ac = new byte[count_ac];
                for (int i = 0; i < count_ac; i++)
                {
                    if (i >= count_ac - N)
                    {
                        pad_left_ac[i] = 0;
                    }
                    else
                    {
                        pad_left_ac[i] = a_mult_c[i];
                    }
                }
                a_mult_c = pad_left_ac;
            }

            if (mult_sum_arrays.Length != b_mult_d.Length)
            {
                int count_bd = b_mult_d.Length + N;
                byte[] pad_left_bd = new byte[count_bd];
                for (int i = 0; i < count_bd; i++)
                {
                    if (i >= count_bd - N)
                    {
                        pad_left_bd[i] = 0;
                    }
                    else
                    {
                        pad_left_bd[i] = b_mult_d[i];
                    }
                }
                b_mult_d = pad_left_bd;
            }

            // Subtraction
            int len = mult_sum_arrays.GetLength(0);
            byte[] Subtract = new byte[len];
            for (int i = 0; i < len; i++)
            {

                LOOP2: if (mult_sum_arrays[i] - (a_mult_c[i] + b_mult_d[i]) < 0)
                {

                    int b = i;
                    LOOP: if (b + 1 != len)
                    {
                        if (mult_sum_arrays[b + 1] != 0)
                        {
                            mult_sum_arrays[b + 1] -= 1;
                            mult_sum_arrays[i] += 10;
                        }
                        else
                        {
                            mult_sum_arrays[b + 1] += 9;
                            b++;
                            goto LOOP;
                        }
                        goto LOOP2;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Subtract[i] = (byte)(mult_sum_arrays[i] - (a_mult_c[i] + b_mult_d[i]));
                }

            }
            mult_sum_arrays = Subtract;

            // right & left padding for(a+b)(c+d)
            int count_mult_sum0 = mult_sum_arrays.GetLength(0) + n_divideby2;
            byte[] pad_right_mult_sum = new byte[count_mult_sum0];

            for (int i = 0; i < count_mult_sum0; i++)
            {
                if (i >= n_divideby2)
                {
                    pad_right_mult_sum[i] = mult_sum_arrays[i - n_divideby2];
                }
                else
                {
                    pad_right_mult_sum[i] = 0;
                }
            }
            mult_sum_arrays = pad_right_mult_sum;


            int count_mult_sum1 = mult_sum_arrays.Length + n_divideby2;
            byte[] pad_left_mult_sum = new byte[count_mult_sum1];
            for (int i = 0; i < count_mult_sum1; i++)
            {
                if (i >= count_mult_sum1 - n_divideby2)
                {
                    pad_left_mult_sum[i] = 0;
                }
                else
                {
                    pad_left_mult_sum[i] = mult_sum_arrays[i];
                }
            }
            mult_sum_arrays = pad_left_mult_sum;

            // left padding for a*c after subtracting
            int count_ac1 = a_mult_c.Length + N;
            byte[] pad_left_ac1 = new byte[count_ac1];
            for (int i = 0; i < count_ac1; i++)
            {
                if (i >= count_ac1 - N)
                {
                    pad_left_ac1[i] = 0;
                }
                else
                {
                    pad_left_ac1[i] = a_mult_c[i];
                }
            }
            a_mult_c = pad_left_ac1;

            // left padding for b*d after subtracting
            int count_bd1 = b_mult_d.GetLength(0) + N;
            byte[] pad_left_bd1 = new byte[count_bd1];

            for (int i = 0; i < count_bd1; i++)
            {
                if (i >= N)
                {
                    pad_left_bd1[i] = b_mult_d[i - N];
                }
                else
                {
                    pad_left_bd1[i] = 0;
                }
            }
            b_mult_d = pad_left_bd1;

            // Adding arays
            byte[] result_array = new byte[2 * N];

            int carry3 = 0;
            int temp3 = 0;
            byte[] sum3 = new byte[2 * N];
            for (int i = 0; i < 2 * N; i++)
            {
                temp3 = (byte)(b_mult_d[i] + mult_sum_arrays[i] + a_mult_c[i] + carry3);
                sum3[i] = (byte)(temp3 % 10);
                carry3 = (byte)(temp3 / 10); ;
            }
            result_array = sum3;
            return result_array;
        }
        #endregion
    }
}
using System;

namespace MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {

            int ARRAY_SIZE = 10;
            int[] arraySingleThread = new int[ARRAY_SIZE];

            


            // TODO : Use the "Random" class in a for loop to initialize an array
            Random rand = new Random();
            int i;
            for (i=0;  i<ARRAY_SIZE; i++)
            {
                arraySingleThread[i] = rand.Next(500);
                //Console.WriteLine("index {0} = value {1}",i, arraySingleThread[i]);
            }

            // copy array by value.. You can also use array.copy()
            // int[] arrayMultiThread = arraySingleThread.Slice(0,arraySingleThread.Length);
            int[] arrayMultiTread = new int[ARRAY_SIZE];
            Array.Copy(arraySingleThread, arrayMultiTread, ARRAY_SIZE);
            PrintArray(arraySingleThread);
            PrintArray(arrayMultiTread);

            /*TODO : Use the  "Stopwatch" class to measure the duration of time that
               it takes to sort an array using one-thread merge sort and
               multi-thead merge sort
            */


            //TODO :start the stopwatch
            int[] test = { 4, 4, 5, 8, 2, 2, 5, 4, 9, 7 };
            
            int [] sortedArray = MergeSort(arraySingleThread);
            PrintArray(sortedArray);
            //TODO :Stop the stopwatch



            //TODO: Multi Threading Merge Sort







            /*********************** Methods **********************
             *****************************************************/
            /*
            implement Merge method. This method takes two sorted array and
            and constructs a sorted array in the size of combined arrays
            */

            static int[] Merge(int[] LA, int[] RA, int[] A)
            {
                // TODO :implement
                int k = 0;
                int i = 0;
                int j = 0;

               
                while ( i < LA.Length && j < RA.Length)
                {
                        
                    if (LA[i] <= RA[j])
                    {
                        A[k] = LA[i];
                        k++;
                        i++;
                    }
                    else
                    {
                        A[k] = RA[j];
                        j++;
                        k++;
                    }
                        
                }
                while (i < LA.Length)
                {
                    A[k] = LA[i];
                    i++;
                    k++;
                }
                while (j < RA.Length)
                {
                    A[k] = RA[j];
                    j++;
                    k++;
                }
                
                PrintArray(A);
                return A;
            }


            /*
            implement MergeSort method: takes an integer array by reference
            and makes some recursive calls to intself and then sorts the array
            */
            static int[] MergeSort(int[] A)
            {
                if (A.Length <= 1)
                {
                    return A;
                }

                int midpoint = (A.Length)/2;
                int[] left = new int[midpoint];
                int[] right = new int[A.Length-midpoint];
                
                for(int i=0; i<midpoint; i++)
                {
                    left[i] = A[i];
                }
                for (int i = 0; i < A.Length-midpoint; i++)
                {
                    right[i] = A[i+midpoint];
                }
                PrintArray(left);
                PrintArray(right);

                left = MergeSort(left);
                right = MergeSort(right);
                return (Merge(left, right, A));
              
            }


            // a helper function to print your array
            static void PrintArray(int[] myArray)
            {
                Console.Write("[");
                for (int i = 0; i < myArray.Length; i++)
                {
                    Console.Write("{0} ", myArray[i]);

                }
                Console.Write("]");
                Console.WriteLine();

            }

            // a helper function to confirm your array is sorted
            // returns boolean True if the array is sorted
            static bool IsSorted(int[] a)
            {
                int j = a.Length - 1;
                if (j < 1) return true;
                int ai = a[0], i = 1;
                while (i <= j && ai <= (ai = a[i])) i++;
                return i > j;
            }


        }


    }
}

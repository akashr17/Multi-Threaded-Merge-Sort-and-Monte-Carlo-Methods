using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace MergeSort
{
    class Program
    {

        
        static void Main(string[] args)
        {
            
            int ARRAY_SIZE = 1000000;

            //Initialise empty arrays
            int[] arraySingleThread = new int[ARRAY_SIZE];
            int[] arrayMultiTread = new int[ARRAY_SIZE];

            // TODO : Use the "Random" class in a for loop to initialize an array
            Random rand = new Random();
            
            for (int i = 0; i < ARRAY_SIZE; i++)
            {
                arraySingleThread[i] = rand.Next(1000);
                //Console.WriteLine("index {0} = value {1}",i, arraySingleThread[i]);
            }

      
            // Copy Array values
            Array.Copy(arraySingleThread, arrayMultiTread, ARRAY_SIZE);
            //PrintArray(arraySingleThread);
            //PrintArray(arrayMultiTread);


            /*TODO : Use the  "Stopwatch" class to measure the duration of time that
               it takes to sort an array using one-thread merge sort and
               multi-thead merge sort
            */


            //TODO :start the stopwatch
            Stopwatch stopWatch = new Stopwatch();

            // Check the time taken to run the Merge Sort for single thread
            stopWatch.Start();
            int[] sortedArray = MergeSort(arraySingleThread);
            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;
            
            // print time taken
            Console.WriteLine("Single Thread is sorted? {0}", IsSorted(sortedArray));
            Console.WriteLine("Time for single tread is {0}", ts);

            //TODO :Stop the stopwatch



            //TODO: Multi Threading Merge Sort
            
            // State the number of threads wanting to be used
            int numOfThreads = 4;

            Stopwatch stopWatch1 = new Stopwatch();

            // Check the time taken to run the Merge Sort for single thread
            stopWatch1.Start();
            int[] sortedMultiThreadArray = MultiThreadSort(arrayMultiTread, numOfThreads);
            stopWatch1.Stop();

            TimeSpan ts1 = stopWatch1.Elapsed;

            //PrintArray(sortedMultiThreadArray);
            Console.WriteLine("Multi-Thread is sorted {0}", IsSorted(sortedMultiThreadArray));
            Console.WriteLine("Time for multi tread is {0}", ts1);

            // Print the speed up factor
            Console.WriteLine("Speed up factor is {0}", ts.TotalSeconds / ts1.TotalSeconds);



            /*********************** Methods **********************
             *****************************************************/
            /*
            implement Merge method. This method takes two sorted array and
            and constructs a sorted array in the size of combined arrays
            */
        }

        // Method used to take 2 sorted arrays and merge them into 1 large sorted array 
        static int[] Merge(int[] LA, int[] RA, int[] A)
        {
            // TODO :implement
            int k = 0;
            int i = 0;
            int j = 0;

               
            /* loop through the left and right arrays comparing the values of 1 to the other to 
            determine where they belong in the larger sorted array*/
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

            // place any left over elements in the larger sorted array
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
                
            //PrintArray(A);
            return A;
        }


        /*
        implement MergeSort method: takes an integer array by reference
        and makes some recursive calls to intself and then sorts the array
        */
        static int [] MergeSort(int[] A)
        {

            // if arrays are broken down to just the last element return
            if (A.Length <= 1)
            {
                return A;
            }

            // determine the midpoint and use that to create the left and right subarrays
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
            //PrintArray(left);
            //PrintArray(right);

            /* recursivly call the mergesort function on left and right array until arrays
               are split down to just 1 element in each and then begin merging the sorted arrays*/

            left = MergeSort(left);
            right = MergeSort(right);
            return (Merge(left, right, A));
           
              
        }
            
        // Method for the multi threaded sort
        static int[] MultiThreadSort(int [] A, int num)
        {
            // Create a queue to store all of the sorted subarryas
            Queue<int[]> qArray = new Queue<int[]>();


            // jagged array for storing all of the subs
            int[][] sub = new int[num][];

            Thread[] t = new Thread[num];

            int subSize = 0;
            int sum = 0;
            
            // populate subarrays
            for (int i = 0; i < num; i++)
            {
                /* To determine an equal split of the array into subarrays,
                there is a loop which determines the subarray size by using the 
                remaining length of the large array and the remaining number of threads
                that it needs to be split into */
                subSize = (A.Length-sum) / (num-i);
                    
                sub[i] = new int[subSize];
                Array.Copy(A, sum, sub[i], 0, subSize);

                //PrintArray(sub[temp]);
                sum += subSize;
            }

            int temp = 0;
            /* loop through the num of threads and create a new thread which calls MergeSort for each
            sub array*/
            for (int i=0; i<num; i++)
            {
                // using temp variable to ensure i is locked for each loop
                temp = i;
                // explicitly re-wrote the sub arrays due to template showing a int [] return for the functions
                t[temp] = new Thread(() => sub[temp]=MergeSort(sub[temp]));
                t[temp].Start();
            }

            // loop through the join the threads and place the sub arrays into a queue
            for (int i = 0; i < num; i++)
            {
                t[i].Join();
                qArray.Enqueue(sub[temp]);

            }

            /* while the queue count is greater than 1 (there is more than 1 sub array in queue,
            keep looping and taking out 2 arrays to merge them together and then place the larger
            merged array back into the queue until 1 array left */
            while (qArray.Count>1)
            {
                int[] temp1 = qArray.Dequeue();

                int[] temp2 = qArray.Dequeue();

                int[] temp3 = new int[temp1.Length + temp2.Length];
                Merge(temp1, temp2, temp3);
                qArray.Enqueue(temp3);
            }
            return qArray.Dequeue();
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

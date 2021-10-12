using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace MultiThreadPi
{
    class MainClass
    {
        static void Main(string[] args)
        {
            long numberOfSamples = 1000000;
            long hits = 0;
            long singleThreadHits = 0;

            /* start timer and check how long single thread takes
            run the single thread estimate pi with full # of samples */

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            EstimatePI(numberOfSamples, ref singleThreadHits);
            
            stopWatch.Stop();
            double singlePI = 4.0 * singleThreadHits / numberOfSamples;
            TimeSpan ts = stopWatch.Elapsed;

            Console.WriteLine("Single threaded pi = {0} and took {1} seconds", singlePI, ts);

            int numOfThreads = 4;

            // Multi thread timing 
            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            long sum = 0;

            Thread[] t = new Thread[numOfThreads];

            /* loop through # of threads and start estimate pi for each thread,
             the samples for thread is done similarly to the first part,
            taking the remaining number of samples divided by remaining threads */
            for (int i = 0; i < numOfThreads; i++)
            {
                int temp = i;
                long samplePerThread = (numberOfSamples-sum)/(numOfThreads-temp);
                t[temp] = new Thread(() => EstimatePI(samplePerThread, ref hits));
                t[temp].Start();
                sum+=samplePerThread;
            }
            stopWatch1.Stop();
            
            // Join the threads
            for (int i = 0; i < numOfThreads; i++)
            {
                t[i].Join();
            }
            // calculate pi
            double pi = 4.0 * hits / numberOfSamples;
            TimeSpan ts1 = stopWatch1.Elapsed;
            
            Console.WriteLine("Multi threaded pi = {0} and took {1} seconds", pi, ts1);

            // Print out speed up factor 
            Console.WriteLine("Speed up factor = {0}", ts.TotalSeconds/ts1.TotalSeconds);

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();

        }
        
        static void EstimatePI(long numberOfSamples, ref long hits)
        {
            // generate the number of samples needed and place into 2D array
            double[,] samples = GenerateSamples(numberOfSamples);
            
            // for each index of the samples, check if it is a hit or not
            for (long i = 0; i < numberOfSamples; i++)
            {
                double x = samples[i, 0];
                double y = samples[i, 1];

                if ((x * x) + (y * y) <= 1)
                {
                    Interlocked.Increment(ref hits);
                }

            }
            

            
        }

        // generate the samples using Next.Double()
        static double[,] GenerateSamples(long numberOfSamples)
        {
            // Implement 
            Random seed = new Random();
            double[,] samples = new double[numberOfSamples,2];

            for (long i = 0; i < numberOfSamples; i++)
            {
                samples[i,0] = seed.NextDouble() * 2 - 1.0;
                samples[i,1] = seed.NextDouble() * 2 - 1.0;
                
            }
            
            return samples;
        }
        
        
    }
}

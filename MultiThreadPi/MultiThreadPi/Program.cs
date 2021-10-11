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
            long numberOfSamples = 10000;
            long hits = 0;
            long singleThreadHits = 0;



            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            EstimatePI(numberOfSamples, ref singleThreadHits);

            double singlePI = 4.0 * singleThreadHits / numberOfSamples;

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;





           
            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();

            int numOfThreads = 4;
            int sum = 0;

            Thread[] t = new Thread[numberOfSamples];

            for (int i = 0; i < numOfThreads; i++)
            {
                int temp = i;
                int samplePerThread = ((int)numberOfSamples-sum)/(numOfThreads-temp);
                t[temp] = new Thread(() => EstimatePI(samplePerThread, ref hits));
                t[temp].Start();
                sum+=samplePerThread;

            }
            for (int i = 0; i < numOfThreads; i++)
            {
                t[i].Join();
            }

            double pi = 4.0 * hits / numberOfSamples;

            stopWatch1.Stop();

            TimeSpan ts1 = stopWatch1.Elapsed;
            
           

            Console.WriteLine("Single threaded pi = {0} and took {1} seconds", singlePI, ts);
            Console.WriteLine("Multi threaded pi = {0} and took {1} seconds", pi, ts1);
            Console.WriteLine("Speed up factor = {0}", ts.TotalSeconds/ts1.TotalSeconds);
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

        }
        
        static void EstimatePI(long numberOfSamples, ref long hits)
        {
            double[,] samples = GenerateSamples(numberOfSamples);
            

            for (int i = 0; i < numberOfSamples; i++)
            {
                double x = samples[i, 0];
                double y = samples[i, 1];

                if ((x * x) + (y * y) <= 1)
                {
                    Interlocked.Increment(ref hits);
                }

            }
            

            
        }
        static double[,] GenerateSamples(long numberOfSamples)
        {
            // Implement 
            Random test = new Random();
            double[,] samples = new double[numberOfSamples,2];

            for (int i = 0; i < numberOfSamples; i++)
            {
                samples[i,0] = test.NextDouble() * 2 - 1.0;
                samples[i,1] = test.NextDouble() * 2 - 1.0;
                
            }
            
            return samples;
        }
        
        
    }
}

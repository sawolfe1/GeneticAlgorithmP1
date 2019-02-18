using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GeneticAlgorithmP1
{
    class Program
    {

        private static Random _randomGen = new Random(69);

        static void Main(string[] args)
        {
            var N = 10;
            var Pc = 0.8;
            var Pm = 0.1;
            var lowerBound = -1;
            var upperbound = 5;

            var AverageOfAogFitness = new List<double>();
            var AverageOfBogFitness = new List<double>();
            var AverageOfBestOfRunFitness = new List<double>();

            for (int i = 0; i < 31; i++)
            {
                var run = new GeneticAlgorithm(N, Pc, Pm, lowerBound, upperbound);

                GenerateGenerations(run);
                AverageOfAogFitness.Add(run.GetAverageOfGenerationsFitness());
                AverageOfBogFitness.Add(run.GetBestOfGenerationsFitness());
                AverageOfBestOfRunFitness.Add(run.BestOfRun.Fitness);

            }

            Console.WriteLine("**************************************************************\n" +
                              "After 30 Runs of 50 Generations each...\n" +
                              $"Average of AOG's: {AverageOfAogFitness.Average()}, StDev : {StandDev(AverageOfAogFitness)}\n" +
                              $"Average of BOG's: {AverageOfBogFitness.Average()}, StDev : {StandDev(AverageOfBogFitness)}\n" +
                              $"Average of BOR's: {AverageOfBestOfRunFitness.Average()}, StDev : {StandDev(AverageOfBestOfRunFitness)}\n" +
                              $"**************************************************************\n");

            Console.ReadLine();

        }

        private static void GenerateGenerations(GeneticAlgorithm run)
        {
            for (int i = 0; i < 51; i++)
            {
                run.AddNewGeneration();

                if (i % 10 == 0)
                {
                    Console.WriteLine($"*******Generation {i}*******");
                    Console.WriteLine("Best Fitness:");
                    Console.WriteLine(run.CandidateInfo(run.GetBestFitness()));
                    Console.WriteLine("Worst Fitness:");
                    Console.WriteLine(run.CandidateInfo(run.GetWorstFitness()));
                    Console.WriteLine("Best of Run:");
                    Console.WriteLine(run.CandidateInfo(run.BestOfRun));
                    Console.WriteLine("-----------------------------------------------");
                }
            }
        }

        public static double StandDev(List<double> values)
        {
            double ret = 0;
            var count = values.Count;
            if (count > 1)
            {
                double avg = values.Average();
                double sum = values.Sum(d => (d - avg) * (d - avg));
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }

    }
}

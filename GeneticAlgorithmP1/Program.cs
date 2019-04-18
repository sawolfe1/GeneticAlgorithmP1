using System;
using System.Collections.Generic;
using System.Linq;

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
            var gens = 50;

            Console.WriteLine("============================================================\n" +
                              "Selection Strategies:\n" +
                              "\t 1. Binary Tournament\n" +
                              "\t 2. Fitness Proportional\n" +
                              "\t 3. Truncation Selection (40%)\n\n" +
                              "Recombination Strategies:\n" +
                              "\t 1. Single-Point\n" +
                              "\t 2. Two-point\n" +
                              "\t 3. Uniform\n\n" +
                              "Mutation Strategies:\n" +
                              "\t 1. Gaussian\n" +
                              "\t 2. Uniform\n" +
                              "\t 3. Non-Uniform\n" +
                              "============================================================\n");

            while (true)
            {
                var bestOfRunFitness = new List<double>();

                Console.WriteLine("Choose Selection Strategy:");
                if (!int.TryParse(Console.ReadLine(), out var selectionStrategy))
                    continue;

                Console.WriteLine("Choose Recombination Strategy:");
                if (!int.TryParse(Console.ReadLine(), out var recombinationStrategy))
                    continue;

                Console.WriteLine("Choose Mutation Strategy:");
                if (!int.TryParse(Console.ReadLine(), out var mutationStrategy))
                    continue;

                //for (int i = 1; i < 4; i++)
                //{
                //    var selectionStrategy = i;
                //    for (int j = 1; j < 4; j++)
                //    {
                //        var recombinationStrategy = j;
                //        for (int k = 1; k < 4; k++)
                //        {
                //            var mutationStrategy = k;
                //            var bestOfRunFitness = new List<double>();

                for (int x = 0; x < 31; x++)
                {

                    var run = new GeneticAlgorithm(gens, N, Pc, Pm, lowerBound, upperbound,
                        selectionStrategy, recombinationStrategy, mutationStrategy);

                    GenerateGenerations(run);
                    bestOfRunFitness.Add(run.BestOfRun.Fitness);
                }

                Console.WriteLine("**************************************************************\n" +
                                  "Strategies Chosen:\n" +
                                  $"\tSelection - {((SelectionStrategy)selectionStrategy)}\n" +
                                  $"\tRecombination - {((RecombinationStrategy)recombinationStrategy)}\n" +
                                  $"\tMutation - {((MutationStrategy)mutationStrategy)}\n" +
                                  "After 30 Runs of 50 Generations each...\n\n" +
                                  $"\tMean of 30 BOR's: {bestOfRunFitness.Average()}, StDev : {StandDev(bestOfRunFitness)}\n" +
                                  $"\tBest of 30 Runs: {bestOfRunFitness.Min()}\n" +
                                  $"**************************************************************\n");
            }

        }

        private static void GenerateGenerations(GeneticAlgorithm run)
        {
            for (int i = 0; i < run.GenerationsPerRun; i++)
            {
                run.AddNewGeneration();

                //if (i % 10 == 0)
                //{
                //    Console.WriteLine($"*******Generation {i}*******");
                //    Console.WriteLine("Best Fitness:");
                //    Console.WriteLine(run.CandidateInfo(run.GetBestFitness()));
                //    Console.WriteLine("Worst Fitness:");
                //    Console.WriteLine(run.CandidateInfo(run.GetWorstFitness()));
                //    Console.WriteLine("Best of Run:");
                //    Console.WriteLine(run.CandidateInfo(run.BestOfRun));
                //    Console.WriteLine("-----------------------------------------------");
                //}
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

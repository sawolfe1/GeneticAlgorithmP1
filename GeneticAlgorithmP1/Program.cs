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

        private static Random _randomGen = new Random();
        private static GeneticAlgorithm Project1;

        static void Main(string[] args)
        {
            var N = 10;
            var Pc = 0.8;
            var Pm = 0.1;
            var lowerBound = -1;
            var upperbound = 5;

            Project1 = new GeneticAlgorithm(N, Pc, Pm, lowerBound, upperbound);


            // Create New Generation
            for (int i = 0; i < 90; i++)
            {
                Project1.AddNewGeneration();

                if (i % 10 == 0)
                {
                    Console.WriteLine($"*******Generation {i}*******\n");
                    Console.WriteLine("Best Fitness:\n");
                    Console.WriteLine(Project1.CandidateInfo(Project1.GetBestFitness()));
                    Console.WriteLine("Worst Fitness:\n");
                    Console.WriteLine(Project1.CandidateInfo(Project1.GetWorstFitness()));
                    Console.WriteLine("Best of Run:\n");
                    Console.WriteLine(Project1.CandidateInfo(Project1.BestOfRun));
                    Console.WriteLine("-----------------------------------------------");
                }
            }
            Console.ReadLine();

        }

    }
}

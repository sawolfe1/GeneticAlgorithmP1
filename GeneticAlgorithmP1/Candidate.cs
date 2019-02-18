using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;

namespace GeneticAlgorithmP1
{
    internal class Candidate
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double X3 { get; set; }

        public double Fitness => Math.Pow(X1, 2) + Math.Pow(X2, 2) + Math.Pow(X3, 2);


        public Candidate()
        {
            X1 = RandomGenerator.Random.Next(-1, 5) + RandomGenerator.Random.NextDouble();
            X2 = RandomGenerator.Random.Next(-1, 5) + RandomGenerator.Random.NextDouble();
            X3 = RandomGenerator.Random.Next(-1, 5) + RandomGenerator.Random.NextDouble();
        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmP1
{
    class GeneticAlgorithm
    {
        public int PopulationSize { get; set; }
        public double CrossoverRate { get; set; }
        public double MutationRate { get; set; }
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
        public Candidate BestOfRun { get; set; } = new Candidate();
        public List<Candidate> CurrentGeneration { get; set; }
        public List<List<Candidate>> Generations { get; set; } = new List<List<Candidate>>();
        public double AverageOfGenerationsFitness { get; set; }
        public double BestOfGenerationsFitness { get; set; }

        public GeneticAlgorithm(int populationSize, double crossoverRate, double mutationRate, int lowerBound, int upperBound)
        {
            var gen1 = new List<Candidate>();
            LowerBound = lowerBound;
            UpperBound = upperBound;
            PopulationSize = populationSize;
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            CurrentGeneration = gen1;
            Generations.Add(gen1);
            InitializePopulation();
        }

        public Candidate Select(List<Candidate> currentCandidates)
        {
            var potentialParent1 = BinaryTournament(currentCandidates);
            var potentialParent2 = BinaryTournament(currentCandidates);

            return potentialParent1.Fitness < potentialParent2.Fitness ? potentialParent1 : potentialParent2;
        }

        public Candidate BinaryTournament(List<Candidate> currentCandidates)
        {
            var rand = RandomGenerator.Random.Next(0, PopulationSize);
            var rand2 = RandomGenerator.Random.Next(0, PopulationSize);
            var parent = currentCandidates[rand].Fitness < currentCandidates[rand2].Fitness ?
                currentCandidates[rand] : currentCandidates[rand2];

            return parent;
        }

        public Candidate Crossover(Candidate parent1, Candidate parent2)
        {
            var crossoverPoint = RandomGenerator.Random.Next(1, 3);
            var child = new Candidate();

            switch (crossoverPoint)
            {
                case 1:
                    child.X1 = parent1.X1;
                    child.X2 = parent2.X2;
                    child.X3 = parent2.X3;
                    break;
                case 2:
                    child.X1 = parent1.X1;
                    child.X2 = parent1.X2;
                    child.X3 = parent2.X3;
                    break;
            }

            return child;
        }

        public Candidate Mutate(Candidate child)
        {

            if (RandomGenerator.Random.NextDouble() <= MutationRate)
                child.X1 += ((child.X1 + GetMutantFactor()) % UpperBound) + LowerBound;


            if (RandomGenerator.Random.NextDouble() <= MutationRate)
                child.X2 = ((child.X2 + GetMutantFactor()) % UpperBound) + LowerBound;


            if (RandomGenerator.Random.NextDouble() <= MutationRate)
                child.X3 += ((child.X3 + GetMutantFactor()) % UpperBound) + LowerBound;

            return child;
        }

        private double GetMutantFactor()
        {
            return RandomGenerator.Random.Next(-2, 2) * RandomGenerator.Random.NextDouble();
        }

        private void InitializePopulation()
        {

            for (int i = 0; i < PopulationSize; i++)
            {
                var candidate = new Candidate();
                CurrentGeneration.Add(candidate);


            }


        }

        public void AddNewGeneration()
        {
            var newGeneration = new List<Candidate>();


            for (int i = 0; i < PopulationSize; i++)
            {
                var candidate = Mutate(Crossover(Select(CurrentGeneration), Select(CurrentGeneration)));

                newGeneration.Add(candidate);


            }

            var min = newGeneration.Select(x => x.Fitness).Min();


            CurrentGeneration = newGeneration;
            Generations.Add(newGeneration);

            if (min < BestOfRun.Fitness)
                BestOfRun = GetBestFitness();
        }

        public Candidate GetBestFitness()
        {
            var min = CurrentGeneration.Min(x => x.Fitness);
            return CurrentGeneration.First(x => x.Fitness <= min);

        }

        public Candidate GetWorstFitness()
        {
            var max = CurrentGeneration.Max(x => x.Fitness);
            return CurrentGeneration.First(x => x.Fitness >= max);
        }

        public string CandidateInfo(Candidate candidate)
        {
            return $"X1 = {candidate.X1}, \n" +
                   $"X2 = {candidate.X2}, \n" +
                   $"X3 = {candidate.X3}, \n" +
                   $"Fitness = {candidate.Fitness}, \n" +
                   $"Average of Generation = {CurrentGeneration.Select(x => x.Fitness).Average()}\n";
        }

        public double GetAverageOfGenerationsFitness()
        {
            var aog = new List<double>();
            foreach (var generation in Generations)
            {
                aog.Add(generation.Select(x => x.Fitness).Average());
            }

            AverageOfGenerationsFitness = aog.Average();

            return AverageOfGenerationsFitness;
        }

        public double GetBestOfGenerationsFitness()
        {
            var bog = new List<double>();
            foreach (var generation in Generations)
            {

                bog.Add(generation.Min(x => x.Fitness));
            }

            BestOfGenerationsFitness = bog.Average();

            return BestOfGenerationsFitness;
        }

    }
}

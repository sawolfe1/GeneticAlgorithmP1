using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmP1
{
    public enum SelectionStrategy
    {
        BinaryTournament = 1,
        FitnessProportional = 2,
        Truncation = 3
    }

    public enum RecombinationStrategy
    {
        SinglePoint = 1,
        TwoPoint = 2,
        Uniform = 3
    }

    public enum MutationStrategy
    {
        Gaussian = 1,
        Uniform = 2,
        NonUniform = 3
    }
    class GeneticAlgorithm
    {
        public int GenerationsPerRun { get; set; }
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
        public SelectionStrategy SelectionStrategy { get; set; }
        public MutationStrategy MutationStrategy { get; set; }
        public RecombinationStrategy RecombinationStrategy { get; set; }

        public GeneticAlgorithm(int generationsPerRun, int populationSize, double crossoverRate, double mutationRate, int lowerBound, int upperBound, int selectionStrategy, int recombinationStrategy, int mutationStrategy)
        {
            var gen1 = new List<Candidate>();
            GenerationsPerRun = generationsPerRun;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            PopulationSize = populationSize;
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            CurrentGeneration = gen1;
            SelectionStrategy = (SelectionStrategy)selectionStrategy;
            RecombinationStrategy = (RecombinationStrategy)recombinationStrategy;
            MutationStrategy = (MutationStrategy)mutationStrategy;
            Generations.Add(gen1);
            InitializePopulation();
        }

        public Candidate Select(List<Candidate> currentCandidates)
        {
            var candidate = new Candidate();

            switch (SelectionStrategy)
            {
                case SelectionStrategy.BinaryTournament:
                    var potentialParent1 = BinaryTournament(currentCandidates);
                    var potentialParent2 = BinaryTournament(currentCandidates);
                    return potentialParent1.Fitness < potentialParent2.Fitness ? potentialParent1 : potentialParent2;
                case SelectionStrategy.FitnessProportional:
                    return FitnessProportional(currentCandidates);
                case SelectionStrategy.Truncation:
                    var random = RandomGenerator.Random.Next(0, 5);
                    var sortedCandidates = currentCandidates.OrderBy(x => x.Fitness).ToList();
                    return sortedCandidates[random];
                default:
                    return candidate;
            }

        }

        public Candidate BinaryTournament(List<Candidate> currentCandidates)
        {
            var rand = RandomGenerator.Random.Next(0, PopulationSize);
            var rand2 = RandomGenerator.Random.Next(0, PopulationSize);
            var parent = currentCandidates[rand].Fitness < currentCandidates[rand2].Fitness ?
                currentCandidates[rand] : currentCandidates[rand2];

            return parent;
        }

        public Candidate FitnessProportional(List<Candidate> currentCandidates)
        {
            var sortedCandidates = currentCandidates.OrderByDescending(x => x.Fitness).ToList();
            var sumOfFitness = sortedCandidates.Sum(x => x.Fitness);
            var previousValue = 0d;

            foreach (var sortedCandidate in sortedCandidates)
            {
                sortedCandidate.Proportionality = previousValue + ((1 / Math.Abs(sortedCandidate.Fitness)) / sumOfFitness);
                previousValue = sortedCandidate.Proportionality;
            }

            var random = RandomGenerator.Random.NextDouble();

            if (random < sortedCandidates[0].Proportionality)
                return sortedCandidates[0];

            var cand = sortedCandidates.Last(x => x.Proportionality < random);

            return cand;
        }

        public Candidate Crossover(Candidate parent1, Candidate parent2)
        {
            var child = new Candidate();

            switch (RecombinationStrategy)
            {
                case RecombinationStrategy.SinglePoint:
                    var crossoverPoint = RandomGenerator.Random.Next(1, 3);
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
                    break;

                case RecombinationStrategy.TwoPoint:
                    child.X1 = parent2.X1;
                    child.X2 = parent1.X2;
                    child.X3 = parent2.X3;
                    break;

                case RecombinationStrategy.Uniform:
                    child.X1 = RandomGenerator.Random.NextDouble() < 0.5 ? parent1.X1 : parent2.X1;
                    child.X2 = RandomGenerator.Random.NextDouble() < 0.5 ? parent1.X2 : parent2.X2;
                    child.X3 = RandomGenerator.Random.NextDouble() < 0.5 ? parent1.X3 : parent2.X3;
                    break;
            }

            return child;
        }

        public Candidate Mutate(Candidate child)
        {

            switch (MutationStrategy)
            {
                case MutationStrategy.Gaussian:
                    if (RandomGenerator.Random.NextDouble() <= MutationRate)
                        child.X1 += ((child.X1 + GetMutantFactor()) % UpperBound) + LowerBound;
                    if (RandomGenerator.Random.NextDouble() <= MutationRate)
                        child.X2 = ((child.X2 + GetMutantFactor()) % UpperBound) + LowerBound;
                    if (RandomGenerator.Random.NextDouble() <= MutationRate)
                        child.X3 += ((child.X3 + GetMutantFactor()) % UpperBound) + LowerBound;
                    break;
                case MutationStrategy.Uniform:
                    if (RandomGenerator.Random.NextDouble() <= MutationRate)
                        child.X1 = RandomGenerator.Random.Next(-1, 5) + RandomGenerator.Random.NextDouble();
                    if (RandomGenerator.Random.NextDouble() <= MutationRate)
                        child.X2 = RandomGenerator.Random.Next(-1, 5) + RandomGenerator.Random.NextDouble();
                    if (RandomGenerator.Random.NextDouble() <= MutationRate)
                        child.X3 = RandomGenerator.Random.Next(-1, 5) + RandomGenerator.Random.NextDouble();
                    break;
                case MutationStrategy.NonUniform:
                    var decrement = 1 - ((Generations.Count - 1) / (float)GenerationsPerRun);
                    if (RandomGenerator.Random.NextDouble() <= MutationRate * decrement)
                        child.X1 = RandomGenerator.Random.Next(-1, 5) + RandomGenerator.Random.NextDouble();
                    if (RandomGenerator.Random.NextDouble() <= MutationRate * decrement)
                        child.X2 = RandomGenerator.Random.Next(-1, 5) + RandomGenerator.Random.NextDouble();
                    if (RandomGenerator.Random.NextDouble() <= MutationRate * decrement)
                        child.X3 = RandomGenerator.Random.Next(-1, 5) + RandomGenerator.Random.NextDouble();
                    break;
            }
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

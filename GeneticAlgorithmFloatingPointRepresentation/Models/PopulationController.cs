using GeneticAlgorithmFloatingPointRepresentation.Crossovers;
using GeneticAlgorithmFloatingPointRepresentation.Extractors;
using GeneticAlgorithmFloatingPointRepresentation.Functions;
using GeneticAlgorithmFloatingPointRepresentation.Mutation;
using GeneticAlgorithmFloatingPointRepresentation.Selections;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmFloatingPointRepresentation.Models
{
    class PopulationController
    {
        public List<Entity> Population { get; private set; }
        private uint CurrentEpoch = 0;

        public IFunction ValueFunction;
        private IExtractor Extractor;
        private ICrossover CrossoverMethod;
        private IMutation MutationMethod;
        private ISelector SelectionMethod;

        private readonly double MinValue;
        private readonly double MaxValue;

        private readonly uint PopulationSize;

        public PopulationController() { }

        public PopulationController(IFunction valueFunction, IExtractor extractor, ICrossover crossoverMethod, IMutation mutationMethod, ISelector selectionMethod, double minValue, double maxValue, uint populationSize)
        {
            ValueFunction = valueFunction;
            Extractor = extractor;
            CrossoverMethod = crossoverMethod;
            MutationMethod = mutationMethod;
            SelectionMethod = selectionMethod;
            MinValue = minValue;
            MaxValue = maxValue;
            PopulationSize = populationSize;
        }

        public IEnumerable<EvolutionResult> StartEvolution(uint epochs)
        {
            InitPopulation();
            yield return GetEvolutionResult();

            for (int i = 0; i < epochs; i++)
            {
                yield return Evolve();
            }
        }

        private void InitPopulation()
        {
            Population = new();
            CurrentEpoch = 0;

            var generator = new EntityGenerator(ValueFunction, MinValue, MaxValue);

            for (int i = 0; i < PopulationSize; i++)
            {
                Population.Add(generator.GetNext());
            }
        }

        private EvolutionResult Evolve()
        {
            var extractedPopulation = Extractor.ExtractFrom(Population).ToArray();
            var processedPopulation = SelectionMethod.Select(Population);

            if (processedPopulation.Any())
            {
                var crossoveredPopulation = CrossoverMethod.Crossover(processedPopulation, Population.Count - extractedPopulation.Count());
                var mutatedPopulation = MutationMethod.Mutate(crossoveredPopulation);
                processedPopulation = mutatedPopulation;
            }
            else
            {
                processedPopulation = Population.ToArray();
            }

            Population.Clear();
            var finalPopulation = processedPopulation.ToList();
            finalPopulation.AddRange(extractedPopulation);
            Population = finalPopulation;
            CurrentEpoch += 1;

            return GetEvolutionResult();
        }

        private EvolutionResult GetEvolutionResult()
        {
            var average = Population.Select(e => e.ValueIndex).Average();
            var best = Population.OrderBy(e => e.ValueIndex).First();
            var bestIndex = Population.Select(e => e.ValueIndex).Min();

            double sumOfSquaresOfDifferences = Population.Select(e => (e.ValueIndex - average) * (e.ValueIndex - average)).Sum();
            double stdDev = Math.Sqrt(sumOfSquaresOfDifferences / Population.Count);

            var result = new EvolutionResult()
            {
                BestResultIndex = new DataPoint(CurrentEpoch, bestIndex),
                ResultsMeanIndex = new DataPoint(CurrentEpoch, average),
                StandardDeviation = new DataPoint(CurrentEpoch, stdDev),
                BestPointCoordinates = new DataPoint(best.ValueX, best.ValueY),
                EpochPoints = Population.Select(x => new Point(x.ValueX, x.ValueY)).ToArray()
            };

            return result;
        }
    }
}

using GeneticAlgorithm.Crossovers;
using GeneticAlgorithm.Extractors;
using GeneticAlgorithm.Functions;
using GeneticAlgorithm.Mutation;
using GeneticAlgorithm.Other;
using GeneticAlgorithm.Selections;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Models
{
    class PopulationController
    {
        public List<Entity> Population { get; private set; }
        private uint CurrentEpoch = 0;

        public IFunction ValueFunction;
        private IExtractor Extractor;
        private IOtherProcessor OtherProcessor;
        private ICrossover CrossoverMethod;
        private IMutation MutationMethod;
        private ISelector SelectionMethod;

        private readonly uint Precision;
        private readonly float MinValue;
        private readonly float MaxValue;

        private readonly uint PopulationSize;

        public PopulationController() { }

        public PopulationController(IFunction valueFunction, IExtractor extractor, IOtherProcessor processor, ICrossover crossoverMethod, IMutation mutationMethod, ISelector selectionMethod, uint precision, float minValue, float maxValue, uint populationSize)
        {
            ValueFunction = valueFunction;
            Extractor = extractor;
            OtherProcessor = processor;
            CrossoverMethod = crossoverMethod;
            MutationMethod = mutationMethod;
            SelectionMethod = selectionMethod;
            Precision = precision;
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

            var decoder = new ChromosomeDecoder(Precision, MinValue, MaxValue);
            var generator = new EntityGenerator(decoder, ValueFunction);

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
                var inversedPopulation = OtherProcessor.Process(mutatedPopulation);
                processedPopulation = inversedPopulation;
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

using GeneticAlgorithm.Crossovers;
using GeneticAlgorithm.Extractors;
using GeneticAlgorithm.Functions;
using GeneticAlgorithm.Mutation;
using GeneticAlgorithm.Other;
using GeneticAlgorithm.Selections;

namespace GeneticAlgorithm.Models
{
    class PopulationControllerBuilder
    {
        private IFunction ValueFunction;
        private IExtractor Extractor;
        private IOtherProcessor OtherProcessor;
        private ICrossover CrossoverMethod;
        private IMutation MutationMethod;
        private ISelector SelectionMethod;

        private uint Precision;
        private float MinValue;
        private float MaxValue;

        public PopulationControllerBuilder AddValueFunction(IFunction function)
        {
            ValueFunction = function;

            return this;
        }

        public PopulationControllerBuilder AddExtractor(IExtractor extractor)
        {
            Extractor = extractor;

            return this;
        }

        public PopulationControllerBuilder AddOtherProcessor(IOtherProcessor processor)
        {
            OtherProcessor = processor;

            return this;
        }

        public PopulationControllerBuilder AddCrossoverMethod(ICrossover crossover)
        {
            CrossoverMethod = crossover;

            return this;
        }

        public PopulationControllerBuilder AddMutationMethod(IMutation mutation)
        {
            MutationMethod = mutation;

            return this;
        }

        public PopulationControllerBuilder AddSelectionMethod(ISelector selector)
        {
            SelectionMethod = selector;

            return this;
        }

        public PopulationControllerBuilder AddPrecision(uint precision, float minValue, float maxValue)
        {
            Precision = precision;
            MaxValue = maxValue;
            MinValue = minValue;

            return this;
        }

        public PopulationController Build(uint populationSize)
        {
            return new PopulationController(ValueFunction, Extractor, OtherProcessor, CrossoverMethod, MutationMethod, SelectionMethod, 
                Precision, MinValue, MaxValue, populationSize);
        }
    }
}

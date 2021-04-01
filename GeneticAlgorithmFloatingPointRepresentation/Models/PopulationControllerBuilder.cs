using GeneticAlgorithmFloatingPointRepresentation.Crossovers;
using GeneticAlgorithmFloatingPointRepresentation.Extractors;
using GeneticAlgorithmFloatingPointRepresentation.Functions;
using GeneticAlgorithmFloatingPointRepresentation.Mutation;
using GeneticAlgorithmFloatingPointRepresentation.Selections;

namespace GeneticAlgorithmFloatingPointRepresentation.Models
{
    class PopulationControllerBuilder
    {
        private IFunction ValueFunction;
        private IExtractor Extractor;
        private ICrossover CrossoverMethod;
        private IMutation MutationMethod;
        private ISelector SelectionMethod;

        private double MinValue;
        private double MaxValue;

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

        public PopulationControllerBuilder AddPrecision(double minValue, double maxValue)
        {
            MaxValue = maxValue;
            MinValue = minValue;

            return this;
        }

        public PopulationController Build(uint populationSize)
        {
            return new PopulationController(ValueFunction, Extractor, CrossoverMethod, MutationMethod, SelectionMethod, 
                MinValue, MaxValue, populationSize);
        }
    }
}

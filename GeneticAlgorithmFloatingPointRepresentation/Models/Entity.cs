using GeneticAlgorithmFloatingPointRepresentation.Functions;

namespace GeneticAlgorithmFloatingPointRepresentation.Models
{
    public class Entity
    {
        public Chromosome ChromosomeX { get; private set; }
        public Chromosome ChromosomeY { get; private set; }
        public IFunction ValueFunction { get; set; }
        public static double MinMaxRange { get; } = 5;

        public Entity(IFunction valueFunction, double xValue, double yValue)
        {
            ChromosomeX = new Chromosome(xValue);
            ChromosomeY = new Chromosome(yValue);
            ValueFunction = valueFunction;
        }

        public Entity Reproduct(double xValue, double yValue)
        {
            return new Entity(ValueFunction, xValue, yValue);
        }

        public double ValueX {
            get
            {
                return ChromosomeX.Value;
            }
        }

        public double ValueY
        {
            get
            {
                return ChromosomeY.Value;
            }
        }

        public double ValueZ
        {
            get
            {
                return ValueFunction.Compute(ValueX, ValueY);
            }
        }

        public double ValueIndex
        {
            get
            {
                return ComputeValueIndex(ValueFunction, ValueZ);
            }
        }

        private static double ComputeValueIndex(IFunction valueFunction, double value)
        {
            var rangeToExtremum = valueFunction.Extremum.Value - value;
            rangeToExtremum *= rangeToExtremum < 0 ? -1 : 1;

            return rangeToExtremum;
        }
    }
}

namespace GeneticAlgorithmFloatingPointRepresentation.Models
{
    public class Chromosome
    {
        public double Value { get; init; }

        public Chromosome(double value)
        {
            Value = value;
        }

        public static Chromosome operator +(Chromosome a, Chromosome b)
        {
            return new Chromosome(a.Value + b.Value);
        }
    }
}

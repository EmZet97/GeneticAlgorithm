namespace GeneticAlgorithm.Functions
{
    public interface IFunction
    {
        public double Compute(double x, double y);
        public Extremum Extremum { get; }
    }

    public record Extremum(bool IsMaximum, float Value, float X, float Y);
}

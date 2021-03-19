namespace GeneticAlgorithm.Functions
{
    public class StyblinskiTangFunction : IFunction
    {
        public Extremum Extremum => new (false, -39.16599f * 2, -2.903534f, -2.903534f);

        public double Compute(double x, double y)
        {
            return 0.5f * (ComputeIteration(x) + ComputeIteration(y));
        }

        private static double ComputeIteration(double xi)
        {
            return (xi * xi * xi * xi) - (16 * xi * xi) + (5 * xi);
        }
    }
}

namespace GeneticAlgorithm.Functions
{
    public class StyblinskiTangFunction : IFunction
    {
        public float Compute(float x, float y)
        {
            return 0.5f * (ComputeIteration(x) + ComputeIteration(y));
        }

        private static float ComputeIteration(float xi)
        {
            return xi * xi * xi * xi - 16 * xi * xi + 5 * xi;
        }
    }
}

using OxyPlot;

namespace GeneticAlgorithm.Models
{
    public record EvolutionMinimalizedResult(int Epoch, double ResultsMean, double StandardDeviation, double BestResult);

    public class EvolutionResult
    {
        public DataPoint ResultsMeanIndex { get; set; } = new();
        public DataPoint BestResultIndex { get; set; } = new();
        public DataPoint StandardDeviation { get; set; } = new();
        public DataPoint BestPointCoordinates { get; set; } = new();
        public Point[] EpochPoints { get; set; }

        public EvolutionMinimalizedResult Minimalize()
        {
            return new EvolutionMinimalizedResult((int)ResultsMeanIndex.X, ResultsMeanIndex.Y, StandardDeviation.Y, BestResultIndex.Y);
        }
    }
}

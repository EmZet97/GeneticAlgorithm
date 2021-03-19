using OxyPlot;

namespace GeneticAlgorithm.Models
{
    public class EvolutionResult
    {
        public DataPoint Indexes { get; set; } = new();
        public DataPoint Best { get; set; } = new();
        public Point[] Points { get; set; }
    }
}

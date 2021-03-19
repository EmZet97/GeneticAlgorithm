using GeneticAlgorithm.Crossovers;
using GeneticAlgorithm.Functions;
using GeneticAlgorithm.Models;
using GeneticAlgorithm.Mutation;
using GeneticAlgorithm.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OxyPlot;
using System.Threading;
using OxyPlot.Series;

namespace GeneticAlgorithm
{
    public partial class MainWindow : Window
    {
        private List<EvolutionResult> EvolutionResults = new();
        private bool process = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EpochSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EpochNumber.Content = (int)EpochSlider.Value;


            var epochPoints = EvolutionResults.Select(r => r.Points);
            Points.ItemsSource = epochPoints.ToList()[(int)EpochSlider.Value].Select(p => new ScatterPoint(p.X, p.Y, size: 2, value: 100));
            Points.MarkerType = MarkerType.Circle;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            StartButton.Content = "Evloving";
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            Chart.IsEnabled = false;
            EpochSlider.IsEnabled = false;

            var t = new Thread(new ThreadStart(Start));
            t.Start();
        }

        private void Start()
        {
            var controllerBuilder = new PopulationControllerBuilder();
            process = true;

            var controller = controllerBuilder
                .AddSelectionMethod(new RouletteSelector(0.1f))
                .AddCrossoverMethod(new OnePointCrossover(0.8f))
                .AddMutationMethod(new BorderMutation(0.0f))
                .AddValueFunction(new StyblinskiTangFunction())
                .AddPrecision(2048, -5f, 5f)
                .Build(100);

            EvolutionResults.Clear();
            int i = 0;
            foreach (var result in controller.StartEvolution(10000))
            {
                if (!process)
                {
                    FinishWork(controller);
                    return;
                }

                EvolutionResults.Add(result);

                if(i % 100 == 0)
                {
                    Dispatcher.Invoke(() => {
                        Line1.ItemsSource = EvolutionResults.Select(r => r.Indexes);
                        Line2.ItemsSource = EvolutionResults.Select(r => r.Best);

                        var epochPoints = EvolutionResults.Select(r => r.Points);
                        Points.ItemsSource = epochPoints.ToList().Last().Select(p => new ScatterPoint(p.X, p.Y, size: 2, value: 100));
                        Points.MarkerType = MarkerType.Circle;

                        var extremum = controller.ValueFunction.Extremum;
                        FinalPoint.ItemsSource = new[] { new ScatterPoint(extremum.X, extremum.Y, size: 10, value: 100) };
                        FinalPoint.MarkerType = MarkerType.Circle;
                        FinalPoint.Color = Colors.Blue;
                    });
                }

                i++;
            }

            FinishWork(controller);
        }

        private void FinishWork(PopulationController controller)
        {
            Dispatcher.Invoke(() => {
                Line1.ItemsSource = EvolutionResults.Select(r => r.Indexes);
                Line2.ItemsSource = EvolutionResults.Select(r => r.Best);                

                var epochPoints = controller.ValueFunction.Extremum;
                FinalPoint.ItemsSource = new[] { new ScatterPoint(epochPoints.X, epochPoints.Y, size: 10, value: 100) };

                EpochSlider.IsEnabled = true;
                EpochSlider.Maximum = EvolutionResults.Count - 1;

                StopButton.IsEnabled = false;
                StartButton.IsEnabled = true;
                StartButton.Content = "Start";
                Chart.IsEnabled = true;
                Chart2.IsEnabled = true;
            });
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            process = false;
        }
    }
}

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
using System.Text.RegularExpressions;
using GeneticAlgorithm.Helpers;

namespace GeneticAlgorithm
{
    public partial class MainWindow : Window
    {
        private List<EvolutionResult> EvolutionResults = new();
        private bool process = false;
        private readonly PopulationControllerBuilder controllerBuilder = new ();
        public MainWindow()
        {
            InitializeComponent();

            Init();

            SelectorDropDown.ItemsSource = new[] { "Rank Selection", "Roulette Selection", "Turnament Selection" };
            SelectorDropDown.SelectedIndex = 0;


        }

        private void Init()
        {
            controllerBuilder
                .AddSelectionMethod(new RouletteSelector(0.3f))
                .AddCrossoverMethod(new OnePointCrossover(0.8f))
                .AddMutationMethod(new BorderMutation(0.2f))
                .AddValueFunction(new StyblinskiTangFunction())
                .AddPrecision(2048, -5f, 5f);
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
            process = true;

            var controller = controllerBuilder
                .AddCrossoverMethod(new OnePointCrossover(0.8f))
                .AddMutationMethod(new BorderMutation(0.5f))
                .AddValueFunction(new StyblinskiTangFunction())
                .AddPrecision(8192, -5f, 5f)
                .Build(200);

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

                if(i % 1 == 0)
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

        private void SelectorDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetSelectorMethod();
        }

        private void SelectorProbabilityTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SelectorProbabilityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var correctRange = PercentRangeParser.TryParse(SelectorProbabilityTextBox.Text, out int percent);

            if (!correctRange)
            {
                SelectorProbabilityTextBox.Text = percent.ToString();
            }

            SetSelectorMethod();
        }

        private void SetSelectorMethod()
        {
            _ = PercentRangeParser.TryParse(SelectorProbabilityTextBox.Text, out int percent);
            float probability = .01f * percent;
            switch (SelectorDropDown.SelectedIndex)
            {
                case 0:
                    controllerBuilder.AddSelectionMethod(new RankSelector(probability));
                    break;
                case 1:
                    controllerBuilder.AddSelectionMethod(new RouletteSelector(probability));
                    break;
                default:
                    controllerBuilder.AddSelectionMethod(new TurnamentSelector(probability));
                    break;
            }
        }
    }
}

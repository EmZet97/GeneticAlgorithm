using GeneticAlgorithm.Crossovers;
using GeneticAlgorithm.Functions;
using GeneticAlgorithm.Models;
using GeneticAlgorithm.Mutation;
using GeneticAlgorithm.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using System.Threading;
using OxyPlot.Series;
using System.Text.RegularExpressions;
using GeneticAlgorithm.Helpers;
using GeneticAlgorithm.Extractors;
using GeneticAlgorithm.Other;
using System.IO;

namespace GeneticAlgorithm
{
    public partial class MainWindow : Window
    {
        private List<EvolutionResult> EvolutionResults = new();
        private bool process = false;

        public MainWindow()
        {
            InitializeComponent();

            InitUI();
        }

        private void InitUI()
        {
            ChromosomeBitLengthTextBox.Text = 12.ToString();

            PopulationSizeTextBox.Text = 100.ToString();

            SelectorDropDown.ItemsSource = new[] { "Rank Selection", "Roulette Selection", "Turnament Selection" };
            SelectorDropDown.SelectedIndex = 0;
            SelectorProbabilityTextBox.Text = 40.ToString();

            CrossoverDropDown.ItemsSource = new[] { "One point Crossover", "Two points Crossover", "Three points Crossover", "Homogeneous Crossover" };
            CrossoverDropDown.SelectedIndex = 0;
            CrossoverProbabilityTextBox.Text = 30.ToString();

            MutationDropDown.ItemsSource = new[] { "One point Mutation", "Two points Mutation", "Border Mutation" };
            MutationDropDown.SelectedIndex = 0;
            MutationProbabilityTextBox.Text = 5.ToString();

            InversionProbabilityTextBox.Text = 5.ToString();

            ElitistStrategyProbabilityTextBox.Text = 1.ToString();

            NumberOfEpochsTextBox.Text = 5000.ToString();
        }

        private void CountTime()
        {
            var time = DateTime.Now;
            while (process)
            {
                Thread.Sleep(100);
                Dispatcher.Invoke(() =>
                {
                    TimeCounter_Label.Content = $"{(DateTime.Now - time).Seconds}:{(int)((DateTime.Now - time).Milliseconds%1000)/100} seconds";
                });
            }            
        }

        private void Start()
        {
            PopulationController controller = new();
            int epochsNumber = 0;

            Dispatcher.Invoke(() =>
            {
                controller = GatherUIData();
                _ = ValueParser.TryIntParse(NumberOfEpochsTextBox.Text, out epochsNumber);
            });

            EvolutionResults.Clear();
            foreach (var result in controller.StartEvolution((uint)epochsNumber))
            {
                if (!process)
                {
                    Finish(controller);
                    return;
                }

                EvolutionResults.Add(result);

                if (DateTime.Now.Millisecond%50 == 0)
                {
                    Dispatcher.Invoke(() => {
                        if (ProgressUpdate_CheckBox.IsChecked is true)
                            DrawGraphs(controller);
                        UpdateCurrentStats(controller);
                    });
                }
            }

            Finish(controller);
        }

        private void Finish(PopulationController controller)
        {
            Dispatcher.Invoke(() =>
            {
                EnableUI(true);
                DrawGraphs(controller);
                UpdateCurrentStats(controller);
            });
            process = false;
            SaveToFile();
        }

        private void UpdateCurrentStats(PopulationController controller)
        {
            var XY = EvolutionResults.Select(r => r.BestPointCoordinates);
            BestValue_Label.Content = $"F({XY.Last().X.ToString("0.00")}, {XY.Last().Y.ToString("0.00")}) = {controller.ValueFunction.Compute(XY.Last().X, XY.Last().Y).ToString("0.00")}";
            CurrentEpoch_Label.Content = EvolutionResults.Select(r => r.BestResultIndex).Last().X;
        }

        private void DrawGraphs(PopulationController controller)
        {
            Line1.ItemsSource = EvolutionResults.Select(r => r.ResultsMeanIndex);
            Line2.ItemsSource = EvolutionResults.Select(r => r.BestResultIndex);
            Line3.ItemsSource = EvolutionResults.Select(r => r.StandardDeviation);

            var extremumPoint = controller.ValueFunction.Extremum;
            FinalPoint.ItemsSource = new[] { new ScatterPoint(extremumPoint.X, extremumPoint.Y, size: 10, value: 100) };
            EpochSlider.Maximum = EvolutionResults.Count - 1;
            EpochSlider.Value = EvolutionResults.Count - 1;

            EpochNumber.Content = (int)EpochSlider.Value;

            var epochPoints = EvolutionResults.Select(r => r.EpochPoints);
            EpochPoints.ItemsSource = epochPoints.ToList()[(int)EpochSlider.Value].Select(p => new ScatterPoint(p.X, p.Y, size: 2, value: 100));
            EpochPoints.MarkerType = MarkerType.Circle;
        }

        private void SaveToFile()
        {
            var simplifedResults = EvolutionResults.Select(r => r.Minimalize());
            
            DirectoryInfo di = Directory.CreateDirectory("Results");

            CsvWriterHelper.WriteToFile($"Results\\Result({DateTime.Now.ToFileTime()}).csv", simplifedResults);
        }

        private PopulationController GatherUIData()
        {
            _ = ValueParser.TryIntParse(ChromosomeBitLengthTextBox.Text, out var chromosomeBitLength);
            var chromosomeBits = (uint)Math.Pow(2, chromosomeBitLength);
            _ = ValueParser.TryIntParse(PopulationSizeTextBox.Text, out var populationSize);

            _ = ValueParser.TryIntParse(SelectorProbabilityTextBox.Text, out var selectionPercent);
            var selectionMethodIndex = SelectorDropDown.SelectedIndex;
            var selectionProbability = (float)selectionPercent / 100;
            var selectionMethod = new List<ISelector> { new RankSelector(selectionProbability), new RouletteSelector(selectionProbability), new TurnamentSelector(selectionProbability) }[selectionMethodIndex];
            
            _ = ValueParser.TryIntParse(MutationProbabilityTextBox.Text, out var mutationPercent);
            var mutationMethodIndex = MutationDropDown.SelectedIndex;
            var mutationProbability = (float)mutationPercent / 100;
            var mutationMethod = new List<IMutation> { new OnePointMutation(mutationProbability), new TwoPointsMutation(mutationProbability), new BorderMutation(mutationProbability) }[mutationMethodIndex];


            _ = ValueParser.TryIntParse(CrossoverProbabilityTextBox.Text, out var crossoverPercent);
            var crossoverMethodIndex = CrossoverDropDown.SelectedIndex;
            var crossoverProbability = (float)crossoverPercent / 100;
            var crossoverMethod = new List<ICrossover> { new OnePointCrossover(crossoverProbability), new TwoPointsCrossover(crossoverProbability), new ThreePointsCrossover(crossoverProbability), new HomogeneousCrossover(crossoverProbability) }[crossoverMethodIndex];

            _ = ValueParser.TryIntParse(InversionProbabilityTextBox.Text, out var inversionPercent);
            var inversionProbability = (float)inversionPercent / 100;
            _ = ValueParser.TryIntParse(ElitistStrategyProbabilityTextBox.Text, out var extractionPercent);
            var extractorPercent = (float)extractionPercent / 100;
            //_ = ValueParser.TryIntParse(NumberOfEpochsTextBox.Text, out var numberOfEpochs);

            var controller = new PopulationControllerBuilder()
                .AddValueFunction(new StyblinskiTangFunction())
                .AddExtractor(new ElitistStrategyExtractor(extractorPercent))
                .AddOtherProcessor(new InversionProcessor(inversionProbability))
                .AddPrecision(chromosomeBits, -5f, 5f)
                .AddSelectionMethod(selectionMethod)
                .AddCrossoverMethod(crossoverMethod)
                .AddMutationMethod(mutationMethod)
                .Build((uint)populationSize);

            return controller;                
        }

        private void EnableUI(bool enable)
        {
            ChromosomeBitLengthTextBox.IsEnabled = enable;

            PopulationSizeTextBox.IsEnabled = enable;

            SelectorDropDown.IsEnabled = enable;
            SelectorProbabilityTextBox.IsEnabled = enable;

            CrossoverDropDown.IsEnabled = enable;
            CrossoverProbabilityTextBox.IsEnabled = enable;

            MutationDropDown.IsEnabled = enable;
            MutationProbabilityTextBox.IsEnabled = enable;

            InversionProbabilityTextBox.IsEnabled = enable;

            ElitistStrategyProbabilityTextBox.IsEnabled = enable;

            NumberOfEpochsTextBox.IsEnabled = enable;

            StartButton.IsEnabled = enable;

            StartButton.Content = enable ? "Start" : "Evloving";
            StartButton.IsEnabled = enable;

            StopButton.IsEnabled = !enable;

            Chart.IsEnabled = enable;

            EpochSlider.IsEnabled = enable;

            ProgressUpdate_CheckBox.IsEnabled = enable;
        }

        private void EpochSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EpochNumber.Content = (int)EpochSlider.Value;

            var epochPoints = EvolutionResults.Select(r => r.EpochPoints);
            EpochPoints.ItemsSource = epochPoints.ToList()[(int)EpochSlider.Value].Select(p => new ScatterPoint(p.X, p.Y, size: 2, value: 100));
            EpochPoints.MarkerType = MarkerType.Circle;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            EnableUI(false);
            process = true;

            var epochsThread = new Thread(new ThreadStart(Start));
            epochsThread.Start();

            var timerThread = new Thread(new ThreadStart(CountTime));
            timerThread.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            process = false;
        }

        private void SelectorDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SelectorProbabilityTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SelectorProbabilityTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var correctValue = ValueParser.TryIntParse(SelectorProbabilityTextBox.Text, out int percent);
            _ = ValueParser.TryIntParse(ElitistStrategyProbabilityTextBox.Text, out int percentOfES);

            if (!correctValue || percent != Math.Clamp(percent, 1, 100))
            {
                percent = Math.Clamp(percent, 1, 100);
                SelectorProbabilityTextBox.Text = percent.ToString();
            }

            if(percentOfES != Math.Clamp(percentOfES, 0, 100 - percent))
            {
                percentOfES = Math.Clamp(percentOfES, 0, 100 - percent);
                ElitistStrategyProbabilityTextBox.Text = percentOfES.ToString();
            }
        }

        private void CrossoverDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CrossoverProbabilityTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var correctValue = ValueParser.TryIntParse(CrossoverProbabilityTextBox.Text, out int percent);

            if (!correctValue || percent != Math.Clamp(percent, 0, 100))
            {
                percent = Math.Clamp(percent, 0, 100);
                CrossoverProbabilityTextBox.Text = percent.ToString();
            }
        }

        private void MutationDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MutationProbabilityTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var correctValue = ValueParser.TryIntParse(MutationProbabilityTextBox.Text, out int percent);

            if (!correctValue || percent != Math.Clamp(percent, 0, 100))
            {
                percent = Math.Clamp(percent, 0, 100);
                MutationProbabilityTextBox.Text = percent.ToString();
            }
        }

        private void ChromosomeBitLengthTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var correctValue = ValueParser.TryIntParse(ChromosomeBitLengthTextBox.Text, out int percent);

            if (!correctValue || percent != Math.Clamp(percent, 2, 16))
            {
                percent = Math.Clamp(percent, 2, 16);
                ChromosomeBitLengthTextBox.Text = percent.ToString();
            }
        }

        private void PopulationSizeTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var correctValue = ValueParser.TryIntParse(PopulationSizeTextBox.Text, out int percent);

            if (!correctValue || percent != Math.Clamp(percent, 1, 10000))
            {
                percent = Math.Clamp(percent, 1, 10000);
                PopulationSizeTextBox.Text = percent.ToString();
            }
        }

        private void InversionProbabilityTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var correctValue = ValueParser.TryIntParse(InversionProbabilityTextBox.Text, out int percent);

            if (!correctValue || percent != Math.Clamp(percent, 0, 100))
            {
                percent = Math.Clamp(percent, 0, 100);
                InversionProbabilityTextBox.Text = percent.ToString();
            }
        }

        private void ElitistStrategyProbabilityTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var correctValue = ValueParser.TryIntParse(ElitistStrategyProbabilityTextBox.Text, out int percent);
            _ = ValueParser.TryIntParse(SelectorProbabilityTextBox.Text, out int percentOfSelection);

            if (!correctValue || percent != Math.Clamp(percent, 0, 100 - percentOfSelection) || percent != Math.Clamp(percent, 0, 10))
            {
                percent = Math.Clamp(percent, 0, 100 - percentOfSelection);
                percent = Math.Clamp(percent, 0, 10);
                ElitistStrategyProbabilityTextBox.Text = percent.ToString();
            }
        }

        private void NumberOfEpochsTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var correctValue = ValueParser.TryIntParse(NumberOfEpochsTextBox.Text, out int percent);

            if (!correctValue || percent != Math.Clamp(percent, 1, 100000))
            {
                percent = Math.Clamp(percent, 1, 100000);
                NumberOfEpochsTextBox.Text = percent.ToString();
            }
        }
    }
}

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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeneticAlgorithm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var controllerBuilder = new PopulationControllerBuilder();

            var controller = controllerBuilder
                .AddSelectionMethod(new RouletteSelector(0.3f))
                .AddCrossoverMethod(new OnePointCrossover(0.8f))
                .AddMutationMethod(new BorderMutation(0.2f))
                .AddValueFunction(new StyblinskiTangFunction())
                .AddPrecision(1024, -5f, 5f)
                .Build(100);

            controller.StartEvolution(100);
        }
    }
}

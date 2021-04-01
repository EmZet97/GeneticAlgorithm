using CsvHelper;
using CsvHelper.Configuration;
using GeneticAlgorithmFloatingPointRepresentation.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GeneticAlgorithmFloatingPointRepresentation.Helpers
{
    public class CsvWriterHelper
    {
        public static void WriteToFile(string fileName, IEnumerable<EvolutionMinimalizedResult> data)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                Delimiter = "; "
            };

            using var writer = new StreamWriter(fileName);
            using var csvWriter = new CsvWriter(writer, config);

            csvWriter.WriteField("Epoch");
            csvWriter.WriteField("Result Mean");
            csvWriter.WriteField("Standard Deviation");
            csvWriter.WriteField("Best Result");
            csvWriter.NextRecord();

            foreach (var record in data)
            {
                csvWriter.WriteField(record.Epoch);
                csvWriter.WriteField(record.ResultsMean);
                csvWriter.WriteField(record.StandardDeviation);
                csvWriter.WriteField(record.BestResult);
                csvWriter.NextRecord();
            }

            writer.Flush();
        }
    }
}

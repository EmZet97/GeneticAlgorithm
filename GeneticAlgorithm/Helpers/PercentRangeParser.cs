using System;

namespace GeneticAlgorithm.Helpers
{
    public class PercentRangeParser
    {
        public static bool TryParse(string valueText, out int percent)
        {
            try
            {
                int value = int.Parse(valueText);
                if (value < 1)
                    value = 1;
                else if (value > 100)
                    value = 100;
                else
                {
                    percent = value;
                    return true;
                }

                percent = value;
                return false;
            }
            catch (FormatException _)
            {
                percent = 1;
                return false;
            }
        }
    }
}

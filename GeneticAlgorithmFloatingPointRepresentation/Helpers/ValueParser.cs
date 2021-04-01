using System;

namespace GeneticAlgorithmFloatingPointRepresentation.Helpers
{
    public class ValueParser
    {
        public static bool TryIntParse(string valueText, out int number)
        {
            try
            {
                number = int.Parse(valueText);

                return true;
            }
            catch (FormatException _)
            {
                number = 0;
                return false;
            }
        }
    }
}

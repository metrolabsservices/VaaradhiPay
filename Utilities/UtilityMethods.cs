namespace VaaradhiPay.Utilities
{
    public class UtilityMethods
    {
        public static string TruncateDecimalString(string numberString, int decimalPlaces)
        {
            int decimalIndex = numberString.IndexOf('.');

            if (decimalIndex == -1 || decimalPlaces <= 0)
                return numberString;

            int endIndex = Math.Min(numberString.Length, decimalIndex + decimalPlaces + 1);
            return numberString.Substring(0, endIndex);
        }
    }

    public static class CurrencySets
    {
        public static readonly Dictionary<string, string> PaperCurrencySet = new Dictionary<string, string>
         {
             { "INR", "INR" },
             { "USD", "USD" },
             { "AED", "AED" }
         };

        public static readonly Dictionary<string, string> DigitalCurrencySet = new Dictionary<string, string>
         {
             { "USDT", "USDT" },
             { "NAFA", "NAFA" }
         };
    }
}

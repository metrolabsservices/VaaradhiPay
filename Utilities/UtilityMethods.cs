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
}

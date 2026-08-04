using System.Security.Cryptography;

namespace Ice_Cream_Parlour_Eproject.Helpers
{
    public static class BarcodeHelper
    {
        private static readonly Random _random = new Random();

        public static string Generate12DigitBarcode()
        {
            // 12-digit string: first digit 1-9, rest 0-9
            char[] digits = new char[12];
            digits[0] = (char)('0' + _random.Next(1, 10)); // first digit non-zero
            for (int i = 1; i < 12; i++)
            {
                digits[i] = (char)('0' + _random.Next(0, 10));
            }
            return new string(digits);
        }

        // Async version with uniqueness check
        public static async Task<string> GenerateUniqueBarcodeAsync(Func<string, Task<bool>> existsCheck)
        {
            string barcode;
            bool exists;
            int attempts = 0;
            const int maxAttempts = 100;

            do
            {
                barcode = Generate12DigitBarcode();
                exists = await existsCheck(barcode);
                attempts++;
            } while (exists && attempts < maxAttempts);

            if (attempts >= maxAttempts)
                throw new InvalidOperationException("Unable to generate a unique barcode after 100 attempts.");

            return barcode;
        }
    }
}
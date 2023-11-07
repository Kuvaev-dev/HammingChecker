public static class HammingEncoder
{
    // Метод кодування текстового повідомлення методом Хемінга.
    public static string Encode(string text)
    {
        // Обчислення необхідної кількості додаткових бітів (r).
        int m = text.Length;
        int r = 0;
        while (m + r + 1 > Math.Pow(2, r))
        {
            r++;
        }

        // Створення масиву для закодованого повідомлення.
        char[] encodedMessage = new char[m + r];
        int j = 0;

        // Заповнення закодованого повідомлення, додавання бітів з тексту та обчислення бітів парності.
        for (int i = 1; i <= m + r; i++)
        {
            if (IsPowerOfTwo(i))
            {
                encodedMessage[i - 1] = '0';
            }
            else
            {
                encodedMessage[i - 1] = text[j++];
            }
        }

        // Виведення закодованого повідомлення в двійковій формі на консоль.
        Console.WriteLine("Закодоване повідомлення в двійковій формі:");
        for (int i = 0; i < encodedMessage.Length; i++)
        {
            Console.Write(encodedMessage[i] + " ");
        }
        Console.WriteLine();

        // Повернення закодованого повідомлення.
        return new string(encodedMessage);
    }

    // Метод декодування закодованого повідомлення методом Хемінга.
    public static string Decode(string encodedText)
    {
        // Обчислення кількості додаткових бітів (r).
        int r = 0;
        while (Math.Pow(2, r) < encodedText.Length)
        {
            r++;
        }

        // Обчислення кількості даних бітів (m).
        int m = encodedText.Length - r;
        char[] decodedMessage = new char[m];
        int j = 0;

        // Видалення бітів парності та відновлення даних бітів.
        for (int i = 1; i <= m + r; i++)
        {
            if (!IsPowerOfTwo(i))
            {
                decodedMessage[j++] = encodedText[i - 1];
            }
        }

        // Виведення розкодованого повідомлення в двійковій формі на консоль.
        Console.WriteLine("Розкодоване повідомлення в двійковій формі:");
        for (int i = 0; i < decodedMessage.Length; i++)
        {
            Console.Write(decodedMessage[i] + " ");
        }
        Console.WriteLine();

        // Повернення розкодованого повідомлення.
        return new string(decodedMessage);
    }

    // Метод введення помилки в закодований байтовий масив.
    public static void IntroduceError(byte[] encodedBytes, int errorPosition)
    {
        // Обчислення індексу байта та біта в байті для введення помилки.
        int byteIndex = errorPosition / 8;
        int bitIndex = errorPosition % 8;

        // Введення помилки в вказаному байті та біті.
        encodedBytes[byteIndex] ^= (byte)(1 << (7 - bitIndex));
    }

    // Метод виправлення помилок у закодованому байтовому масиві.
    public static void CorrectError(byte[] encodedBytes)
    {
        // Обчислення кількості додаткових бітів (r).
        int r = 0;
        while (Math.Pow(2, r) < encodedBytes.Length * 8)
        {
            r++;
        }

        // Виправлення помилок у закодованому масиві шляхом зміни бітів.
        for (int i = 0; i < r; i++)
        {
            int parityPosition = (int)Math.Pow(2, i);
            int parityBit = 0;

            // Обчислення біту парності для кожного позиції.
            for (int k = parityPosition - 1; k < encodedBytes.Length * 8; k += 2 * parityPosition)
            {
                for (int l = 0; l < parityPosition && k + l < encodedBytes.Length * 8; l++)
                {
                    int byteIndex = (k + l) / 8;
                    int bitIndex = (k + l) % 8;
                    parityBit ^= (encodedBytes[byteIndex] >> (7 - bitIndex)) & 1;
                }
            }

            int errorPosition = 0;
            for (int k = parityPosition; k < encodedBytes.Length * 8; k += parityPosition * 2)
            {
                errorPosition += k;
            }

            // Якщо біт парності не дорівнює 0, виправити помилку.
            if (parityBit != 0)
            {
                int byteIndex = (errorPosition - 1) / 8;
                int bitIndex = (errorPosition - 1) % 8;
                encodedBytes[byteIndex] ^= (byte)(1 << (7 - bitIndex));
            }
        }
    }

    // Допоміжний метод для перевірки, чи є число степенем двійки.
    private static bool IsPowerOfTwo(int number)
    {
        return (number & (number - 1)) == 0 && number > 0;
    }
}

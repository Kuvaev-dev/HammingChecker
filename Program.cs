using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Програма для кодування, декодування та корекції помилок за допомогою коду Хемінга");

        while (true)
        {
            Console.WriteLine("1. Виконати кодування методом Хемінга та зберегти у файл .bin");
            Console.WriteLine("2. Виконати декодування методом Хемінга та зберегти у файл .txt");
            Console.WriteLine("3. Виконати корекцію помилок у коді Хемінга");
            Console.WriteLine("4. Вийти");
            Console.Write("Введіть ваш вибір: ");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Некоректний ввід. Будь ласка, введіть правильний вибір.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    PerformHammingEncoding();
                    break;
                case 2:
                    PerformHammingDecoding();
                    break;
                case 3:
                    PerformErrorCorrection();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Некоректний вибір. Будь ласка, введіть правильний вибір.");
                    break;
            }
        }
    }

    static void DisplayBinaryData(byte[] data)
    {
        Console.WriteLine("Двійкові дані:");
        for (int i = 0; i < data.Length; i++)
        {
            Console.Write(Convert.ToString(data[i], 2).PadLeft(8, '0') + " ");
            if ((i + 1) % 8 == 0)
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine();
    }

    // Метод для виконання кодування методом Хемінга.
    static void PerformHammingEncoding()
    {
        Console.Write("Введіть шлях до текстового файлу для кодування: ");
        string inputPath = Console.ReadLine();
        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Файл не знайдено. Будь ласка, введіть правильний шлях.");
            return;
        }

        Console.Write("Введіть шлях для збереження файлу .bin: ");
        string outputPath = Console.ReadLine();

        string text = File.ReadAllText(inputPath, Encoding.UTF8);
        string encodedText = HammingEncoder.Encode(text);
        byte[] encodedBytes = Encoding.UTF8.GetBytes(encodedText);

        using (FileStream fs = new FileStream(outputPath, FileMode.Create))
        {
            fs.Write(encodedBytes, 0, encodedBytes.Length);
        }

        Console.WriteLine("Закодована послідовність:");
        DisplayBinaryData(encodedBytes);
        Console.WriteLine("Кодування методом Хемінга та збереження у файл .bin завершено.");
    }

    // Метод для виконання декодування методом Хемінга.
    static void PerformHammingDecoding()
    {
        Console.Write("Введіть шлях до файлу .bin для декодування: ");
        string inputPath = Console.ReadLine();
        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Файл не знайдено. Будь ласка, введіть правильний шлях.");
            return;
        }

        Console.Write("Введіть шлях для збереження файлу .txt: ");
        string outputPath = Console.ReadLine();

        byte[] encodedBytes = File.ReadAllBytes(inputPath);
        string encodedText = Encoding.UTF8.GetString(encodedBytes);
        string decodedText = HammingEncoder.Decode(encodedText);

        File.WriteAllText(outputPath, decodedText, Encoding.UTF8);

        Console.WriteLine("Закодована послідовність:");
        DisplayBinaryData(encodedBytes);
        Console.WriteLine("Декодована послідовність: " + decodedText);
        Console.WriteLine("Декодування методом Хемінга та збереження у файл .txt завершено.");
    }

    // Метод для виконання корекції помилок в закодованому масиві.
    static void PerformErrorCorrection()
    {
        Console.Write("Введіть шлях до файлу з кодом Хемінга: ");
        string inputPath = Console.ReadLine();
        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Файл не знайдено. Будь ласка, введіть правильний шлях.");
            return;
        }

        byte[] encodedBytes = File.ReadAllBytes(inputPath);
        Console.WriteLine("Закодована послідовність:");
        DisplayBinaryData(encodedBytes);

        Console.Write("Введіть позицію помилки: ");
        if (!int.TryParse(Console.ReadLine(), out int errorPosition) || errorPosition < 1 || errorPosition > encodedBytes.Length * 8)
        {
            Console.WriteLine("Некоректна позиція помилки. Будь ласка, введіть правильну позицію.");
            return;
        }

        HammingEncoder.IntroduceError(encodedBytes, errorPosition - 1);
        Console.WriteLine("Послідовність з помилкою:");
        DisplayBinaryData(encodedBytes);

        HammingEncoder.CorrectError(encodedBytes);
        Console.WriteLine("Виправлена послідовність:");
        DisplayBinaryData(encodedBytes);

        Console.Write("Введіть шлях для збереження файлу .bin: ");
        string outputPath = Console.ReadLine();

        using (FileStream fs = new FileStream(outputPath, FileMode.Create))
        {
            fs.Write(encodedBytes, 0, encodedBytes.Length);
        }

        Console.WriteLine("Корекція помилок та збереження у файл .bin завершено.");
    }
}
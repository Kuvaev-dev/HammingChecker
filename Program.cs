namespace HammingChecker
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Ласкаво просимо до програми кодування та декодування Хемінга!");

            while (true)
            {
                Console.WriteLine("\nОберіть операцію:");
                Console.WriteLine("1. Кодувати з текстового файлу");
                Console.WriteLine("2. Декодувати з текстового файлу");
                Console.WriteLine("3. Ввести помилку та закодувати з текстового файлу");
                Console.WriteLine("4. Вийти");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    EncodeOperationFromFile();
                }
                else if (choice == "2")
                {
                    DecodeOperationFromFile();
                }
                else if (choice == "3")
                {
                    EnterErrorAndEncodeOperationFromFile();
                }
                else if (choice == "4")
                {
                    Console.WriteLine("Завершення програми.");
                    break;
                }
                else
                {
                    Console.WriteLine("Неправильний вибір. Будь ласка, оберіть дійсну операцію.");
                }
            }
        }

        static void EncodeOperationFromFile()
        {
            Console.Write("Введіть шлях до текстового файлу для зчитування та кодування: ");
            string filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                try
                {
                    // Зчитати дані з файлу
                    string inputData = File.ReadAllText(filePath);

                    // Використовувати стратегію HammingEncoder для кодування даних
                    IHammingStrategy encoder = new HammingEncoder();
                    string encodedData = encoder.Encode(inputData);

                    // Записати закодовані дані у новий файл
                    File.WriteAllText("encoded_data.txt", encodedData);

                    Console.WriteLine("Закодовані дані записані в 'encoded_data.txt'.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Виникла помилка: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Файл не знайдено. Будь ласка, вкажіть дійсний шлях до файлу.");
            }
        }

        static void DecodeOperationFromFile()
        {
            Console.Write("Введіть шлях до текстового файлу для зчитування та декодування: ");
            string filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                try
                {
                    // Зчитати закодовані дані з файлу
                    string encodedData = File.ReadAllText(filePath);

                    // Використовувати стратегію HammingEncoder для декодування даних
                    IHammingStrategy decoder = new HammingEncoder();
                    string decodedData = decoder.Decode(encodedData);

                    // Записати декодовані дані у новий файл
                    File.WriteAllText("decoded_data.txt", decodedData);

                    Console.WriteLine("Декодовані дані записані в 'decoded_data.txt'.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Виникла помилка: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Файл не знайдено. Будь ласка, вкажіть дійсний шлях до файлу.");
            }
        }

        static void EnterErrorAndEncodeOperationFromFile()
        {
            Console.Write("Введіть шлях до текстового файлу для зчитування та кодування з помилкою: ");
            string filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                Console.Write("Введіть позицію для введення помилки (індекс з 0): ");
                if (!int.TryParse(Console.ReadLine(), out int errorPosition))
                {
                    Console.WriteLine("Неправильна позиція помилки. Будь ласка, введіть ціле число.");
                    return;
                }

                try
                {
                    // Зчитати дані з файлу
                    string inputData = File.ReadAllText(filePath);

                    // Використовувати стратегію HammingEncoderWithSimulation для кодування з помилкою
                    IHammingStrategy encoder = new HammingEncoderWithSimulation(errorPosition);
                    string encodedDataWithErrors = encoder.Encode(inputData);

                    // Записати закодовані дані з помилкою у новий файл
                    File.WriteAllText("encoded_data_with_errors.txt", encodedDataWithErrors);

                    Console.WriteLine("Закодовані дані з помилкою записані в 'encoded_data_with_errors.txt'.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Виникла помилка: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Файл не знайдено. Будь ласка, вкажіть дійсний шлях до файлу.");
            }
        }
    }
}

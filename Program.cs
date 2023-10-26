using System.Text;
using HammingChecker;

namespace HammingCheckerApp
{
    class Program
    {
        static void Main()
        {
            // Встановлюємо кодування виводу для коректного відображення українських символів.
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Ласкаво просимо до програми кодування та декодування Хемінга!");

            while (true)
            {
                Console.WriteLine("\nОберіть операцію:");
                Console.WriteLine("1. Кодувати текстовий файл");
                Console.WriteLine("2. Декодувати байтовий файл");
                Console.WriteLine("3. Ввести помилку та кодувати текстовий файл");
                Console.WriteLine("4. Вийти");
                Console.Write("Ваш вибір (1 - 4): ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    EncodeTextOperationFromFile();
                }
                else if (choice == "2")
                {
                    DecodeOperationFromFile();
                }
                else if (choice == "3")
                {
                    EnterErrorAndEncodeTextOperationFromFile();
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

        static void EncodeTextOperationFromFile()
        {
            Console.Write("Введіть шлях до текстового файлу для зчитування та кодування: ");
            string filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                try
                {
                    // Зчитуємо текст з файлу і перетворюємо його в байти з кодуванням UTF-8.
                    string text = File.ReadAllText(filePath);
                    byte[] textBytes = Encoding.UTF8.GetBytes(text);

                    Console.WriteLine("Початкові дані (бітова послідовність):");
                    Console.WriteLine(BitSequenceToString(textBytes));

                    // Створюємо кодер Хемінга та кодуємо дані.
                    IHammingStrategy encoder = new HammingEncoder();
                    byte[] encodedData = encoder.Encode(textBytes);

                    Console.WriteLine("Дані, які будуть кодуватися (бітова послідовність):");
                    Console.WriteLine(BitSequenceToString(encodedData));

                    // Записуємо закодовані дані в новий файл.
                    File.WriteAllBytes("encoded_data.bin", encodedData);

                    Console.WriteLine("Закодовані дані записані в 'encoded_data.bin'.");
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
            Console.Write("Введіть шлях до байтового файлу для зчитування та декодування: ");
            string filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                try
                {
                    // Зчитуємо закодовані дані з файлу.
                    byte[] encodedData = File.ReadAllBytes(filePath);

                    Console.WriteLine("Закодовані дані (бітова послідовність):");
                    Console.WriteLine(BitSequenceToString(encodedData));

                    // Створюємо декодер Хемінга та декодуємо дані.
                    IHammingStrategy decoder = new HammingEncoder();
                    byte[] decodedData = decoder.Decode(encodedData);

                    Console.WriteLine("Розкодовані дані (бітова послідовність):");
                    Console.WriteLine(BitSequenceToString(decodedData));

                    // Перетворюємо байти в текст з кодуванням UTF-8 і записуємо в файл.
                    string decodedText = Encoding.UTF8.GetString(decodedData);
                    File.WriteAllText("decoded_data.txt", decodedText);

                    Console.WriteLine("Розкодований текст записано в 'decoded_data.txt'.");
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

        static void EnterErrorAndEncodeTextOperationFromFile()
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
                    // Зчитуємо текст з файлу і перетворюємо його в байти з кодуванням UTF-8.
                    string text = File.ReadAllText(filePath);
                    byte[] textBytes = Encoding.UTF8.GetBytes(text);

                    Console.WriteLine("Початкові дані (бітова послідовність):");
                    Console.WriteLine(BitSequenceToString(textBytes));

                    // Створюємо кодер Хемінга з можливістю симуляції помилки та кодуємо дані.
                    IHammingStrategy encoder = new HammingEncoderWithSimulation(errorPosition);
                    byte[] encodedDataWithErrors = encoder.Encode(textBytes);

                    Console.WriteLine("Закодовані дані з помилкою (бітова послідовність):");
                    Console.WriteLine(BitSequenceToString(encodedDataWithErrors));

                    // Записуємо закодовані дані з помилкою в новий файл.
                    File.WriteAllBytes("encoded_data_with_errors.bin", encodedDataWithErrors);

                    Console.WriteLine("Закодовані дані з помилкою записані в 'encoded_data_with_errors.bin'.");
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

        static string BitSequenceToString(byte[] data)
        {
            return string.Join(" ", data.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        }
    }
}

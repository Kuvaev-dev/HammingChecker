using HammingChecker;
using System.Text;

namespace HammingCheckerApp
{
    // Основний клас програми.
    class Program
    {
        static void Main()
        {
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
                    string text = File.ReadAllText(filePath);
                    byte[] textBytes = Encoding.UTF8.GetBytes(text);

                    Console.WriteLine("Початкові дані (байти):");
                    PrintByteTable(textBytes);

                    // Ініціалізуємо кодер Хемінга.
                    IHammingStrategy encoder = new HammingEncoder();
                    // Округлюємо довжину textBytes до ближнього більшого кратного 4 для коректної обробки.
                    int roundedLength = (int)Math.Ceiling(textBytes.Length / 4.0) * 4;
                    Array.Resize(ref textBytes, roundedLength);
                    byte[] encodedData = encoder.Encode(textBytes);

                    Console.WriteLine("Дані, які будуть кодуватися (байти):");
                    PrintByteTable(encodedData);

                    // Зберігаємо закодовані дані в файл 'encoded_data.bin'.
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
                    byte[] encodedData = File.ReadAllBytes(filePath);

                    Console.WriteLine("Закодовані дані (байти):");
                    PrintByteTable(encodedData);

                    // Ініціалізуємо декодер Хемінга.
                    IHammingStrategy decoder = new HammingEncoder();
                    byte[] decodedData = decoder.Decode(encodedData);

                    // При декодуванні може виникнути декілька нульових байтів на кінці, видалімо їх.
                    int nonZeroLength = decodedData.Length;
                    for (int i = decodedData.Length - 1; i >= 0; i--)
                    {
                        if (decodedData[i] != 0)
                        {
                            break;
                        }
                        nonZeroLength--;
                    }
                    Array.Resize(ref decodedData, nonZeroLength);

                    Console.WriteLine("Розкодовані дані (байти):");
                    PrintByteTable(decodedData);

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
                    string text = File.ReadAllText(filePath);
                    byte[] textBytes = Encoding.UTF8.GetBytes(text);

                    Console.WriteLine("Початкові дані (байти):");
                    PrintByteTable(textBytes);

                    // Ініціалізуємо кодер Хемінга з можливістю симуляції помилки.
                    IHammingStrategy encoder = new HammingEncoderWithSimulation(errorPosition);
                    byte[] encodedDataWithErrors = encoder.Encode(textBytes);

                    Console.WriteLine("Закодовані дані з помилкою (байти):");
                    PrintByteTable(encodedDataWithErrors);

                    // Зберігаємо закодовані дані з помилкою в файл 'encoded_data_with_errors.bin'.
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

        static void PrintByteTable(byte[] data)
        {
            Console.WriteLine("Byte Index | Binary Value");
            Console.WriteLine("----------------------------");
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine($"{i,11} | {Convert.ToString(data[i], 2).PadLeft(8, '0')}");
            }
        }
    }
}

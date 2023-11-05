using System.Text;

namespace HammingChecker
{
    // Клас Program містить точку входу в програму та операції з використанням Хемінга.
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Ласкаво просимо до програми кодування, декодування та виправлення помилок Хемінга!");

            while (true)
            {
                Console.WriteLine("\nОберіть операцію:");
                Console.WriteLine("1. Кодувати текстовий файл");
                Console.WriteLine("2. Декодувати байтовий файл");
                Console.WriteLine("3. Виправити помилку в закодованій послідовності");
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
                    FixErrorInEncodedData();
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

        // Метод EncodeTextOperationFromFile виконує кодування текстового файлу з використанням Хемінга.
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

                    IHammingStrategy encoder = new HammingEncoder();
                    int roundedLength = (int)Math.Ceiling(textBytes.Length / 4.0) * 4;
                    Array.Resize(ref textBytes, roundedLength);
                    byte[] encodedData = encoder.Encode(textBytes);

                    Console.WriteLine("Дані, які будуть кодуватися (байти):");
                    PrintByteTable(encodedData);

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

        // Метод DecodeOperationFromFile виконує декодування байтового файлу з використанням Хемінга.
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

                    IHammingStrategy decoder = new HammingEncoder();
                    byte[] decodedData = decoder.Decode(encodedData);

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

        // Метод FixErrorInEncodedData виконує виправлення помилок у закодованій послідовності.
        static void FixErrorInEncodedData()
        {
            Console.Write("Введіть шлях до файлу з закодованою послідовністю (.bin): ");
            string filePath = Console.ReadLine();

            if (File.Exists(filePath))
            {
                try
                {
                    byte[] encodedData = File.ReadAllBytes(filePath);

                    Console.WriteLine("Закодована послідовність (байти):");
                    PrintByteTable(encodedData);

                    Console.Write("Введіть позицію помилки (1 - " + encodedData.Length + "): ");
                    int errorPosition = int.Parse(Console.ReadLine());

                    if (errorPosition >= 1 && errorPosition <= encodedData.Length)
                    {
                        IHammingStrategy encoder = new HammingEncoder();
                        if (encoder is HammingEncoder hammingEncoder)
                        {
                            Console.WriteLine("Позиція помилки була внесена на біту " + errorPosition);
                            byte[] correctedData = hammingEncoder.FixError(encodedData, errorPosition);
                            File.WriteAllBytes("corrected_data.bin", correctedData);
                            Console.WriteLine("Позиція помилки була виправлена.");
                            Console.WriteLine("Послідовність з помилкою виправлена та збережена в 'corrected_data.bin'.");
                            Console.WriteLine("Виправлена послідовність (байти):");
                            PrintByteTable(correctedData);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неправильна позиція помилки. Введіть число в межах від 1 до " + encodedData.Length + ".");
                    }
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

        // Метод PrintByteTable виводить байтові дані у форматі таблиці з індексами та бінарними значеннями.
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

using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Встановлюємо кодування консолі на Unicode та змінюємо колір тексту на зелений.
        Console.OutputEncoding = Encoding.Unicode;
        Console.ForegroundColor = ConsoleColor.Green;

        // Створюємо екземпляр класу HammingEncoder.
        HammingEncoder hammingEncoder = new();

        while (true)
        {
            // Виводимо меню опцій.
            Console.WriteLine("1. Кодувати");
            Console.WriteLine("2. Декодувати");
            Console.WriteLine("3. Внести помилку");
            Console.WriteLine("4. Вийти");
            Console.Write("Введіть номер опції: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    // Опція кодування.
                    Console.Write("Введіть шлях до вихідного файлу: ");
                    string sourcePath = Console.ReadLine();
                    if (!File.Exists(sourcePath))
                    {
                        Console.WriteLine("Файл не існує. Спробуйте ще раз.");
                        break;
                    }

                    // Зчитуємо байти з вихідного файлу.
                    byte[] sourceBytes = File.ReadAllBytes(sourcePath);
                    Console.WriteLine("Початкові байти:");
                    PrintBytes(sourceBytes);

                    // Виконуємо кодування і зберігаємо закодовані байти в інший файл.
                    byte[] encodedBytes = hammingEncoder.Encode(sourceBytes);
                    Console.Write("Введіть шлях для збереження закодованого файлу: ");
                    string encodedPath = Console.ReadLine();
                    File.WriteAllBytes(encodedPath, encodedBytes);
                    Console.WriteLine("Файл успішно закодовано.");
                    Console.WriteLine("Закодовані байти:");
                    PrintBytes(encodedBytes);

                    break;
                case "2":
                    // Опція декодування.
                    Console.Write("Введіть шлях до закодованого файлу: ");
                    string toDecodePath = Console.ReadLine();
                    if (!File.Exists(toDecodePath))
                    {
                        Console.WriteLine("Файл не існує. Спробуйте ще раз.");
                        break;
                    }

                    // Зчитуємо закодовані байти з файлу.
                    byte[] toDecodeBytes = File.ReadAllBytes(toDecodePath);
                    Console.WriteLine("Закодовані байти:");
                    PrintBytes(toDecodeBytes);

                    // Виконуємо декодування і зберігаємо декодовані байти в інший файл.
                    byte[] decodedBytes = hammingEncoder.Decode(toDecodeBytes);
                    Console.Write("Введіть шлях для збереження декодованого файлу: ");
                    string decodedPath = Console.ReadLine();
                    File.WriteAllBytes(decodedPath, decodedBytes);
                    Console.WriteLine("Файл успішно декодовано.");
                    Console.WriteLine("Декодовані байти:");
                    PrintBytes(decodedBytes);

                    break;
                case "3":
                    // Опція введення помилки.
                    Console.Write("Введіть шлях до закодованого файлу: ");
                    string errorPath = Console.ReadLine();
                    if (!File.Exists(errorPath))
                    {
                        Console.WriteLine("Файл не існує. Спробуйте ще раз.");
                        break;
                    }

                    // Зчитуємо закодовані байти з файлу.
                    byte[] errorBytes = File.ReadAllBytes(errorPath);
                    Console.Write("Введіть позицію помилки: ");
                    int errorPosition;
                    if (!int.TryParse(Console.ReadLine(), out errorPosition) || errorPosition < 0 || errorPosition >= errorBytes.Length * 8)
                    {
                        Console.WriteLine("Невірна позиція помилки. Спробуйте ще раз.");
                        break;
                    }

                    // Вводимо помилку в байти та зберігаємо в інший файл.
                    byte[] errorIntroducedBytes = hammingEncoder.IntroduceError(errorBytes, errorPosition);
                    Console.Write("Введіть шлях для збереження файлу з помилкою: ");
                    string errorIntroducedPath = Console.ReadLine();
                    File.WriteAllBytes(errorIntroducedPath, errorIntroducedBytes);
                    Console.WriteLine("Помилка введена.");
                    Console.WriteLine("Байти з помилкою:");
                    PrintBytes(errorIntroducedBytes);
                    Console.WriteLine("Позиція помилки: " + errorPosition);

                    // Визначаємо та виправляємо помилку у байтах.
                    byte[] correctedBytes = hammingEncoder.DetectAndCorrectError(errorIntroducedBytes);
                    Console.Write("Введіть шлях для збереження виправленого файлу: ");
                    string correctedPath = Console.ReadLine();
                    File.WriteAllBytes(correctedPath, correctedBytes);
                    Console.WriteLine("Помилка виправлена.");
                    Console.WriteLine("Виправлені байти:");
                    PrintBytes(correctedBytes);

                    break;
                case "4":
                    // Вихід із програми.
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    static void PrintBytes(byte[] bytes)
    {
        Console.WriteLine("---------------------------------");
        Console.WriteLine("| Index       | Byte Data       |");
        Console.WriteLine("---------------------------------");
        for (int i = 0; i < bytes.Length; i++)
        {
            Console.WriteLine("| " + i.ToString().PadRight(12) + "| " + Convert.ToString(bytes[i], 2).PadLeft(8, '0') + "        |");
        }
        Console.WriteLine("---------------------------------");
    }
}

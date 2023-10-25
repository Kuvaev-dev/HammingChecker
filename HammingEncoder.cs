using System.Text;

namespace HammingChecker
{
    class HammingEncoder : IHammingStrategy
    {
        public string Encode(string data)
        {
            if (data.Length != 4)
            {
                throw new ArgumentException("Вхідні дані повинні містити 4 біти.");
            }

            // Ініціалізація коду Хемінга з 7 бітів (4 біти даних та 3 біти парності)
            char[] hammingCode = new char[7];
            int dataIdx = 0; // Індекс для ітерації через біти даних

            for (int i = 1; i <= 7; i++)
            {
                // Обчислення позицій бітів парності (1, 2 та 4)
                if (i == 1 || i == 2 || i == 4)
                {
                    // Ініціалізація біта парності значенням 0
                    hammingCode[i - 1] = '0';
                }
                else
                {
                    // Копіювання біту даних до коду Хемінга
                    hammingCode[i - 1] = data[dataIdx++];
                }
            }

            // Обчислення та встановлення значень бітів парності
            hammingCode[0] = CalculateParityBit(hammingCode, new int[] { 1, 3, 5 });
            hammingCode[1] = CalculateParityBit(hammingCode, new int[] { 2, 3, 6 });
            hammingCode[3] = CalculateParityBit(hammingCode, new int[] { 4, 5, 6 });

            // Конвертація масиву символів у рядок
            string encodedData = new string(hammingCode);

            return encodedData;
        }

        public string Decode(string encodedData)
        {
            if (encodedData.Length != 7)
            {
                throw new ArgumentException("Вхідні дані повинні містити 7 бітів.");
            }

            // Ініціалізація коду Хемінга з 7 бітів (4 біти даних та 3 біти парності)
            char[] hammingCode = encodedData.ToCharArray();

            // Обчислення бітів перевірки парності
            int[] errorPositions = new int[3];
            errorPositions[0] = CalculateParityBit(hammingCode, new int[] { 0, 2, 4, 6 });
            errorPositions[1] = CalculateParityBit(hammingCode, new int[] { 1, 2, 5, 6 });
            errorPositions[2] = CalculateParityBit(hammingCode, new int[] { 3, 4, 5, 6 });

            int errorPosition = errorPositions[0] + 2 * errorPositions[1] + 4 * errorPositions[2] - 1;

            if (errorPosition >= 0)
            {
                // Виправлення біта помилки
                hammingCode[errorPosition] = (hammingCode[errorPosition] == '0') ? '1' : '0';
            }

            // Вилучення та повернення початкових 4 бітів даних
            StringBuilder decodedData = new StringBuilder();
            for (int i = 0; i < 7; i++)
            {
                if (i != 0 && i != 1 && i != 3)
                {
                    decodedData.Append(hammingCode[i]);
                }
            }

            return decodedData.ToString();
        }

        private char CalculateParityBit(char[] hammingCode, int[] positions)
        {
            int countOnes = 0;

            foreach (int pos in positions)
            {
                if (hammingCode[pos - 1] == '1')
                {
                    countOnes++;
                }
            }

            // Встановлення біта парності на '1', якщо кількість одиниць непарна, в іншому випадку - '0'
            return (countOnes % 2 == 1) ? '1' : '0';
        }
    }
}
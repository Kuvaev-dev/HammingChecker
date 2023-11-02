namespace HammingChecker
{
    // Клас HammingEncoderWithSimulation також реалізує інтерфейс IHammingStrategy для кодування та декодування Хемінга,
    // але дозволяє симулювати помилку.
    class HammingEncoderWithSimulation : IHammingStrategy
    {
        private readonly int errorPosition;

        public HammingEncoderWithSimulation(int errorPosition)
        {
            this.errorPosition = errorPosition;
        }

        // Метод кодування Хемінга з можливістю симуляції помилки
        public byte[] Encode(byte[] data)
        {
            // Визначимо довжину блоку даних, залежно від розміру вхідних даних.
            int dataBlockSize = 4; // Фіксований розмір блоку.
            if (data.Length > dataBlockSize)
            {
                dataBlockSize = data.Length;
            }

            // Створюємо масив для кодових даних.
            int hammingCodeLength = CalculateHammingCodeLength(dataBlockSize);
            byte[] hammingCode = new byte[hammingCodeLength];
            int dataIdx = 0;

            // Заповнюємо масив, додаючи біти даних та обчислені біти перевірки парності.
            for (int i = 0; i < hammingCodeLength; i++)
            {
                if (IsParityPosition(i))
                {
                    hammingCode[i] = 0; // Позиція біта перевірки парності.
                }
                else if (dataIdx < data.Length)
                {
                    hammingCode[i] = data[dataIdx++];
                }
            }

            // Обчислюємо біти перевірки парності.
            CalculateParityBits(hammingCode);

            // Симулюємо помилку, якщо позиція помилки вказана користувачем.
            if (errorPosition >= 0 && errorPosition < hammingCodeLength)
            {
                hammingCode[errorPosition] = (hammingCode[errorPosition] == 0) ? (byte)1 : (byte)0;
            }

            return hammingCode;
        }

        // Метод декодування Хемінга
        public byte[] Decode(byte[] encodedData)
        {
            // Визначимо довжину блоку даних, залежно від розміру вхідних даних.
            int dataBlockSize = 4; // Фіксований розмір блоку.
            if (encodedData.Length > dataBlockSize + 3)
            {
                dataBlockSize = encodedData.Length - 3; // Враховуємо 3 біти перевірки парності.
            }

            // Створюємо масив для декодованих даних.
            byte[] hammingCode = new byte[encodedData.Length];
            Array.Copy(encodedData, hammingCode, encodedData.Length);

            // Обчислюємо біти перевірки парності для виявлення помилок.
            int[] errorPositions = CalculateParityBits(hammingCode);

            int calculatedErrorPosition = CalculateCalculatedErrorPosition(errorPositions);

            if (calculatedErrorPosition != -1)
            {
                // Виявлено помилку на позиції calculatedErrorPosition, виправимо її.
                hammingCode[calculatedErrorPosition] = (hammingCode[calculatedErrorPosition] == 0) ? (byte)1 : (byte)0;
            }

            // Витягуємо оригінальні дані з кодових даних.
            byte[] decodedData = new byte[dataBlockSize];
            int decodedDataIdx = 0;

            for (int i = 0; i < hammingCode.Length; i++)
            {
                if (!IsParityPosition(i))
                {
                    decodedData[decodedDataIdx++] = hammingCode[i];
                }
            }

            return decodedData;
        }

        // Внутрішній метод для обчислення довжини кодового слова Хемінга.
        private int CalculateHammingCodeLength(int dataBlockSize)
        {
            int hammingCodeLength = dataBlockSize + 3; // Додамо 3 біти перевірки парності.
            while (!IsPowerOfTwo(hammingCodeLength))
            {
                hammingCodeLength++;
            }
            return hammingCodeLength;
        }

        // Внутрішній метод для перевірки, чи є число степенем двійки.
        private bool IsPowerOfTwo(int n)
        {
            return (n != 0) && ((n & (n - 1)) == 0);
        }

        // Внутрішній метод для перевірки, чи це позиція біта перевірки парності.
        private bool IsParityPosition(int position)
        {
            // Позиції бітів перевірки парності це степені 2: 1, 2, 4, 8, 16, ...
            // Тобто позиція парності задана у двійковій формі має лише одну одиницю.
            return (position & (position - 1)) == 0;
        }

        // Внутрішній метод для обчислення бітів перевірки парності.
        private int[] CalculateParityBits(byte[] hammingCode)
        {
            int hammingCodeLength = hammingCode.Length;
            int numParityBits = 0;
            while (IsPowerOfTwo(hammingCodeLength))
            {
                numParityBits++;
                hammingCodeLength--;
            }

            int[] errorPositions = new int[numParityBits];

            for (int i = 0; i < numParityBits; i++)
            {
                int mask = 1 << i;
                for (int j = 1; j < hammingCode.Length; j++)
                {
                    if ((j & mask) != 0 && hammingCode[j] == 1)
                    {
                        errorPositions[i] ^= 1;
                    }
                }
            }

            return errorPositions;
        }

        // Внутрішній метод для обчислення позиції помилки на основі розрахованих позицій помилок.
        private int CalculateCalculatedErrorPosition(int[] errorPositions)
        {
            int calculatedErrorPosition = 0;
            for (int i = 0; i < errorPositions.Length; i++)
            {
                calculatedErrorPosition |= errorPositions[i] << i;
            }
            return (calculatedErrorPosition == 0) ? -1 : calculatedErrorPosition;
        }
    }
}

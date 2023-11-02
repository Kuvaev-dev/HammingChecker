namespace HammingChecker
{
    // Клас HammingEncoder реалізує інтерфейс IHammingStrategy для кодування та декодування Хемінга.
    class HammingEncoder : IHammingStrategy
    {
        // Метод кодування Хемінга
        public byte[] Encode(byte[] data)
        {
            int dataLength = data.Length;
            int parityBitsCount = CalculateParityBitsCount(dataLength);
            int totalLength = dataLength + parityBitsCount;
            byte[] hammingCode = new byte[totalLength];

            int dataIndex = 0;
            int hammingIndex = 0;

            // Заповнюємо масив Хемінг-коду, обчислюючи значення бітів перевірки парності.
            for (int i = 0; i < totalLength; i++)
            {
                if (IsPowerOfTwo(i + 1)) // Індекси бітів перевірки парності
                {
                    hammingCode[i] = 0; // Ініціалізуємо біти перевірки парності зі значенням 0
                }
                else
                {
                    hammingCode[i] = data[dataIndex++];
                }
            }

            // Обчислюємо значення бітів перевірки парності
            for (int i = 0; i < parityBitsCount; i++)
            {
                int parityBitIndex = (int)Math.Pow(2, i) - 1;
                hammingCode[parityBitIndex] = CalculateParityBit(hammingCode, parityBitIndex, i);
            }

            return hammingCode;
        }

        // Метод декодування Хемінга
        public byte[] Decode(byte[] encodedData)
        {
            int encodedLength = encodedData.Length;
            int parityBitsCount = CalculateParityBitsCount(encodedLength);
            int dataLength = encodedLength - parityBitsCount;
            byte[] decodedData = new byte[dataLength];
            int dataIndex = 0;

            // Відновлюємо оригінальні дані, пропускаючи біти перевірки парності.
            for (int i = 0; i < encodedLength; i++)
            {
                if (!IsPowerOfTwo(i + 1)) // Пропускаємо біти перевірки парності
                {
                    decodedData[dataIndex++] = encodedData[i];
                }
            }

            return decodedData;
        }

        // Внутрішній метод для обчислення кількості бітів перевірки парності.
        private int CalculateParityBitsCount(int dataLength)
        {
            int m = dataLength;
            int r = 0;
            while (Math.Pow(2, r) <= m + r + 1)
            {
                r++;
            }
            return r;
        }

        // Внутрішній метод для перевірки, чи є число степенем двійки.
        private bool IsPowerOfTwo(int n)
        {
            return (n & (n - 1)) == 0;
        }

        // Внутрішній метод для обчислення біта перевірки парності.
        private byte CalculateParityBit(byte[] data, int parityBitIndex, int parityBitPosition)
        {
            byte parityBit = 0;
            for (int i = parityBitIndex; i < data.Length; i++)
            {
                if ((i + 1 & (1 << parityBitPosition)) != 0) // Перевірка бітів за позицією
                {
                    parityBit ^= data[i];
                }
            }
            return parityBit;
        }
    }
}

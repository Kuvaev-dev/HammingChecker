namespace HammingChecker
{
    // Клас HammingEncoder реалізує інтерфейс IHammingStrategy для кодування та декодування Хемінга.
    class HammingEncoder : IHammingStrategy
    {
        public byte[] Encode(byte[] data)
        {
            int dataLength = data.Length;
            int parityBitsCount = CalculateParityBitsCount(dataLength);
            int totalLength = dataLength + parityBitsCount;
            byte[] hammingCode = new byte[totalLength];

            int dataIndex = 0;
            int hammingIndex = 0;

            // Кодування бітів даних та додавання підсумкових бітів.
            for (int i = 0; i < totalLength; i++)
            {
                if (IsPowerOfTwo(i + 1))
                {
                    // Якщо це підсумковий біт, встановити його як 0.
                    hammingCode[i] = 0;
                }
                else
                {
                    // Якщо це біт даних, взяти його з вхідних даних.
                    hammingCode[i] = data[dataIndex++];
                }
            }

            // Розрахунок та додавання підсумкових бітів.
            for (int i = 0; i < parityBitsCount; i++)
            {
                int parityBitIndex = (int)Math.Pow(2, i) - 1;
                hammingCode[parityBitIndex] = CalculateParityBit(hammingCode, parityBitIndex, i);
            }

            return hammingCode;
        }

        public byte[] Decode(byte[] encodedData)
        {
            int encodedLength = encodedData.Length;
            int parityBitsCount = CalculateParityBitsCount(encodedLength);
            int dataLength = encodedLength - parityBitsCount;
            byte[] decodedData = new byte[dataLength];
            int dataIndex = 0;

            // Декодування даних, ігнорування підсумкових бітів.
            for (int i = 0; i < encodedLength; i++)
            {
                if (!IsPowerOfTwo(i + 1))
                {
                    // Якщо це біт даних, взяти його до розкодованих даних.
                    decodedData[dataIndex++] = encodedData[i];
                }
            }

            return decodedData;
        }

        public byte[] FixError(byte[] data, int errorPosition)
        {
            int parityBitsCount = CalculateParityBitsCount(data.Length);
            int parityBitPosition = (int)Math.Log(errorPosition, 2);

            // Виправлення помилки шляхом зміни біта.
            data[errorPosition - 1] = (byte)(data[errorPosition - 1] ^ 1);

            // Оновлення підсумкових бітів після виправлення помилки.
            for (int i = 0; i < parityBitsCount; i++)
            {
                int parityBitIndex = (int)Math.Pow(2, i) - 1;
                data[parityBitIndex] = CalculateParityBit(data, parityBitIndex, i);
            }

            return data;
        }

        // Метод CalculateParityBitsCount обчислює кількість додаткових бітів перевірки парності.
        private int CalculateParityBitsCount(int dataLength)
        {
            int m = dataLength;
            int r = 0;

            // Обчислення кількості підсумкових бітів (r) за формулою 2^r >= m + r + 1.
            while (Math.Pow(2, r) <= m + r + 1)
            {
                r++;
            }

            return r;
        }

        // Метод IsPowerOfTwo перевіряє, чи є число степенем двійки.
        private bool IsPowerOfTwo(int n)
        {
            return (n & (n - 1)) == 0;
        }

        // Метод CalculateParityBit обчислює біт перевірки парності для заданого позиційного біту.
        private byte CalculateParityBit(byte[] data, int parityBitIndex, int parityBitPosition)
        {
            byte parityBit = 0;

            // Обчислення підсумкового біта за допомогою операції XOR для відповідних бітів даних.
            for (int i = parityBitIndex; i < data.Length; i++)
            {
                if ((i + 1 & (1 << parityBitPosition)) != 0)
                {
                    parityBit ^= data[i];
                }
            }

            return parityBit;
        }
    }
}

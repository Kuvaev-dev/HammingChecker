namespace HammingChecker
{
    // Клас HammingEncoderWithSimulation також реалізує інтерфейс IHammingStrategy для кодування та декодування Хемінга, але дозволяє симулювати помилку.
    class HammingEncoderWithSimulation : IHammingStrategy
    {
        private readonly int errorPosition;

        public HammingEncoderWithSimulation(int errorPosition)
        {
            this.errorPosition = errorPosition;
        }

        public byte[] Encode(byte[] data)
        {
            if (data.Length != 4)
            {
                throw new ArgumentException("Вхідні дані повинні містити 4 байти.");
            }

            // Створюємо масив для кодових даних.
            byte[] hammingCode = new byte[7];
            int dataIdx = 0;

            // Заповнюємо масив, додаючи біти даних та обчислені біти перевірки парності.
            for (int i = 0; i < 7; i++)
            {
                if (i == 0 || i == 1 || i == 3)
                {
                    hammingCode[i] = 0;
                }
                else
                {
                    hammingCode[i] = data[dataIdx++];
                }
            }

            // Обчислюємо біти перевірки парності.
            hammingCode[0] = CalculateParityBit(hammingCode, new int[] { 0, 2, 4, 6 });
            hammingCode[1] = CalculateParityBit(hammingCode, new int[] { 1, 2, 5, 6 });
            hammingCode[3] = CalculateParityBit(hammingCode, new int[] { 3, 4, 5, 6 });

            // Симулюємо помилку, якщо позиція помилки вказана користувачем.
            if (errorPosition >= 0 && errorPosition < 7)
            {
                hammingCode[errorPosition] = (hammingCode[errorPosition] == 0) ? (byte)1 : (byte)0;
            }

            return hammingCode;
        }

        public byte[] Decode(byte[] encodedData)
        {
            if (encodedData.Length != 7)
            {
                throw new ArgumentException("Вхідні закодовані дані повинні містити 7 байтів.");
            }

            // Створюємо масив для декодованих даних.
            byte[] hammingCode = new byte[7];
            Array.Copy(encodedData, hammingCode, 7);

            // Обчислюємо біти перевірки парності для виявлення помилок.
            int[] errorPositions = new int[3];
            errorPositions[0] = CalculateParityBit(hammingCode, new int[] { 0, 2, 4, 6 });
            errorPositions[1] = CalculateParityBit(hammingCode, new int[] { 1, 2, 5, 6 });
            errorPositions[2] = CalculateParityBit(hammingCode, new int[] { 3, 4, 5, 6 });

            int calculatedErrorPosition = errorPositions[0] + 2 * errorPositions[1] + 4 * errorPositions[2] - 1;

            if (calculatedErrorPosition != -1)
            {
                // Виявлено помилку на позиції calculatedErrorPosition, виправимо її.
                hammingCode[calculatedErrorPosition] = (hammingCode[calculatedErrorPosition] == 0) ? (byte)1 : (byte)0;
            }

            // Витягуємо оригінальні дані з кодових даних.
            byte[] decodedData = new byte[4];
            int decodedDataIdx = 0;

            for (int i = 0; i < 7; i++)
            {
                if (i != 0 && i != 1 && i != 3)
                {
                    decodedData[decodedDataIdx++] = hammingCode[i];
                }
            }

            return decodedData;
        }

        private byte CalculateParityBit(byte[] hammingCode, int[] positions)
        {
            // Обчислюємо біт перевірки парності на основі заданих позицій.
            int countOnes = positions.Count(pos => hammingCode[pos] == 1);

            return (countOnes % 2 == 1) ? (byte)1 : (byte)0;
        }
    }
}

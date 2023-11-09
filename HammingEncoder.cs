using HammingChecker;

public class HammingEncoder : IHammingStrategy
{
    public byte[] Encode(byte[] bytes)
    {
        // Створюємо список для зберігання закодованих байтів.
        List<byte> encodedBytes = new();

        // Проходимо кожний байт вхідних даних.
        foreach (byte b in bytes)
        {
            // Розбиваємо кожний байт на окремі біти.
            int[] bits = new int[8];
            for (int i = 0; i < 8; i++)
            {
                bits[i] = (b >> i) & 1;
            }

            // Створюємо масив для зберігання закодованих бітів.
            int[] encodedBits = new int[12];

            // Розміщуємо біти даних на відповідних позиціях в закодованому слові.
            encodedBits[2] = bits[0];
            encodedBits[4] = bits[1];
            encodedBits[5] = bits[2];
            encodedBits[6] = bits[3];
            encodedBits[8] = bits[4];
            encodedBits[9] = bits[5];
            encodedBits[10] = bits[6];
            encodedBits[11] = bits[7];

            // Обчислюємо біти парності.
            encodedBits[0] = encodedBits[2] ^ encodedBits[4] ^ encodedBits[6] ^ encodedBits[8] ^ encodedBits[10];
            encodedBits[1] = encodedBits[2] ^ encodedBits[5] ^ encodedBits[6] ^ encodedBits[9] ^ encodedBits[10];
            encodedBits[3] = encodedBits[4] ^ encodedBits[5] ^ encodedBits[6] ^ encodedBits[11];
            encodedBits[7] = encodedBits[8] ^ encodedBits[9] ^ encodedBits[10] ^ encodedBits[11];

            // Конвертуємо закодовані біти назад у байти.
            byte encodedByte1 = (byte)(encodedBits[0] | (encodedBits[1] << 1) | (encodedBits[2] << 2) | (encodedBits[3] << 3) | (encodedBits[4] << 4) | (encodedBits[5] << 5) | (encodedBits[6] << 6) | (encodedBits[7] << 7));
            byte encodedByte2 = (byte)(encodedBits[8] | (encodedBits[9] << 1) | (encodedBits[10] << 2) | (encodedBits[11] << 3));

            // Додаємо закодовані байти до списку.
            encodedBytes.Add(encodedByte1);
            encodedBytes.Add(encodedByte2);
        }

        // Повертаємо закодовані байти як масив.
        return encodedBytes.ToArray();
    }

    public byte[] Decode(byte[] bytes)
    {
        // Створюємо список для зберігання декодованих байтів.
        List<byte> decodedBytes = new();

        // Проходимо кожний байт вхідних даних.
        for (int i = 0; i < bytes.Length; i += 2)
        {
            // Розбиваємо кожний байт на окремі біти.
            int[] encodedBits = new int[12];
            for (int j = 0; j < 8; j++)
            {
                encodedBits[j] = (bytes[i] >> j) & 1;
            }
            for (int j = 0; j < 4; j++)
            {
                encodedBits[j + 8] = (bytes[i + 1] >> j) & 1;
            }

            // Обчислюємо біти парності.
            int[] parityBits = new int[4];
            parityBits[0] = encodedBits[0] ^ encodedBits[2] ^ encodedBits[4] ^ encodedBits[6] ^ encodedBits[8] ^ encodedBits[10];
            parityBits[1] = encodedBits[1] ^ encodedBits[2] ^ encodedBits[5] ^ encodedBits[6] ^ encodedBits[9] ^ encodedBits[10];
            parityBits[2] = encodedBits[3] ^ encodedBits[4] ^ encodedBits[5] ^ encodedBits[6] ^ encodedBits[11];
            parityBits[3] = encodedBits[7] ^ encodedBits[8] ^ encodedBits[9] ^ encodedBits[10] ^ encodedBits[11];

            // Визначаємо позицію помилки.
            int errorBit = parityBits[0] | (parityBits[1] << 1) | (parityBits[2] << 2) | (parityBits[3] << 3);
            if (errorBit != 0)
            {
                // Виправляємо помилку, змінюючи біт на вказаній позиції.
                encodedBits[errorBit - 1] ^= 1;
            }

            // Конвертуємо закодовані біти назад у байти.
            byte decodedByte = (byte)(encodedBits[2] | (encodedBits[4] << 1) | (encodedBits[5] << 2) | (encodedBits[6] << 3) | (encodedBits[8] << 4) | (encodedBits[9] << 5) | (encodedBits[10] << 6) | (encodedBits[11] << 7));

            // Додаємо декодований байт до списку.
            decodedBytes.Add(decodedByte);
        }

        // Повертаємо декодовані байти як масив.
        return decodedBytes.ToArray();
    }

    public byte[] IntroduceError(byte[] bytes, int errorPosition)
    {
        // Вносимо помилку в байт, змінюючи біт на вказаній позиції.
        // Використовуємо операцію XOR з маскою, яка має 1 на відповідній позиції.
        bytes[errorPosition / 8] ^= (byte)(1 << (errorPosition % 8));
        return bytes;
    }

    public byte[] DetectAndCorrectError(byte[] bytes)
    {
        // Проходимо через кожний байт у масиві.
        for (int i = 0; i < bytes.Length; i += 2)
        {
            // Розбиваємо кожний байт на окремі біти.
            int[] encodedBits = new int[12];
            for (int j = 0; j < 8; j++)
            {
                encodedBits[j] = (bytes[i] >> j) & 1;
            }
            for (int j = 0; j < 4; j++)
            {
                encodedBits[j + 8] = (bytes[i + 1] >> j) & 1;
            }

            // Обчислюємо біти парності.
            int[] parityBits = new int[4];
            parityBits[0] = encodedBits[0] ^ encodedBits[2] ^ encodedBits[4] ^ encodedBits[6] ^ encodedBits[8] ^ encodedBits[10];
            parityBits[1] = encodedBits[1] ^ encodedBits[2] ^ encodedBits[5] ^ encodedBits[6] ^ encodedBits[9] ^ encodedBits[10];
            parityBits[2] = encodedBits[3] ^ encodedBits[4] ^ encodedBits[5] ^ encodedBits[6] ^ encodedBits[11];
            parityBits[3] = encodedBits[7] ^ encodedBits[8] ^ encodedBits[9] ^ encodedBits[10] ^ encodedBits[11];

            // Визначаємо позицію помилки.
            int errorBit = parityBits[0] | (parityBits[1] << 1) | (parityBits[2] << 2) | (parityBits[3] << 3);
            if (errorBit != 0)
            {
                // Виправляємо помилку, змінюючи біт на вказаній позиції.
                encodedBits[errorBit - 1] ^= 1;
            }

            // Записуємо виправлені біти назад у масив байтів.
            bytes[i] = (byte)(encodedBits[0] | (encodedBits[1] << 1) | (encodedBits[2] << 2) | (encodedBits[3] << 3) | (encodedBits[4] << 4) | (encodedBits[5] << 5) | (encodedBits[6] << 6) | (encodedBits[7] << 7));
            bytes[i + 1] = (byte)(encodedBits[8] | (encodedBits[9] << 1) | (encodedBits[10] << 2) | (encodedBits[11] << 3));
        }
        return bytes;
    }
}
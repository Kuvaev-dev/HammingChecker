namespace HammingChecker
{
    // Інтерфейс IHammingStrategy визначає методи, які мають бути реалізовані в кодерах та декодерах Хемінга.
    interface IHammingStrategy
    {
        byte[] Encode(byte[] data);
        byte[] Decode(byte[] encodedData);
        byte[] FixError(byte[] data, int errorPosition);
    }
}

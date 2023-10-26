namespace HammingChecker
{
    // Інтерфейс IHammingStrategy визначає методи, які мають бути реалізовані в кодерах та декодерах Хемінга.
    interface IHammingStrategy
    {
        byte[] Encode(byte[] data); // Метод кодування
        byte[] Decode(byte[] encodedData); // Метод декодування
    }
}

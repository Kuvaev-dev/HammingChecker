namespace HammingChecker
{
    // Інтерфейс IHammingStrategy визначає методи, які мають бути реалізовані в кодерах та декодерах Хемінга.
    interface IHammingStrategy
    {
        byte[] Encode(byte[] bytes);
        byte[] Decode(byte[] bytes);
        byte[] IntroduceError(byte[] bytes, int errorPosition);
        byte[] DetectAndCorrectError(byte[] bytes);
    }
}

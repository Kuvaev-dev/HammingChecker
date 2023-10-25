namespace HammingChecker
{
    // Визначення інтерфейсу стратегії Хемінга.
    interface IHammingStrategy
    {
        string Encode(string data);
        string Decode(string encodedData);
    }
}

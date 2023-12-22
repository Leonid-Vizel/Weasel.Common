namespace Weasel.Tools.Extensions.Common;

public static class InflectExtensions
{
    /// <summary>
    /// Метод для склонения слов.
    /// </summary>
    /// <param name="number">Само число, к которому надо просклонять слова</param>
    /// <param name="str1">Сколонение слова при 0. Например для слова 'день' - над написать "дней"</param>
    /// <param name="str2">Сколонение слова при 1. Например для слова 'день' - над написать "день"</param>
    /// <param name="str3">Сколонение слова при 2. Например для слова 'день' - над написать "дня"</param>
    /// <returns>Правильной склонение или str1</returns>
    public static string Inflect(this long number, string str1, string str2, string str3)
    {
        if (number % 10 == 0 || number % 100 >= 11 && number % 100 <= 19)
        {
            return str1;
        }
        return (number % 10) switch
        {
            1 => str2,
            2 or 3 or 4 => str3,
            _ => str1,
        };
    }

    /// <summary>
    /// Метод для склонения слов.
    /// </summary>
    /// <param name="number">Само число, к которому надо просклонять слова</param>
    /// <param name="str1">Сколонение слова при 0. Например для слова 'день' - над написать "дней"</param>
    /// <param name="str2">Сколонение слова при 1. Например для слова 'день' - над написать "день"</param>
    /// <param name="str3">Сколонение слова при 2. Например для слова 'день' - над написать "дня"</param>
    /// <returns>Правильной склонение или str1</returns>
    public static string Inflect(this int number, string str1, string str2, string str3)
     => ((long)number).Inflect(str1, str2, str3);

    /// <summary>
    /// Метод для склонения слов.
    /// </summary>
    /// <param name="number">Само число, к которому надо просклонять слова</param>
    /// <param name="str1">Сколонение слова при 0. Например для слова 'день' - над написать "дней"</param>
    /// <param name="str2">Сколонение слова при 1. Например для слова 'день' - над написать "день"</param>
    /// <param name="str3">Сколонение слова при 2. Например для слова 'день' - над написать "дня"</param>
    /// <returns>Правильной склонение или str1</returns>
    public static string Inflect(this byte number, string str1, string str2, string str3)
        => ((long)number).Inflect(str1, str2, str3);
}

using System.Collections.Generic;

public static class MorseTranslator
{
    private static readonly Dictionary<string, char> morseToChar = new()
{
    { ".-", 'A' },
    { "-...", 'B' },
    { "-.-.", 'C' },
    { "-..", 'D' },
    { ".", 'E' },
    { "..-.", 'F' },
    { "--.", 'G' },
    { "....", 'H' },
    { "..", 'I' },
    { ".---", 'J' },
    { "-.-", 'K' },
    { ".-..", 'L' },
    { "--", 'M' },
    { "-.", 'N' },
    { "---", 'O' },
    { ".--.", 'P' },
    { "--.-", 'Q' },
    { ".-.", 'R' },
    { "...", 'S' },
    { "-", 'T' },
    { "..-", 'U' },
    { "...-", 'V' },
    { ".--", 'W' },
    { "-..-", 'X' },
    { "-.--", 'Y' },
    { "--..", 'Z' },

    // Numbers
    { "-----", '0' },
    { ".----", '1' },
    { "..---", '2' },
    { "...--", '3' },
    { "....-", '4' },
    { ".....", '5' },
    { "-....", '6' },
    { "--...", '7' },
    { "---..", '8' },
    { "----.", '9' }
};

    public static bool TryTranslate(string morse, out char result)
    {
        return morseToChar.TryGetValue(morse, out result);
    }
}
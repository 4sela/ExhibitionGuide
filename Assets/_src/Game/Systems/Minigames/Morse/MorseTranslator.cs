using System.Collections.Generic;

namespace Game.Systems.Minigames.Morse
{
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
        };

        public static bool TryTranslate(string morse, out char result)
        {
            return morseToChar.TryGetValue(morse, out result);
        }
    }
}

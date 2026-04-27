using System.Collections.Generic;

namespace Game.Systems.Minigames.Morse
{
    public sealed class MorseWordValidator
    {
        private readonly string targetWord;

        public MorseWordValidator(string targetWord)
        {
            this.targetWord = targetWord.ToUpper();
        }

        /// <summary>
        ///
        /// </summary>
        public bool Validate(char[] input)
        {
            if (input.Length != targetWord.Length)
                return false;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != targetWord[i])
                    return false;
            }

            return true;
        }
    }
}

using System.Collections.Generic;

public class MorseWordValidator
{
    private readonly string targetWord;

    public MorseWordValidator(string targetWord)
    {
        this.targetWord = targetWord.ToUpper();
    }

    public bool Check(char[] input)
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

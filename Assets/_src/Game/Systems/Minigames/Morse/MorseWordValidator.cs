using System.Collections.Generic;

public class MorseWordValidator
{
    private readonly string targetWord;

    public MorseWordValidator(string targetWord)
    {
        this.targetWord = targetWord.ToUpper();
    }

    public bool Check(IReadOnlyList<char> input)
    {
        if (input.Count != targetWord.Length)
            return false;

        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] != targetWord[i])
                return false;
        }

        return true;
    }
}
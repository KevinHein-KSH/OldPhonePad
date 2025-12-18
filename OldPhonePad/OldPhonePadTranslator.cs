namespace OldPhonePad;

using System.Text;

public class OldPhonePadTranslator
{
    
    private static readonly Dictionary<char, string> _keyToCharMapping = new()
    {
        { '2', "abc" },
        { '3', "def" },
        { '4', "ghi" },
        { '5', "jkl" },
        { '6', "mno" },
        { '7', "pqrs" },
        { '8', "tuv" },
        { '9', "wxyz" },
        { '0', " " }
    };
    public static string OldPhonePad(string input)
    {
        var result = new StringBuilder();
        string trimmedInput = input.Trim();
        char? prevDigit = null;
        int count = 0;
        if (!string.IsNullOrEmpty(trimmedInput) && trimmedInput.EndsWith("#"))
        {
            trimmedInput = trimmedInput[..^1]; // work the same way as substring ^n to remove last character
        }
        void flush()
        {
            if (prevDigit.HasValue && count > 0)
            {
                string chars = _keyToCharMapping[prevDigit.Value];
                int charIndex = (count - 1) % chars.Length;
                result.Append(chars[charIndex]);
                prevDigit = null;
                count = 0;
            }
        }
        foreach (char c in trimmedInput)
        {
            if (c == ' ')
            {
                // Handle space character
                flush();
                continue;
            }
            else if (c == '*')
            {
                // Handle backspace
                flush();
                if (result.Length > 0)
                {
                    result.Remove(result.Length - 1, 1); // Remove last character
                }
                continue;
            }
            else if ((c >= '2' && c <= '9') || c == '0')
            {
                // Handle digit keyPresses
                if (prevDigit == c)
                {
                    count++;
                }
                else
                {
                    flush();
                    count = 1;
                }
                prevDigit = c;
                continue;
            }
            else
            {
                throw new ArgumentException($"Character '{c}' is not supported.");
            }
        }
        flush();
        return result.ToString().ToUpperInvariant();
    }
}

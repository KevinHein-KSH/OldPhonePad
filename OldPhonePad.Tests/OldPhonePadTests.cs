namespace OldPhonePad.Tests;

public class OldPhonePadTests
{
    [Fact]
    public void Test1()
    {
        Assert.Equal("TURING", OldPhonePadTranslator.OldPhonePad("8 88777444666664#")); //TURING
        Assert.Equal("THEQUICKBROWNFOXJUMPOVERTHELAZYDOG", OldPhonePadTranslator.OldPhonePad("844337788444222552277766696633366699588677777666888337778443355529999 99936664#"));
        Assert.Equal("YES", OldPhonePadTranslator.OldPhonePad("999337777#"));
    }

    // 1) Core decoding + cycling
    [Theory]
    [InlineData("2#", "A")]
    [InlineData("22#", "B")]
    [InlineData("222#", "C")]
    [InlineData("2222#", "A")] // cycles on key 2
    [InlineData("9999#", "Z")] // cycles on key 9 (wxyz)
    [InlineData("#", "")]      // minimal input
    public void CoreDecodingAndCycling(string input, string expected)
    {
        Assert.Equal(expected, OldPhonePadTranslator.OldPhonePad(input));
    }

    // 2) Space / pause handling (same key twice)
    [Theory]
    [InlineData("2 2#", "AA")]
    [InlineData("22 2#", "BA")]
    [InlineData("222 2 22#", "CAB")]
    [InlineData("4433555 555666#", "HELLO")]
    public void PauseWithSpaces(string input, string expected)
    {
        Assert.Equal(expected, OldPhonePadTranslator.OldPhonePad(input));
    }

    // 3) Backspace behavior (*)
    [Theory]
    [InlineData("227*#", "B")]   // PDF example
    [InlineData("33*#", "")]     // produce 'E' then backspace
    [InlineData("3*3#", "D")]    // remove 'D' then type 'D'
    [InlineData("3**#", "")]     // extra backspaces should not crash
    public void BackspaceBasic(string input, string expected)
    {
        Assert.Equal(expected, OldPhonePadTranslator.OldPhonePad(input));
    }

    // 4) Backspace with pending run (flush then delete)
    [Theory]
    [InlineData("777*#", "")]    // 'R' then delete
    [InlineData("7777*#", "")]   // 'S' then delete (key 7 has 4 letters)
    [InlineData("8 777*4#", "TG")] // T, R then delete R, then G
    public void BackspaceWithPendingOrAfterSpace(string input, string expected)
    {
        Assert.Equal(expected, OldPhonePadTranslator.OldPhonePad(input));
    }

    // 5) Multiple spaces / trimming robustness
    [Theory]
    [InlineData("   2#", "A")]          // Trim leading spaces
    [InlineData("44   33#", "HE")]      // Extra spaces are extra pauses
    [InlineData("44 33#", "HE")]
    public void RobustSpacing(string input, string expected)
    {
        Assert.Equal(expected, OldPhonePadTranslator.OldPhonePad(input));
    }

    // 6) Zero handling (only if you decided 0 => space)
    [Theory]
    [InlineData("0#", " ")]
    [InlineData("6660#", "O ")]
    [InlineData("0 0#", "  ")]
    [InlineData("6 6660#", "MO ")]
    public void ZeroAsSpace_Assumption(string input, string expected)
    {
        Assert.Equal(expected, OldPhonePadTranslator.OldPhonePad(input));
    }

    // 7) Invalid input (negative tests)
    [Theory]
    [InlineData("A#")]
    [InlineData("1#")]
    [InlineData("@#")]
    public void InvalidInput_Throws(string input)
    {
        Assert.Throws<ArgumentException>(() => OldPhonePadTranslator.OldPhonePad(input));
    }

    // 8) Stress-ish test (ensures no shared-state bugs / performance sanity)
    [Fact]
    public void LargeInput_DoesNotThrow_AndIsDeterministic()
    {
        // 1000 times "9999 " => 1000 'Z' characters (with trailing space pause)
        var input = string.Concat(System.Linq.Enumerable.Repeat("9999 ", 1000)) + "#";
        var expected = new string('Z', 1000);

        Assert.Equal(expected, OldPhonePadTranslator.OldPhonePad(input));
        Assert.Equal(expected, OldPhonePadTranslator.OldPhonePad(input)); // repeat call: must match
    }
}
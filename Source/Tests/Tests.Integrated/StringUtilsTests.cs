using Xunit;
using System.Text;
using Spotify.Slsk.Integration.Extensions;

namespace Tests.Integrated;

public class StringUtilsTests
{
    [Theory]
    [InlineData("hello & goodbye - (original mix) sjakie", "hello goodbye  original mix sjakie")]
    public void ShouldReturnCorrectQuery(string input, string expectedOutput)
    {
        StringBuilder stringBuilder = new();

        string processed = input.Replace("-", "").Replace("(", "").Replace(")", "");

        foreach (string queryPart in processed.Split(" "))
        {
            if (!queryPart.HasSpecialChars())
            {
                stringBuilder.Append(queryPart).Append(' ');
            }
        }

        Assert.Equal(expectedOutput, stringBuilder.ToString().Trim());
    }
}

namespace Win32ThemeStudio.Themes;

public static class ThemeIds
{
    public static string CreateStableId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Span<char> buffer = stackalloc char[value.Length];
        var index = 0;
        var lastWasSeparator = false;

        foreach (var character in value.Trim())
        {
            if (char.IsLetterOrDigit(character))
            {
                buffer[index++] = char.ToLowerInvariant(character);
                lastWasSeparator = false;
                continue;
            }

            if (lastWasSeparator || index == 0)
            {
                continue;
            }

            buffer[index++] = '-';
            lastWasSeparator = true;
        }

        while (index > 0 && buffer[index - 1] == '-')
        {
            index--;
        }

        return index == 0 ? "theme" : new string(buffer[..index]);
    }
}
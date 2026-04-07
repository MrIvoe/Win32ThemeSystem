namespace Win32ThemeStudio.Themes;

public sealed class ThemePresetValidationIssue
{
    public ThemePresetValidationIssue(
        string path,
        ThemePresetValidationSeverity severity,
        string message)
    {
        Path = path;
        Severity = severity;
        Message = message;
    }

    public string Path { get; }

    public ThemePresetValidationSeverity Severity { get; }

    public string Message { get; }

    public override string ToString()
    {
        return $"{Severity}: {Path} - {Message}";
    }
}
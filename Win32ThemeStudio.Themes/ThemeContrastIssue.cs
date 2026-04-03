namespace Win32ThemeStudio.Themes;

public sealed class ThemeContrastIssue
{
    public ThemeContrastIssue(
        string foregroundKey,
        string backgroundKey,
        double contrastRatio,
        double requiredContrastRatio,
        ThemeContrastSeverity severity,
        string message)
    {
        ForegroundKey = foregroundKey;
        BackgroundKey = backgroundKey;
        ContrastRatio = contrastRatio;
        RequiredContrastRatio = requiredContrastRatio;
        Severity = severity;
        Message = message;
    }

    public string ForegroundKey { get; }

    public string BackgroundKey { get; }

    public double ContrastRatio { get; }

    public double RequiredContrastRatio { get; }

    public ThemeContrastSeverity Severity { get; }

    public string Message { get; }

    public override string ToString()
    {
        return $"{Severity}: {ForegroundKey} on {BackgroundKey} is {ContrastRatio:F2}:1, expected {RequiredContrastRatio:F2}:1. {Message}";
    }
}
using Spectre.Console;

namespace Common.AoC;

public static class AoCClient {
    public static async Task DownloadInput(int year, int day, string destinationFile) {
        var url = $"https://adventofcode.com/{year}/day/{day}/input";
        var client = CreateHttpClient();

        var input = await client.GetStreamAsync(url);
        await using var file = File.OpenWrite(destinationFile);
        await input.CopyToAsync(file);
    }

    public static Task<string> DownloadPage(int year, int day) {
        var url = $"https://adventofcode.com/{year}/day/{day}";
        var client = CreateHttpClient();
        return client.GetStringAsync(url);
    }

    private static HttpClient CreateHttpClient() {
        var client = new HttpClient();

        var session = GetAoCSession();
        client.DefaultRequestHeaders.Add("Cookie", "session=" + session);
        return client;
    }

    private static string GetAoCSession() => ReadSessionFromEnv() ?? ReadSessionFromFile() ?? PromptForSession();

    private static string PromptForSession() {
        AnsiConsole.WriteLine("No session cookie was provided in AOC_SESSION environment variable");
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter session cookie:").Secret())
            .Replace("session=", string.Empty);
    }

    private static string? ReadSessionFromEnv() {
        var session = Environment.GetEnvironmentVariable("AOC_SESSION");
        if (string.IsNullOrEmpty(session)) return null;

        AnsiConsole.WriteLine("Reading session cookie from AOC_SESSION environment variable");
        return session;

    }

    private static string? ReadSessionFromFile() {
        var sessionFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aoc_session");
        if (!File.Exists(sessionFile)) return null;

        AnsiConsole.WriteLine("Reading session cookie from {0}", sessionFile);
        return File.ReadAllText(sessionFile);
    }
}

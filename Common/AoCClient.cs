using Spectre.Console;

namespace Common;

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

    private static string GetAoCSession() {
        var sessionFromEnv = Environment.GetEnvironmentVariable("AOC_SESSION");
        if (!string.IsNullOrEmpty(sessionFromEnv)) return sessionFromEnv;
        AnsiConsole.WriteLine("No session cookie was provided in AOC_SESSION environment variable");
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter session cookie:").Secret())
            .Replace("session=", string.Empty);
    }
}
namespace FileDiff.Services;

public static class FileService
{
    public static IEnumerable<string> ReadAllLines(string path)
    {
        return !File.Exists(path) ? Enumerable.Empty<string>() : File.ReadAllLines(path);
    }
}

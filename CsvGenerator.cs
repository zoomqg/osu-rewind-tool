namespace Rewind_Scraper;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

public class CsvGenerator
{
    private readonly List<PostInfo> _postInfos;
    public CsvGenerator(List<PostInfo> posts)
    {
        _postInfos = posts;
    }

    public void MakeFile()
    {
        string path = System.Environment.CurrentDirectory + "\\output.csv";
        using var writer = new StreamWriter(path);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(_postInfos);
        Console.WriteLine($"File {path} was made!");
    }
}
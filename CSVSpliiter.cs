using System;
using System.IO;
using System.Linq;

class CSVSplitter
{
    public static void SplitCSV(string filepath, int parts)
    {
        // Open the CSV file
        var rows = File.ReadAllLines(filepath);
        // Get the column names from the first row
        var headers = rows[0];
        // Get the total number of rows in the file
        var rowCount = rows.Length - 1;

        // Calculate the number of rows per part
        var rowsPerPart = rowCount / parts;
        var remainderRows = rowCount % parts;

        // Initialize the current row index
        var currentRow = 0;

        // Iterate over the parts
        for (int i = 0; i < parts; i++)
        {
            // Open a new file for the current part
            var filename = Path.Combine(Path.GetDirectoryName(filepath), $"part_{i + 1:D4}-{Path.GetFileName(filepath)}");
            var part = new string[rowsPerPart + (remainderRows > 0 ? 1 : 0)];
            part[0] = headers;
            // Write the rows for the current part
            for (int j = 0; j < rowsPerPart; j++)
            {
                if (currentRow >= rowCount)
                    break;
                part[j + 1] = rows[currentRow + 1];
                currentRow++;
            }
            if (remainderRows > 0)
            {
                part[rowsPerPart + 1] = rows[currentRow + 1];
                currentRow++;
                remainderRows--;
            }
            File.WriteAllLines(filename, part);
            Console.WriteLine($"Processing part {i + 1}");
        }
        Console.WriteLine("Splitting complete!");
        Console.WriteLine($"The parts reside in the directory: {Path.GetDirectoryName(filepath)}");
    }

    public static void Main(string[] args)
    {
        var filepath = args[0];
        var parts = int.Parse(args[1]);
        SplitCSV(filepath, parts);
    }
}

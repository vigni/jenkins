using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Cnam.UEGLG101.Journey.Data
{
    public class DataReader
    {
        public static List<Area> GetAllAreas()
        {
            var lines = ReadAreaFile();
            var allAreas = new List<Area>();
            var counter = 0;

            Parallel.ForEach(lines, line =>
            {
                counter++;
                try
                {
                    var values = line.Split(';');
                    allAreas.Add(new Area
                    {
                        Name = values[0],
                        Address = values[1],
                        PostalCode = values[2],
                        City = values[3],
                        Latitude = double.Parse(values[4], CultureInfo.InvariantCulture),
                        Longitude = double.Parse(values[5], CultureInfo.InvariantCulture),
                        Type = values[6]
                    });

                    Console.WriteLine($"[{counter}/{lines.Length}] new line imported.");
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{counter}/{lines.Length}] unable to parse current line.");
                }
            });
            return allAreas;
        }

        private static string[] ReadAreaFile()
        {
            return File.ReadAllLines(
                @"C:\Users\Vigni\source\repos\jenkins\Cnam.UEGLG101.Journey.Data\Cnam.UEGLG101.Journey.Data\areas.csv");
        }
    }
}

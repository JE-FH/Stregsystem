using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore {
    internal class CSVReader {
        string _seperator;
        public CSVReader(string seperator) {
            _seperator = seperator;
        }

        //TODO: god ide at bruge ushort?
        public string[][] ReadCSVFile(string path, int expectedColumnCount, ushort linesToSkip) {
            //TODO: add exception handling for file
            return File.ReadAllLines(path)
                .Skip(linesToSkip)
                .Where(line => line.Length > 0)
                .Select((string line) => {
                string[] columns = line.Split(_seperator).Select((string column) => RemoveQuotations(column)).ToArray();
                if (columns.Length != expectedColumnCount) {
                    throw new Exception($"CSV file {path} has incorrect amount of columns");
                }
                return columns;
            }).ToArray();

        }

        string RemoveQuotations(string target) {
            if (target.StartsWith("\"") && target.EndsWith("\"")) {
                return target[1..^1];
            }
            return target;
        }
    }
}

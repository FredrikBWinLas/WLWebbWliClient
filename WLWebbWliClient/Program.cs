using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using WLWebbWliClient.Models;

namespace WLWebbWliClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!_hasArgumentInstruction(args, "-file") || !_hasArgumentInstruction(args, "-connectionString")) return;

            var filename = _getValueOfArgumentInstruction(args, "-file");
            var connectionString = _getValueOfArgumentInstruction(args, "-connectionString");
            try
            {
                var licenseService = new LicenseService(connectionString);
                Console.WriteLine(JsonSerializer.Serialize(licenseService.GetAllLicenses()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine((filename));
            Console.WriteLine((connectionString));

            var extractHandler = new ExtractHandler(connectionString);
                filename = "292-2021-0002787_xml.zip";
                //extractHandler.DoXmlFile(filename);
        }

        private static string _getValueOfArgumentInstruction(string[] args, string instruction)
        {
            if (!_hasArgumentInstruction(args, instruction)) return "";
            int index = args.ToList().IndexOf(instruction);
            return args.Length > index + 1 ? args[index + 1] : "";
        }

        private static bool _hasArgumentInstruction(string[] args, string instruction)
        {
            return args.ToList().IndexOf(instruction) >= 0;
        }
    }
}

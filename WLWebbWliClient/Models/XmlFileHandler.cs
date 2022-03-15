using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models
{
    public class XmlFileHandler
    {
        private string _applicationPath;
        private string _applicationTempPath;
        public XmlFileHandler()
        {
            _applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            _applicationTempPath = Path.Combine(_applicationPath, "temp");
            if (!Directory.Exists(_applicationTempPath)) Directory.CreateDirectory(_applicationTempPath);
            ClearTempFolder();
        }

        public string ApplicationTempPath => _applicationTempPath;

        public void ClearTempFolder()
        {
            foreach (FileInfo file in new DirectoryInfo(_applicationTempPath).GetFiles())
            {
                if ((DateTime.Now-file.CreationTime).Minutes > 60) file.Delete();
            }
        }
        public string GetXmlStringFromFile(string filePath)
        {
            var fileExtension = new FileInfo(filePath).Extension;
            var xmlFilePath = "";

            if (!new FileInfo(filePath).Exists) return "";
            if (fileExtension == ".zip")
            {
                var files = ZipHandler.Unzip(filePath, _applicationTempPath);
                if (files.Length == 0) return "";
                foreach (var fileName in files)
                {
                    if (new FileInfo(fileName).Extension == ".xml")
                    {
                        xmlFilePath = fileName;
                        break;
                    }
                }
            }
            else if (fileExtension == ".gz")
            {
                foreach (var fileName in ZipHandler.DecompressGz(new FileInfo(filePath)))
                {
                    if (new FileInfo(fileName).Extension == ".xml")
                    {
                        xmlFilePath = fileName;
                        break;
                    }
                }
            }

            if (!File.Exists(xmlFilePath)) throw new NoXmlException();
            return File.ReadAllText(xmlFilePath);
        }

        public string[] GelXmlFiles()
        {
            var files = new DirectoryInfo(Path.Combine(_applicationPath, "XmlFiles")).GetFiles().Select(x => x.FullName).ToArray();
            return files;
        }
    }
}
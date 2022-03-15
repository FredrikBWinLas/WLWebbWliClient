using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace WLWebbWliClient.Models
{
    public class ZipHandler {
        public static void DeCompressFile(string CompressedFile, string DeCompressedFile)
        {
            byte[] buffer = new byte[1024 * 1024];

            using (System.IO.FileStream fstrmCompressedFile = System.IO.File.OpenRead(CompressedFile)) // fi.OpenRead())
            {
                using (System.IO.FileStream fstrmDecompressedFile = System.IO.File.Create(DeCompressedFile))
                {
                    using (System.IO.Compression.GZipStream strmUncompress = new System.IO.Compression.GZipStream(fstrmCompressedFile,
                        System.IO.Compression.CompressionMode.Decompress))
                    {
                        int numRead = strmUncompress.Read(buffer, 0, buffer.Length);

                        while (numRead != 0)
                        {
                            fstrmDecompressedFile.Write(buffer, 0, numRead);
                            fstrmDecompressedFile.Flush();
                            numRead = strmUncompress.Read(buffer, 0, buffer.Length);
                        }
                        strmUncompress.Close();
                    }
                    fstrmDecompressedFile.Flush();
                    fstrmDecompressedFile.Close();
                }
                fstrmCompressedFile.Close();
            }
        }

        public static string[] DecompressGz(FileInfo fileInfo)
        {
            var files = new List<string>();
            using (FileStream originalFileStream = fileInfo.OpenRead())
            {
                string currentFileName = fileInfo.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileInfo.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        files.Add(newFileName);
                    }
                }
            }

            return files.ToArray();
        }

        public static string[] Unzip(string file, string path)
        {
            List<string> files = new List<string>();
            using (ZipArchive archive = ZipFile.OpenRead(file))
            {
                var result = from currEntry in archive.Entries
                             where !String.IsNullOrEmpty(currEntry.Name)
                             select currEntry;


                foreach (ZipArchiveEntry entry in result)
                {
                    string extrPath = path;
                    string extension = Path.GetExtension(entry.Name);
                    if (true || extension == ".pdf")
                    {
                        string filepath = Path.Combine(extrPath, entry.Name);
                        string filename = entry.Name.Substring(0, entry.Name.Length - 4);
                        files.Add(filepath);
                        //int i = 1;
                        //while (File.Exists(filepath))
                        //{
                        //    filepath = Path.Combine(extrPath, filename + " (" + (i++) + ")" + extension);
                        //}
                        if (File.Exists(filepath))
                        {
                            File.Delete(filepath);
                        }
                        entry.ExtractToFile(filepath);
                    }
                }
            }
            //try { File.Move(file.FullName, Path.Combine(archivePath, file.Name)); }
            //catch { }
            return files.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DocumentAccessConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            var docsLibrary = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var files = Directory.GetFiles(docsLibrary, "*.*", SearchOption.AllDirectories);
            var fileInfos = files.Select(f => new FileInfo(f)).ToList();
            ValueType totalSize = fileInfos.Sum(f => f.Length);
            sw.Stop();
            Console.WriteLine($"Ellapsed time: {sw.ElapsedMilliseconds}  {fileInfos.Count} files - Total Size {totalSize}");
            Console.ReadLine();
        }
    }
}

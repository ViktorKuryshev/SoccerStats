using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SoccerStats
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "data.txt");
            var file = new FileInfo(fileName);
            if (file.Exists)
            {
                var reader = new StreamReader(file.FullName);

                try
                {
                    Console.SetIn(reader);
                    Console.WriteLine(Console.ReadLine());
                }
                finally // чтобы гарантировать закрытие ридера при эксепшене
                {
                    reader.Close(); // Должен где-то ссылаться на IDisposable чтобы можно было на прямую удалять
                }
                
            }

            
            /* --** Reading files from the directory **--
            var files = directory.GetFiles("*.txt");
            foreach(var file in files)
            {
                Console.WriteLine(file.Name);
            }
            */
        }
    }
}

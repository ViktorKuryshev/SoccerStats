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

                // Как понимаю используем директиву юзинг для того чтобы показать длительность использования
                // переменной и задиспозить ее после того как код в скобках будет выполнен
                using (var reader = new StreamReader(file.FullName))
                {
                    Console.SetIn(reader);
                    Console.WriteLine(Console.ReadLine());
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

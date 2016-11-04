using System;
using System.IO;

namespace printfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] allfiles = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory(), "*.tif", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var file in allfiles)
            {
                printfiles.Document doc = new printfiles.Document(file);
                doc.print();
            }
         }
    }
}

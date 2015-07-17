using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
namespace FileFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryAndFileFind A = new DirectoryAndFileFind();

            A.Patch = @"O:\";
            A.Ffile = "KLIKO.EXE";
            
            A.find();

            
            Console.ReadKey();
        }
    }
}

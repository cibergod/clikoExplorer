using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace clikosearch
{
    class MyConfig 
    {
        public StartSearchDirectory ="";
    }

    class DYrectoryAction 
    {
        public string PatchFile = "";
        public string SearchFile = "";

        public void Search()
        {
            // Ищем файл внутри указанной директории
            string[] Result = Directory.GetFileSystemEntries(PatchFile, SearchFile, SearchOption.AllDirectories);
            //Если поиск принес результат то выводим имя 
            if (Result.Length > 0)
            {
                Console.WriteLine(Result[0]);
                //убираем имя файла из пути
                //DirectoryInfo I = Directory.GetParent(Result[0]);
               
            }

        }

    }




    class Program
    {
    
    }
}

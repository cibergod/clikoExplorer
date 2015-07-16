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
            //обьявляем класс для поиска файлов и папок
            DirectoryAndFileFind.Finder FindFileDir = new DirectoryAndFileFind.Finder();
            //заполняем параметры поиска
            FindFileDir.MyInputParam.SearchFolder = "";
            FindFileDir.MyInputParam.StartPatchFilder = @"C:\";
            FindFileDir.MyInputParam.SearchFile = "cmd.exe";
            //запускаем поиск
            FindFileDir.find();

            //смотрим результат
            foreach (
                    DirectoryAndFileFind.OutputInfo N //струкрута результат найденных данных
                    in FindFileDir.OutputData         //структура хранящая все результаты поиска
                    ) 
            {
                Console.WriteLine(N.FullPatchFilder);
            }

            Console.WriteLine("все");
            Console.ReadKey();
        }
    }
}

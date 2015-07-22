using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace CreateDataReader
{
    class Program
    {
        static void Main(string[] args)
        {
           

            //поиск kliko
            /*
            //обьявляем класс для посиа
            Finder Finders = new Finder();
            //указываем папку где искать 
            Finders.StartPatch = @"o:\";

            Console.WriteLine("начинаем искать ");
            //запускаем поиск в гугле мож че найдет )) 
            Finders.gogle();
            */

            FORMSKLIKO MyReader = new FORMSKLIKO();

            MyReader.GetFormList();
            //GetFormList();

           


            
           // Console.ReadKey();
        }
    }
}

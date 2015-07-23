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
            /*
            //поиск kliko
            //обьявляем класс для посиа
                Finder Finders = new Finder();
            //указываем папку где искать 
                Finders.StartPatch = @"o:\";
            //запускаем поиск в гугле мож че найдет )) 
                Finders.gogle();
            /*************************************************************/
            /*
            //подготовка списка форм 
                FORMSKLIKO MyReader = new FORMSKLIKO();
                MyReader.GetFormList();
            /**************************************************************/
            SPR MySRPFinder = new SPR();
            MySRPFinder.GetDirectory();

            Console.WriteLine("END");
            Console.ReadKey();
        }
    }
}
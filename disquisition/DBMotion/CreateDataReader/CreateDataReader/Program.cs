using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace CreateDataReader
{
    class Program
    {

        static string convertto1251(string kaka) 
        {
            // создаем две кодировки 
            Encoding ascii = Encoding.UTF8;
            Encoding unicode = Encoding.Unicode;

            // преобразуем строку в байты
            byte[] unicodeBytes = unicode.GetBytes(kaka);

            // кодируем строку в другую кодировку
            byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

            // преобразуем строку байт в строку текста
            char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            
            string asciiString = new string(asciiChars);
            //вертаем результат обратно 
            return asciiString;
        }

     

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
            
         
        }
    }
}



//28591
// Perform the conversion from one encoding to the other.
 //     byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);
//      Convert the new byte[] into a char[] and then into a string.

//      char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
//      ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
//      string asciiString = new string(asciiChars);
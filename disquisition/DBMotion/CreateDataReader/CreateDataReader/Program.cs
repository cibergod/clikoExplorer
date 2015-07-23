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
            //обьявляем класс для посиа
            Finder Finders = new Finder();
            //указываем папку где искать 
            Finders.StartPatch = @"o:\";
            //запускаем поиск в гугле мож че найдет )) 
            Finders.gogle();
            /*************************************************************/
            
            //подготовка списка форм 
            FORMSKLIKO MyReader = new FORMSKLIKO();
            MyReader.GetFormList();
            /**************************************************************/

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;


namespace ConsoleApplication2
{
    class Program
    {
        //static string patch = @"d:\Cliko\BIN\DATA\KON\";
        static string patch = @"o:\F250\DATA\KAZ";
        static List<string> ReportDir = new List<string>();

        static string CutChar(string Name) 
        {
            string buf = "";
            foreach (char S in Name) 
            {
                if ((S != '\n') && (S != '\r')) buf += S;
            }
            return buf;
        }

        static string Shrink(string name) 
        {
            string[] M = name.Split(' ');
            int S = M.Length;
            S--;

            string result = CutChar(M[S]);
            return result+".db";
        }

        static bool ExistDigest(string data, string NameFile)
        {
            string P = patch + "SPR"+@"\" + data +@"\"+ NameFile;

           // Console.WriteLine(P);
            return File.Exists(P);
        }
        static void getFileDate(string data, string NameFile)
        {
            string P = patch + "SPR" + @"\" + data + @"\" + NameFile;

            DateTime F = File.GetLastWriteTime(P);
            Console.Write(F);
        }

        static void getDir() 
        {
            DB getDB = new DB();
            //получаем список директорий в папке
            string[] D = Directory.GetDirectories(patch);

            foreach (string nameDir in D) 
            {
                DirectoryInfo MyDir = new DirectoryInfo(nameDir);
                //если 8 символов значит наша дата 
                if (MyDir.Name.Length == 8)
                {
                    try //пробуем получить цифры
                    {
                        int Data_folder = Convert.ToInt32(MyDir.Name);
                        
                        Console.WriteLine(Data_folder);
                        //если прочитали данные пробуем прочитать справочники
                        getDB.DBPatchFolder = MyDir.FullName;

                        List<string[]> ReadDB = getDB.getData("SELECT * FROM rsprav");
                        //посмотрим что нашли 
                        foreach (string[] A in ReadDB) 
                        {
                            Console.WriteLine("");
                            string FileName=Shrink(A[3]);
                           //выводим пока на экран 
                            Console.WriteLine("\n"+A[2] + "\t файл  " + FileName + " файл ");
                            if (ExistDigest(MyDir.Name, FileName))
                            {
                                Console.Write(" существует обновлялся ");
                                getFileDate(MyDir.Name, FileName);
                            }
                            else
                                Console.Write(" нет файла ");
                        }
                    }
                    catch 
                    { 
                    }
                }
            }           

        }

        static void Main(string[] args)
        {
            getDir();
            Console.ReadKey();
        }
    }
}

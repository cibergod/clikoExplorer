using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

using System.Data.Odbc;
using System.Data;

namespace ConsoleApplication1
{

    struct Listdigest
    {
        public string name, DirPatch;
    }
    struct SearchList
    {
        public ReadDir F;
        public Thread S;
    }



 
    //класс работы с базой данных 
    class ReadDB 
    {
        /**Результат запроса*************************************/
        public string[] result = new string [1];
        /*********************************************************/
        OdbcConnection MyConn = new OdbcConnection();
        public string patch="";
        OdbcConnection connector = new OdbcConnection();
        bool OpenConnect()
        {
            bool connect = false;
            //строчка создания соеденения с базой
            string ConnectionString = @"Dsn=Config;dbq=" + patch + ";defaultdir=" + patch + "driverid=538;fil=Paradox 5.X;maxbuffersize=2048;pagetimeout=5";

            connector.ConnectionString = ConnectionString;
            try 
            {
                connector.Open();
                if (connector.State == ConnectionState.Open) connect = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ошибка " +e.Message);
            }
            return connect;
        }
        //читаем данные по таблице
        void ReadData(OdbcCommand cmdSQL) 
        {
            try
            {
                //получаем результат
                OdbcDataReader reader = cmdSQL.ExecuteReader();
                result = new string[reader.FieldCount];
                //читаем данные 
                while (reader.Read())
                {
                    for (int I = 0; I < reader.FieldCount; I++) 
                    {
                        try
                        {
                            result[I] = reader.GetString(I);
                        }
                        catch 
                        {
 
                        }
                    }
                    //Console.WriteLine( reader.GetString(1) + " папка " + reader.GetString(2));
                   
                }
                //освобождаем данные 
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ошибка " + e.Message);
            }
        }
        /*пробуем саязаться с базой и получить из нее данные */
        public void ReadTable(string TableName)
        {
            try
            {
                if (OpenConnect())
                {
                    string SQL = "Select * from " + TableName;

                    OdbcCommand cmdSQL = new OdbcCommand() { Connection = connector, CommandText = SQL };

                    //пробуем прочесть данные 
                    ReadData(cmdSQL);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ошибка " + e.Message);
            }
            finally 
            {
                connector.Close();
            }
        }

    
    }
   
    //чтение директории в отдельном потоке требует создания отдельного класса
    class ReadDir 
    {
        public List<Listdigest> MyListdigest = new List<Listdigest>();
        public string Patch, filename;

        public void SearchInDirectory()
        {
            string F = filename, D = Patch;
               
                try
                {
                    //пробуем найти  файл в директориях
                    string[] Result = Directory.GetFileSystemEntries(D, F, SearchOption.AllDirectories);
                    foreach (string R in Result)
                    {
                        //убираем имя файла из пути
                        DirectoryInfo I = Directory.GetParent(R);
                                      I = Directory.GetParent(I.ToString());
                                      I = Directory.GetParent(I.ToString());
                        //имя нашей отчетности

                        DirectoryInfo P=Directory.GetParent(R);
                        //Console.WriteLine(I.Name + "\n");//\t" + " V " + R);
                        Listdigest NewDigestSearch;
                        //заполняем структуру 
                        NewDigestSearch.name = I.Name;
                        NewDigestSearch.DirPatch =P.FullName;
                        //добавляем в список данные 
                        MyListdigest.Add(NewDigestSearch);
                        
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" " + e.Message);
                }
        }
    }
    //класс для работы с директориями
    class Finddigest 
    {
        public List<SearchList> M = new List<SearchList>();
       
        public void Find(string patch, string FileName)
        {

            string[] DirList = Directory.GetDirectories(patch);

          //  Console.Write("найдено папок " + DirList.Length + " \n");
            int I = 0;
            foreach(string Dirs in DirList)
            {
                SearchList S; 
                S.F=new ReadDir()
                {
                    filename = FileName,
                    Patch = Dirs
                };

                S.S = new Thread(S.F.SearchInDirectory);
                M.Add(S);
                S.S.Start();
                
                I++;
           }
            
        }

        public List<Listdigest> MyListdigestResult = new List<Listdigest>();

        void listAgregator()
        { 
           //агрегируем результаты в виде одного общего списка с которым будем дальее работать
            foreach (SearchList J in M) MyListdigestResult.AddRange(J.F.MyListdigest);
            
        }

        //пока хоть один поток работает нельзя продолжать работу с файлами поэтому ждем 
        public bool SearchFinish()
        {
            Console.Clear();
            bool mov = false;
            int I=0;
            foreach (SearchList J in M)
            {
                if (J.S.ThreadState == ThreadState.Running)
                {
                    mov = true;
                    Console.WriteLine(I + ") ищем на " + J.F.Patch);
                    I++;
                }
                
            }

            if (!mov) listAgregator();

            return mov;
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            Finddigest D = new Finddigest();
              //D.Find(@"d:\Cliko", "KLIKOCFG.DB");
            D.Find(@"o:\", "KLIKOCFG.DB");
            //ждем пока систма не найдет все данные в папках 
            while (D.SearchFinish())
            {
                Console.WriteLine("поиск");
                Thread.Sleep(50);
                
            }
           
            Console.Clear();
            Console.WriteLine("найденые отчетности");
            File.WriteAllText("find.txt", "");
            int Index= 0;
            //читем данные о данном отчете
            foreach (Listdigest S in D.MyListdigestResult)
            {
                File.AppendAllText("find.txt", Index + " ) " + S.name, Encoding.Default);
               // Console.WriteLine(Index + " ) " + S.name);
                Index++;
                //получаем список справочников 
                ReadDB Dbread = new ReadDB() 
                {
                    patch=S.DirPatch
                };
              //  Console.WriteLine(" " +  Directory.GetParent(S.DirPatch));
                File.AppendAllText("find.txt", " " + Directory.GetParent(S.DirPatch).ToString(), Encoding.Default);
                //пробуем найти данные о справочниках 
                Dbread.ReadTable("KLIKOCFG");
                //смотрим что нашли 
                foreach (string R in Dbread.result)
                {
                    //Console.Write("\t" + R + "\t");
                    File.AppendAllText("find.txt", "\t" + R + "\t", Encoding.Default);
                    
                }
                    Console.WriteLine("");

            }
            Console.WriteLine(" \n Выполнил \n");
            Console.ReadKey();
        }
    }
}

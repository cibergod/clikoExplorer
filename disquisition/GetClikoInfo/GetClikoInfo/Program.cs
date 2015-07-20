using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Odbc;
using System.Data.Sql;
using System.Data.ProviderBase;

using System.IO;
using System.Threading;

namespace GetClikoInfo
{
//ConnectDB(@"Dsn=Config;dbq=D:\CLIKO\BIN\DATA\CFG;defaultdir=D:\CLIKO\BIN\DATA\CFG;driverid=538;fil=Paradox 5.X;maxbuffersize=2048;pagetimeout=5");
    class DBAction
    {
        //создаем потключение к данным 
       public bool buildConnect(string patch)
        {
            bool conect = false;
            string Con = @"Dsn=Config;dbq=" + patch + ";defaultdir" + patch + "driverid=538;fil=Paradox 5.X;maxbuffersize=2048;pagetimeout=5";
            if (ConnectDB(Con))
            {
                //Console.WriteLine(patch + "YES");
                conect = true;
               
            }
            return conect;
        }

         OdbcConnection conn;
        /*пробуем потключиться к БД парадокс*/
        bool ConnectDB(string Connect)
        {
            bool Result = false;
            conn = new OdbcConnection()
            {
                ConnectionString = Connect
            };
            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                    Result= true;
            }
            catch 
            {
                Result = false;
            }
            return Result;

        }
        public void GetData(string SQL) 
        {
            try
            {
                OdbcCommand cmd = new OdbcCommand()
                {
                    Connection = conn,
                    CommandText = SQL //"Select * FROM KLIKOCFG"
                };
                OdbcDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(1));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ошибко " + e.Message);
            }

        }

    }

    class SerchFile 
    {
          public string patch, filename;
          public void Search()
          {
              string[] Result = Directory.GetFileSystemEntries(patch, filename, SearchOption.AllDirectories);
              // foreach (string s in Result) Console.WriteLine(s);
              if (Result.Length > 0) 
              {
                  Console.WriteLine(Result[0]);
                  //убираем имя файла из пути
                  DirectoryInfo I = Directory.GetParent(Result[0]);
                  DBAction Read_Paradox = new DBAction();
                  //пробуем прочесть данные 
                  if (Read_Paradox.buildConnect(I.FullName))
                  {
                      Read_Paradox.GetData("Select * FROM KLIKOCFG");
                  }
              }

          }
    }

    class Program
    {

       


        static void GetRootDir(string pachroot, string FileName)
        {
            string[] Result = Directory.GetDirectories(pachroot);
            foreach (string s in Result)
            {
                SerchFile Findes = new SerchFile()
                {
                    patch=s, 
                    filename=FileName
                };
                Thread A = new Thread(Findes.Search);
                A.Start();
            }
        }

       



        static void Main(string[] args)
        {
           // GetRootDir(@"O:\", "KLIKOCFG.DB");

            GetRootDir(@"d:\Cliko\BIN\", "KLIKOCFG.DB");

            

            Console.ReadKey();
        }
    }
}

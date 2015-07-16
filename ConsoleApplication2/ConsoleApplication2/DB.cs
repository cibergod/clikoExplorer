using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Odbc;
using System.Data;

namespace ConsoleApplication2
{
   public class DB
    {
       public string DBPatchFolder;

       OdbcConnection connector = new OdbcConnection();
       //открытие соеденения с базой данных 
       bool OpenConnect()
       {
           bool connect = false;
           //строчка создания соеденения с базой
           string ConnectionString = @"Dsn=Config;dbq=" + DBPatchFolder + ";defaultdir=" + DBPatchFolder + "driverid=538;fil=Paradox 5.X;maxbuffersize=2048;pagetimeout=5";

           connector.ConnectionString = ConnectionString;
           try
           {
               connector.Open();
               if (connector.State == ConnectionState.Open) connect = true;
           }
           catch (Exception e)
           {
               Console.WriteLine("ошибка " + e.Message);
           }
           return connect;
       }
       //чтение данных из таблиц
       public List<string[]> getData(string SQL) 
       {
           List<string[]> ResultRead = new List<string[]>();
           string[] Record;
           //открыли соеденение значит можно что то делать
           if (OpenConnect()) 
           {
               //формируем запрос
               OdbcCommand cmdSQL = new OdbcCommand() { Connection = connector, CommandText = SQL };
               try
               {
                   OdbcDataReader reader = cmdSQL.ExecuteReader();
                   while (reader.Read())
                   {
                       Record = new string[reader.FieldCount];
                       try
                       {
                           for (int Index = 0; Index < reader.FieldCount; Index++)
                           {
                               Record[Index] = reader.GetString(Index);
                           }
                       }
                       catch {}
                       finally
                       {
                           //запоминаем найденные данные
                           ResultRead.Add(Record);
                       }
                   }
               }
               catch
               {

               }
               finally 
               {
                   connector.Close();
               }
           }

           return ResultRead;
       }

    }
}

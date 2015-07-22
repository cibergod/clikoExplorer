using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Data.Odbc;
using System.Data;

namespace CreateDataReader
{
    class ReadDB
    {
        //путь к директории с базой данных 
        public string patch = "";
        //запрос который нужно выполнить
        public string SQL = "";
        //результат будем возвращать в виде таблиц
        public DataTable Result;
        OdbcConnection MyConn = new OdbcConnection();
        OdbcConnection connector = new OdbcConnection();
       //соеденение с базой данных 
        bool OpenConnect()
        {
           //флаг соеденения если оно произошло то все хорошо            
           bool connect = false;

            string ConnectionString = @"Dsn=Paradox;dbq=" + patch + ";defaultdir=" + patch + "driverid=538;fil=Paradox 5.X;maxbuffersize=2048;pagetimeout=5";
            //Driver={Microsoft Paradox Driver (*.db )};DriverID=538;Fil=Paradox 7.X;DefaultDir=D:\Orion\DEMO74\;Dbq=D:\Orion\DEMO74\;CollatingSequence=ANSI;"
            //строчка создания соеденения с базой
           //string ConnectionString = @"Dsn=Paradox;dbq=" + patch + ";defaultdir=" + patch + "driverid=538;fil=Paradox 5.X;maxbuffersize=2048;pagetimeout=5";

           
            //задаем строку по  лючения 
            connector.ConnectionString = ConnectionString;
            try
            {
                connector.Open();
                if (connector.State == ConnectionState.Open) connect = true;
            }
            catch (Exception e)
            {
               // Console.WriteLine("ошибка " + e.Message);
            }
            return connect;
        }
        //получаем данные из таблицы по запросу 
        public void getSQL() 
        {
               //открыли соеденение значит можно что то делать
            if (OpenConnect())
            {
                //формируем запрос
                OdbcCommand cmdSQL = new OdbcCommand() { Connection = connector, CommandText = SQL };
                try
                {
                    OdbcDataReader reader = cmdSQL.ExecuteReader();
                    Result = new DataTable();
                    Result.Load(reader);
                    

                }
                catch (Exception e)
                {
                    // Console.WriteLine("ошибка " + e.Message);
                }
                finally
                {
                    connector.Close();
                }
            }
            connector.Close();
        }
    }
}

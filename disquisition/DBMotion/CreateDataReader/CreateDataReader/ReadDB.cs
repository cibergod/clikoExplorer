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
        bool OpenConnect()
        {
           //флаг соеденения если оно произошло то все хорошо            
           bool connect = false;
            //строчка создания соеденения с базой
           string ConnectionString = @"Dsn=Paradox;dbq=" + patch + ";defaultdir=" + patch + "driverid=538;fil=Paradox 5.X;maxbuffersize=2048;pagetimeout=5";
            //задаем строку подключения 
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

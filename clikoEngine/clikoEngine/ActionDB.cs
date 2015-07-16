using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;

namespace clikoEngine
{
    /*класс работы с базой данных*/
    class ActionDB
    {
        /*путь к папке для чтения таблиц*/
        public string DBPatchFolder;
        /*Обьект для соеденения с базой данных*/
        OdbcConnection connector = new OdbcConnection();
        /*получение данных из файла настроек*/
        ConfigAction config = new ConfigAction();

        //открытие соеденения с базой данных 
        bool OpenConnect()
        {
            config.LoadParam();
            bool connect = false;
            //строчка создания соеденения с базой
            string ConnectionString = @"Dsn=" + config.Parametrs.DSN_ODBC       //имя DSN линка на драйвер 
                                              + ";dbq=" + DBPatchFolder         //директория с базой данных
                                              + ";defaultdir=" + DBPatchFolder  //директория с базой данных по умолчанию
                                              + "driverid=538;fil=Paradox 5.X;maxbuffersize=2048;pagetimeout=5"; //параметры драйвера для соеденения с базой данных
            //настраиваем соеденение
            connector.ConnectionString = ConnectionString;
            try
            {
                //пробуем открыть соеденение
                connector.Open();
                //если все получилось то возвращаем true 
                if (connector.State == ConnectionState.Open) connect = true;
            }
            catch (Exception e)
            {
                //если что то пошло не так то пишем в Windows log в планах
                //Console.WriteLine("ошибка " + e.Message);
            }
            return connect;
        }
        //чтение данных из таблиц
        public List<string[]> getData(string SQL) //получаем запрос на выполнение и возвращаем список строк результата запроса
        {
            //создаем пустой список строк
            List<string[]> ResultRead = new List<string[]>();
            //одна строка записей для дальнейшего помещения их в список
            string[] Record;
            //открыли соеденение значит можно что то делать
            if (OpenConnect())
            {
                //формируем запрос
                OdbcCommand cmdSQL = new OdbcCommand() { Connection = connector, CommandText = SQL };
                try
                {
                    //пробуем выполнить запрос
                    OdbcDataReader reader = cmdSQL.ExecuteReader();
                    //читаем данные из результатов запроса
                    while (reader.Read())
                    {
                        //задаем длинну строки
                        Record = new string[reader.FieldCount];
                        try
                        {
                            //
                            for (int Index = 0; Index < reader.FieldCount; Index++)
                            {
                                //сохраняем запись в массив 
                                Record[Index] = reader.GetString(Index);
                            }
                        }
                        catch 
                        {
                            /*возможен косяк с чтением данных*/
                        }
                        finally
                        {
                            //запоминаем найденные данные
                            ResultRead.Add(Record);
                        }
                    }
                }
                catch
                {
                    //можно будет отдавать ошибки в лог файл но пока нет класса работы с логом допилим позже
                }
                finally
                {
                    //после завершения работы с таблицами отключаемся чтобы не было локов 
                    connector.Close();
                }
            }
            //возвращаем результат
            return ResultRead;
        }
    }
}

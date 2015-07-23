using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace CreateDataReader
{
    class XMLEngine
    {
        //функция поиска максимального значения
        public int MaxID(string TableName, string NameIDcolumn)
        {
            int Max = 1, SelectID;
            if (File.Exists(TableName + ".xml"))
            {
                //загружаем данные из файла
                DataTable tmpLoad = LoadDataTablefromXML(TableName);
                foreach (DataRow Row in tmpLoad.Rows)
                {
                    SelectID = Convert.ToInt32(Row[NameIDcolumn]);

                    if (Max < SelectID) Max = SelectID;
                }
                Max++;
                //возвращаем значение на 1 больше чем нашли 
            }
            return Max;
        }
        //функция выполнения запросов к указанной базе таблице 
        public DataRow[] SelectData(string expresion, string TableName)
        {
            DataRow[] ResultQWERY= null;
            //если таблица сущестует будем что от делать
            if (File.Exists(TableName + ".xml")) 
            {
                //загружаем данные из файла
                DataTable tmpLoad = LoadDataTablefromXML(TableName);
                //если у таблицы есть данные 
                if(tmpLoad.Rows.Count>0)
                //если в условии есть текст то пробуем выбрать данные на основе него 
                    if (expresion != "") 
                    {
                        try
                        {
                            ResultQWERY = tmpLoad.Select(expresion);
                        }
                        catch 
                        {
                        //будет ошибка вернем что нить в лог 
                        }
                    }
                    else
                    {
                        //загружаем список всех строк 
                        ResultQWERY = tmpLoad.Select();
                    }
             }
            return ResultQWERY;
        }

        public bool TableExist(string TableName) 
        {
            if (File.Exists(TableName + ".xml"))
            {
                return true;
            }
            return false;
        }

        public bool ExistRow(string expresion, string TableName) 
        {
            bool Exist = true;
            if (File.Exists(TableName + ".xml"))
            {
                //загружаем данные из файла
                DataTable tmpLoad = LoadDataTablefromXML(TableName);
                 //если у таблицы есть данные 
                if (tmpLoad.Rows.Count > 0) 
                {
                    Exist = false;
                }
            }
            return Exist;
        }

        //Функция создания таблицы
        public DataTable CreateTable(string NameTable, List<DataColumn> collumns)
        {
            //создаем таблицу с указанным именем
            DataTable NewTable = new DataTable(NameTable);
            //перебираем все полученные функцией столбцы и добаляем их в таблицу
            foreach (DataColumn SelectInCollums in collumns)
            {
                NewTable.Columns.Add(SelectInCollums);
            }
            //возвращаем таблицу в виде набора столбцов
            return NewTable;
        }
        //функция записи таблицы в XML формат
        public void SaveDataTableInXML(DataTable TableForSave)
        {
            //читаем имя таблицы
            string Name = TableForSave.TableName;

            //если имя таблицы не пустое то сохраняем данные 
            if (Name != "")
            {
                //добавляем к имени расширение xml 
                Name += ".xml";
                //создаем специальную переменную для преобразования данных из таблицы в XML формат 
                StringWriter writerXML = new StringWriter();
                //получаем данные о таблице и преобразуем их в XML формат 
                TableForSave.WriteXml(writerXML, XmlWriteMode.WriteSchema, true);
                //Сохраняем преобразованные в XML данные в файл
                File.WriteAllText(Name, writerXML.ToString() );
            }
        }
        //Функция чтения таблицы из XML файла по ее миени
        public DataTable LoadDataTablefromXML(string NameTable)
        {
            //результат будем возвращать в виде таблицы
            DataTable ResultReadXML = null; //если не сможем загрузить ничего вернем Null
            //проверяем не пустое ли имя таблицы
            if (NameTable != "")
            {
                //создаем таблицу с указанным именем
                ResultReadXML = new DataTable(NameTable);
                //собираем имя тыблица из названия и расширения
                NameTable += ".xml";
                if (File.Exists(NameTable))
                {
                    //читаем данные из XML файла 
                    var stringReader = new StringReader(File.ReadAllText(NameTable));
                    //загружаем их в таблицу
                    ResultReadXML.ReadXml(stringReader);
                }
            }
            //возвращаме результат 
            return ResultReadXML;
        }
    }
}

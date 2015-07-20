using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Data;

namespace CreateDataReader
{

  

    //класс будет хранить структуры для описания таблиц представлений 
    //Когда сервис прочтет данные он оставит после себя несколько таблиц с описанием найденных данных
    //Данные внутри этих таблиц будет описывать данный класс
    class DataImage
    {
    }

    //класс описания полей справочника
    class SPR 
    {
        
    }


    //класс описания таблицы со списков отчетностей
    class Digest
    {
       
        DataTable 
                  MyDigest = new DataTable("Digest");

        string NameDigestXml = "Digest.xml";

        #region описание столбцов таблици Digest
        void digestInit()
        {
            DataColumn column = new DataColumn();
            //уникальный номер справочника
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            MyDigest.Columns.Add(column);
    
            //имя справочника
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Name";
            MyDigest.Columns.Add(column);

            //дата последнего обновления
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.DateTime");
            column.ColumnName = "LastUpdate";
            MyDigest.Columns.Add(column);

            //путь к справочнику
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "patch";
            MyDigest.Columns.Add(column);

            //путь к справочнику
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Kliko_date";
            MyDigest.Columns.Add(column);
        }
        #endregion

        #region переменные для заполнения таблицы
        //параметры для записи в таблицу
        static string Name, patch;
        static DateTime LastUpdate, Klikodate;
        #endregion 
        #region Функции расчета полей
        //имя отчетности это выход из директории на 2 уровня вверх  d:\Cliko\DATA\CFG\
        static string getDigestName(string patch)
        {
            //получаем родительскую директрию d:\Cliko\DATA\
            DirectoryInfo name = Directory.GetParent(patch);
            //получаем 2 родителя уже у родителя d:\Cliko\
            name = Directory.GetParent(name.Parent.FullName);
            //получаем имя без буквы Cliko
            return name.Name;
        }

        //получаем дату последней записи файла по пути к нему 
        static DateTime getDataUpdate(string patch)
        {
            return File.GetLastWriteTimeUtc(patch);
        }

        //получаем дату последнего редактирования файла kliko
        static DateTime getDataKlico(string patch)
        {
            //получаем родительскую директрию d:\Cliko\DATA\
            DirectoryInfo name = Directory.GetParent(patch);
            //получаем 2 родителя уже у родителя d:\Cliko\
            name = Directory.GetParent(name.Parent.FullName);
            //подставляем имя EXE Файла
            return File.GetLastWriteTimeUtc(name.FullName + @"\KLIKO.EXE");
        }
        #endregion Function
        #region Описание полей  
         //описываем поля справочника (имя справочника) 
        public string DigestName 
        {
            //по требованию возвращаем имя отчетности
            get { return Name; }
            //получаем имя отчетности на основе пути к ней
        }

        //описываем поля справочника (путь к папке с конфигом)
        public string Digestpatch
        {
            get { return patch; }
            set { patch = value; }
        }

        //описываем поля справочника (Дата последнего изменения справочника)
        public DateTime DateUpdate
        {
            get { return LastUpdate; }
        }

        //описываем поля справочника (путь к справочнику)
        public DateTime Klik_odate
        {
            get { return Klikodate; }
        }
    #endregion


        //сохранение данных в таблице 
        void SaveTable() 
        {
            //если есть что записать
            if (MyDigest.Rows.Count > 0)
            {
                //в любом случае фиксим результат в виде файла
                //сохраняем в xml фай таблицу
                System.IO.StringWriter writer = new System.IO.StringWriter();
                //создаем схему документа
                MyDigest.WriteXml(writer, XmlWriteMode.WriteSchema, true);
                //сохраняем данные в виде файла
                File.WriteAllText(NameDigestXml, writer.ToString());
            }
        }

        //добавление отчетности в таблицу если его там нет 
        public void ADDRowDigest() 
        {
           
            
            //заполняем данные на основе пути к файлу отчетности
            LastUpdate = getDataUpdate(patch);
            Klikodate = getDataKlico(patch);
            Name = getDigestName(patch); 
            
             //если файл существует пробуем его загрузить в виде таблицы
            if (File.Exists(NameDigestXml))
            {
                //читаем данные из XML файла 
                var stringReader = new StringReader(File.ReadAllText(NameDigestXml));
                //загружаем их в виде таблици
                MyDigest.ReadXml(stringReader);
                //пробуем найти в таблице отчетность по такому же пути 
                DataRow[] ResultSelect =  MyDigest.Select("patch = '" + patch+"'");
                //проверяем нашли ли мы запись если нашли нужно проверить равна ли она тем параметрам которые мы задали 
                if (ResultSelect.Length > 0)
                {
                    //нужно ли сохранять данные 
                    bool TrySave = false;
                    //хоть найденная запись и будет одна но все же 
                    foreach (DataRow S in ResultSelect)
                    {
                        //есои нашли хоть одно различие в записях сохраняем данные 

                        //если имя не совпадает обновляем его 
                        if (S["Name"] != Name)
                        {
                            S["Name"] = Name;
                            TrySave = true;
                        }
                        //если дата не совпадает обновляем ее
                        if (S["LastUpdate"].ToString() != LastUpdate.ToString())
                        {
                            S["LastUpdate"] = LastUpdate;
                            TrySave = true;
                        }
                        //если дата файла kliko не сопадает то обновляем ее 
                        if (S["Kliko_date"].ToString() != Klik_odate.ToString())
                        {
                            S["Kliko_date"] = Klik_odate;
                            TrySave = true;
                        }


                        if (TrySave)
                        {

                            SaveTable();
                        }
                    }
                }
                else 
                {
                    //добавляем данные в таблицу

                }

            }
            else 
            {
                //создаем таблицу в памяти
                digestInit();
                //заполняем данными 
                DataRow NewRow = MyDigest.NewRow();
                //приставиваем значее будет считаться порядковым номером
                NewRow["ID"] = 1;
                NewRow["Name"]=Name;
                NewRow["LastUpdate"] =LastUpdate;
                NewRow["patch"] = patch;
                NewRow["Kliko_date"] = Klik_odate;
                MyDigest.Rows.Add(NewRow);
                //сохранение данных в таблице 
                SaveTable();
            }

         
        }

    }


    //главный файл конфигурации KLIKO
    class KLIKOCFG
    { 
    
    }

}

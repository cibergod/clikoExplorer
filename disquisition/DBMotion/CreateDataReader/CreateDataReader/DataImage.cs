using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CreateDataReader
{
    #region структура хранения описания столбцов таблици

    #endregion 

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
            set { Name = getDigestName(patch); }
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
            set { LastUpdate = getDataUpdate(patch); }
        }

        //описываем поля справочника (путь к справочнику)
        public DateTime Klik_odate
        {
            get { return Klikodate; }
            set { Klikodate = getDataKlico(patch); }
        }
    #endregion



    }


    //главный файл конфигурации KLIKO
    class KLIKOCFG
    { 
    
    }

}

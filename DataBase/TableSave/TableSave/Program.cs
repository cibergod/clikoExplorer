using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*потключаем класс для чтения записи в XML */
using RWXMLTABLE;
using System.Data;
/*
  фишка в том чтобы передать в таблицу список значений в виде массива 
  а функция должна сама определить типы данных в таблице и преобразовать в них наши данные 
 * для каждого из стобцов
 */
namespace TableSave
{
    class Program
    {
        //обьявляем класс для работы с XML буд от туда брать таблички
        static RWXML XML = new RWXML();
        //табличка для эксперементов
        static DataTable T;
        #region создаем тестовую табличку 
        static DataTable CeateTestTable()
        {
             T = new DataTable("MyTestTable");

            DataColumn column = new DataColumn();
            //числовое поле
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            T.Columns.Add(column);
            //текстове поле
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Name";
            T.Columns.Add(column);
            //дата 
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.DateTime");
            column.ColumnName = "LastUpdate";
            T.Columns.Add(column);
            return T;
        }
        #endregion

        class Converter 
        { 
            struct Resulttip
            {
                public int      _Int32;
                public string   _String;
                public DateTime _DateTime;
            }
            //параметр который нужно преобразовать 
            public string Param;





            //пробуем преобразовать данные в нужный формат 
            public Resulttip GetVal() 
            {
                Resulttip S = new Resulttip() ;

                try
                {
                    S._DateTime = Convert.ToDateTime(Param);
                }
                catch { }

                try 
                {
                    S._Int32 = Convert.ToInt32(Param);
                }
                catch
                { }

                try
                {
                    S._String = Param;
                }
                catch
                { }


                return S;
            }

        }


        static void TableMotion()
        {
             T = XML.LoadDataTablefromXML("MyTestTable");

            //получаес список столбцов
             foreach (DataColumn column in T.Columns) 
             {
                 Type MyType = column.DataType;
                 Console.WriteLine(column.ColumnName + " " + MyType.ToString());
             }

             T = null;
        }


        static void Main(string[] args)
        {
          
            //сохраняем ее в виде XML файла 
            //XML.SaveDataTableInXML(CeateTestTable());
            //TableMotion();

            Converter A = new Converter();

            A.Param = "A";
            Console.WriteLine( A.getMyParam());

            Console.ReadKey();
        }
    }
}

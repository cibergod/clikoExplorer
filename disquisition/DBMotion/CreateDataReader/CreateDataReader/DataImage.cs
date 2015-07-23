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
        //получаем имена таблиц 
        //имя таблицы с формами
        string NameFormTable = "forms";
        //имя таблицы с отчетностями 
        string NameDigestTable = "Digest";
        //имя таблицы списка справочников в формах
        string NameDirectory = "directory";
        //читаем список отчетностей
        XMLEngine ReaderXML = new XMLEngine();
        //класс работы с базой данных 
        ReadDB SQLEngine = new ReadDB();
        
          //функция создания новой таблицы
        DataTable InitFormTable()
        {

            //класс солбца который будем созадвать 
            DataColumn Column;
            //создаем перечень столбцов таблицы содержащей данные о форме 
            List<DataColumn> ListColums = new List<DataColumn>();
            //приводим таблицу к нужному формату
            //уникальный номер записи ключик 

            //IDKLIKOFRORM Уникальный номер отчетности
            Column = new DataColumn()
            {
                ColumnName = "IDKLIKOFRORM",
                DataType = System.Type.GetType("System.Int32")
            };
            //добавляем его в список 
            ListColums.Add(Column);
            //IDS   - уникальный номер справочника
            Column = new DataColumn()
            {
                ColumnName = "IDS",
                DataType = System.Type.GetType("System.Int32")
            };
            //добавляем его в список 
            ListColums.Add(Column);
            //SNAME - название справочника
            Column = new DataColumn()
            {
                ColumnName = "SNAME",
                DataType = System.Type.GetType("System.String")
            };
            //добавляем его в список 
            ListColums.Add(Column);

            //FNAME (SSQLD)- Имя файла справочника
            Column = new DataColumn()
            {
                ColumnName = "FNAME",
                DataType = System.Type.GetType("System.String")
            };
            //добавляем его в список 
            ListColums.Add(Column);

            //FUPDATE (SFDBF) имя файла для обновления справочника
            Column = new DataColumn()
            {
                ColumnName = "FUPDATE",
                DataType = System.Type.GetType("System.String")
            };
            //добавляем его в список 
            ListColums.Add(Column);
            //создаем таблицу directory
            return ReaderXML.CreateTable(NameDirectory, ListColums);


        }
        //убрать непечатаемые символы из строки с названием 
        static string CutChar(string Name)
        {
            string buf = "";
            foreach (char S in Name)
            {
                if ((S != '\n') && (S != '\r')) buf += S;
            }
            return buf;
        }
        //получить из SQL текста последнее слово это будет имя файла со справочником 
        static string ShrinkSQL(string name)
        {
            //режим строку пробелами 
            string[] M = name.Split(' ');
            //получаем длинну строки
            int S = M.Length;
            //расчитываем позицию последнего слова
            S--;
            //собираем результат
            string result = CutChar(M[S]);
            //прибавляем расширение к файлу 
            return result + ".db";
        }
        //получим чистый адресс без имени файла
        string GetPatchDir(string P)
        {
            if (P != "")
            {
                string CurrentDir = Directory.GetParent(P).FullName;
                //возвращаем путь к папке а не к файлу
                return Directory.GetParent(CurrentDir).FullName;
            }
            else return null;
        }

        DataRow CreateRow(DataTable T, DataRow R, String IDkliko) 
        {
            //создаем новую запись в таблице 
            DataRow NR = T.NewRow();
            //заполняем уникальный номер отчетности
            NR["IDKLIKOFRORM"] = Convert.ToInt32(IDkliko);
            //уникальный номер справочника
            NR["IDS"]          = Convert.ToInt32(R["IDS"]);
            //SNAME - название справочника
            NR["SNAME"] = CutChar(R["SNAME"].ToString());
            //FNAME (SSQLD)- Имя файла справочника
            NR["FNAME"] = ShrinkSQL(R["SSQLD"].ToString());
            //FUPDATE (SFDBF) имя файла для обновления справочника
            NR["FUPDATE"] = R["SFDBF"].ToString();
            return NR;
        }


        //добавление данных в таблицу справочников 
        void AddROW(DataRow NewRow, string IDkliko)
        {
           if(ReaderXML.TableExist(NameDirectory))
           {

           }else
           {
                //создаем нову таблицу
                DataTable DIR =  InitFormTable();
                //создаем строку для добавления в таблицу
                DataRow M = CreateRow(DIR, NewRow, IDkliko);
               //добавляем запись в табличку
                DIR.Rows.Add(M);
               //Сохраняем табличку 
                ReaderXML.SaveDataTableInXML(DIR);
           }
        
        }
        
        //передаем путь получаем список справочников 
        void DirectoryList(string patch, string IDkliko) 
        {
            try
            {
            //получаем список директорий в папке
            string[] D = Directory.GetDirectories(patch);
            foreach (string nameDir in D)
            {

                DirectoryInfo MyDir = new DirectoryInfo(nameDir);
                //если 8 символов значит наша дата 
                if (MyDir.Name.Length == 8)
                {
                    try //пробуем получить цифры
                    {
                        int Data_folder = Convert.ToInt32(MyDir.Name);
                        //Console.WriteLine(Data_folder);
                        //если прочитали данные пробуем прочитать справочники
                        SQLEngine.patch = nameDir;
                        //читать будем следующие значения 
                        /*
                         IDS   - уникальный номер справочника 
                         SNAME - название справочника
                         SSQLD - SQL из которого возьмем имя файла справочника
                         SFDBF - имя файла для обновления справочника
                         */
                        SQLEngine.SQL = "SELECT IDS, SNAME, SSQLD, SFDBF FROM rsprav";
                        //запускаем запрос 
                        SQLEngine.getSQL();
                        //читаем результат
                        foreach (DataRow ResultQWERY in SQLEngine.Result.Rows) 
                        {
                            //пробуем добавить справочник в таблицу
                            AddROW(ResultQWERY, IDkliko);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            }
            catch
            {
            }
                
               
            
        }
        public void GetDirectory() 
        {
            string IDDigest, 
                   PatchDigest, 
                   PathDirectory;
            //загружаем таблицу справочников 
            DataTable _Digest = ReaderXML.LoadDataTablefromXML(NameDigestTable);

                   
            //перебираем справочники по одному 
            foreach (DataRow DR in _Digest.Rows) 
            {
                //получаем номер справочника 
                IDDigest    = DR["ID"].ToString();
                //получаем путь к справочнику 
                PatchDigest = DR["patch"].ToString();
                //получим чистый адресс без имени файла
                PatchDigest = GetPatchDir(PatchDigest);
                //находим список форм по ID справочника 
                DataRow[] FindForms = ReaderXML.SelectData("KLIKOID=" + IDDigest, NameFormTable);
                //перебираем результаты поиска
                foreach (DataRow R in FindForms) 
                {   
                    //получаем путь по которому нужно прочитать справочники
                    PathDirectory = PatchDigest + @"\" + R["SNAME"];
                    //Console.WriteLine(PathDirectory);
                    //дальше по данному пути получаем список уникальных справочников 
                    DirectoryList(PathDirectory, IDDigest);
                }
            }

        }

    }
    //класс описания таблицы списков отчетностей
    class Digest
    {
        static DataTable MyDigest = new DataTable("Digest");
        public static string NameDigestXml = "Digest.xml";
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
            try
            {
                //получаем родительскую директрию d:\Cliko\DATA\
                DirectoryInfo name = Directory.GetParent(patch);
                //получаем 2 родителя уже у родителя d:\Cliko\
                name = Directory.GetParent(name.Parent.FullName);
                //получаем имя без буквы Cliko
                return name.Name;
            }
            catch 
            {
                return null;
            }
        }
        //получаем дату последней записи файла по пути к нему 
        static DateTime getDataUpdate(string patch)
        {
            try
            {
            return File.GetLastWriteTimeUtc(patch);
            }
            catch
            {

            }
            //обнуляем дату 
            return new DateTime();
        }
        //получаем дату последнего редактирования файла kliko
        static DateTime getDataKlico(string patch)
        {
            try
            {
                //получаем родительскую директрию d:\Cliko\DATA\
                DirectoryInfo name = Directory.GetParent(patch);
                //получаем 2 родителя уже у родителя d:\Cliko\
                name = Directory.GetParent(name.Parent.FullName);
                //подставляем имя EXE Файла
                return File.GetLastWriteTimeUtc(name.FullName + @"\KLIKO.EXE");
            }
            catch 
            {
 
            }
            //обнуляем дату 
            return new DateTime();
        }
        #endregion Function
        #region Описание полей  
         //описываем поля справочника (имя справочника) 
        public  string DigestName 
        {
            //по требованию возвращаем имя отчетности
            get { return Name;  }
           
            //получаем имя отчетности на основе пути к ней
        }
        //описываем поля справочника (путь к папке с конфигом)
        public  string Digestpatch
        {
            get { return patch; }
            set { patch = value; }
        }
        //описываем поля справочника (Дата последнего изменения справочника)
        public  DateTime DateUpdate
        {
            get { return LastUpdate; }
      
        }
        //описываем поля справочника (путь к справочнику)
        public static DateTime Klik_odate
        {
            get { return Klikodate; }
           
        }
    #endregion
        #region методы сохранения данных в таблицу
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
        //функция поиска максимального значения
        int SearchMaxID()
        {
            int Max = 0, SelectID;

            foreach (DataRow Row in MyDigest.Rows) 
            {
                SelectID = Convert.ToInt32(Row["ID"]);

                if (Max < SelectID) Max = SelectID;
            }
            Max++;
            //возвращаем значение на 1 больше чем нашли 
            return Max;
        }
        static void loadTable() 
        {
            MyDigest = new DataTable("Digest");
            //читаем данные из XML файла 
            var stringReader = new StringReader(File.ReadAllText(NameDigestXml));
            //загружаем их в виде таблици
            MyDigest.ReadXml(stringReader);
        }
        //добавление отчетности в таблицу если его там нет 
        public void ADDRowDigest()
        {
            if (File.Exists(patch))
            {
                //заполняем данные на основе пути к файлу отчетности
                LastUpdate = getDataUpdate(patch);
                Klikodate = getDataKlico(patch);
                Name = getDigestName(patch);
                //если удалось получить имя папки значит это Kliko  а не какято шляпа на моем диске с файлом
                if (Name != null)
                {
                    //если файл существует пробуем его загрузить в виде таблицы
                    if (File.Exists(NameDigestXml))
                    {
                        loadTable();
                        //пробуем найти в таблице отчетность по такому же пути 
                        DataRow[] ResultSelect = MyDigest.Select("patch = '" + patch + "'");
                        //проверяем нашли ли мы запись если нашли нужно проверить равна ли она тем параметрам которые мы задали 
                        if (ResultSelect.Length > 0)
                        {
                            //нужно ли сохранять данные 
                            bool TrySave = false;
                            string N;
                            DateTime L, K;
                            //хоть найденная запись и будет одна но все же 
                            foreach (DataRow S in ResultSelect)
                            {

                                N = (string)S["Name"];
                                L = Convert.ToDateTime(S["LastUpdate"]);
                                K = Convert.ToDateTime(S["Kliko_date"]);

                                if ((N != Name) || (L != LastUpdate) || (K != Klik_odate))
                                {
                                    //если хоть одно из полей не совпадает то обновляем запись 
                                    S["Name"] = Name;
                                    S["LastUpdate"] = LastUpdate;
                                    S["Kliko_date"] = Klik_odate;
                                    TrySave = true;
                                }

                            }
                            //если есть отличия в данных то пересохраняем их 
                            //сохранение данных в таблице 
                            if (TrySave) SaveTable();
                        }
                        else
                        {

                            DataRow NewRow = MyDigest.NewRow();
                            //приставиваем значее будет считаться порядковым номером
                            NewRow["ID"] = SearchMaxID();
                            NewRow["Name"] = Name;
                            NewRow["LastUpdate"] = LastUpdate;
                            NewRow["patch"] = patch;
                            NewRow["Kliko_date"] = Klik_odate;
                            MyDigest.Rows.Add(NewRow);
                            //сохранение данных в таблице 
                            SaveTable();
                        }

                    }
                    else
                    {
                        //создаем таблицу в памяти
                        digestInit();
                        //заполняем данными 
                        DataRow NewRow = MyDigest.NewRow();
                        //приставиваем значее будет считаться порядковым номером
                        NewRow["ID"] = SearchMaxID();
                        NewRow["Name"] = Name;
                        NewRow["LastUpdate"] = LastUpdate;
                        NewRow["patch"] = patch;
                        NewRow["Kliko_date"] = Klik_odate;
                        MyDigest.Rows.Add(NewRow);
                        //сохранение данных в таблице 
                        SaveTable();
                    }


                }
            }
            
        }
        #endregion
        #region получаем ID справочника по его пути 
        public int GetID() 
        {
            //если ничего не нашли то вернем отрицательное число 
            int ID = -1;
            loadTable();
            //пробуем найти в таблице отчетность по такому же пути 
            DataRow[] ResultSelect = MyDigest.Select("patch = '" + patch + "'");
            if (ResultSelect.Length > 0)
            {
                //хоть найденная запись и будет одна но все же 
                foreach (DataRow S in ResultSelect)
                {
                    //находим первый попавшийся ID для данного справочника
                    ID = Convert.ToInt32(S["ID"]);
                    break;
                }
            }
            return ID;
        }
        #endregion
    }
    //список форм из Kliko файла настроек
    class FORMSKLIKO
    {
        //преобразование неверной кодировки в нормальные символы пока в 2 местах 
        static string ConvertToUtf(string source)
        {
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1252");
            byte[] originalByteString = srcEncodingFormat.GetBytes(source);
            return Encoding.Default.GetString(originalByteString);
        }
        //имя таблицы с формами
        string NameFormTable = "forms";
        //имя таблицы с отчетностями 
        string NameDigestTable = "Digest";
        //читаем список отчетностей
        XMLEngine ReaderXML = new XMLEngine();
        //получим чистый адресс без имени файла
        string GetPatchDir(string P)
        {
            if (P != "")
            {
                //возвращаем путь к папке а не к файлу
                return Directory.GetParent(P).FullName;
            }
            else return null;
        }
        DataTable GetFormSList(string ID, string Patch) 
        {
            /*собираем запрос для получения списка форм
           KLIKOID    Уникальный номер отчетности
           IDR        Идентификатор справочника
           NAME       Полное название формы 
           SNAME      Имя папки где он лежит
           RCLASS     Возможно название отчетности
           BEGDATE    Дата начала использования */
            string QWERY = @" SELECT " +ID + @" as KLIKOID, IDR , NAME, SNAME, RCLASS, BEGDATE FROM  KLIKOCFG";
            //потключаем класс работы с Базами данных 
            ReadDB DB = new ReadDB();
            //задаем путь где лежит база данных 
            DB.patch = Patch;
            //передаем параметры запроса 
            DB.SQL = QWERY;
            //запускаем запрос 
            DB.getSQL();
            //получаем результат в виде таблицы данных 
            DataTable R = DB.Result;

            return R;
        }
        //функция создания новой таблицы
        DataTable InitFormTable() 
        {

            //класс солбца который будем созадвать 
            DataColumn Column;
            //создаем перечень столбцов таблицы содержащей данные о форме 
            List<DataColumn> ListColums = new List<DataColumn>();
            //приводим таблицу к нужному формату
            //уникальный номер записи ключик 

            //IDKLIKOFRORM Уникальный номер отчетности
            Column = new DataColumn()
            {
                ColumnName = "IDKLIKOFRORM",
                DataType = System.Type.GetType("System.Int32")
            };
            //добавляем его в список 
            ListColums.Add(Column);


            //KLIKOID Уникальный номер отчетности
                Column = new DataColumn()
                {
                    ColumnName="KLIKOID", DataType= System.Type.GetType("System.Int32")
                };
            //добавляем его в список 
                ListColums.Add(Column);

            //IDR Идентификатор справочника
                Column = new DataColumn()
                {
                    ColumnName = "IDR",
                    DataType = System.Type.GetType("System.Int32")
                };
           //добавляем его в список 
                ListColums.Add(Column);
           
            
            //NAME Полное название формы 
                Column = new DataColumn()
                {
                    ColumnName = "NAME",
                    DataType = System.Type.GetType("System.String")
                };
                //добавляем его в список 
                ListColums.Add(Column);
            
             //SNAME Имя папки где он лежит
                Column = new DataColumn()
                {
                    ColumnName = "SNAME",
                    DataType = System.Type.GetType("System.String")
                };
                //добавляем его в список 
                ListColums.Add(Column);

            //RCLASS Возможно название отчетности
                Column = new DataColumn()
                {
                    ColumnName = "RCLASS",
                    DataType = System.Type.GetType("System.String")
                };
                //добавляем его в список 
                ListColums.Add(Column);
            
            //BEGDATE Дата начала использования 
                Column = new DataColumn()
                {
                    ColumnName = "BEGDATE",
                    DataType = System.Type.GetType("System.DateTime")
                };
                //добавляем его в список 
                ListColums.Add(Column);
                //создаем таблицу Form
                return ReaderXML.CreateTable(NameFormTable, ListColums);
            //
        }
        //вобщем есть траблы у некоторых форм с кодировкой поэтому добавим их в список исключений
        //потом вынесем в конфиг 
        int[] DecodeList = { 7, 17 };
        string GetCurrentName(string name, int ID)
        { 
            //ищем в списке форм с плохой кодировкой 
            foreach (int I in DecodeList) 
            {
                //если нашли возвращаем перобразованное имя 
                if (ID == I) 
                {
                    return ConvertToUtf(name);
                }
            }
            //если не нашли то возвращаем простое имя 
            return name;
        }
        //функция создания 1 записи для добавления ее в таблицу
        DataRow AddRow(DataTable D, DataRow Rows) 
        {
            int ID = Convert.ToInt32(Rows["KLIKOID"]);
            //загрузим в нее данные из столбцов
            DataRow FormRows = D.NewRow();
            //IDKLIKOFRORM Уникальный номер отчетности
            FormRows["IDKLIKOFRORM"] = ReaderXML.MaxID(NameFormTable, "IDKLIKOFRORM"); 
            // KLIKOID    Уникальный номер отчетности
            FormRows["KLIKOID"] = ID;
            //IDR        Идентификатор справочника
            FormRows["IDR"]     = Convert.ToInt32(    Rows["IDR"]);
            //NAME       Полное название формы 
            FormRows["NAME"] = GetCurrentName(Rows["NAME"].ToString(), ID);
            //SNAME      Имя папки где он лежит
            FormRows["SNAME"]   = Convert.ToString(   Rows["SNAME"]);
            //RCLASS     Возможно название отчетности
            FormRows["RCLASS"]  = Convert.ToString(   Rows["RCLASS"]);
            //BEGDATE    Дата начала использования 
            FormRows["BEGDATE"] = Convert.ToDateTime( Rows["BEGDATE"]);
            //возвращаем данные в нужном формате для записи их а таблицу
            return FormRows;
        }
        //добавление неповторяющихся записей в таблицу
        void AddroowinTable(DataRow Rows) 
        {
            //если не существует то будем создадим таблицу
            DataTable D;
            //проверяем существует ли таблица с формами 
            if (File.Exists(NameFormTable + ".xml"))
            {
                //условия отбора форм для добавления
                string expression = "KLIKOID = " + Rows["KLIKOID"].ToString() + " and " + " IDR = " + Rows["IDR"].ToString();
                //ура таблиа существует тогда загружаем ее из файла
                D = ReaderXML.LoadDataTablefromXML(NameFormTable);
                //ищем совпадения данных 
                DataRow[] Result= D.Select(expression);
                //если не нашли запись в таблице то добавляем ее 
                if (Result.Length == 0) 
                {
                    //добавляем запись в таблицу
                    D.Rows.Add(AddRow(D, Rows));
                    //сохраняем таблицу в файл 
                    ReaderXML.SaveDataTableInXML(D);
                }
                //иначе просто будем пропускать ее мимо ушей 

            }
            else 
            {
                //если не существует то будем создадим таблицу
                 D =   InitFormTable();
                //добавляем запись в таблицу
                D.Rows.Add(AddRow(D, Rows));
                //сохраняем таблицу в файл 
                ReaderXML.SaveDataTableInXML(D);
            }

        }
        public void GetFormList()
        {
           
            //пробуем загрузить данные из таблицы
            DataTable DigestTable = ReaderXML.LoadDataTablefromXML(NameDigestTable);
            //из таблицы нам понадабяться 2 параметра 
            string ID_Kliko, patch_kliko;
            
            //если загрузка удалась пробуем прочитать данные 
            if (DigestTable != null) 
            {
                //нам нужны ID отчетности и путь к файлу бызы данных 
                foreach (DataRow R in DigestTable.Rows) 
                {
                    //получаем номер kliko 
                    ID_Kliko    = R["ID"].ToString(); 
                    // получаем путь к kliko 
                    patch_kliko = GetPatchDir (R["patch"].ToString());
                    //получаем результат поиска данных из Kliko 
                    DataTable FormListThisKliko = GetFormSList(ID_Kliko, patch_kliko);

                    //если данные найдены нужно проверит есть ли они у нас или нет в базе
                    if (FormListThisKliko != null) 
                    {
                      //  Console.WriteLine("найдена отчетность");
                      //  Console.WriteLine(R["Name"].ToString());
                        foreach (DataRow S in FormListThisKliko.Rows) 
                        {
                            //Console.WriteLine("формы " + S["NAME"].ToString());

                            AddroowinTable(S);
                        }
                    }

                }

            }

        }

    }
}

 



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

        DataRow CreateRow(DataTable T, DataRow R) 
        {
            //создаем новую запись в таблице 
            DataRow NR = T.NewRow();
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
        void AddROW(DataRow NewRow)
        {
             DataTable DIR;
           if(ReaderXML.TableExist(NameDirectory))
           {
               //если файл существует то загружаем таблицу
               DIR = ReaderXML.LoadDataTablefromXML(NameDirectory);
               //добавляем уникальные записи к существующим в таблице
               if (ReaderXML.ExistRow("IDS='" + NewRow["IDS"].ToString() + "'", NameDirectory))
               {
                   //создаем строку для добавления в таблицу
                   DataRow M = CreateRow(DIR, NewRow);
                   //добавляем запись в табличку
                   DIR.Rows.Add(M);
                   //Сохраняем табличку 
                   ReaderXML.SaveDataTableInXML(DIR);
                   Console.Write(" добавлен");
               }
               else Console.Write(" уже есть");

           }else
           {
                //создаем нову таблицу
               DIR = InitFormTable();
                //создаем строку для добавления в таблицу
                DataRow M = CreateRow(DIR, NewRow);
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
                            Console.WriteLine("добавляем "+ ResultQWERY["SNAME"]);

                            //пробуем добавить справочник в таблицу
                            AddROW(ResultQWERY);
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
                    PathDirectory = PatchDigest + @"\" + R["SNAME"]+"\\";
                    //Console.WriteLine(PathDirectory);
                    //дальше по данному пути получаем список уникальных справочников 
                    DirectoryList(PathDirectory, IDDigest);
                }
            }

        }

    }
   
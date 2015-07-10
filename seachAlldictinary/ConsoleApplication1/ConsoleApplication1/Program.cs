using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;

using System.Threading;
using System.Xml;
//windows будет брать данные сирализуя их из таблиц 
//данный класс будет работать с таблицами
namespace ConsoleApplication1
{
    class Program
    {



       static DataTable digest = new DataTable();
        static void digestInit()
        {
            //имя справочника
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Name";
            digest.Columns.Add(column);

            //дата последнего обновления
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.DateTime");
            column.ColumnName = "LastUpdate";
            digest.Columns.Add(column);

            //путь к справочнику
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "patch";
            digest.Columns.Add(column);

        }

        struct Result
        {
          public  System.String name, patch;
          public System.DateTime T;

        };

         class search 
        {
            

             public string P, F;
             public List<Result> ROW = new List<Result>();

            static string getDigestName(string patch)
            {
                DirectoryInfo name = Directory.GetParent(patch);
                name = Directory.GetParent(name.Parent.FullName);
                return name.Name;
            }

            static DateTime getDataUpdate(string patch)
            {
                return File.GetLastWriteTimeUtc(patch);
            }

           
             public void GetSearch()
            {
               
                try
                {
                    string[] S = Directory.GetFiles(P, F, SearchOption.AllDirectories);

                    if (S.Length > 0)
                    {

                        foreach (string L in S)
                        {
                            Result R;
                            R.name = getDigestName(L);
                            R.patch = L;
                            R.T = getDataUpdate(L);

                            ROW.Add(R);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
              
            }

        }

         struct Potok 
         {
           public   search Sercher;
           public Thread PotockSearch;
         }

       static  bool Finished(List<Potok> Gde) 
         {
             Console.Clear();
             bool mov = false;
             foreach (Potok tut in Gde) 
             {
                 if (tut.PotockSearch.ThreadState == ThreadState.Running) 
                 {
                     mov = true;
                     Console.WriteLine(tut.Sercher.P);
                 }
             }

             return mov;
         }


      static   void Main(string[] args)
        {
            List<Potok> K = new List<Potok>();    

            digestInit();

            string Filename="KLIKOCFG.DB";
            string patch = @"D:\";

            string[] PatchDir = Directory.GetDirectories(patch);
            try
            {
                foreach (string P in PatchDir)
                {
                    Potok  T;
                    T.Sercher = new search()
                    {
                        P=P, F=Filename
                    };
                    T.PotockSearch = new Thread(T.Sercher.GetSearch);
                    T.PotockSearch.Start();
                    K.Add(T);


                }
            }catch
            {
 
            }

            while (Finished(K)) 
            {
                Thread.Sleep(50);
            }
            //значит все нашли собираем таблицу и потом ее сохраним в файлиг 
            foreach (Potok P in K)
            {
                foreach (Result R in P.Sercher.ROW) 
                {
                    DataRow Z= digest.NewRow();
                    Z["Name"] = R.name;
                    Z["patch"] = R.patch;
                    Z["LastUpdate"] = R.T;
                    digest.Rows.Add(Z);
                }
            }

               
            Console.ReadKey();

        }
    }
}

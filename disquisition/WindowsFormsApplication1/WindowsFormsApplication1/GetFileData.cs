using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication1
{
    public struct Listdigest
    {
        public string name, DirPatch;
    }
    //чтение директории в отдельном потоке требует создания отдельного класса
    class ReadDir
    {
        public List<Listdigest> MyListdigest = new List<Listdigest>();
        public string Patch, filename;

        public void SearchInDirectory()
        {
            string F = filename, D = Patch;

            try
            {
                //пробуем найти  файл в директориях
                string[] Result = Directory.GetFileSystemEntries(D, F, SearchOption.AllDirectories);
                foreach (string R in Result)
                {
                    //убираем имя файла из пути
                    DirectoryInfo I = Directory.GetParent(R);
                    I = Directory.GetParent(I.ToString());
                    I = Directory.GetParent(I.ToString());
                    //имя нашей отчетности

                    DirectoryInfo P = Directory.GetParent(R);
                    //Console.WriteLine(I.Name + "\n");//\t" + " V " + R);
                    Listdigest NewDigestSearch;
                    //заполняем структуру 
                    NewDigestSearch.name = I.Name;
                    NewDigestSearch.DirPatch = P.FullName;
                    //добавляем в список данные 
                    MyListdigest.Add(NewDigestSearch);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(" " + e.Message);
            }
        }
    }


    class GetFileData
    {
      
        public struct SearchList
        {
            public ReadDir F;
            public Thread S;
        }


       


    }
}

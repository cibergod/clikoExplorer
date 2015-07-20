using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.IO;

namespace ConsoleApplication2
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
    //класс для работы с файловой системой 
    class FileSystem
    {
       
        //структура для хранения списка результатов в файле.
        public List<Listdigest> MyListdigestResult = new List<Listdigest>();
        //струкура для хранения текущих потоков и результатов поиска
        public struct SearchList
        {
            public ReadDir F;
            public Thread S;
        }
        //
        public List<SearchList> M = new List<SearchList>();

        public void Find(string patch, string FileName)
        {

            string[] DirList = Directory.GetDirectories(patch);

            //  Console.Write("найдено папок " + DirList.Length + " \n");
            int I = 0;
            foreach (string Dirs in DirList)
            {
                SearchList S;
                S.F = new ReadDir()
                {
                    filename = FileName,
                    Patch = Dirs
                };

                S.S = new Thread(S.F.SearchInDirectory);
                M.Add(S);
                S.S.Start();

                I++;
            }

        }

        //пока хоть один поток работает нельзя продолжать работу с файлами поэтому ждем 
        public bool SearchFinish()
        {
            Console.Clear();
            bool mov = false;
            int I = 0;
            foreach (SearchList J in M)
            {
                if (J.S.ThreadState == ThreadState.Running)
                {
                    mov = true;
                    Console.WriteLine(I + ") ищем на " + J.F.Patch);
                    I++;
                }

            }

            if (!mov) 
            {
                //агрегируем результаты в виде одного общего списка с которым будем дальее работать
                foreach (SearchList J in M) MyListdigestResult.AddRange(J.F.MyListdigest);
            }

            return mov;
        }

    }
}

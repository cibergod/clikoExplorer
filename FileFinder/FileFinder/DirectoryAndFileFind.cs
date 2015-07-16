using System;
using System.Collections.Generic;
using System.Text;

namespace FileFinder
{
     class DirectoryAndFileFind
    {
        //структура описания входных параметров
      public  struct inputinfo
        {
            //от куда начинать поиск
            public string StartPatchFilder;
            //если ищем директорию то передаем ее название
            public string SearchFolder;
            //если ищем файл то передаем его название
            public string SearchFile;
        };
        
        //структура для хранения результатов поиска 
      public  struct OutputInfo
        {
            //имя папки в которой найден файл
            public string NameFolder;
            //полный путь к файлу
            public string FullPatchFilder;
            //дата последнего обновления файла в папке
            public DateTime DateFile;
        };
        //структура  для хранения информации о текущем поиске файла/ папки
        struct PotokSearch 
        {
            //класс поиска в текущей папке
            public Finder FindCurrentFolder;
            //класс потока поиска текущей папке
            public System.Threading.Thread PotokSearchs;
        }

        //класс поиска файла или каталога в папке
       public class Finder 
        {
            //в класс будем передавать структуру inputinfo 
            public inputinfo MyInputParam;
            //И ждать от него результат в виде списка найденных значений
            public List<OutputInfo> OutputData = new List<OutputInfo>();

            //функция проверки завершения поиска в дочерних папках 
            bool TestFinsFinish(List<PotokSearch> FinderList) //принимаем на входе список потоков поиска
            {
                //по умолчанию думаем что все работа закончена
                bool Result = false;  //устанавливаем ложь
                foreach (PotokSearch SelectPotok in FinderList) 
                {
                    //если находим хоть один запущенный поток то ждем останавливаем поиск 
                    if (SelectPotok.PotokSearchs.ThreadState == System.Threading.ThreadState.Running) 
                    {
                        //возвращаем правду что поиск еще работает 
                        Result = true;
                        Console.WriteLine(SelectPotok.FindCurrentFolder.MyInputParam.StartPatchFilder);
                    }
                }
                //возвращаем результат работы функции
                return Result;
            }
            //сама функция поиска файлов и каталогов
            public void find()
            {
                Console.WriteLine(MyInputParam.StartPatchFilder);
                //если мы ищем файл то выполняем команду getFile
                if ((MyInputParam.SearchFile != "") &&(MyInputParam.SearchFile!=null))
                {
                    //находим все файлы с таким именем в указанной папке
                    string[] FindFiles = System.IO.Directory.GetFiles(MyInputParam.StartPatchFilder ,MyInputParam.SearchFile);
                    
                    //перебираем результат поиска файлов
                    foreach(string F in FindFiles)
                    {
                        //результат будем помещать в структуру OutputInfo
                        OutputInfo Result;

                        //получаем путь к файлу
                        System.IO.DirectoryInfo FullPatch = System.IO.Directory.GetParent(F);
                        //записываем путь к файлу
                        Result.FullPatchFilder = FullPatch.FullName;
                    
                        //получаем дату изменения файла
                        Result.DateFile = System.IO.File.GetLastWriteTimeUtc(F);

                        //получаем название папки где нашли файл
                        Result.NameFolder = FullPatch.Name;
                        //добавляем результат в список 
                        OutputData.Add(Result);
                    }
                }

                //если мы ищем директорию (папку) то выполняем команду getDirectory
                if ((MyInputParam.SearchFolder!="")&&( MyInputParam.SearchFolder !=null))
                {
                    //находим имена всех директорий подъодящих под наш поиск
                    string[] FindFolder = System.IO.Directory.GetDirectories(MyInputParam.StartPatchFilder, MyInputParam.SearchFolder);
                     //перебираем результат поиска файлов
                    foreach (string D in FindFolder)
                    {
                        //результат будем помещать в струкруту OutputInfo
                        OutputInfo Result;

                        //получаем путь к директории
                        System.IO.DirectoryInfo FullPatch = System.IO.Directory.GetParent(D);

                        //записываем путь к директории
                        Result.FullPatchFilder = FullPatch.FullName;

                        //получаем дату изменения директории
                        Result.DateFile = FullPatch.LastWriteTimeUtc;

                        //получаем название папки 
                        Result.NameFolder = FullPatch.Name;
                        //добавляем результат в список 
                        OutputData.Add(Result);
                    }
                }
                //Объявляем список структур PotokSearch для хранения в них результатов поиска 
                List<PotokSearch> FinderList = new List<PotokSearch>();
                if (MyInputParam.StartPatchFilder != null)
                {
                    //находим все директории по указанному пути
                    string[] DirectroyList = System.IO.Directory.GetDirectories(MyInputParam.StartPatchFilder);
                    //перебираем каждую из найденных директорий в цикле
                    foreach (string SelectDir in DirectroyList)
                    {
                        Console.WriteLine(SelectDir);

                        PotokSearch Task;
                        //обьявляем новый элемент класса DirectoryAndFileFind
                        Task.FindCurrentFolder = new Finder(); //обьявляем класс для поиска функции
                        //переменная для задания параметров поиска заполняем ее текущими значениями
                        inputinfo InputParam = MyInputParam;
                        //меняем параметр выбранной директории
                        InputParam.StartPatchFilder = SelectDir;
                        //создаем новый поток 
                        Task.PotokSearchs = new System.Threading.Thread(Task.FindCurrentFolder.find);//передаем фунцияю котора должна работать в потоке
                        //запускаем поиск файлов
                        Task.PotokSearchs.Start();
                        //добавляем заполненную структуру в список 
                        FinderList.Add(Task);
                    }
                    //ждем завершения поиска в потоках
                    while (TestFinsFinish(FinderList)) ;
                    //собираем результаты поиска в единый массив
                    foreach (PotokSearch SelectPotok in FinderList)//перебираем все созданные потоки
                    {
                        //читаем список найденных результатов
                        foreach (OutputInfo R in SelectPotok.FindCurrentFolder.OutputData)
                        {
                            //записываем их в текущем классе в результат
                            OutputData.Add(R);
                        }
                    }
                }
            }
        }
    }
}

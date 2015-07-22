using System;
using System.Collections.Generic;
using System.Text;

//библиотеки работы с файловой системой
using System.IO;
//делаем много потоков для ускорения чтениял данных из директорий
using System.Threading;

//класс для поиска данных в файловой системе 
namespace CreateDataReader
{
    #region класс поиска файла в указанной директории
    class Searchkliko 
    {
        //путь где изначально запустим поиск баз Kliko
        public string StartPatch=@"d:\";
        //имя файла конфигурации Kliko
        public string Filename = "KLIKOCFG.DB";
        //результат найденные пути к клико  
        public List<string> SerchResult = new List<string>();

        public void GetSearch()
        {
            try
            {
                //ищем файл в указанной директории и ее вложениях 
                string[] S = Directory.GetFiles(StartPatch, Filename, SearchOption.AllDirectories);
                //если есть результаты
                if (S.Length > 0)
                {
                    //перебираем их 
                    foreach (string Patch in S) 
                    {
                        //сохраняем результаты в список 
                        SerchResult.Add(Patch);
                    }
                }
            }
            catch 
            {
                //Допустим ошибка нет доступа к файлу 
            }
        }
    }
    #endregion
    #region Класс многопоточного поиска в директории
    class Finder
    {
        
        //структура для хранения процессов 
        struct Potok
        {
            public Searchkliko Sercher;
            public Thread PotockSearch;
        };
        //список запущенных заданий на поиск 
        List<Potok> TaskList = new List<Potok>();
        //путь где изначально запустим поиск баз Kliko
        public string StartPatch = @"d:\";

        //функция проверки завершения всех задач
        bool Finished()
        {
            //Console.Clear();
            //Console.WriteLine("Поиск в папках ");
            //по умолчанию считаем что все задачи завершены
            bool mov = false;
            //Перебираем задачи 
            foreach (Potok SelectTask in TaskList)
            {
                //если нашли запущенный процесс
                if (SelectTask.PotockSearch.ThreadState == ThreadState.Running)
                {
                    //выведем лог че делаеться 
                   // Console.WriteLine(SelectTask.Sercher.StartPatch);
                    //возвращаем правду и останавливаем поиск 
                    mov = true;
                    break;
                }
            }
            //возвращаем результат работы программы
            return mov;
        }

        //запускаем инкременый поиск в папках 
        public void gogle() 
        {
            string[] PatchDir = Directory.GetDirectories(StartPatch);
            try
            {
                foreach(string MyPatch in PatchDir)
                {
                    //создаем задачу 
                    Potok ForStartTask;
                    //создаем новый обьект класса Sercher
                    ForStartTask.Sercher = new Searchkliko()
                    {
                        //передаем в него путь где нужно искать Kliko
                        StartPatch = MyPatch
                    };
                    //задаем функцию которую будем выполнять в отдельном потоке
                    ForStartTask.PotockSearch = new Thread(ForStartTask.Sercher.GetSearch);
                    //добавляем данный поток в список для поиска 
                    TaskList.Add(ForStartTask);
                    //запускаем поиск 
                    ForStartTask.PotockSearch.Start();
                }
            }
            catch 
            { 
            }
            //Итак ждем завершения поиска всех данных 
            while (Finished())
            {
                //чтобы процесс не вешал систему проверяем действие каждые 50 милисекунд
                Thread.Sleep(50);
            }
            //обьявляем класс вохранения данных в таблицу
            Digest MyDigest = new Digest();
          //  Console.WriteLine(" поиск окончен сохраняем данные ");
            //когда все потоки закончат поиск будем соберать данные в таблицу
            foreach (Potok SelectTask in TaskList) 
            { 
                //перебираем данные из найденного списка
                foreach (string FindPatch in SelectTask.Sercher.SerchResult) 
                {
                   // Console.WriteLine("получение данных о "+FindPatch);
                    //добавляем данные в таблицу
                    MyDigest.Digestpatch = FindPatch;
                    //добавляем данные в таблицу
                    MyDigest.ADDRowDigest();
                   // Console.WriteLine("Время обновления папки " + MyDigest.DateUpdate+"\t Путь к диретории " + MyDigest.Digestpatch+"\t Имя Kliko" + MyDigest.DigestName );
                }
            }

        }

    }
    #endregion 
}

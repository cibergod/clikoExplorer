using System;
using System.Collections.Generic;
using System.Text;

namespace FileFinder
{
    class DirectoryAndFileFind
    {
        //структура описания входных параметров
        struct inputinfo 
        {
            //от куда начинать поиск
            public string StartPatchFilder;
            //если ищем директорию то передаем ее название
            public string SearchFolder;
            //если ищем файл то передаем его название
            public string SearchFile;
        }
    }
}

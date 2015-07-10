using System;
using System.Collections.Generic;
using System.Text;
//библиотека работы с файловой системой
using System.IO;

namespace Serch
{
    //структура хранения сведений о справочнике
    public struct dictynary
    {
        //имя справочника
        public string name;
        //путь к файлу настроек справочника
        public string configPatch;
        //путь к справочнику
        public string dictynaryPatch;
    }

    //класс для поиска справочников
    public class SearchDictynary
    {
        //путь к папке где ищим справочники
        public string patch;
        public string FileName;
        //найденные справочники будем хранить в данном списке
        public extern List<dictynary> ResultListdictynary<dictynary>();
        
        //функция поиска справочника
        void FinderDictynary() 
        {
            //находим все директории в указанной папке 
            string[] FindDirs = Directory.GetDirectories(patch);
        }

    }
}

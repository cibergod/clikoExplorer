using System;
using System.Collections.Generic;
using System.Web;

using System.Threading;
using System.IO;
using System.Xml.Serialization;

namespace WebApplication1
{
    public class ConfigFilds
    {
        [XmlElement("Patch_Cliko")]
        public string Patch_Cliko; //путь к дирекотрии с отчетностями у нас диск d:\Cliko\
        [XmlElement("CFG_file")]
        public string CFG_file;  //имя файла конфигурации вдруг поменяеться по умолчанию KLIKOCFG.DB
        [XmlElement("SPR_DIR")]
        public string SPR_DIR; //путь к папке со справочниками
        [XmlElement("SPR_TAble")]
        public string SPR_TAble; //имя таблицы из которой читаем данные о справочниках 
    }

    public class ConfigAction
    {
        bool read = false;
        //создаем класс сохранения данных
        XmlSerializer s = new XmlSerializer(typeof(ConfigFilds));
        public ConfigFilds Parametrs = new ConfigFilds();
        //сохранение параметров 
        public void SaveParam()
        {

            //создаем поток записи в файл
            StringWriter stringWriter = new StringWriter();
            //записываем данные 
            s.Serialize(stringWriter, Parametrs);

            string xml = stringWriter.ToString();
            File.WriteAllText("config.xml", xml);

        }
        //загрузка параметров
        public void LoadParam()
        {
            try
            {
                var stringReader = new StringReader(File.ReadAllText("config.xml"));
                Parametrs = s.Deserialize(stringReader) as ConfigFilds;
                read = true;
            }
            catch
            {
                read = false;
            }
            finally 
            {
                Parametrs.CFG_file = "KLIKOCFG.DB";
                Parametrs.Patch_Cliko = "";
                Parametrs.SPR_DIR = "SPR";
                Parametrs.SPR_TAble = "rsprav";
            }
        }
    }
  
   
}
using System;
using System.Collections.Generic;
using System.Text;
//библиотека для работы с конфигом
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

namespace clikoEngine
{

    public class ConfigFilds 
    {
         [XmlElement("REM_Patch_Cliko")]
         public string REM_Patch_Cliko=@"путь к дирекотрии с отчетностями у нас диск d:\Cliko\";
         [XmlElement("Patch_Cliko")]
         public string Patch_Cliko; //путь к дирекотрии с отчетностями у нас диск d:\Cliko\

         [XmlElement("REM_CFG_file")]
         public string REM_CFG_file = "имя файла конфигурации вдруг поменяеться по умолчанию KLIKOCFG.DB";
         [XmlElement("CFG_file")]
         public string CFG_file;  //имя файла конфигурации вдруг поменяеться по умолчанию KLIKOCFG.DB

         [XmlElement("REM_SPR_DIR")]
         public string REM_SPR_DIR = "путь к папке со справочниками по умолчанию SPR";
         [XmlElement("SPR_DIR")]
         public string SPR_DIR; //путь к папке со справочниками

         [XmlElement("REM_SPR_TAble")]
         public string REM_SPR_TAble = "имя таблицы из которой читаем данные о справочниках ";
         [XmlElement("SPR_TAble")]
         public string SPR_TAble; //имя таблицы из которой читаем данные о справочниках 

         [XmlElement("REM_DSN_ODBC")]
         public string REM_DSN_ODBC = "DSN имя в ODBC для связки с драйвером Paradox";
         [XmlElement("DSN_ODBC")]
         public string DSN_ODBC; //имя для связки с драйвером Paradox
    }
  
   public class ConfigAction 
   {

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
           //если файла настройки нет то создаем его заново
           if (!File.Exists("config.xml")) CreateDefaultConfig();
           var stringReader = new StringReader(File.ReadAllText("config.xml"));
           Parametrs = s.Deserialize(stringReader) as ConfigFilds; 
       }
       //функция создает конфигурацию по умолчанию в которую можно будет в дальнейшем вносить изменения
       public void CreateDefaultConfig() 
       {
           Parametrs.CFG_file = "KLIKOCFG.DB";
           Parametrs.Patch_Cliko = @"D:\";
           Parametrs.SPR_DIR = "SPR";
           Parametrs.SPR_TAble = "rsprav";
           Parametrs.DSN_ODBC = "Config";
           SaveParam();
       }
   }
}

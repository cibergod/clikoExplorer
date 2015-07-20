using System;
using System.Collections.Generic;
using System.Text;


using System.IO;
using System.Threading;
namespace FileFinder
{
     class DirectoryAndFileFind
    {
         public string Patch, 
                       Fdir, 
                       Ffile;
         
         public void find()
         {
             if ((Patch != null) || (Patch != ""))
             {
                
                 //если существует такая папка 
                 if (Directory.Exists(Patch))
                 {
                     try
                     {
                         string[] GetDirList = Directory.GetDirectories(Patch);
                         foreach (string DirList in GetDirList)
                         {
                             
                             DirectoryAndFileFind A = new DirectoryAndFileFind();
                             A.Patch = DirList;
                             A.Ffile = Ffile;
                             A.Fdir = Fdir;
                             Thread S = new Thread(A.find);
                             S.Start();
                         }
                         //ищем файлы в текущей директории 
                       if((Ffile!="")||(Ffile!=null))
                       {
                         string[] GetFilList = Directory.GetFiles(Patch, Ffile);
                         foreach (string FilList in GetFilList)
                             Console.WriteLine(FilList);
                        }     
                         //ищем директорию подходящюю под название


                     }
                     catch 
                     {
                         Console.WriteLine(Patch + " доступ запрещен");
                     }

                 }
                 else
                 {
                     Console.WriteLine("путь не найден");
                 }
             }
             else 
             {
                 Console.WriteLine("путь не найден");
             }
         }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*потключаем класс для чтения записи в XML */
using RWXMLTABLE;

/*
  фишка в том чтобы передать в таблицу список значений в виде массива 
  а функция должна сама определить типы данных в таблице и преобразовать в них наши данные 
 * для каждого из стобцов
 */
namespace TableSave
{
    class Program
    {
        static void Main(string[] args)
        {
            //обьявляем класс для работы с XML буд от туда брать таблички
            RWXML XML = new RWXML();

            

        }
    }
}

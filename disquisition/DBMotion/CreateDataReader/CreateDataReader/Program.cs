using System;
using System.Collections.Generic;
using System.Text;



namespace CreateDataReader
{
    class Program
    {
        static void Main(string[] args)
        {

            Digest MyDigest = new Digest();

            MyDigest.Digestpatch = @"d:\Cliko\DATA\CFG\KLIKOCFG.DB";

            MyDigest.ADDRowDigest();

            //
            Console.WriteLine("готово");
            Console.ReadKey();
        }
    }
}

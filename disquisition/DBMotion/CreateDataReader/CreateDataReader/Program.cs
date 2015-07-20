using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace CreateDataReader
{
    class Program
    {
        static void Main(string[] args)
        {

            Digest MyDigest = new Digest();

            /*
            MyDigest.Digestpatch = @"d:\Cliko22\DATA\CFG\KLIKOCFG.DB";

            MyDigest.ADDRowDigest();

            MyDigest.Digestpatch = @"d:\Cliko2\DATA\CFG\KLIKOCFG.DB";

            MyDigest.ADDRowDigest();

            MyDigest.Digestpatch = @"d:\Cliko\DATA\CFG\KLIKOCFG.DB";

            MyDigest.ADDRowDigest();


            MyDigest.Digestpatch = @"d:\Cliko2\DATA\CFG\KLIKOCFG.DB";

            MyDigest.ADDRowDigest();
            */

        // string[] a=   Directory.GetDirectories(@"d:\Cliko2\DATA\CFG\KLIKOCFG.DB");

            File.GetAttributes(@"d:\Cliko2\DATA\CFG\KLIKOCFG.DB");

            //
            Console.WriteLine("готово"+a[0]);
            Console.ReadKey();
        }
    }
}

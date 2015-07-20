using System;
using System.Collections.Generic;
using System.Web;

using System.IO;

using System.Data;

namespace Web1
{
    public class getData
    {
        public DataTable Digest = new DataTable("Digest");
        public List<string> NameDigest = new List<string>();
        public void ReadDigest()
        {
            //читаем файл с найденными данными о clicko
            Digest.ReadXml(@"Digest.xml");

            
            foreach(DataRow r in Digest.Rows)
            {
                if (r["Name"] != DBNull.Value) 
                {
                    NameDigest.Add(r["Name"].ToString());
                }
            }

        }
    }
}
// foreach (DataRow dr in ds.Tables["data"].Rows)
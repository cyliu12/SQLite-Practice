using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.IO;

namespace Practice1
{
    class Program
    {
        static SQLiteConnection con;
        static SQLiteCommand com;
        static string dbPath = @".\TestOver83中文.sqlite";
        static string cnStr = "data source=" + dbPath;
      
        static void InitSQLiteDb()
        {
            Console.WriteLine("Init SQLite DB");
            if (File.Exists(dbPath))
            {
                con = new SQLiteConnection(cnStr);
                con.Open();
                com = con.CreateCommand();
                return;
            }
            else
            {
                con = new SQLiteConnection(cnStr);
                con.Open();
                com = con.CreateCommand();
                com.CommandText = "CREATE TABLE test (id integer primary key autoincrement, text varchar(10));";
                com.ExecuteNonQuery();
                Console.WriteLine("Create SQLite DB File");
            }
        }

        static void TestInsert()
        {
            //com.CommandText="DELETE FROM test";
            //com.CommandText = "INSERT INTO test (id, text) VALUES (1, '測試1');";
            DateTime d = DateTime.Now;            
            String insertCMD = String.Format("INSERT INTO test (text) VALUES ('時間 : {0}');", d.ToString());
            com.CommandText = insertCMD;
            com.ExecuteNonQuery();           
            Console.WriteLine("Insert data into SQLite DB");
        }

        static void TestSelect()
        {
            // 查詢剛新增的表test
            com.CommandText = "SELECT * FROM test";

            // 執行查詢塞入 sqlite_datareader
            SQLiteDataReader sqlite_datareader = com.ExecuteReader();

            // 一筆一筆列出查詢的資料
            while (sqlite_datareader.Read())
            {
                // Print out the content of the text field:
                String s = sqlite_datareader["text"].ToString();
                Console.WriteLine(s);

            }
        }

        static void CloseSQLiteDb()
        {
            con.Close();
            Console.WriteLine("Close SQLite DB");

        }

        static void Main(string[] args)
        {
            InitSQLiteDb();
            TestInsert();
            TestSelect();
            CloseSQLiteDb();
            Console.Read();
        }
    }
}

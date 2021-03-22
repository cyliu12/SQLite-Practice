using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Practice1
{
    class SQLiteIO
    {
        static SQLiteConnection _con;
        static SQLiteCommand _com;
        static string _dbPath = @".\db.sqlite";
        static string _cnStr = "data source=" + _dbPath;
        const string _PRIMARY = "filename";
        const string _CONTENTS = "contents";

        static public void SetDbName(string dbName)
        {
            _dbPath = @".\" + dbName + ".sqlite";
            _cnStr = "data source=" + _dbPath;
        }
        
        static public void InitDb(string[] tableNames)
        {
            if (File.Exists(_dbPath))
            {
                _con = new SQLiteConnection(_cnStr);
                _con.Open();
                _com = _con.CreateCommand();
            }
            else
            {
                _con = new SQLiteConnection(_cnStr);
                _con.Open();
                _com = _con.CreateCommand();

                Console.WriteLine("SQLite DB File {0} Created", _dbPath);

                for (int i = 0; i < tableNames.Length; i++)
                {
                    //_com.CommandText = "CREATE TABLE " +  tableNames[i] + "(filename string PRIMARY KEY, contents string)";
                    _com.CommandText = String.Format("CREATE TABLE {0} {1} string PRIMARY KEY, {2} string", tableNames[i], _PRIMARY, _CONTENTS);                      
                    _com.ExecuteNonQuery();
                    Console.WriteLine("Table {0} Created", tableNames[i]);
                }                
            }
            Console.WriteLine("Open SQLite DB Connection");
        }

        static public bool SaveInstance(string tableName, string filename, string contents)
        {
            try
            {
                _com.CommandText = String.Format("INSERT INTO {0} ({1}, {2}) VALUES ('{3}', '{4}')", tableName, _PRIMARY, _CONTENTS, filename, contents);
                _com.ExecuteNonQuery();
                return true;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.ToString());               
                return false;                
            }
        }

        static public string[] GetContents(string tableName, string filename)
        {
            SQLiteDataReader sqlite_datareader;
            List<string> datas = new List<string>();            
            _com.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = {2}", tableName, _PRIMARY, filename);
            if (filename.ToLower().Equals("all")) _com.CommandText = String.Format("SELECT * FROM {0} ", tableName);
            try
            {
                sqlite_datareader = _com.ExecuteReader();
                while (sqlite_datareader.Read())
                {                    
                    String s = sqlite_datareader[_CONTENTS].ToString();
                    //Console.WriteLine(s);
                    datas.Add(s);
                }
                sqlite_datareader.Close();
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.ToString());
            }
            
            return datas.ToArray();
        }

        static public string GetFileContents(string tableName, string filename)
        {
            SQLiteDataReader sqlite_datareader;            
            _com.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = {2}", tableName, _PRIMARY, filename);            
            try
            {
                sqlite_datareader = _com.ExecuteReader();
                if (sqlite_datareader.Read())
                {
                    string s = sqlite_datareader[_CONTENTS].ToString();
                    sqlite_datareader.Close();
                    return s;                    
                }
                else
                {
                    sqlite_datareader.Close();
                    return string.Empty;
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
            
        }

        static public void CloseDb()
        {
            _con.Close();
            Console.WriteLine("Close SQLite DB Connection");
        }
    }
}

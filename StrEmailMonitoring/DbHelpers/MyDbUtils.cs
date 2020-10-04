using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrEmailMonitoring
{
    public class MyDbUtils
    {
        private SQLiteConnection cnn { get; set; } = null;
        private SQLiteCommand cmd { get; set; } = null;
        private SQLiteDataAdapter db { get; set; } = null;
        private DataTable dt { get; set; } = null;


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="dbPath"></param>
        public MyDbUtils(string dbPath = "")
        {
            SetCnn(dbPath);
        }


        /// <summary>
        /// Set Connection
        /// </summary>
        /// <param name="dbPath">Path to db file</param>
        /// <returns></returns>
        public bool SetCnn(string dbPath)
        {
            try
            {
                if (!File.Exists(dbPath))
                {
                    Console.WriteLine($"[MyDbUtils] db file doesn't exists");
                    return false;
                }
                cnn = new SQLiteConnection($"Data Source={dbPath};version=3;New=False;Compress=True;");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SetCnn] DB Connection failed: " + e.Message);
                return false;
            }
        }



        /// <summary>
        /// Open Connection
        /// </summary>
        /// <returns></returns>
        public bool OpenCnn()
        {
            if (cnn != null)
            {
                try
                {
                    cnn.Open();
                    return true;
                }
                catch (Exception e)
                {

                    Console.WriteLine($"Cnn Open failed {e.Message}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"Cnn is null");
                return false;
            }
        }



        /// <summary>
        /// Close Connection
        /// </summary>
        /// <returns></returns>
        public bool CloseCnn()
        {
            if (cnn != null)
            {
                try
                {
                    cnn.Close();
                    return true;
                }
                catch (Exception e)
                {

                    Console.WriteLine($"Cnn Close failed {e.Message}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"Cnn is null");
                return false;
            }
        }




        // <summary>
        /// Loads entries with limit
        /// </summary>
        /// <returns></returns>
        public DataTable LoadEntries(string tableName = "", string orderBy = "", int perBatch = 25, int currentBatch = 1, Dictionary<string, string> criteria = null, Dictionary<string, string> notEqualCriteria = null)
        {
            int offset = perBatch * (currentBatch - 1);
            DataTable xdt = new DataTable(); ;
            if (cnn != null)
            {
                try
                {
                    cnn.Open();
                    SQLiteCommand cmd = cnn.CreateCommand();
                    string q = $"SELECT * FROM {tableName} ";

                    if (criteria != null)
                    {
                        int ct = 1;

                        q += "WHERE ";

                        foreach (KeyValuePair<string, string> kv in criteria)
                        {
                            q += $"{kv.Key} = ${kv.Key} ";

                            if (ct < criteria.Count)
                            {
                                q += "AND ";
                                ct += 1;
                            }
                        }
                    }

                    if (notEqualCriteria != null)
                    {
                        int ct = 1;

                        q += "AND ";

                        foreach (KeyValuePair<string, string> kv in notEqualCriteria)
                        {
                            q += $"{kv.Key} != @{kv.Key} ";

                            if (ct < notEqualCriteria.Count)
                            {
                                q += "AND ";
                                ct += 1;
                            }
                        }
                    }

                    q += $"ORDER BY {orderBy} ASC ";
                    q += "LIMIT $perBatch OFFSET $offset";

                    cmd.CommandText = q;
                    cmd.Parameters.Add("$perBatch", DbType.String).Value = perBatch;
                    cmd.Parameters.Add("$offset", DbType.String).Value = offset;


                    if (criteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in criteria)
                        {
                            cmd.Parameters.Add($"${kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }

                    if (notEqualCriteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in notEqualCriteria)
                        {
                            cmd.Parameters.Add($"@{kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }

                    SQLiteDataAdapter sad = new SQLiteDataAdapter(cmd);
                    sad.Fill(xdt);
                    cnn.Close();
                    return xdt;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[LoadEntries] Query Error >>> {e.Message}");
                    cnn.Close();
                    return xdt;
                }
            }
            else
            {
                Console.WriteLine("[LoadEntries] Connection is null");
                return xdt;
            }



        }




        /// <summary>
        /// Loads entries via LIKE operator
        /// </summary>
        /// <returns></returns>
        public DataTable LoadEntriesLike(string tableName = "", string orderBy = "", int perBatch = 25, int currentBatch = 1, Dictionary<string, string> criteria = null, Dictionary<string, string> likeCriteria = null)
        {
            int offset = perBatch * (currentBatch - 1);
            DataTable xdt = new DataTable(); ;
            if (cnn != null)
            {
                try
                {
                    cnn.Open();
                    SQLiteCommand cmd = cnn.CreateCommand();
                    string q = $"SELECT * FROM {tableName} ";

                    if (criteria != null)
                    {
                        int ct = 1;

                        q += "WHERE ";

                        foreach (KeyValuePair<string, string> kv in criteria)
                        {
                            q += $"{kv.Key} = ${kv.Key} ";

                            if (ct < criteria.Count)
                            {
                                q += "AND ";
                                ct += 1;
                            }
                        }

                        if (likeCriteria != null)
                        {
                            if (criteria.Count > 0) { q += "AND "; }
                            foreach (KeyValuePair<string, string> kvl in likeCriteria)
                            {
                                q += $"{kvl.Key} LIKE ${kvl.Key} ";

                                if (ct < criteria.Count)
                                {
                                    q += "AND ";
                                    ct += 1;
                                }
                            }
                        }

                    }


                    q += $"ORDER BY {orderBy} ASC ";
                    q += "LIMIT $perBatch OFFSET $offset";


                    cmd.CommandText = q;
                    cmd.Parameters.Add("$perBatch", DbType.String).Value = perBatch;
                    cmd.Parameters.Add("$offset", DbType.String).Value = offset;


                    if (criteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in criteria)
                        {
                            cmd.Parameters.Add($"${kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }
                    if (likeCriteria != null)
                    {
                        foreach (KeyValuePair<string, string> kvl in likeCriteria)
                        {
                            cmd.Parameters.Add($"${kvl.Key}", DbType.String).Value = kvl.Value.Trim();
                        }
                    }

                    SQLiteDataAdapter sad = new SQLiteDataAdapter(cmd);
                    sad.Fill(xdt);
                    cnn.Close();
                    return xdt;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[LoadEntries] Query Error >>> {e.Message}");
                    cnn.Close();
                    return xdt;
                }
            }
            else
            {
                Console.WriteLine("[LoadEntries] Connection is null");
                return xdt;
            }



        }




        /// <summary>
        /// Dynamic loading of Tables with subquery structure
        /// </summary>
        /// <param name="perBatch"></param>
        /// <param name="currentBatch"></param>
        /// <param name="targetColumn"></param>
        /// <param name="innerCriteria"></param>
        /// <param name="outerCriteria"></param>
        /// <returns></returns>
        public DataTable LoadEntriesViaSubquery(string tableName = "", int perBatch = 25, int currentBatch = 1, string targetColumn = "BranchCode", Dictionary<string, string> innerCriteria = null, Dictionary<string, string> outerCriteria = null, Dictionary<string, string> innerNotEqualCriteria = null, Dictionary<string, string> outerNotEqualCriteria = null, string outerOrderBy = "")
        {
            if (string.IsNullOrEmpty(outerOrderBy))
            {
                outerOrderBy = targetColumn; // set default order criteria
            }
            int offset = perBatch * (currentBatch - 1);
            DataTable xdt = new DataTable(); ;
            if (cnn != null)
            {
                try
                {
                    cnn.Open();
                    SQLiteCommand cmd = cnn.CreateCommand();

                    string q = $"SELECT * FROM {tableName} ";
                    q += $"WHERE {targetColumn} IN ( ";
                    q += $"SELECT {targetColumn} FROM {tableName} ";

                    if (innerCriteria != null)
                    {
                        int ct = 1;

                        q += "WHERE ";

                        foreach (KeyValuePair<string, string> kv in innerCriteria)
                        {
                            q += $"{kv.Key} = ${kv.Key} ";

                            if (ct < innerCriteria.Count)
                            {
                                q += "AND ";
                                ct += 1;
                            }
                        }
                    }

                    if (innerNotEqualCriteria != null)
                    {
                        int ct = 1;

                        q += "AND ";

                        foreach (KeyValuePair<string, string> kv in innerNotEqualCriteria)
                        {
                            q += $"{kv.Key} != @{kv.Key} ";

                            if (ct < innerNotEqualCriteria.Count)
                            {
                                q += "AND ";
                                ct += 1;
                            }
                        }
                    }

                    q += $"GROUP BY {targetColumn} ";
                    q += $"ORDER BY {targetColumn} ASC ";
                    q += "LIMIT $perBatch OFFSET $offset ";
                    q += " ) ";

                    if (outerCriteria != null)
                    {
                        int ct = 1;

                        q += "AND ";

                        foreach (KeyValuePair<string, string> kvo in outerCriteria)
                        {
                            q += $"{kvo.Key} = @{kvo.Key} ";

                            if (ct < outerCriteria.Count)
                            {
                                q += "AND ";
                                ct += 1;
                            }
                        }
                    }
                    if (outerNotEqualCriteria != null)
                    {
                        int ct = 1;

                        q += "AND ";

                        foreach (KeyValuePair<string, string> kvo in outerNotEqualCriteria)
                        {
                            q += $"{kvo.Key} != ${kvo.Key} ";

                            if (ct < outerNotEqualCriteria.Count)
                            {
                                q += "AND ";
                                ct += 1;
                            }
                        }
                    }

                    q += $"ORDER BY {outerOrderBy} ASC  ";

                    cmd.CommandText = q;
                    cmd.Parameters.Add("$perBatch", DbType.String).Value = perBatch;
                    cmd.Parameters.Add("$offset", DbType.String).Value = offset;

                    if (innerCriteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in innerCriteria)
                        {
                            cmd.Parameters.Add($"${kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }
                    if (outerCriteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in outerCriteria)
                        {
                            cmd.Parameters.Add($"@{kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }

                    if (innerNotEqualCriteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in innerNotEqualCriteria)
                        {
                            cmd.Parameters.Add($"@{kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }

                    if (outerNotEqualCriteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in outerNotEqualCriteria)
                        {
                            cmd.Parameters.Add($"${kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }
                    SQLiteDataAdapter sad = new SQLiteDataAdapter(cmd);
                    sad.Fill(xdt);
                    cnn.Close();
                    return xdt;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[LoadEntriesViaSubquery] Query Error >>> {e.Message}");
                    cnn.Close();
                    return xdt;
                }
            }
            else
            {
                Console.WriteLine("[LoadEntriesViaSubquery] Connection is null");
                return xdt;
            }
        }




        /// <summary>
        /// Count entries within the db
        /// </summary>
        /// <returns></returns>
        public long CountEntries(string tableName = "" , Dictionary<string, string> criteria = null)
        {
            DataTable xdt = new DataTable(); ;
            long countResult = 0;
            if (cnn != null)
            {
                try
                {
                    cnn.Open();
                    SQLiteCommand cmd = cnn.CreateCommand();
                    string q = $"SELECT COUNT(ID) FROM {tableName} ";

                    if (criteria != null)
                    {
                        int ct = 1;

                        q += "WHERE ";

                        foreach (KeyValuePair<string, string> kv in criteria)
                        {
                            q += $"{kv.Key} = ${kv.Key} ";

                            if (ct < criteria.Count)
                            {
                                q += "AND ";
                                ct += 1;
                            }
                        }
                    }
                    cmd.CommandText = q;
                    if (criteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in criteria)
                        {
                            cmd.Parameters.Add($"${kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }

                    countResult = long.TryParse(cmd.ExecuteScalar().ToString(), out long resultX) ? resultX : 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[CountEntries] Query Error >>> {e.Message}");
                    cnn.Close();
                }
            }
            else
            {
                Console.WriteLine("[CountEntries] Connection is null");
            }
            return countResult;
        }


        /// <summary>
        /// Count entries within the db
        /// </summary>
        /// <returns></returns>
        public long CountEntriesViaSubquery(string tableName = "", Dictionary<string, string> criteria = null, Dictionary<string, string> innerCriteria = null, string innerColumn = "", string innerTable = "")
        {
            DataTable xdt = new DataTable(); ;
            long countResult = 0;
            if (cnn != null)
            {
                try
                {
                    cnn.Open();
                    SQLiteCommand cmd = cnn.CreateCommand();
                    string q = $"SELECT COUNT(ID) FROM {tableName} ";


                    if (innerCriteria != null)
                    {
                        int cti = 1;

                        q += $"WHERE {innerColumn} IN ( ";
                        q += $"SELECT {innerColumn} FROM {innerTable} ";
                        q += $"WHERE ";

                        foreach (KeyValuePair<string, string> kv in innerCriteria)
                        {
                            q += $"{kv.Key} = @{kv.Key} ";
                            //q += $"{kv.Key} = '{kv.Value}' ";

                            if (cti < innerCriteria.Count)
                            {
                                q += "AND ";
                                cti += 1;
                            }
                        }
                        q += $" ) ";
                    }


                    if (criteria != null)
                    {
                        int ct = 1;

                        q += "AND ";

                        foreach (KeyValuePair<string, string> kv in criteria)
                        {
                            q += $"{kv.Key} = ${kv.Key} ";
                            // q += $"{kv.Key} = '{kv.Value}' ";

                            if (ct < criteria.Count)
                            {
                                q += "AND ";
                                ct += 1;
                            }
                        }
                    }
                    

                    cmd.CommandText = q;
                    if (criteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in criteria)
                        {
                            cmd.Parameters.Add($"${kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }
                    if (innerCriteria != null)
                    {
                        foreach (KeyValuePair<string, string> kv in innerCriteria)
                        {
                            cmd.Parameters.Add($"@{kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }

                    countResult = long.TryParse(cmd.ExecuteScalar().ToString(), out long resultX) ? resultX : 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[CountEntries] Query Error >>> {e.Message}");
                    cnn.Close();
                }
            }
            else
            {
                Console.WriteLine("[CountEntries] Connection is null");
            }
            return countResult;
        }


        /// <summary>
        /// Insert an Entry into the SasReport DB
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public bool CreateEntry(string tableName, Dictionary<string, string> records)
        {
            if (cnn != null)
            {
                try
                {
                    cnn.Open();
                    SQLiteCommand cmd = cnn.CreateCommand();

                    string q = $"INSERT INTO {tableName} ( ";

                    if (records != null)
                    {
                        int ct = 1;

                        foreach (KeyValuePair<string, string> kv in records)
                        {
                            q += $"{kv.Key} ";

                            if (ct < records.Count)
                            {
                                q += " , ";
                                ct += 1;
                            }
                        }
                    }
                    q += " ) ";
                    q += "VALUES ( ";


                    if (records != null)
                    {
                        int ct = 1;

                        foreach (KeyValuePair<string, string> kv in records)
                        {
                            q += $"@{kv.Key} ";

                            if (ct < records.Count)
                            {
                                q += ", ";
                                ct += 1;
                            }
                        }
                    }

                    q += ") ";

                    cmd.CommandText = q;


                    if (records != null)
                    {
                        foreach (KeyValuePair<string, string> kv in records)
                        {
                            cmd.Parameters.Add($"@{kv.Key}", DbType.String).Value = kv.Value;
                        }
                    }

                    cmd.ExecuteNonQuery();
                    cnn.Close();

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[CreateEntry] Query Error >>> {e.Message}");
                    cnn.Close();
                    return false;
                }
            }
            else
            {
                Console.WriteLine("[CreateEntry] Connection is null");
                return false;
            }
        }



        /// <summary>
        /// Update
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="criteria"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public bool UpdateEntry(string tableName, Dictionary<string, string> criteria, Dictionary<string, string> fields = null)
        {

            if (this.cnn != null && fields != null)
            {
                try
                {
                    cnn.Open();
                    SQLiteCommand cmd = cnn.CreateCommand();


                    string q = $"UPDATE {tableName} SET ";
                    int ct = 1;
                    foreach (KeyValuePair<string, string> kv in fields)
                    {
                        q += $"{kv.Key} = ${kv.Key} ";
                        if (ct < fields.Count)
                        {
                            q += ", ";
                        }
                        ct++;
                    }
                    int ctX = 1;

                    q += "WHERE ";

                    foreach (KeyValuePair<string, string> kv in criteria)
                    {
                        q += $"{kv.Key} = ${kv.Key} ";

                        if (ctX < criteria.Count)
                        {
                            q += "AND ";
                            ctX += 1;
                        }
                    }
                    // Query
                    cmd.CommandText = q;

                    // Query Params
                    foreach (KeyValuePair<string, string> kv in fields)
                    {
                        cmd.Parameters.Add($"${kv.Key}", DbType.String).Value = kv.Value ?? string.Empty;
                    }
                   

                    foreach (KeyValuePair<string, string> kv in criteria)
                    {
                        cmd.Parameters.Add($"${kv.Key}", DbType.String).Value = kv.Value;
                    }

                    // Execute
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    return true;
                }
                catch (Exception e)
                {

                    cnn.Close();
                    Console.WriteLine($"[UpdateEntry] update query failed {e.Message}");
                    return false;
                }

            }
            else
            {

                return false;
            }
        }



        /// <summary>
        /// Delete all entries
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllEntries(string tableName)
        {
            if (this.cnn != null)
            {
                try
                {
                    cnn.Open();
                    SQLiteCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = $"DELETE FROM {tableName} ";
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                    return true;
                }
                catch (Exception e)
                {
                    cnn.Close();
                    Console.WriteLine($"[DeleteEntries]  Delete failed {e.Message}");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

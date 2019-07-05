/* DatabaseAdapter class is used to read and write values from the local database,
 *      it is also used to access the machine learning model and get prediction values from it
 * the local database holds music tracks as follows: path(as key), bpm, zcr, spectirr */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using IronPython.Hosting;

using GamingMusicPlayer.DebugTools;
using GamingMusicPlayer.MusicPlayer;

namespace GamingMusicPlayer.Database
{
    public class DatabaseAdapter
    {
        static MLModel m = new MLModel();
        private string dbConnectionStr;

        public DatabaseAdapter(string connectionStr)
        {
            this.dbConnectionStr = connectionStr;
        }

        public static double predict(float bpm, float zcr, float spectirr)
        {
            double p = 0;
            var engine = Python.CreateEngine();
            dynamic model = engine.ImportModule("model");
            p = model.predict(bpm, zcr, spectirr);
            return p;
        }

        public void removeAllTracks()
        {
            string query = "DELETE FROM Song;";
            using (SqlConnection sqlConnection = new SqlConnection(dbConnectionStr))
            using (SqlCommand sqlCmd = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                Console.WriteLine("rows affected:" + sqlCmd.ExecuteNonQuery() + "  executed query:" + sqlCmd.CommandText);
                sqlConnection.Close();
            }
        }


        //only removes if the path is in the database
        public void removeTrack(string path)
        {
            bool exists = false;
            using (SqlConnection sqlConnection1 = new SqlConnection(dbConnectionStr))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Song where full_path='" + path + "';", sqlConnection1))
            {
                sqlConnection1.Open();
                DataTable songTable = new DataTable();
                adapter.Fill(songTable);
                EnumerableRowCollection<DataRow> dataRows = songTable.AsEnumerable();
                for (int i = 0; i < dataRows.Count(); i++)
                {
                    object[] row = dataRows.ElementAt(i).ItemArray;
                    string ts = (string)row[1];
                    if (ts.Equals(path))
                    {
                        exists = true;
                        break;
                    }
                }
                sqlConnection1.Close();
            }


            if (exists)
            {
                string query = "DELETE FROM Song WHERE full_path='"+path+"';";
                using (SqlConnection sqlConnection = new SqlConnection(dbConnectionStr))
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    Console.WriteLine("rows affected:" + sqlCmd.ExecuteNonQuery() + "  executed query:" + sqlCmd.CommandText);
                    sqlConnection.Close();
                }
            }
            else
            {
                //Console.WriteLine("already in db or file doesnt exist");
            }
        }

        //only adds if file exists on drive and path is not in the database
        public void addTrack(Track t)
        {
            if (File.Exists(t.Path) && getTrack(t.Path) == null)
            {
                string path = strToSafeSqlFormat(t.Path);
                string query = "INSERT INTO Song VALUES('"+path+"', "+t.BPM+", "+t.ZCR+", "+t.SpectralIrregularity+");";
                using (SqlConnection sqlConnection = new SqlConnection(dbConnectionStr))
                using (SqlCommand sqlCmd = new SqlCommand(query, sqlConnection))
                {
                    Console.WriteLine("executing query:" + sqlCmd.CommandText + "  ..");
                    sqlConnection.Open();
                    
                    Console.WriteLine( "\trows affected:" + sqlCmd.ExecuteNonQuery());
                    sqlConnection.Close();
                }
            }
            else
            {
                //Console.WriteLine("already in db or file doesnt exist");
            }
        }

        public Track getTrack(string path) //returns null if track not found
        {
            path = strToSafeSqlFormat(path);
            using (SqlConnection sqlConnection = new SqlConnection(dbConnectionStr))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Song where full_path='"+path+"';", sqlConnection))
            {
                sqlConnection.Open();
                DataTable songTable = new DataTable();
                adapter.Fill(songTable);
                EnumerableRowCollection<DataRow> dataRows = songTable.AsEnumerable();
                for (int i = 0; i < dataRows.Count(); i++)
                {
                    object[] row = dataRows.ElementAt(i).ItemArray;
                    try
                    {
                        if (!File.Exists((string)row[1]))
                            throw new FileNotFoundException();
                        Track t = new Track((string)row[1]);
                        t.BPM = (double)row[2];
                        t.ZCR = (double)row[3];
                        t.SpectralIrregularity = (double)row[4];
                        return t;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("DB ADAPTER REMOVING from get track:" + (string)row[1]);
                        removeTrack((string)row[1]);
                        sqlConnection.Close();
                        return null;
                    }
                    
                }
                sqlConnection.Close();
            }
            return null;
        }

        public List<Track> getAllTracks()
        {
            List<Track> tracks = new List<Track>();
            using (SqlConnection sqlConnection = new SqlConnection(dbConnectionStr))
            using(SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Song", sqlConnection))
            {
                sqlConnection.Open();
                DataTable songTable = new DataTable();
                adapter.Fill(songTable);
                EnumerableRowCollection<DataRow> dataRows = songTable.AsEnumerable();
                for(int i=0; i< dataRows.Count(); i++)
                {
                    object[] row = dataRows.ElementAt(i).ItemArray;

                    try
                    {
                        if (!File.Exists((string)row[1]))
                            throw new FileNotFoundException();
                        Track t = new Track((string)row[1]);
                        t.BPM = (double)row[2];
                        t.ZCR = (double)row[3];
                        t.SpectralIrregularity = (double)row[4];
                        tracks.Add(t);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("DB ADAPTER REMOVING SONG:" + (string)row[1]);
                        removeTrack((string)row[1]);
                    }
                   
                    
                }
                sqlConnection.Close();
            }
            return tracks;
        }

        public string strToSafeSqlFormat(string str)
        {
            string result = "";
            foreach(char c in str)
            {
                if (c == '\'')
                    result += "''";
                else
                    result += c;
            }
            return result;
        }

        public static double predicts(float bpm, float zcr, float spectirr)
        {
            double p = m.predict(bpm, zcr, spectirr);
            return p;
        }
    }
}

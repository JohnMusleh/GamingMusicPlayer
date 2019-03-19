using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

using GamingMusicPlayer.MusicPlayer;

namespace GamingMusicPlayer
{
    public class DatabaseAdapter
    {
        private string dbConnectionStr;

        public DatabaseAdapter(string connectionStr)
        {
            this.dbConnectionStr = connectionStr;
        }

        //only removes if the path is in the database
        //[missing] path check
        public void removeTrack(string path)
        {
            if (getTrack(path)!=null)
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
        //[missing] path check
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

        //[missing] path check -> for each ' add another ' next to it
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
                    Track t = new Track((string)row[1]);
                    t.BPM = (double)row[2];
                    t.ZCR = (double)row[3];
                    t.SpectralIrregularity = (double)row[4];
                    return t;
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
                    Track t = new Track((string)row[1]);
                    t.BPM = (double)row[2];
                    t.ZCR = (double)row[3];
                    t.SpectralIrregularity = (double)row[4];
                    tracks.Add(t);
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
    }
}

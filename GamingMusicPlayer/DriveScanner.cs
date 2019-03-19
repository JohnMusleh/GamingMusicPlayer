using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using GamingMusicPlayer.MusicPlayer;

namespace GamingMusicPlayer
{
    public class DriveScanner
    {
        private string mainDirName;
        private bool scanning;
        private double completePerc;

        private List<Track> tracks;

        public event EventHandler onScanComplete; //subscribe to get notified when threadSupport scan is done

        public event EventHandler onPercChanged;

        public List<Track> Tracks { get { return tracks; } }

        public bool Scanning{ get{ return scanning; } }

        public double CompletePercentage { get { return completePerc; } }


        public DriveScanner(string dir)
        {
            scanning = false;
            this.mainDirName = dir;
            completePerc = 100;
            tracks = null;
        }

        public void cancelScan()
        {
            scanning = false;
        }

        public void scan(bool threadSupport)
        {
            if (!scanning)
            {
                completePerc = 0;
                if (threadSupport)
                {
                    (new Thread(() => {
                        scanning = true;
                        tracks = scanDir(mainDirName);
                        scanning = false;
                        Console.WriteLine("\nThread done---- FINAL TRACKS LENGTH : " + tracks.Count);
                        foreach (Track t in tracks)
                        {
                            Console.WriteLine(t.Path);
                        }
                        onScanComplete(null, null);
                    })).Start();
                }
                else
                {
                    scanning = true;
                    tracks = scanDir(mainDirName);
                    scanning = false;
                    Console.WriteLine("\nThread done---- FINAL TRACKS LENGTH : " + tracks.Count);
                    foreach (Track t in tracks)
                    {
                        Console.WriteLine(t.Path);
                    }
                }
            }
        }


        private List<Track> scanDir(string dirName)
        {
            List<Track> tracks = new List<Track>();
            List<string> allDirs = new List<string>();
            allDirs.Add(dirName);
            Console.WriteLine("exploring all dirs..");
            Stack<string> dirsToCheck = new Stack<string>();
            try
            {
                string[] dirs = Directory.GetDirectories(dirName);
                foreach (string dir in dirs)
                {
                    dirsToCheck.Push(dir);
                }
            }
            catch(Exception e)
            {

            }
            
            
            while (dirsToCheck.Count > 0 && scanning) 
            {
                string dir = dirsToCheck.Pop();
                allDirs.Add(dir);
                try
                {
                    string[] subDirs = Directory.GetDirectories(dir);
                    foreach (string subDir in subDirs)
                    {
                        dirsToCheck.Push(subDir);
                    }
                }
                catch(Exception e)
                {

                }
                
            }
            Console.WriteLine("explored " + allDirs.Count + " dirs.. finding music files..");
            int dirScannedCount = 0;
            foreach(string dir in allDirs)
            {
                if (!scanning)
                    break;
                try
                {
                    string[] files = Directory.GetFiles(dir);
                    foreach (string file in files)
                    {
                        try
                        {
                            Track t = new Track(file);
                            tracks.Add(t);
                        }
                        catch (FileFormatNotSupported e)
                        {

                        }
                    }
                }
                catch(Exception e)
                {

                }
                dirScannedCount++;
                completePerc = (double)(((double)dirScannedCount / (double)allDirs.Count) * 100);
                
                onPercChanged(null,null);
            }
            return tracks;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingMusicPlayer.MusicPlayer
{
    public class Playlist
    {
        private List<Track> tracklist;
        private List<int> play_order; //list of index's of the tracklist
        private int selected_index; //index of the play_order index of track being played
        private bool shuffled;
        public Track SelectedTrack
        {
            get
            {
                if (play_order.Count == 0)
                    return null;
                return tracklist.ElementAt(play_order.ElementAt(selected_index));
            }
        }

        public int SelectedTrackIndex //returns the selected track index in the tracklist
        {
            get
            {
                if (play_order.Count == 0)
                    return 0;
                return play_order.ElementAt(selected_index);
            }
        }

        public int Count
        {
            get
            {
                return tracklist.Count;
            }
        }

        public bool Shuffled
        {
            get
            {
                return this.shuffled;
            }
        }

        public Playlist()
        {
            this.tracklist = new List<Track>();
            this.play_order = new List<int>();
            this.selected_index = 0;
            this.shuffled = false;
        }

        public Playlist(Track t)
        {
            this.tracklist = new List<Track>();
            this.play_order = new List<int>();
            this.selected_index = 0;

            tracklist.Add((Track)t.Clone());
            play_order.Add(0);
        }

        public Playlist(List<Track> tl)
        {
            if(tl == null)
            {
                this.tracklist = new List<Track>();
                this.play_order = new List<int>();
                this.selected_index = 0;
            }
            this.tracklist = new List<Track>(tl.Count);
            this.play_order = new List<int>(tl.Count);
            this.selected_index = 0;

            int i=0;
            tl.ForEach((item) =>
            {
                tracklist.Add((Track)item.Clone());
                play_order.Add(i);
                i++;
            });
        }

        public bool selectTrack(int i)//i index from tracklist
        {
            if (i < 0 || i >= tracklist.Count)
                return false;
            selected_index = play_order.FindIndex(item => item == i);
            return true;
        }

        public Track nextTrack()
        {
            if(tracklist.Count == 0)
                return null;
            selected_index++;
            if (selected_index >= tracklist.Count)
                selected_index = 0;
            return tracklist.ElementAt(play_order.ElementAt(selected_index));
        }

        public Track prevTrack()
        {
            if (tracklist.Count == 0)
                return null;
            selected_index--;
            if (selected_index < 0)
                selected_index = tracklist.Count - 1;
            return tracklist.ElementAt(play_order.ElementAt(selected_index));
        }

        public void addTrack(Track t)
        {
            this.tracklist.Add((Track)t.Clone());
            this.play_order.Add(tracklist.Count - 1);
        }

        public bool removeTrack()//remove selected track
        { 
            if (play_order.Count == 0)
                return false;
            int track_index = play_order.ElementAt(selected_index);
            Console.WriteLine("attempting to remove track index:" + track_index);
            selected_index = 0;
            for (int i = 0; i < play_order.Count; i++)
                play_order[i] = i;
            tracklist.RemoveAt(track_index);
            play_order.RemoveAt(track_index);
            for (int i = 0; i < play_order.Count; i++)
                play_order[i] = i;


            if (shuffled)
                shuffle();
            return true;
        }

        public bool shuffle()
        {
            if (play_order.Count <= 2)
                return false;
            int track_index = play_order.ElementAt(selected_index); //storing trackindex
            Random rng = new Random();
            List<int> tmp = new List<int>(play_order.Count);
            play_order.ForEach((item) =>
            {
                tmp.Add(item);
            });
            play_order.Clear();
            while (tmp.Count > 0)
            {
                int i = rng.Next(tmp.Count);//random number between 0 and count-1
                play_order.Add(tmp.ElementAt(i));
                tmp.RemoveAt(i);
            }
            //find the new playing_index in the new shuffled list of index's
            selected_index = play_order.FindIndex(item => item == track_index);
            shuffled = true;
            return true;
        }

        public bool deshuffle()
        {
            int track_index = play_order.ElementAt(selected_index); //storing trackindex
            for (int i=0 ;i<play_order.Count; i++)
                play_order[i] = i;
            selected_index = track_index;
            shuffled = false;
            return true;
        }

        public void setAsDefaultOrder()
        {
            List<Track> tmp = new List<Track>(tracklist.Count);
            tracklist.ForEach((item) =>
            {
                tmp.Add((Track)item.Clone());
            });

            tracklist.Clear();
            for(int i=0; i<play_order.Count; i++)
            {
                tracklist[i] = tmp.ElementAt(play_order.ElementAt(i));
            } 
        }
        
    }
}

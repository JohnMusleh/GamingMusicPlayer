using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GamingMusicPlayer.MusicPlayer
{
    public class MusicPlayer
    {
        private string mci_command;
        private StringBuilder mci_return;
        private int rflag;//return flag

        private string error_msg;
        private Playlist loaded_playlist;
        private bool playing;
        private bool paused; //to resume when attempting to play again
        private bool seeked; //seeked flag to not reload the file when seeked
        private int volume;

        public string ErrorMsg
        {
            get
            {
                return error_msg;
            }
        }

        public int Volume
        {
            get
            {
                return volume;
            }
        }

        public bool Playing
        {
            get
            {
                return playing;
            }
        }

        public bool Paused
        {
            get
            {
                return paused;
            }
        }

        public Playlist LoadedPlaylist
        {
            get
            {
                return loaded_playlist;
            }
        }

        public Track SelectedTrack
        {
            get
            {
                return loaded_playlist.SelectedTrack;
            }
        }

        public int SelectedTrackIndex
        {
            get
            {

                return loaded_playlist.SelectedTrackIndex;
            }
        }

        public List<Track> PlaylistTracklist
        {
            get
            {

                return loaded_playlist.TrackList;
            }
        }

        public bool Shuffled
        {
            get
            {
                return loaded_playlist.Shuffled;
            }
        }

        public MusicPlayer()
        {
            mci_return = new StringBuilder(128);
            error_msg = "no error";
            loaded_playlist = new Playlist();
            playing = false;
            seeked = false;
            volume = 1000;
        }

        public void addTrack(Track t)
        {
            loaded_playlist.addTrack(t);
        }

        public bool removeTrack()//removes selected track
        {
            if (!loaded_playlist.removeTrack())
            {
                error_msg = "removeTrack(): playlist may be empty.";
                return false;
            }
            paused = false;
            playing = false;
            return true;
        }

        public bool selectTrack(int i)
        {
            if (!loaded_playlist.selectTrack(i))
            {
                error_msg = "selectTrack(): desired index is out of boundaries";
                return false;
            }
            return loadFile(loaded_playlist.SelectedTrack.Path);
        }

        private void close()
        {
            mci_command = "close MediaFile";
            MusicFileInfo.mciSendString(mci_command, null, 0, IntPtr.Zero);
            paused = false;
            playing = false;
        }

        private bool loadFile(string path)
        {
            if (seeked)
            {
                seeked = false;
                return true;
            }
            close();
            //try to open as mpegvideo 
            mci_command = "open \"" + path +
                           "\" type mpegvideo alias MediaFile";
            rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
            if (rflag != 0)
            {
                // Let MCI decide which file type the song is
                mci_command = "open \"" + path +
                               "\" alias MediaFile";
                rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
                if (rflag == 0)
                    return true;
                else
                {
                    MusicFileInfo.mciGetErrorString(rflag, mci_return, mci_return.Capacity);
                    error_msg = "loadFile(): mci open command failed -> " + mci_return.ToString();
                    return false;
                }
            }
            else
                return true;
        }

        private bool initPlay() //begin playing selected track
        {
            if (loaded_playlist.Count == 0)
            {
                error_msg = "initPlay(): loaded playlist is empty.";
                return false;
            }

            Track t = loaded_playlist.SelectedTrack;
            if (loadFile(t.Path))
            {
                mci_command = "play MediaFile";
                rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
                if (rflag == 0)
                {
                    playing = true;
                    return true;
                }
                close();
                MusicFileInfo.mciGetErrorString(rflag, mci_return, mci_return.Capacity);
                error_msg = "initPlay(): mci play command failed -> " + mci_return.ToString();
                return false;
            }
            return false;
        }

        public bool next()
        {
            loaded_playlist.nextTrack();
            return initPlay();
        }

        public bool prev()
        {
            loaded_playlist.prevTrack();
            return initPlay();
        }

        public bool pause()
        {
            if (playing)
            {
                mci_command = "pause MediaFile";
                rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
                if (rflag == 0)
                {
                    playing = false;
                    paused = true;
                    return true;
                }
                MusicFileInfo.mciGetErrorString(rflag, mci_return, mci_return.Capacity);
                error_msg = "pause(): mci pause command failed -> " + mci_return.ToString();
                return false;
            }
            error_msg = "pause(): no music playing";
            return false;
        }

        public bool resume()//this method should be used to toggle play/resume button
        {
            if (paused)
            {
                mci_command = "resume MediaFile";
                rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
                if (rflag == 0)
                {
                    paused = false;
                    playing = true;
                    return true;
                }
                MusicFileInfo.mciGetErrorString(rflag, mci_return, mci_return.Capacity);
                error_msg = "resume(): mci resume command failed -> " + mci_return.ToString();
                return false;
            }
            return initPlay();
        }

        public bool stop()
        {
            mci_command = "stop MediaFile";
            rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
            if (rflag == 0)
            {
                paused = false;
                playing = false;
                close();
                return true;
            }
            MusicFileInfo.mciGetErrorString(rflag, mci_return, mci_return.Capacity);
            error_msg = "stop(): mci stop command failed -> " + mci_return.ToString();
            return false;
        }

        public bool shuffle()
        {
            return loaded_playlist.shuffle();
        }

        public bool deshuffle()
        {
            return loaded_playlist.deshuffle();
        }

        public bool setVolume(int v)
        {
            if (v >= 0 || v <= 1000)
            {
                this.volume = v;
                mci_command = "setaudio MediaFile volume to " + volume.ToString();
                rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
                if (rflag == 0)
                    return true;
                MusicFileInfo.mciGetErrorString(rflag, mci_return, mci_return.Capacity);
                error_msg = "setVolume(): mci setaudio command failed -> " + mci_return.ToString();
                return false;
            }
            error_msg = "setVolume(): desired volume out of range";
            return false;
        }

        public int getTrackLength() //returns -1 on errors
        {
            if (loaded_playlist.Count == 0)
            {
                error_msg = "getTrackLength(): loaded playlist is empty";
                return -1;
            }
            if (loaded_playlist.SelectedTrack.Length <= 0)
            {
                mci_command = "status MediaFile length";
                rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
                if (rflag == 0)
                {
                    if (mci_return.Length == 0)
                        return 0;
                    try
                    {
                        int l = int.Parse(mci_return.ToString());
                        return l;
                    }
                    catch (Exception e)
                    {
                        error_msg = "getTrackLength(): return string:" + mci_return.ToString() + "  exception:" + e.Message;
                        return -1;
                    }
                }
                MusicFileInfo.mciGetErrorString(rflag, mci_return, mci_return.Capacity);
                error_msg = "getTrackLength(): mci status command failed -> " + mci_return.ToString();
                return -1;
            }
            return loaded_playlist.SelectedTrack.Length;
        }

        public int getCurrentPosition()//in milliseconds [returns -1 on errors]
        {
            if (loaded_playlist.Count == 0)
                return 0;
            mci_command = "status MediaFile position";
            rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
            if (rflag == 0)
            {
                if (mci_return.Length == 0)
                    return 0;
                try
                {
                    int c = int.Parse(mci_return.ToString());
                    if (c > SelectedTrack.Length)
                        return SelectedTrack.Length;
                    return c;
                }
                catch (Exception e)
                {
                    error_msg = "getCurrentPosition(): return string:" + mci_return.ToString() + "  exception:" + e.Message;
                    return -1;
                }
            }
            MusicFileInfo.mciGetErrorString(rflag, mci_return, mci_return.Capacity);
            error_msg = "getCurrentPosition(): mci status command failed -> " + mci_return.ToString();
            return -1;
        }

        public bool setPosition(int ms)
        {
            if (ms < 0 || ms > getTrackLength())
            {
                error_msg = "setPosition(): desired position out of range";
                return false;
            }

            if (playing)
                mci_command = "play MediaFile from " + ms.ToString();
            else
            {
                mci_command = "seek MediaFile to " + ms.ToString();
                paused = false;
                seeked = true;
            }

            rflag = MusicFileInfo.mciSendString(mci_command, mci_return, mci_return.Capacity, IntPtr.Zero);
            if (rflag == 0)
            {
                return true;
            }
            MusicFileInfo.mciGetErrorString(rflag, mci_return, mci_return.Capacity);
            error_msg = "setPosition(): mci play/seek command failed -> " + mci_return.ToString();
            return false;
        }
    }
}
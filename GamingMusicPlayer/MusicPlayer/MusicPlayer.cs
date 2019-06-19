/* MusicPlayer class is an implementation of a music player, the music player can load only one playlist at a time
     *      MusicPlayer uses mci commands to communication with mci device and play music files, it only supports MP3 and WAV files.*/
using System;
using System.Collections.Generic;
using System.Text;

namespace GamingMusicPlayer.MusicPlayer
{
    public class MusicPlayer
    {
        private string mciCommand;
        private StringBuilder mciReturn;
        private int rflag;//return flag for the mci methods

        private string errorMsg;
        private Playlist loadedPlaylist;
        private bool playing;
        private bool paused; //to resume when attempting to play again
        private bool seeked; //seeked flag to not reload the file when seeked
        private int volume;

        public string ErrorMsg
        {
            get
            {
                return errorMsg;
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
                return loadedPlaylist;
            }
        }

        public Track SelectedTrack
        {
            get
            {
                return loadedPlaylist.SelectedTrack;
            }
        }

        public int SelectedTrackIndex
        {
            get
            {

                return loadedPlaylist.SelectedTrackIndex;
            }
        }

        public List<Track> PlaylistTracklist
        {
            get
            {

                return loadedPlaylist.TrackList;
            }
        }

        public bool Shuffled
        {
            get
            {
                return loadedPlaylist.Shuffled;
            }
        }

        public MusicPlayer()
        {
            mciReturn = new StringBuilder(128);
            errorMsg = "no error";
            loadedPlaylist = new Playlist();
            playing = false;
            seeked = false;
            volume = 1000;
        }

        //addTrack(): adds a track to the current playlist
        public void addTrack(Track t)
        {
            loadedPlaylist.addTrack(t);
        }

        //removeTrack(): removes the currently selected track
        public bool removeTrack()
        {
            if (!loadedPlaylist.removeTrack())
            {
                errorMsg = "removeTrack(): playlist may be empty.";
                return false;
            }
            paused = false;
            playing = false;
            return true;
        }

        //selectTrack(): selects a track in the playlist and loads the file.
        public bool selectTrack(int i)
        {
            if (!loadedPlaylist.selectTrack(i))
            {
                errorMsg = "selectTrack(): desired index is out of boundaries";
                return false;
            }
            return loadFile(loadedPlaylist.SelectedTrack.Path);
        }

        //next(): selects next track in the playlist and plays it
        public bool next()
        {
            loadedPlaylist.nextTrack();
            return initPlay();
        }

        //next(): selects previous track in the playlist and plays it
        public bool prev()
        {
            loadedPlaylist.prevTrack();
            return initPlay();
        }

        public bool pause()
        {
            if (playing)
            {
                mciCommand = "pause MediaFile";
                rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
                if (rflag == 0)
                {
                    playing = false;
                    paused = true;
                    return true;
                }
                MusicFileInfo.mciGetErrorString(rflag, mciReturn, mciReturn.Capacity);
                errorMsg = "pause(): mci pause command failed -> " + mciReturn.ToString();
                return false;
            }
            errorMsg = "pause(): no music playing";
            return false;
        }

        //resume(): public method used to play music tracks 
        //this method should be used to toggle play/resume button
        public bool resume()
        {
            if (paused)
            {
                mciCommand = "resume MediaFile";
                rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
                if (rflag == 0)
                {
                    paused = false;
                    playing = true;
                    return true;
                }
                MusicFileInfo.mciGetErrorString(rflag, mciReturn, mciReturn.Capacity);
                errorMsg = "resume(): mci resume command failed -> " + mciReturn.ToString();
                return false;
            }
            return initPlay();
        }

        //stop(): stops the currently playing file
        public bool stop()
        {
            mciCommand = "stop MediaFile";
            rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
            if (rflag == 0)
            {
                paused = false;
                playing = false;
                close();
                return true;
            }
            MusicFileInfo.mciGetErrorString(rflag, mciReturn, mciReturn.Capacity);
            errorMsg = "stop(): mci stop command failed -> " + mciReturn.ToString();
            return false;
        }

        //shuffle(): shuffles the playlist
        public bool shuffle()
        {
            return loadedPlaylist.shuffle();
        }

        //deshuffle(): deshuffles the playlist
        public bool deshuffle()
        {
            return loadedPlaylist.deshuffle();
        }

        //setVolume(): sets the volume of the MCI device
        public bool setVolume(int v)
        {
            if (v >= 0 || v <= 1000)
            {
                this.volume = v;
                mciCommand = "setaudio MediaFile volume to " + volume.ToString();
                rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
                if (rflag == 0)
                    return true;
                MusicFileInfo.mciGetErrorString(rflag, mciReturn, mciReturn.Capacity);
                errorMsg = "setVolume(): mci setaudio command failed -> " + mciReturn.ToString();
                return false;
            }
            errorMsg = "setVolume(): desired volume out of range";
            return false;
        }

        //getTrackLength(): gets the length in ms of the currently selected track in the playlist
        //returns -1 on errors
        public int getTrackLength() 
        {
            if (loadedPlaylist.Count == 0)
            {
                errorMsg = "getTrackLength(): loaded playlist is empty";
                return -1;
            }
            if (loadedPlaylist.SelectedTrack.Length <= 0)
            {
                mciCommand = "status MediaFile length";
                rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
                if (rflag == 0)
                {
                    if (mciReturn.Length == 0)
                        return 0;
                    try
                    {
                        int l = int.Parse(mciReturn.ToString());
                        return l;
                    }
                    catch (Exception e)
                    {
                        errorMsg = "getTrackLength(): return string:" + mciReturn.ToString() + "  exception:" + e.Message;
                        return -1;
                    }
                }
                MusicFileInfo.mciGetErrorString(rflag, mciReturn, mciReturn.Capacity);
                errorMsg = "getTrackLength(): mci status command failed -> " + mciReturn.ToString();
                return -1;
            }
            return loadedPlaylist.SelectedTrack.Length;
        }

        //getCurrentPosition(): gets the current position in ms of the currently playing music track
        //returns -1 on errors
        public int getCurrentPosition()
        {
            if (loadedPlaylist.Count == 0)
                return 0;
            mciCommand = "status MediaFile position";
            rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
            if (rflag == 0)
            {
                if (mciReturn.Length == 0)
                    return 0;
                try
                {
                    int c = int.Parse(mciReturn.ToString());
                    if (c > SelectedTrack.Length)
                        return SelectedTrack.Length;
                    return c;
                }
                catch (Exception e)
                {
                    errorMsg = "getCurrentPosition(): return string:" + mciReturn.ToString() + "  exception:" + e.Message;
                    return -1;
                }
            }
            MusicFileInfo.mciGetErrorString(rflag, mciReturn, mciReturn.Capacity);
            errorMsg = "getCurrentPosition(): mci status command failed -> " + mciReturn.ToString();
            return -1;
        }

        //setPosition(): seeks into a desired position in ms of the currently selected track
        public bool setPosition(int ms)
        {
            if (ms < 0 || ms > getTrackLength())
            {
                errorMsg = "setPosition(): desired position out of range";
                return false;
            }

            if (playing)
                mciCommand = "play MediaFile from " + ms.ToString();
            else
            {
                mciCommand = "seek MediaFile to " + ms.ToString();
                paused = false;
                seeked = true;
            }

            rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
            if (rflag == 0)
            {
                return true;
            }
            MusicFileInfo.mciGetErrorString(rflag, mciReturn, mciReturn.Capacity);
            errorMsg = "setPosition(): mci play/seek command failed -> " + mciReturn.ToString();
            return false;
        }


        //close(): closes MCI device
        private void close()
        {
            mciCommand = "close MediaFile";
            MusicFileInfo.mciSendString(mciCommand, null, 0, IntPtr.Zero);
            paused = false;
            playing = false;
        }

        //loadFile(): initializes the MCI device with the desired music file
        private bool loadFile(string path)
        {
            if (seeked)
            {
                seeked = false;
                return true;
            }
            close();
            //try to open as mpegvideo 
            mciCommand = "open \"" + path +
                           "\" type mpegvideo alias MediaFile";
            rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
            if (rflag != 0)
            {
                // Let MCI decide which file type the song is
                mciCommand = "open \"" + path +
                               "\" alias MediaFile";
                rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
                if (rflag == 0)
                    return true;
                else
                {
                    MusicFileInfo.mciGetErrorString(rflag, mciReturn, mciReturn.Capacity);
                    errorMsg = "loadFile(): mci open command failed -> " + mciReturn.ToString();
                    return false;
                }
            }
            else
                return true;
        }

        //initPlay(): plays the loaded music file, this method is to be used only when the file is fresh in the MCI device (not paused)
        private bool initPlay() 
        {
            if (loadedPlaylist.Count == 0)
            {
                errorMsg = "initPlay(): loaded playlist is empty.";
                return false;
            }

            Track t = loadedPlaylist.SelectedTrack;
            if (loadFile(t.Path))
            {
                mciCommand = "play MediaFile";
                rflag = MusicFileInfo.mciSendString(mciCommand, mciReturn, mciReturn.Capacity, IntPtr.Zero);
                if (rflag == 0)
                {
                    setVolume(volume);
                    playing = true;
                    return true;
                }
                close();
                MusicFileInfo.mciGetErrorString(rflag, mciReturn, mciReturn.Capacity);
                errorMsg = "initPlay(): mci play command failed -> " + mciReturn.ToString();
                return false;
            }
            return false;
        }
    }
}
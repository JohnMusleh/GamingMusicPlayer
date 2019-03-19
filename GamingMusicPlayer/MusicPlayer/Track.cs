using System;

namespace GamingMusicPlayer.MusicPlayer
{
    /*Track represents a music track, only MP3 and WAV is supported */
    public class Track : ICloneable
    {

        private string path;
        private string artist; //can be null
        private double bpm; //can be null
        private double zcr; //can be null
        private double spectrIrr; //can be null
        private int length; //in ms

        private string error_msg;

        public short[] Data
        {
            get { return readDataFromFile(); }
        }

        public double ZCR
        {
            get { return zcr; }
            set
            {
                this.zcr = value;
            }
        }

        public double SpectralIrregularity
        {
            get { return spectrIrr; }
            set
            {
                this.spectrIrr = value;
            }
        }

        public double BPM
        {
            get { return bpm; }
            set
            {
                this.bpm = value;
            }
        }

        public string Path
        {
            get { return path; }
            set
            {
                this.path = value;
                if (!fixLength())
                {
                    this.length = -1;
                }
                validateFileFormat_throws();
            }
        }

        public string Artist
        {
            get { return artist; }
            set { this.artist = value; }
        }


        public string Name
        {
            get
            {
                string[] tokens = path.Split('\\');
                if (tokens.Length == 0)
                    return "";
                return tokens[tokens.Length - 1];
            }
        }

        public string ErrorMsg
        {
            get
            {
                return error_msg;
            }
        }

        public int Length
        {
            get
            {
                return this.length;
            }
        }

        public string Format
        {
            get
            {
                string[] tokens = path.Split('.');
                string fileType = "NA";
                if (tokens.Length > 0)
                {
                    fileType = tokens[tokens.Length - 1];
                }
                fileType = fileType.ToUpper();
                return fileType;
            }
        }


        public Track(string path)
        {
            Random rng = new Random();
            this.path = path;
            this.artist = null;
            this.bpm = -1;
            this.zcr = -1;
            this.spectrIrr = -1;
            this.error_msg = "no error";
            if (!fixLength())
            {
                this.length = -1;
            }
            validateFileFormat_throws();
        }

        private Track(Track t)
        {
            if (t.artist == null)
                this.artist = null;
            else
                this.artist = (string)t.artist.Clone();
            this.bpm = t.bpm;
            this.zcr = t.zcr;
            this.spectrIrr = t.spectrIrr;
            this.path = (string)t.path.Clone();

            this.length = t.length;
        }

        public object Clone()
        {
            return new Track(this);
        }

        private bool fixLength()
        {
            this.length = MusicFileInfo.getLength(path);
            if (length < 0)
            {
                error_msg = MusicFileInfo.error_msg;
                return false;
            }
            return true;
        }

        private void validateFileFormat_throws()//throws exception if file type is not supported
        {
            if (!MusicFileInfo.isCorrectFormat(path))
            {
                error_msg = MusicFileInfo.error_msg;
                throw new FileFormatNotSupported(error_msg);
            }
        }

        private short[] readDataFromFile()
        {
            short[] data = null;
            NAudio.Wave.WaveStream reader = null;
            if (this.Format.Equals("MP3"))
            {
                reader = new NAudio.Wave.Mp3FileReader(this.Path);
            }
            else if (this.Format.Equals("WAV"))
            {
                reader = new NAudio.Wave.WaveFileReader(this.Path);

            }

            if (reader != null)
            {
                byte[] buffer = new byte[reader.Length];
                int read = reader.Read(buffer, 0, buffer.Length);
                data = new short[read / sizeof(short)];
                Buffer.BlockCopy(buffer, 0, data, 0, read);
            }
            return data;
        }
    }
}

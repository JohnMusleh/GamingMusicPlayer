using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingMusicPlayer
{
    //The track class represents a music track
    public class Track : ICloneable
    {

        private string path;
        private string artist; //can be null
        private string genre; //can be null
        private int length; //in ms

        private string error_msg;

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

        public string Genre
        {
            get { return genre; }
            set { this.genre = value; }
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
            this.genre = null;
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
            if (t.genre == null)
                this.genre = null;
            else
                this.genre = (string)t.genre.Clone();
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
    }
}

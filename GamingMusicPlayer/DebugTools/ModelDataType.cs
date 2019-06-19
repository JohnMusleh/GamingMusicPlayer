using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Runtime;

namespace GamingMusicPlayer.DebugTools
{
    public class ModelDataType
    {
        [LoadColumn(0)]
        public float Score;
        [LoadColumn(1)]
        public float BPM;
        [LoadColumn(2)]
        public float ZCR;
        [LoadColumn(3)]
        public float SpectIrr;
        
    }
}

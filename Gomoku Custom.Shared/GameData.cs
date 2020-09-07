using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Gomoku_Custom.Shared
{
    [Serializable]
    public class GameData
    {
        //public const int FieldDim = 19;
        public Team[][] Field { get; set; }
        public Point Updated { get; set; }
        //public string Message { get; set; }
        public ResponseCode Code { get; set; }
        public Team NextPlayer { get; set; }

        //public static GameData FromByteArray(byte[] input)
        //{
        //    return 
        //}

        //public static byte[] ToByteArray(GameData data)
        //{
        //    //byte[] utfString = Encoding.UTF8.GetBytes(data.Message);
        //    byte[] transfer = new byte[
        //        data.Field.Length + //Field
        //        2 + //Point
        //        //utfString.Length + //Message
        //        1 //Code
        //        ];
        //    int index = 0;
        //    foreach (var element in data.Field)
        //        transfer[index++] = element;

        //    transfer[index++] = data.Updated.X;
        //    transfer[index++] = data.Updated.Y;
        //    transfer[index] = (byte)data.Code;
            
        //    return transfer;
        //}
    }

    
}

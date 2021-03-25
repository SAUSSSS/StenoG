using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
namespace StenoG
{
    class ProcessBytes
    {
        public Bitmap MakeLSB(Bitmap src, ref BackgroundWorker worker)
        {
            Bitmap res = new Bitmap(src.Width, src.Height);
            for (int i = 0; i < src.Width; i++)
            {
                worker.ReportProgress((int)((float)i / src.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < src.Height; j++)
                {
                    Color pixelColor = src.GetPixel(i, j);

                    BitArray colorArray = Byte2Bit(pixelColor.R);
                    colorArray[3] = false; // т.е. 0
                    colorArray[4] = false;
                    colorArray[5] = false;
                    colorArray[6] = false;
                    colorArray[7] = false;
                    byte newR = Bit2Byte(colorArray);

                    colorArray = Byte2Bit(pixelColor.G);
                    colorArray[3] = false;
                    colorArray[4] = false;
                    colorArray[5] = false;
                    colorArray[6] = false;
                    colorArray[7] = false;
                    byte newG = Bit2Byte(colorArray);

                    colorArray = Byte2Bit(pixelColor.B);
                    colorArray[3] = false;
                    colorArray[4] = false;
                    colorArray[5] = false;
                    colorArray[6] = false;
                    colorArray[7] = false;
                    byte newB = Bit2Byte(colorArray);

                    Color newColor = Color.FromArgb(newR, newG, newB);
                    res.SetPixel(i, j, newColor);
                }
            }
            return res;
        }

        private BitArray Byte2Bit(byte src)
        {
            BitArray bitArray = new BitArray(8);
            bool bit = false;
            for (int i = 0; i < 8; i++)
            {
                if ((src >> i & 1) == 1)
                {
                    bit = true;
                }
                else bit = false;
                bitArray[i] = bit;
            }
            return bitArray;
        }

        private byte Bit2Byte(BitArray src)
        {
            byte value = 0;
            for (int i = 0; i < src.Count; i++)
                if (src[i] == true)
                    value += (byte)Math.Pow(2, i);
            return value;
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

    }
}

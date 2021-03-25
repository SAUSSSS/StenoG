using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.IO;
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

        public static long text_size = 0;
        byte[] input_text_byte_list;
        public Bitmap EncodeRawImage(Bitmap src, ref BackgroundWorker worker)
        {
            ReadText(ref worker);
            if (input_text_byte_list == null)
            {
                Form1.ShowMsgBox("List is empty!");
                return src;
            }

            Bitmap res = new Bitmap(src);
            int i = 0, j = 0;

            // write text into image
            bool stop = false;
            int text_ind = 0;
            for (; i < src.Width; i++)
            {
                worker.ReportProgress(Clamp((int)((float)i / src.Width * 50 + 50),
                    0, 100));
                if (worker.CancellationPending)
                    return null;
                for (j = 0; j < src.Height; j++)
                {
                    if (text_ind == text_size)
                    {
                        stop = true;
                        break;
                    }
                    EncodeSymbol(ref src, ref res, ref i, ref j, ref text_ind,
                        input_text_byte_list[text_ind]);
                }
                if (stop) break;
            }
            return res;
        }

        private void ReadText(ref BackgroundWorker worker)
        {
            string text_file = Form1.input_file_name();
            if (!File.Exists(text_file))
            {
                Form1.ShowMsgBox("File is empty!");
                return;
            }
            int input_text_byte_list_ind = 0;
            FileStream fs = File.Open(text_file, FileMode.Open, FileAccess.ReadWrite);
            FileInfo finfo = new FileInfo(text_file);
            text_size = finfo.Length;
            if (text_size > Form1.img_size())
            {
                Form1.ShowMsgBox("Picture is too small for encoding!");
                return;
            }
            fs.SetLength(text_size);
            input_text_byte_list = new byte[text_size];
            for (int w = 0; w < fs.Length; w++)
            {
                worker.ReportProgress((int)((float)w / fs.Length * 50));
                if (worker.CancellationPending)
                    return;
                input_text_byte_list[input_text_byte_list_ind] =
                    Convert.ToByte(fs.ReadByte());
                input_text_byte_list_ind++;
            }
            fs.Close();
        }

        private void EncodeSymbol(ref Bitmap src, ref Bitmap res,
            ref int i, ref int j, ref int index, byte byte_)
        {
            Color pixelColor = src.GetPixel(i, j);
            BitArray colorArray = Byte2Bit(pixelColor.R);
            BitArray messageArray = Byte2Bit(byte_);
            colorArray[0] = messageArray[0];
            colorArray[1] = messageArray[1];
            byte newR = Bit2Byte(colorArray);

            colorArray = Byte2Bit(pixelColor.G);
            colorArray[0] = messageArray[2];
            colorArray[1] = messageArray[3];
            colorArray[2] = messageArray[4];
            byte newG = Bit2Byte(colorArray);

            colorArray = Byte2Bit(pixelColor.B);
            colorArray[0] = messageArray[5];
            colorArray[1] = messageArray[6];
            colorArray[2] = messageArray[7];
            byte newB = Bit2Byte(colorArray);

            Color newColor = Color.FromArgb(newR, newG, newB);
            res.SetPixel(i, j, newColor);
            index++;
        }

    }
}

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

        string label_start = "St@rT";
        string label_end = "4IniSh";
        string str_message;
        public void EjectTextFromImage(Bitmap src, ref BackgroundWorker worker)
        {
            long text_size_local = 0;
            int start_label_ends_i = 0, start_label_ends_j = 0;
            bool start_label_found = false;
            bool stop = false;

            // Ищем в изображении метку начала

            for (int i_ = 0; i_ < src.Width; i_++)
            {
                worker.ReportProgress((int)((float)i_ / src.Width * 33));
                if (worker.CancellationPending)
                    return;
                for (int j_ = 0; j_ < src.Height; j_++)
                {
                    byte[] byte_label_start = new byte[label_start.Length];
                    int label_ind = 0;
                    while (label_ind < label_start.Length)
                    {
                        if (j_ + label_ind > src.Height - 1)
                        {
                            if (i_ + 1 < src.Width)
                            {
                                int add2j_ = label_ind - (src.Height - 1 - j_ - 1);
                                start_label_ends_i = i_ + 1;
                                start_label_ends_j = j_ + add2j_;
                            }
                            else
                            {
                                Form1.ShowMsgBox("Start label not found!");
                                return;
                            }
                        }
                        else
                        {
                            start_label_ends_i = i_;
                            start_label_ends_j = j_ + label_ind;
                        }
                        byte_label_start[label_ind] = Bit2Byte(DecodeSymbol(ref src,
                            ref start_label_ends_i, ref start_label_ends_j));
                        label_ind++;
                    }
                    string str_label_start = Encoding.ASCII.GetString(byte_label_start
                        ); //     UTF8 // ASCII
                    if (str_label_start == label_start)
                    {
                        start_label_found = true;
                        stop = true;
                        break;
                    }
                }
                if (stop) break;
            }
            if (!start_label_found)
            {
                Form1.ShowMsgBox("Text error 1 !");
                return;
            }

            // Ищем в изображении метку конца

            int end_label_ends_i = 0, end_label_ends_j = 0;
            bool end_label_found = false;
            stop = false;
            int ii_ = 0, jj_ = 0;
            if (start_label_ends_j == src.Height - 1)
            {
                start_label_ends_j = 0;
                start_label_ends_i++;
            }
            else
            {
                start_label_ends_j++;
            }

            for (ii_ = start_label_ends_i; ii_ < src.Width; ii_++)
            {
                worker.ReportProgress((int)((float)ii_ / src.Width * 33));
                if (worker.CancellationPending)
                    return;
                jj_ = 0;
                if (ii_ == start_label_ends_i) jj_ = start_label_ends_j;
                for (; jj_ < src.Height; jj_++)
                {
                    byte[] byte_label_end = new byte[label_end.Length];
                    int label_ind = 0;
                    while (label_ind < label_end.Length)
                    {
                        if (jj_ + label_ind > src.Height - 1)
                        {
                            if (ii_ + 1 < src.Width)
                            {
                                int add2j_ = label_ind - (src.Height - 1 - jj_ - 1);
                                end_label_ends_i = ii_ + 1;
                                end_label_ends_j = add2j_;
                            }
                            else
                            {
                                Form1.ShowMsgBox("End label not found!");
                                return;
                            }
                        }
                        else
                        {
                            end_label_ends_i = ii_;
                            end_label_ends_j = jj_ + label_ind;
                        }
                        byte_label_end[label_ind] = Bit2Byte(DecodeSymbol(ref src,
                            ref end_label_ends_i, ref end_label_ends_j));
                        label_ind++;
                    }
                    string str_label_end = Encoding.ASCII.GetString(byte_label_end
                        ); // UTF8 // ASCII
                    if (str_label_end == label_end)
                    {
                        end_label_found = true;
                        stop = true;
                        break;
                    }
                    text_size_local++;
                }
                if (stop) break;
            }
            if (!end_label_found)
            {
                Form1.ShowMsgBox("Text error 2 !");
                return;
            }

            int text_ends_i = 0, text_ends_j = 0;
            if (jj_ == 0)
            {
                if (ii_ > 0)
                {
                    text_ends_i = ii_ - 1;
                }
                else
                {
                    Form1.ShowMsgBox("Text error 3 !");
                    return;
                }
                text_ends_j = src.Height - 1;
            }
            else
            {
                text_ends_i = ii_;
                if (jj_ > 0)
                {
                    text_ends_j = jj_ - 1;
                }
                else
                {
                    Form1.ShowMsgBox("Text error 4 !");
                    return;
                }
            }

            Form1.ShowMsgBox("Labels detection done!");

            // Считываем из изображения текст между метками

            stop = false;
            byte[] message = new byte[text_size_local];
            int message_ind = 0;
            for (int i_ = start_label_ends_i; i_ < src.Width; i_++)
            {
                worker.ReportProgress((int)((float)i_ / src.Width * 33));
                if (worker.CancellationPending)
                    return;
                int j_ = 0;
                if (i_ == start_label_ends_i) j_ = start_label_ends_j;
                for (; j_ < src.Height; j_++)
                {
                    if (message_ind > text_size_local - 1)
                    {
                        break;
                        stop = true;
                    }
                    message[message_ind] = Bit2Byte(DecodeSymbol(ref src, ref i_, ref j_));
                    str_message = Encoding.ASCII.GetString(message); // UTF8 // ASCII
                    message_ind++;
                }
                if (stop) break;
            }

            WriteText();
        }

        private void WriteText()
        {
            byte[] temp_message = Encoding.ASCII.GetBytes(str_message);
            FileStream SourceStream = File.Open(Form1.ejected_file_name(),
                FileMode.Create);
            SourceStream.Seek(0, SeekOrigin.Begin);
            SourceStream.Write(temp_message, 0, temp_message.Length);
            SourceStream.Close();
        }

        private BitArray DecodeSymbol(ref Bitmap src, ref int i, ref int j)
        {
            Color pixelColor = src.GetPixel(i, j);

            BitArray colorArray = Byte2Bit(pixelColor.R);
            BitArray messageArray = Byte2Bit(pixelColor.R);
            messageArray[0] = colorArray[0];
            messageArray[1] = colorArray[1];

            colorArray = Byte2Bit(pixelColor.G);
            messageArray[2] = colorArray[0];
            messageArray[3] = colorArray[1];
            messageArray[4] = colorArray[2];

            colorArray = Byte2Bit(pixelColor.B);
            messageArray[5] = colorArray[0];
            messageArray[6] = colorArray[1];
            messageArray[7] = colorArray[2];
            return messageArray;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StenoG
{
    public partial class Form1 : Form
    {
        static Bitmap image;
        Bitmap image_original;


        Bitmap image_r;
        Bitmap image_g;
        Bitmap image_b;


        Bitmap diff_img;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessImg prc_im = new FormChannels();
            backgroundWorker1.RunWorkerAsync(prc_im);
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.png;*.jpg;*.bmp|All files(*.*)|*.*";
            dialog.Title = "Open an Image File";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                image_original = new Bitmap(dialog.FileName);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "Image files|*.png;*.jpg;*.bmp|All files(*.*)|*.*";
            dialog.RestoreDirectory = true;

            var res1 = (Bitmap)pictureBox1.Image;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var savepath = dialog.FileName;

                res1.Save(savepath);

            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessImg prc_im = (ProcessImg)e.Argument;
            prc_im.processImage(image, ref backgroundWorker1, true);
            if (backgroundWorker1.CancellationPending != true)
            {
                image_r = prc_im.get_image_r();
                image_g = prc_im.get_image_g();
                image_b = prc_im.get_image_b();
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox2.Image = image_r;
                pictureBox2.Refresh();
                pictureBox3.Image = image_g;
                pictureBox3.Refresh();
                pictureBox4.Image = image_b;
                pictureBox4.Refresh();
                toolStripStatusLabel1.Text =
                    "Channels` retrieving has just completed";
            }
            else
            {
                toolStripStatusLabel1.Text = "Channels` retrieving was cancelled";
            }
            progressBar1.Value = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (image_original == null)
            {
                MessageBox.Show("Original image is empty!");
                return;
            }
            image = image_original;
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            backgroundWorker2.CancelAsync();
            backgroundWorker3.CancelAsync();
            backgroundWorker4.CancelAsync();
            backgroundWorker5.CancelAsync();
            backgroundWorker6.CancelAsync();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ProcessImg prc_im = new FormChannelsInverted();
            backgroundWorker1.RunWorkerAsync(prc_im);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ProcessImg prc_im = new LSB4Img();
            backgroundWorker5.RunWorkerAsync(prc_im);
        }

        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessImg prc_im = (ProcessImg)e.Argument;
            prc_im.processImage(image, ref backgroundWorker5);
            if (backgroundWorker5.CancellationPending != true)
            {
                image = prc_im.get_image_LSB();
            }

        }

        private void backgroundWorker5_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
                toolStripStatusLabel1.Text = "LSB retrieving has just completed";
            }
            else
            {
                toolStripStatusLabel1.Text = "LSB retrieving was cancelled";
            }
            progressBar1.Value = 0;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ProcessImg prc_im = new DiffWithOrigin();
            backgroundWorker6.RunWorkerAsync(prc_im);
        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessImg prc_im = (ProcessImg)e.Argument;
            prc_im.processImage(image, image_original, backgroundWorker6);
            if (backgroundWorker6.CancellationPending != true)
            {
                diff_img = prc_im.get_image_diff();
                image = diff_img;
            }
        }

        private void backgroundWorker6_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
                toolStripStatusLabel1.Text = "Computing of the difference against original image has just completed";
            }
            else
            {
                toolStripStatusLabel1.Text = "Computing of the difference against original image was cancelled";
            }
            progressBar1.Value = 0;

        }
        static string src_text_file;
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|All files(*.*)|*.*";
            dialog.Title = "Open a Text File";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                src_text_file = dialog.FileName;
            }
            ProcessImg prc_im = new EncodeRawImg();
            backgroundWorker2.RunWorkerAsync(prc_im);

        }
        public static void ShowMsgBox(string input)
        {
            MessageBox.Show(input);
        }

        public static int img_size()
        {
            if (image == null)
                return 0;
            return image.Width * image.Height;
        }

        public static string input_file_name()
        {
            return src_text_file;
        }

        Bitmap image_encoded_raw;
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessImg prc_im = (ProcessImg)e.Argument;
            prc_im.processImage(image, ref backgroundWorker2);
            if (backgroundWorker2.CancellationPending != true)
            {
                image_encoded_raw = prc_im.get_image_raw();
                image = image_encoded_raw;
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
                toolStripStatusLabel1.Text = "Raw image encoding has just completed";
            }
            else
            {
                toolStripStatusLabel1.Text = "Raw image encoding was cancelled";
            }
            progressBar1.Value = 0;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (image_encoded_raw == null)
            {
                MessageBox.Show("Raw image is empty!");
                return;
            }
            image = image_encoded_raw;
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }
        static string ejected_text_file;
        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt";
            dialog.Title = "Save a Text File";
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                System.IO.FileStream fs =
                    (System.IO.FileStream)dialog.OpenFile();
                fs.Close();
            }
            ejected_text_file = dialog.FileName;
            ProcessImg prc_im = new DecodeImg();
            backgroundWorker4.RunWorkerAsync(prc_im);
        }
    }
}

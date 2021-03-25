using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{
    class LSB4Img : ProcessImg
    {
        Bitmap image_LSB;
        protected ProcessBytes process_bytes;
        public override Bitmap get_image_LSB() { return image_LSB; }

        public override Bitmap processImage(Bitmap sourceImage,
            ref BackgroundWorker worker)
        {
            process_bytes = new ProcessBytes();
            Bitmap resultImage = process_bytes.MakeLSB(sourceImage, ref worker);
            image_LSB = resultImage;
            return resultImage;
        }
    }
}

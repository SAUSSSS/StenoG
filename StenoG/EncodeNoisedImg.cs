using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{
    class EncodeNoisedImg : ProcessImg
    {
        Bitmap image_noised;

        public override Bitmap get_image_noised() { return image_noised; }
        protected ProcessBytes process_bytes;
        public override Bitmap processImage(Bitmap sourceImage,
            ref BackgroundWorker worker)
        {
            process_bytes = new ProcessBytes();
            image_noised = process_bytes.EncodeNoisedImage(sourceImage, ref worker);
            return image_noised;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{
    class EncodeRawImg : ProcessImg
    {
        Bitmap image_raw;
        protected ProcessBytes process_bytes;
        public override Bitmap get_image_raw() { return image_raw; }

        public override Bitmap processImage(Bitmap sourceImage,
            ref BackgroundWorker worker)
        {
            process_bytes = new ProcessBytes();
            image_raw = process_bytes.EncodeRawImage(sourceImage, ref worker);
            return image_raw;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace StenoG
{

    class DecodeImg : ProcessImg
    {
        protected ProcessBytes process_bytes;
        public override void processImage(Bitmap sourceImage,
           ref BackgroundWorker worker, bool no_return_value)
        {
            process_bytes = new ProcessBytes();
            process_bytes.EjectTextFromImage(sourceImage, ref worker);
        }
    }
}

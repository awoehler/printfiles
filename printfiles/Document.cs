using System.Collections.Generic;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Imaging;

namespace printfiles
{
    public class Document
    {
        private List<Image> images;
        private Image currentImagePage;
        public string filename { get; set; }
        public Document(string filename)
        {
            this.filename = filename;
            images = new List<Image>();
            Bitmap bitmap = (Bitmap)Image.FromFile(filename);
            int count = bitmap.GetFrameCount(FrameDimension.Page);
            for (int idx = 0; idx < count; idx++)
            {
                // save each frame to a bytestream
                bitmap.SelectActiveFrame(FrameDimension.Page, idx);
                MemoryStream byteStream = new MemoryStream();
                bitmap.Save(byteStream, ImageFormat.Tiff);

                // and then create a new Image from it
                images.Add(Image.FromStream(byteStream));
            }
        }

        public void print()
        {
            foreach (Image image in images)
            {
                currentImagePage = image;
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.Color = false;
                pd.PrintPage += PrintPage;
                pd.Print();
            }
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {

            Rectangle m = e.PageBounds;

            if ((double)currentImagePage.Width / (double)currentImagePage.Height > (double)m.Width / (double)m.Height) // image is wider
            {
                e.PageSettings.Landscape = true;
                m.Height = (int)((double)currentImagePage.Height / (double)currentImagePage.Width * (double)m.Width) - 2;
                e.PageBounds.Inflate(1098, 848);

            }
            else
            {
                e.PageSettings.Landscape = false;
                m.Width = (int)((double)currentImagePage.Width / (double)currentImagePage.Height * (double)m.Height) - 2;
                e.PageBounds.Inflate(848, 1098);
            }

            e.PageBounds.Offset(1, 1);
            e.Graphics.DrawImage(currentImagePage, m);
        }
    }
}

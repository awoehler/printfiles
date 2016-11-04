using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Printing;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Imaging;

namespace printfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            print();
         }
        static void print()
        {
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.Color = false;
            pd.PrintPage += PrintPage;
            pd.Print();
        }

        static List<Image> GetAllPages(string file)
        {
            List<Image> images = new List<Image>();
            Bitmap bitmap = (Bitmap)Image.FromFile(file);
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
            return images;
        }

        static void PrintPage(object o, PrintPageEventArgs e)
        {
            String[] allfiles = System.IO.Directory.GetFiles( Directory.GetCurrentDirectory(), "*.tif", System.IO.SearchOption.TopDirectoryOnly );

            foreach (var file in allfiles)
            {
                List<Image> pages = GetAllPages(file);
                foreach( Image page in pages )
                {
                    Rectangle m = e.PageBounds;

                    if ((double)page.Width / (double)page.Height > (double)m.Width / (double)m.Height) // image is wider
                    {
                        e.PageSettings.Landscape = true;
                        m.Height = (int)((double)page.Height / (double)page.Width * (double)m.Width) - 2;
                        e.PageBounds.Inflate(1098, 848);

                    }
                    else
                    {
                        e.PageSettings.Landscape = false;
                        m.Width = (int)((double)page.Width / (double)page.Height * (double)m.Height) - 2;
                        e.PageBounds.Inflate(848, 1098);
                    }

                    e.PageBounds.Offset(1, 1);
                    e.Graphics.DrawImage(page, m);
                }
            }
        }

    }
}

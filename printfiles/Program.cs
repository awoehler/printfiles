using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Printing;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;

namespace printfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            //Only works if printer accepts the given format.
            //var file = File.ReadAllBytes(args[0]);
            //var printQueue = LocalPrintServer.GetDefaultPrintQueue();

            //using (var job = printQueue.AddJob())
            //using (var stream = job.JobStream)
            //{
            //    stream.Write(file, 0, file.Length);
            //}
            print();
         }
        static void print()
        {
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.Color = false;
            pd.PrintPage += PrintPage;
            pd.Print();
        }

            static void PrintPage(object o, PrintPageEventArgs e)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile("sample.TIF");
            Rectangle m = e.PageBounds;

            if ((double)img.Width / (double)img.Height > (double)m.Width / (double)m.Height) // image is wider
            {
                e.PageSettings.Landscape = true;
                m.Height = (int)((double)img.Height / (double)img.Width * (double)m.Width) - 2;
                //e.PageSettings.i
                e.PageBounds.Inflate(1098, 848 );

            }
            else
            {
                e.PageSettings.Landscape = false;
                m.Width = (int)((double)img.Width / (double)img.Height * (double)m.Height) - 2;
                e.PageBounds.Inflate(848, 1098);
            }

            e.PageBounds.Offset(1, 1);
            e.Graphics.DrawImage(img, m);

        }

    }
}

using System.IO;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;

namespace IEGOFormationEditor.Tools
{
    public class Draw
    {
        public static Image DrawString(string text, int x, int y, Image baseImage)
        {
            Bitmap bmp = new Bitmap(baseImage);
            var graphics = Graphics.FromImage(bmp);
            graphics.DrawString(text, new Font("Arial", 8.5f), Brushes.White, new Point(x, y));
            return bmp;
        }

        public static Bitmap DrawImage(string path, int x, int y, Image baseImage)
        {
            Bitmap bmp = new Bitmap(baseImage);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(Image.FromStream(GetResourceStream(path)), new Point(x, y));
            }
            return bmp;
        }

        public static Bitmap DrawImage(Stream file, int x, int y, Image baseImage)
        {
            Bitmap bmp = new Bitmap(baseImage);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(Image.FromStream(file), new Point(x, y));
            }
            return bmp;
        }

        public static Stream GetResourceStream(string path)
        {
            string resourcePath = path;
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            List<string> source = new List<string>(executingAssembly.GetManifestResourceNames());
            resourcePath = source.FirstOrDefault((string r) => r.Contains(resourcePath));
            if (resourcePath == null)
            {
                throw new FileNotFoundException("Resource not found");
            }
            return executingAssembly.GetManifestResourceStream(resourcePath);
        }
    }
}

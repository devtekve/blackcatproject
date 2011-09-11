using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using Decapcha;

namespace ocr
{
    public sealed class ReferenceCharacterStore
    {
        private static readonly ReferenceCharacterStore instance = new ReferenceCharacterStore();
        private string path = "References";

        public static ReferenceCharacterStore getInstance()
        {
            return instance;
        }

        private ReferenceCharacterStore()  // use
        {
            path = "References";
        }

        [MethodImpl(MethodImplOptions.Synchronized)] // use
        public int[,] get1(ReferenceCharacterDescriptor referencecharacterdescriptor, int i)
        {
            int[,] ai = loadImage(referencecharacterdescriptor, i);
            if (ai != null)
            {
                return ai;
            }
            return ai;
        }

        private Bitmap getImageFile(ReferenceCharacterDescriptor referencecharacterdescriptor, int i)
        {
            string p = path + "\\" + "Ref_" + referencecharacterdescriptor.ToString() + "\\" + i.ToString() + ".png";
            return new Bitmap(p);
        }

        private int[,] loadImage(ReferenceCharacterDescriptor referencecharacterdescriptor, int i)
        {
            try
            {
                Bitmap file = getImageFile(referencecharacterdescriptor, i);
                return fromImage(file);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private int[,] fromImage(Bitmap buffered)
        {
            int i = buffered.Width;
            int j = buffered.Height;
            int[,] ai = new int[i, j];
            for (int k = 0; k < j; k++)
            {
                for (int l = 0; l < i; l++)
                    ai[l, k] = buffered.GetPixel(l, k).ToArgb() & 0xffffff;
            }

            buffered.Dispose();
            return ai;
        }

    }
}
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ocr;
using System.Windows.Forms;

namespace Decapcha
{
    class CapchaKiller
    {
        private byte[] _payload;

        public CapchaKiller(byte[] payload)
        {
            _payload = payload;
        }

        public string GetStringCapcha()
        {
            byte[] uncompressedImage = UncompressedImage();
            Bitmap bmp = new Bitmap(200, 64);
            string result = String.Empty;
            try
            {
                CreateBitmapFromByte(uncompressedImage, bmp);
                ImagePreparation imagePreparation = new ImagePreparation();
                int[][,] ai = imagePreparation.prepare(bmp);

                OpticalCharacterRecognition opticalcharacterrecognition = new OpticalCharacterRecognition();
                result = opticalcharacterrecognition.recognizeCharacters(ai);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                bmp.Dispose();
            }
            return result;
        }

        private void CreateBitmapFromByte(byte[] image, Bitmap bmp)
        {
            BitmapData bData = bmp.LockBits(new Rectangle(new Point(), bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            byte[] data1 = image;
            Marshal.Copy(data1, 0, bData.Scan0, image.GetUpperBound(0) + 1);
            bmp.UnlockBits(bData);
            bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
        }

        private byte[] UncompressedImage()
        {
            using (MemoryStream stream = new MemoryStream(_payload))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    br.BaseStream.Position += 3;
                    ushort compressed = br.ReadUInt16();
                    ushort uncompressed = br.ReadUInt16();
                    ushort width = br.ReadUInt16();
                    ushort height = br.ReadUInt16();
                    byte[] compressedImage = br.ReadBytes((int)compressed);
                    using (MemoryStream str = new MemoryStream(compressedImage))
                    {
                        using (InflaterInputStream zlip = new InflaterInputStream(str))
                        {
                            byte[] buffer = new byte[51200];
                            zlip.Read(buffer, 0, uncompressed);
                            return UncompressedImg(width, height, buffer);
                        }
                    }
                }
            }
        }

        private byte[] UncompressedImg(ushort width, ushort height, byte[] buffer)
        {
            byte[] uncompressedImage = new byte[width * height * 4]; // test array
            int imageIndex = 0;
            for (int c = 0; c < height; c++)
            {
                for (int d = 0; d < width; d++)
                {
                    imageIndex = (height - 1 - c) * width + d;
                    if ((-((1 << (int)(0xFF & (d & 0x80000007))) & buffer[((c * width + d) >> 3)])) != 0)
                    {
                        uncompressedImage[imageIndex * 4 + 0] = 0xFF; //red
                        uncompressedImage[imageIndex * 4 + 1] = 0xFF; //green
                        uncompressedImage[imageIndex * 4 + 2] = 0xFF; //blue
                        uncompressedImage[imageIndex * 4 + 3] = 0xFF; //alpha not used   
                    }
                    else
                    {
                        uncompressedImage[imageIndex * 4 + 0] = 0x0;  //blue
                        uncompressedImage[imageIndex * 4 + 1] = 0x0;  //green
                        uncompressedImage[imageIndex * 4 + 2] = 0x0; //red
                        uncompressedImage[imageIndex * 4 + 3] = 0xFF; //alpha not used

                    }
                }
            }
            return uncompressedImage;
        }

        
    }
}

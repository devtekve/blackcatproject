using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using worker;
using System.Windows.Forms;

namespace Decapcha
{
    public class ImagePreparation
    {
        public static readonly Worker worker = new Worker("ImagePreparation", 6);
        private int[,] _image;
        private int _width;
        private int _height;

        //ok
        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual int[][,] prepare(Bitmap bmp)
        {
            _image = fromImage(bmp);

            _width = _image.GetLength(0);
            _height = _image.GetLength(1);
            return doPrepare();
        }

        //ok
        private int[,] fromImage(Bitmap bmp)
        {
            int i = bmp.Width;
            int j = bmp.Height;
            int[,] ai = new int[i, j];
            for (int k = 0; k < j; k++)
            {
                for (int l = 0; l < i; l++)
                    ai[l, k] = bmp.GetPixel(l, k).ToArgb() & 0xffffff;
            }
            return ai;
        }

        #region private stuff cropImage
        private int[,] getSubimage(int[,] ai, int i, int j, int k, int l)
        {
            int i1 = ai.GetLength(0);
            int j1 = ai.GetLength(1);
            int[,] ai1 = new int[k, l];
            for (int k1 = 0; k1 < l; k1++)
            {
                int l1 = k1 + j;
                for (int i2 = 0; i2 < k; i2++)
                {
                    int j2 = i2 + i;
                    if (j2 < 0 || j2 >= i1 || l1 < 0 || l1 >= j1)
                        ai1[i2, k1] = 0;
                    else
                        ai1[i2, k1] = ai[j2, l1];
                }
            }
            return ai1;
        }

        private int[] findImageCodeLocation()
        {
            int i = _height - 68;
            int j = _width - 204;
            for (int k = 1; k < i; k++)
            {
                for (int l = 1; l < j; l++)
                    if (isImageCodeLocation(l, k))
                        return (new int[] { l, k });

            }

            throw new Exception("Image Code Border not found!");
        }
        #endregion

        #region private stuff findLines
        private void cleanLines()
        {
            double[,] ad = { { 0.10000000000000001D, 0.14999999999999999D, 0.10000000000000001D }, { 0.14999999999999999D, 0.0D, 0.14999999999999999D }, { 0.10000000000000001D, 0.14999999999999999D, 0.10000000000000001D } };
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    int k = _image[j, i];
                    if (k == 0)
                        continue;
                    int i1 = k & 0xff;
                    if (k != i1)
                        continue;
                    double d = 0.0D;
                    for (int i2 = -1; i2 <= 1; i2++)
                    {
                        int k2 = j + i2;
                        int l2 = i2 + 1;
                        for (int j3 = -1; j3 <= 1; j3++)
                        {
                            int k3 = i + j3;
                            if (k2 < 0 || k2 >= _width || k3 < 0 || k3 >= _height)
                                continue;
                            int l3 = _image[k2, k3];
                            int i4 = l3 & 0xff;
                            if (i4 != 0)
                            {
                                bool flag = (l3 & 0xff00) == 0;
                                double d2 = flag ? 0.5D : 2D;
                                int j4 = j3 + 1;
                                d += ad[l2, j4] * d2;
                            }
                        }
                    }
                    if (d >= 0.10000000000000001D)
                    {
                        int j2 = Math.Min(255, (int)(d * (double)i1));
                        _image[j, i] = j2 << 16 | i1;
                    }
                    else
                    {
                        _image[j, i] = 0;
                    }
                }
            }

            for (int l = 0; l < _height; l++)
            {
                for (int j1 = 0; j1 < _width; j1++)
                {
                    int k1 = _image[j1, l];
                    if (k1 == 0 || k1 == 0xffffff)
                        continue;
                    int l1 = k1 & 0xff0000;
                    if (l1 != 0)
                    {
                        double d1 = (double)((l1 >> 16) - 128) / 128D;
                        int i3 = Math.Max(0, Math.Min(255, (int)(Math.Sign(d1) * Math.Pow(Math.Abs(d1), 0.33333333333333331D) * 128D) + 128));
                        _image[j1, l] = i3 << 16 | i3 << 8 | i3;
                    }
                }
            }
        }
        #endregion

        #region private weightedWipe
        private void maskImage(int[,] ai, int i, int j, int k)
        {
            for (int l = 0; l < j; l++)
            {
                for (int i1 = 0; i1 < i; i1++)
                    ai[i1, l] = ai[i1, l] & k;
            }
        }

        private void markPixelGroupCount(int i, int j, int k)
        {
            int l = (int)Math.Min(255D, getPixelGroupCount(i, j, k));
            markPixelGroup(i, j, l);
        }

        private double getWeight(int i)
        {
            int j = 2 * i + 1;
            double f = 0.0D;
            for (int k = 0; k < j; k++)
            {
                for (int l = 0; l < j; l++)
                {

                    double f1 = double.Parse(Math.Sqrt(Math.Pow(i - l, 2D) + Math.Pow(i - k, 2D)).ToString("N6"));
                    f += double.Parse((1.0 / (1.0 + 5 * f1)).ToString("N8"));
                }
            }
            if (f != 0)
                f = double.Parse(f.ToString("N8")) - 0.00000012;
            return f;
        }

        private void transformBlue2Gray()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    int k = _image[j, i] & 0xff;
                    _image[j, i] = k << 16 | k << 8 | k;
                }

            }

        }
        #endregion

        #region hehe
        private void removeIslands(int[,] ai, int i, int j)
        {
            for (int k = 0; k < j; k++)
            {
                for (int l = 0; l < i; l++)
                {
                    int i1 = ai[l, k];
                    int j1 = i1 & 0xffff00;
                    if (j1 == 0 && !isFullHeight(ai, i, j, l, k))
                        revisit(ai, i, j, l, k);
                }

            }

        }

        private int getRGB(int[,] ai, int i, int j, int k, int l)
        {
            if (k < 0 || l < 0 || k >= i || l >= j)
                return 0;
            else
                return ai[k, l];
        }

        private int getRGB(int i, int j)
        {
            if (i < 0 || j < 0 || i >= _width || j >= _height)
                return 0;
            else
                return _image[i, j];
        }

        private void findLines(Border border)
        {
            int i;
            int j;
            int k;
            int l;
            if (border == Border.BOTTOM)
            {
                i = 0;
                j = _width;
                k = _height - 4;
                l = _height;
            }
            else
                if (border == Border.RIGHT)
                {
                    i = _width - 4;
                    j = _width;
                    k = 0;
                    l = _height;
                }
                else
                    if (border == Border.LEFT)
                    {
                        i = 0;
                        j = 4;
                        k = 0;
                        l = _height;
                    }
                    else
                        if (border == Border.TOP)
                        {
                            i = 0;
                            j = _width;
                            k = 0;
                            l = 4;
                        }
                        else
                        {
                            return;
                        }
            for (int i1 = i; i1 < j; i1++)
            {
                for (int j1 = k; j1 < l; j1++)
                {
                    if (border != Border.RIGHT)
                        findLine(i1, j1, border, true);
                    if (border != Border.BOTTOM)
                        findLine(i1, j1, border, false);
                }

            }

        }

        private void findLine(int i, int j, Border border, bool flag)
        {
            if ((_image[i, j] & 0xff) == 0)
                return;
            int k = flag ? _width : _height;
            for (int l = 1; l < k; l++)
                findLine(i, j, l, border, flag);

        }

        private void findLine(int i, int j, int k, Border border, bool flag)
        {
            for (int l = 0; l < 2; l++)
                findLine(i, j, k, l != 0 ? -1 : 1, 1, true, 0, border, flag);

        }

        private bool findLine(int i, int j, int k, int l, int i1, bool flag, int j1, Border border, bool flag1)
        {
            if (i == -1 || i == _width || j == -1 || j == _height)
                return true;
            bool flag2 = false;
            if (j1 > 2)
            {
                if (_width - i < 4 && border != Border.RIGHT)
                    flag2 = true;
                if (_height - j < 4 && border != Border.BOTTOM)
                    flag2 = true;
                if (j < 4 && border != Border.TOP)
                    flag2 = true;
                if (i < 4 && border != Border.LEFT)
                    flag2 = true;
            }
            if ((_image[i, j] & 0xff) == 0)
                return flag2;
            bool flag3 = false;
            int k1 = -99;
            int l1 = -99;
            if (flag1)
                k1 = i + 1;
            else
                l1 = j + 1;
            if (i1 <= k)
            {
                if (flag1)
                    l1 = j;
                else
                    k1 = i;
                if (findLine(k1, l1, k, l, i1 + 1, flag, j1 + 1, border, flag1))
                    flag3 = true;
            }
            if (i1 >= k || flag)
            {
                if (flag1)
                    l1 = j + l;
                else
                    k1 = i + l;
                if (findLine(k1, l1, k, l, 1, false, j1 + 1, border, flag1))
                    flag3 = true;
            }
            if (flag3)
                _image[i, j] = 255;
            return flag3 || flag2;
        }

        private void markPixelGroup(int i, int j, int k)
        {
            if (i < 0 || i >= _width || j < 0 || j >= _height)
                return;
            int l = _image[i, j];
            if (l == 0)
                return;
            int i1 = l & 0xff;
            if (i1 == 0)
            {
                return;
            }
            else
            {
                _image[i, j] = l & 0xff00 | k << 16;
                markPixelGroup(i - 1, j, k);
                markPixelGroup(i + 1, j, k);
                markPixelGroup(i, j - 1, k);
                markPixelGroup(i, j + 1, k);
                return;
            }
        }

        private double getPixelGroupCount(int i, int j, int k)
        {
            if (i < 0 || i >= _width || j < 0 || j >= _height)
                return 0.0D;
            int l = _image[i, j];
            if (l == 0)
                return 0.0D;
            int i1 = (l & 0xff00) >> 8;
            if (i1 < k)
                return 0.0D;
            int j1 = l & 0xff;
            if (j1 != 0)
                return 0.0D;
            int k1 = l & 0xff0000;
            if (k1 != 0)
            {
                return 0.0D;
            }
            else
            {
                _image[i, j] = l | 0xff;
                double d = (double)i1 / 255D;
                d += getPixelGroupCount(i - 1, j, k);
                d += getPixelGroupCount(i + 1, j, k);
                d += getPixelGroupCount(i, j - 1, k);
                d += getPixelGroupCount(i, j + 1, k);
                return d;
            }
        }

        private void transform2Gray()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    int k = _image[j, i];
                    int l = (k & 0xff0000) >> 16;
                    int i1 = (k & 0xff00) >> 8;
                    int j1 = (k & 0xff) >> 0;
                    int k1 = (l + i1 + j1) / 3 & 0xff;
                    _image[j, i] = k1 << 16 | k1 << 8 | k1;
                }

            }

        }

        private bool isImageCodeLocation(int i, int j)
        {
            if (!isUpperImageCodeBorder(i, j))
                return false;
            if (!isLowerImageCodeBorder(i, j))
                return false;
            if (!isLeftImageCodeBorder(i, j))
                return false;
            return isRightImageCodeBorder(i, j);
        }

        private bool isUpperImageCodeBorder(int i, int j)
        {
            int k = j - 1;
            for (int l = 0; l < 204; l++)
                if ((_image[(i + l) - 1, k] & 0xff) != 255)
                    return false;

            return true;
        }

        private bool isLowerImageCodeBorder(int i, int j)
        {
            int k = j + 66;
            for (int l = 0; l < 204; l++)
                if ((_image[(i + l) - 1, k] & 0xff) != 255)
                    return false;

            return true;
        }

        private bool isLeftImageCodeBorder(int i, int j)
        {
            int k = i - 1;
            for (int l = 0; l < 68; l++)
                if ((_image[k, (j + l) - 1] & 0xff) != 255)
                    return false;

            return true;
        }

        private bool isRightImageCodeBorder(int i, int j)
        {
            int k = i + 202;
            for (int l = 0; l < 68; l++)
                if ((_image[k, (j + l) - 1] & 0xff) != 255)
                    return false;

            return true;
        }

        private double calculateRotationHeight(int i, int j, double d)
        {
            double d1 = (d / 180D) * 3.1415926535897931D;
            double d2 = Math.Cos(d1);
            double d3 = Math.Sin(d1);
            double d4 = -1000D;
            double d5 = 1000D;
            for (int k = 0; k < _height; k++)
            {
                int l = k - j;
                for (int i1 = 0; i1 < _width; i1++)
                {
                    if ((_image[i1, k] & 0xff) == 0)
                        continue;
                    int j1 = i1 - i;
                    double d6 = (double)l * d2 + (double)j1 * d3;
                    if (d6 > d4)
                        d4 = d6;
                    if (d6 < d5)
                        d5 = d6;
                }

            }

            return d4 - d5;
        }

        private void revisit(int[,] ai, int i, int j, int k, int l)
        {
            if (k < 0 || k >= i)
                return;
            if (l < 0)
                return;
            if (l >= j)
                return;
            if ((ai[k, l] & 0xffff00) != 65280)
            {
                return;
            }
            else
            {
                ai[k, l] = ai[k, l] | 0x60ff00;
                revisit(ai, i, j, k + 1, l);
                revisit(ai, i, j, k - 1, l);
                revisit(ai, i, j, k, l + 1);
                revisit(ai, i, j, k, l - 1);
                return;
            }
        }

        private bool isFullHeight(int[,] ai, int i, int j, int k, int l)
        {
            return visit(ai, i, j, k, l);
        }

        private bool visit(int[,] ai, int i, int j, int k, int l)
        {
            bool flag = false;
            bool flag1 = false;
            LinkedList<int[]> linkedlist = new LinkedList<int[]>();
            linkedlist.AddLast(new int[] { k, l });
            do
            {

                if (linkedlist.Count == 0)
                    break;

                int[] ai1 = (int[])linkedlist.Last.Value;
                linkedlist.RemoveLast();
                int i1 = ai1[0];
                int j1 = ai1[1];
                if (i1 >= 0 && i1 < i)
                    if (j1 < 0)
                        flag = true;
                    else
                        if (j1 >= j)
                            flag1 = true;
                        else
                            if ((ai[i1, j1] & 0xffff00) == 0)
                            {
                                ai[i1, j1] = ai[i1, j1] | 0xff00;
                                linkedlist.AddLast(new int[] { i1 + 1, j1 });
                                linkedlist.AddLast(new int[] { i1 - 1, j1 });
                                if (!flag1)
                                    linkedlist.AddLast(new int[] { i1, j1 + 1 });
                                if (!flag)
                                    linkedlist.AddLast(new int[] { i1, j1 - 1 });
                            }
            } while (true);
            return flag && flag1;
        }

        private void drawRedLine(int[,] ai, int i, int j, int k, int l, int i1, int j1)
        {
            int k1 = 0;
            int l1 = l;
            int i2 = Math.Abs(i1);
            for (int j2 = 0; l1 < j; j2++)
            {
                if (j2 >= i2)
                {
                    j2 = 0;
                    if (i1 > 0)
                        k1++;
                    else
                        k1--;
                }
                if (l1 >= 0)
                {
                    int k2 = k + k1;
                    if (k2 < 0 || k2 >= i)
                        return;
                    ai[k2, l1] = ai[k2, l1] | j1 << 16;
                }
                l1++;
            }

        }

        private Hashtable calculateLines(int[,] ai, int i, int j)
        {
            try
            {

                Hashtable hashmap = new Hashtable();
                for (int k = j; k >= -j; k--)
                {
                    if (Math.Abs(k) <= 3)
                        continue;
                    int l = 0;
                    do
                    {
                        if (l < -Math.Abs(k))
                            break;
                        for (int i1 = 0; i1 < i; i1++)
                        {
                            long l1 = calculateLineCostsBlue(ai, i, j, i1, l, k);
                            if (l1 == 0x7fffffffffffffffL)
                                continue;
                            ArrayList obj;
                            try
                            {
                                obj = (ArrayList)hashmap[Convert.ToInt64(l1)];
                            }
                            catch
                            {
                                obj = null;
                            }

                            if (obj == null)
                            {
                                obj = new ArrayList();
                                hashmap.Add(Convert.ToInt64(l1), obj);
                            }
                            obj.Add(new int[] { i1, l, k });
                        }

                        l--;
                    } while (true);
                }


                return hashmap;
            }
            catch (Exception e)
            {
                Console.WriteLine("calculate: " + e.ToString());
                return null;
            }
        }

        private long calculateLineCostsBlue(int[,] ai, int i, int j, int k, int l, int i1)
        {
            long l1 = 0L;
            int j1 = k;
            int k1 = l;
            int i2 = Math.Abs(i1);
            for (int j2 = 0; k1 < j; j2++)
            {
                if (j2 >= i2)
                {
                    j2 = 0;
                    if (i1 > 0)
                        j1++;
                    else
                        j1--;
                    if (j1 >= i || j1 <= -1)
                        return 0x7fffffffffffffffL;
                }
                if (k1 >= 0)
                    l1 += getRGB(ai, i, j, j1, k1) & 0xff;
                k1++;
            }

            return l1;
        }
        #endregion

        private int[,] shrinkImage(int[,] ai, int i, int j, int k, bool flag)
        {
            int l = i;
            int i1 = -1;
            int j1 = flag ? j : 0;
            int k1 = flag ? -1 : j - 1;
            for (int l1 = 0; l1 < j; l1++)
            {
                for (int j2 = 0; j2 < i; j2++)
                {
                    if ((ai[j2, l1] & 0xff) <= k)
                        continue;
                    l = Math.Min(l, j2);
                    i1 = Math.Max(i1, j2);
                    if (flag)
                    {
                        j1 = Math.Min(j1, l1);
                        k1 = Math.Max(k1, l1);
                    }
                }

            }

            int i2 = (i1 - l) + 1;
            int k2 = (k1 - j1) + 1;
            int l2 = l;
            int i3 = j1;
            int[,] ai1 = new int[i2, k2];
            for (int j3 = 0; j3 < k2; j3++)
            {
                int k3 = j3 + i3;
                for (int l3 = 0; l3 < i2; l3++)
                {
                    int i4 = l3 + l2;
                    ai1[l3, j3] = ai[i4, k3];
                }

            }

            return ai1;
        }

        private int calculateWithForShift(int[,] ai, int i, int j, int k, int l)
        {
            int i1 = i;
            int j1 = -1;
            int k1 = 0;
            int l1 = 0;
            int i2 = Math.Abs(k);
            for (int j2 = 0; l1 < j; j2++)
            {
                if (j2 >= i2)
                {
                    j2 = 0;
                    if (k < 0)
                        k1++;
                    else
                        k1--;
                }
                for (int k2 = 0; k2 < i; k2++)
                {
                    int l2 = k2 + k1;
                    if ((l2 >= i1 && l2 <= j1) || ((ai[k2, l1] & 255) <= l))
                        continue;
                    if (l2 < i1)
                        i1 = l2;
                    if (l2 > j1)
                        j1 = l2;
                }

                l1++;
            }

            return (j1 - i1) + 1;
        }

        private long insertRedLines(int[,] ai, int i, int j, Hashtable map)
        {
            try
            {
                maskImage(ai, i, j, 65535); // ok             
                ArrayList sort = new ArrayList(map.Keys);
                sort.Sort();
                for (IEnumerator iterator = sort.GetEnumerator(); iterator.MoveNext(); )
                {

                    long l = (long)iterator.Current;
                    int[] ai1;


                    for (IEnumerator iterator1 = ((ArrayList)map[Convert.ToInt64(l)]).GetEnumerator(); iterator1.MoveNext(); drawRedLine(ai, i, j, ai1[0], ai1[1], ai1[2], l != 0L ? 160 : 255))
                        ai1 = (int[])iterator1.Current;

                    maskImage(ai, i, j, 0xff00ff);
                    removeIslands(ai, i, j);
                    int k = 0;
                    int i1 = j / 2;
                    bool flag = true;
                    for (int j1 = 0; j1 < i; j1++)
                    {
                        int k1 = ai[j1, i1] & 0xff0000;
                        if (k1 == 0)
                        {
                            if (flag)
                            {
                                flag = false;
                                k++;
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                    }

                    if (k == 6)
                        return l;
                    if (k == 0)
                        throw new Exception("No Groups found!");
                }

                return 0x7fffffffffffffffL;
            }
            catch (Exception e)
            {
                Console.WriteLine("insert: " + e.ToString());
                return 0L;
            }
        }

        private int[,] widenImageForShift(int[,] ai, int i, int j)
        {
            int k = j + 10;
            int l = i + k * 2;
            int[,] ai1 = new int[l, j];
            for (int i1 = 0; i1 < j; i1++)
            {
                for (int j1 = 0; j1 < l; j1++)
                {
                    int k1 = j1 - k;
                    ai1[j1, i1] = getRGB(ai, i, j, k1, i1);
                }

            }

            return ai1;
        }

        private void shiftImageForLines(int[,] ai, int i, int j, int k)
        {
            int[] ai1 = new int[i];
            int l = 0;
            int i1 = 0;
            int j1 = Math.Abs(k);
            for (int k1 = 0; i1 < j; k1++)
            {
                if (k1 >= j1)
                {
                    k1 = 0;
                    if (k < 0)
                        l++;
                    else
                        l--;
                }
                if (l != 0)
                {
                    for (int l1 = 0; l1 < i; l1++)
                        ai1[l1] = ai[l1, i1];

                    for (int i2 = 0; i2 < i; i2++)
                    {
                        int j2 = i2 + l;
                        if (j2 >= 0 && j2 < i)
                            ai[j2, i1] = ai1[i2];
                    }

                }
                i1++;
            }

        }

        #region private stuff doPrepare
        private void transform2BlackAndWhite(int[,] ai, int i, int j, int k)
        {
            for (int l = 0; l < j; l++)
            {
                for (int i1 = 0; i1 < i; i1++)
                {
                    int j1 = ai[i1, l];
                    int k1 = (j1 & 0xff0000) >> 16;
                    int l1 = (j1 & 0xff00) >> 8;
                    int i2 = (j1 & 0xff) >> 0;
                    if ((k1 + l1 + i2) / 3 >= k)
                        ai[i1, l] = 0xffffff;
                    else
                        ai[i1, l] = 0;
                }
            }
        }

        private void cropImage()
        {
            if (_width > 202 || _height > 66)
            {
                int[] ai = findImageCodeLocation();
                _image = getSubimage(_image, ai[0], ai[1], 202, 66);
                _width = _image.GetLength(0);
                _height = _image.GetLength(1);
            }
        }

        private void findLines()
        {
            findLines(Border.LEFT);
            findLines(Border.TOP);
            findLines(Border.BOTTOM);
            findLines(Border.RIGHT);
            removeSinglePixels();
            cleanLines();
        }

        private void weightedWipe(int i, int j, int k)
        {
            maskImage(_image, _width, _height, 65280);
            for (int l = 0; l < _height; l++)
            {
                for (int i1 = 0; i1 < _width; i1++)
                    markPixelGroupCount(i1, l, j);

            }
            double f = getWeight(i);
            int j1 = 2 * i + 1;
            for (int k1 = 0; k1 < _height; k1++)
            {
                for (int l1 = 0; l1 < _width; l1++)
                {
                    int i2 = _image[l1, k1];
                    int j2 = i2 & 0xff00;
                    if (j2 >> 8 > 240 && (i2 & 0xff0000) >> 16 > k)
                    {
                        _image[l1, k1] = j2 | 0xff;
                        continue;
                    }
                    double f1 = 0.0;
                    for (int k2 = 0; k2 < j1; k2++)
                    {
                        for (int l2 = 0; l2 < j1; l2++)
                        {
                            int i3 = (getRGB((l1 + l2) - i, (k1 + k2) - i) & 0xff00) >> 8;
                            double f2 = double.Parse(Math.Sqrt(Math.Pow(i - l2, 2D) + Math.Pow(i - k2, 2D)).ToString("N6"));
                            f1 += double.Parse((i3 / (1.0 + 5 * f2)).ToString("N8"));
                        }
                    }
                    if (f1 != 0)
                        f1 = double.Parse(f1.ToString("N8")) - 0.00000012;
                    _image[l1, k1] = j2 | Math.Max(0, Math.Min((int)(f1 / f), 255));
                }

            }

            transformBlue2Gray();
        }

        private void removeSinglePixels()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    int k = _image[j, i];
                    int l = k & 0xff00;
                    if (l == 0)
                        continue;
                    bool flag = true;
                    for (int i1 = -1; i1 <= 1 && flag; i1++)
                    {
                        int j1 = i + i1;
                        if (j1 < 0 || j1 >= _height)
                            continue;
                        for (int k1 = -1; k1 <= 1 && flag; k1++)
                        {
                            if (i1 == 0 && k1 == 0)
                                continue;
                            int l1 = j + k1;
                            if (l1 < 0 || l1 >= _width)
                                continue;
                            int i2 = _image[l1, j1];
                            int j2 = i2 & 0xff00;
                            if (j2 != 0)
                                flag = false;
                        }

                    }

                    if (flag)
                        _image[j, i] = 0;
                }
            }
        }

        private void shrinkImage(int i)
        {
            _image = shrinkImage(_image, _width, _height, i, true);
            _width = _image.GetLength(0);
            _height = _image.GetLength(1);
        }

        private void enlargeImage()
        {
            sbyte byte0 = 10;
            sbyte byte1 = 5;
            int i = _width + byte0 * 2;
            int j = _height + byte1 * 2;
            int[,] ai = new int[i, j];
            for (int k = 0; k < j; k++)
            {
                int l = k - byte1;
                for (int i1 = 0; i1 < i; i1++)
                {
                    int j1 = i1 - byte0;
                    ai[i1, k] = getRGB(j1, l);
                }
            }

            _image = ai;
            _width = i;
            _height = j;
        }

        private void rotateImage()
        {
            int i = _width / 2;
            int j = _height / 2;
            double d1 = -1;
            int k = _height;
            for (int l = 0; l < _height; l++)
            {
                for (int i1 = 0; i1 < _width; i1++)
                {
                    if ((_image[i1, l] & 0xff) == 0)
                        continue;
                    if (l > d1)
                        d1 = l;
                    if (l < k)
                        k = l;
                }

            }

            double d = d1 - k;
            d1 = d;
            double d2 = 1.0D;
            double d3 = 0.0D;
            bool flag = false;

            while (Math.Abs(d2) > 0.0001D)
            {
                double d4 = d3 + d2;
                double d6 = calculateRotationHeight(i, j, d4);
                if (d6 < d1)
                {
                    d1 = d6;
                    d3 = d4;
                    flag = true;
                }
                else
                    if (!flag)
                    {
                        d2 = -d2;
                        flag = true;
                    }
                    else
                    {
                        d2 /= 2D;
                        flag = false;
                    }
            }
            double d5 = (d3 / 180D) * 3.1415926535897931D;
            double d7 = Math.Cos(d5);
            double d8 = Math.Sin(d5);

            int[,] ai = new int[_width, _height];
            for (int j1 = 0; j1 < _height; j1++)
            {
                int k1 = j1 - j;
                for (int l1 = 0; l1 < _width; l1++)
                {
                    int i2 = _image[l1, j1] & 0xff;
                    if (i2 == 0)
                        continue;
                    int j2 = l1 - i;
                    double d9 = ((double)j2 * d7 - (double)k1 * d8) + (double)i;
                    double d10 = (double)k1 * d7 + (double)j2 * d8 + (double)j;
                    int k2 = (int)d9;
                    int l2 = (int)d10;
                    int i3 = k2 + 1;
                    int j3 = l2 + 1;
                    double d11 = d9 - (double)k2;
                    double d12 = 1.0D - d11;
                    double d13 = d10 - (double)l2;
                    double d14 = 1.0D - d13;
                    int[,] ai1 = { { k2, l2, (int)((double)i2 * d12 * d14) },
                                   { i3, l2, (int)((double)i2 * d11 * d14) },
                                   { k2, j3, (int)((double)i2 * d12 * d13) },
                                   { i3, j3, (int)((double)i2 * d11 * d13) } };
                    int[,] ai2 = ai1;
                    int k3 = ai2.GetLength(0);
                    for (int l3 = 0; l3 < k3; l3++)
                    {

                        int[] ai3 = new int[ai2.GetLength(1)];
                        for (int y = 0; y < ai2.GetLength(1); y++)
                            ai3[y] = ai2[l3, y];

                        int i4 = Math.Max(0, Math.Min(255, ai3[2]));
                        if (i4 != 0)
                        {
                            int j4 = ai3[0];
                            int k4 = ai3[1];
                            int l4 = ai[j4, k4] & 0xff;
                            int i5 = Math.Max(0, Math.Min(i4 + l4, 255));
                            ai[j4, k4] = i5 << 16 | i5 << 8 | i5;
                        }
                    }

                }

            }

            _image = ai;
        }

        private void killJitter()
        {
            try
            {
                Hashtable hashmap = new Hashtable();
                for (int i = 0; i < _width; i++)
                {
                    int j = 0;
                    for (int i1 = 0; i1 < _height; i1++)
                    {
                        int l1 = _image[i, i1];
                        int i2 = l1 & 0xff;
                        if (i2 < 16)
                        {
                            i2 = 0;
                            _image[i, i1] = 0;
                        }
                        j += i2;
                    }
                    hashmap.Add(Convert.ToInt32(i), Convert.ToInt32(j));
                }


                Hashtable hashmap1 = new Hashtable();
                for (int k = 0; k < _width; k++)
                {
                    int j1 = (int)hashmap[Convert.ToInt32(k)];

                    if (j1 == 0)
                    {
                        hashmap1.Add(Convert.ToInt32(k), true);
                        continue;
                    }
                    bool flag = true;
                    int j2 = j1 / 400;
                    if (j2 > 0)
                    {
                        for (int k2 = Math.Max(0, k - j2); k2 <= Math.Min(_width - 1, k + j2); k2++)
                        {
                            if (k2 == k)
                                continue;
                            int l2 = (int)hashmap[Convert.ToInt32(k2)];
                            if (l2 < j1)
                                continue;
                            flag = false;
                            break;
                        }

                    }
                    hashmap1.Add(Convert.ToInt32(k), flag);
                }

                for (int l = 0; l < _width; l++)
                {

                    if ((bool)hashmap1[Convert.ToInt32(l)] == false || l > 0 && (bool)hashmap1[Convert.ToInt32(l - 1)] == false || l < _width - 1 && (bool)hashmap1[Convert.ToInt32(l + 1)] == false)
                        continue;
                    for (int k1 = 0; k1 < _height; k1++)
                        _image[l, k1] = 0;

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void wipe(int i)
        {
            double f = getWeight(i);
            int j = 2 * i + 1;
            for (int k = 0; k < _height; k++)
            {
                for (int l = 0; l < _width; l++)
                {

                    double f1 = 0.0;
                    for (int i1 = 0; i1 < j; i1++)
                    {
                        for (int j1 = 0; j1 < j; j1++)
                        {
                            int l1 = (getRGB((l + j1) - i, (k + i1) - i) & 0xff00) >> 8;
                            double f3 = double.Parse(Math.Sqrt(Math.Pow(i - j1, 2D) + Math.Pow(i - i1, 2D)).ToString("N6"));
                            f1 += double.Parse((l1 / (1.0 + 5 * f3)).ToString("N8"));
                        }
                    }
                    if (f1 != 0)
                        f1 = double.Parse(f1.ToString("N8")) - 0.00000012;

                    double f2 = f1 / f;
                    f2 = double.Parse(f2.ToString("N8"));
                    int k1 = Math.Max(0, Math.Min((int)f2, 255));
                    _image[l, k] = _image[l, k] & 0xffff00 | k1 & 0xff;
                }

            }

            transformBlue2Gray();
        }

        private List<int> calculateBestShifts(int[,] ai, int i, int j)
        {
            List<int> list = new List<int>();
            int k = i;
            for (int l = -j; l <= j; l++)
            {
                if (Math.Abs(l) <= 0)
                    continue;
                int i1 = calculateWithForShift(ai, i, j, l, 254);
                if (i1 > k)
                    continue;
                if (i1 < k)
                {
                    list.Clear();
                    k = i1;
                }
                list.Add(l);
            }

            return list;
        }

        public long insertRedLines(int[,] ai, int i, int j)
        {
            Hashtable map = calculateLines(ai, i, j); // coi ham nay
            return insertRedLines(ai, i, j, map);
        }

        public int[,] shiftImage(int[,] ai, int i, int j, int k)
        {
            int[,] ai1 = widenImageForShift(ai, i, j);
            int l = ai1.GetLength(0);
            shiftImageForLines(ai1, l, j, k);
            return shrinkImage(ai1, l, j, 0, true);
        }

        private int[][,] extractSymbols()
        {
            try
            {
                int i = _image.GetLength(0);
                int j = _image.GetLength(1);

                int[][,] ai = new int[6][,]{
                          new int[i,j],
                          new int[i,j],
                          new int[i,j],
                          new int[i,j],
                          new int[i,j],
                          new int[i,j]
            };
                for (int k = 0; k < j; k++)
                {
                    bool flag = true;
                    bool flag1 = false;
                    int l1 = 5;
                    int j2 = 0;
                    int i3 = -1;
                    for (int k3 = -i; k3 <= i; k3++)
                    {
                        if (k3 == 0)
                        {
                            i3 = -i3;
                            l1 = 0;
                            j2 = 0;
                            flag = true;
                            flag1 = false;
                            continue;
                        }
                        int i4 = Math.Abs(k3) - 1;
                        int k4 = _image[i4, k];
                        int i5 = k4 & 0xff0000;
                        int j5 = k4 & 0xff;
                        if (i5 != 0)
                        {
                            if (flag)
                                continue;
                            flag1 = true;
                            if (i5 == 0xff0000)
                            {
                                flag = true;
                                continue;
                            }
                            if (j5 > j2)
                            {
                                flag = true;
                                continue;
                            }
                        }
                        else
                            if (flag1)
                            {
                                l1 += i3;
                                j2 = 0;
                                flag = true;
                                flag1 = false;
                                k3--;
                                continue;
                            }
                        flag = false;
                        ai[l1][i4, k] = j5 << 16 | j5 << 8 | j5;
                        j2 = j5;
                    }

                }

                for (int l = 0; l < ai.Length; l++)
                {
                    int[,] ai2 = ai[l];
                    int j1 = i;
                    int i2 = -1;
                    for (int k2 = 0; k2 < j; k2++)
                    {
                        for (int j3 = 0; j3 < i; j3++)
                            if (ai2[j3, k2] != 0)
                            {
                                j1 = Math.Min(j1, j3);
                                i2 = Math.Max(i2, j3);
                            }

                    }

                    int l2 = (i2 - j1) + 1;

                    int[,] ai4 = new int[l2, j];
                    for (int l3 = 0; l3 < l2; l3++)
                    {
                        int j4 = l3 + j1;
                        for (int l4 = 0; l4 < j; l4++)
                            ai4[l3, l4] = ai2[j4, l4];

                    }
                    ai[l] = ai4;
                }
                return ai;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
                return null;
            }
        }

        private int[][,] shiftSymbols(int[][,] ai)
        {
            if (ai == null || ai.Length == 0)
                return ai;
            int i = 0;
            for (int j = 0; j < ai.Length; j++)
                i += ai[j].GetLength(0);

            int k = 0;
            int l = ai[0].GetLength(1);
            for (int i1 = -l; i1 <= l; i1++)
            {
                if (i1 == 0)
                    continue;
                int k1 = 0;
                for (int l1 = 0; l1 < ai.Length; l1++)
                    k1 += calculateWithForShift(ai[l1], ai[l1].GetLength(0), l, i1, 0);

                if (k1 <= i && (k1 != i || Math.Abs(i1) >= Math.Abs(k)))
                {
                    i = k1;
                    k = i1;
                }
            }

            if (k == 0)
                return ai;
            for (int j1 = 0; j1 < ai.Length; j1++)
            {
                ai[j1] = widenImageForShift(ai[j1], ai[j1].GetLength(0), l);
                shiftImageForLines(ai[j1], ai[j1].GetLength(0), l, k);
                ai[j1] = shrinkImage(ai[j1], ai[j1].GetLength(0), l, 0, false);
            }

            return ai;
        }
        #endregion

        private int[][,] doPrepare()
        {
            transform2BlackAndWhite(_image, _width, _height, 96);
            cropImage();
            findLines();
            weightedWipe(2, 128, 10);
            transform2BlackAndWhite(_image, _width, _height, 128);
            removeSinglePixels();
            shrinkImage(0);
            enlargeImage();
            rotateImage();
            weightedWipe(2, 40, 8);
            killJitter();
            shrinkImage(16);
            transform2BlackAndWhite(_image, _width, _height, 96);
            removeSinglePixels();
            wipe(1);
            List<int> list = calculateBestShifts(_image, _width, _height);
            if (list.Count == 0)
            {
                insertRedLines(_image, _width, _height);
            }
            else
            {
                if (list.Count == 1)
                {
                    int i = (int)list[0];
                    _image = shiftImage(_image, _width, _height, i);
                    _width = _image.GetLength(0);
                    _height = _image.GetLength(1);
                    insertRedLines(_image, _width, _height);
                }
                else
                {
                    long l = 0x7fffffffffffffffL;
                    int[,] ai4 = _image;
                    ArrayList arraylist = new ArrayList(list.Count);
                    int i2;
                    for (IEnumerator iterator = list.GetEnumerator(); iterator.MoveNext(); arraylist.Add(FindBestShift.asyncExec(_image, _width, _height, i2)))
                        i2 = (int)iterator.Current;

                    IEnumerator iterator1 = arraylist.GetEnumerator();

                    do
                    {
                        if (!iterator1.MoveNext())
                            break;
                        Future future = (Future)iterator1.Current;
                        try
                        {

                            FindBestShift.Result result = (FindBestShift.Result)future.waitFor();

                            if (result.cost >= l)
                                continue;
                            l = result.cost;
                            ai4 = result.image;
                            if (l == 0L)
                                break;
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.ToString());
                        }
                    } while (true);
                    if (_image != ai4)
                    {
                        _image = ai4;
                        _width = _image.GetLength(0);
                        _height = _image.GetLength(1);
                    }
                    else
                    {
                        insertRedLines(_image, _width, _height);
                    }
                }
            }

            int[][,] ai = extractSymbols();
            int[][,] ai1 = ai;
            int j = ai1.Length;
            for (int j1 = 0; j1 < j; j1++)
            {
                int[,] ai5 = ai1[j1];
                transform2BlackAndWhite(ai5, ai5.GetLength(0), ai5.GetLength(1), 100);
            }

            shiftSymbols(ai);
            return ai;
        }
    }
}
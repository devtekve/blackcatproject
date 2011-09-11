using System;
using System.Collections;
using System.Threading;
using worker;

namespace ocr
{

    public class SymbolRecognition : Job
    {
        public class Params
        {

            internal readonly int[,] symbol;
            internal readonly ArrayList references;


            public Params(int[,] ai, ArrayList list)
            {
                symbol = ai;
                references = list;

            }
        }

        public static Future asyncExec(int[,] ai, ArrayList list)
        {
            return instance.asyncExec(new Params(ai, list));
        }

        public static OCRCharacterResult syncExec(int[,] ai, ArrayList list)
        {
            return (OCRCharacterResult)instance.syncExec(new Params(ai, list));
        }
        private SymbolRecognition(Worker worker1)
            : base(worker1)
        {
        }

        protected internal OCRCharacterResult run(Params params1)
        {
            double d = 4.9406564584124654E-324D;
            int[] ai = { -1, -1 };

            ReferenceCharacter referencecharacter = ReferenceCharacter.INVALID;
            IEnumerator iterator = params1.references.GetEnumerator();
            do
            {
                if (!iterator.MoveNext())
                    break;
                ReferenceCharacter referencecharacter1 = (ReferenceCharacter)iterator.Current;
                double[] ad = calculateMatch(params1.symbol, referencecharacter1.image);
                double d1 = ad[0];
                if (d1 > d)
                {
                    d = d1;
                    ai[0] = (int)ad[1];
                    ai[1] = (int)ad[2];
                    referencecharacter = referencecharacter1;
                }
            } while (true);

            if (referencecharacter == ReferenceCharacter.INVALID)
                return OCRCharacterResult.INVALID;
            else
                return new OCRCharacterResult(referencecharacter, d);
        }

        private double[] calculateMatch(int[,] ai, int[,] ai1)
        {
            double[] ad = { 4.9406564584124654E-324D, -1D, -1D };

            int i = ai.GetLength(0);
            int j = ai.GetLength(1);
            int k = ai1.GetLength(0);
            int l = ai1.GetLength(1);
            int i1 = Math.Max(k, i) + 2;
            int j1 = Math.Max(l, j) + 2;
            int k1 = i1 / 2;
            int l1 = j1 / 2;
            int[,] ai2 = new int[i1, j1];
            int i2 = k1 - i / 2;
            int j2 = l1 - j / 2;
            int k2 = j1 - l;
            int l2 = i1 - k;
            int i3 = Math.Min(k2 / 4, 5);
            int j3 = Math.Min(l2 / 4, 5);
            for (int k3 = -i3; k3 <= i3; k3++)
            {
                int l3 = (l1 + k3) - l / 2;
                for (int i4 = -j3; i4 <= j3; i4++)
                {
                    int j4 = (k1 + i4) - k / 2;
                    mergeImages(ai, i, j, i2, j2, ai1, k, l, j4, l3, ai2, i1, j1);

                    double d = calculateMatch(ai2, i1, j1);
                    if (d > ad[0])
                    {
                        ad[0] = d;
                        ad[1] = j4;
                        ad[2] = l3;
                    }
                }

            }

            return ad;
        }

        private int[,] mergeImages(int[,] ai, int[,] ai1, int i, int j)
        {
            int k = ai.GetLength(0);
            int l = ai.GetLength(1);
            int i1 = ai1.GetLength(0);
            int j1 = ai1.GetLength(1);
            int k1 = Math.Max(i1, k) + 2;
            int l1 = Math.Max(j1, l) + 2;
            int i2 = k1 / 2;
            int j2 = l1 / 2;
            int k2 = i2 - k / 2;
            int l2 = j2 - l / 2;

            int[,] ai2 = new int[k1, l1];
            mergeImages(ai, k, l, k2, l2, ai1, i1, j1, i, j, ai2, k1, l1);
            return ai2;
        }

        private void mergeImages(int[,] ai, int i, int j, int k, int l, int[,] ai1, int i1, int j1, int k1, int l1, int[,] ai2, int i2, int j2)
        {
            for (int k2 = 0; k2 < j2; k2++)
            {
                int l2 = k2 - l1;
                int i3 = k2 - l;
                bool flag = l2 >= 0 && l2 < j1;
                bool flag1 = i3 >= 0 && i3 < j;
                for (int j3 = 0; j3 < i2; j3++)
                {
                    int k3 = j3 - k1;
                    int l3 = j3 - k;
                    bool flag2 = flag && k3 >= 0 && k3 < i1;
                    bool flag3 = flag1 && l3 >= 0 && l3 < i;
                    int i4 = flag3 ? ai[l3, i3] : 0;
                    int j4 = flag2 ? ai1[k3, l2] : 0;
                    int k4 = i4 & 0xff0000 | j4 & 0xff;
                    if (flag3 || flag2)
                        k4 |= 0x2000;
                    ai2[j3, k2] = k4;
                }

            }

        }

        private double calculateMatch(int[,] ai, int i, int j)
        {
            double d = 0.0D;
            int k = 0;
            for (int l = 0; l < j; l++)
            {
                for (int i1 = 0; i1 < i; i1++)
                {
                    int j1 = ai[i1, l];
                    int k1 = (j1 & 0xff00) >> 8;
                    if (k1 == 0)
                        continue;
                    k++;
                    bool flag = (j1 & 0xff0000) != 0;
                    bool flag1 = (j1 & 0xff) != 0;
                    if (flag == flag1)
                    {
                        d++;
                        continue;
                    }
                    int l1 = flag1 ? 0xff0000 : 255;
                    bool flag2 = i1 > 0;
                    bool flag3 = i1 < i - 1;
                    bool flag4 = l > 0;
                    bool flag5 = l < j - 1;
                    if (flag2 && (ai[i1 - 1, l] & l1) != 0 || flag3 && (ai[i1 + 1, l] & l1) != 0 || flag4 && (ai[i1, l - 1] & l1) != 0 || flag5 && (ai[i1, l + 1] & l1) != 0)
                    {
                        d += 0.90000000000000002D;
                        continue;
                    }
                    if (flag2 && flag4 && (ai[i1 - 1, l - 1] & l1) != 0 || flag2 && flag5 && (ai[i1 - 1, l + 1] & l1) != 0 || flag3 && flag4 && (ai[i1 + 1, l - 1] & l1) != 0 || flag3 && flag5 && (ai[i1 + 1, l + 1] & l1) != 0)
                        d += 0.75D;
                    bool flag6 = i1 > 1;
                    bool flag7 = i1 < i - 2;
                    bool flag8 = l > 1;
                    bool flag9 = l < j - 2;
                    if (flag6 && (ai[i1 - 2, l] & l1) != 0 || flag7 && (ai[i1 + 2, l] & l1) != 0 || flag8 && (ai[i1, l - 2] & l1) != 0 || flag9 && (ai[i1, l + 2] & l1) != 0)
                    {
                        d += 0.20000000000000001D;
                        continue;
                    }
                    if (flag6 && flag4 && (ai[i1 - 2, l - 1] & l1) != 0 || flag6 && flag5 && (ai[i1 - 2, l + 1] & l1) != 0 || flag7 && flag4 && (ai[i1 + 2, l - 1] & l1) != 0 || flag7 && flag5 && (ai[i1 + 2, l + 1] & l1) != 0 || flag8 && flag2 && (ai[i1 - 1, l - 2] & l1) != 0 || flag8 && flag3 && (ai[i1 + 1, l - 2] & l1) != 0 || flag9 && flag2 && (ai[i1 - 1, l + 2] & l1) != 0 || flag9 && flag3 && (ai[i1 + 1, l + 2] & l1) != 0)
                        d += 0.125D;
                }

            }

            return d / (double)k;
        }

        public sealed override object run(object obj)
        {
            return run((Params)obj);
        }

        private static readonly Worker worker;
        private static readonly SymbolRecognition instance;

        static SymbolRecognition()
        {
            Thread.CurrentThread.Name = "Main ";
            worker = new Worker("SymbolRecognition", 20);
            instance = new SymbolRecognition(worker);
        }
    }
}
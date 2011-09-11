using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worker;

namespace Decapcha
{
    public class FindBestShift : Job
    {
        public class Params
        {

            internal readonly int[,] image;
            internal readonly int width;
            internal readonly int height;
            internal readonly int m;

            public Params(int[,] ai, int i, int j, int k)
            {
                image = ai;
                width = i;
                height = j;
                m = k;
            }
        }

        public class Result
        {

            internal readonly int[,] image;
            internal readonly int width;
            internal readonly int height;
            internal readonly long cost;

            public Result(int[,] ai, int i, int j, long l)
            {
                image = ai;
                width = i;
                height = j;
                cost = l;
            }
        }

        public static Future asyncExec(int[,] ai, int i, int j, int k)
        {
            return instance.asyncExec(new Params(ai, i, j, k));
        }

        protected Result run(Params param)
        {
            ImagePreparation imagePreparation = new ImagePreparation();
            int[,] ai = imagePreparation.shiftImage(param.image, param.width, param.height, param.m);
            int i = ai.GetLength(0);
            int j = ai.GetLength(1);
            long l;
            try
            {
                l = imagePreparation.insertRedLines(ai, i, j);
            }
            catch (Exception)
            {
                l = 0x7fffffffffffffffL;
            }
            return new Result(ai, i, j, l);
        }

        public sealed override object run(object obj)
        {
            return run((Params)obj);
        }

        private static readonly FindBestShift instance = new FindBestShift(ImagePreparation.worker);

        private FindBestShift(Worker worker1): base(worker1)
        {
        }


    }
}

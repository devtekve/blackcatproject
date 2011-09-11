using System;
using System.Collections;
using worker;

namespace ocr
{
    public sealed class SymbolsRecognition : Job
    {
        class Params
        {
            internal readonly ArrayList references;
            internal readonly int[][,] symbols;

            public Params(ArrayList list, int[][,] ai)
            {
                references = list;
                symbols = ai;
            }
        }

        public static Future asyncExec(ArrayList list, int[][,] ai)
        {
            return instance.asyncExec(new Params(list, ai));
        }


        public static OCRSymbolsResult syncExec(ArrayList list, int[][,] ai)
        {
            return (OCRSymbolsResult)instance.syncExec(new Params(list, ai));
        }

        private SymbolsRecognition(Worker worker1)
            : base(worker1)
        {
        }


        OCRSymbolsResult run(Params params1)
        {
            try
            {
                OCRSymbolsResult ocrsymbolsresult = new OCRSymbolsResult();
                OCRSymbolsResult ocrsymbolsresult1 = new OCRSymbolsResult();
                ArrayList arraylist = new ArrayList();
                int[][,] ai = params1.symbols;
                int i = ai.Length;
                for (int j = 0; j < i; j++)
                {
                    int[,] ai1 = ai[j];
                    arraylist.Add(SymbolRecognition.asyncExec(ai1, params1.references));
                }
                Future future1;

                for (IEnumerator iterator = arraylist.GetEnumerator(); iterator.MoveNext(); ocrsymbolsresult.append((OCRCharacterResult)future1.waitFor()))
                {
                    future1 = (Future)iterator.Current;
                }

                return ocrsymbolsresult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public sealed override object run(object obj)
        {
            return run((Params)obj);
        }



        public static Worker worker;

        private static readonly SymbolsRecognition instance;


        static SymbolsRecognition()
        {

            worker = new Worker("SymbolsRecognition", 10);
            instance = new SymbolsRecognition(worker);
        }
    }
}
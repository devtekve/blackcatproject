using System.Collections;


namespace ocr
{
    public class OCRSymbolsResult
    {

        readonly ArrayList result = new ArrayList();

        public void append(OCRCharacterResult ocrcharacterresult)
        {
            if (ocrcharacterresult == null)
                result.Add(OCRCharacterResult.INVALID);
            else
                result.Add(ocrcharacterresult);
        }

        public string getResultAsString()
        {
            System.Text.StringBuilder stringbuilder = new System.Text.StringBuilder();
            OCRCharacterResult ocrcharacterresult;
            for (IEnumerator iterator = result.GetEnumerator(); iterator.MoveNext(); stringbuilder.Append(ocrcharacterresult.character.descriptor.c))
                ocrcharacterresult = (OCRCharacterResult)iterator.Current;
            return stringbuilder.ToString();
        }

        public double getMatch()
        {
            if (isDestroyed())
                return 4.9406564584124654E-324D;
            double d = 1.0D;
            for (IEnumerator iterator = result.GetEnumerator(); iterator.MoveNext(); )
            {
                OCRCharacterResult ocrcharacterresult = (OCRCharacterResult)iterator.Current;
                d *= ocrcharacterresult.match;
            }

            return d;
        }

        public bool isDestroyed()
        {
            if (result.Count == 0)
                return true;
            for (IEnumerator iterator = result.GetEnumerator(); iterator.MoveNext(); )
            {
                OCRCharacterResult ocrcharacterresult = (OCRCharacterResult)iterator.Current;
                if (ocrcharacterresult.destroyed)
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return result.ToString();
        }


    }
}
using System.Collections;
using worker;

namespace ocr
{
    public class OpticalCharacterRecognition
    {

        public string recognizeCharacters(int[][,] ai)
        {
            ArrayList arraylist = new ArrayList();
            int i = ai[0].GetLength(1);
            for (int j = 0; j < 6; )
            {
                ArrayList arraylist1 = new ArrayList();
                string s = "Arial";
                int[] frontStyles = { 0, 1 };
                for (int j1 = 0; j1 < frontStyles.Length; j1++)
                {
                    int k1 = frontStyles[j1];
                    char[] characters = { 'A', 'a', 'B', 'b', 'C', 'D', 'd', 'E', 'e', 'F', 'G', 'H', 'h', 'i', 'L', 'M', 'm', 'N', 'n', 'Q', 'R', 'T', '2', '3', '4', '5', '6', '7', '8' };
                    for (int i2 = 0; i2 < characters.Length; i2++)
                    {
                        char c = characters[i2];
                        arraylist1.Add(new ReferenceCharacter(new ReferenceCharacterDescriptor(c, s, k1), store, i));
                    }

                }
                Future sym = SymbolsRecognition.asyncExec(arraylist1, ai);
                arraylist.Add(sym);
                j++;
                i--;
            }

            OCRSymbolsResult ocrsymbolsresult = new OCRSymbolsResult();
            IEnumerator iterator = arraylist.GetEnumerator();
            do
            {
                if (!iterator.MoveNext())
                    break;
                Future future = (Future)iterator.Current;
                OCRSymbolsResult ocrsymbolsresult1 = (OCRSymbolsResult)future.waitFor();
                if (!ocrsymbolsresult1.isDestroyed() && (ocrsymbolsresult1.getMatch() > ocrsymbolsresult.getMatch() || ocrsymbolsresult.isDestroyed()))
                    ocrsymbolsresult = ocrsymbolsresult1;
            } while (true);
            return ocrsymbolsresult.getResultAsString();
        }

        private static readonly ReferenceCharacterStore store = ReferenceCharacterStore.getInstance();
    }
}
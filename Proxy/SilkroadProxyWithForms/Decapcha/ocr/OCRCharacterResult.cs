namespace ocr
{

    public class OCRCharacterResult
    {

        public OCRCharacterResult(ReferenceCharacter referencecharacter, double d)
        {
            character = referencecharacter;
            match = d;
            destroyed = referencecharacter == ReferenceCharacter.INVALID;
        }

        public override string ToString()
        {

            return (new System.Text.StringBuilder()).Append(character.ToString()).Append("(").Append(destroyed ? "D" : match.ToString()).Append(")").ToString();
        }

        public static readonly OCRCharacterResult INVALID;
        public readonly ReferenceCharacter character;
        public readonly double match;
        public bool destroyed;

        static OCRCharacterResult()
        {
            INVALID = new OCRCharacterResult(ReferenceCharacter.INVALID, 4.9406564584124654E-324D);
        }
    }
}
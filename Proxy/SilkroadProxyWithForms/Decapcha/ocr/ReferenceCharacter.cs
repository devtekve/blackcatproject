namespace ocr
{
    public class ReferenceCharacter
    {
        public ReferenceCharacter(ReferenceCharacterDescriptor referencecharacterdescriptor, ReferenceCharacterStore referencecharacterstore, int i)
        {
            descriptor = referencecharacterdescriptor;
            size = i;
            image = referencecharacterstore.get1(referencecharacterdescriptor, i);
        }

        private ReferenceCharacter(ReferenceCharacterDescriptor referencecharacterdescriptor)
        {
            descriptor = referencecharacterdescriptor;
            size = 0;
            image = new int[1, 1];

        }

        public override string ToString()
        {
            return (new System.Text.StringBuilder()).Append(size).Append("_").Append(descriptor).ToString();
        }

        public static readonly ReferenceCharacter INVALID;
        public readonly ReferenceCharacterDescriptor descriptor;
        public readonly int size;
        public readonly int[,] image;

        static ReferenceCharacter()
        {
            INVALID = new ReferenceCharacter(ReferenceCharacterDescriptor.INVALID);
        }
    }
}
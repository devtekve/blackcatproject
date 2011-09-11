namespace ocr
{
    public sealed class ReferenceCharacterDescriptor
    {

        public ReferenceCharacterDescriptor(char c1, string s, int i)
        {
            c = c1;
            fontName = s;
            fontStyle = i;
        }

        public override string ToString()
        {
            return (new System.Text.StringBuilder()).Append(c).Append(char.IsUpper(c) ? "U" : "L").Append("_").Append(fontName.Replace(' ', '_')).Append("_").Append(fontStyle).ToString();

        }

        public static readonly ReferenceCharacterDescriptor INVALID = new ReferenceCharacterDescriptor('?', null, 0);
        public readonly char c;
        public readonly string fontName;
        public readonly int fontStyle;

    }
}
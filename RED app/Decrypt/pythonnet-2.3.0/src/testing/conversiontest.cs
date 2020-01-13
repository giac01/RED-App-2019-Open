namespace Python.Test
{
    /// <summary>
    /// Supports units tests for field access.
    /// </summary>
    public class ConversionTest
    {
        public ConversionTest()
        {
            EnumField = ShortEnum.Zero;
            SpamField = new Spam("spam");
            StringField = "spam";
        }

        public bool BooleanField = false;
        public byte ByteField = 0;
        public sbyte SByteField = 0;
        public char CharField = 'A';
        public short Int16Field = 0;
        public int Int32Field = 0;
        public long Int64Field = 0;
        public ushort UInt16Field = 0;
        public uint UInt32Field = 0;
        public ulong UInt64Field = 0;
        public float SingleField = 0.0F;
        public double DoubleField = 0.0;
        public decimal DecimalField = 0;
        public string StringField;
        public ShortEnum EnumField;
        public object ObjectField = null;
        public ISpam SpamField;

        public byte[] ByteArrayField;
        public sbyte[] SByteArrayField;
    }


    public interface ISpam
    {
        string GetValue();
    }

    public class Spam : ISpam
    {
        private string value;

        public Spam(string value)
        {
            this.value = value;
        }

        public string GetValue()
        {
            return value;
        }
    }
}

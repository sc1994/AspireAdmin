namespace Aspire
{
    internal class GlobalResponse
    {
        public int Code { get; set; }

        public object Result { get; set; }

#if DEBUG
        public string StackTrace { get; set; }
#endif
    }
}

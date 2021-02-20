using System;

namespace Aspire
{
    public static class GuidUtility
    {
        public static Guid NewOrderlyGuid()
        {
            return Guid.NewGuid(); // TODO 有序的GUID
        }
    }
}

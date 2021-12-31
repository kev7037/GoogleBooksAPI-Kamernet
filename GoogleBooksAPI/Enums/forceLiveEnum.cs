using System.ComponentModel;

namespace GoogleBooksAPI.Enums
{
    public enum searchType
    {
        [Description("NotForced")]
        NotForced = 0,
        [Description("LiveForced")]
        LiveForced = 1,
        [Description("FiveMinForced")]
        FiveMinForced = 2
    }
}
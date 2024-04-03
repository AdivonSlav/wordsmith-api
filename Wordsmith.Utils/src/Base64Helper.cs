using System.Text;

namespace Wordsmith.Utils;

public static class Base64Helper
{
    public static string EncodeToBase64(string value)
    {
        var data = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(data);
    }
    
    public static string DecodeFromBase64(string value)
    {
        var data = Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(data);
    }
}
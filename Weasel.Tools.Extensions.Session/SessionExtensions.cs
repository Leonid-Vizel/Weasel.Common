using Microsoft.AspNetCore.Http;

namespace Weasel.Tools.Extensions.Session;

public static class SessionExtensions
{
    #region Boolean Operations
    public static void SetBoolean(this ISession session, string key, bool value)
    {
        session.Set(key, BitConverter.GetBytes(value));
    }
    public static bool? GetBoolean(this ISession session, string key)
    {
        var data = session.Get(key);
        if (data == null)
        {
            return null;
        }
        return BitConverter.ToBoolean(data, 0);
    }
    #endregion

    #region DateTime Operations
    public static void SetDateTime(this ISession session, string key, DateTime value)
    {
        session.Set(key, BitConverter.GetBytes(value.ToBinary()));
    }
    public static DateTime? GetDateTime(this ISession session, string key)
    {
        var data = session.Get(key);
        if (data == null)
        {
            return null;
        }
        long binaryValue = BitConverter.ToInt64(data, 0);
        return DateTime.FromBinary(binaryValue);
    }
    #endregion

    #region Int64 Operations
    public static void SetInt64(this ISession session, string key, long value)
    {
        var bytes = new byte[]
        {
                (byte)(value >> 56),
                (byte)(0xFF & (value >> 48)),
                (byte)(0xFF & (value >> 40)),
                (byte)(0xFF & (value >> 32)),
                (byte)(0xFF & (value >> 24)),
                (byte)(0xFF & (value >> 16)),
                (byte)(0xFF & (value >> 8)),
                (byte)(0xFF & value)
        };
        session.Set(key, bytes);
    }

    public static long? GetInt64(this ISession session, string key)
    {
        var data = session.Get(key);
        if (data == null || data.Length < 8)
        {
            return null;
        }

        return (long)data[0] << 56 | (long)data[1] << 48 | (long)data[2] << 40 | (long)data[3] << 32 | (long)data[4] << 24 | (long)data[5] << 16 | (long)data[6] << 8 | (long)data[7];
    }
    #endregion
}

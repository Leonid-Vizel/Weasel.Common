namespace Weasel.Tools.Extensions.Common;

public static class Power2Extensions
{
    public static bool IsPowerOfTwo(this ulong x)
        => (x != 0) && ((x & (x - 1)) == 0);
    public static bool IsPowerOfTwo(this long x)
        => (x != 0) && ((x & (x - 1)) == 0);
    public static bool IsPowerOfTwo(this uint x)
        => (x != 0) && ((x & (x - 1)) == 0);
    public static bool IsPowerOfTwo(this int x)
        => (x != 0) && ((x & (x - 1)) == 0);
    public static bool IsPowerOfTwo(this short x)
        => (x != 0) && ((x & (x - 1)) == 0);
    public static bool IsPowerOfTwo(this ushort x)
        => (x != 0) && ((x & (x - 1)) == 0);
    public static bool IsPowerOfTwo(this sbyte x)
        => (x != 0) && ((x & (x - 1)) == 0);
    public static bool IsPowerOfTwo(this byte x)
        => (x != 0) && ((x & (x - 1)) == 0);
    public static bool IsPowerOfTwo(this nint x)
        => (x != 0) && ((x & (x - 1)) == 0);
    public static bool IsPowerOfTwo(this nuint x)
        => (x != 0) && ((x & (x - 1)) == 0);
}

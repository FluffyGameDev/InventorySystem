
class Utils
{
    public static void Swap<T>(ref T left, ref T right)
    {
        T temp;
        temp = left;
        left = right;
        right = temp;
    }
}

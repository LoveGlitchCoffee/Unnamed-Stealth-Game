using System.Collections.Generic;

public static class ListExtensions {

    public static void Reverse_NoHeapAlloc<T>(this List<T> list)
    {
        int count = list.Count;

        for (int i = 0; i < count / 2; i++)
        {
            T tmp = list[i];
            list[i] = list[count - i - 1];
            list[count - i - 1] = tmp;
        }
    }
}

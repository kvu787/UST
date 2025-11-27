using System;
using System.Collections.Generic;

namespace UST;

public static class ListExtensions {
    public static T PickRandom<T>(this List<T> list) {
        ArgumentNullException.ThrowIfNull(list);
        if (list.Count == 0) {
            throw new InvalidOperationException("Cannot pick a random item from an empty list.");
        }
        return list[Random.Shared.Next(list.Count)];
    }
}

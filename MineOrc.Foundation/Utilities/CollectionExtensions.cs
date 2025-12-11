// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Utilities;

public static class CollectionExtensions
{
    public static void AddIf<T>(this ICollection<T> collection,
        bool condition,
        T value)
    {
        if (condition)
        {
            collection.Add(value);
        }
    }
    
    public static void AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        TKey key,
        TValue? value)
        where TKey : notnull
    {
        if (value == null)
        {
            return;
        }
        
        dictionary.Add(key, value);
    }
    
    public static void AddIfNotNull<T>(this ICollection<T> collection,
        T? value)
    {
        if (value == null)
        {
            return;
        }
        
        collection.Add(value);
    }
}
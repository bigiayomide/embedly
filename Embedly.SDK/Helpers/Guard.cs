using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Embedly.SDK.Helpers;

/// <summary>
///     Helper class for argument validation to maintain compatibility across .NET versions.
/// </summary>
internal static class Guard
{
    /// <summary>
    ///     Throws ArgumentNullException if the argument is null.
    /// </summary>
    public static T ThrowIfNull<T>([NotNull] T? argument, string? paramName = null) where T : class
    {
        return argument ?? throw new ArgumentNullException(paramName);
    }

    /// <summary>
    ///     Throws ArgumentException if the argument is null, empty, or whitespace.
    /// </summary>
    public static string ThrowIfNullOrWhiteSpace([NotNull] string? argument, string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(argument))
            throw new ArgumentException("Value cannot be null, empty, or whitespace.", paramName);
        return argument;
    }

    /// <summary>
    ///     Creates a readonly wrapper for a dictionary.
    /// </summary>
    public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this Dictionary<TKey, TValue>? dictionary)
        where TKey : notnull
    {
        return dictionary is null ? new Dictionary<TKey, TValue>() : new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }
}

/// <summary>
///     Simple readonly dictionary wrapper for compatibility.
/// </summary>
internal sealed class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary;

    public ReadOnlyDictionary(Dictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
    }

    public TValue this[TKey key] => _dictionary[key];
    public IEnumerable<TKey> Keys => _dictionary.Keys;
    public IEnumerable<TValue> Values => _dictionary.Values;
    public int Count => _dictionary.Count;

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value!);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
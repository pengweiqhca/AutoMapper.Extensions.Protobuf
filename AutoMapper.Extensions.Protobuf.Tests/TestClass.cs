namespace AutoMapper.Extensions.Protobuf.Tests;

public class DurationClass<T>
{
    public T? Duration { get; set; }
}

public class TimestampClass<T>
{
    public T? Timestamp { get; set; }
}

public class ByteStringClass<T>
{
    public T? Bytes { get; set; }
}

public class RepeatedFieldClass<T>
{
    public T[]? List { get; set; }
}

public class MapFieldClass<TKey, TValue> where TKey : notnull
{
    public Dictionary<TKey, TValue>? Map { get; set; }
}

public class ListMapClass<TKey, TValue> where TKey : notnull
{
    public Dictionary<TKey, TValue>? ListMap { get; set; }
}

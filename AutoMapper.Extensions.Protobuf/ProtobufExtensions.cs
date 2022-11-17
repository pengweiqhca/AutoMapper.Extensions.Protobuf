using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace AutoMapper.Extensions.Protobuf;

public static class ProtobufExtensions
{
    /// <summary>
    /// Creates a lot of mapping configuration from protobuf and .NET.<br />
    /// DateTime ⮀ Timestamp, DateTime? ⮀ Timestamp, DateTimeOffset ⮀ Timestamp, DateTimeOffset? ⮀ Timestamp.<br />
    /// TimeSpan ⮀ Duration, TimeSpan? ⮀ Duration.<br />
    /// byte[] ⮀ ByteString, ReadOnlyMemory&lt;byte&gt; ⮀ ByteString, ReadOnlyMemory&lt;byte&gt;? ⮀ ByteString.
    /// </summary>
    public static IMapperConfigurationExpression AddProtobuf(this IMapperConfigurationExpression configuration)
    {
        #region Timestamp

        configuration.CreateMap<DateTime, Timestamp?>().ConvertUsing(static (source, _) =>
            Timestamp.FromDateTime(source.ToUniversalTime()));

        configuration.CreateMap<DateTimeOffset, Timestamp?>().ConvertUsing(static (source, _) =>
            Timestamp.FromDateTimeOffset(source));

        configuration.CreateMap<Timestamp?, DateTime>().ConvertUsing(static (source, _) =>
            source == null ? throw new ArgumentNullException(nameof(source)) : source.ToDateTime().ToLocalTime());

        configuration.CreateMap<Timestamp?, DateTimeOffset>().ConvertUsing(static (source, _) =>
            source == null ? throw new ArgumentNullException(nameof(source)) : source.ToDateTimeOffset().ToLocalTime());

        #endregion

        #region Duration

        configuration.CreateMap<TimeSpan, Duration?>().ConvertUsing(static (source, _) => source.ToDuration());

        configuration.CreateMap<Duration?, TimeSpan>().ConvertUsing(static (source, _) =>
            source?.ToTimeSpan() ?? throw new ArgumentNullException(nameof(source)));

        #endregion

        #region Bytes

        configuration.CreateMap<byte[], ByteString?>().ConvertUsing(static (source, _) =>
            source == null ? null : ByteString.CopyFrom(source));

        configuration.CreateMap<ByteString?, byte[]?>().ConvertUsing(static (source, _) => source?.ToByteArray());

        configuration.CreateMap<ReadOnlyMemory<byte>, ByteString>().ConvertUsing(static (source, _) =>
            UnsafeByteOperations.UnsafeWrap(source));

        configuration.CreateMap<ReadOnlyMemory<byte>?, ByteString?>().ConvertUsing(static (source, _) =>
            source == null ? null : UnsafeByteOperations.UnsafeWrap(source.Value));

        configuration.CreateMap<ByteString?, ReadOnlyMemory<byte>>().ConvertUsing(static (source, _) =>
            source == null ? throw new ArgumentNullException(nameof(source)) : source.Memory);

        #endregion

        return configuration;
    }

    /// <summary>Creates a <see href="https://developers.google.com/protocol-buffers/docs/proto3#backwards_compatibility">protocol buffers backwards compatible</see> mapping configuration from the KeyValuePair&lt;<typeparamref name="TSourceKey"/>, <typeparamref name="TSourceValue"/>&gt; type to the <typeparamref name="TDestinationMapFieldEntry"/>, and reverse map.</summary>
    public static IMapperConfigurationExpression AddMapFieldEntry<TSourceKey, TSourceValue, TDestinationMapFieldEntry>(
        this IMapperConfigurationExpression configuration)
        where TDestinationMapFieldEntry : IMessage
    {
        configuration.CreateMap<KeyValuePair<TSourceKey, TSourceValue>, TDestinationMapFieldEntry>().ReverseMap();

        return configuration;
    }
}

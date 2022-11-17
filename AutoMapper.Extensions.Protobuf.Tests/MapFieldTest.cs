using Google.Protobuf.WellKnownTypes;
using Xunit;

namespace AutoMapper.Extensions.Protobuf.Tests;

public class MapFieldTest
{
    private readonly IMapper _mapper = new MapperConfiguration(static cfg =>
    {
        cfg.AddProtobuf();

        cfg.CreateMap<MapFieldClass<int, TimestampClass<DateTime?>>, TestValue>().ReverseMap();
        cfg.CreateMap<TimestampClass<DateTime?>, NullableValue>().ReverseMap();
    }).CreateMapper();

    [Fact]
    public void Dictionary2MapField()
    {
        var source = new MapFieldClass<int, TimestampClass<DateTime?>>();

        var destination = _mapper.Map<MapFieldClass<int, TimestampClass<DateTime?>>, TestValue>(source);

        Assert.Empty(destination.Map);

        source.Map = new() { { 3, new() { Timestamp = DateTime.UtcNow } } };

        destination = _mapper.Map<MapFieldClass<int, TimestampClass<DateTime?>>, TestValue>(source);

        var kv = Assert.Single(destination.Map);

        Assert.Equal(3, kv.Key);

        Assert.Equal(source.Map[3].Timestamp, kv.Value.Timestamp.ToDateTime());
    }

    [Fact]
    public void MapField2Dictionary()
    {
        var source = new TestValue();

        var destination = _mapper.Map<TestValue, MapFieldClass<int, TimestampClass<DateTime?>>>(source);

        Assert.NotNull(destination.Map);
        Assert.Empty(destination.Map);

        source.Map.Add(3, new() { Timestamp = DateTime.UtcNow.ToTimestamp() });

        destination = _mapper.Map<TestValue, MapFieldClass<int, TimestampClass<DateTime?>>>(source);

        Assert.NotNull(destination.Map);

        var kv = Assert.Single(destination.Map);

        Assert.Equal(3, kv.Key);

        Assert.Equal(source.Map[3].Timestamp.ToDateTime().ToLocalTime(), kv.Value.Timestamp);
    }
}
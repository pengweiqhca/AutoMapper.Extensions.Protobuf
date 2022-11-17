using Google.Protobuf.WellKnownTypes;
using Xunit;

namespace AutoMapper.Extensions.Protobuf.Tests;

public class ListMapTest
{
    private readonly IMapper _mapper = new MapperConfiguration(static cfg =>
    {
        cfg.AddProtobuf();

        cfg.AddMapFieldEntry<Corpus, TimestampClass<DateTime?>, TestValue.Types.MapFieldEntry>();
        cfg.CreateMap<ListMapClass<Corpus, TimestampClass<DateTime?>>, TestValue>().ReverseMap();
        cfg.CreateMap<TimestampClass<DateTime?>, NullableValue>().ReverseMap();
    }).CreateMapper();

    [Fact]
    public void Dictionary2MapField()
    {
        var source = new ListMapClass<Corpus, TimestampClass<DateTime?>>();

        var destination = _mapper.Map<ListMapClass<Corpus, TimestampClass<DateTime?>>, TestValue>(source);

        Assert.Empty(destination.Map);

        source.ListMap = new() { { Corpus.Images, new() { Timestamp = DateTime.UtcNow } } };

        destination = _mapper.Map<ListMapClass<Corpus, TimestampClass<DateTime?>>, TestValue>(source);

        var kv = Assert.Single(destination.ListMap);

        Assert.Equal(Corpus.Images, kv.Key);

        Assert.Equal(source.ListMap[Corpus.Images].Timestamp, kv.Value.Timestamp.ToDateTime());
    }

    [Fact]
    public void MapField2Dictionary()
    {
        var source = new TestValue();

        var destination = _mapper.Map<TestValue, ListMapClass<Corpus, TimestampClass<DateTime?>>>(source);

        Assert.NotNull(destination.ListMap);
        Assert.Empty(destination.ListMap);

        source.ListMap.Add(new TestValue.Types.MapFieldEntry
        {
            Key = Corpus.Images,
            Value = new() { Timestamp = DateTime.UtcNow.ToTimestamp() }
        });

        destination = _mapper.Map<TestValue, ListMapClass<Corpus, TimestampClass<DateTime?>>>(source);

        Assert.NotNull(destination.ListMap);

        var kv = Assert.Single(destination.ListMap);

        Assert.Equal(Corpus.Images, kv.Key);

        Assert.Equal(source.ListMap[0].Value.Timestamp.ToDateTime().ToLocalTime(), kv.Value.Timestamp);
    }
}

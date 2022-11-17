using Xunit;

namespace AutoMapper.Extensions.Protobuf.Tests;

public class RepeatedFieldTest
{
    private readonly IMapper _mapper = new MapperConfiguration(static cfg =>
    {
        cfg.AddProtobuf();

        cfg.CreateMap<RepeatedFieldClass<int>, TestValue>().ReverseMap();
    }).CreateMapper();

    [Fact]
    public void List2RepeatedField()
    {
        var source = new RepeatedFieldClass<int>();
        var destination = new TestValue { List = { 0 } };

        _mapper.Map(source, destination);

        Assert.Empty(destination.List);

        source.List = new[] { 1, 2, 3, 4 };
        destination.List.Add(0);

        _mapper.Map(source, destination);

        Assert.Equal(source.List, destination.List);
    }

    [Fact]
    public void RepeatedField2List()
    {
        var source = new TestValue();

        var destination = _mapper.Map<TestValue, RepeatedFieldClass<int>>(source);

        Assert.NotNull(destination.List);
        Assert.Empty(destination.List);

        source.List.AddRange(Enumerable.Range(1, 4));
        destination.List = new[] { 0 };

        _mapper.Map(source, destination);

        Assert.NotNull(destination.List);
        Assert.Equal(source.List, destination.List);
    }
}

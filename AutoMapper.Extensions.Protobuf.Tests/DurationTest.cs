using Google.Protobuf.WellKnownTypes;
using Xunit;

namespace AutoMapper.Extensions.Protobuf.Tests;

public class DurationTest
{
    private readonly IMapper _mapper = new MapperConfiguration(static cfg =>
    {
        cfg.AddProtobuf();

        cfg.CreateMap<DurationClass<TimeSpan>, NullableValue>().ReverseMap();
        cfg.CreateMap<DurationClass<TimeSpan?>, NullableValue>().ReverseMap();
    }).CreateMapper();

    [Fact]
    public void TimeSpan2Duration()
    {
        var source = new DurationClass<TimeSpan> { Duration = TimeSpan.FromSeconds(10) };

        var destination = _mapper.Map<DurationClass<TimeSpan>, NullableValue>(source);

        Assert.Equal(source.Duration.ToDuration(), destination.Duration);
    }

    [Fact]
    public void TimeSpanNullable2Duration()
    {
        var source = new DurationClass<TimeSpan?>();

        var destination = _mapper.Map<DurationClass<TimeSpan?>, NullableValue>(source);

        Assert.Null(destination.Duration);

        source.Duration = TimeSpan.FromSeconds(10);

        destination = _mapper.Map<DurationClass<TimeSpan?>, NullableValue>(source);

        Assert.Equal(source.Duration.Value.ToDuration(), destination.Duration);
    }

    [Fact]
    public void Duration2TimeSpan()
    {
        var source = new NullableValue();

        var ex = Assert.IsType<ArgumentNullException>(Assert.Throws<AutoMapperMappingException>(() =>
            _mapper.Map<NullableValue, DurationClass<TimeSpan>>(source)).InnerException);

        Assert.Equal(nameof(source), ex.ParamName);

        var timeSpan = TimeSpan.FromSeconds(10);

        source.Duration = timeSpan.ToDuration();

        var destination = _mapper.Map<NullableValue, DurationClass<TimeSpan>>(source);

        Assert.Equal(timeSpan, destination.Duration);
    }

    [Fact]
    public void Duration2TimeSpanNullable()
    {
        var source = new NullableValue();

        var destination = _mapper.Map<NullableValue, DurationClass<TimeSpan?>>(source);

        Assert.Null(destination.Duration);

        var timeSpan = TimeSpan.FromSeconds(10);

        source.Duration = timeSpan.ToDuration();

        destination = _mapper.Map<NullableValue, DurationClass<TimeSpan?>>(source);

        Assert.Equal(timeSpan, destination.Duration);
    }
}

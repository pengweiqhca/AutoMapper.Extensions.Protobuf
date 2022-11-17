using Google.Protobuf.WellKnownTypes;
using Xunit;

namespace AutoMapper.Extensions.Protobuf.Tests;

public class TimestampTest
{
    private readonly IMapper _mapper = new MapperConfiguration(static cfg =>
    {
        cfg.AddProtobuf();

        cfg.CreateMap<TimestampClass<DateTime>, NullableValue>().ReverseMap();
        cfg.CreateMap<TimestampClass<DateTime?>, NullableValue>().ReverseMap();

        cfg.CreateMap<TimestampClass<DateTimeOffset>, NullableValue>().ReverseMap();
        cfg.CreateMap<TimestampClass<DateTimeOffset?>, NullableValue>().ReverseMap();
    }).CreateMapper();

    #region DateTime

    [Fact]
    public void DateTime2Duration()
    {
        var source = new TimestampClass<DateTime> { Timestamp = DateTime.Now };

        var destination = _mapper.Map<TimestampClass<DateTime>, NullableValue>(source);

        Assert.Equal(source.Timestamp, destination.Timestamp.ToDateTime().ToLocalTime());
    }

    [Fact]
    public void DateTimeNullable2Duration()
    {
        var source = new TimestampClass<DateTime?>();

        var destination = _mapper.Map<TimestampClass<DateTime?>, NullableValue>(source);

        Assert.Null(destination.Duration);

        source.Timestamp = DateTime.Now;

        destination = _mapper.Map<TimestampClass<DateTime?>, NullableValue>(source);

        Assert.Equal(source.Timestamp.Value, destination.Timestamp.ToDateTime().ToLocalTime());
    }

    [Fact]
    public void Duration2DateTime()
    {
        var source = new NullableValue();

        var ex = Assert.IsType<ArgumentNullException>(Assert.Throws<AutoMapperMappingException>(() =>
            _mapper.Map<NullableValue, TimestampClass<DateTime>>(source)).InnerException);

        Assert.Equal(nameof(source), ex.ParamName);

        var now = DateTime.UtcNow;

        source.Timestamp = now.ToTimestamp();

        var destination = _mapper.Map<NullableValue, TimestampClass<DateTime>>(source);

        Assert.Equal(now.ToLocalTime(), destination.Timestamp);
    }

    [Fact]
    public void Duration2DateTimeNullable()
    {
        var source = new NullableValue();

        var destination = _mapper.Map<NullableValue, TimestampClass<DateTime?>>(source);

        Assert.Null(destination.Timestamp);

        var now = DateTime.UtcNow;

        source.Timestamp = now.ToTimestamp();

        destination = _mapper.Map<NullableValue, TimestampClass<DateTime?>>(source);

        Assert.Equal(now.ToLocalTime(), destination.Timestamp);
    }

    #endregion

    #region DateTimeOffset

    [Fact]
    public void DateTimeOffset2Duration()
    {
        var source = new TimestampClass<DateTimeOffset> { Timestamp = DateTimeOffset.Now };

        var destination = _mapper.Map<TimestampClass<DateTimeOffset>, NullableValue>(source);

        Assert.Equal(source.Timestamp, destination.Timestamp.ToDateTimeOffset().ToLocalTime());
    }

    [Fact]
    public void DateTimeOffsetNullable2Duration()
    {
        var source = new TimestampClass<DateTimeOffset?>();

        var destination = _mapper.Map<TimestampClass<DateTimeOffset?>, NullableValue>(source);

        Assert.Null(destination.Duration);

        source.Timestamp = DateTimeOffset.Now;

        destination = _mapper.Map<TimestampClass<DateTimeOffset?>, NullableValue>(source);

        Assert.Equal(source.Timestamp.Value, destination.Timestamp.ToDateTimeOffset().ToLocalTime());
    }

    [Fact]
    public void Duration2DateTimeOffset()
    {
        var source = new NullableValue();

        var ex = Assert.IsType<ArgumentNullException>(Assert.Throws<AutoMapperMappingException>(() =>
            _mapper.Map<NullableValue, TimestampClass<DateTimeOffset>>(source)).InnerException);

        Assert.Equal(nameof(source), ex.ParamName);

        var now = DateTimeOffset.UtcNow;

        source.Timestamp = now.ToTimestamp();

        var destination = _mapper.Map<NullableValue, TimestampClass<DateTimeOffset>>(source);

        Assert.Equal(now.ToLocalTime(), destination.Timestamp);
    }

    [Fact]
    public void Duration2DateTimeOffsetNullable()
    {
        var source = new NullableValue();

        var destination = _mapper.Map<NullableValue, TimestampClass<DateTimeOffset?>>(source);

        Assert.Null(destination.Timestamp);

        var now = DateTimeOffset.UtcNow;

        source.Timestamp = now.ToTimestamp();

        destination = _mapper.Map<NullableValue, TimestampClass<DateTimeOffset?>>(source);

        Assert.Equal(now.ToLocalTime(), destination.Timestamp);
    }

    #endregion
}
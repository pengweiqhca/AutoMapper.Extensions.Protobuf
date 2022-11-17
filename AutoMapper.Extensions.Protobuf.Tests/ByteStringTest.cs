using Google.Protobuf;
using Xunit;

namespace AutoMapper.Extensions.Protobuf.Tests;

public class ByteStringTest
{
    private readonly IMapper _mapper = new MapperConfiguration(static cfg =>
    {
        cfg.AddProtobuf();

        cfg.CreateMap<ByteStringClass<byte[]>, TestValue>().ReverseMap();
        cfg.CreateMap<ByteStringClass<byte[]>, NullableValue>().ReverseMap();

        cfg.CreateMap<ByteStringClass<ReadOnlyMemory<byte>>, TestValue>().ReverseMap();
        cfg.CreateMap<ByteStringClass<ReadOnlyMemory<byte>>, NullableValue>().ReverseMap();

        cfg.CreateMap<ByteStringClass<ReadOnlyMemory<byte>?>, TestValue>().ReverseMap();
        cfg.CreateMap<ByteStringClass<ReadOnlyMemory<byte>?>, NullableValue>().ReverseMap();
    }).CreateMapper();

    #region byte[]

    [Fact]
    public void Bytes2ByteString()
    {
        var source = new ByteStringClass<byte[]>();

        var ex = Assert.IsType<ArgumentNullException>(Assert
            .Throws<AutoMapperMappingException>(() => _mapper.Map<ByteStringClass<byte[]>, TestValue>(source))
            .InnerException);

        Assert.Equal("value", ex.ParamName);

        source.Bytes = Guid.NewGuid().ToByteArray();

        var destination = _mapper.Map<ByteStringClass<byte[]>, TestValue>(source);

        Assert.Equal(source.Bytes, destination.Bytes);
    }

    [Fact]
    public void Bytes2BytesValue()
    {
        var source = new ByteStringClass<byte[]>();

        Assert.Null(_mapper.Map<ByteStringClass<byte[]>, NullableValue>(source).Bytes);

        source.Bytes = Guid.NewGuid().ToByteArray();

        var destination = _mapper.Map<ByteStringClass<byte[]>, NullableValue>(source);

        Assert.Equal(source.Bytes, destination.Bytes);
    }

    [Fact]
    public void ByteString2Bytes()
    {
        var source = new TestValue();

        var destination = _mapper.Map<TestValue, ByteStringClass<byte[]>>(source);

        Assert.NotNull(destination.Bytes);
        Assert.Empty(destination.Bytes);

        source.Bytes = ByteString.CopyFrom(Guid.NewGuid().ToByteArray());

        destination = _mapper.Map<TestValue, ByteStringClass<byte[]>>(source);

        Assert.NotNull(destination.Bytes);
        Assert.Equal(source.Bytes, destination.Bytes);
    }

    [Fact]
    public void BytesValue2Bytes()
    {
        var source = new NullableValue();

        var destination = _mapper.Map<NullableValue, ByteStringClass<byte[]>>(source);

        Assert.Null(destination.Bytes);

        source.Bytes = ByteString.CopyFrom(Guid.NewGuid().ToByteArray());

        destination = _mapper.Map<NullableValue, ByteStringClass<byte[]>>(source);

        Assert.NotNull(destination.Bytes);
        Assert.Equal(source.Bytes, destination.Bytes);
    }

    #endregion

    #region ReadOnlyMemory<byte>

    [Fact]
    public void ReadOnlyMemory2ByteString()
    {
        var source = new ByteStringClass<ReadOnlyMemory<byte>>();

        var destination = _mapper.Map<ByteStringClass<ReadOnlyMemory<byte>>, TestValue>(source);

        Assert.Empty(destination.Bytes);

        source.Bytes = Guid.NewGuid().ToByteArray();

        destination = _mapper.Map<ByteStringClass<ReadOnlyMemory<byte>>, TestValue>(source);

        Assert.Equal(source.Bytes, destination.Bytes.Memory);
    }

    [Fact]
    public void ReadOnlyMemory2BytesValue()
    {
        var source = new ByteStringClass<ReadOnlyMemory<byte>>();

        Assert.Empty(_mapper.Map<ByteStringClass<ReadOnlyMemory<byte>>, NullableValue>(source).Bytes);

        source.Bytes = Guid.NewGuid().ToByteArray();

        var destination = _mapper.Map<ByteStringClass<ReadOnlyMemory<byte>>, NullableValue>(source);

        Assert.Equal(source.Bytes, destination.Bytes.Memory);
    }

    [Fact]
    public void ByteString2ReadOnlyMemory()
    {
        var source = new TestValue();

        var destination = _mapper.Map<TestValue, ByteStringClass<ReadOnlyMemory<byte>>>(source);

        Assert.Empty(destination.Bytes.ToArray());

        source.Bytes = ByteString.CopyFrom(Guid.NewGuid().ToByteArray());

        destination = _mapper.Map<TestValue, ByteStringClass<ReadOnlyMemory<byte>>>(source);

        Assert.Equal(source.Bytes.Memory, destination.Bytes);
    }

    [Fact]
    public void BytesValue2ReadOnlyMemory()
    {
        var source = new NullableValue();

        var ex = Assert.IsType<ArgumentNullException>(Assert.Throws<AutoMapperMappingException>(() =>
            _mapper.Map<NullableValue, ByteStringClass<ReadOnlyMemory<byte>>>(source)).InnerException);

        Assert.Equal(nameof(source), ex.ParamName);

        source.Bytes = ByteString.CopyFrom(Guid.NewGuid().ToByteArray());

        var destination = _mapper.Map<NullableValue, ByteStringClass<ReadOnlyMemory<byte>>>(source);

        Assert.Equal(source.Bytes.Memory, destination.Bytes);
    }

    #endregion

    #region ReadOnlyMemory<byte>?

    [Fact]
    public void ReadOnlyMemoryNullable2ByteString()
    {
        var source = new ByteStringClass<ReadOnlyMemory<byte>?>();

        var ex = Assert.IsType<ArgumentNullException>(Assert
            .Throws<AutoMapperMappingException>(() =>
                _mapper.Map<ByteStringClass<ReadOnlyMemory<byte>?>, TestValue>(source)).InnerException);

        Assert.Equal("value", ex.ParamName);

        source.Bytes = Guid.NewGuid().ToByteArray();

        var destination = _mapper.Map<ByteStringClass<ReadOnlyMemory<byte>?>, TestValue>(source);

        Assert.Equal(source.Bytes.Value, destination.Bytes.Memory);
    }

    [Fact]
    public void ReadOnlyMemoryNullable2BytesValue()
    {
        var source = new ByteStringClass<ReadOnlyMemory<byte>?>();

        Assert.Null(_mapper.Map<ByteStringClass<ReadOnlyMemory<byte>?>, NullableValue>(source).Bytes);

        source.Bytes = Guid.NewGuid().ToByteArray();

        var destination = _mapper.Map<ByteStringClass<ReadOnlyMemory<byte>?>, NullableValue>(source);

        Assert.Equal(source.Bytes.Value, destination.Bytes.Memory);
    }

    [Fact]
    public void ByteString2ReadOnlyMemoryNullable()
    {
        var source = new TestValue();

        var destination = _mapper.Map<TestValue, ByteStringClass<ReadOnlyMemory<byte>?>>(source);

        Assert.NotNull(destination.Bytes);
        Assert.Empty(destination.Bytes.Value.ToArray());

        source.Bytes = ByteString.CopyFrom(Guid.NewGuid().ToByteArray());

        destination = _mapper.Map<TestValue, ByteStringClass<ReadOnlyMemory<byte>?>>(source);

        Assert.NotNull(destination.Bytes);
        Assert.Equal(source.Bytes.Memory, destination.Bytes.Value);
    }

    [Fact]
    public void BytesValue2ReadOnlyMemoryNullable()
    {
        var source = new NullableValue();

        var destination = _mapper.Map<NullableValue, ByteStringClass<ReadOnlyMemory<byte>?>>(source);

        Assert.Null(destination.Bytes);

        source.Bytes = ByteString.CopyFrom(Guid.NewGuid().ToByteArray());

        destination = _mapper.Map<NullableValue, ByteStringClass<ReadOnlyMemory<byte>?>>(source);

        Assert.Equal(source.Bytes.Memory, destination.Bytes);
    }

    #endregion
}
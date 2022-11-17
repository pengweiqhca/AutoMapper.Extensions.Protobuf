# AutoMapper.Extensions.Protobuf

AutoMapper support protobuf.

``` C#
new MapperConfiguration(static cfg =>
{
    cfg.AddProtobuf();
    
    cfg.AddMapFieldEntry<TSourceKey, TSourceValue, TDestinationMapFieldEntry>();
})
```

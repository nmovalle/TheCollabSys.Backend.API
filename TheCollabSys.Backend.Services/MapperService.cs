using AutoMapper;

namespace TheCollabSys.Backend.Services;

public class MapperService<TSource, TDestination> : IMapperService<TSource, TDestination>
{
    private readonly IMapper _mapper;

    public MapperService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map(TSource source, TDestination destination)
    {
        return _mapper.Map(source, destination);
    }

    public TDestination MapToDestination(TSource source)
    {
        return _mapper.Map<TDestination>(source);
    }

    public TSource MapToSource(TDestination destination)
    {
        return _mapper.Map<TSource>(destination);
    }
}

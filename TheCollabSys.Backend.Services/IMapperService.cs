namespace TheCollabSys.Backend.Services;

public interface IMapperService<TSource, TDestination>
{
    TDestination MapToDestination(TSource source);
    TSource MapToSource(TDestination destination);
    TDestination Map(TSource source, TDestination destination, List<string>? excludeProperties = null);
}

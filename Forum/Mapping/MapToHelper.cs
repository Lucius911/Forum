namespace Forum.Mapping
{
  // Manually created a mapper as apposed to using AutoMapper to avoid unnecessary dependencies.
  public class MapToHelper
  {
    public static TEntity MapDto<TDto, TEntity>(TDto dto)
      where TDto : IMapTo<TEntity>
    {
      return dto.Map();
    }
  }
}

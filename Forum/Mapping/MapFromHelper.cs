namespace Forum.Mapping
{
  public class MapFromHelper
  {
    public static TDto MapEntityToDto<TEntity, TDto>(TEntity entity)
      where TDto : IMapFrom<TEntity>, new()
    {
      var dto = new TDto();
      dto.MapFrom(entity);
      return dto;
    }
  }
}

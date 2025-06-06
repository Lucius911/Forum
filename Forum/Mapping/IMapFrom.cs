namespace Forum.Mapping
{
  public interface IMapFrom<TEntity>
  {
    void MapFrom(TEntity entity);
  }
}

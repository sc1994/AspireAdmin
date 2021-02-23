namespace Aspire.Dto
{
    public interface IEntityDto<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}

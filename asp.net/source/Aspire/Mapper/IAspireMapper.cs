namespace Aspire.Mapper
{
    public interface IAspireMapper
    {
        TTarget MapTo<TTarget>(object source);

        TTarget MapTo<TSource, TTarget>(TSource source);
    }
}

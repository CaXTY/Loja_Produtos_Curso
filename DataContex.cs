using LojaProdutosCurso.Data;

internal class DataContex
{
    public object Produtos { get; internal set; }

    public static implicit operator DataContex(DataContext v)
    {
        throw new NotImplementedException();
    }
}
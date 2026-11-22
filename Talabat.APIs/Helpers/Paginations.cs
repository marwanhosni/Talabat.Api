using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers;

public class Paginations<T> 
{
    public Paginations(int pageIndex, int pageSize, int cout, IReadOnlyList<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Cout = cout;
        Data = data;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Cout { get; set; }
    public IReadOnlyList<T> Data{ get; set; }
}


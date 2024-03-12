namespace Shared.Pagination;
public abstract class BaseRequestParameters
{
    const int maxPageSize = 1000;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }
    //DATATABLES NET DEFAULT PAYLOAD
    public List<Column> columns { get; set; } = new List<Column>();
    public Search search { get; set; } = new();
    public List<Order> order { get; set; } = new List<Order>();
}

public class Column
{
    public string data { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public bool searchable { get; set; }
    public bool orderable { get; set; }
    public Search search { get; set; } = new();
}

public class Search
{
    public string value { get; set; } = string.Empty;
    public string regex { get; set; } = string.Empty;
}

public class Order
{
    public int column { get; set; }
    public string dir { get; set; } = string.Empty;
}

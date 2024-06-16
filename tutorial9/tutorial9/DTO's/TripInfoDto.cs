namespace tutorial9.DTO_s;

public class TripInfoDto //klasa odpowiedzialna za HttpGet ze stronicowanim
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDto> Trips { get; set; } = new List<TripDto>();
}
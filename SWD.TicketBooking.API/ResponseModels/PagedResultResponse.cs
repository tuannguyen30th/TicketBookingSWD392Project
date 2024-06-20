namespace SWD.TicketBooking.API.ResponseModels
{
    public class PagedResultResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}

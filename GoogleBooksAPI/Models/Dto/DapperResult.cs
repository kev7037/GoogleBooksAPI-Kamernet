namespace GoogleBooksAPI.Models.Dto
{
    public class DapperResult<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageItemCount { get; set; } = 40;
        public int TotalRows { get; set; } = 0;
    }

}
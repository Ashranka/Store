namespace Store.RequestHelpers
{
    public class PaginationParams
    {
        private const int MaxPageSixe = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 5;

        public int PageSize     
        { 
            get => _pageSize;
            set => _pageSize = value > MaxPageSixe ? MaxPageSixe: value;
        }
    }
}

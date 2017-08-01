namespace WebAppHomeWork.ViewModel
{
    public class SearchBaseModel
    {
        public int PageStart { get; set; }
        public int PageSize { get; set; }
        public int Draw { get; set; }
        public int SortCol { get; set; }
        public string SortDir { get; set; }

        public string Search { get; set; }
    }
}

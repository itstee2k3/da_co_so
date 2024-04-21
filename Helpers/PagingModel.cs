using System;

namespace XTL.Helpers
{
    public class PagingModel
    {
        public int currentpage { get; set; }
        public int countpages { get; set; }
        public int TotalProducts { get; set; } // Thêm thuộc tính TotalProducts

        public Func<int?, string> generateUrl { get; set; }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared
{
    public class BaseSearchDto
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private const int MaxPageSize = 50;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 10 : value;
        }

        public string SortBy { get; set; } = "Id";
        public bool IsDescending { get; set; } = false;
    }
}

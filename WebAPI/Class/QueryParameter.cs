using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Class
{


    public class QueryParameter
    {
        const int _maxSize = 50;
        private int _size = 25;

        public int page { get; set; }

        public int size
        {
            get { return _size; }
            set
            {
                _size = Math.Min(_maxSize, value);
            }
        }

        public string sortBy { get; set; } = "Id";

        private string _sortOrder = "desc";

        public string sortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                if (value == "asc" || value == "desc")
                {
                    _sortOrder = value;
                }
            }
        }

    }
}

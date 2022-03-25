using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Class
{
    public class ProductQueryParameter : QueryParameter
    {

        public string sku { get; set; }
        public decimal? maxPrice { get; set; }
        public decimal? minPrice { get; set; }
        public string Name { get; set; }
        public string searachItem { get; set; }

    }
}

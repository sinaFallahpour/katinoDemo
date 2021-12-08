using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AllResult<TData>
        //where TData 
    {
        public double PageCount { get; set; }
        public TData ListOfData { get; set; }
    }
}

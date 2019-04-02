using System;
using System.Collections.Generic;

namespace Light.WebApi.Client
{
    public class PageList<T> 
    {
        public List<T> List { get; private set; }

        public int TotalCount { get; private set; }

        public PageList(List<T> list,int totalCount)
        {
            List = list ?? throw new ArgumentNullException(nameof(list));
            TotalCount = totalCount;
        }
    }
}

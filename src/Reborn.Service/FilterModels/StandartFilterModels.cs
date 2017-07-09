using System;
using System.Collections.Generic;
using System.Text;

namespace Reborn.Service.FilterModels
{
    public class StandartFilterModels
    {
        public class GetByIdFilterModel
        {
            public string Id { get; set; }
        }

        public abstract class BasePagingModel
        {
            public int PageSize { get; set; }
            public int Page { get; set; }
            public bool TotalCount { get; set; }
        }
    }
}

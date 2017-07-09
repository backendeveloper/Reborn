namespace Reborn.Service.FilterModels
{
    public class CategoryFilterModels
    {
        public class GetPageFilterModel : StandartFilterModels.BasePagingModel
        {
            public int Status { get; set; }
            public string Slug { get; set; }
        }
    }
}

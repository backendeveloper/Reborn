namespace Reborn.Service.RequestModels
{
    public class CategoryRequestModels
    {
        public class GetPageRequestModel : StandartRequestModels.BasePagingModel
        {
            public int Status { get; set; }
            public string Slug { get; set; }
        }
    }
}

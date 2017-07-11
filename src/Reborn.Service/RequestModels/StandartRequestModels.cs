namespace Reborn.Service.RequestModels
{
    public class StandartRequestModels
    {
        public class GetByIdRequestModel
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

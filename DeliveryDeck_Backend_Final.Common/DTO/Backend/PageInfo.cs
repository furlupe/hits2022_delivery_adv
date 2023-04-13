using Microsoft.AspNetCore.Http;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class PageInfo
    {
        public int CurrentPage { get; set; }
        public int Pages { get; set; }
        public int PageSize { get; set; }

        public PageInfo(int collectionSize, int pageSize, int currPage = 1)
        {
            var pagesAmount = (int)Math.Ceiling((double)collectionSize / pageSize);

            if ((pagesAmount < currPage || currPage < 1) && collectionSize != 0)
            {
                throw new BadHttpRequestException("Page out of range");
            }

            var diff = collectionSize - (currPage - 1) * pageSize;

            CurrentPage = currPage;
            Pages = pagesAmount;
            PageSize = diff >= pageSize ? pageSize : diff;
        }
    }
}

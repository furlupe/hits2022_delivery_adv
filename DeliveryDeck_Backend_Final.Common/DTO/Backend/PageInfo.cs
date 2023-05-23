using Microsoft.AspNetCore.Http;

namespace DeliveryDeck_Backend_Final.Common.DTO.Backend
{
    public class PageInfo
    {
        public int CurrentPage { get; set; }
        public int Pages { get; set; }
        public int PageSize { get; set; }

        public PageInfo() { }

        public PageInfo(int collectionSize, int pageSize, int currPage = 1)
        {
            var pagesAmount = (int)Math.Ceiling((double)collectionSize / pageSize);

            if (pagesAmount < currPage && collectionSize != 0 || currPage < 1)
            {
                throw new BadHttpRequestException("Page out of range");
            }

            var diff = (collectionSize != 0) ? collectionSize - (currPage - 1) * pageSize : 0;

            CurrentPage = currPage;
            Pages = pagesAmount;
            PageSize = diff >= pageSize ? pageSize : diff;
        }
    }
}

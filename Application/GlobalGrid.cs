namespace Application
{
    public class GlobalGrid
    {
        public virtual GlobalGrid DefaultPagination(int? pageNumber = 1, int? Count = 10)
        {
            return new GlobalGrid
            {
                PageNumber = pageNumber,
                Count = count,
                Skip = skip,
                Take = take
            };
        }

        int? pageNumber;
        public int? PageNumber
        {
            get
            {
                if (pageNumber == null || pageNumber <= 0)
                    return 1;
                return pageNumber.Value;
            }
            set
            {
                pageNumber = value;
            }
        }

        int? count;
        public int? Count
        {
            get
            {
                if (count == null || count <= 0)
                    return 10;
                if (count > 100)
                    return 100;

                return count.Value;
            }
            set
            {
                count = value;
            }
        }

        int skip;
        internal int Skip
        {
            get
            {

                return (PageNumber.Value - 1) * Count.Value;
            }
            set
            {
                skip = value;
            }

        }

        int take;
        internal int Take
        {
            get
            {
                return Count.Value;
            }
            set
            {
                take = value;
            }

        }
    }
}

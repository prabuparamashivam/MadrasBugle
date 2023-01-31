using System.Collections.Generic;

namespace RandomBugle.Helpers
{
    public static class PageHelpers
    {
        public static IEnumerable<int> PageNumbers(int pageNumber, int pageCount)
        {

            if (pageCount <= 5)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    yield return i;
                }
            }
            else
            {

                int midpoint = pageNumber < 3 ? 3
                    : pageNumber > pageCount - 2 ? pageCount - 2
                    : pageNumber;

                int lowerbound = midpoint - 2;
                int upperBound = midpoint + 2;

                if (lowerbound != 1)
                {

                    yield return 1;
                    if (lowerbound - 1 > 1)
                    {
                        yield return -1;
                    }
                }
                for (int i = midpoint - 2; i <= upperBound; i++)
                {
                    yield return i;
                }

                if (upperBound != pageCount)
                {
                    if (pageCount - upperBound > 1)
                    {
                        yield return -1;
                    }
                    yield return pageCount;

                }
            }

        }
    }
}

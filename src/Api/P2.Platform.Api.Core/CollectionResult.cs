namespace P2.Platform.Api.Core
{
    using System.Collections.Generic;

    public class CollectionResult<T> where T : class
    {
        public int Count { get; set; }
        public int Total { get; set; }
        public int Top { get; set; }
        public int Skip { get; set; }
        public IList<T> Results { get; set; }
    }

    public class NullCollectionResult<T> : CollectionResult<T> where T: class
    {
        public NullCollectionResult()
        {
            this.Count = 0;
            this.Total = 0;
            this.Top = 0;
            this.Skip = 0;
            this.Results = new List<T>();
        }
    }
}

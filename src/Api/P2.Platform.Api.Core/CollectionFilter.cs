namespace P2.Platform.Api.Core
{
    using System;
    using System.Reflection;

    public class CollectionFilter
    {
        public int? Skip { get; set; }
        public int? Top { get; set; }
        public string OrderBy { get; set; }
        public string Keywords { get; set; }
    }

    public static class CollectionFilterExtension
    {
        public static bool ParseOrderBy(this CollectionFilter filter, out string sort, out bool ascending)
        {
            sort = String.Empty;
            ascending = false;

            var hasProperty = false;

            if (!String.IsNullOrEmpty(filter.OrderBy))
            {
                string[] splits = filter.OrderBy.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (splits.Length > 0)
                {
                    hasProperty = filter.GetType().GetProperty(splits[0], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) != null;

                    if (hasProperty)
                    {
                        // parse sorting field
                        sort = splits[0];
                    }
                }

                // parse order direction
                if (splits.Length > 1)
                {
                    switch (splits[1].ToLower())
                    {
                        case "asc":
                            ascending = true;
                            break;
                        case "desc":
                            ascending = false;
                            break;
                    }
                }
            }

            return hasProperty;
        }
    }
}

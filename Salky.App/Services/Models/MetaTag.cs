namespace Salky.App.Services
{
    public struct MetaTag
    {
        public string? SiteName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Href { get; set; }

        public bool IsMinialFilled()
        {
            return 
                Title != null ||
                Title != null && Picture != null ||
                Description != null ||
                Description != null && Picture != null;
        }
    }
}

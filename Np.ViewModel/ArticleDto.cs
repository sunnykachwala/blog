namespace Np.ViewModel
{
    public class ArticleDto
    {
        public Guid ArticleId { get; set; }

       
        public string Title { get; set; }

    
        public string Content { get; set; }

        public Guid AuthorId { get; set; }
   

        public string? DefaultImage { get; set; }

   
        public bool IsPublished { get; set; }
        public DateTime PublishedDate { get; set; }

        public int DispalyOrder { get; set; }

        #region Seo
     
        public string? Keywords { get; set; }

       
        public string? MetaDescription { get; set; }

        
        public string? MetaTitle { get; set; }

     
        public string Slug { get; set; }
        #endregion

     
        public string IpAddress { get; set; }
    }
}

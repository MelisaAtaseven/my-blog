//yorum tablosu
using System;

namespace MyBlog.Models
{
    public class Comment
    {
        public int Id { get; set; } 
        public string Content { get; set; } 
        public string UserName { get; set; } 
        public string EmailAddress { get; set; } 
 
        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        public int BlogId { get; set; } // İlgili blog yazısının kimliği
        public Blog Blog { get; set; } // Yorumun ait olduğu blog yazısı

    }
}

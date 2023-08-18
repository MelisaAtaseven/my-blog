using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogContext _context;

        public HomeController(ILogger<HomeController> logger, BlogContext context)
        {
            _logger = logger; 
            _context = context; 
        }

        // About sayfasına yönlendirme
        public IActionResult About()
        {
            return View(); 
        }

        // Contact sayfasına yönlendirme
        public IActionResult Contact()
        {
            return View(); 
        }

        public IActionResult Index()
        {
            // İndex sayfasında gösterilecek en son yayınlanmış blog yazıları çekilir
            var list = _context.Blog.Take(10).Where(b => b.IsPublish).OrderByDescending(x => x.CreateTime).ToList();

            // Her bir blog yazısının yazar bilgisi eklenir
            foreach (var blog in list)
            {
                blog.Author = _context.Author.Find(blog.AuthorId);
            }

            return View(list); // Index view'ına blog yazılarının listesi gönderilir
        }

        // Yorum ekleme methodu
        [HttpPost]
        public ActionResult Comment(int postId, string commentContent, string emailAddress, string name)
        {
            // Yeni yorum nesnesi oluşturulur ve veritabanına eklenir
            Comment newComment = new Comment
            {
                Content = commentContent,
                UserName = name,
                EmailAddress = emailAddress,
               

                BlogId = postId, // BlogId postId değerine göre ayarlanır
                CreatedAt = DateTime.Now // Oluşturulma zamanı ayarlanır
            };

            _context.Comments.Add(newComment);
            _context.SaveChanges();

            return RedirectToAction("Post", new { id = postId }); // İlgili blog yazısının detay sayfasına yönlendirilir
        }

        public IActionResult Post(int Id)
        {
            var blog = _context.Blog.Find(Id);
            if (blog == null)
            {
                return NotFound(); // Blog yazısı bulunamazsa
            }

            blog.Author = _context.Author.Find(blog.AuthorId);
            blog.ImagePath = "/img/" + blog.ImagePath;

            // Blog yorumları ekleme
            blog.Comments = _context.Comments.Where(c => c.BlogId == Id).ToList();

            return View(blog); // Post view'ına blog nesnesi gönderilir
        }

        // Hata sayfası
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

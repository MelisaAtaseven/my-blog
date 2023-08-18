using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
public class SubscriptionController : Controller
{
    private readonly BlogContext _context;

    public SubscriptionController(BlogContext context)
    {
        _context = context;
    }
    [HttpPost]
    public IActionResult Subscribe(string email)
    {
        // E-posta adresi boş değilse işlemleri gerçekleştir
        if (!string.IsNullOrEmpty(email))
        {
            // Yeni abonelik nesnesi oluşturuluyor
            Subscription subscription = new Subscription
            {
                Email = email,
            };

            // Oluşturulan Subscription nesnesi veritabanına ekleniyor
            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();
        }
        return RedirectToAction("Home"); 
    }

}

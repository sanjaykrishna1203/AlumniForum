using AlumniForum.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlumniForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Alumni()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Posts()
        {
            return View();
        }

        public IActionResult AddEvent()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddPost()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public JsonResult Register(string rawData)
        {
            using AlumniContext db = new AlumniContext();
            try
            {
                if(rawData != null)
                {
                    User user = JsonConvert.DeserializeObject<User>(rawData);
                    var userName = db.Users.Where(x => x.Email == user.Email).Select(x=>x.Email).ToList();
                    if (userName.Count == 0)
                    {
                        user.Password = CreateMD5(user.Password);
                        user.Role = 1;
                        db.Users.Add(user);
                        db.SaveChanges();
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
                
                
                
            }
            catch(Exception ex)
            {
                return Json(false);
            }
        
        
        }

        [HttpPost]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]
        public IActionResult Validator(string email, string password)
        {
            try
            {
                User user = new User();
                
                
                    using AlumniContext db = new AlumniContext();
                    password = CreateMD5(password);


                    user = db.Users.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
                    
                
                
                if (user != null)
                {
                    HttpContext.Session.SetString("Email",email);
                    HttpContext.Session.SetString("UserRefId", user.Id.ToString());
                    HttpContext.Session.SetString("Role", user.Role.ToString());
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, email)
                };
                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Invalid Username or Password";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Some Error Occured, Please Try again Later!";
                Console.WriteLine(ex.Message);
                return View();
            }

        }

        [HttpPost]
        [ActionName("AddPost")]
        public JsonResult AddPost(string title, string description)
        {
            using AlumniContext db = new AlumniContext();
            var  post = new Post();
            post.CreatedAt = DateTime.Now;
            post.Description = description;
            post.Title = title;
            post.UserRefId = Convert.ToInt32(HttpContext.Session.GetString("UserRefId"));
            db.Posts.Add(post);
            db.SaveChanges();
            return Json(true);
            
            
        }

        [HttpPost]
        [ActionName("AddEvent")]
        public JsonResult AddEvent(string data)
        {
            using AlumniContext db = new AlumniContext();
            var events = new Event();
            events = JsonConvert.DeserializeObject<Event>(data);
            events.UserRefId = Convert.ToInt32(HttpContext.Session.GetString("UserRefId"));
            db.Events.Add(events);
            db.SaveChangesAsync();
            return Json(true);

        }
        [HttpPost]
        [ActionName("GetEvents")]
        public JsonResult GetEvents()
        {
            using AlumniContext db = new AlumniContext();
            var result = db.Events.Where(x => x.Id != 0).Include(x => x.UserRef).OrderByDescending(x => x.DateAndTime).FirstOrDefault(); 
            return Json(result);
        }



        [HttpPost]
        public JsonResult GetPosts()
        {
            using AlumniContext db = new AlumniContext();
            var result = db.Posts.Where(x=>x.Id != 0).Include(x=>x.UserRef).OrderByDescending(x=>x.CreatedAt).ToList();
            return Json(result);
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}

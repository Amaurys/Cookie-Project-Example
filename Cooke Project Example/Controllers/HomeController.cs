using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cooke_Project_Example.Models;
using Microsoft.AspNetCore.Http;

namespace Cooke_Project_Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            if (Get("key") != null)
            {
               ViewBag.ItemName = Get("key").ToString();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ShowCookies(int id)
        {
            //set the cookies here
            //if()
            string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["key"];
            string cookieValueFromReq = Request.Cookies["key"];

            Set("key", "Item " + id, 100);

            var itemId = id;
            var itemName = string.Empty;

            switch(id){
                case 1:
                    itemName = "Item 1";
                    break;
                case 2:
                    itemName = "Item 2";
                    break;
                case 3:
                    itemName = "Item 3";
                    break;
                default:
                    itemName = "Item no selected";
                    break;
            }

            var model = new ItemModel()
            {
                //Categ
                ItemId = itemId,
                ItemName = itemName
            };

            //return View(model);
            return RedirectToAction("index");
        }

        public string Get(string key) {
            return Request.Cookies[key];
        }

        public void Set(string key, string value, int? expireTime) {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            Response.Cookies.Append(key, value, option);
        }

        public IActionResult Remove(string key) {
            Response.Cookies.Delete(key);

            return Redirect("index");
        }
    }
}

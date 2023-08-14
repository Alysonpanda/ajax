using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT150Site.Models;

namespace MSIT150Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;

        private readonly IWebHostEnvironment _host;
        public ApiController(DemoContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public async Task< IActionResult> CheckAccount(string Name)
        {
            //if ( string.IsNullOrEmpty(Name))
            //{
            //    return Json(new { message = "請輸入姓名" });                
            //}
            var memberExitName = await  _context.Members.AnyAsync( m => m.Name ==Name);

            if (memberExitName)
            {
                return Json(new { message = "該姓名已存在" });            
            }

            return Json( new {  message=""});
        }


        public IActionResult Index()
        {
            //Content不是View，不會產生畫面，Content代表很簡單的ex.句子
            //執行後一樣輸入controller名字/方法名 api/index

            //讓程式睡五秒鐘，晚一點出現hello ajax
            //System.Threading.Thread.Sleep(5000);
            //return Content("Hello Ajax!!");

            //改回在homeController的First方法顯示Hello Ajax!!

            //改成fetch            
            return Content("Hello Fetch!!");
        }

        //public IActionResult GetDemo( string name , int age=30) ，改成用類別
        public IActionResult GetDemo(CUserViewModel user)
        {
            if (string.IsNullOrEmpty(user.name))
            {
                user.name = "guest";            
            }
            return Content( $"Hello { user.name} , you are {user.age } years old.");

        }

        //放資料
        //public IActionResult Register(Members member)
        //{
        //    _context.Members.Add(member);
        //    _context.SaveChanges();

        //    return Content("新增成功!!");
        //}

        //放照片用IFormFile
        public IActionResult Register(Members member, IFormFile file)
        {
            //
            //return Content($"{_host.ContentRootPath}");  //C:\shared\Ajax\MSIT150Site\
            //return Content($"{_host.WebRootPath}");  //C:\shared\Ajax\MSIT150Site\wwwroot
            string filePath = Path.Combine(_host.WebRootPath, "uploads", file.FileName); //C:\shared\Ajax\MSIT150Site\wwwroot\uploads\walk.gif

            //上傳檔案到uploads資料夾中
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            //return Content($"上傳檔案到 {filePath}");

            //  return Content($"{file.FileName} - {file.Length} - {file.ContentType}");

            //_context.Members.Add(member);
            //_context.SaveChanges();

            //把圖轉成二進位
            byte[]? imgByte = null; 
            using (var memoryStream = new MemoryStream())   //filestream是上傳，memorystream是傳路徑
            {
                file.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();     //把這個路徑定義為照片
            }

            //存至member資料表中
            member.FileName = file.FileName;
            member.FileData = imgByte;
            _context.Members.Add(member);
            _context.SaveChanges();           

            return Content("新增成功!!");
        }


        //把照片顯示在list
        public IActionResult GetImageByte(int id = 1)
        {
            //用find方法，不要用where.firstordefault，find會直接找pk
            Members? member = _context.Members.Find(id);
            byte[]? img = member.FileData;
            return File(img, "image/jpeg");  //file裡面的參數也有別的可選ex.text

        }

        //先找出城市，用json呈現
        public IActionResult Cities()
        {
            var cities = _context.Address.Select(c => c.City).Distinct();
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return Json(cities);
        }

        //根據城市名稱，回傳城市的鄉鎮區JSON資料
        public IActionResult Districts(string city)
        {
            var districts = _context.Address.Where(a => a.City == city).Select(c => c.SiteId).Distinct();

            return Json(districts);
        }

        //根據鄉鎮區名稱，回傳鄉鎮區的路名JSON資料
        public IActionResult Roads(string siteId)
        {
            var roads = _context.Address.Where(a => a.SiteId == siteId).Select(c => c.Road).Distinct();

            return Json(roads);
        }

    }
}

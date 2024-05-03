using eProject3.Data;
using eProject3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eProject3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public FileController(ApplicationDbContext context)
        {
            _context = context;
        }
        //[HttpPost]
        //public ActionResult File(IFormFile imageFile)
        //{
        //    if (imageFile != null && imageFile.Length > 0)
        //    {
        //        byte[] imageData;
        //        using (var binaryReader = new BinaryReader(imageFile.InputStream))
        //        {
        //            imageData = binaryReader.ReadBytes(imageFile.Length);
        //        }

        //        // Lưu trữ thông tin tệp ảnh vào cơ sở dữ liệu
        //        FileEntity image = new FileEntity
        //        {
        //            FileName = imageFile.FileName,
        //            Data = imageData
        //        };
        //        _context.Files.Add(image);
        //        _context.SaveChanges();

        //        return RedirectToAction("Index"); // Hoặc chuyển hướng đến một trang khác sau khi tải lên thành công
        //    }

        //    // Xử lý khi không có tệp được chọn hoặc có lỗi xảy ra

        //    return RedirectToAction("Index");
        //}


        //[HttpPost]
        //public ActionResult UploadImage(IFormFile imageFile)
        //{
        //    if (imageFile != null && imageFile.Length > 0)
        //    {
        //        // Xử lý tệp tin được tải lên
        //        // Ví dụ: Lưu tệp tin vào ổ đĩa
        //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", imageFile.FileName);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            imageFile.CopyTo(stream);
        //        }

        //        // Đọc dữ liệu từ tệp tin vào một mảng byte[]
        //        byte[] imageData;
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            imageFile.CopyTo(memoryStream);
        //            imageData = memoryStream.ToArray();
        //        }

        //        // Thêm dữ liệu vào cơ sở dữ liệu
        //        var image= new FileEntity
        //        {
        //            FileName = imageFile.FileName,
        //            Data = imageData
        //        };
        //        _context.Files.Add(image);
        //        _context.SaveChanges();

        //        // Thực hiện các tác vụ khác sau khi tải lên và lưu dữ liệu thành công
        //        // ...
        //    }

        //    return RedirectToAction("Index");
        //}
    }
}

using eProject3.Data;
using eProject3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eProject3.Repository
{
    public interface IPaintingRepository : IBaseRepository<Painting>
    {
        Task<Painting> CreatePainting(Painting entity, IFormFile file);
        Task<Painting> UpdatePainting(Painting entity, IFormFile file);
        Task<List<Painting>> GetAllPaintingsInCompetition();
        Task<List<Painting>> GetAllPaintingsInExhibition();
        Task<List<Painting>> GetPaintingInExByKeyword(string keyword);
        Task<List<Painting>> GetPaintingInExByPrice(float price);
        Task<List<Painting>> GetPaintingsWithBeingMarked();
    }
    public class PaintingRepository : BaseRepository<Painting>, IPaintingRepository
    {
        private ApplicationDbContext _context;
        public PaintingRepository(ApplicationDbContext context, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor) : base(context, userManager, contextAccessor)
        {
            _context = context;
        }

        public async Task<Painting> CreatePainting(Painting entity, IFormFile file)
        {
            //using (var StreamUploadFile = new FileStream(SavePath, FileMode.CreateNew))
            //{
            //    file.CopyTo(StreamUploadFile);
            //}
            // Check if a file was actually uploaded
            if (file == null || file.Length == 0 || entity == null)
            {
                // Handle the case when no file was selected for upload
                // Redirect to an appropriate action or return an error message
                return null;
            }

            // Get the file name and extension
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName);

            // Generate a unique file name to prevent conflicts
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            entity.FileEntity = uniqueFileName;

            // Define the path where the file will be saved
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", uniqueFileName);

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            entity.CreatedUser = currentUserId;
            entity.CreatedTime = DateTime.Now;
            entity.IsDeleted = false;
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;

            // Optionally, you can perform additional operations with the uploaded file
            // For example, you can store information in a database or process the file's content


        }
        public async Task<Painting> UpdatePainting(Painting entity, IFormFile file)
        {
            var FileEntity = entity.FileEntity;
            if (file != null)
            {
                string uploadImgName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", uploadImgName);

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                using (var StreamUploadFile = new FileStream(savePath, FileMode.CreateNew))
                {
                    file.CopyTo(StreamUploadFile);
                }

                entity.FileEntity = uploadImgName;
            }

            _dbSet.Update(entity);
            entity.UpdatedUser = currentUserId;
            entity.UpdatedTime = DateTime.Now;
            entity.IsDeleted = false;
            await _context.SaveChangesAsync();
            return entity;

            // Optionally, you can perform additional operations with the uploaded file
            // For example, you can store information in a database or process the file's content


        }

        public async Task<List<Painting>> GetAllPaintingsInCompetition()
        {
            var query = from p in _context.Paintings.AsQueryable()
                        join pa in _context.PaintingInComps.AsQueryable() on p.Id equals pa.PaintingId
                        where p.IsDeleted == false && pa.IsDeleted == false
                        select p;
            return await query.Where(x =>
            x.CreatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email
            || x.UpdatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email).ToListAsync();
        }

        public async Task<List<Painting>> GetAllPaintingsInExhibition()
        {
            var query = from p in _context.Paintings.AsQueryable()
                        join ex in _context.PaintingInExs.AsQueryable() on p.Id equals ex.PaintingId
                        where p.IsDeleted == false && ex.IsDeleted == false
                        select p;
            return await query.Where(x =>
            x.CreatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email
            || x.UpdatedUser == _userManager.GetUserAsync(_contextAccessor.HttpContext.User).GetAwaiter().GetResult().Email).ToListAsync();
        }

        public async Task<List<Painting>> GetPaintingInExByKeyword(string keyword)
        {
            var query = from st in _context.Paintings.AsQueryable()
                        join pa in _context.PaintingInExs.AsQueryable() on st.Id equals pa.PaintingId

                        where st.Name.ToLower().Contains(keyword.ToLower()) ||
                              st.Description.ToLower().Contains(keyword.ToLower()) ||
                              st.Code.ToLower().Contains(keyword.ToLower())
                        select st;

            return await query.ToListAsync();
        }

        public async Task<List<Painting>> GetPaintingInExByPrice(float price)
        {
            var query = from st in _context.Paintings.AsQueryable()
                        join pa in _context.PaintingInExs.AsQueryable() on st.Id equals pa.PaintingId
                        where pa.Price.Equals(price)
                        select st;
            return await query.ToListAsync();
        }
        public async Task<List<Painting>> GetPaintingsWithBeingMarked()
        {
            var validMarks = new List<string> { "best", "better", "good", "moderate", "normal", "disqualified" };

            var query = from ma in _context.Paintings.AsQueryable()
                        join p in _context.PaintingInComps.AsQueryable() on ma.Id equals p.PaintingId
                        join bb in _context.MarkOfPaintInComps.AsQueryable() on p.Id equals bb.PaintingInCompId
                        where validMarks.Contains(bb.Mark.ToLower())
                        select ma;

            return await query.ToListAsync();
        }
    }
}

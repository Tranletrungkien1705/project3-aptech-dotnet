using eProject3.DTO;
using eProject3.Models;
using eProject3.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using System.Security.Cryptography.X509Certificates;
using X.PagedList;

namespace eProject3.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize(Roles ="STUDENT")]
    public class StudentController : APIBaseController<Student>
    {
        private IStudentRepository _studentRepository;
        private IPaintingRepository _paintingRepository;
        private IAwardRepository _awardRepository;
        private IPaintingInCompRepository _paintingInCompRepository;
        private IPaintingInExRepository _paintingInExRepository;
        private ICompetitionRepository _competitionRepository;
        private IMarkOfPaintInCompRepository _markOfPaintInCompRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISubInClassRepository _subInClassRepository;
        public StudentController(IBaseRepository<Student> repository, IStudentRepository studentRepository, IPaintingRepository paintingRepository, IAwardRepository awardRepository, IPaintingInCompRepository paintingInCompRepository, IPaintingInExRepository paintingInExRepository, ICompetitionRepository competitionRepository, IMarkOfPaintInCompRepository markOfPaintInCompRepository, IClassRepository classRepository, ISubInClassRepository subInClassRepository) : base(repository)
        {
            _studentRepository = studentRepository;
            _paintingRepository = paintingRepository;
            _awardRepository = awardRepository;
            _paintingInCompRepository = paintingInCompRepository;
            _paintingInExRepository = paintingInExRepository;
            _competitionRepository = competitionRepository;
            _markOfPaintInCompRepository = markOfPaintInCompRepository;
            _classRepository = classRepository;
            _subInClassRepository= subInClassRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Students(string? sortOrder, string? q, int? page)
        {
            List<Student> entity = await _studentRepository.GetAllAsync(); ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _studentRepository.GetStudentByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public async Task<IActionResult> Painting(string? sortOrder, string? q, int? page)
        {
            List<Painting> entity = await _paintingRepository.GetAllAsync(); ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _paintingRepository.GetAllAsync().GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> PaintingInEx(string? sortOrder, string? q, int? page)
        {
            List<PaintingInEx> entity = await _paintingInExRepository.GetAllPublicAsync(); ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _paintingInExRepository.GetAllAsync().GetAwaiter().GetResult();
                return View(entity);
            }
            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> StaffsFeedback(string? sortOrder, string? q, int? page)
        {
            List<MarkOfPaintInComp> entity = await _markOfPaintInCompRepository.GetAllPublicAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _markOfPaintInCompRepository.GetMarkAndFeedBackOfPainInCompByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> UpcomingCompetition(string? sortOrder, string? q, int? page)
        {
            List<Competition> entity = await _competitionRepository.GetAllPublicAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _competitionRepository.GetCompetionByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Award(string? sortOrder, string? q, int? page)
        {
            List<Award> entity = await _awardRepository.GetAllPublicAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _awardRepository.GetAwardsWhichBeingReceivedByStudentInCompetitionByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet("[controller]/[action]")]
        public ActionResult AddOrUpdatePainting()
        {
            return View();
        }

        //paintings
        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> AddOrUpdatePainting(Painting entity, IFormFile FileEntity)
        {
            Painting result;
            entity.Id = Convert.ToInt32(TempData["idOfPain"]);
            var findId = entity.Id;
            if (findId > 0)
            {
                result = await _paintingRepository.UpdatePainting(entity, FileEntity);
            }
            else
            {
                result = await _paintingRepository.CreatePainting(entity, FileEntity);

            }
            if (result != null)
            {
                return RedirectToAction("Painting", "Student");
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdatePainting(int id)
        {
            TempData["idOfPain"] = id;
            Painting x = await _paintingRepository.GetByIdAsync(id);
            return View(x);
        }
        [HttpGet("Student/DeletePaintingASync/{id}")]
        public async Task<ActionResult> DeletePainting(int id)
        {
            //Student entity = new Student();
            //List<Student> x = await _adminRepository.GetStudentByKeyword(id);
            //entity= x.FirstOrDefault();
            //entity.Id = id;
            Painting entity = await _paintingRepository.GetByIdAsync(id);
            var result = await _paintingRepository.DeleteAsync(entity);
            if (result != null)
            {
                return View(nameof(Painting));
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        // students can update their paintings only if not surpassing deadline
        [HttpPut]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> UpdateWithoutSurpassingEndDate(Painting entity, int id)
        {
            DateTime? submissionDeadline = _competitionRepository.GetByIdAsync(id).GetAwaiter().GetResult().EndOfDate;
            if (DateTime.Now <= submissionDeadline)
            {
                var result = await _paintingRepository.UpdateAsync(entity);
                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return BadRequest("Error!");
                }
            }
            else
            {
                return BadRequest("Cannot update after submission deadline");
            }
        }

        //get current award in exact competition
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<List<Award>> GetAwardInExactCompetitionAsync()
        {
            var result = await _awardRepository.GetAwardInExactCompetition();
            return result;
        }

        //paintings with best marked
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<List<Painting>> GetPaintingsWithBeingMarkedAsync()
        {
            var result = await _paintingRepository.GetPaintingsWithBeingMarked();
            return result;
        }

        //paintings in exhibitions
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<List<Painting>> GetPaintingInExesAsync()
        {
            var result = await _paintingRepository.GetAllPaintingsInExhibition();
            return result;
        }

        //=================================================================PaintingInComp========================================================
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> PaintingInComp(string? sortOrder, string? q, int? page)
        {
            List<PaintingInComp> entity = await _paintingInCompRepository.GetAllAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            //if (!String.IsNullOrEmpty(q))
            //{
            //    entity = _paintingInExRepository.GetDetailPaintingInEx().GetAwaiter().GetResult();
            //    return View(entity.ToPagedList(pageNumber, pageSize));
            //}
            return View(entity.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdatePaintingInCompAsync(int id)
        {
            ViewData["paintingName"] = new SelectList(await _paintingRepository.GetAllPublicAsync(), "Id", "Name");
            TempData["competitionId"] = id;
            return View();
        }
        [HttpGet("[controller]/[action]/{id}&{keyword}")]
        public async Task<ActionResult> AddOrUpdatePaintingInCompAsync(int id,int keyword)
        {
            TempData["competitionId"] = id;
            var painInCop = new PaintingInComp();
            painInCop = _paintingInCompRepository.GetAllAsync().GetAwaiter().GetResult().Where(x=>x.PaintingId==keyword && x.CompetitionId==id && x.IsDeleted==false).ToList().FirstOrDefault();
            return View(painInCop);
        }
        //[HttpGet("[controller]/[action]")]
        //public async Task<ActionResult> AddOrUpdatePaintingInCompAsync()
        //{
        //    ViewData["paintingName"] = new SelectList(await _paintingRepository.GetAllPublicAsync(), "Id", "Name");
        //    return View();
        //}


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdatePaintingInCompAsync(PaintingInComp entity)
        {
            var competition = await _competitionRepository.GetByIdAsync((int)entity.CompetitionId);
            if (DateTime.Now > (DateTime)competition.EndOfDate)
            {
                ViewData["ErrorMessage"] = "Error: Date of post is greater than end date of competition.";
                return View(entity);
            }
            entity.DateOfPost= DateTime.Now;
            PaintingInComp result;
            if (entity.Id != 0)
            {
                result = await _paintingInCompRepository.UpdateAsync(entity);
            }
            else
            {
                result = await _paintingInCompRepository.CreateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("Competition", "Student");
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        [HttpDelete("Student/DeletePaintingInCompAsync/{id}")]
        public async Task<ActionResult> DeletePaintingInCompAsync(int id)
        {
            var result = await _paintingInCompRepository.DeleteDetailPaintingInCompAsync(id);
            if (result != null)
            {
                return RedirectToAction("Competition", "Student");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [Route("[controller]/[action]")]
        public async Task<IActionResult> Competition(string? sortOrder, string? q, int? page)
        {
            List<Competition> entity = await _competitionRepository.GetAllPublicAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            //if (!String.IsNullOrEmpty(q))
            //{
            //    entity = _competitionRepository.GetCompetionByKeyword(q).GetAwaiter().GetResult();
            //    return View(entity.ToPagedList(pageNumber, pageSize));
            //}
            return View(entity);
        }
        //========================================================Class==========================================================

        [HttpGet]

        public async Task<IActionResult> Classes(string? sortOrder, string? q, int? page)
        {
            List<Class> entity = _classRepository.GetClassByStudentId();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _classRepository.GetAllAsync().GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }

        //===================================================StudentInSub=====================================================
        [HttpGet]
        [Route("[controller]/[action]/{Id}")]
        public async Task<ActionResult> StudentInSub(string? sortOrder, string? q, int? page, int Id)
        {
            // Lưu dữ liệu vào TempData
            TempData["classId"] = Id;
            //if (!String.IsNullOrEmpty(q))
            //{
            //    entity = _paintingInExRepository.GetDetailPaintingInEx().GetAwaiter().GetResult();
            //    return View(entity.ToPagedList(pageNumber, pageSize));
            //}
            return View();
        }
    }
}

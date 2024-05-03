using eProject3.Models;
using eProject3.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using X.PagedList;

namespace eProject3.Controllers
{
    //[Route("api/[controller]")]
    [Authorize(Roles = "MANAGER")]
    public class ManagerController : APIBaseController<Manager>
    {
        private IManagerRepository _managerRepository;
        private IStudentRepository _studentRepository;
        private IStaffRepository _staffRepository;
        private ICompetitionRepository _competitionRepository;
        private IAwardRepository _awardRepository;
        private IPaintingInCompRepository _paintingInCompRepository;
        private IPaintingInExRepository _paintingInExRepository;
        private IExhibitionRepository _exhibitionRepository;
        private IPaintingRepository _paintingRepository;
        private IMarkOfPaintInCompRepository _markOfPaintInCompRepository;
        public ManagerController(IBaseRepository<Manager> repository, IManagerRepository managerRepository, IStudentRepository studentRepository, IStaffRepository staffRepository, ICompetitionRepository competitionRepository, IAwardRepository awardRepository, IPaintingInCompRepository paintingInCompRepository, IPaintingInExRepository paintingInExRepository, IExhibitionRepository exhibitionRepository, IPaintingRepository paintingRepository, IMarkOfPaintInCompRepository markOfPaintInCompRepository) : base(repository)
        {
            _managerRepository = managerRepository;
            _studentRepository = studentRepository;
            _staffRepository = staffRepository;
            _competitionRepository = competitionRepository;
            _awardRepository = awardRepository;
            _paintingInCompRepository = paintingInCompRepository;
            _paintingInExRepository = paintingInExRepository;
            _exhibitionRepository = exhibitionRepository;
            _paintingRepository = paintingRepository;
            _markOfPaintInCompRepository = markOfPaintInCompRepository;
        }

        //show information of students and staffs
        [HttpGet]
        public async Task<IActionResult> Award(string? sortOrder, string? q, int? page)
        {
            List<Award> entity = await _awardRepository.GetAllPublicAsync(); ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _awardRepository.GetAwardsWhichBeingReceivedByStudentInCompetitionByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public async Task<IActionResult> Competition(string? sortOrder, string? q, int? page)
        {
            List<Competition> entity = await _competitionRepository.GetAllPublicAsync(); ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _competitionRepository.GetCompetionByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }
        //show all competitions
        [HttpGet]
        public async Task<IActionResult> Staffs(string? sortOrder, string? q, int? page)
        {
            List<Staff> entity = await _staffRepository.GetAllPublicAsync(); ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _staffRepository.GetStaffsByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }
        //show all awards
        [HttpGet]
        public async Task<IActionResult> Students(string? sortOrder, string? q, int? page)
        {
            List<Student> entity = await _studentRepository.GetAllPublicAsync(); ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _studentRepository.GetStudentByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }
        //show all paintings in competition 
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<List<Painting>> GetPaintingsInCompetitionAsync()
        {
            var result = await _paintingRepository.GetAllPaintingsInCompetition();
            return result;
        }
        //show all feedbacks and remarks of paintings
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<List<MarkOfPaintInComp>> GetFeebacksAndRemarksAsync()
        {
            var result = await _markOfPaintInCompRepository.GetAllPublicAsync();
            return result;
        }
        //show all paintings in exhibitions
        //[HttpGet]
        //[Route("[controller]/[action]")]
        //public IActionResult Exhibitions(string? sortOrder, string? q)
        //{
        //    if (!String.IsNullOrEmpty(q))
        //    {
        //        List<Exhibition> entity = _exhibitionRepository.GetExhibitionByKeyword(q).GetAwaiter().GetResult();
        //        return View(entity);
        //    }
        //    return View();
        //}
        //CRUDI + search exhibitions
        [HttpGet]
        public async Task<IActionResult> Exhibition(string? sortOrder, string? q, int? page)
        {
            List<Exhibition> entity = await _exhibitionRepository.GetAllPublicAsync(); ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _exhibitionRepository.GetExhibitionByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdateExhibitionAsync(int id)
        {
            Exhibition x = await _exhibitionRepository.GetByIdAsync(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]")]
        public ActionResult AddOrUpdateExhibitionAsync()
        {
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdateExhibitionAsyn(Exhibition entity)
        {
            Exhibition result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _exhibitionRepository.UpdateAsync(entity);
            }
            else
            {
                result = await _exhibitionRepository.CreateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("Exhibition", "Manager");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("Manager/DeleteExhibitionAsync/{id}")]
        public async Task<ActionResult> DeleteExhibitionAsync(int id)
        {
            Exhibition entity = await _exhibitionRepository.GetByIdAsync(id);
            var result = await _exhibitionRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("Exhibition", "Manager");
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<Exhibition> GetExhibitionById(int id)
        {
            var result = await _exhibitionRepository.GetByIdAsync(id);
            return result;
        }
        //[HttpGet]
        //[Route("[controller]/[action]")]
        //public async Task<List<Exhibition>> GetExhibitionByKeyword(string keyword)
        //{
        //    var result = await _exhibitionRepository.GetExhibitionByKeyword(keyword);
        //    return result;
        //}
    }
}

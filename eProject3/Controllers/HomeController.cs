using eProject3.Data;
using eProject3.Models;
using eProject3.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using X.PagedList;
namespace eProject3.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStudentRepository _studentRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IPaintingRepository _paintingRepository;
        public HomeController(IHttpContextAccessor httpContextAccessor, UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IStudentRepository studentRepository, ICompetitionRepository competitionRepository, IPaintingRepository paintingRepository)
        {
            _studentRepository = studentRepository;
            _roleManager = roleManager;
            _competitionRepository = competitionRepository;
            _paintingRepository = paintingRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Roling()
        {
            var x = new DbSeedRole(_roleManager);
            await x.RoleData();
            return Ok("SUcced");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public async Task<IActionResult> EliteStudents(string? sortOrder, string? q, int? page)
        {
            List<Student> entity = await _studentRepository.GetAllPublicAsync();
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
        public async Task<IActionResult> CurrentCompetition(string? sortOrder, string? q, int? page)
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
        public async Task<IActionResult> CurrentPainting(string? sortOrder, string? q, int? page)
        {
            List<Painting> entity = await _paintingRepository.GetAllPublicAsync(); ;
           
            if (!String.IsNullOrEmpty(q))
            {
                entity = _paintingRepository.GetAllPublicAsync().GetAwaiter().GetResult();
                return View(entity);
            }
            return View(entity);
        }
    }
    
}
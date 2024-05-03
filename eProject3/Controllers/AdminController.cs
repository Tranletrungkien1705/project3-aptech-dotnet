using eProject3.Models;
using eProject3.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eProject3.Controllers
{
    //[Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class AdminController : APIBaseController<Administrator>
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRepository _classRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISubInClassRepository _subInClassRepository;
        private readonly IStudentInSubRepository _studentInSubRepository;
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IBaseRepository<Administrator> repository, IAdminRepository adminRepository, IStaffRepository staffRepository, IStudentRepository studentRepository, IClassRepository classRepository,IManagerRepository managerRepository, ISubjectRepository subjectRepository, ISubInClassRepository subInClassRepository, IStudentInSubRepository studentInSubRepository, UserManager<CustomUser> userManager, IHttpContextAccessor contextAccessor, RoleManager<IdentityRole> roleManager) : base(repository)
        {
            _adminRepository = adminRepository;
            _staffRepository = staffRepository;
            _studentRepository = studentRepository;
            _classRepository = classRepository;
            _managerRepository = managerRepository;
            _subjectRepository = subjectRepository;
            _subInClassRepository= subInClassRepository;
            _studentInSubRepository= studentInSubRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Authorizations(string? sortOrder, string? q, int? page)
        {
            var entity = await _adminRepository.GetAllCustomUser() ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            //if (!String.IsNullOrEmpty(q))
            //{
            //    entity = _studentRepository.GetStudentByKeyword(q).GetAwaiter().GetResult();
            //    return View(entity.ToPagedList(pageNumber, pageSize));
            //}
            return View(entity.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> UpdateUser(string id)
        {
            TempData["IdOfUser"] = id;
            var x = await _userManager.FindByIdAsync(id);
            return PartialView(x);
        }
        [HttpPost]
        public async Task<ActionResult> DoUpdateUser([Bind("Id,UserName,Email,PhoneNumber")] CustomUser customUser)
        {
            if(customUser != null)
            {
                var userId = TempData["IdOfUser"].ToString();
                var x = await _userManager.FindByIdAsync(userId);
                if(x != null)
                {
                    x.PhoneNumber = customUser.PhoneNumber;
                    x.UserName = customUser.UserName;
                    x.Email = customUser.Email;
                    await _userManager.UpdateAsync(x);
                    return RedirectToAction("Authorizations");
                }
            }
            return BadRequest();

        }


        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> UpdateRoll(string id)
        {
            TempData["IdOfuser"] = id;
            var x = await _userManager.FindByIdAsync(id);
            var dsAllQuyen = await _roleManager.Roles.ToListAsync();
            if(x != null)
            {
                var userRole = await _userManager.GetRolesAsync(x);
                ViewBag.UserRole = userRole.ToList();
                ViewBag.userId = id;
                //var userRole = await _userManager.GetUsersInRoleAsync(id);
            }
            return PartialView(dsAllQuyen);
        }
        [HttpPost]
        public async Task<ActionResult> DoUpdateRoll(string userId,List<String> arrRole)
        {
            var cuId = TempData["IdOfuser"];
            var user = await _userManager.FindByIdAsync(cuId.ToString());
            var dsAllQuyen = await _roleManager.Roles.ToListAsync();
            var arrQuyen = dsAllQuyen.Select(x => x.Name);
            if (user != null)
                {
                foreach (var quyen in arrQuyen)
                {
                    await _userManager.RemoveFromRoleAsync(user, quyen);
                }
                if(arrRole.Count >0)
                {
                    foreach (var item in arrRole)
                    {
                        await _userManager.AddToRoleAsync(user, item);
                    }
                }
                    return RedirectToAction("Authorizations");
                }
            return BadRequest();

        }

        //CRUDI + search students
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
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdateStudentAsync(int id)
        {
            Student x = await _studentRepository.GetByIdAsync(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]")]
        public ActionResult AddOrUpdateStudentAsync()
        {
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdateStudentAsyn(Student entity)
        {
            Student result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _studentRepository.UpdateAsync(entity);
            }
            else
            {
                result = await _studentRepository.CreateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("Students", "Admin");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("Admin/DeleteStudentAsync/{id}")]
        public async Task<ActionResult> DeleteStudentAsync(int id)
        {
            Student entity = await _studentRepository.GetByIdAsync(id);
            var result = await _studentRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("Students", "Admin");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("[controller]/[action]/{keyword}")]
        public async Task<List<Student>> GetStudentByKeyword(string keyword)
        {
            var result = await _adminRepository.GetStudentByKeyword(keyword);
            return result;
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<Student> GetStudentById(int id)
        {
            var result = await _studentRepository.GetByIdAsync(id);
            return result;
        }

        //CRUDI + SEARCH STAFFS
        [HttpGet]

        public async Task<IActionResult> Staffs(string? sortOrder, string? q, int? page)
        {
            List<Staff> entity = await _staffRepository.GetAllPublicAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _staffRepository.GetStaffsByKeyword(q).GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdateStaffAsync(int id)
        {
            Staff x = await _staffRepository.GetByIdAsync(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]")]
        public ActionResult AddOrUpdateStaffAsync()
        {
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdateStaffAsyn(Staff entity)
        {
            Staff result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _staffRepository.UpdateAsync(entity);
            }
            else
            {
                result = await _staffRepository.CreateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("Staffs", "Admin");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("Admin/DeleteStaffAsync/{id}")]
        public async Task<ActionResult> DeleteStaffAsync(int id)
        {
            Staff entity = await _staffRepository.GetByIdAsync(id);
            var result = await _staffRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("Staffs", "Admin");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("[controller]/[action]/{keyword}")]
        public async Task<List<Staff>> GetStaffsByKeyword(string keyword)
        {
            var result = await _adminRepository.GetStaffByKeyword(keyword);
            return result;
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<Staff> GetStaffById(int id)
        {
            var result = await _staffRepository.GetByIdAsync(id);
            return result;
        }
        [HttpGet]

        public async Task<IActionResult> Classes(string? sortOrder, string? q, int? page)
        {
            List<Class> entity = await _classRepository.GetAllPublicAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            //if (!String.IsNullOrEmpty(q))
            //{
            //    entity = _classRepository.GetStaffsByKeyword(q).GetAwaiter().GetResult();
            //    return View(entity.ToPagedList(pageNumber, pageSize));
            //}
            return View(entity.ToPagedList(pageNumber, pageSize));
        }

        //===================================================SubInClass=====================================================
        [HttpGet]
        [Route("[controller]/[action]/{Id}")]
        public async Task<ActionResult> SubInClass(string? sortOrder, string? q, int? page, int Id)
        {
            // Lưu dữ liệu vào TempData
            TempData["Message"] = Id;
            List<SubInClass> entity = await _subInClassRepository.GetDetailSubInClass(Id);
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
        public async Task<ActionResult> AddOrUpdateSubInClassAsync(int id)
        {
            TempData["idofsubinclass"] = id;
            TempData["idofclass"] = TempData["Message"];
            TempData.Keep("Message");
            SubInClass x = _subInClassRepository.GetDetailSubInClassById(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdateSubInClassAsync()
        {
            return View();
        }


        [HttpPost("Admin/AddOrUpdateSubInClassAsync")]
        public async Task<ActionResult> AddOrUpdateSubInClassAsync(SubInClass entity)
        {
            // Lấy dữ liệu từ TempData
            entity.ClassId = Convert.ToInt32(TempData["Message"]);
            TempData.Keep("Message");
            SubInClass result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _subInClassRepository.UpdateDetailSubInClass(entity);
            }
            else
            {
                result = await _subInClassRepository.CreateDetailSubInClass(entity);
            }
            if (result != null)
            {
                return RedirectToAction("Classes", "Admin");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("Staff/DeleteSubInClassAsync/{id}")]
        public async Task<ActionResult> DeleteSubInClassAsync(int id)
        {
            SubInClass entity = await _subInClassRepository.GetByIdAsync(id);
            var result = await _subInClassRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("Class", "Admin");
            }
            else
            {
                return BadRequest("Error!");
            }
        }
    }
}
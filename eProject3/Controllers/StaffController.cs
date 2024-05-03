using eProject3.Models;
using eProject3.Repository;
using eProject3.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace eProject3.Controllers
{
    //[Route("api/[controller]")]
    [Authorize(Roles = "STAFF,ADMIN")]

    public class StaffController : APIBaseController<Staff>
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IMarkOfPaintInCompRepository _markOfPaintInCompRepository;
        private readonly IAwardRepository _awardRepository;
        private readonly IExhibitionRepository _exhibitionRepository;
        private readonly IPaintingInExRepository _paintingInExRepository;
        private readonly IPaintingRepository _paintingRepository;
        private readonly ISubInClassRepository _subInClassRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IStudentInSubRepository _studentInSubRepository;
        private readonly IPaintingInCompRepository _paintingInCompRepository;
        public StaffController(IBaseRepository<Staff> repository, IStaffRepository staffRepository, IStudentRepository studentRepository, ICompetitionRepository competitionRepository, IMarkOfPaintInCompRepository markOfPaintInCompRepository, IAwardRepository awardRepository, IExhibitionRepository exhibitionRepository, IPaintingInExRepository paintingInExRepository, IPaintingRepository paintingRepository, ISubInClassRepository subInClassRepository, IManagerRepository managerRepository, IClassRepository classRepository, ISubjectRepository subjectRepository, IStudentInSubRepository studentInSubRepository, IPaintingInCompRepository paintingInCompRepository) : base(repository)
        {
            _staffRepository = staffRepository;
            _studentRepository = studentRepository;
            _competitionRepository = competitionRepository;
            _markOfPaintInCompRepository = markOfPaintInCompRepository;
            _awardRepository = awardRepository;
            _exhibitionRepository = exhibitionRepository;
            _paintingInExRepository = paintingInExRepository;
            _paintingRepository = paintingRepository;
            _subInClassRepository = subInClassRepository;
            _managerRepository = managerRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _studentInSubRepository = studentInSubRepository;
            _paintingInCompRepository = paintingInCompRepository;
        }
        [HttpGet]

        public IActionResult Index()
        {
            return RedirectToAction("/Admin/Staffs");
        }
        //=========================================================show all students=========================================

        [HttpGet]

        public async Task<IActionResult> StudentInfo(string? sortOrder, string? q, int? page)
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

        //=================================================================STAFFS FEEDBACK==================================================
        [HttpGet]

        public async Task<IActionResult> StaffFeedback(string? sortOrder, string? q, int? page)
        {
            List<MarkOfPaintInComp> entity = await _markOfPaintInCompRepository.GetAllAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _markOfPaintInCompRepository.GetAllAsync().GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdateStaffFeedbackAsync(int id)
        {
            MarkOfPaintInComp x = await _markOfPaintInCompRepository.GetByIdAsync(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]")]
        public ActionResult AddOrUpdateStaffFeedbackAsync()
        {
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdateStaffFeedbackAsyn(MarkOfPaintInComp entity)
        {
            MarkOfPaintInComp result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _markOfPaintInCompRepository.UpdateAsync(entity);
            }
            else
            {
                result = await _markOfPaintInCompRepository.CreateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("StaffFeedback", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }


        [HttpGet("Staff/DeleteStaffFeedbackAsync/{id}")]
        public async Task<ActionResult> DeleteStaffFeedbackAsync(int id)
        {
            MarkOfPaintInComp entity = await _markOfPaintInCompRepository.GetByIdAsync(id);
            var result = await _markOfPaintInCompRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("StaffFeedback", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        //[HttpGet("[controller]/[action]/{keyword}")]
        //public async Task<List<Staff>> GetStaffsByKeyword(string keyword)
        //{
        //    var result = await _adminRepository.GetStaffByKeyword(keyword);
        //    return result;
        //}
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<MarkOfPaintInComp> GetMarkOrFeedbackById(int id)
        {
            var result = await _markOfPaintInCompRepository.GetByIdAsync(id);
            return result;
        }

        //==========================================================PaintingInEx==================================================================
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> PaintingsInEx(string? sortOrder, string? q, int? page)
        {
            List<PaintingInEx> entity = await _paintingInExRepository.GetAllPublicAsync(); ;
            int pageNumber = page ?? 1;
            int pageSize = 5;
            //if (!String.IsNullOrEmpty(q))
            //{
            //    entity = _paintingInExRepository.GetDetailPaintingInEx().GetAwaiter().GetResult();
            //    return View(entity.ToPagedList(pageNumber, pageSize));
            //}
            return View(entity);
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdatePaintingsInEx(int id)
        {
            TempData["idofEx"] = id;
            ViewData["paintingName"] = new SelectList(await _paintingRepository.GetAllPublicAsync(), "Id", "Name");
            return View();
        }
        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdatePaintingsInEx()
        {
            ViewData["paintingName"] = new SelectList(await _paintingRepository.GetAllPublicAsync(), "Id", "Name");
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdatePaintingsInExAsyn(DetailPaintingInEx entity)
        {
            ViewData["paintingName"] = new SelectList(await _paintingRepository.GetAllPublicAsync(), "Id", "Name", entity.PaintingName);
            entity.ExhibitionName = Convert.ToInt32(TempData["idofEx"]);
            DetailPaintingInEx result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _paintingInExRepository.UpdateDetailPaintingInEx(entity);
            }
            else
            {
                result = await _paintingInExRepository.CreateDetailPaintingInEx(entity);
            }
            if (result != null)
            {
                return RedirectToAction("PaintingsInEx", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("Staff/DeletePaintingsInExAsync/{id}")]
        public async Task<ActionResult> DeletePaintingsInExAsync(int id)
        {
            PaintingInEx entity = await _paintingInExRepository.GetByIdAsync(id);
            var result = await _paintingInExRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("PaintingsInEx", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        //[HttpGet("[controller]/[action]/{keyword}")]
        //public async Task<List<Staff>> GetStaffsByKeyword(string keyword)
        //{
        //    var result = await _adminRepository.GetStaffByKeyword(keyword);
        //    return result;
        //}
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<PaintingInEx> GetMarkPaintingInExkById(int id)
        {
            var result = await _paintingInExRepository.GetByIdAsync(id);
            return result;
        }

        //=================================================================PaintingInComp========================================================
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> PaintingInComp(string? sortOrder, string? q, int? page)
        {
            List<PaintingInComp> entity = await _paintingInCompRepository.GetAllPublicAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            //if (!String.IsNullOrEmpty(q))
            //{
            //    entity = _paintingInExRepository.GetDetailPaintingInEx().GetAwaiter().GetResult();
            //    return View(entity.ToPagedList(pageNumber, pageSize));
            //}
            return View(entity);
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdatePaintingInCompAsync(int id)
        {
            ViewData["paintingName"] = new SelectList(await _paintingRepository.GetAllPublicAsync(), "Id", "Name");
            ViewData["competitionName"] = new SelectList(await _competitionRepository.GetAllAsync(), "Id", "Name");
            DetailPaintingInComp x = _paintingInCompRepository.GetDetailPaintingInCompById(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdatePaintingInCompAsync()
        {
            ViewData["paintingName"] = new SelectList(await _paintingRepository.GetAllPublicAsync(), "Id", "Name");
            ViewData["competitionName"] = new SelectList(await _competitionRepository.GetAllAsync(), "Id", "Name");
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdatePaintingInCompAsyn(DetailPaintingInComp entity)
        {

            var competition = await _competitionRepository.GetByIdAsync(entity.CompetitionName);
            var paintingInComp = await _paintingInCompRepository.GetByIdAsync(entity.PaintingName);
            if (paintingInComp.DateOfPost > competition.EndOfDate)
            {
                ViewData["ErrorMessage"] = "Error: Date of post is greater than end date of competition.";
                return View(entity);
            }
            ViewData["paintingName"] = new SelectList(await _paintingRepository.GetAllPublicAsync(), "Id", "Name", entity.PaintingName);
            ViewData["competitionName"] = new SelectList(await _competitionRepository.GetAllAsync(), "Id", "Name", entity.CompetitionName);
            DetailPaintingInComp result;
            var findID = entity.Id;
            if (entity.Id != 0)
            {
                result = await _paintingInCompRepository.UpdateDetailPaintingInComp(entity);
            }
            else
            {
                result = await _paintingInCompRepository.CreateDetailPaintingInComp(entity);
            }
            if (result != null)
            {
                return RedirectToAction("PaintingInComp", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        [HttpDelete("Staff/DeletePaintingInCompAsync/{id}")]
        public async Task<ActionResult> DeletePaintingInCompAsync(int id)
        {
            var result = await _paintingInCompRepository.DeleteDetailPaintingInCompAsync(id);
            if (result != null)
            {
                return RedirectToAction("PaintingInComp", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> MarkPainInComp(int id)
        {
            List<string> mark = new List<string>();
            // Tạo một mảng chứa các phần tử cần thêm vào danh sách
            string[] mangPhanTu = { "Best", "Bestter", "Good", "Moderate", "Normal" };

            // Thêm các phần tử từ mảng vào danh sách
            mark.AddRange(mangPhanTu);
            ViewBag.MarkOfStuInSub = mark;
            TempData["idOfPain"] = id;
            var x = new MarkOfPaintInComp();
            x= await _markOfPaintInCompRepository.GetMarkOfPaintInComp(id);
            return View(x);
        }

        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> DoMarkPainInComp(MarkOfPaintInComp entity)
        {
            MarkOfPaintInComp result;
            var User1 = User.Identities.FirstOrDefault().Name;
            entity.TeacherId = _staffRepository.GetCurrentStaff(User1).GetAwaiter().GetResult().Id;
            if (entity.Id == 0)
            {
                result = await _markOfPaintInCompRepository.CreateAsync(entity);
            }
            else
            {
                result = await _markOfPaintInCompRepository.UpdateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("PaintingInComp", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        //====================================================================Exhibition==============================================================
        [HttpGet]
        public async Task<IActionResult> Exhibition(string? sortOrder, string? q, int? page)
        {
            List<Exhibition> entity = await _exhibitionRepository.GetAllAsync();
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
                return RedirectToAction("Exhibition", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("Staff/DeleteExhibitionAsync/{id}")]
        public async Task<ActionResult> DeleteExhibitionAsync(int id)
        {
            Exhibition entity = await _exhibitionRepository.GetByIdAsync(id);
            var result = await _exhibitionRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("PaintingsInEx", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        //[HttpGet("[controller]/[action]/{keyword}")]
        //public async Task<List<Staff>> GetStaffsByKeyword(string keyword)
        //{
        //    var result = await _adminRepository.GetStaffByKeyword(keyword);
        //    return result;
        //}
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<Exhibition> GeExhibitionById(int id)
        {
            var result = await _exhibitionRepository.GetByIdAsync(id);
            return result;
        }

        //=========================================================Competition============================================================
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Competition(string? sortOrder, string? q, int? page)
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

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdateCompetitionAsync(int id)
        {
            Competition x = await _competitionRepository.GetByIdAsync(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]")]
        public ActionResult AddOrUpdateCompetitionAsync()
        {
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdateCompetitionAsyn(Competition entity)
        {
            Competition result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _competitionRepository.UpdateAsync(entity);
            }
            else
            {
                result = await _competitionRepository.CreateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("Competition", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("Staff/DeleteCompetitionAsync/{id}")]
        public async Task<ActionResult> DeleteCompetitionAsync(int id)
        {
            Competition entity = await _competitionRepository.GetByIdAsync(id);
            var result = await _competitionRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("Competition", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        //[HttpGet("[controller]/[action]/{keyword}")]
        //public async Task<List<Staff>> GetStaffsByKeyword(string keyword)
        //{
        //    var result = await _adminRepository.GetStaffByKeyword(keyword);
        //    return result;
        //}
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<Competition> GeCompetitionById(int id)
        {
            var result = await _competitionRepository.GetByIdAsync(id);
            return result;
        }
        //=======================================================================Award=============================================================

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


        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdateAwardAsync(int id)
        {
            Award x = await _awardRepository.GetByIdAsync(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]")]
        public ActionResult AddOrUpdateAwardAsync()
        {
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdateAwardAsyn(Award entity)
        {
            Award result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _awardRepository.UpdateAsync(entity);
            }
            else
            {
                result = await _awardRepository.CreateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("Award", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("Staff/DeleteAwardAsync/{id}")]
        public async Task<ActionResult> DeleteAwardAsync(int id)
        {
            Award entity = await _awardRepository.GetByIdAsync(id);
            var result = await _awardRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("Award", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        //[HttpGet("[controller]/[action]/{keyword}")]
        //public async Task<List<Staff>> GetStaffsByKeyword(string keyword)
        //{
        //    var result = await _adminRepository.GetStaffByKeyword(keyword);
        //    return result;
        //}
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<Award> GetAwardById(int id)
        {
            var result = await _awardRepository.GetByIdAsync(id);
            return result;
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


        [HttpPost("Staff/AddOrUpdateSubInClassAsync")]
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
                return RedirectToAction("Classes", "Staff");
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
                return RedirectToAction("Class", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }


        //========================================================Class==========================================================

        [HttpGet]

        public async Task<IActionResult> Class(string? sortOrder, string? q, int? page)
        {
            List<Class> entity = await _classRepository.GetAllAsync();
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (!String.IsNullOrEmpty(q))
            {
                entity = _classRepository.GetAllAsync().GetAwaiter().GetResult();
                return View(entity.ToPagedList(pageNumber, pageSize));
            }
            return View(entity.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdateClassAsync(int id)
        {
            Class x = await _classRepository.GetByIdAsync(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]")]
        public ActionResult AddOrUpdateClassAsync()
        {
            return View();
        }


        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> AddOrUpdateClassAsyn(Class entity)
        {
            Class result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _classRepository.UpdateAsync(entity);
            }
            else
            {
                result = await _classRepository.CreateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("Class", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }


        [HttpGet("Staff/DeleteStaffFeedbackAsync/{id}")]
        public async Task<ActionResult> DeleteClassAsync(int id)
        {
            Class entity = await _classRepository.GetByIdAsync(id);
            var result = await _classRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("Class", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }


        //===================================================StudentInSub=====================================================
        [HttpGet]
        [Route("[controller]/[action]/{Id}")]
        public async Task<ActionResult> StudentInSub(string? sortOrder, string? q, int? page, int Id)
        {
            // Lưu dữ liệu vào TempData
            TempData["StudentInSub"] = Id;
            List<SubInClass> x = _subInClassRepository.GetAllAsync().GetAwaiter().GetResult();
            //if (!String.IsNullOrEmpty(q))
            //{
            //    entity = _paintingInExRepository.GetDetailPaintingInEx().GetAwaiter().GetResult();
            //    return View(entity.ToPagedList(pageNumber, pageSize));
            //}
            return View();
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddOrUpdateStudentInSub(int id)
        {
            // Lưu dữ liệu vào TempData
            TempData["AddStudentToSub"] = id;
            List<Student> x = await _studentRepository.GetStudentToClass(id);
            return View(x);
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> AddStudentToClass(int id)
        {
            int SubId = Convert.ToInt32(TempData["AddStudentToSub"]);
            int StuInSubId = Convert.ToInt32(TempData["StudentInSub"]);
            var entity = new StudentInSub();
            entity.SubInClassId = SubId;
            entity.StudentId = id;
            var result =  await _studentInSubRepository.CreateStudentInSub(entity);
            if (result != null)
            {
                return RedirectToAction("StudentInSub", new { id = StuInSubId });
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("Staff/DeleteStudentInSubAsync/{id}")]
        public async Task<ActionResult> DeleteStudentInSubAsync(int id)
        {
            StudentInSub entity = await _studentInSubRepository.GetByIdAsync(id);
            var result = await _studentInSubRepository.DeleteAsync(entity);
            if (result != null)
            {
                return RedirectToAction("Class","Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult> MarkStudentInSub(int id)
        {
            List<string> markS = new List<string>();
            // Tạo một mảng chứa các phần tử cần thêm vào danh sách
            string[] mangPhanTu = { "Best", "Bestter", "Good", "Moderate", "Normal" };

            // Thêm các phần tử từ mảng vào danh sách
            markS.AddRange(mangPhanTu);
            ViewBag.MarkOfStuInSubS = markS;
            var x = await _studentInSubRepository.GetByIdAsync(id);
            return PartialView(x);
        }
        [HttpPost("[controller]/[action]")]
        public async Task<ActionResult> DoMarkStudent(StudentInSub entity)
        {
            StudentInSub result;
            var findID = entity.Id;
            if (findID != null)
            {
                result = await _studentInSubRepository.UpdateAsync(entity);
            }
            else
            {
                result = await _studentInSubRepository.CreateAsync(entity);
            }
            if (result != null)
            {
                return RedirectToAction("Class", "Staff");
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        //=================================================================DetailPainting========================================================
        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DetailOfPainting(string? sortOrder, string? q, int? page,int id)
        {
            var x = new Painting();
            x = await _paintingRepository.GetByIdAsync(id);
            return View(x);
        }

    }
}


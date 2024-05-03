using eProject3.Models;
using eProject3.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eProject3.Controllers
{
    //[Route("api/[controller]")]
    public class APIBaseController<T> : Controller where T : Base
    {
        private readonly IBaseRepository<T> _repository;
        public APIBaseController(IBaseRepository<T> repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAllAsync();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Error!");
            }
        }
        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create(T entity)
        {
            var result = await _repository.CreateAsync(entity);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpPut]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Update(T entity)
        {
            var result = await _repository.UpdateAsync(entity);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Error!");
            }
        }

        [HttpDelete]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Delete(T entity)
        {
            var result = await _repository.DeleteAsync(entity);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Error!");
            }
        }
    }
}

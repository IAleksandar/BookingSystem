namespace BookingSystem.Controllers
{
    using BookingSystem.Dtos;
    using BookingSystem.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private IManagerService _managerService;

        public SearchController(IManagerService managerService)
        {
            this._managerService = managerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] SearchRequestDto searchRequestDto)
        {
            try
            {
                var result = await _managerService.Search(searchRequestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}

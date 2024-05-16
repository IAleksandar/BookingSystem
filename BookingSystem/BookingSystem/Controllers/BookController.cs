namespace BookingSystem.Controllers
{
    using BookingSystem.Dtos;
    using BookingSystem.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IManagerService _managerService;

        public BookController(IManagerService managerService)
        {
            this._managerService = managerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookRequestDto bookRequestDto)
        {
            try
            {
                var claims = User.Claims;
                string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                string username = User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                string userFullName = User.Claims.First(x => x.Type == "userFullName").Value;

                if(username != "superAdmin")
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                var result = await _managerService.Book(bookRequestDto);
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

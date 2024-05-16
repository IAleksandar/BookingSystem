namespace BookingSystem.Controllers
{
    using BookingSystem.Dtos;
    using BookingSystem.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class CheckStatusController : ControllerBase
    {
        private IManagerService _managerService;

        public CheckStatusController(IManagerService managerService)
        {
            this._managerService = managerService;
        }

        [HttpPost]
        public async Task<IActionResult> CheckStatus([FromBody] CheckStatusRequestDto checkStatusRequestDto)
        {
            try
            {
                var result = _managerService.CheckStatus(checkStatusRequestDto);
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

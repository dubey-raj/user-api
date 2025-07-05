using UserService.Model;
using UserService.Services;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("inspector")]
    public class InspectorController : ControllerBase
    {
        private readonly ILogger<InspectorController> _logger;
        private readonly IUserService _userService;

        public InspectorController(ILogger<InspectorController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        //[HttpGet]
        //[Route("availability")]
        //public async Task<IActionResult> GetInspectorAvailability(string region, int limit)
        //{
        //    var inspectors = await _userService.GetAvailableInspector(region, limit);
        //    return Ok(inspectors);
        //}

        //[HttpPatch("update-assigned-count")]
        //public async Task<IActionResult> UpdateAssignedClaimCount([FromBody] UpdateAssignedCaseCountRequest request)
        //{
        //    var res = await _userService.UpdateUserAssignmentAsync(request);
        //    return Ok(res.IsSuccess);
        //}
    }
}

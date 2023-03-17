using Application.Contracts;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly AnnouncementsService _announcementsService;

    public AnnouncementsController(AnnouncementsService announcementsService)
    {
        _announcementsService = announcementsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAsync() => Ok(await _announcementsService.GetAnnouncementsAsync());

    [HttpPost]
    public async Task<ActionResult<AnnouncementDto>> CreateAsync([FromBody] AnnouncementBaseDto data) => Ok(await _announcementsService.CreateAnnouncementAsync(data));
}
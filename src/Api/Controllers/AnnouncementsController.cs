using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    [HttpGet]
    public IEnumerable<AnnouncementDto> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new AnnouncementDto
        {
            Date = DateTime.Now.AddDays(index),
            Title = $"Toto je title {index}",
            Text = $"Toto je text {index}",
        })
        .ToArray();
    }
}

using Application.Contracts;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Services;

public class AnnouncementsService
{
    private readonly UnitOfWork _unitOfWork;

    public AnnouncementsService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AnnouncementDto>> GetAnnouncementsAsync()
    {
        var entities = await _unitOfWork.AnnouncementsRepository.GetAsync();
        return entities.Select(x => new AnnouncementDto
        {
            Id = x.Id,
            Title = x.Title,
            Text = x.Text,
        });
    }

    public async Task<AnnouncementDto> CreateAnnouncementAsync(AnnouncementBaseDto data)
    {
        var entity = await _unitOfWork.AnnouncementsRepository.CreateAsync(new Announcement
        {
            Title = data.Title,
            Text = data.Text,
        });

        await _unitOfWork.SaveChangesAsync();
        
        var addedEntity = await _unitOfWork.AnnouncementsRepository.GetByIdAsync(entity.Id);
        return new AnnouncementDto
        {
            Id = addedEntity.Id,
            Title = addedEntity.Title,
            Text = addedEntity.Text,
        };
    }
}
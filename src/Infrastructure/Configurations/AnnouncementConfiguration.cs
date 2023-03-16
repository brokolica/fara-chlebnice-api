using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.ToTable("announcements");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseMySqlIdentityColumn();
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Text).IsRequired();
    }
}
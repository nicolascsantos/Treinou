using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinou.Domain.Entities;

namespace Treinou.Infraestructure.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> teachersConfiguration)
        {
            teachersConfiguration
                .HasKey(x => x.Id);

            teachersConfiguration
                .HasMany(x => x.Students)
                .WithOne(x => x.Teacher)
                .HasForeignKey(x => x.TeacherId);   

            teachersConfiguration.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            teachersConfiguration.Property(x => x.Description)
                .HasMaxLength(200)
                .IsRequired();

            teachersConfiguration
                .OwnsOne(x => x.Email)
                .Property(x => x.Address)
                .HasColumnName("Email");

            teachersConfiguration
                .OwnsOne(x => x.CPF)
                .Property(x => x.Number)
                .HasColumnName("CPF");

            teachersConfiguration
                .OwnsOne(x => x.PhoneNumber)
                .Property(x => x.Number)
                .HasColumnName("PhoneNumber");

            teachersConfiguration
                .OwnsOne(x => x.CREF)
                .Property(x => x.Number)
                .HasColumnName("CREF");
        }
    }
}

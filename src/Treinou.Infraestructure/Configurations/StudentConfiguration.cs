using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Treinou.Domain.Entities;

namespace Treinou.Infraestructure.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> studentConfiguration)
        {
            studentConfiguration.HasKey(x => x.Id);
            studentConfiguration.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            studentConfiguration
                .HasOne(x => x.Teacher)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.TeacherId);

            studentConfiguration
                .OwnsOne(p => p.Email)
                .Property(x => x.Address)
                .HasColumnName("Email")
                .IsRequired();

            studentConfiguration
                .OwnsOne(p => p.CPF)
                .Property(x => x.Number)
                .HasColumnName("CPF")
                .IsRequired();

            studentConfiguration
                .OwnsOne(p => p.PhoneNumber)
                .Property(x => x.Number)
                .HasColumnName("PhoneNumber")
                .IsRequired();

            studentConfiguration.Property(x => x.Weight)
                .IsRequired();

            studentConfiguration.Property(x => x.Height)
                .IsRequired();


        }
    }
}

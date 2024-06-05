using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EliteSoftTask.Data.Database.Entities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        User defaultUser = new()
        {
            Id = 1,
            Username = "user",
            Password = BCrypt.Net.BCrypt.HashPassword("password"),
            AuthSource = User.AuthenticationSource.Db
        };
        builder.HasData([defaultUser]);
    }
}
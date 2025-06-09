namespace WebAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }   // UUID u PostgreSQL mapira se na Guid u C#
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}



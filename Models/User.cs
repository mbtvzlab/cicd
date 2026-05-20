using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiCd.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = "";

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = "";

    [Required]
    public string PasswordHash { get; set; } = "";

    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public UserRole Role { get; set; }

    public virtual ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();
}

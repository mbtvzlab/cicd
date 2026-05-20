using CiCd.Models;
using Microsoft.EntityFrameworkCore;

namespace CiCd.Data;

public class EfUserRepository : IUserRepository
{
    private readonly CiCdDbContext _context;

    public EfUserRepository(CiCdDbContext context)
    {
        _context = context;
    }

    public List<User> GetAll()
    {
        return _context.Users
            .Include(u => u.ProjectMemberships)
                .ThenInclude(m => m.Project)
            .Where(u => u.DeletedAt == null)
            .ToList();
    }

    public User? GetById(int id)
    {
        return _context.Users
            .Include(u => u.ProjectMemberships)
                .ThenInclude(m => m.Project)
            .FirstOrDefault(u => u.Id == id && u.DeletedAt == null);
    }

    public User? GetByUsername(string username)
    {
        return _context.Users
            .Include(u => u.ProjectMemberships)
                .ThenInclude(m => m.Project)
            .FirstOrDefault(u => u.Username == username);
    }

    public User Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            user.DeletedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }

    public List<User> Search(string query)
    {
        return _context.Users
            .Include(u => u.ProjectMemberships)
                .ThenInclude(m => m.Project)
            .Where(u => u.DeletedAt == null &&
                (u.Username.Contains(query) || u.Email.Contains(query)))
            .ToList();
    }
}
using CiCd.Models;
using Microsoft.EntityFrameworkCore;

namespace CiCd.Data;

public class EfUserRepository : IUserRepository
{
    private readonly CiCdDbContext _context;
    private readonly ILogger<EfUserRepository> _logger;

    public EfUserRepository(CiCdDbContext context, ILogger<EfUserRepository> logger)
    {
        _context = context;
        _logger = logger;
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
        try
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user '{Username}'", user.Username);
            throw;
        }
    }

    public void Update(User user)
    {
        try
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user id={Id}", user.Id);
            throw;
        }
    }

    public void Delete(int id)
    {
        try
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.DeletedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete user id={Id}", id);
            throw;
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
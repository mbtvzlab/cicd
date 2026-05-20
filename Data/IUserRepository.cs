using CiCd.Models;

namespace CiCd.Data;

public interface IUserRepository
{
    List<User> GetAll();
    User? GetById(int id);
    User? GetByUsername(string username);
    User Add(User user);
    void Update(User user);
    void Delete(int id);
    List<User> Search(string query);
}
using CiCd.Models;

namespace CiCd.Data;

public interface IProjectRepository
{
    List<Project> GetAll();
    Project? GetById(int id);
    Project Add(Project project);
    void Update(Project project);
    void Delete(int id);
    List<Project> Search(string query);
}
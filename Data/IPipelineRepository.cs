using CiCd.Models;

namespace CiCd.Data;

public interface IPipelineRepository
{
    List<Pipeline> GetAll();
    Pipeline? GetById(int id);
    List<Pipeline> GetByProjectId(int projectId);
    Pipeline Add(Pipeline pipeline);
    void Update(Pipeline pipeline);
    void Delete(int id);
}
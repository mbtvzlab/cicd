using CiCd.Models;

namespace CiCd.Data;

public interface IRunLogRepository
{
    List<RunLog> GetAll();
    RunLog? GetById(int id);
    List<RunLog> GetByPipelineId(int pipelineId);
    RunLog Add(RunLog runLog);
    void Update(RunLog runLog);
    void Delete(int id);
}
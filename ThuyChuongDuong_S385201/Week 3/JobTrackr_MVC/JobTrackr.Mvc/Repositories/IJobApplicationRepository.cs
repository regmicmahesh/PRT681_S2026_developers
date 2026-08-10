using JobTrackr.Mvc.Models;

namespace JobTrackr.Mvc.Repositories;

public interface IJobApplicationRepository
{
    void Add(JobApplication jobApplication);

    JobApplication? GetById(Guid id);

    IReadOnlyList<JobApplication> GetAll();

    bool Update(JobApplication jobApplication);

    bool Delete(Guid id);
}

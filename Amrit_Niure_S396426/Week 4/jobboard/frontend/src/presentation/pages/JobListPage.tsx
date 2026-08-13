import { Link } from 'react-router-dom';
import { useJobs } from '../../application/jobs/useJobs';
import { JobCard } from '../components/JobCard';

export function JobListPage() {
  const { data: jobs, isPending, isError, error } = useJobs();

  return (
    <section>
      <header className="page-header">
        <h1>Job openings</h1>
        <Link to="/jobs/new">Post a job</Link>
      </header>

      {isPending && <p>Loading jobs...</p>}
      {isError && <p role="alert">Failed to load jobs: {error.message}</p>}
      {jobs && jobs.length === 0 && <p>No jobs posted yet.</p>}

      {jobs && jobs.length > 0 && (
        <ul className="job-list">
          {jobs.map((job) => (
            <JobCard key={job.id} job={job} />
          ))}
        </ul>
      )}
    </section>
  );
}

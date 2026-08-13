import { useParams } from 'react-router-dom';
import { useJob } from '../../application/jobs/useJob';
import { usePublishJob } from '../../application/jobs/usePublishJob';
import { useCloseJob } from '../../application/jobs/useCloseJob';
import { ApplyForm } from '../components/ApplyForm';

export function JobDetailPage() {
  const { id } = useParams<{ id: string }>();
  const { data: job, isPending, isError, error } = useJob(id);
  const publishJob = usePublishJob();
  const closeJob = useCloseJob();

  if (isPending) return <p>Loading job...</p>;
  if (isError) return <p role="alert">Failed to load job: {error.message}</p>;
  if (!job) return <p>Job not found.</p>;

  return (
    <article>
      <header className="page-header">
        <div>
          <h1>{job.title}</h1>
          <p className="job-card__meta">
            {job.employmentType} &middot; {job.salaryMin.toLocaleString()}-{job.salaryMax.toLocaleString()}{' '}
            {job.salaryCurrency} / {job.payPeriod} &middot;{' '}
            <span className={`status status--${job.status.toLowerCase()}`}>{job.status}</span> &middot;{' '}
            {job.applicationCount} application{job.applicationCount === 1 ? '' : 's'}
          </p>
        </div>

        {job.status === 'Draft' && (
          <button
            className="primary-button"
            onClick={() => publishJob.mutate(job.id)}
            disabled={publishJob.isPending}
          >
            {publishJob.isPending ? 'Publishing...' : 'Publish'}
          </button>
        )}

        {job.status === 'Published' && (
          <button
            className="secondary-button"
            onClick={() => closeJob.mutate(job.id)}
            disabled={closeJob.isPending}
          >
            {closeJob.isPending ? 'Closing...' : 'Close job'}
          </button>
        )}
      </header>

      <p>{job.description}</p>

      {job.status === 'Published' && <ApplyForm jobId={job.id} />}
    </article>
  );
}

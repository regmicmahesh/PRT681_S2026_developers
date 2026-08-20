import { Link } from 'react-router-dom';
import type { Job } from '../../domain/job';

interface JobCardProps {
  job: Job;
}

export function JobCard({ job }: JobCardProps) {
  return (
    <li className="job-card">
      <Link to={`/jobs/${job.id}`}>
        <h3>{job.title}</h3>
      </Link>
      <p className="job-card__meta">
        {job.employmentType} &middot; {job.salaryMin.toLocaleString()}-{job.salaryMax.toLocaleString()}{' '}
        {job.salaryCurrency} / {job.payPeriod} &middot;{' '}
        <span className={`status status--${job.status.toLowerCase()}`}>{job.status}</span>
      </p>
    </li>
  );
}

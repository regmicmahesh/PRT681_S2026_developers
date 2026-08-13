import { useState, type FormEvent } from 'react';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import { useCreateJob } from '../../application/jobs/useCreateJob';
import type { EmploymentType } from '../../domain/job';

const EMPLOYMENT_TYPES: EmploymentType[] = ['FullTime', 'PartTime', 'Contract', 'Internship'];

export function NewJobPage() {
  const [searchParams] = useSearchParams();
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [employmentType, setEmploymentType] = useState<EmploymentType>('FullTime');
  const [salaryMin, setSalaryMin] = useState('');
  const [salaryMax, setSalaryMax] = useState('');
  const [salaryCurrency, setSalaryCurrency] = useState('USD');
  const [companyId, setCompanyId] = useState(searchParams.get('companyId') ?? '');
  const createJob = useCreateJob();
  const navigate = useNavigate();

  function handleSubmit(event: FormEvent) {
    event.preventDefault();
    createJob.mutate(
      {
        title,
        description,
        employmentType,
        salaryMin: Number(salaryMin),
        salaryMax: Number(salaryMax),
        salaryCurrency,
        companyId,
      },
      {
        onSuccess: (jobId) => navigate(`/jobs/${jobId}`),
      },
    );
  }

  return (
    <section>
      <h1>Post a job</h1>

      <form onSubmit={handleSubmit}>
        <div className="field">
          <label htmlFor="companyId">Company id</label>
          <input id="companyId" value={companyId} onChange={(e) => setCompanyId(e.target.value)} required />
          <small>
            Don't have a company yet? <Link to="/companies/new">Register one</Link>.
          </small>
        </div>

        <div className="field">
          <label htmlFor="title">Title</label>
          <input id="title" value={title} onChange={(e) => setTitle(e.target.value)} required />
        </div>

        <div className="field">
          <label htmlFor="description">Description</label>
          <textarea
            id="description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            rows={5}
            required
          />
        </div>

        <div className="field">
          <label htmlFor="employmentType">Employment type</label>
          <select
            id="employmentType"
            value={employmentType}
            onChange={(e) => setEmploymentType(e.target.value as EmploymentType)}
          >
            {EMPLOYMENT_TYPES.map((type) => (
              <option key={type} value={type}>
                {type}
              </option>
            ))}
          </select>
        </div>

        <div className="field">
          <label htmlFor="salaryMin">Minimum salary</label>
          <input
            id="salaryMin"
            type="number"
            min="0"
            value={salaryMin}
            onChange={(e) => setSalaryMin(e.target.value)}
            required
          />
        </div>

        <div className="field">
          <label htmlFor="salaryMax">Maximum salary</label>
          <input
            id="salaryMax"
            type="number"
            min="0"
            value={salaryMax}
            onChange={(e) => setSalaryMax(e.target.value)}
            required
          />
        </div>

        <div className="field">
          <label htmlFor="salaryCurrency">Currency</label>
          <input
            id="salaryCurrency"
            value={salaryCurrency}
            onChange={(e) => setSalaryCurrency(e.target.value)}
            required
          />
        </div>

        {createJob.isError && <p className="error-text">{createJob.error.message}</p>}

        <button type="submit" className="primary-button" disabled={createJob.isPending}>
          {createJob.isPending ? 'Posting...' : 'Post job'}
        </button>
      </form>
    </section>
  );
}

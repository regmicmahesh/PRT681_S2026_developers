import { useState, type FormEvent } from 'react';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import { useCreateJob } from '../../application/jobs/useCreateJob';
import { useCompanies } from '../../application/companies/useCompanies';
import type { EmploymentType, PayPeriod } from '../../domain/job';

const EMPLOYMENT_TYPES: EmploymentType[] = ['FullTime', 'PartTime', 'Contract', 'Internship'];
const PAY_PERIODS: PayPeriod[] = ['Hourly', 'Daily', 'Weekly', 'Monthly', 'Annually'];

export function NewJobPage() {
  const [searchParams] = useSearchParams();
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [employmentType, setEmploymentType] = useState<EmploymentType>('FullTime');
  const [salaryMin, setSalaryMin] = useState('');
  const [salaryMax, setSalaryMax] = useState('');
  const [salaryCurrency, setSalaryCurrency] = useState('USD');
  const [payPeriod, setPayPeriod] = useState<PayPeriod>('Annually');
  const [companyId, setCompanyId] = useState(searchParams.get('companyId') ?? '');
  const createJob = useCreateJob();
  const companies = useCompanies();
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
        payPeriod,
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
          <label htmlFor="companyId">Company</label>
          <select
            id="companyId"
            value={companyId}
            onChange={(e) => setCompanyId(e.target.value)}
            required
            disabled={companies.isLoading}
          >
            <option value="" disabled>
              {companies.isLoading ? 'Loading companies...' : 'Select a company'}
            </option>
            {companies.data?.map((company) => (
              <option key={company.id} value={company.id}>
                {company.name}
              </option>
            ))}
          </select>
          {companies.isError && <p className="error-text">{companies.error.message}</p>}
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
            onChange={(e) => setSalaryCurrency(e.target.value.toUpperCase())}
            maxLength={3}
            pattern="[A-Za-z]{3}"
            title="3-letter ISO 4217 code, e.g. USD"
            required
          />
        </div>

        <div className="field">
          <label htmlFor="payPeriod">Pay period</label>
          <select id="payPeriod" value={payPeriod} onChange={(e) => setPayPeriod(e.target.value as PayPeriod)}>
            {PAY_PERIODS.map((period) => (
              <option key={period} value={period}>
                {period}
              </option>
            ))}
          </select>
        </div>

        {createJob.isError && <p className="error-text">{createJob.error.message}</p>}

        <button type="submit" className="primary-button" disabled={createJob.isPending}>
          {createJob.isPending ? 'Posting...' : 'Post job'}
        </button>
      </form>
    </section>
  );
}

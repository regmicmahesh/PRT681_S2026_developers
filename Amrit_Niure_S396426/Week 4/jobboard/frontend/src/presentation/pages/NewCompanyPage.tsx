import { useState, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCreateCompany } from '../../application/companies/useCreateCompany';

export function NewCompanyPage() {
  const [name, setName] = useState('');
  const [contactEmail, setContactEmail] = useState('');
  const createCompany = useCreateCompany();
  const navigate = useNavigate();

  function handleSubmit(event: FormEvent) {
    event.preventDefault();
    createCompany.mutate(
      { name, contactEmail },
      {
        onSuccess: (companyId) => navigate(`/jobs/new?companyId=${companyId}`),
      },
    );
  }

  return (
    <section>
      <h1>Register a company</h1>

      <form onSubmit={handleSubmit}>
        <div className="field">
          <label htmlFor="name">Company name</label>
          <input id="name" value={name} onChange={(e) => setName(e.target.value)} required />
        </div>

        <div className="field">
          <label htmlFor="contactEmail">Contact email</label>
          <input
            id="contactEmail"
            type="email"
            value={contactEmail}
            onChange={(e) => setContactEmail(e.target.value)}
            required
          />
        </div>

        {createCompany.isError && <p className="error-text">{createCompany.error.message}</p>}

        <button type="submit" className="primary-button" disabled={createCompany.isPending}>
          {createCompany.isPending ? 'Registering...' : 'Register company'}
        </button>
      </form>
    </section>
  );
}

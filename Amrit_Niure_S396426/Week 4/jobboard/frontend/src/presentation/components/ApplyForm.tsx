import { useState, type FormEvent } from 'react';
import { useApplyToJob } from '../../application/jobs/useApplyToJob';

interface ApplyFormProps {
  jobId: string;
}

export function ApplyForm({ jobId }: ApplyFormProps) {
  const [candidateName, setCandidateName] = useState('');
  const [candidateEmail, setCandidateEmail] = useState('');
  const [resumeUrl, setResumeUrl] = useState('');
  const applyToJob = useApplyToJob(jobId);

  function handleSubmit(event: FormEvent) {
    event.preventDefault();
    applyToJob.mutate(
      { candidateName, candidateEmail, resumeUrl },
      {
        onSuccess: () => {
          setCandidateName('');
          setCandidateEmail('');
          setResumeUrl('');
        },
      },
    );
  }

  if (applyToJob.isSuccess) {
    return <p>Application submitted. Good luck!</p>;
  }

  return (
    <form onSubmit={handleSubmit}>
      <h3>Apply for this job</h3>

      <div className="field">
        <label htmlFor="candidateName">Full name</label>
        <input
          id="candidateName"
          value={candidateName}
          onChange={(e) => setCandidateName(e.target.value)}
          required
        />
      </div>

      <div className="field">
        <label htmlFor="candidateEmail">Email</label>
        <input
          id="candidateEmail"
          type="email"
          value={candidateEmail}
          onChange={(e) => setCandidateEmail(e.target.value)}
          required
        />
      </div>

      <div className="field">
        <label htmlFor="resumeUrl">Resume URL</label>
        <input
          id="resumeUrl"
          type="url"
          value={resumeUrl}
          onChange={(e) => setResumeUrl(e.target.value)}
          required
        />
      </div>

      {applyToJob.isError && <p className="error-text">{applyToJob.error.message}</p>}

      <button type="submit" className="primary-button" disabled={applyToJob.isPending}>
        {applyToJob.isPending ? 'Submitting...' : 'Submit application'}
      </button>
    </form>
  );
}

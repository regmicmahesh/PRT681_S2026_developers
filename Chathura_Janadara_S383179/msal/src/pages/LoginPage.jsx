import { useState } from 'react';
import { useMsal, useIsAuthenticated } from '@azure/msal-react';
import { Navigate } from 'react-router-dom';
import { loginRequest } from '../config/authConfig';

const styles = {
  page: {
    display: 'flex',
    minHeight: '100vh',
    alignItems: 'center',
    justifyContent: 'center',
    padding: '1rem',
    background: '#f5f6f8',
    fontFamily: 'system-ui, -apple-system, "Segoe UI", sans-serif',
  },
  card: {
    width: '100%',
    maxWidth: '360px',
    padding: '2rem',
    background: '#fff',
    border: '1px solid #e2e5ea',
    borderRadius: '10px',
    boxShadow: '0 1px 3px rgba(0,0,0,0.06)',
  },
  title: {
    margin: 0,
    fontSize: '1.25rem',
    fontWeight: 600,
    color: '#1a1d23',
  },
  subtitle: {
    margin: '0.35rem 0 0',
    fontSize: '0.875rem',
    color: '#6b7280',
  },
  button: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    gap: '0.75rem',
    width: '100%',
    marginTop: '1.5rem',
    padding: '0.7rem 1rem',
    fontSize: '0.875rem',
    fontWeight: 500,
    color: '#3c4043',
    background: '#fff',
    border: '1px solid #cbd0d8',
    borderRadius: '6px',
    cursor: 'pointer',
  },
  buttonBusy: {
    opacity: 0.6,
    cursor: 'default',
  },
  error: {
    marginTop: '1rem',
    marginBottom: 0,
    fontSize: '0.875rem',
    color: '#c0392b',
  },
};

function MicrosoftMark() {
  return (
    <svg viewBox="0 0 20 20" width="18" height="18" aria-hidden="true">
      <rect x="0" y="0" width="9" height="9" fill="#F25022" />
      <rect x="11" y="0" width="9" height="9" fill="#7FBA00" />
      <rect x="0" y="11" width="9" height="9" fill="#00A4EF" />
      <rect x="11" y="11" width="9" height="9" fill="#FFB900" />
    </svg>
  );
}

export default function LoginPage() {
  const { instance } = useMsal();
  const isAuthenticated = useIsAuthenticated();
  const [error, setError] = useState(null);
  const [busy, setBusy] = useState(false);

  if (isAuthenticated) return <Navigate to="/home" replace />;

  const handleSignIn = async () => {
    setBusy(true);
    setError(null);
    try {
      await instance.loginRedirect(loginRequest);
    } catch (err) {
      if (err.errorCode !== 'user_cancelled') {
        setError('Sign-in failed. Check your account and try again.');
      }
    } finally {
      setBusy(false);
    }
  };

  return (
    <div style={styles.page}>
      <div style={styles.card}>
        <h1 style={styles.title}>Subject Guide</h1>
        <p style={styles.subtitle}>
          Sign in with your school account to browse subjects.
        </p>

        <button
          type="button"
          onClick={handleSignIn}
          disabled={busy}
          style={busy ? { ...styles.button, ...styles.buttonBusy } : styles.button}
        >
          <MicrosoftMark />
          {busy ? 'Opening Microsoft…' : 'Sign in with Microsoft'}
        </button>

        {error && (
          <p role="alert" style={styles.error}>
            {error}
          </p>
        )}
      </div>
    </div>
  );
}
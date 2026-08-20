import { BrowserRouter, Link, Route, Routes } from 'react-router-dom';
import { JobListPage } from './presentation/pages/JobListPage';
import { JobDetailPage } from './presentation/pages/JobDetailPage';
import { NewJobPage } from './presentation/pages/NewJobPage';
import { NewCompanyPage } from './presentation/pages/NewCompanyPage';

function App() {
  return (
    <BrowserRouter>
      <nav className="app-nav">
        <Link to="/">JobBoard</Link>
      </nav>

      <Routes>
        <Route path="/" element={<JobListPage />} />
        <Route path="/jobs/new" element={<NewJobPage />} />
        <Route path="/jobs/:id" element={<JobDetailPage />} />
        <Route path="/companies/new" element={<NewCompanyPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

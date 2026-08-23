const elements = {
  form: document.querySelector('#application-form'),
  formTitle: document.querySelector('#form-title'),
  formErrorSummary: document.querySelector('#form-error-summary'),
  applicationId: document.querySelector('#application-id'),
  company: document.querySelector('#company'),
  role: document.querySelector('#role'),
  status: document.querySelector('#status'),
  applicationDate: document.querySelector('#application-date'),
  followUpDate: document.querySelector('#follow-up-date'),
  jobUrl: document.querySelector('#job-url'),
  notes: document.querySelector('#notes'),
  notesCount: document.querySelector('#notes-count'),
  saveButton: document.querySelector('#save-application'),
  cancelEdit: document.querySelector('#cancel-edit'),
  newApplication: document.querySelector('#new-application'),
  filterForm: document.querySelector('#filter-form'),
  search: document.querySelector('#search'),
  statusFilter: document.querySelector('#status-filter'),
  rows: document.querySelector('#application-rows'),
  listState: document.querySelector('#list-state'),
  tableContainer: document.querySelector('#table-container'),
  resultCount: document.querySelector('#result-count'),
  previousPage: document.querySelector('#previous-page'),
  nextPage: document.querySelector('#next-page'),
  pageDescription: document.querySelector('#page-description'),
  summaryGrid: document.querySelector('#summary-grid'),
  summaryTotal: document.querySelector('#summary-total'),
  summaryApplied: document.querySelector('#summary-applied'),
  summaryInterview: document.querySelector('#summary-interview'),
  summaryOffer: document.querySelector('#summary-offer'),
  toast: document.querySelector('#toast'),
};

const state = {
  page: 1,
  pageCount: 1,
  pageSize: 20,
  search: '',
  status: '',
};

let toastTimer;

async function api(path, options = {}) {
  const response = await fetch(path, {
    ...options,
    headers: options.body
      ? { 'content-type': 'application/json', ...options.headers }
      : options.headers,
  });

  if (response.status === 204) {
    return null;
  }

  const body = await response.json().catch(() => null);
  if (!response.ok) {
    const error = new Error(body?.error?.message ?? 'The request could not be completed.');
    error.status = response.status;
    error.details = body?.error?.details ?? {};
    throw error;
  }

  return body;
}

function showToast(message) {
  window.clearTimeout(toastTimer);
  elements.toast.textContent = message;
  elements.toast.classList.add('is-visible');
  toastTimer = window.setTimeout(() => {
    elements.toast.classList.remove('is-visible');
  }, 3200);
}

function clearErrors() {
  elements.formErrorSummary.hidden = true;
  elements.formErrorSummary.textContent = '';

  for (const field of ['company', 'role', 'status', 'applicationDate', 'followUpDate', 'jobUrl', 'notes']) {
    const input = elements[field];
    const error = document.querySelector(`#${field}-error`);
    input.removeAttribute('aria-invalid');
    input.removeAttribute('aria-describedby');
    error.textContent = '';
  }
}

function showErrors(details) {
  clearErrors();
  const fields = Object.keys(details);
  if (fields.length === 0) {
    elements.formErrorSummary.textContent = 'The application could not be saved. Try again.';
  } else {
    elements.formErrorSummary.textContent = `Check ${fields.length} field${fields.length === 1 ? '' : 's'} and try again.`;
    for (const field of fields) {
      if (!elements[field]) continue;
      const errorId = `${field}-error`;
      elements[field].setAttribute('aria-invalid', 'true');
      elements[field].setAttribute('aria-describedby', errorId);
      document.querySelector(`#${errorId}`).textContent = details[field];
    }
  }
  elements.formErrorSummary.hidden = false;
  elements.formErrorSummary.focus();
}

function updateDateRequirement() {
  const required = elements.status.value !== 'Wishlist';
  elements.applicationDate.required = required;
  elements.applicationDate.labels[0].textContent = required
    ? 'Application date *'
    : 'Application date';
}

function readForm() {
  return {
    company: elements.company.value,
    role: elements.role.value,
    status: elements.status.value,
    applicationDate: elements.applicationDate.value || null,
    followUpDate: elements.followUpDate.value || null,
    jobUrl: elements.jobUrl.value || null,
    notes: elements.notes.value || null,
  };
}

function resetForm({ focus = false } = {}) {
  elements.form.reset();
  elements.applicationId.value = '';
  elements.formTitle.textContent = 'Add an opportunity';
  elements.saveButton.textContent = 'Save application';
  elements.cancelEdit.hidden = true;
  elements.notesCount.textContent = '0';
  clearErrors();
  updateDateRequirement();
  if (focus) {
    elements.company.focus();
  }
}

function fillForm(application) {
  elements.applicationId.value = application.id;
  elements.company.value = application.company;
  elements.role.value = application.role;
  elements.status.value = application.status;
  elements.applicationDate.value = application.applicationDate ?? '';
  elements.followUpDate.value = application.followUpDate ?? '';
  elements.jobUrl.value = application.jobUrl ?? '';
  elements.notes.value = application.notes ?? '';
  elements.notesCount.textContent = String(elements.notes.value.length);
  elements.formTitle.textContent = 'Edit application';
  elements.saveButton.textContent = 'Update application';
  elements.cancelEdit.hidden = false;
  clearErrors();
  updateDateRequirement();
  document.querySelector('.form-panel').scrollIntoView({ behavior: 'smooth' });
  elements.company.focus({ preventScroll: true });
}

function cell(className = '') {
  const element = document.createElement('td');
  element.className = className;
  return element;
}

function button(label, className, handler) {
  const element = document.createElement('button');
  element.type = 'button';
  element.className = className;
  element.textContent = label;
  element.addEventListener('click', handler);
  return element;
}

function renderRows(applications) {
  const fragment = document.createDocumentFragment();

  for (const application of applications) {
    const row = document.createElement('tr');
    const opportunity = cell('opportunity');
    const company = document.createElement('strong');
    const role = document.createElement('span');
    company.textContent = application.company;
    role.textContent = application.role;
    opportunity.append(company, role);

    const status = cell();
    const statusText = document.createElement('span');
    statusText.className = 'status-badge';
    statusText.textContent = application.status;
    status.append(statusText);

    const followUp = cell('follow-up');
    followUp.textContent = application.followUpDate ?? 'Not scheduled';

    const actions = cell('row-actions');
    actions.append(
      button('Edit', 'button button-secondary', () => editApplication(application.id)),
      button('Delete', 'button button-danger', () => deleteApplication(application)),
    );

    row.append(opportunity, status, followUp, actions);
    fragment.append(row);
  }

  elements.rows.replaceChildren(fragment);
}

async function loadApplications() {
  elements.listState.hidden = false;
  elements.listState.textContent = 'Loading applications…';
  elements.tableContainer.hidden = true;
  elements.resultCount.textContent = 'Loading…';

  const query = new URLSearchParams({
    page: String(state.page),
    pageSize: String(state.pageSize),
  });
  if (state.search) query.set('search', state.search);
  if (state.status) query.set('status', state.status);

  try {
    const result = await api(`/api/applications?${query}`);
    state.page = result.page;
    state.pageCount = result.pageCount;
    elements.resultCount.textContent = `${result.total} result${result.total === 1 ? '' : 's'}`;
    elements.pageDescription.textContent = `Page ${result.page} of ${result.pageCount}`;
    elements.previousPage.disabled = result.page <= 1;
    elements.nextPage.disabled = result.page >= result.pageCount;

    if (result.items.length === 0) {
      elements.listState.textContent = state.search || state.status
        ? 'No applications match these filters. Change or clear a filter.'
        : 'No applications yet. Add your first opportunity using the form.';
      return;
    }

    renderRows(result.items);
    elements.listState.hidden = true;
    elements.tableContainer.hidden = false;
  } catch {
    elements.listState.textContent = 'Applications could not be loaded. Check that the local server is running, then retry.';
    elements.resultCount.textContent = 'Unavailable';
  }
}

async function loadSummary() {
  elements.summaryGrid.setAttribute('aria-busy', 'true');
  try {
    const summary = await api('/api/summary');
    elements.summaryTotal.textContent = summary.total;
    elements.summaryApplied.textContent = summary.byStatus.Applied;
    elements.summaryInterview.textContent = summary.byStatus.Interview;
    elements.summaryOffer.textContent = summary.byStatus.Offer;
  } catch {
    for (const item of [elements.summaryTotal, elements.summaryApplied, elements.summaryInterview, elements.summaryOffer]) {
      item.textContent = '—';
    }
  } finally {
    elements.summaryGrid.setAttribute('aria-busy', 'false');
  }
}

async function refresh() {
  await Promise.all([loadApplications(), loadSummary()]);
}

async function editApplication(id) {
  try {
    fillForm(await api(`/api/applications/${id}`));
  } catch {
    showToast('That application could not be loaded.');
    await refresh();
  }
}

async function deleteApplication(application) {
  const confirmed = window.confirm(
    `Delete ${application.role} at ${application.company}? This cannot be undone.`,
  );
  if (!confirmed) return;

  try {
    await api(`/api/applications/${application.id}`, { method: 'DELETE' });
    if (elements.applicationId.value === String(application.id)) {
      resetForm();
    }
    showToast('Application deleted.');
    await refresh();
  } catch {
    showToast('The application could not be deleted.');
  }
}

elements.form.addEventListener('submit', async (event) => {
  event.preventDefault();
  clearErrors();
  updateDateRequirement();

  if (!elements.form.checkValidity()) {
    elements.form.reportValidity();
    return;
  }

  const id = elements.applicationId.value;
  elements.saveButton.disabled = true;
  elements.saveButton.textContent = id ? 'Updating…' : 'Saving…';

  try {
    await api(id ? `/api/applications/${id}` : '/api/applications', {
      method: id ? 'PUT' : 'POST',
      body: JSON.stringify(readForm()),
    });
    resetForm();
    showToast(id ? 'Application updated.' : 'Application saved.');
    state.page = 1;
    await refresh();
  } catch (error) {
    showErrors(error.details ?? {});
  } finally {
    elements.saveButton.disabled = false;
    elements.saveButton.textContent = elements.applicationId.value
      ? 'Update application'
      : 'Save application';
  }
});

elements.filterForm.addEventListener('submit', async (event) => {
  event.preventDefault();
  state.search = elements.search.value.trim();
  state.status = elements.statusFilter.value;
  state.page = 1;
  await loadApplications();
});

elements.previousPage.addEventListener('click', async () => {
  if (state.page <= 1) return;
  state.page -= 1;
  await loadApplications();
});

elements.nextPage.addEventListener('click', async () => {
  if (state.page >= state.pageCount) return;
  state.page += 1;
  await loadApplications();
});

elements.status.addEventListener('change', updateDateRequirement);
elements.notes.addEventListener('input', () => {
  elements.notesCount.textContent = String(elements.notes.value.length);
});
elements.cancelEdit.addEventListener('click', () => resetForm({ focus: true }));
elements.newApplication.addEventListener('click', () => {
  resetForm({ focus: true });
  document.querySelector('.form-panel').scrollIntoView({ behavior: 'smooth' });
});

elements.applicationDate.max = new Date().toISOString().slice(0, 10);
updateDateRequirement();
await refresh();

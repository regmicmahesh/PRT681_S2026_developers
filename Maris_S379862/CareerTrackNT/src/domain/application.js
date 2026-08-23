export const APPLICATION_STATUSES = Object.freeze([
  'Wishlist',
  'Applied',
  'Interview',
  'Offer',
  'Rejected',
  'Withdrawn',
]);

const DATE_PATTERN = /^\d{4}-\d{2}-\d{2}$/;

function cleanText(value) {
  return typeof value === 'string' ? value.trim() : '';
}

function normaliseOptionalText(value) {
  const cleaned = cleanText(value);
  return cleaned.length === 0 ? null : cleaned;
}

function isRealIsoDate(value) {
  if (!DATE_PATTERN.test(value)) {
    return false;
  }

  const [year, month, day] = value.split('-').map(Number);
  const date = new Date(Date.UTC(year, month - 1, day));

  return (
    date.getUTCFullYear() === year &&
    date.getUTCMonth() === month - 1 &&
    date.getUTCDate() === day
  );
}

function todayAsIsoDate(today) {
  return today.toISOString().slice(0, 10);
}

function validateRequiredText(errors, field, label, value, min, max) {
  if (!value) {
    errors[field] = `${label} is required.`;
    return;
  }

  if (value.length < min || value.length > max) {
    errors[field] = `${label} must be between ${min} and ${max} characters.`;
  }
}

export function validateApplication(input, options = {}) {
  const today = options.today ?? new Date();
  const source = input && typeof input === 'object' && !Array.isArray(input) ? input : {};
  const value = {
    company: cleanText(source.company),
    role: cleanText(source.role),
    status: cleanText(source.status),
    applicationDate: normaliseOptionalText(source.applicationDate),
    followUpDate: normaliseOptionalText(source.followUpDate),
    jobUrl: normaliseOptionalText(source.jobUrl),
    notes: normaliseOptionalText(source.notes),
  };
  const errors = {};

  validateRequiredText(errors, 'company', 'Company', value.company, 2, 100);
  validateRequiredText(errors, 'role', 'Role', value.role, 2, 120);

  if (!APPLICATION_STATUSES.includes(value.status)) {
    errors.status = `Choose one of: ${APPLICATION_STATUSES.join(', ')}.`;
  }

  if (value.applicationDate) {
    if (!isRealIsoDate(value.applicationDate)) {
      errors.applicationDate = 'Enter a valid application date.';
    } else if (value.applicationDate > todayAsIsoDate(today)) {
      errors.applicationDate = 'Application date cannot be in the future.';
    }
  } else if (value.status !== 'Wishlist') {
    errors.applicationDate = 'Application date is required for this status.';
  }

  if (value.followUpDate && !isRealIsoDate(value.followUpDate)) {
    errors.followUpDate = 'Enter a valid follow-up date.';
  } else if (
    value.followUpDate &&
    value.applicationDate &&
    isRealIsoDate(value.applicationDate) &&
    value.followUpDate < value.applicationDate
  ) {
    errors.followUpDate = 'Follow-up date cannot be before the application date.';
  }

  if (value.jobUrl) {
    if (value.jobUrl.length > 500) {
      errors.jobUrl = 'Job URL must be 500 characters or fewer.';
    } else {
      try {
        const url = new URL(value.jobUrl);
        if (url.protocol !== 'https:') {
          errors.jobUrl = 'Job URL must start with https://.';
        }
      } catch {
        errors.jobUrl = 'Enter a valid HTTPS job URL.';
      }
    }
  }

  if (value.notes && value.notes.length > 1000) {
    errors.notes = 'Notes must be 1,000 characters or fewer.';
  }

  return Object.keys(errors).length > 0
    ? { value: null, errors }
    : { value, errors };
}

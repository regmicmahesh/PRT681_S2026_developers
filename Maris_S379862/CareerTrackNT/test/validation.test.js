import test from 'node:test';
import assert from 'node:assert/strict';

import {
  APPLICATION_STATUSES,
  validateApplication,
} from '../src/domain/application.js';

const today = new Date('2026-08-20T12:00:00Z');

test('validateApplication trims and accepts a valid applied application', () => {
  const result = validateApplication(
    {
      company: '  Territory Tech  ',
      role: '  Full-Stack Developer ',
      status: 'Applied',
      applicationDate: '2026-08-19',
      followUpDate: '2026-08-26',
      jobUrl: 'https://example.com/jobs/123',
      notes: '  Follow up after one week.  ',
    },
    { today },
  );

  assert.deepEqual(result.errors, {});
  assert.deepEqual(result.value, {
    company: 'Territory Tech',
    role: 'Full-Stack Developer',
    status: 'Applied',
    applicationDate: '2026-08-19',
    followUpDate: '2026-08-26',
    jobUrl: 'https://example.com/jobs/123',
    notes: 'Follow up after one week.',
  });
});

test('validateApplication reports required fields and invalid status', () => {
  const result = validateApplication(
    { company: ' ', role: '', status: 'Waiting' },
    { today },
  );

  assert.equal(result.value, null);
  assert.equal(result.errors.company, 'Company is required.');
  assert.equal(result.errors.role, 'Role is required.');
  assert.match(result.errors.status, /Choose one of/);
});

test('validateApplication allows Wishlist without application date', () => {
  const result = validateApplication(
    { company: 'CDU', role: 'Developer', status: 'Wishlist' },
    { today },
  );

  assert.deepEqual(result.errors, {});
  assert.equal(result.value.applicationDate, null);
});

test('validateApplication requires an application date after Wishlist', () => {
  const result = validateApplication(
    { company: 'CDU', role: 'Developer', status: 'Interview' },
    { today },
  );

  assert.equal(
    result.errors.applicationDate,
    'Application date is required for this status.',
  );
});

test('validateApplication rejects impossible and future application dates', () => {
  const impossible = validateApplication(
    {
      company: 'CDU',
      role: 'Developer',
      status: 'Applied',
      applicationDate: '2026-02-30',
    },
    { today },
  );
  const future = validateApplication(
    {
      company: 'CDU',
      role: 'Developer',
      status: 'Applied',
      applicationDate: '2026-08-21',
    },
    { today },
  );

  assert.equal(impossible.errors.applicationDate, 'Enter a valid application date.');
  assert.equal(future.errors.applicationDate, 'Application date cannot be in the future.');
});

test('validateApplication rejects a follow-up before application date', () => {
  const result = validateApplication(
    {
      company: 'CDU',
      role: 'Developer',
      status: 'Applied',
      applicationDate: '2026-08-19',
      followUpDate: '2026-08-18',
    },
    { today },
  );

  assert.equal(
    result.errors.followUpDate,
    'Follow-up date cannot be before the application date.',
  );
});

test('validateApplication accepts only HTTPS job URLs', () => {
  const result = validateApplication(
    {
      company: 'CDU',
      role: 'Developer',
      status: 'Applied',
      applicationDate: '2026-08-19',
      jobUrl: 'http://example.com/job',
    },
    { today },
  );

  assert.equal(result.errors.jobUrl, 'Job URL must start with https://.');
});

test('APPLICATION_STATUSES exposes the six approved stages', () => {
  assert.deepEqual(APPLICATION_STATUSES, [
    'Wishlist',
    'Applied',
    'Interview',
    'Offer',
    'Rejected',
    'Withdrawn',
  ]);
});

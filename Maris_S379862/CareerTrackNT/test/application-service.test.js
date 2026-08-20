import test from 'node:test';
import assert from 'node:assert/strict';

import { createDatabase } from '../src/data/database.js';
import { createApplicationRepository } from '../src/data/application-repository.js';
import { createApplicationService } from '../src/domain/application-service.js';

function setup() {
  const database = createDatabase(':memory:');
  const repository = createApplicationRepository(database);
  const service = createApplicationService(repository, {
    now: () => new Date('2026-08-20T12:00:00Z'),
  });

  return { database, service };
}

const validApplication = {
  company: 'Territory Tech',
  role: 'Full-Stack Developer',
  status: 'Applied',
  applicationDate: '2026-08-19',
  followUpDate: '2026-08-26',
  jobUrl: 'https://example.com/jobs/123',
  notes: 'Follow up after one week.',
};

test('application service creates and retrieves a validated application', () => {
  const { database, service } = setup();

  try {
    const created = service.create(validApplication);
    const retrieved = service.getById(created.id);

    assert.equal(created.id, 1);
    assert.equal(retrieved.company, 'Territory Tech');
    assert.equal(retrieved.status, 'Applied');
    assert.match(retrieved.createdAt, /^2026-08-20T/);
  } finally {
    database.close();
  }
});

test('application service rejects invalid input without writing a row', () => {
  const { database, service } = setup();

  try {
    assert.throws(
      () => service.create({ company: '', role: '', status: 'Waiting' }),
      (error) =>
        error.code === 'VALIDATION_ERROR' &&
        error.status === 422 &&
        error.details.company === 'Company is required.',
    );
    assert.equal(service.list({}).total, 0);
  } finally {
    database.close();
  }
});

test('application service filters by status and searches company or role', () => {
  const { database, service } = setup();

  try {
    service.create(validApplication);
    service.create({
      ...validApplication,
      company: 'Charles Darwin University',
      role: 'Software Developer',
      status: 'Interview',
    });

    const byStatus = service.list({ status: 'Interview', page: 1, pageSize: 20 });
    const bySearch = service.list({ search: 'territory', page: 1, pageSize: 20 });

    assert.equal(byStatus.total, 1);
    assert.equal(byStatus.items[0].role, 'Software Developer');
    assert.equal(bySearch.total, 1);
    assert.equal(bySearch.items[0].company, 'Territory Tech');
  } finally {
    database.close();
  }
});

test('application service bounds pagination values', () => {
  const { database, service } = setup();

  try {
    const result = service.list({ page: '-5', pageSize: '1000' });

    assert.equal(result.page, 1);
    assert.equal(result.pageSize, 100);
  } finally {
    database.close();
  }
});

test('application service updates a record and reports missing identifiers', () => {
  const { database, service } = setup();

  try {
    const created = service.create(validApplication);
    const updated = service.update(created.id, {
      ...validApplication,
      status: 'Interview',
      notes: 'First interview booked.',
    });

    assert.equal(updated.status, 'Interview');
    assert.equal(updated.notes, 'First interview booked.');
    assert.throws(
      () => service.update(999, validApplication),
      (error) => error.code === 'NOT_FOUND' && error.status === 404,
    );
  } finally {
    database.close();
  }
});

test('application service deletes an existing record', () => {
  const { database, service } = setup();

  try {
    const created = service.create(validApplication);

    assert.equal(service.remove(created.id), true);
    assert.throws(
      () => service.getById(created.id),
      (error) => error.code === 'NOT_FOUND',
    );
  } finally {
    database.close();
  }
});

test('application service returns total and per-status summary', () => {
  const { database, service } = setup();

  try {
    service.create(validApplication);
    service.create({ ...validApplication, company: 'CDU', status: 'Interview' });
    service.create({ ...validApplication, company: 'NTG', status: 'Interview' });

    assert.deepEqual(service.summary(), {
      total: 3,
      byStatus: {
        Wishlist: 0,
        Applied: 1,
        Interview: 2,
        Offer: 0,
        Rejected: 0,
        Withdrawn: 0,
      },
    });
  } finally {
    database.close();
  }
});

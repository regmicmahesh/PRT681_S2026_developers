import test from 'node:test';
import assert from 'node:assert/strict';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

import { createDatabase } from '../src/data/database.js';
import { createCareerTrackServer } from '../src/http/app.js';

const testDirectory = dirname(fileURLToPath(import.meta.url));
const publicDirectory = join(testDirectory, '..', 'public');

async function setup() {
  const database = createDatabase(':memory:');
  const server = createCareerTrackServer({
    database,
    now: () => new Date('2026-08-20T12:00:00Z'),
    publicDirectory,
  });
  await new Promise((resolve) => server.listen(0, '127.0.0.1', resolve));
  const address = server.address();

  return {
    database,
    server,
    baseUrl: `http://127.0.0.1:${address.port}`,
    async close() {
      await new Promise((resolve, reject) =>
        server.close((error) => (error ? reject(error) : resolve())),
      );
      database.close();
    },
  };
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

async function json(response) {
  return response.json();
}

test('API supports create, retrieve, update, summary and delete', async () => {
  const context = await setup();

  try {
    const createdResponse = await fetch(`${context.baseUrl}/api/applications`, {
      method: 'POST',
      headers: { 'content-type': 'application/json' },
      body: JSON.stringify(validApplication),
    });
    const created = await json(createdResponse);

    assert.equal(createdResponse.status, 201);
    assert.equal(created.company, 'Territory Tech');
    assert.match(createdResponse.headers.get('location'), /\/api\/applications\/1$/);

    const retrievedResponse = await fetch(
      `${context.baseUrl}/api/applications/${created.id}`,
    );
    assert.equal(retrievedResponse.status, 200);

    const updatedResponse = await fetch(
      `${context.baseUrl}/api/applications/${created.id}`,
      {
        method: 'PUT',
        headers: { 'content-type': 'application/json' },
        body: JSON.stringify({ ...validApplication, status: 'Interview' }),
      },
    );
    assert.equal((await json(updatedResponse)).status, 'Interview');

    const summaryResponse = await fetch(`${context.baseUrl}/api/summary`);
    assert.deepEqual(await json(summaryResponse), {
      total: 1,
      byStatus: {
        Wishlist: 0,
        Applied: 0,
        Interview: 1,
        Offer: 0,
        Rejected: 0,
        Withdrawn: 0,
      },
    });

    const deletedResponse = await fetch(
      `${context.baseUrl}/api/applications/${created.id}`,
      { method: 'DELETE' },
    );
    assert.equal(deletedResponse.status, 204);

    const missingResponse = await fetch(
      `${context.baseUrl}/api/applications/${created.id}`,
    );
    assert.equal(missingResponse.status, 404);
  } finally {
    await context.close();
  }
});

test('API returns validation details without writing invalid input', async () => {
  const context = await setup();

  try {
    const response = await fetch(`${context.baseUrl}/api/applications`, {
      method: 'POST',
      headers: { 'content-type': 'application/json' },
      body: JSON.stringify({ company: '', role: '', status: 'Unknown' }),
    });
    const body = await json(response);

    assert.equal(response.status, 422);
    assert.equal(body.error.code, 'VALIDATION_ERROR');
    assert.equal(body.error.details.company, 'Company is required.');

    const list = await json(await fetch(`${context.baseUrl}/api/applications`));
    assert.equal(list.total, 0);
  } finally {
    await context.close();
  }
});

test('API list accepts bounded query parameters and filters', async () => {
  const context = await setup();

  try {
    for (const application of [
      validApplication,
      { ...validApplication, company: 'CDU', role: 'Developer', status: 'Interview' },
    ]) {
      await fetch(`${context.baseUrl}/api/applications`, {
        method: 'POST',
        headers: { 'content-type': 'application/json' },
        body: JSON.stringify(application),
      });
    }

    const response = await fetch(
      `${context.baseUrl}/api/applications?status=Interview&search=developer&pageSize=1000`,
    );
    const body = await json(response);

    assert.equal(response.status, 200);
    assert.equal(body.total, 1);
    assert.equal(body.pageSize, 100);
    assert.equal(body.items[0].company, 'CDU');
  } finally {
    await context.close();
  }
});

test('API rejects unsupported media types and malformed JSON', async () => {
  const context = await setup();

  try {
    const wrongType = await fetch(`${context.baseUrl}/api/applications`, {
      method: 'POST',
      headers: { 'content-type': 'text/plain' },
      body: JSON.stringify(validApplication),
    });
    const malformed = await fetch(`${context.baseUrl}/api/applications`, {
      method: 'POST',
      headers: { 'content-type': 'application/json' },
      body: '{not-json}',
    });

    assert.equal(wrongType.status, 415);
    assert.equal((await json(wrongType)).error.code, 'UNSUPPORTED_MEDIA_TYPE');
    assert.equal(malformed.status, 400);
    assert.equal((await json(malformed)).error.code, 'INVALID_JSON');
  } finally {
    await context.close();
  }
});

test('API rejects request bodies larger than 64 KB', async () => {
  const context = await setup();

  try {
    const response = await fetch(`${context.baseUrl}/api/applications`, {
      method: 'POST',
      headers: { 'content-type': 'application/json' },
      body: JSON.stringify({ ...validApplication, notes: 'x'.repeat(70 * 1024) }),
    });
    const body = await json(response);

    assert.equal(response.status, 413);
    assert.equal(body.error.code, 'PAYLOAD_TOO_LARGE');
  } finally {
    await context.close();
  }
});

test('API supplies security headers and a health response', async () => {
  const context = await setup();

  try {
    const response = await fetch(`${context.baseUrl}/api/health`);

    assert.equal(response.status, 200);
    assert.equal((await json(response)).status, 'ok');
    assert.match(response.headers.get('content-security-policy'), /default-src 'self'/);
    assert.equal(response.headers.get('x-content-type-options'), 'nosniff');
    assert.equal(response.headers.get('x-frame-options'), 'DENY');
  } finally {
    await context.close();
  }
});

test('server returns the accessible application shell', async () => {
  const context = await setup();

  try {
    const response = await fetch(`${context.baseUrl}/`);
    const html = await response.text();

    assert.equal(response.status, 200);
    assert.match(response.headers.get('content-type'), /^text\/html/);
    assert.match(html, /<html lang="en">/);
    assert.match(html, /href="#main-content"/);
    assert.match(html, /<h1 id="page-title">/);
    assert.match(html, /aria-live="polite"/);
  } finally {
    await context.close();
  }
});

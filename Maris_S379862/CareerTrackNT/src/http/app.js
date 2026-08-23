import { createReadStream, existsSync } from 'node:fs';
import { createServer } from 'node:http';
import { extname, join } from 'node:path';

import { createApplicationRepository } from '../data/application-repository.js';
import { createApplicationService } from '../domain/application-service.js';
import { ApplicationError } from '../domain/errors.js';

const MAX_BODY_BYTES = 64 * 1024;
const STATIC_FILES = new Map([
  ['/', 'index.html'],
  ['/app.js', 'app.js'],
  ['/styles.css', 'styles.css'],
]);

const CONTENT_TYPES = {
  '.html': 'text/html; charset=utf-8',
  '.js': 'text/javascript; charset=utf-8',
  '.css': 'text/css; charset=utf-8',
};

function setSecurityHeaders(response) {
  response.setHeader(
    'Content-Security-Policy',
    "default-src 'self'; script-src 'self'; style-src 'self'; img-src 'self' data:; connect-src 'self'; object-src 'none'; base-uri 'none'; frame-ancestors 'none'; form-action 'self'",
  );
  response.setHeader('X-Content-Type-Options', 'nosniff');
  response.setHeader('X-Frame-Options', 'DENY');
  response.setHeader('Referrer-Policy', 'no-referrer');
  response.setHeader(
    'Permissions-Policy',
    'camera=(), microphone=(), geolocation=()',
  );
  response.setHeader('Cache-Control', 'no-store');
}

function sendJson(response, status, body, headers = {}) {
  setSecurityHeaders(response);
  response.writeHead(status, {
    'content-type': 'application/json; charset=utf-8',
    ...headers,
  });
  response.end(JSON.stringify(body));
}

function sendNoContent(response) {
  setSecurityHeaders(response);
  response.writeHead(204);
  response.end();
}

async function readJson(request) {
  const contentType = request.headers['content-type'] ?? '';
  if (!contentType.toLowerCase().startsWith('application/json')) {
    throw new ApplicationError(
      'UNSUPPORTED_MEDIA_TYPE',
      'Use application/json for this request.',
      415,
    );
  }

  const declaredLength = Number(request.headers['content-length'] ?? 0);
  if (declaredLength > MAX_BODY_BYTES) {
    throw new ApplicationError(
      'PAYLOAD_TOO_LARGE',
      'Request body must be 64 KB or smaller.',
      413,
    );
  }

  let total = 0;
  let tooLarge = false;
  const chunks = [];

  for await (const chunk of request) {
    total += chunk.length;
    if (total > MAX_BODY_BYTES) {
      tooLarge = true;
      continue;
    }
    chunks.push(chunk);
  }

  if (tooLarge) {
    throw new ApplicationError(
      'PAYLOAD_TOO_LARGE',
      'Request body must be 64 KB or smaller.',
      413,
    );
  }

  try {
    return JSON.parse(Buffer.concat(chunks).toString('utf8'));
  } catch {
    throw new ApplicationError('INVALID_JSON', 'Enter valid JSON.', 400);
  }
}

function sendError(response, error) {
  if (error instanceof ApplicationError) {
    const body = {
      error: {
        code: error.code,
        message: error.message,
      },
    };
    if (error.details !== undefined) {
      body.error.details = error.details;
    }
    sendJson(response, error.status, body);
    return;
  }

  sendJson(response, 500, {
    error: {
      code: 'INTERNAL_ERROR',
      message: 'Something went wrong.',
    },
  });
}

function sendStatic(response, publicDirectory, pathname) {
  const fileName = STATIC_FILES.get(pathname);
  if (!fileName || !publicDirectory) {
    return false;
  }

  const filePath = join(publicDirectory, fileName);
  if (!existsSync(filePath)) {
    return false;
  }

  setSecurityHeaders(response);
  response.writeHead(200, {
    'content-type': CONTENT_TYPES[extname(filePath)] ?? 'application/octet-stream',
  });
  createReadStream(filePath).pipe(response);
  return true;
}

export function createCareerTrackServer({ database, now, publicDirectory } = {}) {
  if (!database) {
    throw new TypeError('A database is required.');
  }

  const repository = createApplicationRepository(database);
  const service = createApplicationService(repository, { now });

  return createServer(async (request, response) => {
    const url = new URL(request.url ?? '/', 'http://localhost');
    const idMatch = url.pathname.match(/^\/api\/applications\/(\d+)$/);

    try {
      if (request.method === 'GET' && url.pathname === '/api/health') {
        sendJson(response, 200, { status: 'ok' });
        return;
      }

      if (request.method === 'GET' && url.pathname === '/api/summary') {
        sendJson(response, 200, service.summary());
        return;
      }

      if (request.method === 'GET' && url.pathname === '/api/applications') {
        sendJson(
          response,
          200,
          service.list(Object.fromEntries(url.searchParams.entries())),
        );
        return;
      }

      if (request.method === 'POST' && url.pathname === '/api/applications') {
        const application = service.create(await readJson(request));
        sendJson(response, 201, application, {
          location: `/api/applications/${application.id}`,
        });
        return;
      }

      if (request.method === 'GET' && idMatch) {
        sendJson(response, 200, service.getById(idMatch[1]));
        return;
      }

      if (request.method === 'PUT' && idMatch) {
        sendJson(
          response,
          200,
          service.update(idMatch[1], await readJson(request)),
        );
        return;
      }

      if (request.method === 'DELETE' && idMatch) {
        service.remove(idMatch[1]);
        sendNoContent(response);
        return;
      }

      if (request.method === 'GET' && sendStatic(response, publicDirectory, url.pathname)) {
        return;
      }

      sendJson(response, 404, {
        error: { code: 'NOT_FOUND', message: 'Resource was not found.' },
      });
    } catch (error) {
      sendError(response, error);
    }
  });
}

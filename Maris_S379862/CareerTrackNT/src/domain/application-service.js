import { APPLICATION_STATUSES, validateApplication } from './application.js';
import { ApplicationError, notFoundError } from './errors.js';

function positiveInteger(value, fallback, maximum = Number.MAX_SAFE_INTEGER) {
  const parsed = Number.parseInt(String(value ?? ''), 10);
  if (!Number.isFinite(parsed) || parsed < 1) {
    return fallback;
  }
  return Math.min(parsed, maximum);
}

function validId(value) {
  const id = Number(value);
  if (!Number.isSafeInteger(id) || id < 1) {
    throw new ApplicationError('INVALID_ID', 'Enter a valid application ID.', 400);
  }
  return id;
}

export function createApplicationService(repository, options = {}) {
  const now = options.now ?? (() => new Date());

  function validated(input) {
    const result = validateApplication(input, { today: now() });
    if (result.value === null) {
      throw new ApplicationError(
        'VALIDATION_ERROR',
        'Check the highlighted fields.',
        422,
        result.errors,
      );
    }
    return result.value;
  }

  return {
    create(input) {
      return repository.create(validated(input), now().toISOString());
    },

    getById(id) {
      const application = repository.getById(validId(id));
      if (!application) {
        throw notFoundError();
      }
      return application;
    },

    list(query) {
      const page = positiveInteger(query.page, 1);
      const pageSize = positiveInteger(query.pageSize, 20, 100);
      const status = typeof query.status === 'string' ? query.status.trim() : '';
      const search = typeof query.search === 'string'
        ? query.search.trim().slice(0, 120)
        : '';

      if (status && !APPLICATION_STATUSES.includes(status)) {
        throw new ApplicationError(
          'INVALID_FILTER',
          'Status filter is not recognised.',
          400,
        );
      }

      const result = repository.list({ status, search, page, pageSize });
      return {
        ...result,
        page,
        pageSize,
        pageCount: Math.max(1, Math.ceil(result.total / pageSize)),
      };
    },

    update(id, input) {
      const application = repository.update(
        validId(id),
        validated(input),
        now().toISOString(),
      );
      if (!application) {
        throw notFoundError();
      }
      return application;
    },

    remove(id) {
      if (!repository.remove(validId(id))) {
        throw notFoundError();
      }
      return true;
    },

    summary() {
      const byStatus = Object.fromEntries(
        APPLICATION_STATUSES.map((status) => [status, 0]),
      );
      let total = 0;

      for (const row of repository.summary()) {
        byStatus[row.status] = row.count;
        total += row.count;
      }

      return { total, byStatus };
    },
  };
}

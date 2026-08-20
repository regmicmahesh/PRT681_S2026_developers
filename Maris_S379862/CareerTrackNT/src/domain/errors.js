export class ApplicationError extends Error {
  constructor(code, message, status, details = undefined) {
    super(message);
    this.name = 'ApplicationError';
    this.code = code;
    this.status = status;
    this.details = details;
  }
}

export function notFoundError() {
  return new ApplicationError(
    'NOT_FOUND',
    'Application was not found.',
    404,
  );
}

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5289/api';

export class ApiError extends Error {
  status: number;
  title: string;
  detail?: string;

  constructor(status: number, title: string, detail?: string) {
    super(detail ?? title);
    this.status = status;
    this.title = title;
    this.detail = detail;
  }
}

async function request<TResponse>(path: string, init?: RequestInit): Promise<TResponse> {
  const response = await fetch(`${BASE_URL}${path}`, {
    ...init,
    headers: {
      'Content-Type': 'application/json',
      ...init?.headers,
    },
  });

  if (!response.ok) {
    const problem = await response.json().catch(() => null);
    throw new ApiError(response.status, problem?.title ?? response.statusText, problem?.detail);
  }

  if (response.status === 204) {
    return undefined as TResponse;
  }

  return (await response.json()) as TResponse;
}

export const httpClient = {
  get: <TResponse>(path: string) => request<TResponse>(path),
  post: <TResponse>(path: string, body?: unknown) =>
    request<TResponse>(path, { method: 'POST', body: body ? JSON.stringify(body) : undefined }),
};

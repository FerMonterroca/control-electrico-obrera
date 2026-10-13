import { env } from './env';

type RequestOptions = Omit<RequestInit, 'body' | 'method'>;
const requestTimeoutMs = 10_000;

export class ApiError extends Error {
  constructor(
    message: string,
    public readonly status: number,
  ) {
    super(message);
    this.name = 'ApiError';
  }
}

async function request<T>(
  path: string,
  init: RequestInit = {},
): Promise<T> {
  const timeoutSignal = AbortSignal.timeout(requestTimeoutMs);
  const signal = init.signal
    ? AbortSignal.any([init.signal, timeoutSignal])
    : timeoutSignal;

  const response = await fetch(`${env.apiUrl}${path.startsWith('/') ? path : `/${path}`}`, {
    ...init,
    signal,
    headers: {
      Accept: 'application/json',
      ...init.headers,
    },
  });

  if (!response.ok) {
    throw new ApiError('No fue posible completar la solicitud.', response.status);
  }

  return response.json() as Promise<T>;
}

export const api = {
  get<T>(path: string, options?: RequestOptions): Promise<T> {
    return request<T>(path, { ...options, method: 'GET' });
  },

  post<TResponse, TBody>(
    path: string,
    body: TBody,
    options?: RequestOptions,
  ): Promise<TResponse> {
    return request<TResponse>(path, {
      ...options,
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        ...options?.headers,
      },
      body: JSON.stringify(body),
    });
  },
};

import { useEffect, useState } from 'react';
import { api } from '../lib/api';
import type { HealthResponse } from '../types/health';

type BackendStatus = 'checking' | 'ok' | 'error';

export default function HealthStatus() {
  const [backendStatus, setBackendStatus] = useState<BackendStatus>('checking');
  const [databaseStatus, setDatabaseStatus] = useState<BackendStatus>('checking');

  useEffect(() => {
    const controller = new AbortController();

    api.get<HealthResponse>('/api/health', { signal: controller.signal })
      .then((response) => {
        setBackendStatus(response.status === 'ok' ? 'ok' : 'error');
      })
      .catch((error: unknown) => {
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        setBackendStatus('error');
      });

    api.get<HealthResponse>('/api/ready', { signal: controller.signal })
      .then((response) => {
        setDatabaseStatus(response.status === 'ok' ? 'ok' : 'error');
      })
      .catch((error: unknown) => {
        if (error instanceof DOMException && error.name === 'AbortError') {
          return;
        }

        setDatabaseStatus('error');
      });

    return () => controller.abort();
  }, []);

  const labels: Record<BackendStatus, string> = {
    checking: 'Verificando…',
    ok: 'OK',
    error: 'No disponible',
  };

  return (
    <dl className="health-list" aria-live="polite">
      <div>
        <dt>Frontend</dt>
        <dd className="status-ok">OK</dd>
      </div>
      <div>
        <dt>Backend</dt>
        <dd className={`status-${backendStatus}`}>{labels[backendStatus]}</dd>
      </div>
      <div>
        <dt>Database</dt>
        <dd className={`status-${databaseStatus}`}>{labels[databaseStatus]}</dd>
      </div>
    </dl>
  );
}

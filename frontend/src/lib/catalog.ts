import type { AvailabilityStatus, ProductQuery } from '../types/catalog';

export const availabilityLabels: Record<AvailabilityStatus, string> = {
  Consult: 'Consultar disponibilidad',
  AvailableOnRequest: 'Disponible bajo pedido',
  DiscontinuedSearch: 'Búsqueda especial',
  Unavailable: 'No disponible',
};

export const availabilityOptions = Object.entries(availabilityLabels) as [AvailabilityStatus, string][];

export function resolveMediaUrl(_storageKey: string | null | undefined): string | null {
  return null;
}

export function parseCatalogQuery(params: URLSearchParams): ProductQuery {
  const pageValue = Number(params.get('page'));
  const pageSizeValue = Number(params.get('pageSize'));
  const availability = params.get('availability');
  const sort = params.get('sort');
  const featured = params.get('featured');
  const result: ProductQuery = {
    page: Number.isInteger(pageValue) && pageValue > 0 ? pageValue : 1,
    pageSize: Number.isInteger(pageSizeValue) && pageSizeValue >= 1 && pageSizeValue <= 100 ? pageSizeValue : 12,
  };

  const copy = (key: 'search' | 'brand' | 'category') => {
    const value = params.get(key)?.trim();
    if (value) result[key] = value;
  };
  copy('search'); copy('brand'); copy('category');

  if (availability && availability in availabilityLabels) result.availability = availability as AvailabilityStatus;
  if (sort === 'newest' || sort === 'name-asc' || sort === 'name-desc') result.sort = sort;
  if (featured === 'true') result.featured = true;
  return result;
}

export function toSearchParams(query: ProductQuery): URLSearchParams {
  const params = new URLSearchParams();
  for (const [key, value] of Object.entries(query)) {
    if (value !== undefined && value !== null && value !== '' && !(key === 'page' && value === 1)) {
      params.set(key, String(value));
    }
  }
  return params;
}

export function pageUrl(current: URLSearchParams, page: number, path = '/catalogo'): string {
  const next = new URLSearchParams(current);
  if (page <= 1) next.delete('page'); else next.set('page', String(page));
  const query = next.toString();
  return `${path}${query ? `?${query}` : ''}`;
}

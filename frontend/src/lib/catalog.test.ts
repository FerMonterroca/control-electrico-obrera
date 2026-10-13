import { describe, expect, it } from 'vitest';
import { availabilityLabels, pageUrl, parseCatalogQuery, resolveMediaUrl, toSearchParams } from './catalog';

describe('catalog query helpers', () => {
  it('parses supported filters and rejects invalid values', () => {
    const query = parseCatalogQuery(new URLSearchParams('search=contactor&brand=schneider-electric&page=2&pageSize=24&availability=Consult&sort=name-asc&featured=true'));
    expect(query).toMatchObject({ search: 'contactor', brand: 'schneider-electric', page: 2, pageSize: 24, availability: 'Consult', sort: 'name-asc', featured: true });
    expect(parseCatalogQuery(new URLSearchParams('page=-2&availability=Other&sort=random'))).toEqual({ page: 1, pageSize: 12 });
  });

  it('serializes API query values and preserves filters in pagination links', () => {
    expect(toSearchParams({ page: 1, search: 'motor', featured: false }).toString()).toBe('search=motor&featured=false');
    expect(pageUrl(new URLSearchParams('brand=iusa&page=2'), 3)).toBe('/catalogo?brand=iusa&page=3');
    expect(pageUrl(new URLSearchParams('brand=iusa&page=2'), 1)).toBe('/catalogo?brand=iusa');
  });
});

describe('catalog presentation helpers', () => {
  it('provides every confirmed availability label', () => {
    expect(availabilityLabels).toEqual({ Consult: 'Consultar disponibilidad', AvailableOnRequest: 'Disponible bajo pedido', DiscontinuedSearch: 'Búsqueda especial', Unavailable: 'No disponible' });
  });

  it('does not invent media URLs before storage is configured', () => {
    expect(resolveMediaUrl('products/example.webp')).toBeNull();
  });
});

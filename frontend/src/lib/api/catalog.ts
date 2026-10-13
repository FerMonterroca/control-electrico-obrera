import { api } from '../api';
import { toSearchParams } from '../catalog';
import type { BrandDetail, BrandSummary, CategoryDetail, CategorySummary, PagedResponse, ProductDetail, ProductListItem, ProductQuery } from '../../types/catalog';

export const catalogApi = {
  products(query: ProductQuery = {}) {
    const params = toSearchParams(query).toString();
    return api.get<PagedResponse<ProductListItem>>(`/api/products${params ? `?${params}` : ''}`);
  },
  product(slug: string) { return api.get<ProductDetail>(`/api/products/${encodeURIComponent(slug)}`); },
  brands() { return api.get<BrandSummary[]>('/api/brands'); },
  brand(slug: string) { return api.get<BrandDetail>(`/api/brands/${encodeURIComponent(slug)}`); },
  categories() { return api.get<CategorySummary[]>('/api/categories'); },
  category(slug: string) { return api.get<CategoryDetail>(`/api/categories/${encodeURIComponent(slug)}`); },
};

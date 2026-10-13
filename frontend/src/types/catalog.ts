export type AvailabilityStatus =
  | 'Consult'
  | 'AvailableOnRequest'
  | 'DiscontinuedSearch'
  | 'Unavailable';

export interface BrandSummary { id: number; name: string; slug: string }
export interface CategorySummary { id: number; name: string; slug: string; parentCategoryId: number | null }

export interface ProductListItem {
  id: number;
  name: string;
  slug: string;
  shortDescription: string | null;
  brand: BrandSummary | null;
  category: CategorySummary;
  model: string | null;
  skuOrCode: string | null;
  unit: string;
  availabilityStatus: AvailabilityStatus;
  isFeatured: boolean;
  primaryImageStorageKey: string | null;
  createdAt: string;
}

export interface ProductImage { storageKey: string; altText: string; sortOrder: number; isPrimary: boolean }
export interface ProductSpecification { name: string; value: string; unit: string | null; sortOrder: number }

export interface ProductDetail extends Omit<ProductListItem, 'primaryImageStorageKey'> {
  description: string | null;
  seoTitle: string | null;
  seoDescription: string | null;
  images: ProductImage[];
  specifications: ProductSpecification[];
  updatedAt: string;
}

export interface BrandDetail extends BrandSummary {
  description: string | null;
  logoStorageKey: string | null;
  seoTitle: string | null;
  seoDescription: string | null;
}

export interface CategoryDetail extends CategorySummary {
  description: string | null;
  imageStorageKey: string | null;
  parentCategory: CategorySummary | null;
  childCategories: CategorySummary[];
  seoTitle: string | null;
  seoDescription: string | null;
}

export interface PagedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}

export interface ProductQuery {
  page?: number;
  pageSize?: number;
  search?: string;
  brand?: string;
  category?: string;
  availability?: AvailabilityStatus;
  featured?: boolean;
  sort?: 'newest' | 'name-asc' | 'name-desc';
}

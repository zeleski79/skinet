import { Injectable } from '@angular/core';
import { Product } from '../../shared/models/product';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Pagination } from '../../shared/models/pagination';
import { ShopParams } from '../../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  brands: string[] = [];
  types: string[] = [];

  constructor(private http: HttpClient) {
  }

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.brands.length > 0) {
      params = params.append('brands', shopParams.brands.join(','));
    }

    if (shopParams.types.length > 0) {
      params = params.append('types', shopParams.types.join(','));
    }  
    if (shopParams.sort) {
      params = params.append('sort', shopParams.sort);
    }
    if (shopParams.search.trim() != '') {
      params = params.append('search', shopParams.search);
    }
    params = params.append('pageSize', shopParams.pageSize);
    params = params.append('pageIndex', shopParams.pageNumber);
    return this.http.get<Pagination<Product>>(this.baseUrl + 'products', {params});
  }

  getProduct(id: number) {
    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  getBrands() {
    if (this.brands.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: response => this.brands = response
    })
  }

  getTypes() {
    if (this.types.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'products/types').subscribe({
      next: response => this.types = response
    })
  }
}

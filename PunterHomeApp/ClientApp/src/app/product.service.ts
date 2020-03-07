import { Injectable, Inject } from '@angular/core';
import { IProduct } from './models/product';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getProducts(): Observable<IProduct[]>{

    console.log('get proucts');
    let result;
    return this.http.get<IProduct[]>(this.baseUrl + 'api/product');
  }

  search(searchText: string): Observable<IProduct[]>
  {
    return this.http.get<IProduct[]>(this.baseUrl + 'api/product');
  }
}

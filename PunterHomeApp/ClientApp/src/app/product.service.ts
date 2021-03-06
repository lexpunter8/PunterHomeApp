import { Injectable, Inject } from '@angular/core';
import { IProduct } from './models/product';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IRecipe } from './models/Recipe';

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
    return this.http.get<IProduct[]>(this.baseUrl + 'api/product/search?searchText=' + searchText);
  }

  saveRecipe(recipe: IRecipe) {
    const headers = new HttpHeaders().set('Content-Type', 'application/json;');
    return this.http.post(this.baseUrl + 'api/recipe',
      JSON.stringify(recipe),
      { headers: headers });
  }
}

import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public products: IProduct[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.getProducts()
  }

  getProducts(): void {
    console.log('get proucts');
    this.http.get<IProduct[]>(this.baseUrl + 'api/product').subscribe(result => {
      this.products = result;
    }, error => console.error(error));
  }

  ngOnInit(): void {
    this.unitTypeOptions = Object.keys(EUnitVolumeType);
  }

  public newProduct: Product = { name: "test", quantity: 1, unitQuantity: 0, unitQuantityType: EUnitVolumeType.Kg };

  private unitTypes = EUnitVolumeType;
  public unitTypeOptions = [];
   
  addProduct(): void {
    console.log(this.newProduct.name);
    console.log(this.newProduct.unitQuantity);
    console.log(this.newProduct.unitQuantityType);
    console.log(JSON.stringify(this.newProduct));

    const headers = new HttpHeaders().set('Content-Type', 'application/json;');
    this.http.post(this.baseUrl + 'api/product',
      JSON.stringify(this.newProduct),
      { headers: headers })
      .subscribe(
        (val) => {
          console.log('POST call successful value returned in body',
            val);
        },
        response => {
          console.log('POST call in error', response);
        },
        () => {
          console.log('The POST observable is now completed.');

          this.getProducts();
        });
  }

}

interface IProduct {
  id: string;
  name: string;
  quantity: string;
  unitQuantity: string;
  unitQuantityType: string;
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
export class Product {
  name: string;
  quantity: number;
  unitQuantity: number;
  unitQuantityType: EUnitVolumeType;
}

export enum EUnitVolumeType {
  Kg,
  gr,
  L,
  ml ,
  piece,
}

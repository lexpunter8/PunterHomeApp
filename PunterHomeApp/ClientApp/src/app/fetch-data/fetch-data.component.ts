import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public products: IProduct[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<IProduct[]>(baseUrl + 'api/product').subscribe(result => {
      this.products = result;
    }, error => console.error(error));
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

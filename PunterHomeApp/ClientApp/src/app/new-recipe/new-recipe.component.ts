import { Component, OnInit } from '@angular/core';
import { IRecipe } from '../models/Recipe';
import { IProduct } from '../models/product';
import { ProductService } from '../product.service';
import { Observable } from 'rxjs';
import { EUnitVolumeType } from '../fetch-data/fetch-data.component';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-new-recipe',
  templateUrl: './new-recipe.component.html',
  styleUrls: ['./new-recipe.component.css']
})
export class NewRecipeComponent implements OnInit {
    products: IProduct[];

  constructor(private productService: ProductService) {
    productService.getProducts().subscribe(data => this.productSearchResult = data);
  }

  ngOnInit() {
    this.unitTypeOptions = Object.keys(EUnitVolumeType);
  }
  public productSearchResult: IProduct[];
  public newRecipe: IRecipe = { name: "", ingredients: [], steps: [] }
  public newStep: string = "";
  public productSearchText: string = "";


  public unitTypes = EUnitVolumeType;
  public unitTypeOptions = [];

  addStep(step: string): void {
    this.newRecipe.steps.push(step);
  }

  addProduct(product: IProduct) {
    console.log("added producy: " + product.name)
    this.newRecipe.ingredients.push({ productId: product.id, productName: product.name, unitQuantity: 1, unitQunatityType: EUnitVolumeType.Kg });
  }

  searchProduct(searchText: string): void
  {
    this.productService.search(searchText).subscribe(data => this.productSearchResult = data);
  }

  saveRecipe() {
    this.productService.saveRecipe(this.newRecipe).subscribe(
      (val) => {
        console.log('POST call successful value returned in body',
          val);
      },
      response => {
        console.log('POST call in error', response);
      },
      () => {
        console.log('The POST observable is now completed.');

      });
  }
}

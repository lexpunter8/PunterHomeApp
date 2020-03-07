import { Component, OnInit } from '@angular/core';
import { IRecipe } from '../models/Recipe';
import { IProduct } from '../models/product';
import { ProductService } from '../product.service';
import { Observable } from 'rxjs';

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
  }
  public productSearchResult: IProduct[];
  public newRecipe: IRecipe = { name: "", ingredients: [], steps: [] }
  public newStep: string = "";
  public productSearchText: string = "";

  addStep(step: string): void {
    this.newRecipe.steps.push(step);
  }

  searchProduct(searchText: string): void
  {
    this.productService.search(searchText).subscribe(data => this.productSearchResult = data);
  }
}

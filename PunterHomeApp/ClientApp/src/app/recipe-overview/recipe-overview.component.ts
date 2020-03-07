import { Component, OnInit, Inject } from '@angular/core';
import { IRecipe } from '../models/Recipe';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-recipe-overview',
  templateUrl: './recipe-overview.component.html',
  styleUrls: ['./recipe-overview.component.css']
})
export class RecipeOverviewComponent implements OnInit {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.getRecipes();
  }
  ngOnInit() {
  }

  recipes: IRecipe[];
  selectedRecipe: IRecipe;

  getRecipes(): void {
    console.log('get recipes');
    this.http.get<IRecipe[]>(this.baseUrl + 'api/recipe').subscribe(result => {
      this.recipes = result;
    }, error => console.error(error));
  }

  onSelect(recipe: IRecipe): void {
    this.selectedRecipe = recipe;
  }
}

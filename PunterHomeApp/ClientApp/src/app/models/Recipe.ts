import { EUnitVolumeType } from "../fetch-data/fetch-data.component";

export interface IIgredient {
  productId: string;
  productName: string;
  unitQuantity: number;
  unitQunatityType: EUnitVolumeType;
}
export interface IRecipe {
  name: string;
  steps: string[];
  ingredients: IIgredient[];
}
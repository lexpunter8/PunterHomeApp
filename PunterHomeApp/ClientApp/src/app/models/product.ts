import { EUnitVolumeType } from "../fetch-data/fetch-data.component";

export interface IProduct {
  id: string;
  name: string;
  quantity: string;
  unitQuantity: string;
  unitQuantityType: EUnitVolumeType;
}
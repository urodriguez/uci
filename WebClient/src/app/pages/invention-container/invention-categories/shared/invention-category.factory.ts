import { Injectable } from '@angular/core';
import {InventionCategory} from './invention-category.model';
import {CrudModelFactory} from '../../../../shared/factories/crud-model.factory';

@Injectable({
  providedIn: 'root'
})
export class InventionCategoryFactory extends CrudModelFactory<InventionCategory>{
  create(jsonData: any): InventionCategory {
    return jsonData ? new InventionCategory(
      jsonData.id,
      jsonData.code,
      jsonData.name,
      jsonData.description
    ) : new InventionCategory();
  }
}

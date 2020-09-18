import { Injectable } from '@angular/core';
import {InventionCategory} from './invention-category.model';

@Injectable({
  providedIn: 'root'
})
export class InventionCategoryFactory {

  constructor() { }

  create(jsonData: any): InventionCategory {
    return jsonData ? new InventionCategory(
      jsonData.id,
      jsonData.code,
      jsonData.name,
      jsonData.description
    ) : new InventionCategory();
  }

  createList(jsonDataList: any): InventionCategory[] {
    return jsonDataList ? jsonDataList.map(ud => this.create(ud)) : [];
  }
}

import { Injectable } from '@angular/core';
import {InventionCategory} from './invention-category.model';

@Injectable({
  providedIn: 'root',
})
export class InventionCategoryService {

  constructor() { }

  getAll(): InventionCategory[] {
    const inventionCategories = [];

    inventionCategories.push(
      new InventionCategory('1', 'code01', 'clothes', 'to dress people'));

    inventionCategories.push(
      new InventionCategory('2', 'code02', 'food', 'to dress people'));

    inventionCategories.push(
      new InventionCategory('3', 'code03', 'technology', 'to move forward to the future'));

    inventionCategories.push(
      new InventionCategory('4', 'code04', 'other', 'other'));

    return inventionCategories;
  }
}

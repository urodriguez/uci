import { Injectable } from '@angular/core';
import {InventionCategory} from './invention-category.model';
import {HttpClient} from '@angular/common/http';
import {AppContext} from '../../../../app-context';
import {CrudService} from '../../../../shared/services/crud.service';
import {InventionCategoryFactory} from './invention-category.factory';

@Injectable({
  providedIn: 'root',
})
export class InventionCategoryService extends CrudService<InventionCategory>{
  constructor(httpClient: HttpClient,
              appContext: AppContext,
              factory: InventionCategoryFactory) {
    super(httpClient, appContext, factory);
    this.setResource('InventionCategories');
  }
}

import { Injectable } from '@angular/core';
import {Invention} from './invention.model';
import {CrudModelFactory} from '../../../../shared/factories/crud-model.factory';

@Injectable({
  providedIn: 'root'
})
export class InventionFactory extends CrudModelFactory<Invention>{
  create(jsonData: any): Invention {
    return jsonData ? new Invention(
      jsonData.id,
      jsonData.inventorName,
      jsonData.code,
      jsonData.name,
      jsonData.description,
      jsonData.categoryName,
      jsonData.price,
      jsonData.enable,
    ) : new Invention();
  }
}

import {CrudModel} from '../../../../shared/models/crud.model';

export class InventionCategory extends CrudModel{
  constructor(id?: string,
              public code?: string,
              public name?: string,
              public description?: string) {
    super(id, InventionCategory.name);
  }
}

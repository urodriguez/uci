import {CrudModel} from '../../../../shared/models/crud.model';

export class Invention extends CrudModel {
  constructor(id?: string,
              public inventorName?: string,
              public code?: string,
              public name?: string,
              public description?: string,
              public categoryName?: string,
              public price?: string,
              public enable?: boolean
  ) {
    super(id, Invention.name);
  }
}

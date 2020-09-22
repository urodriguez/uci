import {CrudModel} from '../models/crud.model';

export abstract class CrudModelFactory<T extends CrudModel> {
  abstract create(jsonData: any): T;

  createList(jsonDataList: any): T[] {
    return jsonDataList ? jsonDataList.map(ud => this.create(ud)) : [];
  }
}

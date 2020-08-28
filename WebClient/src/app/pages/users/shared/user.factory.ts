import { Injectable } from '@angular/core';
import {User} from './user.model';

@Injectable({
  providedIn: 'root'
})
export class UserFactory {

  constructor() { }

  create(jsonData: any): User {
    return jsonData ? new User(
      jsonData.id,
      jsonData.email,
      jsonData.firstName,
      jsonData.middleName,
      jsonData.lastName,
      jsonData.role,
      jsonData.active
    ) : new User();
  }

  createList(jsonDataList: any): User[] {
    return jsonDataList ? jsonDataList.map(ud => this.create(ud)) : [];
  }
}

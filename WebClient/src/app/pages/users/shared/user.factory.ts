import { Injectable } from '@angular/core';
import {User} from './user.model';
import {CrudModelFactory} from '../../../shared/factories/crud-model.factory';

@Injectable({
  providedIn: 'root'
})
export class UserFactory extends CrudModelFactory<User>{
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
}

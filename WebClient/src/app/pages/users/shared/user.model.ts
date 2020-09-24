import {UserRole} from './user-role.enum';
import {CrudModel} from '../../../shared/models/crud.model';

export class User extends CrudModel {
  roleName: string;

  constructor(id?: string,
              public email?: string,
              public firstName?: string,
              public middleName?: string,
              public lastName?: string,
              public role?: UserRole,
              public active?: boolean) {
    super(id, User.name);
    this.roleName = UserRole[role]; 
  }

  get fullName(): string {
    return `${this.firstName} ${this.lastName}`;
  }

  get picture(): string {
    return this.firstName === 'Invent' ? 'assets/images/admin.png' : 'assets/images/uciel.png';
  }
}

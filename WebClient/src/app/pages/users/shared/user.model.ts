import {UserRole} from './user-role.enum';

export class User {
  constructor(public id?: string,
              public email?: string,
              public firstName?: string,
              public middleName?: string,
              public lastName?: string,
              public role?: UserRole,
              public active?: boolean) {
  }

  get fullName(): string {
    return `${this.firstName} ${this.lastName}`;
  }

  get picture(): string {
    return 'assets/images/uciel.png';
  }
}

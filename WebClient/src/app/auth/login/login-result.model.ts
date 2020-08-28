import {LoginStatus} from './login-status.enum';
import {SecurityToken} from '../shared/security-token.model';

export class LoginResult {
  constructor(public status?: LoginStatus,
              public securityToken?: SecurityToken) {
  }
}

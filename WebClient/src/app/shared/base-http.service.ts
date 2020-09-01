import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {throwError} from 'rxjs';
import {AppContext} from '../app-context';

export class BaseHttpService {
  protected readonly baseApiURL: string;
  protected httpHeaders: HttpHeaders;

  constructor(protected readonly httpClient: HttpClient,
              protected readonly appContext: AppContext) {
    if (window.location.hostname === 'localhost' || window.location.hostname === 'www.ucirod.inventapp-dev.com') {
      this.baseApiURL = 'http://www.ucirod.inventapp-dev.com:8080/WebApi/api/v1.0';
    } else {
      this.baseApiURL = 'http://152.171.94.90:8080/WebApi/api/v1.0';
    }
    console.log('using baseApiURL = ' + this.baseApiURL);

    if (appContext.securityToken)
      this.initializeHttpHeadersWithToken(appContext.securityToken.token);
    else
      appContext.securityTokenChanged.subscribe(
        st => this.initializeHttpHeadersWithToken(st.token)
      );
  }

  private initializeHttpHeadersWithToken(token: string) {
    this.httpHeaders = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': token
    });
  }

  protected getHttpOptions(): {} {
    return {
      headers: this.httpHeaders
    };
  }

  protected handleError(httpErrorResponse: HttpErrorResponse) {
    // return an observable with a user-facing error message
    return throwError(httpErrorResponse.error);
  }
}

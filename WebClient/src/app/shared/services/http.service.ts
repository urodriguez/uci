import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {throwError} from 'rxjs';
import {AppContext} from '../../app-context';

export class HttpService {
  protected readonly baseApiURL: string;
  protected httpHeaders: HttpHeaders;

  protected resourceApiUrl: string;

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

  setResource(resource: string): void {
    this.resourceApiUrl = `${this.baseApiURL}/${resource}`;
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
    // returns an observable with a user-facing error message
    return httpErrorResponse.status !== 401 ? throwError(httpErrorResponse.error) : throwError('UNAUTHENTICATED');
  }
}

import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import {catchError, map} from 'rxjs/operators';
import {AppContext} from '../../app-context';
import {CrudModel} from '../models/crud.model';
import {HttpService} from './http.service';
import {CrudModelFactory} from '../factories/crud-model.factory';

export class CrudService<T extends CrudModel> extends HttpService {

  constructor(httpClient: HttpClient,
              appContext: AppContext,
              protected readonly factory: CrudModelFactory<T>) {
    super(httpClient, appContext);
  }

  getById(id: string): Observable<T> {
    return this.httpClient.get<T>(
      `${this.resourceApiUrl}/${id}`,
      this.getHttpOptions()
    ).pipe(
      map(response => this.factory.create(response)),
      catchError(this.handleError)
    );
  }

  getAll(): Observable<T[]> {
    return this.httpClient.get<T[]>(
      this.resourceApiUrl,
      this.getHttpOptions()
    ).pipe(
      map(response => this.factory.createList(response)),
      catchError(this.handleError)
    );
  }

  create(crudModel: T): Observable<string> {
    return this.httpClient.post<string>(
      this.resourceApiUrl,
      crudModel,
      this.getHttpOptions()
    ).pipe(
      catchError(this.handleError)
    );
  }

  update(crudModel: T) {
    return this.httpClient.put(
      `${this.resourceApiUrl}/${crudModel.id}`,
      crudModel,
      this.getHttpOptions()
    ).pipe(
        catchError(this.handleError)
    );
  }

  delete(crudModel: T) {
    return this.httpClient.delete(
      `${this.resourceApiUrl}/${crudModel.id}`,
      this.getHttpOptions()
    ).pipe(
        catchError(this.handleError)
    );
  }
}

import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';

import {Invention} from './invention.model';
import {InventionFactory} from './invention.factory';
import {AppContext} from '../../../../app-context';
import {CrudService} from '../../../../shared/services/crud.service';
import {catchError} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class InventionService extends CrudService<Invention> {

  constructor(httpClient: HttpClient,
              appContext: AppContext,
              factory: InventionFactory) {
    super(httpClient, appContext, factory);
    this.setResource('inventions');
  }

  updateState(model: Invention) {
    return this.httpClient.patch(
      `${this.resourceApiUrl}/${model.id}`,
      model,
      this.getHttpOptions()
    ).pipe(
      catchError(this.handleError)
    );
  }
}

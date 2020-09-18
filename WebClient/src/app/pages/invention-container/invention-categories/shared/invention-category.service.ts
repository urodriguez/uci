import { Injectable } from '@angular/core';
import {InventionCategory} from './invention-category.model';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {AppContext} from '../../../../app-context';
import {catchError, map} from 'rxjs/operators';
import {BaseHttpService} from '../../../../shared/base-http.service';
import {InventionCategoryFactory} from './invention-category.factory';

@Injectable({
  providedIn: 'root',
})
export class InventionCategoryService extends BaseHttpService{

  private readonly inventionCategoriesApiURL = `${this.baseApiURL}/inventionCategories`;

  constructor(httpClient: HttpClient,
              appContext: AppContext,
              private readonly inventionCategoryFactory: InventionCategoryFactory) {
    super(httpClient, appContext);
  }

  getById(id: string): Observable<InventionCategory> {
    return this.httpClient.get<InventionCategory>(`${this.inventionCategoriesApiURL}/${id}`, this.getHttpOptions()).pipe(
      map(response => this.inventionCategoryFactory.create(response)),
      catchError(this.handleError)
    );
  }

  getAll(): Observable<InventionCategory[]> {
    return this.httpClient.get<InventionCategory[]>(this.inventionCategoriesApiURL, this.getHttpOptions()).pipe(
      map(response => this.inventionCategoryFactory.createList(response)),
      catchError(this.handleError)
    );
  }

  create(inventionCategory: InventionCategory): Observable<string> {
    return this.httpClient.post<string>(this.inventionCategoriesApiURL, inventionCategory, this.getHttpOptions()).pipe(
      catchError(this.handleError)
    );
  }

  update(inventionCategory: InventionCategory) {
    return this.httpClient.put(
      `${this.inventionCategoriesApiURL}/${inventionCategory.id}`,
      inventionCategory,
      this.getHttpOptions()
    ).pipe(
      catchError(this.handleError)
    );
  }

  delete(inventionCategory: InventionCategory) {
    return this.httpClient.delete(`${this.inventionCategoriesApiURL}/${inventionCategory.id}`, this.getHttpOptions())
      .pipe(
        catchError(this.handleError)
      );
  }
}

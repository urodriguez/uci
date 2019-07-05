import { Injectable } from '@angular/core';
import { Product } from './product.model';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly baseApiURL = `http://localhost:53444/api`;
  private readonly productsApiURL = `${this.baseApiURL}/products`;

  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    })
  };

  constructor(private readonly http: HttpClient) { }

  getAll() {
    return this.http.get(this.productsApiURL);
  }

  create(product: Product) {
    return this.http.post<Product>(this.productsApiURL, product, this.httpOptions)
                    .pipe(
                      catchError(this.handleError)
                    );
  }

  update(product: Product) {
    return this.http.put<Product>(`${this.productsApiURL}/${product.id}`, product, this.httpOptions)
                    .pipe(
                      catchError(this.handleError)
                    );
  }

  delete(id: number) {
    return this.http.delete(`${this.productsApiURL}/${id}`, this.httpOptions)
                    .pipe(
                      catchError(this.handleError)
                    );
  }

  deleteBulk(ids: number[]) {
    return this.http.delete(`${this.productsApiURL}?ids=${ids.join(';')}`, this.httpOptions)
                    .pipe(
                      catchError(this.handleError)
                    );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  }
}

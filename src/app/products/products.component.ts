import { Component, OnInit, ViewChild  } from '@angular/core';
import { ProductService } from './shared/product.service';
import { Product } from './shared/product.model';
import { AgGridAngular } from 'ag-grid-angular';

@Component({
  selector: 'app-product',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  @ViewChild('agGrid', {static: false}) agGrid: AgGridAngular;

  columnDefs: Array<any>;
  rowData: Array<Product>;

  product: Product;

  constructor(private readonly productService: ProductService) {
  }

  ngOnInit() {
    this.columnDefs = [
      {headerName: 'Name', field: 'name', sortable: true, filter: true, checkboxSelection: true},
      {headerName: 'Category', field: 'category', sortable: true, filter: true},
      {headerName: 'Price', field: 'price', sortable: true, filter: true}
    ];

    this.productService
        .getAll()
        .subscribe((products: Product[]) => this.rowData = products);

    this.product = new Product();
  }

  create() {
    this.product = new Product();
    $('#modal').modal();
    $('#modal').modal('open');
  }

  update() {
    const selectedNodes = this.agGrid.api.getSelectedNodes();
    const products = selectedNodes.map( node => node.data );

    if (products.length !== 1) {
      Materialize.toast('Select only one element!', 3000, 'rounded');
    } else {
      $('#modal').modal();
      $('#modal').modal('open');
      this.product = products[0];
    }
  }

  deleteBulk() {
    const selectedNodes = this.agGrid.api.getSelectedNodes();
    const products = selectedNodes.map( node => node.data );

    if (products.length === 0) {
      Materialize.toast('Select at least one element!', 3000, 'rounded');
    } else {
      this.productService
          .deleteBulk(products.map(p => p.id))
          .subscribe(resp => {
            this.rowData = this.rowData.filter(el => !products.includes(el));
          });
    }
  }

  save() {
    if (this.product.id !== undefined) {
      this.productService
          .update(this.product)
          .subscribe(p => {
            const index = this.rowData.findIndex((e) => e.id === p.id);
            this.rowData[index] = p;
            this.rowData = Object.assign([], this.rowData );
          });
    } else {
      this.productService
          .create(this.product)
          .subscribe(p => this.rowData = this.rowData.concat([p]));
    }
  }
}

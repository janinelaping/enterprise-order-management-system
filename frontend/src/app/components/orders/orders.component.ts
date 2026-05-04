import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../core/order.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent implements OnInit {

  orders: any[] = [];

  newOrder = {
    customerName: '',
    productName: '',
    quantity: 1,
    price: 0,
    createdBy: 'admin'
  };

  editingOrderId: number | null = null;

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  editOrder(order: any) {
    this.newOrder = { ...order };
    this.editingOrderId = order.id;
  }
  loadOrders() {
    this.orderService.getOrders().subscribe(data => {
      this.orders = data;
    });
  }

  saveOrder() {
    if (this.editingOrderId) {
      this.orderService.updateOrder(this.editingOrderId, this.newOrder)
        .subscribe(() => {
          this.loadOrders();
          this.resetForm();
        });
    } else {
      this.orderService.createOrder(this.newOrder)
        .subscribe(() => {
          this.loadOrders();
          this.resetForm();
        });
    }
  }

  deleteOrder(id: number) {
    this.orderService.deleteOrder(id).subscribe(() => {
      this.loadOrders();
    });
  }

  resetForm() {
    this.newOrder = {
      customerName: '',
      productName: '',
      quantity: 1,
      price: 0,
      createdBy: 'admin'
    };
  }
}

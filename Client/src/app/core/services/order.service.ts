import {
  Injectable,
  inject,
} from '@angular/core';

import {
  HttpClient,
} from '@angular/common/http';

import {
  Observable,
  timeout,
} from 'rxjs';

import {
  CreateOrderRequest,
  OrderResponse,
} from '../../models/order.model';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5223/api';

  private readonly requestTimeout = 10000;

  createOrder(
    payload: CreateOrderRequest
  ): Observable<OrderResponse> {
    return this.http
      .post<OrderResponse>(
        `${this.apiUrl}/orders`,
        payload
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }

  getOrderById(
    orderId: number
  ): Observable<OrderResponse> {
    return this.http
      .get<OrderResponse>(
        `${this.apiUrl}/orders/${orderId}`
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }
}

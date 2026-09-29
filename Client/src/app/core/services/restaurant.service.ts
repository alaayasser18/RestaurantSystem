import {
  Injectable,
  inject,
} from '@angular/core';

import {
  HttpClient,
  HttpParams,
} from '@angular/common/http';

import {
  Observable,
  timeout,
} from 'rxjs';

import {
  CitiesResponse,
  RestaurantsResponse,
  StatsResponse,
} from '../../models/restaurant.model';

import {
  RestaurantDetailsResponse,
  RestaurantMenuResponse,
} from '../../models/menu-item.model';

import {
  CreateOrderRequest,
  OrderResponse,
} from '../../models/order.model';

@Injectable({
  providedIn: 'root',
})
export class RestaurantService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5223/api';

  private readonly requestTimeout = 10000;

  private readonly defaultPageSize = 6;

  // ==========================================
  // Restaurants
  // ==========================================

  getRestaurants(
    page = 1,
    pageSize = this.defaultPageSize
  ): Observable<RestaurantsResponse> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http
      .get<RestaurantsResponse>(
        `${this.apiUrl}/restaurants`,
        { params }
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }

  searchRestaurants(
    search: string,
    page = 1,
    pageSize = this.defaultPageSize
  ): Observable<RestaurantsResponse> {
    const params = new HttpParams()
      .set('search', search.trim())
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http
      .get<RestaurantsResponse>(
        `${this.apiUrl}/restaurants/search`,
        { params }
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }

  getRestaurantsByCity(
    cityId: number,
    page = 1,
    pageSize = this.defaultPageSize
  ): Observable<RestaurantsResponse> {
    const params = new HttpParams()
      .set('cityId', cityId)
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http
      .get<RestaurantsResponse>(
        `${this.apiUrl}/restaurants/city`,
        { params }
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }

  searchRestaurantsByNameAndCity(
    search: string,
    cityId: number,
    page = 1,
    pageSize = this.defaultPageSize
  ): Observable<RestaurantsResponse> {
    const params = new HttpParams()
      .set('search', search.trim())
      .set('cityId', cityId)
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http
      .get<RestaurantsResponse>(
        `${this.apiUrl}/restaurants/search-and-city`,
        { params }
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }

  // ==========================================
  // Cities
  // ==========================================

  getCities(): Observable<CitiesResponse> {
    return this.http
      .get<CitiesResponse>(
        `${this.apiUrl}/cities`
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }

  // ==========================================
  // Home Statistics
  // ==========================================

  getStats(): Observable<StatsResponse> {
    return this.http
      .get<StatsResponse>(
        `${this.apiUrl}/home/stats`
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }

  // ==========================================
  // Restaurant Details
  // ==========================================

  getRestaurantById(
    restaurantId: number
  ): Observable<RestaurantDetailsResponse> {
    return this.http
      .get<RestaurantDetailsResponse>(
        `${this.apiUrl}/restaurants/${restaurantId}`
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }

  // ==========================================
  // Restaurant Menu
  // ==========================================

  getRestaurantMenu(
    restaurantId: number,
    page = 1,
    pageSize = this.defaultPageSize
  ): Observable<RestaurantMenuResponse> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http
      .get<RestaurantMenuResponse>(
        `${this.apiUrl}/restaurants/${restaurantId}/menu`,
        { params }
      )
      .pipe(
        timeout(this.requestTimeout)
      );
  }

  // ==========================================
  // Orders
  // ==========================================

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
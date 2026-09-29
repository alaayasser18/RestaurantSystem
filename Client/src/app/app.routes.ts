import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',

    loadComponent: () =>
      import('./layout/layout').then(
        (m) => m.Layout
      ),

    children: [
      {
        path: '',

        redirectTo: 'restaurants',

        pathMatch: 'full',
      },

      {
        path: 'restaurants',

        loadComponent: () =>
          import('./pages/restaurants/restaurants').then(
            (m) => m.Restaurants
          ),
      },

      {
        path: 'restaurants/:id',

        loadComponent: () =>
          import('./pages/restaurant-details/restaurant-details').then(
            (m) => m.RestaurantDetailsComponent
          ),
      },

      {
        path: 'orders',

        loadComponent: () =>
          import('./pages/orders/orders').then(
            (m) => m.Orders
          ),
      },

      {
        path: 'orders/:orderId',

        loadComponent: () =>
          import('./pages/orders/orders').then(
            (m) => m.Orders
          ),
      },
    ],
  },

  {
    path: '**',

    redirectTo: 'restaurants',
  },
];
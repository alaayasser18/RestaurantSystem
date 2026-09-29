import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject,
} from '@angular/core';

import {
  DatePipe,
} from '@angular/common';

import {
  FormsModule,
  NgForm,
} from '@angular/forms';

import {
  ActivatedRoute,
  ParamMap,
  Router,
  RouterLink,
} from '@angular/router';

import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { RestaurantService } from '../../core/services/restaurant.service';
import { CreateOrderRequest, OrderDto } from '../../models/order.model';
import { RestaurantDetails } from '../../models/menu-item.model';

interface MenuItem {
  id: number;
  name: string;
  price: number;
  image: string;
}

interface Customer {
  name: string;
  phone: string;
  email: string;
  address: string;
}

interface OrderItem {
  item: MenuItem;
  quantity: number;
}

@Component({
  selector: 'app-orders',
  imports: [FormsModule, DatePipe, RouterLink],
  templateUrl: './orders.html',
})
export class Orders implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly restaurantService = inject(RestaurantService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  restaurantId = 1;

  restaurant: RestaurantDetails | null = null;

  currentStep = 3;

  selectedItems: number[] = [];

  submitted = false;

  checkoutCompleted = false;

  isSubmittingOrder = false;

  orderError = '';

  createdOrder: OrderDto | null = null;

  viewedOrder: OrderDto | null = null;

  isLoadingOrder = false;

  orderLookupError = '';

  customer: Customer = {
    name: '',
    phone: '',
    email: '',
    address: '',
  };

  menuItems: MenuItem[] = [
    {
      id: 1,
      name: 'Creamy Pasta',
      price: 185,
      image:
        'https://images.unsplash.com/photo-1551892374-ecf8754cf8b0?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 2,
      name: 'Margherita Pizza',
      price: 220,
      image:
        'https://images.unsplash.com/photo-1574071318508-1cdbab80d002?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 3,
      name: 'Grilled Chicken',
      price: 260,
      image:
        'https://images.unsplash.com/photo-1532550907401-a500c9a57435?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 4,
      name: 'Beef Burger',
      price: 235,
      image:
        'https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 5,
      name: 'Caesar Salad',
      price: 165,
      image:
        'https://images.unsplash.com/photo-1550304943-4f24f54ddde9?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 6,
      name: 'Seafood Risotto',
      price: 310,
      image:
        'https://images.unsplash.com/photo-1476124369491-e7addf5db371?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 7,
      name: 'Chicken Alfredo',
      price: 210,
      image:
        'https://images.unsplash.com/photo-1645112411341-6c4fd023714a?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 8,
      name: 'Four Cheese Pizza',
      price: 255,
      image:
        'https://images.unsplash.com/photo-1579751626657-72bc17010498?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 9,
      name: 'Steak Plate',
      price: 390,
      image:
        'https://images.unsplash.com/photo-1544025162-d76694265947?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 10,
      name: 'Chocolate Cake',
      price: 140,
      image:
        'https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 11,
      name: 'Tiramisu',
      price: 150,
      image:
        'https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?auto=format&fit=crop&w=500&q=85',
    },
    {
      id: 12,
      name: 'Fresh Lemonade',
      price: 85,
      image:
        'https://images.unsplash.com/photo-1523677011781-c91d1bbe2f9e?auto=format&fit=crop&w=500&q=85',
    },
  ];

  orderItems: OrderItem[] = [];

  ngOnInit(): void {
    const routeOrderId = Number(this.route.snapshot.paramMap.get('orderId'));
    const queryOrderId = Number(this.route.snapshot.queryParamMap.get('orderId'));
    const orderIdToLoad = routeOrderId || queryOrderId;

    if (orderIdToLoad && orderIdToLoad > 0) {
      this.loadOrderById(orderIdToLoad);
      return;
    }

    this.loadCustomerInfo();

    this.route.queryParamMap.subscribe((params) => {
      const qOrderId = Number(params.get('orderId'));
      if (qOrderId && qOrderId > 0) {
        this.loadOrderById(qOrderId);
        return;
      }

      let restaurantId = Number(params.get('restaurantId'));
      if (!restaurantId || isNaN(restaurantId)) {
        try {
          const savedActiveId = localStorage.getItem('restaurant_last_active_id');
          if (savedActiveId) {
            restaurantId = Number(savedActiveId);
          }
        } catch (_) {}
      }

      if (restaurantId && !isNaN(restaurantId) && restaurantId > 0) {
        this.restaurantId = restaurantId;
        try {
          localStorage.setItem('restaurant_last_active_id', String(this.restaurantId));
        } catch (_) {}
      }

      // Check for previously placed order
      try {
        const savedOrder =
          localStorage.getItem(`restaurant_${this.restaurantId}_created_order`) ||
          localStorage.getItem('last_placed_order');
        if (savedOrder) {
          this.createdOrder = JSON.parse(savedOrder);
          const modalState = localStorage.getItem(`restaurant_${this.restaurantId}_modal_open`);
          this.checkoutCompleted = modalState === 'true';
        }
      } catch (_) {}

      const items = params.get('items');
      const step = Number(params.get('step'));

      if (items) {
        this.selectedItems = items
          .split(',')
          .map(Number)
          .filter(Boolean);
      } else {
        try {
          const saved = localStorage.getItem(`restaurant_${this.restaurantId}_selected_data`);
          if (saved) {
            const parsed = JSON.parse(saved);
            if (Array.isArray(parsed.items)) {
              this.selectedItems = parsed.items;
            }
          }
        } catch (_) {}
      }

      // Fallback items if an order exists
      if (this.selectedItems.length === 0 && this.createdOrder && this.createdOrder.items) {
        this.selectedItems = this.createdOrder.items.map((i) => i.menuItemId || i.id);
      }

      if (step === 4 || step === 3) {
        this.currentStep = step;
      } else {
        try {
          const savedStep = localStorage.getItem('restaurant_orders_step');
          this.currentStep = savedStep === '4' ? 4 : 3;
        } catch (_) {
          this.currentStep = 3;
        }
      }

      this.buildOrderItems();

      if (this.selectedItems.length === 0 && !this.createdOrder) {
        this.router.navigate(['/restaurants']);
        return;
      }

      this.loadCustomerFromParams(params);

      if (this.restaurantId) {
        try {
          const cachedRestaurant = localStorage.getItem(`restaurant_${this.restaurantId}_details`);
          if (cachedRestaurant) {
            this.restaurant = JSON.parse(cachedRestaurant);
          }
        } catch (_) {}

        this.restaurantService
          .getRestaurantById(this.restaurantId)
          .pipe(catchError(() => of(null)))
          .subscribe((res) => {
            if (res && res.success && res.data) {
              this.restaurant = res.data;
              try {
                localStorage.setItem(
                  `restaurant_${this.restaurantId}_details`,
                  JSON.stringify(this.restaurant)
                );
              } catch (_) {}
              this.cdr.markForCheck();
            }
          });

        this.restaurantService
          .getRestaurantMenu(this.restaurantId, 1, 50)
          .pipe(catchError(() => of(null)))
          .subscribe((res) => {
            if (res && res.data) {
              if (res.data.restaurant && !this.restaurant) {
                this.restaurant = res.data.restaurant;
              }
              if (res.data.items) {
                const apiItems = res.data.items.map((m) => ({
                  id: m.id,
                  name: m.name,
                  price: m.price,
                  image: m.imageUrl,
                }));
                for (const item of apiItems) {
                  const idx = this.menuItems.findIndex((m) => m.id === item.id);
                  if (idx >= 0) {
                    this.menuItems[idx] = item;
                  } else {
                    this.menuItems.push(item);
                  }
                }
                this.buildOrderItems();
                this.cdr.markForCheck();
              }
            }
          });
      }

      this.cdr.markForCheck();
    });
  }

  saveCustomerInfo(): void {
    try {
      localStorage.setItem('restaurant_customer_info', JSON.stringify(this.customer));
    } catch (_) {}
  }

  private loadCustomerInfo(): void {
    try {
      const saved = localStorage.getItem('restaurant_customer_info');
      if (saved) {
        const parsed = JSON.parse(saved);
        if (parsed && typeof parsed === 'object') {
          if (parsed.name) this.customer.name = parsed.name;
          if (parsed.phone) this.customer.phone = parsed.phone;
          if (parsed.email) this.customer.email = parsed.email;
          if (parsed.address) this.customer.address = parsed.address;
        }
      }
    } catch (_) {}
  }

  private saveOrderQuantities(): void {
    try {
      const quantities: Record<number, number> = {};
      for (const oi of this.orderItems) {
        quantities[oi.item.id] = oi.quantity;
      }
      localStorage.setItem(
        `restaurant_${this.restaurantId}_quantities`,
        JSON.stringify(quantities)
      );
    } catch (_) {}
  }

  private buildOrderItems(): void {
    let savedDetails: Record<number, any> = {};
    let savedPrices: Record<number, number> = {};
    let savedQuantities: Record<number, number> = {};

    try {
      const saved = localStorage.getItem(`restaurant_${this.restaurantId}_selected_data`);
      if (saved) {
        const parsed = JSON.parse(saved);
        if (parsed.itemDetails) savedDetails = parsed.itemDetails;
        if (parsed.prices) savedPrices = parsed.prices;
      }
    } catch (_) {}

    try {
      const qSaved = localStorage.getItem(`restaurant_${this.restaurantId}_quantities`);
      if (qSaved) {
        savedQuantities = JSON.parse(qSaved);
      }
    } catch (_) {}

    if (this.createdOrder && this.createdOrder.items) {
      for (const item of this.createdOrder.items) {
        const itemId = item.menuItemId || item.id;
        if (!savedDetails[itemId]) {
          savedDetails[itemId] = {
            id: itemId,
            name: item.name,
            price: item.unitPrice,
            image: item.imageUrl,
          };
        }
        if (!savedQuantities[itemId]) {
          savedQuantities[itemId] = item.quantity;
        }
      }
    }

    this.orderItems = this.selectedItems.map((id) => {
      const existing = this.orderItems.find((oi) => oi.item.id === id);
      const quantity = existing ? existing.quantity : (savedQuantities[id] || 1);

      const found = this.menuItems.find((m) => m.id === id);
      if (found) {
        return { item: found, quantity };
      }

      if (savedDetails[id]) {
        return {
          item: {
            id,
            name: savedDetails[id].name || `Item #${id}`,
            price: savedDetails[id].price || 0,
            image:
              savedDetails[id].image ||
              'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?auto=format&fit=crop&w=500&q=85',
          },
          quantity,
        };
      }

      return {
        item: {
          id,
          name: `Menu Item #${id}`,
          price: savedPrices[id] || 0,
          image:
            'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?auto=format&fit=crop&w=500&q=85',
        },
        quantity,
      };
    });

    this.saveOrderQuantities();
  }

  private loadCustomerFromParams(params: ParamMap): void {
    const name = params.get('name');
    const phone = params.get('phone');
    const email = params.get('email');
    const address = params.get('address');

    if (name) {
      this.customer.name = name;
    }

    if (phone) {
      this.customer.phone = phone;
    }

    if (email) {
      this.customer.email = email;
    }

    if (address) {
      this.customer.address = address;
    }

    this.saveCustomerInfo();
  }

  get subtotal(): number {
    return this.orderItems.reduce(
      (total, orderItem) =>
        total + orderItem.item.price * orderItem.quantity,
      0
    );
  }

  get deliveryFee(): number {
    return 0;
  }

  get grandTotal(): number {
    return this.subtotal + this.deliveryFee;
  }

  increaseQuantity(itemId: number): void {
    const orderItem = this.orderItems.find((order) => order.item.id === itemId);

    if (!orderItem) {
      return;
    }

    orderItem.quantity += 1;
    this.saveOrderQuantities();
    this.cdr.markForCheck();
  }

  decreaseQuantity(itemId: number): void {
    const orderItem = this.orderItems.find((order) => order.item.id === itemId);

    if (!orderItem) {
      return;
    }

    if (orderItem.quantity > 1) {
      orderItem.quantity -= 1;
      this.saveOrderQuantities();
      this.cdr.markForCheck();
      return;
    }

    this.orderItems = this.orderItems.filter(
      (order) => order.item.id !== itemId
    );

    this.selectedItems = this.selectedItems.filter((id) => id !== itemId);
    this.saveOrderQuantities();

    try {
      const saved = localStorage.getItem(`restaurant_${this.restaurantId}_selected_data`);
      if (saved) {
        const parsed = JSON.parse(saved);
        parsed.items = this.selectedItems;
        localStorage.setItem(`restaurant_${this.restaurantId}_selected_data`, JSON.stringify(parsed));
      }
    } catch (_) {}

    if (this.orderItems.length === 0 && !this.createdOrder) {
      this.router.navigate(['/restaurants', this.restaurantId]);
      return;
    }

    this.cdr.markForCheck();
  }

  goBackToMenu(): void {
    this.router.navigate(['/restaurants', this.restaurantId]);
  }

  goBackToCustomer(): void {
    this.currentStep = 3;
    try {
      localStorage.setItem('restaurant_orders_step', '3');
    } catch (_) {}

    this.router.navigate(['/orders'], {
      queryParams: {
        restaurantId: this.restaurantId,
        items: this.selectedItems.join(','),
        name: this.customer.name,
        phone: this.customer.phone,
        email: this.customer.email,
        address: this.customer.address,
      },
    });
  }

  goBackToRestaurants(): void {
    this.checkoutCompleted = false;
    try {
      localStorage.setItem(`restaurant_${this.restaurantId}_modal_open`, 'false');
    } catch (_) {}
    this.router.navigate(['/restaurants']);
  }

  closeSuccessModal(): void {
    this.checkoutCompleted = false;
    try {
      localStorage.setItem(`restaurant_${this.restaurantId}_modal_open`, 'false');
    } catch (_) {}
    this.cdr.markForCheck();
  }

  next(form: NgForm): void {
    this.submitted = true;

    if (form.invalid) {
      return;
    }

    this.saveCustomerInfo();
    try {
      localStorage.setItem('restaurant_orders_step', '4');
    } catch (_) {}

    this.router.navigate(['/orders'], {
      queryParams: {
        restaurantId: this.restaurantId,
        items: this.selectedItems.join(','),
        name: this.customer.name,
        phone: this.customer.phone,
        email: this.customer.email,
        address: this.customer.address,
        step: 4,
      },
    });
  }

  checkout(): void {
    if (this.isSubmittingOrder) {
      return;
    }

    if (
      !this.customer.name.trim() ||
      !this.customer.phone.trim() ||
      !this.customer.email.trim() ||
      !this.customer.address.trim()
    ) {
      this.currentStep = 3;
      return;
    }

    if (this.orderItems.length === 0) {
      this.orderError = 'Please select at least one item.';
      return;
    }

    const payload: CreateOrderRequest = {
      restaurantId: this.restaurantId,
      customerName: this.customer.name.trim(),
      customerPhone: this.customer.phone.trim(),
      customerEmail: this.customer.email.trim(),
      customerAddress: this.customer.address.trim(),
      items: this.orderItems.map((oi) => ({
        menuItemId: oi.item.id,
        quantity: oi.quantity,
      })),
    };

    this.isSubmittingOrder = true;
    this.orderError = '';
    this.cdr.markForCheck();

    this.restaurantService
      .createOrder(payload)
      .pipe(
        catchError((error) => {
          console.error('Failed to create order:', error);
          this.orderError =
            error?.error?.message ||
            error?.message ||
            'Unable to place your order. Please check the information and try again.';
          this.isSubmittingOrder = false;
          this.cdr.markForCheck();
          return of(null);
        })
      )
      .subscribe((response) => {
        this.isSubmittingOrder = false;

        if (response && response.success && response.data) {
          this.createdOrder = response.data;
          this.checkoutCompleted = true;

          try {
            localStorage.setItem(
              `restaurant_${this.restaurantId}_created_order`,
              JSON.stringify(this.createdOrder)
            );
            localStorage.setItem('last_placed_order', JSON.stringify(this.createdOrder));
            localStorage.setItem(`restaurant_${this.restaurantId}_modal_open`, 'true');
          } catch (_) {}
        } else if (response && !response.success) {
          this.orderError = response.message || 'Unable to place order.';
        }

        this.cdr.markForCheck();
      });
  }

  loadOrderById(orderId: number): void {
    this.currentStep = 5;
    this.isLoadingOrder = true;
    this.orderLookupError = '';
    this.viewedOrder = null;
    this.cdr.markForCheck();

    this.restaurantService
      .getOrderById(orderId)
      .pipe(
        catchError((error) => {
          console.error('Failed to load order:', error);
          this.orderLookupError =
            error?.error?.message ||
            error?.message ||
            `Order #${orderId} was not found.`;
          this.isLoadingOrder = false;
          this.cdr.markForCheck();
          return of(null);
        })
      )
      .subscribe((response) => {
        this.isLoadingOrder = false;
        if (response && response.success && response.data) {
          this.viewedOrder = response.data;
          this.orderLookupError = '';
        } else if (response && !response.success) {
          this.orderLookupError =
            response.message || `Order #${orderId} not found.`;
        }
        this.cdr.markForCheck();
      });
  }

  onHeaderBackClick(): void {
    if (this.currentStep === 5) {
      this.returnToConfirmation(true);
      return;
    }
    if (this.currentStep === 4) {
      this.goBackToCustomer();
      return;
    }
    this.goBackToMenu();
  }

  returnToConfirmation(reopenModal = true): void {
    this.currentStep = 4;
    try {
      localStorage.setItem('restaurant_orders_step', '4');
    } catch (_) {}

    if (reopenModal && this.createdOrder) {
      this.checkoutCompleted = true;
      try {
        localStorage.setItem(`restaurant_${this.restaurantId}_modal_open`, 'true');
      } catch (_) {}
    } else {
      this.checkoutCompleted = false;
      try {
        localStorage.setItem(`restaurant_${this.restaurantId}_modal_open`, 'false');
      } catch (_) {}
    }

    this.router.navigate(['/orders'], {
      queryParams: {
        restaurantId: this.restaurantId,
        items: this.selectedItems.join(','),
        name: this.customer.name,
        phone: this.customer.phone,
        email: this.customer.email,
        address: this.customer.address,
        step: 4,
      },
    });
    this.cdr.markForCheck();
  }

  viewPlacedOrder(): void {
    if (this.createdOrder) {
      this.checkoutCompleted = false;
      try {
        localStorage.setItem(`restaurant_${this.restaurantId}_modal_open`, 'false');
        localStorage.setItem('restaurant_orders_step', '5');
      } catch (_) {}
      this.viewedOrder = this.createdOrder;
      this.currentStep = 5;
      this.router.navigate(['/orders'], {
        queryParams: {
          restaurantId: this.restaurantId,
          orderId: this.createdOrder.id,
        },
      });
      this.cdr.markForCheck();
    }
  }
}
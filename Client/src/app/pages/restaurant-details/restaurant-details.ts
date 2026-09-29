import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
  inject,
} from '@angular/core';

import {
  ActivatedRoute,
  Router,
} from '@angular/router';

import {
  animate,
  stagger,
} from 'motion';

import {
  Subscription,
  catchError,
  finalize,
  of,
} from 'rxjs';

import {
  RestaurantService,
} from '../../core/services/restaurant.service';

import {
  MenuItem,
  RestaurantDetails,
} from '../../models/menu-item.model';

@Component({
  selector: 'app-restaurant-details',
  imports: [],
  templateUrl: './restaurant-details.html',
})
export class RestaurantDetailsComponent
  implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild('menuSection') menuSection?: ElementRef<HTMLElement>;

  private readonly cdr = inject(ChangeDetectorRef);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly restaurantService = inject(RestaurantService);

  restaurantId = 0;

  currentPage = 1;

  itemsPerPage = 6;

  selectedItems: number[] = [];

  selectedPrices: Record<number, number> = {};

  selectedItemDetails: Record<number, { id: number; name: string; price: number; image: string }> = {};

  showValidation = false;

  isLoading = true;

  isMenuLoading = false;

  errorMessage = '';

  restaurant: RestaurantDetails = {
    id: 0,
    name: '',
    city: {
      id: 0,
      name: '',
    },
    category: '',
    description: '',
    imageUrl: '',
    rating: 0,
    likes: 0,
    comments: 0,
    menuItemCount: 0,
  };

  menuItems: MenuItem[] = [];

  totalPages = 0;

  totalCount = 0;

  hasNextPage = false;

  hasPreviousPage = false;

  private routeSub?: Subscription;

  ngOnInit(): void {
    window.scrollTo({ top: 0, left: 0, behavior: 'instant' });

    const initialId = Number(this.route.snapshot.paramMap.get('id'));
    if (initialId && initialId > 0) {
      this.restaurantId = initialId;
      try {
        localStorage.setItem('restaurant_last_active_id', String(this.restaurantId));
      } catch (_) {}
      this.loadSavedSelections();
      this.loadRestaurant();
      const savedPage = this.getSavedMenuPage(initialId);
      this.loadMenu(savedPage, false);
    }

    this.routeSub = this.route.paramMap.subscribe((params) => {
      const id = Number(params.get('id'));

      if (!id || id <= 0) {
        this.errorMessage = 'Invalid restaurant ID.';
        this.isLoading = false;
        this.isMenuLoading = false;
        this.cdr.markForCheck();
        return;
      }

      if (id !== this.restaurantId) {
        this.restaurantId = id;
        try {
          localStorage.setItem('restaurant_last_active_id', String(this.restaurantId));
        } catch (_) {}
        window.scrollTo({ top: 0, left: 0, behavior: 'instant' });
        this.loadSavedSelections();
        this.loadRestaurant();
        const savedPage = this.getSavedMenuPage(id);
        this.loadMenu(savedPage, false);
      }
    });
  }

  private getSavedMenuPage(id: number): number {
    try {
      const saved = localStorage.getItem(`restaurant_${id}_menu_page`);
      if (saved) {
        const p = Number(saved);
        if (p > 0) return p;
      }
    } catch (_) {}
    return 1;
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.animatePage();
    });
  }

  ngOnDestroy(): void {
    this.routeSub?.unsubscribe();
  }

  get paginatedItems(): MenuItem[] {
    return this.menuItems;
  }

  get selectedCount(): number {
    return this.selectedItems.length;
  }

  get selectedTotal(): number {
    return this.selectedItems.reduce((total, id) => {
      const price =
        this.selectedPrices[id] ??
        this.menuItems.find((m) => m.id === id)?.price ??
        0;
      return total + price;
    }, 0);
  }

  private getStorageKey(id: number): string {
    return `restaurant_${id}_selected_data`;
  }

  private loadSavedSelections(): void {
    if (!this.restaurantId) {
      return;
    }

    try {
      const saved = localStorage.getItem(this.getStorageKey(this.restaurantId));
      if (saved) {
        const parsed = JSON.parse(saved);
        if (parsed && typeof parsed === 'object') {
          if (Array.isArray(parsed.items)) {
            this.selectedItems = parsed.items
              .map(Number)
              .filter((n: number) => !isNaN(n) && n > 0);
          }
          if (parsed.prices && typeof parsed.prices === 'object') {
            this.selectedPrices = parsed.prices;
          }
          if (parsed.itemDetails && typeof parsed.itemDetails === 'object') {
            this.selectedItemDetails = parsed.itemDetails;
          }
        }
      }
      this.cdr.markForCheck();
    } catch (e) {
      console.error('Failed to load saved selections:', e);
    }
  }

  private saveSelections(): void {
    if (!this.restaurantId) {
      return;
    }

    try {
      if (this.selectedItems.length > 0) {
        localStorage.setItem(
          this.getStorageKey(this.restaurantId),
          JSON.stringify({
            items: this.selectedItems,
            prices: this.selectedPrices,
            itemDetails: this.selectedItemDetails,
          })
        );
      } else {
        localStorage.removeItem(this.getStorageKey(this.restaurantId));
      }
    } catch (e) {
      console.error('Failed to save selections:', e);
    }
  }

  loadRestaurant(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.cdr.markForCheck();

    this.restaurantService
      .getRestaurantById(this.restaurantId)
      .pipe(
        catchError((error) => {
          console.error(
            'Failed to load restaurant:',
            error
          );

          this.errorMessage =
            'Unable to load restaurant information.';

          return of(null);
        }),
        finalize(() => {
          this.isLoading = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe((response) => {
        if (!response) {
          return;
        }

        if (!response.success || !response.data) {
          this.errorMessage =
            response.message ||
            'Unable to load restaurant information.';

          this.cdr.markForCheck();
          return;
        }

        this.restaurant = response.data;
        try {
          localStorage.setItem(`restaurant_${this.restaurantId}_details`, JSON.stringify(this.restaurant));
        } catch (_) {}
        this.cdr.markForCheck();
      });
  }

  loadMenu(page: number, shouldScroll = false): void {
    if (page < 1) {
      return;
    }

    this.isMenuLoading = true;
    this.cdr.markForCheck();

    this.restaurantService
      .getRestaurantMenu(
        this.restaurantId,
        page,
        this.itemsPerPage
      )
      .pipe(
        catchError((error) => {
          console.error(
            'Failed to load restaurant menu:',
            error
          );

          this.errorMessage =
            'Unable to load the restaurant menu.';

          return of(null);
        }),
        finalize(() => {
          this.isMenuLoading = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe((response) => {
        if (!response) {
          return;
        }

        if (!response.success || !response.data) {
          this.errorMessage =
            response.message ||
            'Unable to load the restaurant menu.';

          this.cdr.markForCheck();
          return;
        }

        const data = response.data;

        if (data.restaurant && (!this.restaurant.id || this.restaurant.id === 0)) {
          this.restaurant = data.restaurant;
        }

        this.menuItems = data.items;

        for (const item of this.menuItems) {
          if (this.selectedItems.includes(item.id)) {
            this.selectedPrices[item.id] = item.price;
            this.selectedItemDetails[item.id] = {
              id: item.id,
              name: item.name,
              price: item.price,
              image: item.imageUrl,
            };
          }
        }
        this.saveSelections();

        this.currentPage = data.page;

        this.itemsPerPage = data.pageSize;

        this.totalCount = data.totalCount;

        this.totalPages = data.totalPages;

        this.hasNextPage = data.hasNextPage;

        this.hasPreviousPage = data.hasPreviousPage;

        this.cdr.markForCheck();

        setTimeout(() => {
          this.animateMenuCards();
        });

        if (shouldScroll) {
          setTimeout(() => {
            this.scrollToMenu();
          });
        }
      });
  }

  private scrollToMenu(): void {
    const target =
      this.menuSection?.nativeElement ??
      document.getElementById('menu-section');

    if (target) {
      target.scrollIntoView({
        behavior: 'smooth',
        block: 'start',
      });
    }
  }

  toggleItem(itemId: number): void {
    this.showValidation = false;

    if (this.selectedItems.includes(itemId)) {
      this.selectedItems = this.selectedItems.filter(
        (id) => id !== itemId
      );
      delete this.selectedPrices[itemId];
      delete this.selectedItemDetails[itemId];
    } else {
      this.selectedItems = [...this.selectedItems, itemId];
      const item = this.menuItems.find((m) => m.id === itemId);
      if (item) {
        this.selectedPrices[itemId] = item.price;
        this.selectedItemDetails[itemId] = {
          id: item.id,
          name: item.name,
          price: item.price,
          image: item.imageUrl,
        };
      }
    }

    this.saveSelections();
    this.cdr.markForCheck();
  }

  isSelected(itemId: number): boolean {
    return this.selectedItems.includes(
      itemId
    );
  }

  changePage(page: number): void {
    if (
      page < 1 ||
      (this.totalPages > 0 && page > this.totalPages) ||
      (page === this.currentPage && this.menuItems.length > 0) ||
      this.isMenuLoading
    ) {
      return;
    }

    try {
      localStorage.setItem(`restaurant_${this.restaurantId}_menu_page`, String(page));
    } catch (_) {}

    this.loadMenu(page, true);
  }

  goBack(): void {
    this.router.navigate([
      '/restaurants',
    ]);
  }

  next(): void {
    if (
      this.selectedItems.length === 0
    ) {
      this.showValidation = true;

      setTimeout(() => {
        this.showValidation = false;
        this.cdr.markForCheck();
      }, 3000);

      this.cdr.markForCheck();
      return;
    }

    this.router.navigate(
      ['/orders'],
      {
        queryParams: {
          restaurantId:
            this.restaurantId,

          items:
            this.selectedItems.join(','),
        },
      }
    );
  }

  private animatePage(): void {
    animate(
      '.menu-hero',
      {
        opacity: [0, 1],
        y: [25, 0],
      },
      {
        duration: 0.7,
        ease: 'easeOut',
      }
    );

    animate(
      '.restaurant-info',
      {
        opacity: [0, 1],
        y: [20, 0],
      },
      {
        duration: 0.6,
        delay: 0.15,
        ease: 'easeOut',
      }
    );

    this.animateMenuCards();
  }

  private animateMenuCards(): void {
    animate(
      '.menu-card',
      {
        opacity: [0, 1],
        y: [25, 0],
      },
      {
        duration: 0.45,
        delay: stagger(0.08),
        ease: 'easeOut',
      }
    );
  }
}

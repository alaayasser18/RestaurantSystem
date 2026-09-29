import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  inject,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { animate } from 'motion';
import { Subject, merge, of } from 'rxjs';
import { catchError, map, switchMap, takeUntil, tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { RestaurantService } from '../../core/services/restaurant.service';
import { City, Restaurant, HomeStats } from '../../models/restaurant.model';

@Component({
  selector: 'app-restaurants',
  imports: [RouterLink, FormsModule],
  templateUrl: './restaurants.html',
})
export class Restaurants implements OnInit, OnDestroy, AfterViewInit {
  private readonly restaurantService = inject(RestaurantService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly cdr = inject(ChangeDetectorRef);

  private readonly destroy$ = new Subject<void>();
  private readonly reload$ = new Subject<void>();
  private readonly filterChange$ = new Subject<{search: string, cityId: string}>();

  // =========================
  // Search & Filters
  // =========================
  restaurantName = '';
  selectedCity = '';

  // =========================
  // Data
  // =========================
  filteredRestaurants: Restaurant[] = [];
  cities: City[] = [];
  stats: HomeStats | null = null;

  // =========================
  // Pagination
  // =========================
  readonly pageSize = 6;
  currentPage = 1;
  hasNextPage = false;
  totalCount = 0;
  totalPages = 0;
  pagesList: number[] = [];

  // =========================
  // Loading & States
  // =========================
  isLoadingRestaurants = false;
  isSearching = false;
  isLoadingCities = false;
  isLoadingStats = false;

  // =========================
  // Errors
  // =========================
  restaurantsError = '';
  citiesError = '';
  statsError = '';
  emptyMessage = '';

  ngOnInit(): void {
    this.loadCities();
    this.loadStats();
    this.setupReactiveSearch();

    const currentParams = this.route.snapshot.queryParams;
    const hasParams =
      currentParams['search'] !== undefined ||
      currentParams['cityId'] !== undefined ||
      currentParams['page'] !== undefined;

    if (!hasParams) {
      try {
        const saved = localStorage.getItem('restaurants_catalog_filters');
        if (saved) {
          const parsed = JSON.parse(saved);
          if (parsed && (parsed.search || parsed.cityId || (parsed.page && parsed.page > 1))) {
            this.router.navigate([], {
              relativeTo: this.route,
              queryParams: {
                search: parsed.search || null,
                cityId: parsed.cityId || null,
                page: parsed.page > 1 ? parsed.page : null,
              },
              replaceUrl: true,
            });
          }
        }
      } catch (_) {}
    }

    this.filterChange$.pipe(
      debounceTime(400),
      distinctUntilChanged((prev, curr) => prev.search === curr.search && prev.cityId === curr.cityId),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.applyFilters();
    });
  }

  onFilterChange(): void {
    this.filterChange$.next({
      search: this.restaurantName,
      cityId: this.selectedCity
    });
  }

  private setupReactiveSearch(): void {
    merge(
      this.route.queryParams,
      this.reload$.pipe(map(() => this.route.snapshot.queryParams))
    )
      .pipe(
        takeUntil(this.destroy$),
        tap((params) => {
          // Keep input controls in sync with the URL state
          this.restaurantName = params['search'] || '';
          this.selectedCity = params['cityId'] || '';
          this.currentPage = Number(params['page']) || 1;

          try {
            localStorage.setItem(
              'restaurants_catalog_filters',
              JSON.stringify({
                search: this.restaurantName,
                cityId: this.selectedCity,
                page: this.currentPage,
              })
            );
          } catch (_) {}

          this.isSearching = !!(this.restaurantName || this.selectedCity);
          this.isLoadingRestaurants = true;
          this.restaurantsError = '';
          this.cdr.markForCheck();
        }),
        // switchMap cancels any previous pending API requests when URL changes,
        // preventing race conditions and out-of-order responses.
        switchMap((params) => {
          const search = (params['search'] || '').trim();
          const cityId = params['cityId'] ? Number(params['cityId']) : null;
          const page = Number(params['page']) || 1;

          let request$;
          if (search && cityId) {
            request$ = this.restaurantService.searchRestaurantsByNameAndCity(
              search,
              cityId,
              page,
              this.pageSize
            );
          } else if (search) {
            request$ = this.restaurantService.searchRestaurants(
              search,
              page,
              this.pageSize
            );
          } else if (cityId) {
            request$ = this.restaurantService.getRestaurantsByCity(
              cityId,
              page,
              this.pageSize
            );
          } else {
            request$ = this.restaurantService.getRestaurants(
              page,
              this.pageSize
            );
          }

          return request$.pipe(
            catchError((error) => {
              console.error('API Error loading restaurants:', error);
              return of({ error: true, message: error?.message });
            })
          );
        })
      )
      .subscribe((response: any) => {
        this.isLoadingRestaurants = false;
        this.isSearching = false;

        if (response?.error) {
          this.restaurantsError =
            'Unable to load restaurants. Please check your connection and try again.';
          this.filteredRestaurants = [];
          this.totalPages = 0;
          this.totalCount = 0;
          this.pagesList = [];
          this.cdr.markForCheck();
          return;
        }

        const data = response?.data;
        this.filteredRestaurants = data?.items ?? [];
        this.currentPage = data?.page ?? 1;
        this.hasNextPage = data?.hasNextPage ?? false;
        this.totalCount = data?.totalCount ?? 0;
        this.totalPages = data?.totalPages ?? 0;
        this.pagesList = Array.from(
          { length: this.totalPages },
          (_, i) => i + 1
        );

        if (this.filteredRestaurants.length === 0) {
          this.emptyMessage = response?.message || 'No restaurants found.';
        } else {
          this.emptyMessage = '';
        }

        this.cdr.markForCheck();
      });
  }

  // =========================
  // Actions
  // =========================

  applyFilters(): void {
    const search = this.restaurantName.trim();
    const cityId = this.selectedCity;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        search: search || null,
        cityId: cityId || null,
        page: null, // Reset to page 1 on new filter/search
      },
    });
  }

  changePage(page: number): void {
    if (
      page < 1 ||
      (this.totalPages > 0 && page > this.totalPages) ||
      page === this.currentPage
    ) {
      return;
    }

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { page: page === 1 ? null : page },
      queryParamsHandling: 'merge',
    });

    // Smooth scroll to restaurants section for better UX
    const section = document.getElementById('restaurants-section');
    if (section) {
      section.scrollIntoView({ behavior: 'smooth' });
    }
  }

  clearSearch(): void {
    this.restaurantName = '';
    this.selectedCity = '';

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        search: null,
        cityId: null,
        page: null,
      },
    });
  }

  retry(): void {
    this.reload$.next();
  }

  // =========================
  // Cities & Stats
  // =========================

  loadCities(): void {
    this.isLoadingCities = true;
    this.citiesError = '';
    this.cdr.markForCheck();
    this.restaurantService
      .getCities()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.cities = response?.data ?? [];
          this.isLoadingCities = false;
          this.cdr.markForCheck();
        },
        error: (error) => {
          console.error('Error loading cities:', error);
          this.isLoadingCities = false;
          this.citiesError = 'Unable to load cities.';
          this.cdr.markForCheck();
        },
      });
  }

  loadStats(): void {
    this.isLoadingStats = true;
    this.statsError = '';
    this.cdr.markForCheck();
    this.restaurantService
      .getStats()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.stats = response?.data ?? null;
          this.isLoadingStats = false;
          this.cdr.markForCheck();
        },
        error: (error) => {
          console.error('Error loading stats:', error);
          this.isLoadingStats = false;
          this.statsError = 'Unable to load statistics.';
          this.cdr.markForCheck();
        },
      });
  }

  getSelectedCityName(): string {
    const cityId = Number(this.selectedCity);
    return this.cities.find((city) => city.id === cityId)?.name || '';
  }

  ngAfterViewInit(): void {
    animate(
      '.hero-animate',
      { opacity: [0, 1], y: [30, 0] },
      { duration: 0.8, ease: 'easeOut' }
    );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.reload$.complete();
  }
}
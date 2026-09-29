export interface City {
  id: number;
  name: string;
}

export interface Restaurant {
  id: number;
  name: string;
  city: City;
  category: string;
  rating: number;
  likes: number;
  comments: number;
  description: string;
  imageUrl: string;
}

export interface HomeStats {
  restaurantCount: number;
  peopleServed: number;
  happyService: string;
  regularUsers: number;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface PaginatedRestaurants {
  items: Restaurant[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
}

export type RestaurantsResponse =
  ApiResponse<PaginatedRestaurants>;

export type CitiesResponse =
  ApiResponse<City[]>;

export type StatsResponse =
  ApiResponse<HomeStats>;
export interface MenuItem {
  id: number;
  restaurantId: number;
  name: string;
  description: string;
  category: string;
  price: number;
  imageUrl: string;
  isAvailable: boolean;
}

export interface RestaurantDetails {
  id: number;
  name: string;
  city: {
    id: number;
    name: string;
  };
  category: string;
  description: string;
  imageUrl: string;
  rating: number;
  likes: number;
  comments: number;
  menuItemCount: number;
}

export interface RestaurantMenuData {
  restaurant: RestaurantDetails;
  items: MenuItem[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface RestaurantDetailsResponse {
  success: boolean;
  message: string;
  data: RestaurantDetails;
}

export interface RestaurantMenuResponse {
  success: boolean;
  message: string;
  data: RestaurantMenuData;
}
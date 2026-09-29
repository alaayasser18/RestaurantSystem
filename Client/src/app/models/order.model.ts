export interface CreateOrderItemRequest {
  menuItemId: number;
  quantity: number;
}

export interface CreateOrderRequest {
  restaurantId: number;
  customerName: string;
  customerPhone: string;
  customerEmail: string;
  customerAddress: string;
  items: CreateOrderItemRequest[];
}

export interface OrderItemDto {
  id: number;
  menuItemId: number;
  name: string;
  imageUrl: string;
  unitPrice: number;
  quantity: number;
  lineTotal: number;
}

export interface OrderDto {
  id: number;
  restaurantId: number;
  restaurantName: string;
  restaurantImageUrl: string;
  customerName: string;
  customerPhone: string;
  customerEmail: string;
  customerAddress: string;
  subtotal: number;
  deliveryFee: number;
  total: number;
  createdAt: string;
  items: OrderItemDto[];
}

export interface OrderResponse {
  success: boolean;
  message: string;
  data: OrderDto;
}

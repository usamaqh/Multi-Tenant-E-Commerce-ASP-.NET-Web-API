using Microsoft.EntityFrameworkCore;
using Multi_Tenant_E_Commerce_API.Data;
using Multi_Tenant_E_Commerce_API.Dtos.CartDtos;
using Multi_Tenant_E_Commerce_API.Dtos.CompanyDtos;
using Multi_Tenant_E_Commerce_API.Dtos.ItemDtos;
using Multi_Tenant_E_Commerce_API.Dtos.OrderDtos;
using Multi_Tenant_E_Commerce_API.Models;

namespace Multi_Tenant_E_Commerce_API.Services.CustomerService
{
    public class CustomerCartService : ICustomerCartService
    {
        private AppDbContext _dbContext;
        public CustomerCartService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CartResponse?> GetCartDetails(Guid userId)
        {
            return await _dbContext.Carts
                .AsNoTracking()
                .Where(c => c.Customer != null && c.Customer.UserId == userId)
                .Select(c => new CartResponse
                {
                    CartId = c.CartId,
                    TotalPrice = c.TotalPrice,
                    CreatedAt = c.CreatedAt,

                    CartItems = c.CartItems != null ? c.CartItems.Select(ci => new CartItemResponse
                    {
                        Item = new ItemForCartResponse
                        {
                            ItemId = ci.Item.ItemId,
                            Name = ci.Item.Name,
                            Price = ci.Item.Price,
                            InStock = ci.Item.InStock,
                            ImagePath = ci.Item.ImagePath,

                            ItemCompany = ci.Item.Company != null ? new CompanyForCartItemResponse
                            {
                                Name = ci.Item.Company.Name,
                                ImagePath = ci.Item.Company.ImagePath
                            } : new CompanyForCartItemResponse()
                        },

                        Quantity = ci.Quantity
                    }).ToList() : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(string, CartResponse?)> AddItemToCart(Guid userId, CartItemRequest newItem)
        {
            Item? item = await _dbContext.Items
                .FirstOrDefaultAsync(i =>
                    !i.IsDeleted &&
                    i.ItemId == newItem.ItemId &&
                    i.InStock >= newItem.Quantity);

            if (item == null)
                return ("no item data found or insufficient stock!", null);

            Cart? cart = await _dbContext.Carts
                .Where(c => c.Customer != null && c.Customer.UserId == userId)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                User? customer = await _dbContext.Users
                    .FirstOrDefaultAsync(u => !u.IsDeleted && u.UserId == userId);

                if (customer == null)
                    return ("no customer data found!", null);

                cart = new Cart { UserId = customer.Id };
                _dbContext.Carts.Add(cart);
            }

            CartItem? cartItem = cart.CartItems
                .FirstOrDefault(ci => ci.Item != null && ci.Item.ItemId == newItem.ItemId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ItemId = item.Id,
                    Quantity = newItem.Quantity
                };
                _dbContext.CartItems.Add(cartItem);
                cart.CartItems.Add(cartItem);
            }
            else
                cartItem.Quantity += newItem.Quantity;

            item.InStock -= newItem.Quantity;

            cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * (ci.Item?.Price ?? 0));

            await _dbContext.SaveChangesAsync();

            List<CartItemResponse> cartItemResponses = await BuildCartItemResponsesAsync(cart.Id);

            return ("", new CartResponse
            {
                CartId = cart.CartId,
                TotalPrice = cart.TotalPrice,
                CreatedAt = cart.CreatedAt,
                CartItems = cartItemResponses
            });
        }

        public async Task<(string, CartResponse?)> UpdateCartItem(Guid userId, CartItemRequest newItem)
        {
            Cart? cart = await _dbContext.Carts
                .Where(c => c.Customer != null && c.Customer.UserId == userId)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync();

            if (cart == null)
                return ("no cart found!", null);

            Item? item = await _dbContext.Items
                .FirstOrDefaultAsync(i => !i.IsDeleted && i.ItemId == newItem.ItemId);

            if (item == null)
                return ("no item data found!", null);

            CartItem? cartItem = cart.CartItems
                .FirstOrDefault(ci => ci.Item != null && ci.Item.ItemId == newItem.ItemId);

            if (cartItem == null)
                return ("no similar cart item found to update!", null);

            if ((item.InStock + cartItem.Quantity) < newItem.Quantity)
                return ("not enough stock!", null);

            item.InStock += cartItem.Quantity;
            cartItem.Quantity = newItem.Quantity;
            item.InStock -= newItem.Quantity;

            cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * (ci.Item?.Price ?? 0));

            await _dbContext.SaveChangesAsync();

            List<CartItemResponse> cartItemResponses = await BuildCartItemResponsesAsync(cart.Id);

            return ("", new CartResponse
            {
                CartId = cart.CartId,
                TotalPrice = cart.TotalPrice,
                CreatedAt = cart.CreatedAt,
                CartItems = cartItemResponses
            });
        }

        public async Task<(string, CartResponse?)> RemoveCartItem(Guid userId, Guid cartItemId)
        {
            Cart? cart = await _dbContext.Carts
                .Where(c => c.Customer != null && c.Customer.UserId == userId)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync();

            if (cart == null)
                return ("no cart found!", null);

            CartItem? cartItem = cart.CartItems
                .FirstOrDefault(ci => ci.Item != null && ci.Item.ItemId == cartItemId);

            if (cartItem == null)
                return ("no cart item found!", null);

            if (cartItem.Item.IsDeleted)
                return ("no item found in db!", null);

            cartItem.Item.InStock += cartItem.Quantity;
            cart.CartItems.Remove(cartItem);
            _dbContext.CartItems.Remove(cartItem);

            if (cart.CartItems.Count > 0)
            {
                cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * (ci.Item?.Price ?? 0));

                await _dbContext.SaveChangesAsync();

                List<CartItemResponse> cartItemResponses = await BuildCartItemResponsesAsync(cart.Id);

                return ("", new CartResponse
                {
                    CartId = cart.CartId,
                    TotalPrice = cart.TotalPrice,
                    CreatedAt = cart.CreatedAt,
                    CartItems = cartItemResponses
                });
            }

            _dbContext.Carts.Remove(cart);
            await _dbContext.SaveChangesAsync();

            return ("no remaining items, cart removed", null);
        }

        public async Task<bool> RemoveCart(Guid userId)
        {
            Cart? cart = await _dbContext.Carts
                .Where(c => c.Customer != null && c.Customer.UserId == userId)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync();

            if (cart == null)
                return false;

            foreach (CartItem cartItem in cart.CartItems)
            {
                cartItem.Item.InStock += cartItem.Quantity;
                _dbContext.CartItems.Remove(cartItem);
            }

            _dbContext.Carts.Remove(cart);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<OrderResponse?> CheckOutCart(Guid userId)
        {
            Cart? cart = await _dbContext.Carts
                .Where(c => c.Customer != null && c.Customer.UserId == userId)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync();

            if (cart == null)
                return null;

            Order order = new Order
            {
                TotalPrice = cart.TotalPrice,
                CustomerId = cart.UserId
            };

            _dbContext.Orders.Add(order);

            List<OrderItem> orderItems = cart.CartItems.Select(ci => new OrderItem
            {
                Order = order,
                ItemName = ci.Item.Name,
                ItemImageURL = ci.Item.ImagePath,
                ItemPrice = ci.Item.Price,
                Quantity = ci.Quantity
            }).ToList();

            foreach (CartItem ci in cart.CartItems)
            {
                ci.Item.Revenue += ci.Item.Price * ci.Quantity;
                ci.Item.SoldUnits += ci.Quantity;
                _dbContext.CartItems.Remove(ci);
            }

            _dbContext.OrderItems.AddRange(orderItems);
            _dbContext.Carts.Remove(cart);

            await _dbContext.SaveChangesAsync();

            return new OrderResponse
            {
                TotalPrice = order.TotalPrice,
                PaidAt = order.PaidAt,
                PurchasedItems = orderItems.Select(oi => new OrderItemRequest
                {
                    ItemName = oi.ItemName,
                    ItemImageURL = oi.ItemImageURL,
                    ItemPrice = oi.ItemPrice,
                    Quantity = oi.Quantity
                }).ToList()
            };
        }

        #region HelperFunctions
        private async Task<List<CartItemResponse>> BuildCartItemResponsesAsync(int cartId)
        {
            return await _dbContext.CartItems
                .AsNoTracking()
                .Where(ci => ci.CartId == cartId)
                .Select(ci => new CartItemResponse
                {
                    Quantity = ci.Quantity,
                    Item = new ItemForCartResponse
                    {
                        ItemId = ci.Item.ItemId,
                        Name = ci.Item.Name,
                        Price = ci.Item.Price,
                        InStock = ci.Item.InStock,
                        ImagePath = ci.Item.ImagePath,
                        ItemCompany = new CompanyForCartItemResponse
                        {
                            Name = ci.Item.Company.Name,
                            ImagePath = ci.Item.Company.ImagePath
                        }
                    }
                })
                .ToListAsync();
        }
        #endregion HelperFunctions
    }
}

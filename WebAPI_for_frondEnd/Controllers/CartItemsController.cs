using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_for_frondEnd.DTO;
using WebAPI_for_frondEnd.Models;

namespace WebAPI_for_frondEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly charityContext _context;

        public CartItemsController(charityContext context)
        {
            _context = context;
        }

        // GET: api/CartItems
        [HttpGet]
        public async Task<IEnumerable<CartItemDTO>> GetCartItems()
        {
            return _context.CartItems
                .Include(p => p.BuyerNavigation) 
                .Include(p => p.PIdNavigation)
                .Select(p => new CartItemDTO { 
                    Id = p.Id,
                    BuyerId = p.Buyer,
                    SellerId = p.PIdNavigation.Seller,
                    PId = p.PIdNavigation.Id,
                    Quantity = p.Quantity,
                });
        }

        //GET: api/CartItems/memberID/5
        [HttpGet("memberID/{id}")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItemsByMemberId(int id) {
            var cartItems = _context.CartItems
                .Include(p => p.BuyerNavigation)
                .Include(p => p.PIdNavigation)
                .ThenInclude(p => p.ProductImgs)
                .Where(p => p.Buyer == id)
                .Select(p => new CartItemGetDTO {
                    Id = p.Id,
                    Buyer = p.Buyer,
                    PId = p.PId,
                    Quantity = p.Quantity,
                    ProductName = p.PIdNavigation.Name,
                    ProductPrice = p.PIdNavigation.Price,
                    MainImageUrl = p.PIdNavigation.ProductImgs.Any()
                ? p.PIdNavigation.ProductImgs.Select(img => img.ImgName).FirstOrDefault()
                : "/images/non-found.jpg"
                });
            if (!cartItems.Any()) {
                return NotFound("No items found for this member.");
            }

            return Ok(cartItems);
        }

        // GET: api/CartItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetCartItem(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            return cartItem;
        }

        // PUT: api/CartItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemAddDTO cartItemDto) {
            if (cartItemDto == null || cartItemDto.Quantity <= 0 || !cartItemDto.BuyerId.HasValue || !cartItemDto.PId.HasValue) {
                return BadRequest("Invalid cart item details.");
            }
            // 檢查是否已存在相同的 BuyerId 和 PId 的項目
            var existingCartItem = await _context.CartItems
                .Include(ci => ci.PIdNavigation)  // 確保加載關聯導航屬性
                .FirstOrDefaultAsync(ci => ci.Buyer == cartItemDto.BuyerId && ci.PId == cartItemDto.PId);

            if (existingCartItem != null) {
                // 如果已存在，更新現有項目的 Quantity
                existingCartItem.Quantity = cartItemDto.Quantity;
                await _context.SaveChangesAsync();
                // 創建回應DTO
                var responseDto = new CartItemDTO {
                    Id = existingCartItem.Id,
                    BuyerId = existingCartItem.Buyer,
                    SellerId = existingCartItem.PIdNavigation.Seller,
                    PId = existingCartItem.PId,
                    Quantity = existingCartItem.Quantity
                };

                return Ok(new { message = "Cart item quantity updated successfully", cartItem = responseDto });
            }
            else {
                // 若不存在，新增新的 CartItem
                var newCartItem = new CartItem {
                    Buyer = cartItemDto.BuyerId,
                    PId = cartItemDto.PId,
                    Quantity = cartItemDto.Quantity
                };
                _context.CartItems.Add(newCartItem);
                await _context.SaveChangesAsync();

                var responseDto = new CartItemDTO {
                    Id = newCartItem.Id,
                    BuyerId = newCartItem.Buyer,
                    SellerId = newCartItem.PIdNavigation?.Seller, // 若無導航屬性可用
                    PId = newCartItem.PId,
                    Quantity = newCartItem.Quantity
                };

                return Ok(new { message = "Item added to cart successfully", cartItem = responseDto });
            }
        }

        // POST: api/CartItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartItem", new { id = cartItem.Id }, cartItem);
        }

        // DELETE: api/CartItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("UpdateCartItem")]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartItemAddDTO inputDto) {
            if (inputDto == null || inputDto.Quantity <= 0 || !inputDto.BuyerId.HasValue || !inputDto.PId.HasValue) {
                return BadRequest("Invalid cart item details.");
            }

            var existingCartItem = await _context.CartItems
            .Include(ci => ci.PIdNavigation)  // 加載 PIdNavigation
            .FirstOrDefaultAsync(ci => ci.Buyer == inputDto.BuyerId && ci.PId == inputDto.PId);

            if (existingCartItem != null) {
                existingCartItem.Quantity = inputDto.Quantity;
                await _context.SaveChangesAsync();
                var responseDto = new CartItemDTO {
                    Id = existingCartItem.Id,
                    BuyerId = existingCartItem.Buyer,
                    SellerId = existingCartItem.PIdNavigation.Seller,
                    PId = existingCartItem.PId,
                    Quantity = existingCartItem.Quantity
                };

                return Ok(new { message = "Cart item quantity updated successfully", responseDto });
            }

            return NotFound("Cart item not found.");
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItems.Any(e => e.Id == id);
        }
    }
}

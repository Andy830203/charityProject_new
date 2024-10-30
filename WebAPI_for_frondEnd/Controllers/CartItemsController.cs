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
            var cartItem = new CartItem {
                Buyer = cartItemDto.BuyerId,
                PId = cartItemDto.PId,
                Quantity = cartItemDto.Quantity
            };
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Item added to cart successfully", cartItem });
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

        private bool CartItemExists(int id)
        {
            return _context.CartItems.Any(e => e.Id == id);
        }
    }
}

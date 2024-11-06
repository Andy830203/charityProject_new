using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_for_frondEnd.DTO;
using WebAPI_for_frondEnd.Models;
using System.Reflection.Metadata;

namespace WebAPI_for_frondEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly charityContext _context;

        public OrdersController(charityContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderDto) {
            if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any()) {
                return BadRequest("Invalid order data.");
            }
            // 建立 Order 實例
            var order = new Order {
                Buyer = orderDto.Buyer,
                TotalPrice = orderDto.TotalPrice,
                Status = 1,  // 假設1是訂單初始狀態
                OrderTime = orderDto.OrderTime
            };
            // 加入每一筆訂單細項
            foreach (var itemDto in orderDto.OrderItems) {
                var orderItem = new OrderItem {
                    PId = itemDto.PId,
                    Quantity = itemDto.Quantity
                };
                order.OrderItems.Add(orderItem);
            }
            // 將訂單加到資料庫並保存
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { OrderId = order.Id, Message = "Order created successfully." });
        }

        [HttpPost("ToECpay")]
        public async Task<IActionResult> OrderToECpay([FromBody] OrderCreateDTO orderDto) {
            // Step 1: Validate the order data
            if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any()) {
                return BadRequest("Invalid order data.");
            }

            // Step 2: Create the order in the database
            var order = new Order {
                Buyer = orderDto.Buyer,
                TotalPrice = orderDto.TotalPrice,
                Status = 1,  // 假設1是訂單初始狀態
                OrderTime = orderDto.OrderTime
            };

            foreach (var itemDto in orderDto.OrderItems) {
                var orderItem = new OrderItem {
                    PId = itemDto.PId,
                    Quantity = itemDto.Quantity
                };
                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Step 3: Generate MerchantTradeNo using the newly created OrderId
            string merchantTradeNo = GenerateMerchantTradeNo(order.Id);

            // Step 4: Prepare the ECPay payload with order information
            var ecpayPayload = new Dictionary<string, string>
            {
                { "MerchantID", "3002607" },
                { "MerchantTradeNo", merchantTradeNo },
                { "MerchantTradeDate", orderDto.OrderTime.ToString("yyyy/MM/dd HH:mm:ss") },
                { "PaymentType", "aio" },
                { "TotalAmount", order.TotalPrice.ToString() },
                { "TradeDesc", "ecpay test" },
                { "ItemName", string.Join("#", order.OrderItems.Select(i => i.PId)) },
                { "ReturnURL", "http://localhost:5173/" },
                { "ChoosePayment", "ALL" },
                { "EncryptType", "1" },
                { "ClientBackURL", "http://localhost:5173/shop"}
            };

            // Step 5: Generate CheckMacValue
            string checkMacValue = GenerateCheckMacValue(ecpayPayload);
            ecpayPayload.Add("CheckMacValue", checkMacValue);
            // Step 6: Return the ECPay payload
            return Ok(ecpayPayload);
        }

        // Helper Method to Generate MerchantTradeNo
        private string GenerateMerchantTradeNo(int orderId) {
            var randomPart = GenerateRandomAlphanumeric(20 - orderId.ToString().Length);
            return $"{orderId}{randomPart}";
        }

        // Helper Method to Generate Random Alphanumeric String
        private string GenerateRandomAlphanumeric(int length) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var builder = new StringBuilder();
            for (int i = 0; i < length; i++) {
                builder.Append(chars[random.Next(chars.Length)]);
            }
            return builder.ToString();
        }

        // Helper Method to Generate CheckMacValue
        private string GenerateCheckMacValue(Dictionary<string, string> parameters) {
            string hashKey = "pwFHCqoQZGmho4w6";
            string hashIV = "EkRm7iFT261dpevs";

            // Step (1): Sort the parameters alphabetically by key
            var sortedParams = parameters.OrderBy(p => p.Key).ToList();

            // Step (2): Concatenate all parameters with format "key=value" and use '&' to separate
            var paramStr = string.Join("&", sortedParams.Select(p => $"{p.Key}={p.Value}"));

            // Step (3): Add HashKey and HashIV
            paramStr = $"HashKey={hashKey}&{paramStr}&HashIV={hashIV}";

            // Step (4): URL Encode the entire string
            paramStr = HttpUtility.UrlEncode(paramStr).ToLower();

            // Step (5): Compute SHA256 hash
            using (var sha256 = SHA256.Create()) {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(paramStr));
                var hash = BitConverter.ToString(bytes).Replace("-", "").ToUpper();
                return hash;
            }
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}

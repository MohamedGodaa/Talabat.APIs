using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Services.Contract;
using Talabat.DTOs;
using Talabat.Errors;
using Talabat.Services;

namespace Talabat.Controllers
{
   
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        // Create Order
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost] // Post => baseurl/api/Orders
        [Authorize]
        
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<OrderAddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);
            if (order is null) return BadRequest(new ApiResponse(400, "There is a problem With Your Order"));
            return Ok(order);
        }
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [HttpGet] // Get => baseurl/api/Orders
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmail);
            if (Orders is null) return NotFound(new ApiResponse(404, "There is no Orders For This User"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(MappedOrders);
        }



        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] // Get => baseurl/api/Orders/1
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrdersByIdForSpecificUserAsync(BuyerEmail, id);
            if (Order is null) return NotFound(new ApiResponse(404, $"There is no Order With Id = {id}"));
            var MappedOrder = _mapper.Map<Order,OrderToReturnDto>(Order);
            return Ok(MappedOrder); 
        }

        [HttpGet("DeliveryMethods")] // Get => baseurl/api/Orders/DeliveryMethods
        
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(DeliveryMethods);
        }

    }
}

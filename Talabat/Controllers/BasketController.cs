using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.DTOs;
using Talabat.Errors;

namespace Talabat.Controllers
{
    
    public class BasketController : BaseController
    {
        private readonly IBasketRepository _basketRepo;
        
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepo,IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }
        // Get or ReCreate Basket
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
        {
            var Basket = await _basketRepo.GetBasketAsync(BasketId);
            //if (Basket is null)
                //return new CustomerBasket(BasketId);
            return Ok(Basket);
        }
        //Update or Create New Basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepo.UpdateBasketAsync(MappedBasket);
            if (CreatedOrUpdatedBasket is null)
                return BadRequest(new ApiResponse(400));
            return Ok(CreatedOrUpdatedBasket);
        }
        // Delete Basket

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepo.DeleteBasketAsync(BasketId);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecifications;
using Talabat.DTOs;
using Talabat.Errors;
using Talabat.Helpers;
using Talabat.Repository.Repository;

namespace Talabat.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork
                                , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [Authorize]
        [ProducesResponseType(typeof(ProductDTo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDTo>>> GetProduct([FromQuery]ProductSpecParams Params)
        {
            var spec = new ProductWithBrandAndCategory(Params);
            var products = await _unitOfWork.Repository<Product>().GetAllWithspecificationAsync(spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTo>>(products);
            var CountSpec = new ProductWithFiltrationWithCountAsync(Params);
            var Count = await _unitOfWork.Repository<Product>().GetCountAsync(CountSpec);
            //var ReturnedObject = new Pagination<ProductDTo>()
            //{
            //    PageIndex = Params.PageIndex,
            //    PageSize = Params.PageSize,
            //    Data = MappedProducts
            //};
            return Ok(new Pagination<ProductDTo>(Params.PageSize, Params.PageIndex, MappedProducts,Count));
        }


        [ProducesResponseType(typeof(ProductDTo),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTo>> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndCategory(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(_mapper.Map<Product, ProductDTo>(product));
        }

        [HttpGet("Brand")] // Get : api/product/brand
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrand()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
        [HttpGet("categories")] // Get : api/product/categories
        public async Task<ActionResult<IReadOnlyList<Productcategories>>> Getcategories()
        {
            var categories = await _unitOfWork.Repository<Productcategories>().GetAllAsync();
            return Ok(categories);
        }
    }
}

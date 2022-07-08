using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using Basket.API.Entities;
using System;
using System.Threading.Tasks;
using System.Net;
using Basket.API.GrpcServices;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGprcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcService = discountGprcService ?? throw new ArgumentNullException(nameof(discountGprcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpGet("{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket (string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            //TODO : Communicate with Discount.Gprc
            //and Calculate latest prices of product into shopping cart.

            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{username}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing shopping cart for given username
            // create basketcheckout event - set total price on basketCheckout eventMessage 
            // send checkout event to rabbitmq
            // remove the basket

            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null) return BadRequest();

            // need to map basket to basketcheckoutevent class
            var message = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            message.TotalPrice = basket.TotalPrice; 
            await _publishEndpoint.Publish(message);

            // remove the basket
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        }
    }
}

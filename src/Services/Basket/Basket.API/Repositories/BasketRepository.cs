﻿using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _rediscache;

        public BasketRepository(IDistributedCache rediscache)
        {
            _rediscache = rediscache ?? throw new ArgumentNullException(nameof(rediscache));
        }

        public async Task DeleteBasket(string userName)
        {
            await _rediscache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _rediscache.GetStringAsync(userName);
            if (String.IsNullOrEmpty(basket))
                return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _rediscache.SetStringAsync(basket.UserName,JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);
        }
    }
}

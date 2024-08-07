using Microsoft.Extensions.Caching.Memory;

using WebApiAgenda.Interfaces;

namespace WebApiAgenda.Service
{
    public class CacheService : ICacheService
    {

        private readonly IServiceProvider _serviceProvider;
        private const string CACHE_KEY = "ValorCache";
        private const string VALOR_INSERIR_CACHE = "Valor a ser inserido no cache";

        public CacheService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public string RetornaValorCache()
        {
            var resultado = string.Empty;

            // recupera interface MemmoryCache com o Service Provide

            var cache = _serviceProvider.GetRequiredService<IMemoryCache>();

            //define tempo de expiracao

            MemoryCacheEntryOptions options = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // a cada 10 min
            };

            if (cache.TryGetValue("ValorCache", out object? valor))
            {
                return $"Retornando do Cache: {valor}";
            };

            // Se não encontrar, define o valor no cache
            cache.Set(CACHE_KEY, VALOR_INSERIR_CACHE, options);

            return $"Valor inserido no Cache: {VALOR_INSERIR_CACHE}";
        }
    }
}

using MusiCom.Core.Contracts;
using MusiCom.Core.Services;
using MusiCom.Infrastructure.Data.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension of the Service Collection
    /// </summary>
    public static class MusiComServiceCollectionExtension
    {
        /// <summary>
        /// Adds Services to the Service Collection
        /// </summary>
        /// <param name="services">the Service Collection</param>
        /// <returns>modified Service Collection</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ITagService, TagService>();

            return services;
        }
    }
}

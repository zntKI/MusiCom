﻿using MusiCom.Core.Contracts;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Services;
using MusiCom.Core.Services.Admin;
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
            services.AddScoped<INewService, NewService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IPostService, PostService>();

            //Admin
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}

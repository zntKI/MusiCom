using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.New;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Services
{
    /// <summary>
    /// Contains the logic for Genre functionalities
    /// </summary>
    public class NewService : INewServices
    {
        private readonly IRepository repo;

        public NewService(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Creates a New and Seeds it to the Database
        /// </summary>
        /// <param name="userId">EditorId passed by the Controller</param>
        /// <param name="model">ViewModel passed by the Controller</param>
        /// <param name="titlePhoto">The ImageFile passed by the Controller</param>
        public async Task CreateNewAsync(Guid editorId, NewAddViewModel model, IFormFile titlePhoto)
        {
            New neww = new New()
            {
                Title = model.Title,
                Content = model.Content,
                Tags = model.Tags
                    .Select(id => new NewTags()
                    { 
                        TagId = id,
                        DateOfCreation = DateTime.Now,
                        IsDeleted = false
                    }).ToList(),
                GenreId = model.GenreId,
                EditorId = editorId,
                PostedOn = DateTime.Now,
                IsDeleted = false
            };

            if (titlePhoto.Length > 0)
            {
                using var stream = new MemoryStream();
                await titlePhoto.CopyToAsync(stream);
                neww.TitlePhoto = stream.ToArray();
            }

            await repo.AddAsync(neww);
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Takes last three News stored in the Database
        /// </summary>
        /// <returns>Collection of NewAllViewModel</returns>
        public IEnumerable<NewAllViewModel> GetLastThreeNewsAsync()
        {
            var news = repo.All<New>()
                .OrderByDescending(n => n.PostedOn)
                .Take(3)
                .Select(n => new NewAllViewModel()
                { 
                    Id = n.Id,
                    Title = n.Title,
                    TitlePhoto = n.TitlePhoto,
                });

            return news;
        }
    }
}

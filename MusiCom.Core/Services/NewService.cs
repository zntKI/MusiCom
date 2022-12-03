using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.New;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.News;
using System.Linq;

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
        public async Task CreateNewAsync(Guid editorId, NewAddViewModel model, IFormFile image)
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

            string type = image.ContentType;

            if (!type.Contains("image"))
            {
                throw new InvalidOperationException();
            }

            string contentType = type.Substring(type.IndexOf('/') + 1, type.Length - type.Substring(0, type.IndexOf('/')).Length - 1);

            if (contentType != "png" && contentType != "jpeg" && contentType != "jpg")
            {
                throw new InvalidOperationException("Please import an image in one of the formats shown above!");
            }

            //TODO: Fix
            if (image.Length > 0)
            {
                using var stream = new MemoryStream();
                await image.CopyToAsync(stream);
                neww.TitleImage = stream.ToArray();
            }
            else
            {
                throw new InvalidOperationException();
            }

            await repo.AddAsync(neww);
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Gets All Comments attached to the New
        /// </summary>
        /// <param name="newId">New Id</param>
        /// <returns>Collection of Comments</returns>
        public ICollection<NewComment> GetAllCommentsForNew(Guid newId)
        {
            return repo.All<NewComment>()
                    .Include(n => n.User)
                    .Where(n => n.NewId == newId)
                    .OrderByDescending(n => n.DateOfPost)
                    .ToList();
        }

        /// <summary>
        /// Gets all tags which are attached to the given New
        /// </summary>
        /// <param name="newId">New Id</param>
        /// <returns>Collection of Tag</returns>
        public ICollection<Tag> GetAllTagsForNew(Guid newId)
        {
            return repo.All<NewTags>()
                    .Include(nt => nt.Tag)
                    .Where(nt => nt.NewId == newId)
                    .Select(nt => new Tag()
                    { 
                        Id = nt.TagId,
                        Name = nt.Tag.Name
                    }).ToList();
        }

        /// <summary>
        /// Takes last three News stored in the Database
        /// </summary>
        /// <returns>Collection of NewLastThreeViewModel</returns>
        public IEnumerable<NewLastThreeViewModel> GetLastThreeNews()
        {
            var news = repo.All<New>()
                .OrderByDescending(n => n.PostedOn)
                .Take(3)
                .Select(n => new NewLastThreeViewModel()
                { 
                    Id = n.Id,
                    Title = n.Title,
                    Image = n.TitleImage
                });

            return news;
        }

        /// <summary>
        /// Gets the New by Id
        /// </summary>
        /// <param name="newId">New Id</param>
        /// <returns>New</returns>
        public async Task<New> GetNewByIdAsync(Guid newId)
        {
            return await repo.GetByIdAsync<New>(newId);
        }

        /// <summary>
        /// Takes Remaining News stored in the Database
        /// </summary>
        /// <returns>Collection of NewLastThreeViewModel</returns>
        public IEnumerable<NewLastThreeViewModel> GetRemainingNews()
        {
            var news = repo.All<New>()
                .OrderByDescending(n => n.PostedOn)
                .Skip(3)
                .Select(n => new NewLastThreeViewModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Image = n.TitleImage
                });

            return news;
        }
    }
}

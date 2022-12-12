using Ganss.Xss;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.New;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Services
{
    /// <summary>
    /// Contains the logic for Genre functionalities
    /// </summary>
    public class NewService : INewService
    {
        private readonly IRepository repo;
        private HtmlSanitizer sanitizer;

        public NewService(IRepository _repo)
        {
            repo = _repo;
            sanitizer = new HtmlSanitizer();
        }

        public async Task CreateNewAsync(Guid editorId, NewAddViewModel model, IFormFile image)
        {
            New neww = new New()
            {
                Title = sanitizer.Sanitize(model.Title),
                Content = sanitizer.Sanitize(model.Content),
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

            await AddImage(neww, image);

            await repo.AddAsync(neww);
            await repo.SaveChangesAsync();
        }

        public async Task DeleteNewAsync(New neww)
        {
            neww.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        public async Task EditNewAsync(New neww, NewEditViewModel model, IFormFile image)
        {
            neww.Title = sanitizer.Sanitize(model.Title);
            neww.Content = sanitizer.Sanitize(model.Content);
            neww.GenreId = model.GenreId;

            if (image != null)
            {
                await AddImage(neww, image);
            }

            await repo.SaveChangesAsync();
        }

        public ICollection<NewComment> GetAllCommentsForNew(Guid newId)
        {
            return repo.All<NewComment>()
                    .Include(n => n.User)
                    .Where(n => n.NewId == newId)
                    .OrderByDescending(n => n.DateOfPost)
                    .ToList();
        }

        public async Task<NewQueryServiceModel> GetAllNewsAsync(string? genre = null, string? searchTerm = null, int currentPage = 1, int newsPerPage = 1)
        {
            var newsQuery = repo.AllReadonly<New>()
                .Where(n => n.IsDeleted == false);

            if (!String.IsNullOrWhiteSpace(genre))
            {
                newsQuery = repo.AllReadonly<New>()
                    .Where(e => e.Genre.Name == genre);
            }

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                newsQuery = newsQuery
                    .Where(e => EF.Functions.Like(e.Title.ToLower(), searchTerm) ||
                        EF.Functions.Like(e.Editor.FirstName!.ToLower(), searchTerm) ||
                        EF.Functions.Like(e.Editor.LastName!.ToLower(), searchTerm));
            }

            var news = await newsQuery
                .OrderByDescending(e => e.PostedOn)
                .Skip((currentPage - 1) * newsPerPage)
                .Take(newsPerPage)
                .Include(e => e.Editor)
                .Select(e => new NewAllNewViewModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Image = e.TitleImage,
                    Editor = new NewAllEditorViewModel()
                    { 
                        Id = e.EditorId,
                        EditorName = $"{e.Editor.FirstName} {e.Editor.LastName}"
                    }
                })
                .ToListAsync();

            var totalNews = await newsQuery.CountAsync();

            return new NewQueryServiceModel()
            {
                TotakNewsCount = totalNews,
                News = news
            };
        }

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

        public async Task<New> GetNewByIdAsync(Guid newId)
        {
            return await repo.GetByIdAsync<New>(newId);
        }

        /// <summary>
        /// Adds the given Image to the New
        /// </summary>
        /// <param name="neww">The New</param>
        /// <param name="image">The Image</param>
        /// <returns>The modified New</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<New> AddImage(New neww, IFormFile image)
        {
            string type = image.ContentType;

            if (!type.Contains("image"))
            {
                throw new InvalidOperationException("Not an image");
            }

            string contentType = type.Substring(type.IndexOf('/') + 1, type.Length - type.Substring(0, type.IndexOf('/')).Length - 1);

            if (contentType != "png" && contentType != "jpeg" && contentType != "jpg")
            {
                throw new InvalidOperationException("Not the right image format");
            }

            if (image.Length > 0)
            {
                using var stream = new MemoryStream();
                await image.CopyToAsync(stream);
                neww.TitleImage = stream.ToArray();
            }
            else
            {
                throw new InvalidOperationException("Image else");
            }

            return neww;
        }
    }
}

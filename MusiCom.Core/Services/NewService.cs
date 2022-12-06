using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Event;
using MusiCom.Core.Models.New;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.Events;
using MusiCom.Infrastructure.Data.Entities.News;
using System.Linq;
using static MusiCom.Infrastructure.Data.DataConstraints;

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
        /// <param name="editorId">EditorId passed by the Controller</param>
        /// <param name="model">ViewModel passed by the Controller</param>
        /// <param name="image">The ImageFile passed by the Controller</param>
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
        /// Marks the Given New as Deleted
        /// </summary>
        /// <param name="eventt">The New</param>
        public async Task DeleteNewAsync(New neww)
        {
            neww.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Edits a given New
        /// </summary>
        /// <param name="neww">The New to be Edited</param>
        /// <param name="model">The Model which contains the New Data for the New</param>
        /// <param name="image">The new Image file if there is such</param>
        public async Task EditNewAsync(New neww, NewEditViewModel model, IFormFile image)
        {
            neww.Title = model.Title;
            neww.Content = model.Content;
            neww.GenreId = model.GenreId;

            //foreach (var tag in model.Tags)
            //{
            //    if (!neww.Tags.Any(et => et.TagId == tag))
            //    {
            //        neww.Tags = model.Tags
            //        .Select(id => new NewTags()
            //        {
            //            TagId = id,
            //            DateOfCreation = DateTime.Now,
            //            IsDeleted = false
            //        }).ToList();
            //    }
            //}

            if (image != null)
            {
                await AddImage(neww, image);
            }

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
        /// Gets all News which correspond to the given criteria
        /// </summary>
        /// <param name="genre">Genre Name if passed by the View</param>
        /// <param name="tag">Tag Name if passed by the View</param>
        /// <param name="searchTerm">Word or Phrase which will be searched either in the New Title or the New's Creator</param>
        /// <param name="currentPage">The Current Page of all which hold News</param>
        /// <param name="newsPerPage">The Number of News that could be held in a Single Page</param>
        /// <returns>Model which will be used for the Visualisation in the View</returns>
        public async Task<NewQueryServiceModel> GetAllNewsAsync(string? genre = null, string? tag = null, string? searchTerm = null, int currentPage = 1, int newsPerPage = 1)
        {
            var newsQuery = repo.AllReadonly<New>()
                .Where(n => n.IsDeleted == false);

            if (!String.IsNullOrWhiteSpace(genre))
            {
                newsQuery = repo.AllReadonly<New>()
                    .Where(e => e.Genre.Name == genre);
            }

            if (!String.IsNullOrWhiteSpace(tag))
            {
                newsQuery = repo.AllReadonly<New>()
                    .Where(e => e.Tags.Any(t => t.Tag.Name == tag));
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
                .Skip((currentPage - 1) * newsPerPage)
                .Take(newsPerPage)
                .OrderByDescending(e => e.PostedOn)
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
        public IEnumerable<NewAllNewViewModel> GetLastThreeNews()
        {
            var news = repo.All<New>()
                .OrderByDescending(n => n.PostedOn)
                .Take(3)
                .Select(n => new NewAllNewViewModel()
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
        public IEnumerable<NewAllNewViewModel> GetRemainingNews()
        {
            var news = repo.All<New>()
                .OrderByDescending(n => n.PostedOn)
                .Skip(3)
                .Select(n => new NewAllNewViewModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Image = n.TitleImage
                });

            return news;
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

            //TODO: Fix
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

            return neww;
        }
    }
}

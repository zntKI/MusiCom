using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Genre;
using MusiCom.Core.Models.Tag;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Services
{
    /// <summary>
    /// Contains the logic for Tag functionalities
    /// </summary>
    public class TagService : ITagService
    {
        private readonly IRepository repo;
        private HtmlSanitizer sanitizer;

        public TagService(IRepository _repo)
        {
            repo = _repo;
            sanitizer = new HtmlSanitizer();
        }

        public async Task CreateTagAsync(TagViewModel model)
        {
            var tag = new Tag()
            {
                Name = sanitizer.Sanitize(model.Name),
                DateOfCreation = DateTime.Now,
                IsDeleted = false
            };

            await repo.AddAsync(tag);
            await repo.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(Tag tag)
        {
            tag.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        public async Task EditTagAsync(Tag tag, TagAllViewModel model)
        {
            tag.Name = sanitizer.Sanitize(model.Name);
            tag.DateOfCreation = DateTime.Now;

            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetAllTagNamesAsync()
        {
            return await repo.AllReadonly<Tag>()
                .Where(t => t.IsDeleted == false)
                .Select(t => t.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<TagNewAllViewModel>> GetAllTagsAsync()
        {
            return await repo.All<Tag>()
                .Where(t => t.IsDeleted == false)
                .Select(t => new TagNewAllViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsDeleted = t.IsDeleted,
                    DateOfCreation = t.DateOfCreation,
                })
                .OrderByDescending(t => t.DateOfCreation)
                .ToListAsync();
        }

        public async Task<Tag> GetTagByIdAsync(Guid id)
        {
            return await repo.GetByIdAsync<Tag>(id);
        }
    }
}

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

        public TagService(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Creates a Tag
        /// </summary>
        /// <param name="model">model passed by the controller</param>s
        public async Task CreateTagAsync(TagViewModel model)
        {
            var tag = new Tag()
            {
                Name = model.Name,
                DateOfCreation = DateTime.Now,
                IsDeleted = false
            };

            await repo.AddAsync(tag);
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Marks a given Tag as Deleted
        /// </summary>
        /// <param name="id">The Id of the given Tag</param>
        public async Task DeleteTagAsync(Guid id)
        {
            Tag tag = await GetTagByIdAsync(id);
            tag.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Edits a Tag with a given Id
        /// </summary>
        /// <param name="id">Id of the Tag</param>
        /// <param name="model">Model which is passed from the View</param>
        public async Task EditTagAsync(Guid id, TagAllViewModel model)
        {
            var tag = await GetTagByIdAsync(id);

            tag.Name = model.Name;
            tag.DateOfCreation = DateTime.Now;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Lists all Tags ordered by DateOfCreation
        /// </summary>
        /// <returns>All Tags</returns>
        //TODO:
        public IEnumerable<TagNewAllViewModel> GetAllTags()
        {
            var models = repo.All<Tag>()
                .Where(t => t.IsDeleted == false)
                .Select(t => new TagNewAllViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    IsDeleted = t.IsDeleted,
                    DateOfCreation = t.DateOfCreation,
                })
                .OrderByDescending(t => t.DateOfCreation);

            return models;
        }

        /// <summary>
        /// Gets the Tag with the given Id
        /// </summary>
        /// <param name="id">The Id of a Tag</param>
        /// <returns>The desired Tag</returns>
        public async Task<Tag> GetTagByIdAsync(Guid id)
        {
            return await repo.GetByIdAsync<Tag>(id);
        }
    }
}

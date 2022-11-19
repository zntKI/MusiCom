using MusiCom.Core.Contracts;
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
    }
}

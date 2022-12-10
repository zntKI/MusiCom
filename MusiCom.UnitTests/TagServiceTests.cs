using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Tag;
using MusiCom.Core.Services;
using MusiCom.Infrastructure.Data;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.UnitTests
{
    /// <summary>
    /// Contains Unit Tests for TagService
    /// </summary>
    [TestFixture]
    public class TagServiceTests
    {
        private ApplicationDbContext context;
        private ITagService tagService;
        private IRepository repo;

        /// <summary>
        /// Sets up InMemoryDatabase
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TagDb")
                .Options;

            context = new ApplicationDbContext(contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            repo = new Repository(context);
            tagService = new TagService(repo);
        }

        /// <summary>
        /// Asserts whether the method has taken the right Tag
        /// </summary>
        [Test]
        public async Task TestTagByIdAsyncInMemory()
        {
            await repo.AddRangeAsync(new List<Tag>()
            {
                new Tag() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
            });
            await repo.SaveChangesAsync();

            var tag = await tagService.GetTagByIdAsync(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"));

            Assert.That(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Is.EqualTo(tag.Id));
        }

        /// <summary>
        /// Asserts whether the method takes only the NotDeleted Tags
        /// </summary>
        [Test]
        public async Task TestGetAllTagsAsyncNotDeletedInMemory()
        {
            await repo.AddRangeAsync(new List<Tag>()
            {
                new Tag() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "", IsDeleted = true, DateOfCreation = DateTime.Now },
                new Tag() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
            });
            await repo.SaveChangesAsync();

            var tags = await tagService.GetAllTagsAsync();

            Assert.That(tags.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Asserts whether the method orders Tags correctly
        /// </summary>
        [Test]
        public async Task TestGetAllTagsAsyncOrderedByDateInMemory()
        {
            await repo.AddRangeAsync(new List<Tag>()
            {
                new Tag() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "", IsDeleted = false, DateOfCreation = new DateTime(2019, 12, 12, 12, 12, 12) },
                new Tag() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "", IsDeleted = false, DateOfCreation = new DateTime(2019, 12, 12, 12, 12, 13) },
                new Tag() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "", IsDeleted = false, DateOfCreation = new DateTime(2019, 12, 12, 12, 12, 11) },
                new Tag() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"), Name = "", IsDeleted = false, DateOfCreation = new DateTime(2019, 12, 12, 12, 12, 10) },
            });
            await repo.SaveChangesAsync();

            IEnumerable<DateTime> ordered = new List<DateTime>()
            {
                new DateTime(2019, 12, 12, 12, 12, 13),
                new DateTime(2019, 12, 12, 12, 12, 12),
                new DateTime(2019, 12, 12, 12, 12, 11),
                new DateTime(2019, 12, 12, 12, 12, 10)
            };

            List<DateTime> dates = new List<DateTime>();
            var tags = await tagService.GetAllTagsAsync();
            foreach (var genre in tags)
            {
                dates.Add(genre.DateOfCreation);
            }

            Assert.That(dates, Is.EqualTo(ordered));
        }

        /// <summary>
        /// Asserts whether the method gets the Tag Names
        /// </summary>
        [Test]
        public async Task TestGetAllTagNamesAsyncInMemory()
        {
            await repo.AddRangeAsync(new List<Tag>()
            {
                new Tag() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "Yeah!", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() {Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "Cool", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() {Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "Very", IsDeleted = false, DateOfCreation = DateTime.Now }
            });
            await repo.SaveChangesAsync();

            List<string> names = new List<string>()
            {
                "Yeah!", "Cool", "Very"
            };

            var tNames = await tagService.GetAllTagNamesAsync();

            Assert.That(tNames, Is.EqualTo(names));
        }

        /// <summary>
        /// Asserts wheter the method Edits a Tag correctly by comparing TagName
        /// </summary>
        [Test]
        public async Task TestEditTagAsyncInMemory()
        {
            await repo.AddRangeAsync(new List<Tag>()
            {
                new Tag() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "Yeah!", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "Cool", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() {Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "Very", IsDeleted = false, DateOfCreation = DateTime.Now }
            });
            await repo.SaveChangesAsync();

            TagAllViewModel model = new TagAllViewModel()
            {
                Name = "Very Cool",
                DateOfCreation = DateTime.Now
            };

            var tag = await tagService.GetTagByIdAsync(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"));
            await tagService.EditTagAsync(tag, model);

            var newTag = await tagService.GetTagByIdAsync(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"));

            Assert.That(newTag.Name, Is.EqualTo("Very Cool"));
        }

        /// <summary>
        /// Asserts that the method Deletes the Tag successfully
        /// </summary>
        [Test]
        public async Task TestDeleteTagAsyncInMemory()
        {
            await repo.AddRangeAsync(new List<Tag>()
            {
                new Tag() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() {Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() {Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now }
            });
            await repo.SaveChangesAsync();

            var tag = await tagService.GetTagByIdAsync(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"));
            await tagService.DeleteTagAsync(tag);

            var tags = await tagService.GetAllTagsAsync();

            Assert.That(tags.Count(), Is.EqualTo(2));
        }

        /// <summary>
        /// Asserts that the method Creates Tag successfully
        /// </summary>
        [Test]
        public async Task TestCreateTagAsyncInMemory()
        {
            await repo.AddRangeAsync(new List<Tag>()
            {
                new Tag() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now },
                new Tag() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now }
            });
            await repo.SaveChangesAsync();

            var model = new TagAllViewModel()
            {
                Name = "Jazz"
            };
            await tagService.CreateTagAsync(model);

            var tags = await tagService.GetAllTagsAsync();

            Assert.That(tags.Count(), Is.EqualTo(4));
        }

        /// <summary>
        /// Disposes the Context
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Genre;
using MusiCom.Core.Services.Admin;
using MusiCom.Infrastructure.Data;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.Events;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.UnitTests
{
    /// <summary>
    /// Contains Unit Tests for GenreService
    /// </summary>
    [TestFixture]
    public class GenreServiceTests
    {
        private ApplicationDbContext context;
        private IGenreService genreService;
        private IRepository repo;

        /// <summary>
        /// Sets up InMemoryDatabase
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("GenreDb")
                .Options;

            context = new ApplicationDbContext(contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            repo = new Repository(context);
            genreService = new GenreService(repo);
        }

        /// <summary>
        /// Asserts whether the method has taken the right Genre
        /// </summary>
        [Test]
        public async Task TestGenreByIdAsyncInMemory()
        {
            await repo.AddRangeAsync(new List<Genre>()
            {
                new Genre() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
            });
            await repo.SaveChangesAsync();

            var genre = await genreService.GetGenreByIdAsync(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"));

            Assert.That(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Is.EqualTo(genre.Id));
        }

        /// <summary>
        /// Asserts whether the method takes only the NotDeleted Genres
        /// </summary>
        [Test]
        public async Task TestGetAllGenresAsyncNotDeletedInMemory()
        {
            await repo.AddRangeAsync(new List<Genre>()
            {
                new Genre() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "", IsDeleted = true, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"), Name = "", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
            });
            await repo.SaveChangesAsync();

            var genres = await genreService.GetAllGenresAsync();

            Assert.That(genres.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Asserts whether the method orders Genres correctly
        /// </summary>
        [Test]
        public async Task TestGetAllGenresAsyncOrderedByDateInMemory()
        {
            await repo.AddRangeAsync(new List<Genre>()
            {
                new Genre() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "", IsDeleted = false, DateOfCreation = new DateTime(2019, 12, 12, 12, 12, 12), Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "", IsDeleted = false, DateOfCreation = new DateTime(2019, 12, 12, 12, 12, 13), Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "", IsDeleted = false, DateOfCreation = new DateTime(2019, 12, 12, 12, 12, 11), Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"), Name = "", IsDeleted = false, DateOfCreation = new DateTime(2019, 12, 12, 12, 12, 10), Events = new List<Event>(), News = new List<New>() },
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
            var genres = await genreService.GetAllGenresAsync();
            foreach (var genre in genres)
            {
                dates.Add(genre.DateOfCreation);
            }

            Assert.That(dates, Is.EqualTo(ordered));
        }

        /// <summary>
        /// Asserts whether the method gets the Genre Names
        /// </summary>
        [Test]
        public async Task TestGetAllGenreNamesAsync()
        {
            await repo.AddRangeAsync(new List<Genre>()
            {
                new Genre() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "Rock", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "Metal", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "Pop", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() }
            });
            await repo.SaveChangesAsync();

            List<string> names = new List<string>()
            {
                "Rock", "Metal", "Pop"
            };

            var gNames = await genreService.GetAllGenreNamesAsync();

            Assert.That(gNames, Is.EqualTo(names));
        }

        /// <summary>
        /// Asserts wheter the method Edits a Genre correctly by comparing GenreName
        /// </summary>
        [Test]
        public async Task TestEditGenreAsync()
        {
            await repo.AddRangeAsync(new List<Genre>()
            {
                new Genre() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "Rock", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "Metal", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "Pop", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() }
            });
            await repo.SaveChangesAsync();

            GenreAllViewModel model = new GenreAllViewModel()
            {
                Name = "Folk",
                DateOfCreation = DateTime.Now
            };

            await genreService.EditGenreAsync(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), model);

            var newGenre = await genreService.GetGenreByIdAsync(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"));

            Assert.That(newGenre.Name, Is.EqualTo("Folk"));
        }

        /// <summary>
        /// Asserts that the method Deletes the Genre successfully
        /// </summary>
        [Test]
        public async Task TestDeleteGenreAsync()
        {
            await repo.AddRangeAsync(new List<Genre>()
            {
                new Genre() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "Rock", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "Metal", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "Pop", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() }
            });
            await repo.SaveChangesAsync();

            await genreService.DeleteGenreAsync(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"));

            var genres = await genreService.GetAllGenresAsync();

            Assert.That(genres.Count(), Is.EqualTo(2));
        }

        /// <summary>
        /// Asserts that the method Creates Genre successfully
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestCreateGenreAsync()
        {
            await repo.AddRangeAsync(new List<Genre>()
            {
                new Genre() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "Rock", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "Metal", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"), Name = "Pop", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() }
            });
            await repo.SaveChangesAsync();

            var model = new GenreViewModel()
            {
                Name = "Jazz"
            };
            await genreService.CreateGenreAsync(model);

            var genres = await genreService.GetAllGenresAsync();

            Assert.That(genres.Count(), Is.EqualTo(4));
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

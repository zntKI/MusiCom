using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Event;
using MusiCom.Core.Services;
using MusiCom.Infrastructure.Data;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities;
using MusiCom.Infrastructure.Data.Entities.Events;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.UnitTests
{
    /// <summary>
    /// Contains Unit Tests for EventService
    /// </summary>
    [TestFixture]
    public class EventServiceTests
    {
        private ApplicationDbContext context;
        private IEventService eventService;
        private IRepository repo;

        [SetUp]
        public async Task SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TagDb")
                .Options;

            context = new ApplicationDbContext(contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            repo = new Repository(context);
            eventService = new EventService(repo);


            await repo.AddRangeAsync(new List<Genre>()
            {
                new Genre() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "Second", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() },
                new Genre() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "First", IsDeleted = false, DateOfCreation = DateTime.Now, Events = new List<Event>(), News = new List<New>() }
            });
            await repo.AddRangeAsync(new List<Event>()
            {
                new Event() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    Title = "", Image = new byte[1000],
                    Description = "",
                    GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                    ArtistId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    Artist = new ApplicationUser()
                    { 
                        Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                        UserName = "Test",
                        Email = "Test"
                    },
                    Date = DateTime.Now,
                    DateOfCreation = DateTime.Now,
                    IsDeleted = false
                },
                new Event() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Title = "", Image = new byte[1000],
                    Description = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    ArtistId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    Artist = new ApplicationUser()
                    {
                        Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                        UserName = "Test1",
                        Email = "Test1"
                    },
                    Date = DateTime.Now,
                    DateOfCreation = DateTime.Now,
                    IsDeleted = false
                }
            });
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Asserts whether the method takes the right Event
        /// </summary>
        [Test]
        public async Task TestGetEventByIdAsyncInMemory()
        {
            var eventt = await eventService.GetEventByIdAsync(new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"));

            Assert.That(eventt.Id, Is.EqualTo(new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f")));
        }

        /// <summary>
        /// Asserts whether the method deletes the Event successfully
        /// </summary>
        [Test]
        public async Task TestDeleteEventAsyncInMemory()
        {
            var eventt = await eventService.GetEventByIdAsync(new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"));
            await eventService.DeleteEventAsync(eventt);

            var events = repo.All<Event>(e => e.IsDeleted == false);

            Assert.That(events.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Asserts whether the method edits an Event successfully
        /// </summary>
        [Test]
        public async Task TestEditEventAsyncInMemory()
        {
            var eventt = await eventService.GetEventByIdAsync(new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"));

            var model = new EventEditViewModel()
            {
                Title = "Title",
                Description = "Description",
                GenreId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                Date = DateTime.Now,
            };
            IFormFile? image = null;

            await eventService.EditEventAsync(eventt, model, image);

            var newEventt = await eventService.GetEventByIdAsync(new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"));

            Assert.That(newEventt.GenreId, Is.EqualTo(new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f")));
        }

        /// <summary>
        /// Asserts whether the method gets the right Posts for Event
        /// </summary>
        [Test]
        public async Task TestGetAllPostsForEventAsyncInMemory()
        {
            await repo.AddRangeAsync(new List<EventPost>()
            {
                new EventPost(){ Id = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c"),
                    Content = "",
                    DateOfPost = DateTime.Now,
                    IsDeleted = false,
                    NumberOfDislikes = 0,
                    NumberOfLikes = 0,
                    EventId = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    UserId = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c")
                },
                new EventPost(){ Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    Content = "",
                    DateOfPost = DateTime.Now,
                    IsDeleted = false,
                    NumberOfDislikes = 0,
                    NumberOfLikes = 0,
                    EventId = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c"),
                    UserId = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c")
                },
                new EventPost(){ Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                    Content = "",
                    DateOfPost = DateTime.Now,
                    IsDeleted = false,
                    NumberOfDislikes = 0,
                    NumberOfLikes = 0,
                    EventId = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    UserId = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c")
                }
            });
            await repo.SaveChangesAsync();

            var all = repo.All<EventPost>(ep => ep.EventId == new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"));

            var posts = await eventService.GetAllPostsForEventAsync(new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"));

            Assert.That(posts.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Asserts that the method gets the right Event for Edit
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestGetEventByIdForDetailsAsyncInMemory()
        {
            var eventt = await eventService.GetEventByIdAsync(new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"));
            var model = await eventService.GetEventByIdForDetailsAsync(eventt);

            Assert.That(model.Genre.Id, Is.EqualTo(new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569")));
        }

        /// <summary>
        /// Asserts that the method gets all Events without any criteria
        /// </summary>
        [Test]
        public async Task TestGetAllEventsAsyncAllInMemory()
        {
            var model = await eventService.GetAllEventsAsync(eventsPerPage: 2);

            Assert.That(model.TotalEventsCount, Is.EqualTo(2));
        }

        [Test]
        public async Task TestGetAllEventsAsyncForGenreInMemory()
        {
            await repo.AddAsync<ApplicationUser>(new ApplicationUser()
            {
                Id = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                UserName = "Test1",
                Email = "Test1"
            });
            await repo.AddRangeAsync(new List<Event>()
            {
                new Event() { Id = new Guid("6f7394d4-5425-4ece-b0f0-3585f8eab2f1"),
                    Title = "", Image = new byte[1000],
                    Description = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    ArtistId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    Date = DateTime.Now,
                    DateOfCreation = DateTime.Now,
                    IsDeleted = false
                },
                new Event() { Id = new Guid("5cd5a0d5-0bef-4a96-ac05-ef7cfadc7a91"),
                    Title = "", Image = new byte[1000],
                    Description = "",
                    GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                    ArtistId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    Date = DateTime.Now,
                    DateOfCreation = DateTime.Now,
                    IsDeleted = false
                }
            });
            await repo.SaveChangesAsync();

            var model = await eventService.GetAllEventsAsync(genre: "First", currentPage:1, eventsPerPage: 3);

            Assert.That(model.Events.Count, Is.EqualTo(2));
        }

        [TearDown]
        public void TearDown()
        { 
            context.Dispose();
        }
    }
}

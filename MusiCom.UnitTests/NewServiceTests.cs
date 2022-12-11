using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.New;
using MusiCom.Core.Services;
using MusiCom.Infrastructure.Data;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.UnitTests
{
    /// <summary>
    /// Contains Unit Tests for NewService
    /// </summary>
    [TestFixture]
    public class NewServiceTests
    {
        private ApplicationDbContext context;
        private INewService newService;
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
            newService = new NewService(repo);
        }

        /// <summary>
        /// Asserts whether the method takes the right New
        /// </summary>
        [Test]
        public async Task TestGetEventByIdAsyncInMemory()
        {
            await repo.AddRangeAsync(new List<New>()
            {
                new New() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                },
                new New() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                }
            });
            await repo.SaveChangesAsync();

            var eventt = await newService.GetNewByIdAsync(new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"));

            Assert.That(eventt.Id, Is.EqualTo(new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f")));
        }

        /// <summary>
        /// Asserts that the method takes all tags for a given New
        /// </summary>
        [Test]
        public async Task TestGetAllTagsForNewInMemory()
        {
            var tag1 = new Tag()
            {
                Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                Name = "1",
                DateOfCreation = DateTime.Now,
                IsDeleted = false,
            };
            var tag2 = new Tag()
            {
                Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                Name = "2",
                DateOfCreation = DateTime.Now,
                IsDeleted = false,
            };
            await repo.AddRangeAsync(new List<New>()
            {
                new New() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false,
                    Tags = new List<NewTags>()
                    { 
                        new NewTags(){
                            NewId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                            Tag = tag1
                        },
                        new NewTags(){
                            NewId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                            Tag = tag2
                        },
                    }
                },
                new New() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                }
            });
            await repo.SaveChangesAsync();

            var newTags = newService.GetAllTagsForNew(new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"));

            Assert.That(newTags.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Asserts that the method takes all Comments for a given New
        /// </summary>
        [Test]
        public async Task TestGetAllCommentsForNewInMemory()
        {
            var newComments = new List<NewComment>()
            {
                new NewComment(){
                    Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    Content = "",
                    DateOfPost = DateTime.Now,
                    IsDeleted = false,
                    NewId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    NumberOfDislikes= 0,
                    NumberOfLikes= 0,
                    User = new ApplicationUser()
                    { 
                        Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                        UserName = "",
                        Email = ""
                    }
                },
                new NewComment(){
                    Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Content = "",
                    DateOfPost = DateTime.Now,
                    IsDeleted = false,
                    NewId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    NumberOfDislikes= 0,
                    NumberOfLikes= 0,
                    User = new ApplicationUser()
                    {
                        Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                        UserName = "",
                        Email = ""
                    }
                }
            };
            await repo.AddRangeAsync(newComments);
            await repo.AddRangeAsync(new List<New>()
            {
                new New() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false,
                    NewComments = newComments,
                },
                new New() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                }
            });
            await repo.SaveChangesAsync();

            var finalComments = newService.GetAllCommentsForNew(new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"));

            Assert.That(finalComments.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Asserts that the method successfully Deletes the New
        /// </summary>
        [Test]
        public async Task TestDeleteNewAsyncInMemory()
        {
            var neww = new New()
            {
                Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                Title = "",
                TitleImage = new byte[1000],
                Content = "",
                GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                PostedOn = DateTime.Now,
                IsDeleted = false
            };
            await repo.AddRangeAsync(new List<New>()
            {
                neww,
                new New() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                }
            });
            await repo.SaveChangesAsync();

            await newService.DeleteNewAsync(neww);
            await newService.GetNewByIdAsync(neww.Id);

            Assert.That(neww.IsDeleted, Is.EqualTo(true));
        }

        /// <summary>
        /// Asserts that the method successfully Edits the given New
        /// </summary>
        [Test]
        public async Task TestEditNewAsyncInMemory()
        {
            var neww = new New()
            {
                Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                Title = "",
                TitleImage = new byte[1000],
                Content = "",
                GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                PostedOn = DateTime.Now,
                IsDeleted = false
            };
            await repo.AddRangeAsync(new List<New>()
            {
                neww,
                new New() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                }
            });
            await repo.SaveChangesAsync();

            var model = new NewEditViewModel()
            {
                Title = "Title",
                Content = "1",
                GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
            };
            IFormFile? img = null;

            await newService.EditNewAsync(neww, model, img);
            var finalNew = await newService.GetNewByIdAsync(neww.Id);

            Assert.That(finalNew.Title, Is.EqualTo("Title"));
        }

        /// <summary>
        /// Asserts that the method gets All News
        /// </summary>
        [Test]
        public async Task TestGetAllNewsAsyncInMemory()
        {
            var neww = new New()
            {
                Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                Title = "",
                TitleImage = new byte[1000],
                Content = "",
                GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                PostedOn = DateTime.Now,
                IsDeleted = false
            };
            await repo.AddRangeAsync(new List<New>()
            {
                neww,
                new New() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                }
            });
            await repo.SaveChangesAsync();

            var news = await newService.GetAllNewsAsync(newsPerPage: 2);

            Assert.That(news.TotakNewsCount, Is.EqualTo(2));
        }

        /// <summary>
        /// Asserts that the method takes all News corresponding to a given Genre
        /// </summary>
        [Test]
        public async Task TestGetAllNewsAsyncForGenreInMemory()
        {
            var genre1 = new Genre() { Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"), Name = "Second", IsDeleted = false, DateOfCreation = DateTime.Now, News = new List<New>() };
            var genre2 = new Genre() { Id = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"), Name = "First", IsDeleted = false, DateOfCreation = DateTime.Now, News = new List<New>() };
            await repo.AddRangeAsync(new List<Genre>()
            {
                genre1,
                genre2
            });
            var neww = new New()
            {
                Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                Title = "",
                TitleImage = new byte[1000],
                Content = "",
                GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                Genre = genre2,
                EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                PostedOn = DateTime.Now,
                IsDeleted = false
            };
            await repo.AddRangeAsync(new List<New>()
            {
                neww,
                new New() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Title = "", TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    Genre = genre1,
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false
                }
            });
            await repo.SaveChangesAsync();

            var news = await newService.GetAllNewsAsync(genre: "Second", newsPerPage: 2);

            Assert.That(news.TotakNewsCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Asserts that the method takes the News corresponding to a given SearchTerm
        /// </summary>
        [Test]
        public async Task TestGetAllNewsAsyncForSearchTermInMemory()
        {
            var user1 = new ApplicationUser()
            {
                Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                UserName = "user",
                Email = "a"
            };
            var neww = new New()
            {
                Id = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                Title = "first",
                TitleImage = new byte[1000],
                Content = "",
                GenreId = new Guid("89fedf3f-282c-43db-bfec-0ec8004cb3eb"),
                EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                PostedOn = DateTime.Now,
                IsDeleted = false,
                Editor = user1
            };
            await repo.AddRangeAsync(new List<New>()
            {
                neww,
                new New() { Id = new Guid("5872f3f2-4c44-476d-a67b-59a1bcda8b1f"),
                    Title = "firs",
                    TitleImage = new byte[1000],
                    Content = "",
                    GenreId = new Guid("7e2ec5de-ad4d-4900-925c-4147cbdf9569"),
                    EditorId = new Guid("68576c9f-be19-42eb-8b8b-f44205396b7f"),
                    PostedOn = DateTime.Now,
                    IsDeleted = false,
                    Editor = user1
                }
            });
            await repo.SaveChangesAsync();

            var news = await newService.GetAllNewsAsync(searchTerm: "firs", newsPerPage: 2);

            Assert.That(news.TotakNewsCount, Is.EqualTo(2));
        }
    }
}
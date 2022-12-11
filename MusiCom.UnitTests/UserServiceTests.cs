using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Admin.User;
using MusiCom.Core.Services.Admin;
using MusiCom.Infrastructure.Data;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.UnitTests
{
    /// <summary>
    /// Contains Unit Tests for UserService
    /// </summary>
    [TestFixture]
    public class UserServiceTests
    {
        private ApplicationDbContext context;
        private IUserService userService;
        private IRepository repo;

        /// <summary>
        /// Sets up InMemoryDatabase
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CommentDb")
                .Options;

            context = new ApplicationDbContext(contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            repo = new Repository(context);
            userService = new UserService(repo);
        }

        /// <summary>
        /// Asserts that the method Sets the IsDeleted prop back to false
        /// </summary>
        [Test]
        public async Task TestBringBackUserAsyncInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = true };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            await userService.BringBackUserAsync(user);

            var newUser = await repo.GetByIdAsync<ApplicationUser>(user.Id);

            Assert.That(newUser.IsDeleted, Is.EqualTo(false));
        }

        /// <summary>
        /// Asserts that the method Sets the IsDeleted prop to true
        /// </summary>
        [Test]
        public async Task TestDeleteUserAsyncInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            await userService.DeleteUserAsync(user);

            var newUser = await repo.GetByIdAsync<ApplicationUser>(user.Id);

            Assert.That(newUser.IsDeleted, Is.EqualTo(true));
        }

        /// <summary>
        /// Asserts that the method assigns the User as an Artist successfully
        /// </summary>
        [Test]
        public async Task TestCreateArtistAsyncInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            await userService.CreateArtistAsync(user);

            var newUser = await repo.GetByIdAsync<ApplicationUser>(user.Id);

            Assert.IsNotNull(newUser.ArtistId);
        }

        /// <summary>
        /// Asserts that the method unassigns the User from being an Artist successfully
        /// </summary>
        [Test]
        public async Task TestRemoveArtistAsyncInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            await userService.CreateArtistAsync(user);
            var newUser = await repo.GetByIdAsync<ApplicationUser>(user.Id);
            await userService.RemoveArtistAsync(newUser);

            var finalUser = await repo.GetByIdAsync<ApplicationUser>(user.Id);
            Assert.IsNull(finalUser.ArtistId);
        }

        /// <summary>
        /// Asserts that the method assigns the User as an Editor successfully
        /// </summary>
        [Test]
        public async Task TestCreateEditorAsyncInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            var model = new EditorAddViewModel()
            {
                Salary = 1,
                UserId = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f")
            };
            await userService.CreateEditorAsync(model, user);

            var newUser = await repo.GetByIdAsync<ApplicationUser>(user.Id);

            Assert.IsNotNull(newUser.EditorId);
        }

        /// <summary>
        /// Asserts that the method unassigns the User from being an Editor successfully
        /// </summary>
        [Test]
        public async Task TestRemoveEditorAsyncInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            var model = new EditorAddViewModel()
            {
                Salary = 1,
                UserId = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f")
            };
            await userService.CreateEditorAsync(model, user);
            var newUser = await repo.GetByIdAsync<ApplicationUser>(user.Id);
            await userService.RemoveEditorAsync(newUser);

            var finalUser = await repo.GetByIdAsync<ApplicationUser>(user.Id);
            Assert.IsNull(finalUser.EditorId);
        }

        /// <summary>
        /// Asserts that the method gets all Users successfully
        /// </summary>
        [Test]
        public async Task TestAllAsyncInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            var users = await userService.AllAsync(usersPerPage: 2);

            Assert.That(users.TotalUsersCount, Is.EqualTo(2));
        }

        /// <summary>
        /// Asserts that the method gets all Users who are UsersOnly successfully
        /// </summary>
        [Test]
        public async Task TestAllAsyncAsUsersOnlyInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            var users = await userService.AllAsync(type: "UserOnly", usersPerPage: 2);

            Assert.That(users.TotalUsersCount, Is.EqualTo(2));
        }

        /// <summary>
        /// Asserts that the method gets all Users who are Editors successfully
        /// </summary>
        [Test]
        public async Task TestAllAsyncAsEditorInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false, EditorId = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f") };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            var users = await userService.AllAsync(type: "Editor", usersPerPage: 2);

            Assert.That(users.TotalUsersCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Asserts that the method gets all Users who are Artists successfully
        /// </summary>
        [Test]
        public async Task TestAllAsyncAsArtistInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false, ArtistId = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f") };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true, ArtistId = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6") },
            });
            await repo.SaveChangesAsync();

            var users = await userService.AllAsync(type: "Artist", usersPerPage: 2);

            Assert.That(users.TotalUsersCount, Is.EqualTo(2));
        }

        /// <summary>
        /// Asserts that the method gets all Users who are Editors and Artists successfully
        /// </summary>
        [Test]
        public async Task TestAllAsyncAsArtistsAndEditorsInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "1", Email = "1", IsDeleted = false, ArtistId = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), EditorId = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6") };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            var users = await userService.AllAsync(type: "Editors and Artists", usersPerPage: 2);

            Assert.That(users.TotalUsersCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Asserts that the method successfully gets all Users who are in a given criteria
        /// </summary>
        [Test]
        public async Task TestAllAsyncAsSearchTermInMemory()
        {
            var user = new ApplicationUser() { Id = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), UserName = "Ivan", Email = "1", IsDeleted = false, ArtistId = new Guid("b08b2e6e-c22a-4306-ae57-5325a7d5854f"), EditorId = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6") };
            await repo.AddRangeAsync(new List<ApplicationUser>()
            {
                user,
                new ApplicationUser(){ Id = new Guid("3842e541-d0cd-4cb6-9827-3ba61ca11fd6"), UserName = "2", Email = "2", IsDeleted = true },
            });
            await repo.SaveChangesAsync();

            var users = await userService.AllAsync(searchTerm: "iv", usersPerPage: 2);

            Assert.That(users.TotalUsersCount, Is.EqualTo(1));
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

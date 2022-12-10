using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts;
using MusiCom.Core.Services;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data;
using MusiCom.Infrastructure.Data.Entities.News;
using static MusiCom.Infrastructure.Data.DataConstraints;
using MusiCom.Core.Models.Comment;

namespace MusiCom.UnitTests
{
    /// <summary>
    /// Contains Unit Tests for CommentService
    /// </summary>
    [TestFixture]
    public class CommentServiceTests
    {
        private ApplicationDbContext context;
        private ICommentService commentService;
        private IRepository repo;

        /// <summary>
        /// Sets up InMemoryDatabase
        /// </summary>
        [SetUp]
        public async Task SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CommentDb")
                .Options;

            context = new ApplicationDbContext(contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            repo = new Repository(context);
            commentService = new CommentService(repo);

            await repo.AddRangeAsync(new List<NewComment>()
            {
                new NewComment(){ Id = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c"), Content = "", DateOfPost = DateTime.Now, IsDeleted = false, NumberOfDislikes = 0, NumberOfLikes = 0, NewId = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c"), UserId = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c") },
                new NewComment(){ Id = new Guid("21689234-319c-440b-89f3-7aa02cf11d80"), Content = "", DateOfPost = DateTime.Now, IsDeleted = false, NumberOfDislikes = 0, NumberOfLikes = 0, NewId = new Guid("21689234-319c-440b-89f3-7aa02cf11d80"), UserId = new Guid("21689234-319c-440b-89f3-7aa02cf11d80") },
            });
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Asserts whether the method has taken the right Comment
        /// </summary>
        [Test]
        public async Task TestGetCommentByIdAsyncInMemory()
        {
            var comment = await commentService.GetCommentByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            Assert.That(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"), Is.EqualTo(comment.Id));
        }

        /// <summary>
        /// Asserts that the method Creates Comment successfully
        /// </summary>
        [Test]
        public async Task TestCreateCommentAsyncInMemory()
        {
            var model = new CommentAddViewModel()
            {
                Content = ""
            };
            await commentService.CreateCommentAsync(model, new Guid("172bc8c4-4825-4950-bf18-b136cda8792f"), new Guid("69bef169-8653-4093-98be-721b1108afef"));

            var comments = repo.All<NewComment>();

            Assert.That(comments.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Asserts whether the method adds Like to Comment
        /// </summary>
        [Test]
        public async Task TestAddLikeToCommentAsyncInMemory()
        {
            var comment = await commentService.GetCommentByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            await commentService.AddLikeToCommentAsync(comment);

            var commentNew = await commentService.GetCommentByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            Assert.That(commentNew.NumberOfLikes, Is.EqualTo(1));
        }

        /// <summary>
        /// Asserts whether the method adds DisLike to Comment
        /// </summary>
        [Test]
        public async Task TestAddDislikeToCommentAsyncInMemory()
        {
            var comment = await commentService.GetCommentByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            await commentService.AddDislikeToCommentAsync(comment);

            var commentNew = await commentService.GetCommentByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            Assert.That(commentNew.NumberOfDislikes, Is.EqualTo(1));
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

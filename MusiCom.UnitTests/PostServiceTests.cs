using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts;
using MusiCom.Core.Services;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.News;
using MusiCom.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusiCom.Infrastructure.Data.Entities.Events;
using MusiCom.Core.Models.Comment;
using MusiCom.Core.Models.Post;
using Microsoft.AspNetCore.Http;

namespace MusiCom.UnitTests
{
    /// <summary>
    /// Contains Unit Tests for PostService
    /// </summary>
    [TestFixture]
    public class PostServiceTests
    {
        private ApplicationDbContext context;
        private IPostService postService;
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
            postService = new PostService(repo);

            await repo.AddRangeAsync(new List<EventPost>()
            {
                new EventPost(){ Id = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c"), Content = "", DateOfPost = DateTime.Now, IsDeleted = false, NumberOfDislikes = 0, NumberOfLikes = 0, EventId = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c"), UserId = new Guid("7b9e687e-2465-4ec2-b025-562b6446675c") },
                new EventPost(){ Id = new Guid("21689234-319c-440b-89f3-7aa02cf11d80"), Content = "", DateOfPost = DateTime.Now, IsDeleted = false, NumberOfDislikes = 0, NumberOfLikes = 0, EventId = new Guid("21689234-319c-440b-89f3-7aa02cf11d80"), UserId = new Guid("21689234-319c-440b-89f3-7aa02cf11d80") },
            });
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Asserts whether the method adds Like to Post
        /// </summary>
        [Test]
        public async Task TestAddLikeToPostAsyncInMemory()
        {
            var post = await postService.GetPostByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            await postService.AddLikeToPostAsync(post);

            var postNew = await postService.GetPostByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            Assert.That(postNew.NumberOfLikes, Is.EqualTo(1));
        }

        /// <summary>
        /// Asserts whether the method adds DisLike to Post
        /// </summary>
        [Test]
        public async Task TestAddDislikeToPostAsyncInMemory()
        {
            var post = await postService.GetPostByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            await postService.AddDislikeToPostAsync(post);

            var postNew = await postService.GetPostByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            Assert.That(postNew.NumberOfDislikes, Is.EqualTo(1));
        }

        /// <summary>
        /// Asserts whether the method has taken the right Post
        /// </summary>
        [Test]
        public async Task TestGetCommentByIdAsyncInMemory()
        {
            var post = await postService.GetPostByIdAsync(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"));

            Assert.That(new Guid("21689234-319c-440b-89f3-7aa02cf11d80"), Is.EqualTo(post.Id));
        }

        /// <summary>
        /// Asserts that the method Creates Post successfully
        /// </summary>
        [Test]
        public async Task TestCreateCommentAsyncInMemory()
        {
            var model = new PostAddViewModel()
            {
                Content = ""
            };
            IFormFile? image = null;
            await postService.CreatePostAsync(model, new Guid("172bc8c4-4825-4950-bf18-b136cda8792f"), new Guid("69bef169-8653-4093-98be-721b1108afef"), image);

            var posts = repo.All<EventPost>();

            Assert.That(posts.Count(), Is.EqualTo(3));
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

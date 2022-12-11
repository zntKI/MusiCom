using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.Event;
using MusiCom.Core.Models.New;
using MusiCom.Infrastructure.Data.Entities.News;
using static MusiCom.Infrastructure.Data.DataConstraints;
using static System.Net.Mime.MediaTypeNames;

namespace MusiCom.Core.Contracts
{
    /// <summary>
    /// Interface for GenreService
    /// </summary>
    public interface INewService
    {
        /// <summary>
        /// Gets all News which correspond to the given criteria
        /// </summary>
        /// <param name="genre">Genre Name if passed by the View</param>
        /// <param name="tag">Name if passed by the View</param>
        /// <param name="searchTerm">Word or Phrase which will be searched either in the New Title or the New's Creator</param>
        /// <param name="currentPage">Current Page of all which hold News</param>
        /// <param name="newsPerPage">Number of News that could be held in a Single Page</param>
        /// <returns>Model which will be used for the Visualisation in the View</returns>
        Task<NewQueryServiceModel> GetAllNewsAsync(
            string? genre = null,
            string? tag = null,
            string? searchTerm = null,
            int currentPage = 1,
            int newsPerPage = 1);

        /// <summary>
        /// Creates a New and Seeds it to the Database
        /// </summary>
        /// <param name="editorId">EditorId passed by the Controller</param>
        /// <param name="model">ViewModel passed by the Controller</param>
        /// <param name="image">The ImageFile passed by the Controller</param>
        Task CreateNewAsync(Guid editorId, NewAddViewModel model, IFormFile image);

        /// <summary>
        /// Gets the New by Id
        /// </summary>
        /// <param name="newId">New Id</param>
        /// <returns>New</returns>
        Task<New> GetNewByIdAsync(Guid newId);

        /// <summary>
        /// Gets all tags which are attached to the given New
        /// </summary>
        /// <param name="newId">New Id</param>
        /// <returns>Collection of Tag</returns>
        ICollection<Tag> GetAllTagsForNew(Guid newId);

        /// <summary>
        /// Gets All Comments attached to the New
        /// </summary>
        /// <param name="newId">New Id</param>
        /// <returns>Collection of Comments</returns>
        ICollection<NewComment> GetAllCommentsForNew(Guid newId);

        /// <summary>
        /// Marks the Given New as Deleted
        /// </summary>
        /// <param name="neww">The New</param>
        Task DeleteNewAsync(New neww);

        /// <summary>
        /// Edits a given New
        /// </summary>
        /// <param name="neww">New to be Edited</param>
        /// <param name="model">Model which contains the New Data for the New</param>
        /// <param name="image">new Image file if there is such</param>
        Task EditNewAsync(New neww, NewEditViewModel model, IFormFile image);
    }
}

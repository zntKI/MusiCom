namespace MusiCom.Core.Models.Genre
{
    /// <summary>
    /// Contains Genre data for showing all Genres
    /// </summary>
    public class GenreAllViewModel : GenreViewModel
    {
        public Guid Id { get; init; }

        public DateTime DateOfCreation { get; set; }
    }
}

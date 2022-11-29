namespace MusiCom.Core.Models.Post
{
    /// <summary>
    /// Contains data for adding a Post
    /// </summary>
    public class PostAddViewModel
    {
        public string Content { get; set; } = null!;

        public byte[]? Image { get; set; }
    }
}

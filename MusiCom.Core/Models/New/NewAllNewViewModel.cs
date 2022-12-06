namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Contains data for showing lst Three News
    /// </summary>
    public class NewAllNewViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public byte[] Image { get; set; } = null!;

        public NewAllEditorViewModel Editor { get; set; }
    }
}

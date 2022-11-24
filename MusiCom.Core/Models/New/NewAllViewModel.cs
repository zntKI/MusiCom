namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Contains Collection for LastThreeNews and TheRemainingNews
    /// </summary>
    public class NewAllViewModel
    {
        public IEnumerable<NewLastThreeViewModel> LastThreeNews { get; set; }

        public IEnumerable<NewLastThreeViewModel> RestOfNews { get; set; }
    }
}

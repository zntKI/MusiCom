﻿namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Contains Collection for LastThreeNews and TheRemainingNews
    /// </summary>
    public class NewAllViewModel
    {
        public IEnumerable<NewAllNewViewModel> LastThreeNews { get; set; }

        public IEnumerable<NewAllNewViewModel> RestOfNews { get; set; }
    }
}

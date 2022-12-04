﻿namespace MusiCom.Core.Models.Admin.User
{
    public class UserServiceModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public Guid? EditorId { get; set; }

        public Guid? ArtistId { get; set; }

        public bool IsDeleted { get; set; }
    }
}

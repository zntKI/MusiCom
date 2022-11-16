﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusiCom.Infrastructure.Data.DataConstraints.UserC;

namespace MusiCom.Infrastructure.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(FirstNameMaxLength)]
        public string? FirstName { get; set; }

        [MaxLength(LastNameMaxLength)]
        public string? LastName { get; set; }
    }
}

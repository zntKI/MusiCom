using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Infrastructure.Data
{
    /// <summary>
    /// Holds information about data constraints.
    /// </summary>
    /// <remarks>
    ///  Data constraints are grouped by Entities and ViewModels.
    /// </remarks>
    public class DataConstraints
    {
        /// <summary>
        /// Data constraints for User.
        /// </summary>
        public class UserC
        {
            public const int FirstNameMaxLength = 50;
            public const int FirstNameMinLength = 2;

            public const int LastNameMaxLength = 50;
            public const int LastNameMinLength = 2;

            public const int UserNameMaxLength = 30;
            public const int UserNameMinLength = 3;

            public const int UserEmailMaxLength = 254;
            public const int UserEmailMinLength = 3;

            public const int UserPasswordMaxLength = 20;
            public const int UserPasswordMinLength = 5;
        }
    }
}

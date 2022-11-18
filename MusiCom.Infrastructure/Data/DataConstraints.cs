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

        /// <summary>
        /// Data constraints for Tag.
        /// </summary>
        public class TagC
        {
            public const int TagNameMaxLength = 40;
            public const int TagNameMinLength = 2;
        }

        /// <summary>
        /// Data constraints for New.
        /// </summary>
        public class NewC
        {
            public const int NewTitleMaxLength = 100;
            public const int NewTitleMinLength = 5;
        }

        /// <summary>
        /// Data constraints for Genre.
        /// </summary>
        public class GenreC
        {
            public const int GenreNameMaxLength = 20;
            public const int GenreNameMinLength = 3;
        }

        /// <summary>
        /// Data constraints for New's Comment.
        /// </summary>
        public class NewCommentC
        {
            public const int ContentMaxLength = 450;
            public const int ContentMinLength = 3;
        }

        /// <summary>
        /// Data constraints for New's Comment.
        /// </summary>
        public class EventC
        {
            public const int EventTitleMaxLength = 50;
            public const int EventTitleMinLength = 5;

            public const int DescriptionMaxLength = 450;
            public const int DescriptionMinLength = 3;
        }

        /// <summary>
        /// Data constraints for Event's Post.
        /// </summary>
        public class EventPostC
        {
            public const int ContentMaxLength = 450;
            public const int ContentMinLength = 3;
        }
    }
}

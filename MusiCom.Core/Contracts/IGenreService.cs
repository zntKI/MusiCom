using MusiCom.Core.Models.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Contracts
{
    /// <summary>
    /// Interface for GenreService
    /// </summary>
    public interface IGenreService
    {
        Task CreateGenreAsync(GenreViewModel model);
    }
}

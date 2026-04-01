using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Business.Contracts.DTO;

namespace Webzine.Repository.Contracts
{
    public interface IDeezerRepository
    {
        Task<PlaylistDto> GetPlaylistAsync(long playlistId);
    }
}

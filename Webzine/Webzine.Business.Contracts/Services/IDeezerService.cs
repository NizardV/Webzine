using Webzine.Business.Contracts.DTO;
using Webzine.Entity;

namespace Webzine.Business.Contracts.Services
{
    public interface IDeezerService
    {
        Task<PlaylistExtractionResult> ExtractDataFromPlaylist(long playlistId);
    }
}

using Webzine.Business.Contracts.DTO;
using Webzine.Business.Contracts.Services;
using Webzine.Repository.Contracts;
using Webzine.Entity;
using Bogus;

namespace Webzine.Business.Services
{
    public class DeezerService : IDeezerService
    {
        private readonly IDeezerRepository _deezerRepository;
        private readonly Faker _faker;
        public DeezerService(IDeezerRepository deezerRepository)
        {
            _deezerRepository = deezerRepository;
            _faker = new Faker();
        }
        public async Task<PlaylistExtractionResult> ExtractDataFromPlaylist(long playlistId)
        {
            var result = new PlaylistExtractionResult();
            var playlist = await this._deezerRepository.GetPlaylistAsync(playlistId);
            playlist.TrackList.Tracks.ForEach(track =>
            {
                var date = this._faker.Date.Past().ToUniversalTime();
                var artiste = result.Artistes.FirstOrDefault(a => a.Nom == track.Artist.Name);

                if (artiste == null)
                {
                    artiste = new Artiste
                    {
                        Nom = track.Artist.Name,
                        Biographie = this._faker.Lorem.Paragraph(),
                        Titres = new List<Titre>()
                    };
                    result.Artistes.Add(artiste);
                }

                if (track.Preview is null || track.Album.CoverUrl is null)
                    {
                        var x = playlistId;
                    }
                var titre = new Titre
                {
                    Libelle = track.Title,
                    Chronique = this._faker.Lorem.Paragraph(),
                    UrlJaquette = track.Album.CoverUrl ?? this._faker.Image.PicsumUrl(),
                    UrlEcoute = track.Link.Remove(0, 23),
                    DateCreation = date,
                    DateSortie = this._faker.Date.Between(date, date.AddDays(30)).ToUniversalTime(),
                    Duree = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(track.Duration)),
                    NbLectures = this._faker.Random.Int(0, 10000),
                    NbLikes = this._faker.Random.Int(0, 5000),

                    Artiste = artiste,

                    Album = track.Album.Title,
                    Commentaires = new List<Commentaire>(),
                    Styles = new List<Style>()
                };

                artiste.Titres.Add(titre);
                result.Titres.Add(titre);
            });

            return result;
        }
    }
}

using NLog;
using Webzine.Business.Contracts.Services;
using Webzine.Entity;
using Webzine.Entity.Fixtures;

namespace Webzine.EntitiesContext.Seeders
{
    public class SeedDataDbDeezer
    {
        public static void Seed(IDeezerService deezerService, WebzineDbContext _context, ILogger _logger)
        {
            var styles = StyleFactory.CreerStyles(18);

            var tousLesTitres = new List<Titre>();
            var tousLesArtistes = new List<Artiste>();

            var playlistsId = new List<long> { 13308460803, 6774435444, 14787642143, 14651835621, 14332300381, 14332301501, 14332302721, 10846562102, 15066016883, 11339664184, 8341896982, 634393245, 11977979021, 13583739241, 6931773584, 10367539242, 8081071762, 2175892102, 282050331, 4375893682, 3884424862, 8796651422, 8967037122, 14868665283, 2252127126, 8380202462, 8154030222, 1218512601, 12649912143, 13317928583, 4885902924,1542804, 11398228324, 1321696237,10566894642, 10051127502, 2753248004, 1565553361, 1282495565, 7559195462, 9148675142, 1252278941, 969361861, 10331644722, 7361420344, 13115216803, 11514107224, 67175576, 3419539542, 1478649355, 1109890291, 761191911, 2143562442, 3279798822, 1189520191, 1071669561, 6682665064, 759282441, 1132251583, 6548712124, 2448918882, 3155776842, 1313621735, 1111142221, 747148961 };

            foreach (var playlistId in playlistsId)
            {
                try
                {
                    var result = deezerService.ExtractDataFromPlaylist(playlistId).GetAwaiter().GetResult();

                    var nouveauxTitres = new List<Titre>();

                    foreach (var titre in result.Titres)
                    {
                        var artisteExistant = tousLesArtistes.FirstOrDefault(a => a.Nom == titre.Artiste.Nom);

                        if (artisteExistant != null)
                        {
                            titre.Artiste = artisteExistant;
                        }
                        else
                        {
                            tousLesArtistes.Add(titre.Artiste);
                        }

                        if (!tousLesTitres.Any(t => t.Libelle == titre.Libelle && t.Artiste.Nom == titre.Artiste.Nom))
                        {
                            nouveauxTitres.Add(titre);
                        }
                    }

                    nouveauxTitres = TitreFactory.AssignerStylesATitres(nouveauxTitres, styles);
                    tousLesTitres.AddRange(nouveauxTitres);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            var tousLesCommentaires = CommentaireFactory.CreerCommentaires(tousLesTitres, 100);

            _context.Styles.AddRange(styles);
            _context.Artistes.AddRange(tousLesArtistes);
            _context.Titres.AddRange(tousLesTitres);
            _context.Commentaires.AddRange(tousLesCommentaires);

            _context.SaveChanges();
            _logger.Info("Seeding réussi depuis playlists : " + playlistsId.Count());
        }
    }
}

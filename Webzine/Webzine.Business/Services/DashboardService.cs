using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Business.Contracts.Dto;
using Webzine.Business.Contracts.Services;
using Webzine.Repository.Contracts;

namespace Webzine.Business.Services
{
    public class DashboardService : IDashboardService
    {

        private readonly IArtisteRepository artisteRepository;
        private readonly ITitreRepository titreRepository;
        private readonly IStyleRepository styleRepository;

        public DashboardService(IArtisteRepository artisteRepository, ITitreRepository titreRepository, IStyleRepository styleRepository)
        {
            this.artisteRepository = artisteRepository;
            this.titreRepository = titreRepository;
            this.styleRepository = styleRepository;
        }

        public DashboardDto GetDashboardStats()
        {
            return new DashboardDto
            {
                NbTitres = this.titreRepository.Count(),
                NbLectures = this.titreRepository.SumLectures(),
                NbLikes = this.titreRepository.SumLikes(),
                TitrePlusLu = this.titreRepository.GetTitrePlusLu()?.Libelle?? "Aucun",

                NbArtistes = this.artisteRepository.Count(),
                ArtistePlusAlbums = this.artisteRepository.GetArtistePlusAlbums()?.Nom ?? "Aucun",
                NbBiographiesArtistes = this.artisteRepository.CountWithBiography(),
                ArtistesPlusChroniques = this.artisteRepository.GetArtistePlusChronique()?.Nom ?? "Aucun",

                NbStyles = this.styleRepository.Count(),
            };
        }
    }
}

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Webzine.Entity;
    using Webzine.Entity.Fixtures;

    [Area("Administration")]
    [Route("administration")]
    /// <summary>
    /// Contrôleur d'administration des titres.
    /// Cette partie est orientée IHM : aucune persistance n'est effectuée.
    /// Les données proviennent des Factory et les modifications/créations sont simulées via TempData.
    /// </summary>
    public class TitreController : Controller
    {
        private static string GetTitreOverrideKey(int id) => $"ui:admin:titre:{id}:override";

        private static string GetTitreStylesKey(int id) => $"ui:admin:titre:{id}:styles";

        private const string CreatedTitresKey = "ui:admin:titres:created";
        private const string DeletedTitresKey = "ui:admin:titres:deleted";

        private static string GetTitreFormKey(int id) => $"ui:admin:titre:{id}:form";

        private static void ApplyFormIfAny(Titre titre, string? formPayload)
        {
            if (string.IsNullOrWhiteSpace(formPayload))
            {
                return;
            }

            var parts = formPayload.Split('|');
            if (parts.Length != 8)
            {
                return;
            }

            titre.Libelle = parts[0];
            titre.Album = parts[1];
            titre.UrlEcoute = parts[2];
            titre.UrlJaquette = parts[3];
            titre.Chronique = parts[4];

            if (int.TryParse(parts[5], out var idArtiste))
            {
                titre.IdArtiste = idArtiste;
            }

            if (DateTime.TryParse(parts[6], out var dateSortie))
            {
                titre.DateSortie = dateSortie;
            }

            if (int.TryParse(parts[7], out var lectures))
            {
                titre.NbLectures = lectures;
            }
        }

        private static string EscapePipe(string? value) => (value ?? string.Empty).Replace("|", " ");

        /// <summary>
        /// Applique (si présent) un override IHM sur un titre (ex: durée, likes).
        /// </summary>
        /// <param name="titre">Titre à modifier.</param>
        /// <param name="overridePayload">Payload TempData.</param>
        private static void ApplyOverrideIfAny(Titre titre, string? overridePayload)
        {
            if (string.IsNullOrWhiteSpace(overridePayload))
            {
                return;
            }

            var parts = overridePayload.Split('|');
            if (parts.Length != 2)
            {
                return;
            }

            if (TimeOnly.TryParseExact(parts[0], "HH:mm:ss", out var duree)
                || TimeOnly.TryParseExact(parts[0], "mm:ss", out duree)
                || TimeOnly.TryParse(parts[0], out duree))
            {
                titre.Duree = duree;
            }

            if (int.TryParse(parts[1], out var likes))
            {
                titre.NbLikes = likes;
            }
        }

        [Route("titres")]
        /// <summary>
        /// Affiche la liste des titres (Factory + créations IHM) en réappliquant les overrides TempData.
        /// </summary>
        public IActionResult Index()
        {
            var titres = TitreFactory.GetTitres().ToList();

            var deletedRaw = TempData.Peek(DeletedTitresKey)?.ToString() ?? string.Empty;
            var deletedIds = deletedRaw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
                .Where(x => x is not null)
                .Select(x => x!.Value)
                .ToHashSet();

            var createdPayload = TempData.Peek(CreatedTitresKey)?.ToString();
            if (!string.IsNullOrWhiteSpace(createdPayload))
            {
                foreach (var raw in createdPayload.Split(";;", StringSplitOptions.RemoveEmptyEntries))
                {
                    var p = raw.Split('|');
                    if (p.Length < 3)
                    {
                        continue;
                    }

                    if (!int.TryParse(p[0], out var id))
                    {
                        continue;
                    }

                    if (deletedIds.Contains(id))
                    {
                        continue;
                    }

                    if (!DateTime.TryParse(p[2], out var dateSortie))
                    {
                        dateSortie = DateTime.Today;
                    }

                    titres.Add(new Titre
                    {
                        IdTitre = id,
                        Libelle = p[1],
                        DateSortie = dateSortie,
                        DateCreation = DateTime.Now,
                        UrlJaquette = string.Empty,
                        UrlEcoute = string.Empty,
                        Chronique = string.Empty,
                        Album = string.Empty,
                        Commentaires = new List<Webzine.Entity.Commentaire>(),
                        Styles = new List<Style>(),
                    });
                }
            }

            titres = titres.Where(t => !deletedIds.Contains(t.IdTitre)).ToList();

            foreach (var t in titres)
            {
                ApplyFormIfAny(t, TempData.Peek(GetTitreFormKey(t.IdTitre))?.ToString());
                var key = GetTitreOverrideKey(t.IdTitre);
                ApplyOverrideIfAny(t, TempData.Peek(key)?.ToString());

                var styleIdsRaw = TempData.Peek(GetTitreStylesKey(t.IdTitre))?.ToString();
                if (!string.IsNullOrWhiteSpace(styleIdsRaw))
                {
                    var all = LocalStylesFactory.CreerStyles();
                    var ids = styleIdsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.TryParse(x, out var v) ? v : (int?)null)
                        .Where(x => x is not null)
                        .Select(x => x!.Value)
                        .ToHashSet();

                    t.Styles = all.Where(s => ids.Contains(s.IdStyle)).ToList();
                }
            }
            return View(titres);
        }

        [HttpGet]
        [Route("titre/create")]
        /// <summary>
        /// Affiche le formulaire de création d'un titre.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("titre/create")]
        /// <summary>
        /// Simule la création d'un titre côté IHM (TempData), puis redirige vers la liste.
        /// </summary>
        /// <param name="titre">Titre saisi via le formulaire.</param>
        public IActionResult Create(Titre titre)
        {
            var existing = TempData.Peek(CreatedTitresKey)?.ToString() ?? string.Empty;
            var newId = titre.IdTitre;
            if (newId <= 0)
            {
                newId = (TitreFactory.GetTitres().Max(t => t.IdTitre) + 1);
            }

            var payload = $"{newId}|{titre.Libelle}|{titre.DateSortie:O}";
            var combined = string.IsNullOrWhiteSpace(existing) ? payload : (existing + ";;" + payload);
            TempData[CreatedTitresKey] = combined;

            var dureeRaw = Request.Form["Duree"].ToString();
            if (!string.IsNullOrWhiteSpace(dureeRaw))
            {
                var p = dureeRaw.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (p.Length == 2
                    && int.TryParse(p[0], out var minutes)
                    && int.TryParse(p[1], out var seconds))
                {
                    minutes = Math.Clamp(minutes, 0, 59);
                    seconds = Math.Clamp(seconds, 0, 59);
                    titre.Duree = new TimeOnly(0, minutes, seconds);
                }
            }

            TempData[GetTitreOverrideKey(newId)] = $"{titre.Duree:HH\\:mm\\:ss}|{titre.NbLikes}";

            TempData[GetTitreFormKey(newId)] = string.Join("|", new[]
            {
                EscapePipe(titre.Libelle),
                EscapePipe(titre.Album),
                EscapePipe(titre.UrlEcoute),
                EscapePipe(titre.UrlJaquette),
                EscapePipe(titre.Chronique),
                titre.IdArtiste.ToString(),
                titre.DateSortie.ToString("O"),
                titre.NbLectures.ToString(),
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("titre/edit/{id}")]
        /// <summary>
        /// Affiche le formulaire d'édition d'un titre.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        public IActionResult Edit(int id)
        {
            var titre = TitreFactory.GetTitreById(id);
            if (titre == null)
            {
                return NotFound();
            }

            ApplyFormIfAny(titre, TempData.Peek(GetTitreFormKey(id))?.ToString());
            ApplyOverrideIfAny(titre, TempData.Peek(GetTitreOverrideKey(id))?.ToString());

            ViewBag.AllStyles = LocalStylesFactory.CreerStyles();
            ViewBag.SelectedStyleIds = (TempData.Peek(GetTitreStylesKey(id))?.ToString() ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries);
            return View(titre);
        }

        [HttpPost]
        [Route("titre/edit/{id}")]
        /// <summary>
        /// Simule l'édition d'un titre côté IHM (TempData), puis redirige vers la liste.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        /// <param name="titre">Valeurs postées par le formulaire.</param>
        public IActionResult Edit(int id, Titre titre)
        {
            if (id != titre.IdTitre)
            {
                return BadRequest();
            }

            var dureeRaw = Request.Form["Duree"].ToString();
            if (!string.IsNullOrWhiteSpace(dureeRaw))
            {
                var p = dureeRaw.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (p.Length == 2
                    && int.TryParse(p[0], out var minutes)
                    && int.TryParse(p[1], out var seconds))
                {
                    minutes = Math.Clamp(minutes, 0, 59);
                    seconds = Math.Clamp(seconds, 0, 59);
                    titre.Duree = new TimeOnly(0, minutes, seconds);
                }
            }

            TempData[GetTitreOverrideKey(id)] = $"{titre.Duree:HH\\:mm\\:ss}|{titre.NbLikes}";

            TempData[GetTitreFormKey(id)] = string.Join("|", new[]
            {
                EscapePipe(titre.Libelle),
                EscapePipe(titre.Album),
                EscapePipe(titre.UrlEcoute),
                EscapePipe(titre.UrlJaquette),
                EscapePipe(titre.Chronique),
                titre.IdArtiste.ToString(),
                titre.DateSortie.ToString("O"),
                titre.NbLectures.ToString(),
            });

            var selectedStyleIds = Request.Form["SelectedStyleIds"].ToArray();
            TempData[GetTitreStylesKey(id)] = string.Join(',', selectedStyleIds);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("titre/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var titre = TitreFactory.GetTitreById(id);
            if (titre == null)
            {
                return NotFound();
            }

            return View(titre);
        }

        [HttpPost]
        [Route("titre/delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var existing = TempData.Peek(DeletedTitresKey)?.ToString() ?? string.Empty;
            var ids = existing.Split(',', StringSplitOptions.RemoveEmptyEntries).ToHashSet(StringComparer.OrdinalIgnoreCase);
            ids.Add(id.ToString());
            TempData[DeletedTitresKey] = string.Join(',', ids);
            return RedirectToAction(nameof(Index));
        }

    }
}

namespace Webzine.Entity.Fixtures
{
    /// <summary>
    /// Factory pour la création d'instances de la classe Titre.
    /// Cette classe peut être utilisée pour centraliser la logique de création d'objets Titre,
    /// notamment si la création de ces objets nécessite des étapes supplémentaires ou des valeurs par défaut.
    /// </summary>
    public class TitreFactory
    {
        private readonly ICollection<Titre> titres;

        public TitreFactory()
        {
            var styles = LocalStylesFactory.CreerStyles();
            var artistes = ArtisteFactory.CreerArtistes(8);

            // Helper pour récupérer un style existant par id
            Style GetStyle(int id) => styles.First(s => s.IdStyle == id);

            var titresList = new List<Titre>
            {
                new Titre
                {
                    IdTitre = 1,
                    Libelle = "La javanaise",
                    Album = "Windowlicker",
                    Chronique = "Breaks imprévisibles, sound design chirurgical et humour noir. Un titre qui montre la maîtrise de la production électronique.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut lobortis tellus a turpis elementum luctus. Proin euismod ante id facilisis placerat. Nam sed diam enim. Sed venenatis lacus ullamcorper, volutpat velit at, fringilla est. Donec interdum, arcu at ultrices eleifend, magna odio commodo mauris, vel facilisis velit est at purus. Integer quam nisl, consequat quis laoreet ut, dignissim a lacus. Aliquam eget finibus sem. Suspendisse potenti. Maecenas imperdiet eros purus, et gravida lectus semper et. Sed leo eros, maximus eget neque facilisis, interdum iaculis quam. Nulla tristique pulvinar viverra. Donec interdum metus sit amet blandit malesuada. Integer felis risus, fringilla non gravida et, varius sit amet ligula. Nunc efficitur purus at mollis fringilla.\r\n\r\nNam eleifend nulla a faucibus congue. Nulla quam sapien, volutpat ac arcu vel, luctus euismod enim. Nulla condimentum mauris eget tellus ultricies, non pharetra ex ultricies. Maecenas molestie dignissim nunc. Etiam nunc ex, finibus mattis leo eu, mattis euismod dui. Ut efficitur quam sagittis velit sollicitudin lacinia. In sed eros arcu. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vestibulum tincidunt metus ex, sit amet semper erat lobortis eget. Maecenas pretium est quis gravida hendrerit. Proin lorem ligula, finibus et laoreet at, pretium sit amet orci. In convallis arcu orci, at placerat urna semper id.\r\n\r\nMauris sapien orci, ultrices ut vehicula id, placerat vitae ex. Praesent vel augue tincidunt, mollis mi eget, egestas sem. Cras ut tempor est. Ut quis fringilla diam, vitae hendrerit ante. Phasellus vitae aliquet nulla. Sed a felis ante. Curabitur posuere, libero quis faucibus mattis, nibh elit consectetur lectus, vitae faucibus erat tellus et mi. Suspendisse eu est nec dui aliquam pulvinar. Cras semper orci vitae dolor convallis tincidunt. Fusce ut malesuada lorem.\r\n\r\nMaecenas erat est, tincidunt quis consectetur eget, suscipit eu tortor. Fusce semper sem arcu, sit amet aliquet odio venenatis sed. Curabitur vel laoreet sem, ac interdum metus. In rutrum lacinia lacus. Mauris egestas, dui sit amet commodo dignissim, enim turpis pellentesque quam, et aliquam enim sapien eu odio. Mauris tincidunt justo nec laoreet aliquam. Duis vitae feugiat ipsum. Duis ac blandit ligula.\r\n\r\nInteger leo diam, pretium ac pharetra sit amet, laoreet nec enim. Proin nulla ipsum, lacinia eu finibus in, sodales a leo. Duis dictum pharetra magna, ut convallis diam auctor vitae. Morbi ac nunc ac sapien congue luctus. Proin sapien quam, elementum eu posuere in, tristique sed ex. Vestibulum et dui luctus urna dictum vehicula ut vel magna. Sed feugiat ac nisi ut ornare. Nulla facilisi. Morbi lorem elit, malesuada ac dignissim quis, malesuada quis quam. Phasellus non dignissim est. Ut cursus turpis tellus, eget interdum mi rhoncus eu. Pellentesque sed nulla cursus, tristique magna mattis, euismod ligula. Aenean risus erat, condimentum eget orci sit amet, feugiat aliquet massa. Nullam erat quam, varius et nibh quis, elementum euismod eros. Sed convallis viverra metus sed dignissim. Etiam in tortor eget sapien ultrices ultricies ut a quam. ",
                    UrlJaquette = "https://m.media-amazon.com/images/I/519E9mjhELL._UF1000,1000_QL80_.jpg",
                    UrlEcoute = "https://www.youtube.com/embed/V6gjzNm6dA0?si=rHcow2XWlbbJFyw5",
                    DateCreation = DateTime.Now.AddDays(-25),
                    DateSortie = new DateTime(2016, 11, 21),
                    Duree = new TimeOnly(0, 7, 44),
                    NbLectures = 156,
                    NbLikes = 18,
                    IdArtiste = 1,
                    Artiste = artistes.First(a => a.IdArtiste == 1),
                    Commentaires = new List<Commentaire>(),
                    Styles = new List<Style> { GetStyle(9), GetStyle(3) },
                },
                new Titre
                {
                    IdTitre = 2,
                    Libelle = "One More Time",
                    Album = "Discovery",
                    Chronique = "Un hymne house/disco ultra efficace. Groove, voix filtrée et énergie positive: impossible de résister.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut lobortis tellus a turpis elementum luctus. Proin euismod ante id facilisis placerat. Nam sed diam enim. Sed venenatis lacus ullamcorper, volutpat velit at, fringilla est. Donec interdum, arcu at ultrices eleifend, magna odio commodo mauris, vel facilisis velit est at purus. Integer quam nisl, consequat quis laoreet ut, dignissim a lacus. Aliquam eget finibus sem. Suspendisse potenti. Maecenas imperdiet eros purus, et gravida lectus semper et. Sed leo eros, maximus eget neque facilisis, interdum iaculis quam. Nulla tristique pulvinar viverra. Donec interdum metus sit amet blandit malesuada. Integer felis risus, fringilla non gravida et, varius sit amet ligula. Nunc efficitur purus at mollis fringilla.\r\n\r\nNam eleifend nulla a faucibus congue. Nulla quam sapien, volutpat ac arcu vel, luctus euismod enim. Nulla condimentum mauris eget tellus ultricies, non pharetra ex ultricies. Maecenas molestie dignissim nunc. Etiam nunc ex, finibus mattis leo eu, mattis euismod dui. Ut efficitur quam sagittis velit sollicitudin lacinia. In sed eros arcu. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vestibulum tincidunt metus ex, sit amet semper erat lobortis eget. Maecenas pretium est quis gravida hendrerit. Proin lorem ligula, finibus et laoreet at, pretium sit amet orci. In convallis arcu orci, at placerat urna semper id.\r\n\r\nMauris sapien orci, ultrices ut vehicula id, placerat vitae ex. Praesent vel augue tincidunt, mollis mi eget, egestas sem. Cras ut tempor est. Ut quis fringilla diam, vitae hendrerit ante. Phasellus vitae aliquet nulla. Sed a felis ante. Curabitur posuere, libero quis faucibus mattis, nibh elit consectetur lectus, vitae faucibus erat tellus et mi. Suspendisse eu est nec dui aliquam pulvinar. Cras semper orci vitae dolor convallis tincidunt. Fusce ut malesuada lorem.\r\n\r\nMaecenas erat est, tincidunt quis consectetur eget, suscipit eu tortor. Fusce semper sem arcu, sit amet aliquet odio venenatis sed. Curabitur vel laoreet sem, ac interdum metus. In rutrum lacinia lacus. Mauris egestas, dui sit amet commodo dignissim, enim turpis pellentesque quam, et aliquam enim sapien eu odio. Mauris tincidunt justo nec laoreet aliquam. Duis vitae feugiat ipsum. Duis ac blandit ligula.\r\n\r\nInteger leo diam, pretium ac pharetra sit amet, laoreet nec enim. Proin nulla ipsum, lacinia eu finibus in, sodales a leo. Duis dictum pharetra magna, ut convallis diam auctor vitae. Morbi ac nunc ac sapien congue luctus. Proin sapien quam, elementum eu posuere in, tristique sed ex. Vestibulum et dui luctus urna dictum vehicula ut vel magna. Sed feugiat ac nisi ut ornare. Nulla facilisi. Morbi lorem elit, malesuada ac dignissim quis, malesuada quis quam. Phasellus non dignissim est. Ut cursus turpis tellus, eget interdum mi rhoncus eu. Pellentesque sed nulla cursus, tristique magna mattis, euismod ligula. Aenean risus erat, condimentum eget orci sit amet, feugiat aliquet massa. Nullam erat quam, varius et nibh quis, elementum euismod eros. Sed convallis viverra metus sed dignissim. Etiam in tortor eget sapien ultrices ultricies ut a quam. ",
                    UrlJaquette = "https://picsum.photos/seed/onemoretime/640/360",
                    UrlEcoute = "https://www.youtube.com/embed/FGBhQbmPwH8?si=RCAFdqEmmxCs8flB",
                    DateCreation = DateTime.Now.AddDays(-60),
                    DateSortie = new DateTime(2001, 01, 01),
                    Duree = new TimeOnly(0, 5, 20),
                    NbLectures = 981,
                    NbLikes = 240,
                    IdArtiste = 2,
                    Artiste = artistes.First(a => a.IdArtiste == 2),
                    Commentaires = new List<Commentaire>(),
                    Styles = new List<Style> { GetStyle(5), GetStyle(4) },
                },
                new Titre
                {
                    IdTitre = 3,
                    Libelle = "Dayvan Cowboy",
                    Album = "The Campfire Headphase",
                    Chronique = "Une montée lumineuse, guitares et textures analogiques. Mélancolique mais plein d'espoir.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut lobortis tellus a turpis elementum luctus. Proin euismod ante id facilisis placerat. Nam sed diam enim. Sed venenatis lacus ullamcorper, volutpat velit at, fringilla est. Donec interdum, arcu at ultrices eleifend, magna odio commodo mauris, vel facilisis velit est at purus. Integer quam nisl, consequat quis laoreet ut, dignissim a lacus. Aliquam eget finibus sem. Suspendisse potenti. Maecenas imperdiet eros purus, et gravida lectus semper et. Sed leo eros, maximus eget neque facilisis, interdum iaculis quam. Nulla tristique pulvinar viverra. Donec interdum metus sit amet blandit malesuada. Integer felis risus, fringilla non gravida et, varius sit amet ligula. Nunc efficitur purus at mollis fringilla.\r\n\r\nNam eleifend nulla a faucibus congue. Nulla quam sapien, volutpat ac arcu vel, luctus euismod enim. Nulla condimentum mauris eget tellus ultricies, non pharetra ex ultricies. Maecenas molestie dignissim nunc. Etiam nunc ex, finibus mattis leo eu, mattis euismod dui. Ut efficitur quam sagittis velit sollicitudin lacinia. In sed eros arcu. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vestibulum tincidunt metus ex, sit amet semper erat lobortis eget. Maecenas pretium est quis gravida hendrerit. Proin lorem ligula, finibus et laoreet at, pretium sit amet orci. In convallis arcu orci, at placerat urna semper id.\r\n\r\nMauris sapien orci, ultrices ut vehicula id, placerat vitae ex. Praesent vel augue tincidunt, mollis mi eget, egestas sem. Cras ut tempor est. Ut quis fringilla diam, vitae hendrerit ante. Phasellus vitae aliquet nulla. Sed a felis ante. Curabitur posuere, libero quis faucibus mattis, nibh elit consectetur lectus, vitae faucibus erat tellus et mi. Suspendisse eu est nec dui aliquam pulvinar. Cras semper orci vitae dolor convallis tincidunt. Fusce ut malesuada lorem.\r\n\r\nMaecenas erat est, tincidunt quis consectetur eget, suscipit eu tortor. Fusce semper sem arcu, sit amet aliquet odio venenatis sed. Curabitur vel laoreet sem, ac interdum metus. In rutrum lacinia lacus. Mauris egestas, dui sit amet commodo dignissim, enim turpis pellentesque quam, et aliquam enim sapien eu odio. Mauris tincidunt justo nec laoreet aliquam. Duis vitae feugiat ipsum. Duis ac blandit ligula.\r\n\r\nInteger leo diam, pretium ac pharetra sit amet, laoreet nec enim. Proin nulla ipsum, lacinia eu finibus in, sodales a leo. Duis dictum pharetra magna, ut convallis diam auctor vitae. Morbi ac nunc ac sapien congue luctus. Proin sapien quam, elementum eu posuere in, tristique sed ex. Vestibulum et dui luctus urna dictum vehicula ut vel magna. Sed feugiat ac nisi ut ornare. Nulla facilisi. Morbi lorem elit, malesuada ac dignissim quis, malesuada quis quam. Phasellus non dignissim est. Ut cursus turpis tellus, eget interdum mi rhoncus eu. Pellentesque sed nulla cursus, tristique magna mattis, euismod ligula. Aenean risus erat, condimentum eget orci sit amet, feugiat aliquet massa. Nullam erat quam, varius et nibh quis, elementum euismod eros. Sed convallis viverra metus sed dignissim. Etiam in tortor eget sapien ultrices ultricies ut a quam. ",
                    UrlJaquette = "https://picsum.photos/seed/dayvancowboy/640/360",
                    UrlEcoute = "https://www.youtube.com/embed/A2zKARkpDW4",
                    DateCreation = DateTime.Now.AddDays(-18),
                    DateSortie = new DateTime(2006, 10, 17),
                    Duree = new TimeOnly(0, 5, 56),
                    NbLectures = 312,
                    NbLikes = 55,
                    IdArtiste = 3,
                    Artiste = artistes.First(a => a.IdArtiste == 3),
                    Commentaires = new List<Commentaire>(),
                    Styles = new List<Style> { GetStyle(2), GetStyle(6) },
                },
                new Titre
                {
                    IdTitre = 4,
                    Libelle = "Glue",
                    Album = "Glue (Single)",
                    Chronique = "Breakbeat nostalgique, pads émotionnels et un drop qui tombe parfaitement. Un morceau taillé pour la scène.  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut lobortis tellus a turpis elementum luctus. Proin euismod ante id facilisis placerat. Nam sed diam enim. Sed venenatis lacus ullamcorper, volutpat velit at, fringilla est. Donec interdum, arcu at ultrices eleifend, magna odio commodo mauris, vel facilisis velit est at purus. Integer quam nisl, consequat quis laoreet ut, dignissim a lacus. Aliquam eget finibus sem. Suspendisse potenti. Maecenas imperdiet eros purus, et gravida lectus semper et. Sed leo eros, maximus eget neque facilisis, interdum iaculis quam. Nulla tristique pulvinar viverra. Donec interdum metus sit amet blandit malesuada. Integer felis risus, fringilla non gravida et, varius sit amet ligula. Nunc efficitur purus at mollis fringilla.\r\n\r\nNam eleifend nulla a faucibus congue. Nulla quam sapien, volutpat ac arcu vel, luctus euismod enim. Nulla condimentum mauris eget tellus ultricies, non pharetra ex ultricies. Maecenas molestie dignissim nunc. Etiam nunc ex, finibus mattis leo eu, mattis euismod dui. Ut efficitur quam sagittis velit sollicitudin lacinia. In sed eros arcu. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vestibulum tincidunt metus ex, sit amet semper erat lobortis eget. Maecenas pretium est quis gravida hendrerit. Proin lorem ligula, finibus et laoreet at, pretium sit amet orci. In convallis arcu orci, at placerat urna semper id.\r\n\r\nMauris sapien orci, ultrices ut vehicula id, placerat vitae ex. Praesent vel augue tincidunt, mollis mi eget, egestas sem. Cras ut tempor est. Ut quis fringilla diam, vitae hendrerit ante. Phasellus vitae aliquet nulla. Sed a felis ante. Curabitur posuere, libero quis faucibus mattis, nibh elit consectetur lectus, vitae faucibus erat tellus et mi. Suspendisse eu est nec dui aliquam pulvinar. Cras semper orci vitae dolor convallis tincidunt. Fusce ut malesuada lorem.\r\n\r\nMaecenas erat est, tincidunt quis consectetur eget, suscipit eu tortor. Fusce semper sem arcu, sit amet aliquet odio venenatis sed. Curabitur vel laoreet sem, ac interdum metus. In rutrum lacinia lacus. Mauris egestas, dui sit amet commodo dignissim, enim turpis pellentesque quam, et aliquam enim sapien eu odio. Mauris tincidunt justo nec laoreet aliquam. Duis vitae feugiat ipsum. Duis ac blandit ligula.\r\n\r\nInteger leo diam, pretium ac pharetra sit amet, laoreet nec enim. Proin nulla ipsum, lacinia eu finibus in, sodales a leo. Duis dictum pharetra magna, ut convallis diam auctor vitae. Morbi ac nunc ac sapien congue luctus. Proin sapien quam, elementum eu posuere in, tristique sed ex. Vestibulum et dui luctus urna dictum vehicula ut vel magna. Sed feugiat ac nisi ut ornare. Nulla facilisi. Morbi lorem elit, malesuada ac dignissim quis, malesuada quis quam. Phasellus non dignissim est. Ut cursus turpis tellus, eget interdum mi rhoncus eu. Pellentesque sed nulla cursus, tristique magna mattis, euismod ligula. Aenean risus erat, condimentum eget orci sit amet, feugiat aliquet massa. Nullam erat quam, varius et nibh quis, elementum euismod eros. Sed convallis viverra metus sed dignissim. Etiam in tortor eget sapien ultrices ultricies ut a quam. ",
                    UrlJaquette = "https://picsum.photos/seed/glue/640/360",
                    UrlEcoute = "https://www.youtube.com/embed/A7ZxRs45tTg",
                    DateCreation = DateTime.Now.AddDays(-7),
                    DateSortie = new DateTime(2017, 10, 01),
                    Duree = new TimeOnly(0, 4, 36),
                    NbLectures = 225,
                    NbLikes = 67,
                    IdArtiste = 4,
                    Artiste = artistes.First(a => a.IdArtiste == 4),
                    Commentaires = new List<Commentaire>(),
                    Styles = new List<Style> { GetStyle(4), GetStyle(9) },
                },
                new Titre
                {
                    IdTitre = 5,
                    Libelle = "The Model",
                    Album = "The Man-Machine",
                    Chronique = "Minimalisme, mélodie froide et efficacité maximale. Un classique qui a défini la pop électronique. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut lobortis tellus a turpis elementum luctus. Proin euismod ante id facilisis placerat. Nam sed diam enim. Sed venenatis lacus ullamcorper, volutpat velit at, fringilla est. Donec interdum, arcu at ultrices eleifend, magna odio commodo mauris, vel facilisis velit est at purus. Integer quam nisl, consequat quis laoreet ut, dignissim a lacus. Aliquam eget finibus sem. Suspendisse potenti. Maecenas imperdiet eros purus, et gravida lectus semper et. Sed leo eros, maximus eget neque facilisis, interdum iaculis quam. Nulla tristique pulvinar viverra. Donec interdum metus sit amet blandit malesuada. Integer felis risus, fringilla non gravida et, varius sit amet ligula. Nunc efficitur purus at mollis fringilla.\r\n\r\nNam eleifend nulla a faucibus congue. Nulla quam sapien, volutpat ac arcu vel, luctus euismod enim. Nulla condimentum mauris eget tellus ultricies, non pharetra ex ultricies. Maecenas molestie dignissim nunc. Etiam nunc ex, finibus mattis leo eu, mattis euismod dui. Ut efficitur quam sagittis velit sollicitudin lacinia. In sed eros arcu. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vestibulum tincidunt metus ex, sit amet semper erat lobortis eget. Maecenas pretium est quis gravida hendrerit. Proin lorem ligula, finibus et laoreet at, pretium sit amet orci. In convallis arcu orci, at placerat urna semper id.\r\n\r\nMauris sapien orci, ultrices ut vehicula id, placerat vitae ex. Praesent vel augue tincidunt, mollis mi eget, egestas sem. Cras ut tempor est. Ut quis fringilla diam, vitae hendrerit ante. Phasellus vitae aliquet nulla. Sed a felis ante. Curabitur posuere, libero quis faucibus mattis, nibh elit consectetur lectus, vitae faucibus erat tellus et mi. Suspendisse eu est nec dui aliquam pulvinar. Cras semper orci vitae dolor convallis tincidunt. Fusce ut malesuada lorem.\r\n\r\nMaecenas erat est, tincidunt quis consectetur eget, suscipit eu tortor. Fusce semper sem arcu, sit amet aliquet odio venenatis sed. Curabitur vel laoreet sem, ac interdum metus. In rutrum lacinia lacus. Mauris egestas, dui sit amet commodo dignissim, enim turpis pellentesque quam, et aliquam enim sapien eu odio. Mauris tincidunt justo nec laoreet aliquam. Duis vitae feugiat ipsum. Duis ac blandit ligula.\r\n\r\nInteger leo diam, pretium ac pharetra sit amet, laoreet nec enim. Proin nulla ipsum, lacinia eu finibus in, sodales a leo. Duis dictum pharetra magna, ut convallis diam auctor vitae. Morbi ac nunc ac sapien congue luctus. Proin sapien quam, elementum eu posuere in, tristique sed ex. Vestibulum et dui luctus urna dictum vehicula ut vel magna. Sed feugiat ac nisi ut ornare. Nulla facilisi. Morbi lorem elit, malesuada ac dignissim quis, malesuada quis quam. Phasellus non dignissim est. Ut cursus turpis tellus, eget interdum mi rhoncus eu. Pellentesque sed nulla cursus, tristique magna mattis, euismod ligula. Aenean risus erat, condimentum eget orci sit amet, feugiat aliquet massa. Nullam erat quam, varius et nibh quis, elementum euismod eros. Sed convallis viverra metus sed dignissim. Etiam in tortor eget sapien ultrices ultricies ut a quam. ",
                    UrlJaquette = "https://picsum.photos/seed/themodel/640/360",
                    UrlEcoute = "https://www.youtube.com/embed/gMrsOmkYhIk",
                    DateCreation = DateTime.Now.AddDays(-90),
                    DateSortie = new DateTime(1978, 05, 19),
                    Duree = new TimeOnly(0, 3, 46),
                    NbLectures = 1156,
                    NbLikes = 344,
                    IdArtiste = 5,
                    Artiste = artistes.First(a => a.IdArtiste == 5),
                    Commentaires = new List<Commentaire>(),
                    Styles = new List<Style> { GetStyle(9) },
                },
                new Titre
                {
                    IdTitre = 6,
                    Libelle = "Digital Love",
                    Album = "Discovery",
                    Chronique = "Une pépite disco-house rêveuse : guitare funky, vocoder doux et mélodie qui reste en tête. Nostalgique mais lumineuse, elle résume l’esprit de Discovery. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut lobortis tellus a turpis elementum luctus. Proin euismod ante id facilisis placerat. Nam sed diam enim. Sed venenatis lacus ullamcorper, volutpat velit at, fringilla est. Donec interdum, arcu at ultrices eleifend, magna odio commodo mauris, vel facilisis velit est at purus. Integer quam nisl, consequat quis laoreet ut, dignissim a lacus. Aliquam eget finibus sem. Suspendisse potenti. Maecenas imperdiet eros purus, et gravida lectus semper et. Sed leo eros, maximus eget neque facilisis, interdum iaculis quam. Nulla tristique pulvinar viverra. Donec interdum metus sit amet blandit malesuada. Integer felis risus, fringilla non gravida et, varius sit amet ligula. Nunc efficitur purus at mollis fringilla.\r\n\r\nNam eleifend nulla a faucibus congue. Nulla quam sapien, volutpat ac arcu vel, luctus euismod enim. Nulla condimentum mauris eget tellus ultricies, non pharetra ex ultricies. Maecenas molestie dignissim nunc. Etiam nunc ex, finibus mattis leo eu, mattis euismod dui. Ut efficitur quam sagittis velit sollicitudin lacinia. In sed eros arcu. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vestibulum tincidunt metus ex, sit amet semper erat lobortis eget. Maecenas pretium est quis gravida hendrerit. Proin lorem ligula, finibus et laoreet at, pretium sit amet orci. In convallis arcu orci, at placerat urna semper id.\r\n\r\nMauris sapien orci, ultrices ut vehicula id, placerat vitae ex. Praesent vel augue tincidunt, mollis mi eget, egestas sem. Cras ut tempor est. Ut quis fringilla diam, vitae hendrerit ante. Phasellus vitae aliquet nulla. Sed a felis ante. Curabitur posuere, libero quis faucibus mattis, nibh elit consectetur lectus, vitae faucibus erat tellus et mi. Suspendisse eu est nec dui aliquam pulvinar. Cras semper orci vitae dolor convallis tincidunt. Fusce ut malesuada lorem.\r\n\r\nMaecenas erat est, tincidunt quis consectetur eget, suscipit eu tortor. Fusce semper sem arcu, sit amet aliquet odio venenatis sed. Curabitur vel laoreet sem, ac interdum metus. In rutrum lacinia lacus. Mauris egestas, dui sit amet commodo dignissim, enim turpis pellentesque quam, et aliquam enim sapien eu odio. Mauris tincidunt justo nec laoreet aliquam. Duis vitae feugiat ipsum. Duis ac blandit ligula.\r\n\r\nInteger leo diam, pretium ac pharetra sit amet, laoreet nec enim. Proin nulla ipsum, lacinia eu finibus in, sodales a leo. Duis dictum pharetra magna, ut convallis diam auctor vitae. Morbi ac nunc ac sapien congue luctus. Proin sapien quam, elementum eu posuere in, tristique sed ex. Vestibulum et dui luctus urna dictum vehicula ut vel magna. Sed feugiat ac nisi ut ornare. Nulla facilisi. Morbi lorem elit, malesuada ac dignissim quis, malesuada quis quam. Phasellus non dignissim est. Ut cursus turpis tellus, eget interdum mi rhoncus eu. Pellentesque sed nulla cursus, tristique magna mattis, euismod ligula. Aenean risus erat, condimentum eget orci sit amet, feugiat aliquet massa. Nullam erat quam, varius et nibh quis, elementum euismod eros. Sed convallis viverra metus sed dignissim. Etiam in tortor eget sapien ultrices ultricies ut a quam. ",
                    UrlJaquette = "https://picsum.photos/seed/onemoretime/640/360",
                    UrlEcoute = "https://www.youtube.com/embed/FxzBvqY5PP0",
                    DateCreation = DateTime.Now.AddDays(-60),
                    DateSortie = new DateTime(2001, 06, 11),
                    Duree = new TimeOnly(0, 4, 58),
                    NbLectures = 178,
                    NbLikes = 94,
                    IdArtiste = 2,
                    Artiste = artistes.First(a => a.IdArtiste == 2),
                    Commentaires = new List<Commentaire>(),
                    Styles = new List<Style> { GetStyle(5), GetStyle(4) },
                },
            };

            // commentaires de test sur un titre
            titresList[0].Commentaires.Add(new Commentaire
            {
                IdCommentaire = 1,
                Auteur = "Michel",
                DateCreation = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                Contenu = "Très bon son, la prod est vraiment propre.",
                IdTitre = titresList[0].IdTitre,
                Titre = titresList[0],
            });

            titresList[0].Commentaires.Add(new Commentaire
            {
                IdCommentaire = 2,
                Auteur = "Emma",
                DateCreation = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                Contenu = "J'aime beaucoup la rythmique, ça marche en boucle !",
                IdTitre = titresList[0].IdTitre,
                Titre = titresList[0],
            });

            this.titres = titresList;
        }

        /// <summary>
        /// Retourne une liste de titres préconfigurés. Cette méthode peut être utilisée pour obtenir des instances de Titre
        /// </summary>
        /// <returns>Liste de titres</returns>
        public static ICollection<Titre> GetTitres()
        {
            return new TitreFactory().titres;
        }

        /// <summary>
        /// Retourne un titre spécifique en fonction de son identifiant. Cette méthode peut être utilisée pour obtenir une instance de Titre.
        /// </summary>
        /// <param name="id">Identifiant du titre</param>
        /// <returns>Titre</returns>
        public static Titre? GetTitreById(int id)
        {
            return new TitreFactory().titres.FirstOrDefault(t => t.IdTitre == id);
        }

        /// <summary>
        /// Récupérer une liste de titres associés à un artiste spécifique en fonction de l'identifiant de l'artiste.
        /// </summary>
        /// <param name="idArtiste">L'identifiant de l'artiste</param>
        /// <returns>Liste de titres fait par l'artiste en paramètre</returns>
        public static List<Titre> TrouverParIdArtiste(int idArtiste)
        {
            return GetTitres().Where(t => t.IdArtiste == idArtiste).ToList();
        }
    }
}

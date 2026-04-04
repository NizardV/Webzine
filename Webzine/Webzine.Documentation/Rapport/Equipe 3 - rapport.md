## Introduction

Ce rappport permet d'analyser ce qu'il se passe et d'améliorer notre groupe. Le
groupe est composé de 5 membres répartis en : 4 devs et 1 ops. Les membres du
groupe sont :

- _CHAUDRILLIER_ Florian (**Ops**)
- _DI LENA_ Aurore
- _DUMONT_ Clément (**Chef de projet**)
- _TILLARD_ Guillaume
- _VERDENAL_ Nizard

L'équipe réalise des daily chaque matin dès 9h pour faire le point sur les
tâches de chacun, à savoir :

- Les tâches réalisées la veille
- Les tâches à réaliser dans la journée
- Les difficultés rencontrées

Cela permet à notre Ops de savoir où en est el développement de l'application,
et aux devs de savoir où en est l'Ops.

## Organisation de l'équipe Dev

Organisation de l'équipe Dev
L'organisation de l'équipe de développement a été pensée pour maximiser notre autonomie tout en favorisant la montée en compétence collective de chaque membre.
Face à un projet structuré autour de quatre entités principales, nous avons opté pour une répartition des tâches basée sur le découpage fonctionnel de ces données.

Stratégie de répartition des tâches:
Conscients de l'hétérogénéité des niveaux techniques au sein de l'équipe face aux technologies imposées,
nous avons adopté une approche stratégique et bienveillante. Les entités présentant la charge logique la plus légère ont été confiées aux développeurs nécessitant plus de temps d'adaptation.
À l'inverse, les entités structurellement plus complexes ont été prises en charge par les profils plus à l'aise.

L'objectif de ce choix était double :
Permettre une montée en compétence progressive, en commençant par un périmètre restreint pour bien assimiler l'architecture du projet sans générer de blocage.
Garantir que les développeurs assignés aux entités dites "simples", une fois leurs tâches achevées, puissent venir en renfort de leurs coéquipiers sur les entités plus complexes.

Outils et suivi de projet (Gitea)
Pour la gestion de notre backlog et le suivi quotidien, nous nous sommes appuyés sur Gitea. Cet outil étant nouveau pour l'équipe, une phase d'appropriation a été nécessaire en début de projet pour en maîtriser les concepts.
Nous avons structuré notre travail par jalons. Au début de chaque jalon, les tâches à accomplir étaient découpées en tickets et assignées selon notre répartition par entité.
Pour fluidifier la lecture de notre tableau de bord de suivi, nous avons mis en place une taxonomie stricte grâce aux étiquettes (labels) :

Étiquettes d'entité : Permettant d'identifier immédiatement la ressource impactée par le ticket.
Étiquettes de contexte (Scope) : Permettant de différencier les fonctionnalités destinées au panel d'administration de celles destinées à l'interface publique utilisateur.

Stratégie de versioning et qualité du code
Notre stratégie Git s'est initialement calquée sur notre répartition fonctionnelle. Nous avons mis en place une branche d'intégration principale (dev) sur laquelle les fonctionnalités terminées venaient s'agréger.
Durant la première phase du projet, nous avons travaillé avec une branche par entité (ex: feat/commentaires, feat/artistes).
Cette isolation stricte a permis à chaque développeur d'avancer en totale autonomie sur son périmètre, réduisant drastiquement le risque de conflits lors des fusions (merge conflicts).
Pour garantir la stabilité de l'application, aucune fusion vers la branche dev n'était autorisée sans validation. La procédure de Pull Request exigeait obligatoirement une revue de code effectuée par un autre développeur de l'équipe afin d'éviter l'introduction de bugs.

Évolution de l'organisation au fil des jalons
L'équipe a su faire preuve d'agilité en adaptant son organisation à l'évolution du projet :

Jalon 2 (Rotation des responsabilités) : Tout en conservant notre arborescence Git et notre gestion par tickets, nous avons imposé une rotation complète des sujets.
Chaque membre a dû reprendre le périmètre d'une autre entité.
Cette démarche volontaire visait à garantir que chaque développeur manipule l'ensemble du code, assurant ainsi une compréhension globale de l'application par l'ensemble de l'équipe.

Jalon 3 : Pour le dernier jalon, la structure de base et le CRUD des entités étant finalisés, nous avons fait évoluer notre stratégie Git.
Nous avons abandonné les branches par entité au profit de branches d'interventions très spécifiques (orientées Feature pour les développements transverses manquants, ou Fix pour la résolution de bugs).

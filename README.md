# Webzine

> Webzine musical avec chatbot RAG intégré / Music webzine with integrated RAG chatbot

[![C#](https://img.shields.io/badge/C%23-12-512BD4?style=flat&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/mvc)
[![Python](https://img.shields.io/badge/Python-3.11-3776AB?style=flat&logo=python&logoColor=white)](https://www.python.org)
[![LangChain](https://img.shields.io/badge/LangChain-latest-1C3C3C?style=flat)](https://www.langchain.com)
[![ChromaDB](https://img.shields.io/badge/ChromaDB-vector--db-E18A32?style=flat)](https://www.trychroma.com)
[![Ollama](https://img.shields.io/badge/Ollama-LLM-000000?style=flat)](https://ollama.com)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?style=flat&logo=postgresql&logoColor=white)](https://www.postgresql.org)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat&logo=docker&logoColor=white)](https://www.docker.com)

## 🇫🇷 Français | 🇬🇧 English

[Voir en français](#-présentation) | [View in English](#-overview)

---

## 🇫🇷 Présentation

Webzine est un webzine musical développé en ASP.NET Core MVC, enrichi d'un chatbot RAG (Retrieval-Augmented Generation) intégré. Le pipeline RAG est développé en Python avec LangChain, ChromaDB et Ollama pour répondre à des questions sur les artistes à partir des données Deezer. Projet P4 DIIAGE.

## Stack technique

| Composant | Technologies |
|-----------|-------------|
| Application web | C#, ASP.NET Core MVC, PostgreSQL, NLog, cache mémoire |
| Pipeline RAG | Python, LangChain, ChromaDB, Ollama, FastAPI |
| Données | API Deezer (ingestion), PostgreSQL |
| Infrastructure | Docker, Docker Compose |

## Architecture

```
Webzine/
├── Webzine/                    # Solution ASP.NET Core MVC (couches)
│   ├── Webzine.Entity/         # Entités du domaine
│   ├── Webzine.Repository/     # Accès aux données (PostgreSQL)
│   ├── Webzine.Business/       # Logique métier, cache mémoire
│   └── Webzine.WebApplication/ # Controllers MVC, vues Razor, NLog
└── rag/                        # Pipeline RAG (Python)
    ├── api.py                  # API FastAPI (endpoint /query)
    ├── pipeline_deezer.py      # Pipeline RAG sur les données Deezer
    └── fetch_deezer_data.py    # Ingestion des données Deezer
```

> Déploiement Docker : [README-DOCKER.md](./README-DOCKER.md) · [Webzine/README.md](./Webzine/README.md)

## Fonctionnalités principales

- CRUD artistes, titres, styles musicaux et commentaires
- Intégration API Deezer (récupération de données musicales)
- Chatbot RAG sur les artistes (questions/réponses en langage naturel)
- Cache mémoire côté application
- Logging structuré avec NLog
- Déploiement Docker multi-services (web + RAG + PostgreSQL + Ollama)

## Lancer en local

### Prérequis

- [Docker Desktop](https://www.docker.com/products/docker-desktop) v24.0+
- [Ollama](https://ollama.com) installé et lancé sur la machine hôte
- 4 Go de RAM disponibles, 3 Go d'espace disque

### Avec Docker Compose

```bash
git clone https://github.com/NizardV/Webzine
cd Webzine
cp Webzine/.env.example .env   # éditer les variables si nécessaire
docker compose build
docker compose up -d
```

Services accessibles :
- **Webzine** : http://localhost:7110
- **API RAG** : http://localhost:8000/docs
- **Base de données** : localhost:5432

### Vérifier les services

```bash
docker compose ps
docker compose logs -f
```

> Pour les commandes avancées (Makefile, scripts de validation, production) : voir [README-DOCKER.md](./README-DOCKER.md)

## Équipe

Projet P4 DIIAGE — 4 contributeurs

| Rôle | Nom |
|------|-----|
| Développeur | [À COMPLÉTER] |
| Développeur | [À COMPLÉTER] |
| Développeur | [À COMPLÉTER] |
| Développeur | [À COMPLÉTER] |

---

## 🇬🇧 Overview

Webzine is a music webzine built with ASP.NET Core MVC, enhanced with an integrated RAG (Retrieval-Augmented Generation) chatbot. The RAG pipeline is written in Python using LangChain, ChromaDB, and Ollama to answer questions about artists using Deezer data. P4 DIIAGE project.

## Tech stack

| Component | Technologies |
|-----------|-------------|
| Web application | C#, ASP.NET Core MVC, PostgreSQL, NLog, memory cache |
| RAG pipeline | Python, LangChain, ChromaDB, Ollama, FastAPI |
| Data | Deezer API (ingestion), PostgreSQL |
| Infrastructure | Docker, Docker Compose |

## Architecture

```
Webzine/
├── Webzine/                    # ASP.NET Core MVC solution (layered)
│   ├── Webzine.Entity/         # Domain entities
│   ├── Webzine.Repository/     # Data access (PostgreSQL)
│   ├── Webzine.Business/       # Business logic, memory cache
│   └── Webzine.WebApplication/ # MVC controllers, Razor views, NLog
└── rag/                        # RAG pipeline (Python)
    ├── api.py                  # FastAPI endpoint (/query)
    ├── pipeline_deezer.py      # RAG pipeline over Deezer data
    └── fetch_deezer_data.py    # Deezer data ingestion
```

> Docker deployment: [README-DOCKER.md](./README-DOCKER.md) · [Webzine/README.md](./Webzine/README.md)

## Key features

- CRUD for artists, tracks, musical styles, and comments
- Deezer API integration (music data retrieval)
- RAG chatbot for artists (natural language Q&A)
- Application-level memory cache
- Structured logging with NLog
- Multi-service Docker deployment (web + RAG + PostgreSQL + Ollama)

## Run locally

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop) v24.0+
- [Ollama](https://ollama.com) installed and running on the host machine
- 4 GB available RAM, 3 GB disk space

### With Docker Compose

```bash
git clone https://github.com/NizardV/Webzine
cd Webzine
cp Webzine/.env.example .env   # edit variables if needed
docker compose build
docker compose up -d
```

Services available at:
- **Webzine**: http://localhost:7110
- **RAG API**: http://localhost:8000/docs
- **Database**: localhost:5432

### Check services

```bash
docker compose ps
docker compose logs -f
```

> For advanced commands (Makefile, validation scripts, production): see [README-DOCKER.md](./README-DOCKER.md)

## Team

P4 DIIAGE project — 4 contributors

| Role | Name |
|------|------|
| Developer | [TO COMPLETE] |
| Developer | [TO COMPLETE] |
| Developer | [TO COMPLETE] |
| Developer | [TO COMPLETE] |

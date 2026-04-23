# Configuration du RAG avec Deezer

## 📋 Instructions pour intégrer les données Deezer

### 1️⃣ Récupérer les données Deezer

Exécutez le script Python pour télécharger les données des artistes depuis l'API Deezer:

```bash
cd rag
python fetch_deezer_data.py
```

**Qu'est-ce que ce script fait:**
- ✅ Récupère les informations des 25 artistes populaires depuis Deezer
- ✅ Sauvegarde les données dans le dossier `data_deezer/`
- ✅ Crée un fichier texte par artiste avec:
  - Nombre de fans
  - Top 5 titres
  - Albums récents
  - Lien Deezer

### 2️⃣ Construire l'index vectoriel

Le RAG construit automatiquement l'index Deezer au démarrage s'il détecte le dossier `data_deezer/`.

**Vérification:** Vérifiez que le dossier `chroma_db_deezer/` est créé après le premier démarrage du service RAG.

### 3️⃣ Utiliser les données dans le chatbot

#### 🎯 Interface Web
1. Ouvrez le Webzine
2. Cliquez sur le bouton **💬 Assistant Webzine** en bas à droite
3. Sélectionnez **🎵 Deezer** dans le sélecteur "Source"
4. Posez vos questions!

#### 💻 API Directe

Changer la source du RAG:
```bash
curl -X POST http://localhost:8000/initialize \
  -H "Content-Type: application/json" \
  -d '{"source": "deezer"}'
```

Vérifier le statut:
```bash
curl http://localhost:8000/status
```

### 📁 Structure des fichiers

```
rag/
├── api.py                    # API FastAPI (modifiée)
├── pipeline.py              # Pipeline Wikipedia
├── pipeline_deezer.py       # Pipeline Deezer (nouveau)
├── fetch_deezer_data.py     # Script de récupération (nouveau)
├── data/                    # Données Wikipedia
├── data_deezer/             # Données Deezer (créé après fetch)
├── chroma_db/               # Index Wikipedia
└── chroma_db_deezer/        # Index Deezer
```

### 🔧 Personnalisation

Modifiez la liste des artistes dans `fetch_deezer_data.py`:

```python
ARTISTS_TO_FETCH = [
    "The Beatles",
    "Pink Floyd",
    # Ajoutez vos artistes ici...
]
```

Puis relancez:
```bash
python fetch_deezer_data.py
python pipeline_deezer.py
```

### ⚠️ Troubleshooting

**Q: Le service RAG démarre mais Deezer ne fonctionne pas**
- ✅ Vérifiez que `python fetch_deezer_data.py` a été exécuté
- ✅ Vérifiez que le dossier `data_deezer/` existe et n'est pas vide
- ✅ Vérifiez que `pipeline_deezer.py` a été testé

**Q: Les réponses ne sont pas pertinentes**
- ✅ Les données Deezer peuvent être limitées, augmentez le nombre d'artistes
- ✅ Utilisez des questions spécifiques sur les artistes connus

**Q: Je veux revenir à Wikipedia**
- ✅ Sélectionnez "📚 Wikipedia" dans le widget de chat
- ✅ Ou appelez l'API: `POST /initialize {"source": "wikipedia"}`

### 📊 API Endpoints

| Endpoint | Méthode | Description |
|----------|---------|-------------|
| `/query` | POST | Pose une question au RAG |
| `/initialize` | POST | Change la source (Deezer/Wikipedia) |
| `/status` | GET | Vérifie l'état du RAG |

**Body pour `/initialize`:**
```json
{
  "source": "deezer" // ou "wikipedia"
}
```

---

**Note:** Les données Deezer améliorent les réponses spécifiques sur les artistes stockés dans votre Webzine! 🎵

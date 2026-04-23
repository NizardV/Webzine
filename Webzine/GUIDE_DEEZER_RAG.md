# 📋 Guide complet: Intégration Deezer au RAG

## 🎯 Objectif
Remplacer les données Wikipedia locales par les données Deezer dans votre RAG pour fournir des réponses plus précises et pertinentes.

---

## 🚀 Étapes de mise en place

### Phase 1: Télécharger les données Deezer (Python)

#### 1.1 Exécuter le script de récupération
```bash
cd C:\DIIAGE1\Webzine\rag
python fetch_deezer_data.py
```

**Résultat attendu:**
```
🎵 Récupération des données Deezer pour le RAG

Répertoire de destination: ./data_deezer

Récupération de 25 artistes...

Récupération: The Beatles...
  ✅ Données sauvegardées pour 'The Beatles'
...
✨ Succès: 25/25 artistes traités
📁 Les fichiers sont disponibles dans: ./data_deezer/
```

#### 1.2 Vérifier les fichiers générés
```bash
ls -la data_deezer/
# Vous devriez voir 25 fichiers .txt avec les données des artistes
```

### Phase 2: Tester le pipeline Deezer (Python)

```bash
python pipeline_deezer.py
```

**Résultat attendu:**
```
🎵 Chargement des données Deezer...
  Chargement de The Beatles.txt...
  Chargement de Pink Floyd.txt...
  ...
✅ 25 documents chargés
📝 Création du splitter...
✅ 625 chunks créés
🧠 Initialisation des embeddings...
💾 Création de la DB Chroma...
✨ Index vectoriel Deezer créé!
```

### Phase 3: Redémarrer le service RAG

```bash
# Arrêter l'API RAG (Ctrl+C)
# Puis relancer:
uvicorn api:app --reload --port 8000
```

**Vérification:**
```bash
curl http://localhost:8000/status

# Réponse attendue:
{
  "status": "running",
  "current_source": "wikipedia",
  "wikipedia_available": true,
  "deezer_available": true
}
```

### Phase 4: Utiliser Deezer dans le Webzine

1. **Lancer le Webzine** (Visual Studio ou `dotnet run`)
2. **Ouvrir le navigateur**: http://localhost:7110
3. **Cliquer sur le bouton chat** en bas à droite 💬
4. **Sélectionner Deezer** dans le dropdown "Source"
5. **Poser une question:**
   - "Qui sont les artistes les plus populaires?"
   - "Quels sont les albums de The Beatles?"
   - "Quel est le lien Deezer pour Madonna?"

---

## 📊 Architecture

### Fichiers créés/modifiés:

**Python (RAG):**
- ✅ `fetch_deezer_data.py` - Script de téléchargement des données
- ✅ `pipeline_deezer.py` - Pipeline pour indexer les données Deezer  
- ✅ `api.py` - API modifiée pour supporter 2 sources

**C# (.NET):**
- ✅ `ChatController.cs` - Nouveau action `SetSource()` et `Status()`
- ✅ `RagService.cs` - Service RAG (Business)
- ✅ `IRagService.cs` - Interface du service RAG
- ✅ `RagResponse.cs` - Classe de réponse
- ✅ `_ChatWidget.cshtml` - Widget avec sélecteur de source

---

## 🎨 Interface utilisateur

### Sélecteur de source dans le widget:
```
┌─────────────────────────────┐
│ 💬 Assistant Webzine    [×]│
├─────────────────────────────┤
│ 🎵 Source: [Deezer    ▼]   │
│ 📚            Wikipedia    │
├─────────────────────────────┤
│ Messages...                 │
│                             │
├─────────────────────────────┤
│ [Posez votre question...] [📤]
└─────────────────────────────┘
```

---

## 🔄 Commutateur entre sources

### Via l'interface web:
1. Ouvrir le widget de chat
2. Cliquer sur le dropdown "Source"
3. Sélectionner Deezer ou Wikipedia
4. Un message de confirmation s'affiche

### Via l'API:
```bash
# Passer à Deezer
curl -X POST http://localhost:8000/initialize \
  -H "Content-Type: application/json" \
  -d '{"source": "deezer"}'

# Passer à Wikipedia
curl -X POST http://localhost:8000/initialize \
  -H "Content-Type: application/json" \
  -d '{"source": "wikipedia"}'
```

---

## 🆘 Dépannage

### ❌ "Erreur: Les données Deezer ne sont pas disponibles"
**Solution:**
1. Vérifier que `fetch_deezer_data.py` a été exécuté
2. Vérifier que le dossier `data_deezer/` existe
3. Vérifier que des fichiers .txt sont présents

### ❌ "Les réponses ne sont pas pertinentes"
**Solutions:**
1. Les données Deezer sont basées sur les artistes populaires
2. Augmentez la liste `ARTISTS_TO_FETCH` dans `fetch_deezer_data.py`
3. Relancez `fetch_deezer_data.py` et `pipeline_deezer.py`

### ❌ "Connexion refusée au service RAG"
**Solutions:**
1. Vérifier que le service RAG s'exécute: `http://localhost:8000/status`
2. Vérifier le port configuré dans `appsettings.json` (`RagApi`)
3. Vérifier que le pare-feu n'a pas bloqué le port

### ❌ "IndexError: list index out of range" (Python)
**Solution:**
La requête Deezer a échoué, généralement due à un problème de réseau:
1. Vérifier la connexion Internet
2. Vérifier que l'API Deezer répond: `curl https://api.deezer.com/search/artist?q=Madonna`
3. Réessayer `fetch_deezer_data.py`

---

## 📚 Résumé des endpoints

| Endpoint | Méthode | Description | Body |
|----------|---------|-------------|------|
| `/query` | POST | Interroger le RAG | `{"question": "..."}` |
| `/initialize` | POST | Changer la source | `{"source": "deezer"┆"wikipedia"}` |
| `/status` | GET | État du service | - |

---

## 💡 Personnalisation

### Ajouter plus d'artistes:

1. Modifiez `fetch_deezer_data.py`:
```python
ARTISTS_TO_FETCH = [
    "The Beatles",
    "Pink Floyd",
    # ... vos artistes ...
    "Votre Artiste Préféré"
]
```

2. Relancez:
```bash
python fetch_deezer_data.py
python pipeline_deezer.py
```

3. Redémarrez le service RAG et le Webzine

---

## ✨ Avantages de Deezer

✅ **Données actualisées** - Utilise les données actuelles de Deezer  
✅ **Statistiques pertinentes** - Nombre de fans, albums récents, top titres  
✅ **Liens directs** - Liens vers les profils Deezer  
✅ **Meilleure pertinence** - Spécifique aux artistes du Webzine  

---

## 📞 Support

En cas de problème, consultez:
- `README_DEEZER.md` - Documentation RAG
- Les logs du service RAG (terminal Python)
- Les logs du Webzine (Visual Studio Debug)


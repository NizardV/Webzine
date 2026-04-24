import os

from langchain_community.document_loaders import DirectoryLoader, TextLoader
from langchain_text_splitters import RecursiveCharacterTextSplitter
from langchain_community.embeddings import HuggingFaceEmbeddings
from langchain_community.vectorstores import Chroma
from langchain_community.llms import Ollama

DATA_DIR = "./data_deezer"
CHROMA_DIR = "./chroma_db_deezer"
CACHE_DIR = "./cache"
OLLAMA_HOST = os.getenv("OLLAMA_HOST", "http://localhost:11434")
MODEL_NAME = os.getenv("OLLAMA_MODEL", "llama3.2")

# Modèle multilingue : gère le français et l'anglais
EMBEDDING_MODEL = "paraphrase-multilingual-MiniLM-L12-v2"

# Seuil de pertinence cosinus — ne pas baisser sans changer le modèle d'embedding
RELEVANCE_THRESHOLD = 0.5

# Message renvoyé quand aucun chunk pertinent n'est trouvé — le LLM n'est PAS appelé
NO_INFO_MESSAGE = "Je n'ai pas cette information dans ma base de données."

_db = None
_embeddings = None


def _get_embeddings():
    global _embeddings
    if _embeddings is None:
        _embeddings = HuggingFaceEmbeddings(
            model_name=EMBEDDING_MODEL,
            cache_folder=CACHE_DIR,
        )
    return _embeddings


def build_deezer_index():
    global _db

    embeddings = _get_embeddings()

    # Charger l'index existant si disponible et non vide
    if os.path.exists(CHROMA_DIR) and os.listdir(CHROMA_DIR):
        print("📦 Chargement de l'index Deezer existant...")
        candidate = Chroma(persist_directory=CHROMA_DIR, embedding_function=embeddings)
        if candidate._collection.count() > 0:
            _db = candidate
            print(f"✅ Index Deezer chargé ({_db._collection.count()} chunks)")
            return _db
        print("⚠️  Index existant vide — reconstruction depuis les fichiers source")

    if not os.path.exists(DATA_DIR) or not os.listdir(DATA_DIR):
        print(f"⚠️  Répertoire {DATA_DIR} vide ou introuvable — exécutez fetch_deezer_data.py")
        return None

    print("📂 Chargement des fichiers Deezer...")
    loader = DirectoryLoader(
        DATA_DIR,
        glob="*.txt",
        loader_cls=TextLoader,
        loader_kwargs={"encoding": "utf-8"},
    )
    docs = loader.load()
    print(f"✅ {len(docs)} fichiers chargés")

    splitter = RecursiveCharacterTextSplitter(chunk_size=500, chunk_overlap=50)
    chunks = splitter.split_documents(docs)
    print(f"✂️  {len(chunks)} chunks créés")

    print(f"🔢 Calcul des embeddings avec {EMBEDDING_MODEL}...")
    _db = Chroma.from_documents(
        chunks,
        embeddings,
        persist_directory=CHROMA_DIR,
        collection_metadata={"hnsw:space": "cosine"},
    )
    print("✅ Index Deezer construit et sauvegardé")
    return _db


def _format_history(history: list) -> str:
    if not history:
        return ""
    lines = []
    for msg in history[-6:]:
        role = "Utilisateur" if msg.get("role") == "user" else "Assistant"
        lines.append(f"{role}: {msg.get('content', '')}")
    return "Historique de la conversation:\n" + "\n".join(lines) + "\n\n"


def query_deezer(question: str, history: list = None):
    global _db

    if _db is None:
        build_deezer_index()

    if _db is None:
        return "Données Deezer non disponibles. Exécutez fetch_deezer_data.py puis relancez le service.", []

    # Pour les questions de suivi très courtes, ajouter les mots-clés de l'historique
    retrieval_query = question
    if history and len(question.split()) < 6:
        last_user = next((m["content"] for m in reversed(history) if m.get("role") == "user"), "")
        if last_user:
            stop_words = {
                "combien", "de", "d", "a", "sur", "le", "la", "les", "un", "une",
                "est", "et", "ou", "que", "qui", "quoi", "du", "des", "il", "elle",
                "par", "pour", "avec", "dans", "au", "aux", "en", "ce", "cette",
            }
            keywords = [
                w.lower().strip("?!.,")
                for w in last_user.split()
                if w.lower().strip("?!.,") not in stop_words and len(w.strip("?!.,")) > 2
            ]
            if keywords:
                retrieval_query = f"{question} {' '.join(keywords)}"
                print(f"   (Requête enrichie: {retrieval_query})")

    # Log diagnostic : question + scores
    results_with_scores = _db.similarity_search_with_relevance_scores(retrieval_query, k=5)
    print(f"🔍 Question: {question}")
    for doc, score in results_with_scores:
        src = doc.metadata.get("source", "?").split("/")[-1].split("\\")[-1]
        print(f"   Score: {score:.4f} | {src} | {doc.page_content[:60]}")

    relevant_docs = [doc for doc, score in results_with_scores if score >= RELEVANCE_THRESHOLD]

    # Aucun chunk pertinent → réponse directe SANS appeler le LLM
    if not relevant_docs:
        print("   → Aucun chunk au-dessus du seuil, réponse canned sans LLM")
        return NO_INFO_MESSAGE, []

    context = "\n\n".join(doc.page_content for doc in relevant_docs)
    history_text = _format_history(history or [])

    prompt = f"""{history_text}Tu es un assistant musical de la plateforme Webzine.
RÈGLE ABSOLUE : tu réponds UNIQUEMENT à partir des extraits fournis ci-dessous.
Si l'information n'est pas présente dans les extraits, réponds EXACTEMENT : "{NO_INFO_MESSAGE}"
N'utilise JAMAIS tes connaissances générales. Ne complète JAMAIS avec des informations issues de ta mémoire.

Extraits Deezer:
{context}

Question: {question}

Réponse en français (basée exclusivement sur les extraits) :"""

    try:
        llm = Ollama(base_url=OLLAMA_HOST, model=MODEL_NAME)
        return llm.invoke(prompt), relevant_docs
    except Exception as e:
        print(f"⚠️  Ollama indisponible ({e}) — réponse basée sur le contexte brut")
        return f"Voici les informations trouvées :\n\n{context}", relevant_docs

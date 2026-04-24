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

# Seuil de similarité cosinus : en dessous, les chunks sont considérés hors-sujet
RELEVANCE_THRESHOLD = 0.5

_db = None
_embeddings = None


def _get_embeddings():
    global _embeddings
    if _embeddings is None:
        _embeddings = HuggingFaceEmbeddings(
            model_name="all-MiniLM-L6-v2",
            cache_folder=CACHE_DIR,
        )
    return _embeddings


def build_deezer_index():
    global _db

    embeddings = _get_embeddings()

    # Charger l'index existant si disponible et non vide
    if os.path.exists(CHROMA_DIR) and os.listdir(CHROMA_DIR):
        print("Chargement de l'index Deezer existant...")
        candidate = Chroma(persist_directory=CHROMA_DIR, embedding_function=embeddings)
        if candidate._collection.count() > 0:
            _db = candidate
            print(f"Index Deezer chargé ({_db._collection.count()} chunks)")
            return _db
        print("Index existant vide — reconstruction depuis les fichiers source")

    if not os.path.exists(DATA_DIR) or not os.listdir(DATA_DIR):
        print(f"Répertoire {DATA_DIR} vide ou introuvable — exécutez fetch_deezer_data.py")
        return None

    print("Chargement des fichiers Deezer...")
    loader = DirectoryLoader(
        DATA_DIR,
        glob="*.txt",
        loader_cls=TextLoader,
        loader_kwargs={"encoding": "utf-8"},
    )
    docs = loader.load()
    print(f"{len(docs)} fichiers chargés")

    splitter = RecursiveCharacterTextSplitter(chunk_size=500, chunk_overlap=50)
    chunks = splitter.split_documents(docs)
    print(f"{len(chunks)} chunks créés")

    print("Calcul des embeddings (peut prendre quelques minutes au premier démarrage)...")
    _db = Chroma.from_documents(chunks, embeddings, persist_directory=CHROMA_DIR)
    print("Index Deezer construit et sauvegardé")
    return _db


def _format_history(history: list) -> str:
    if not history:
        return ""
    lines = []
    for msg in history[-6:]:  # garder les 3 derniers échanges max
        role = "Utilisateur" if msg.get("role") == "user" else "Assistant"
        lines.append(f"{role}: {msg.get('content', '')}")
    return "Historique de la conversation:\n" + "\n".join(lines) + "\n\n"


def query_deezer(question: str, history: list = None):
    global _db

    if _db is None:
        build_deezer_index()

    if _db is None:
        return "Données Deezer non disponibles. Exécutez fetch_deezer_data.py puis relancez le service.", []

    # Récupérer les documents avec leur score de similarité
    results_with_scores = _db.similarity_search_with_relevance_scores(question, k=3)

    # Filtrer les chunks dont le score est trop bas (hors-sujet)
    relevant_docs = [doc for doc, score in results_with_scores if score >= RELEVANCE_THRESHOLD]

    history_text = _format_history(history or [])

    if not relevant_docs:
        # Question hors-sujet ou artiste inconnu : répondre sans forcer le contexte
        no_context_prompt = f"""{history_text}Tu es un assistant musical de la plateforme Webzine.
La question de l'utilisateur ne correspond à aucune information disponible dans notre base de données musicales.
Réponds honnêtement que tu ne trouves pas d'informations sur ce sujet dans ta base.
Si c'est une salutation ou une question générale, réponds naturellement.

Question: {question}

Réponse en français:"""
        try:
            llm = Ollama(base_url=OLLAMA_HOST, model=MODEL_NAME)
            return llm.invoke(no_context_prompt), []
        except Exception as e:
            print(f"Ollama indisponible ({e})")
            return "Je n'ai pas trouvé d'informations sur ce sujet dans notre base Deezer.", []

    context = "\n\n".join(doc.page_content for doc in relevant_docs)

    prompt = f"""{history_text}Tu es un assistant musical spécialisé dans les artistes présents sur Deezer.
Utilise uniquement le contexte ci-dessous pour répondre à la question.
Si le contexte ne contient pas l'information demandée, dis-le honnêtement.

Contexte Deezer:
{context}

Question: {question}

Réponse en français (concise et informative):"""

    try:
        llm = Ollama(base_url=OLLAMA_HOST, model=MODEL_NAME)
        return llm.invoke(prompt), relevant_docs
    except Exception as e:
        print(f"Ollama indisponible ({e}) — réponse basée sur le contexte brut")
        return f"Voici les informations trouvées :\n\n{context}", relevant_docs


if __name__ == "__main__":
    db = build_deezer_index()
    if db:
        question = "Qui sont les artistes les plus populaires?"
        print(f"\n Question: {question}")
        answer, sources = query_deezer(question)
        print(f"Réponse: {answer}")
        print(f"Sources: {[doc.metadata.get('source', 'N/A') for doc in sources]}")

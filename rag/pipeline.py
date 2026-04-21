import os
import ollama
from langchain_community.vectorstores import Chroma
from langchain_community.embeddings import HuggingFaceEmbeddings
from langchain_text_splitters import RecursiveCharacterTextSplitter
from langchain_community.document_loaders import TextLoader
from langchain_core.documents import Document
from pathlib import Path
DATA_DIR = "./data"
CHROMA_DIR = "./chroma_db"

def build_index():
    print("Chargement des biographies...")
    docs = []

    # Charger manuellement les fichiers texte
    for file_path in Path(DATA_DIR).glob("*.txt"):
        print(f"  Chargement de {file_path.name}...")
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
                doc = Document(
                    page_content=content,
                    metadata={"source": file_path.name}
                )
                docs.append(doc)
        except Exception as e:
            print(f"    Erreur pour {file_path.name}: {e}")

    print(f"{len(docs)} documents chargés")

    print("Création du splitter...")
    splitter = RecursiveCharacterTextSplitter(chunk_size=500, chunk_overlap=50)
    chunks = splitter.split_documents(docs)
    print(f"{len(chunks)} chunks créés")

    print("Initialisation des embeddings...")
    embeddings = HuggingFaceEmbeddings(model_name="all-MiniLM-L6-v2")
    print("Création de la DB Chroma...")
    db = Chroma.from_documents(chunks, embeddings, persist_directory=CHROMA_DIR)
    print("Index vectoriel créé !")
    return db

def query(question):
    embeddings = HuggingFaceEmbeddings(model_name="all-MiniLM-L6-v2")
    db = Chroma(persist_directory=CHROMA_DIR, embedding_function=embeddings)
    docs = db.similarity_search(question, k=6)
    context = "\n\n---\n\n".join([d.page_content for d in docs])

    prompt = f"""Tu es un assistant musical expert. Réponds à la question en te basant UNIQUEMENT sur le contexte fourni. Sois précis et synthétique.

Contexte :
{context}

Question : {question}

Réponse :"""

    response = ollama.chat(
        model="llama3.2",
        messages=[{"role": "user", "content": prompt}]
    )
    return response["message"]["content"], docs

if __name__ == "__main__":
    db = build_index()

    # Test avec une question
    question = "Compare Eminiem et 50 Cent"
    print(f"\nQuestion: {question}")
    answer, sources = query(question)
    print(f"Réponse: {answer}")
    print(f"Sources: {[doc.metadata['source'] for doc in sources]}")
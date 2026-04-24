from contextlib import asynccontextmanager
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from pipeline import query, build_index
from pipeline_deezer import query_deezer, build_deezer_index
import os

# Variable globale pour tracker la source actuellement active
current_source = "deezer"  # "wikipedia" ou "deezer"


@asynccontextmanager
async def lifespan(_app: FastAPI):
    print("🚀 Démarrage du service RAG")
    print(f"📍 Source initiale: {current_source}")

    build_index()

    if os.path.exists("./data_deezer"):
        print("🎵 Chargement des données Deezer disponibles...")
        try:
            build_deezer_index()
            print("✅ Index Deezer chargé avec succès")
        except Exception as e:
            print(f"⚠️  Impossible de charger les données Deezer: {e}")

    yield


app = FastAPI(lifespan=lifespan)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)


class HistoryMessage(BaseModel):
    role: str  # "user" ou "assistant"
    content: str

class Question(BaseModel):
    question: str
    history: list[HistoryMessage] = []

class InitRequest(BaseModel):
    source: str = "deezer"  # "wikipedia" ou "deezer"

@app.post("/query")
async def ask(q: Question):
    global current_source

    try:
        history = [{"role": m.role, "content": m.content} for m in q.history]
        if current_source == "deezer":
            answer, sources = query_deezer(q.question, history)
        else:
            answer, sources = query(q.question, history)

        return {
            "answer": answer,
            "sources": [doc.metadata.get("source", "unknown") for doc in sources],
            "source": current_source
        }
    except Exception as e:
        return {
            "answer": f"Erreur lors du traitement: {str(e)}",
            "sources": [],
            "source": current_source
        }

@app.post("/initialize")
async def initialize(req: InitRequest):
    global current_source

    try:
        if req.source == "deezer":
            if not os.path.exists("./data_deezer"):
                return {
                    "success": False,
                    "message": "Les données Deezer ne sont pas disponibles. Exécutez d'abord fetch_deezer_data.py"
                }
            build_deezer_index()
            current_source = "deezer"
            return {
                "success": True,
                "message": "Index Deezer initialisé avec succès",
                "source": "deezer"
            }
        elif req.source == "wikipedia":
            build_index()
            current_source = "wikipedia"
            return {
                "success": True,
                "message": "Index Wikipedia initialisé avec succès",
                "source": "wikipedia"
            }
        else:
            return {
                "success": False,
                "message": f"Source inconnue: {req.source}"
            }
    except Exception as e:
        return {
            "success": False,
            "message": f"Erreur: {str(e)}"
        }

@app.get("/status")
async def status():
    return {
        "status": "running",
        "current_source": current_source,
        "wikipedia_available": os.path.exists("./data"),
        "deezer_available": os.path.exists("./data_deezer")
    }


@app.get("/health")
async def health():
    """Health check endpoint for Docker/Kubernetes"""
    return {
        "status": "healthy",
        "service": "RAG API",
        "current_source": current_source
    }


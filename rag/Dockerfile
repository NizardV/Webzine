# Dockerfile pour API RAG Python FastAPI - Minimalist
FROM python:3.11-slim

WORKDIR /app

# Installer dépendances système minimales
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copier requirements.txt et installer les dépendances
COPY rag/requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

# Copier code source RAG
COPY rag/api.py ./api.py
COPY rag/pipeline.py ./pipeline.py
COPY rag/pipeline_deezer.py ./pipeline_deezer.py

# Créer dossiers pour les données
RUN mkdir -p /app/data_deezer /app/chroma_db

# Variables d'environnement
ENV PYTHONUNBUFFERED=1
ENV HOST=0.0.0.0
ENV PORT=8000

EXPOSE 8000

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8000/health || exit 1

CMD ["uvicorn", "api:app", "--host", "0.0.0.0", "--port", "8000"]

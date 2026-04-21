from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from pipeline import query, build_index

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)

class Question(BaseModel):
    question: str

@app.on_event("startup")
async def startup():
    build_index()

@app.post("/query")
async def ask(q: Question):
    answer, sources = query(q.question)
    return {
        "answer": answer,
        "sources": [doc.metadata["source"] for doc in sources]
    }
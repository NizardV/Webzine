"""
Minimal pipeline stub for Deezer - full implementation requires heavy dependencies
"""
def build_deezer_index():
    """Build Deezer index"""
    print("🎵 Deezer index build stub (requires LangChain, Chroma, etc.)")
    pass

def query_deezer(question: str):
    """Query Deezer data"""
    return "Deezer query stub", []

if __name__ == "__main__":
    # Test
    db = build_deezer_index()

    if db:
        question = "Qui sont les artistes les plus populaires?"
        print(f"\n❓ Question: {question}")
        answer, sources = query_deezer(question)
        print(f"💬 Réponse: {answer}")
        print(f"📚 Sources: {[doc.metadata['source'] for doc in sources]}")

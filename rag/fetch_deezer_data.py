"""
Script pour récupérer les données des artistes depuis Deezer et les sauvegarder
"""
import os
import json
import requests
from pathlib import Path

DEEZER_API_BASE = "https://api.deezer.com"
DATA_DIR_DEEZER = "./data_deezer"
CACHE_FILE = "./deezer_cache.json"

# Liste des artistes à récupérer (à adapter selon vos besoins)
ARTISTS_TO_FETCH = [
    "The Beatles",
    "Pink Floyd",
    "Radiohead",
    "Nirvana",
    "Queen",
    "Bob Marley",
    "David Bowie",
    "Michael Jackson",
    "Prince",
    "Madonna",
    "The Weeknd",
    "Ed Sheeran",
    "Drake",
    "Kanye West",
    "Eminem",
    "Jay-Z",
    "Kendrick Lamar",
    "Coldplay",
    "Daft Punk",
    "Bruno Mars",
    "Beyoncé",
    "Adele",
    "Amy Winehouse",
    "Rihanna",
    "50 Cent"
]

def create_deezer_data_directory():
    """Crée le répertoire pour les données Deezer"""
    Path(DATA_DIR_DEEZER).mkdir(exist_ok=True)

def fetch_artist_info(artist_name):
    """Récupère les informations d'un artiste depuis Deezer"""
    try:
        # Chercher l'artiste
        search_response = requests.get(
            f"{DEEZER_API_BASE}/search/artist",
            params={"q": artist_name},
            timeout=5
        )
        search_response.raise_for_status()

        data = search_response.json()
        if not data.get("data"):
            print(f"Aucun artiste trouvé pour '{artist_name}'")
            return None

        artist = data["data"][0]
        artist_id = artist["id"]

        # Récupérer les détails de l'artiste
        artist_response = requests.get(
            f"{DEEZER_API_BASE}/artist/{artist_id}",
            timeout=5
        )
        artist_response.raise_for_status()
        artist_details = artist_response.json()

        # Récupérer l'album top
        top_response = requests.get(
            f"{DEEZER_API_BASE}/artist/{artist_id}/top",
            params={"limit": 10},
            timeout=5
        )
        top_response.raise_for_status()
        top_tracks = top_response.json()

        # Récupérer les albums
        albums_response = requests.get(
            f"{DEEZER_API_BASE}/artist/{artist_id}/albums",
            params={"limit": 10},
            timeout=5
        )
        albums_response.raise_for_status()
        albums = albums_response.json()

        return {
            "artist": artist_details,
            "top_tracks": top_tracks.get("data", []),
            "albums": albums.get("data", [])
        }
    except Exception as e:
        print(f"Erreur pour '{artist_name}': {str(e)}")
        return None

def save_artist_data(artist_name, data):
    """Sauvegarde les données d'un artiste dans un fichier texte"""
    if not data:
        return

    filename = os.path.join(DATA_DIR_DEEZER, f"{artist_name}.txt")

    try:
        with open(filename, "w", encoding="utf-8") as f:
            f.write(f"# {artist_name}\n\n")

            # Infos artiste
            artist = data["artist"]
            f.write(f"## Artiste\n")
            f.write(f"Nom: {artist.get('name', 'N/A')}\n")
            f.write(f"Fans: {artist.get('fans', 0):,}\n")
            if artist.get('picture_medium'):
                f.write(f"Image: {artist.get('picture_medium')}\n")
            if artist.get('link'):
                f.write(f"Lien Deezer: {artist.get('link')}\n")
            f.write("\n")

            # Top tracks
            if data["top_tracks"]:
                f.write(f"## Top Titres\n")
                for i, track in enumerate(data["top_tracks"][:5], 1):
                    f.write(f"{i}. {track.get('title', 'N/A')} ")
                    f.write(f"(Album: {track.get('album', {}).get('title', 'N/A')})\n")
                f.write("\n")

            # Albums
            if data["albums"]:
                f.write(f"## Albums Récents\n")
                for album in data["albums"][:5]:
                    f.write(f"- {album.get('title', 'N/A')} ")
                    f.write(f"({album.get('release_date', 'N/A')[:4]})\n")
                f.write("\n")

        print(f"Données sauvegardées pour '{artist_name}'")
    except Exception as e:
        print(f"Erreur lors de la sauvegarde pour '{artist_name}': {str(e)}")

def main():
    print("Récupération des données Deezer pour le RAG\n")
    print(f"Répertoire de destination: {DATA_DIR_DEEZER}\n")

    create_deezer_data_directory()

    print(f"Récupération de {len(ARTISTS_TO_FETCH)} artistes...\n")

    success_count = 0
    for artist_name in ARTISTS_TO_FETCH:
        print(f"Récupération: {artist_name}...")
        data = fetch_artist_info(artist_name)
        if data:
            save_artist_data(artist_name, data)
            success_count += 1

    print(f"\nSuccès: {success_count}/{len(ARTISTS_TO_FETCH)} artistes traités")
    print(f"Les fichiers sont disponibles dans: {DATA_DIR_DEEZER}/")

if __name__ == "__main__":
    main()

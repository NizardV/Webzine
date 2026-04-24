import requests
import os

os.makedirs("data", exist_ok=True)

artists = [
    "Eminem", "Daft Punk", "Queen (band)", "The Beatles", "Drake (musician)",
    "Pink Floyd", "The Weeknd", "Beyoncé", "Coldplay", "Adele",
    "Michael Jackson", "Bob Marley", "David Bowie", "Prince (musician)", "Madonna (singer)",
    "Kendrick Lamar", "Amy Winehouse", "Radiohead", "Nirvana (band)", "Jay-Z",
    "50 Cent", "Kanye West", "Rihanna", "Ed Sheeran", "Bruno Mars"
]

for artist in artists:
    r = requests.get(
        "https://en.wikipedia.org/api/rest_v1/page/summary/" + artist.replace(" ", "_"),
        headers={"User-Agent": "WebzineRAG/1.0"}
    )
    if r.status_code == 200:
        data = r.json()
        bio = data.get("extract", "")
        name = data.get("title", artist)
        if len(bio) > 100:
            with open(f"data/{name.replace('/', '-')}.txt", "w", encoding="utf-8") as f:
                f.write(f"Artiste : {name}\n\n{bio}")
            print(f"{name} ({len(bio)} caractères)")
        else:
            print(f"{name} — bio trop courte")
    else:
        print(f"{artist} — erreur {r.status_code}")

print("Corpus prêt !")
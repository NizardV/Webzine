-- Script d'initialisation PostgreSQL pour Webzine
-- Tables pour mémorisation, historique conversationnel, et contenu musical

-- ============================================================
-- SUPPRESSION DES TABLES EXISTANTES (si elles existent)
-- ============================================================

DROP TABLE IF EXISTS conversation_feedback CASCADE;
DROP TABLE IF EXISTS user_preferences CASCADE;
DROP TABLE IF EXISTS user_sessions CASCADE;
DROP TABLE IF EXISTS conversation_history CASCADE;
DROP TABLE IF EXISTS "Commentaire" CASCADE;
DROP TABLE IF EXISTS "TitreStyle" CASCADE;
DROP TABLE IF EXISTS "Titre" CASCADE;
DROP TABLE IF EXISTS "Style" CASCADE;
DROP TABLE IF EXISTS "Artiste" CASCADE;

-- ============================================================
-- TABLES DE CONTENU MUSICAL
-- ============================================================

-- Table pour les artistes
CREATE TABLE "Artiste" (
    "IdArtiste" SERIAL PRIMARY KEY,
    "Nom" VARCHAR(255) NOT NULL,
    "Biographie" TEXT
);

-- Table pour les titres musicaux
CREATE TABLE "Titre" (
    "IdTitre" SERIAL PRIMARY KEY,
    "Libelle" VARCHAR(200) NOT NULL,
    "Chronique" VARCHAR(4000) NOT NULL,
    "UrlJaquette" VARCHAR(250) NOT NULL,
    "UrlEcoute" VARCHAR(250) NOT NULL,
    "DateCreation" TIMESTAMP NOT NULL,
    "DateSortie" TIMESTAMP NOT NULL,
    "Duree" TIME NOT NULL,
    "NbLectures" INTEGER NOT NULL DEFAULT 0,
    "NbLikes" INTEGER NOT NULL DEFAULT 0,
    "IdArtiste" INTEGER NOT NULL REFERENCES "Artiste"("IdArtiste"),
    "Album" VARCHAR(255) NOT NULL
);

-- Table pour les styles musicaux
CREATE TABLE "Style" (
    "IdStyle" SERIAL PRIMARY KEY,
    "Libelle" VARCHAR(255) NOT NULL
);

-- Table de liaison Titre-Style (many-to-many)
CREATE TABLE "TitreStyle" (
    "IdTitre" INTEGER NOT NULL REFERENCES "Titre"("IdTitre") ON DELETE CASCADE,
    "IdStyle" INTEGER NOT NULL REFERENCES "Style"("IdStyle") ON DELETE CASCADE,
    PRIMARY KEY ("IdTitre", "IdStyle")
);

-- Table pour les commentaires
CREATE TABLE "Commentaire" (
    "IdCommentaire" SERIAL PRIMARY KEY,
    "Auteur" VARCHAR(30) NOT NULL,
    "DateCreation" TIMESTAMP NOT NULL,
    "Contenu" VARCHAR(1000) NOT NULL,
    "IdTitre" INTEGER NOT NULL REFERENCES "Titre"("IdTitre") ON DELETE CASCADE
);

-- ============================================================
-- TABLES CONVERSATIONNELLES
-- ============================================================

-- Table pour l'historique des conversations
CREATE TABLE conversation_history (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(255) NOT NULL,
    session_id UUID NOT NULL,
    user_message TEXT NOT NULL,
    assistant_response TEXT NOT NULL,
    source VARCHAR(50) DEFAULT 'wikipedia' CHECK (source IN ('wikipedia', 'deezer')),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table pour les sessions utilisateur
CREATE TABLE user_sessions (
    id UUID PRIMARY KEY,
    user_id VARCHAR(255) NOT NULL,
    source VARCHAR(50) DEFAULT 'wikipedia' CHECK (source IN ('wikipedia', 'deezer')),
    context TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_activity TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table pour les préférences utilisateur
CREATE TABLE user_preferences (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(255) NOT NULL UNIQUE,
    preferred_source VARCHAR(50) DEFAULT 'wikipedia',
    enable_history BOOLEAN DEFAULT TRUE,
    enable_context BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table pour les feedback utilisateurs
CREATE TABLE conversation_feedback (
    id SERIAL PRIMARY KEY,
    conversation_id INTEGER NOT NULL REFERENCES conversation_history(id) ON DELETE CASCADE,
    user_id VARCHAR(255) NOT NULL,
    rating INTEGER CHECK (rating BETWEEN 1 AND 5),
    feedback_text TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================================
-- INDEXES
-- ============================================================

-- Indexes pour les tables de contenu
CREATE INDEX IF NOT EXISTS idx_titre_artiste ON "Titre"("IdArtiste");
CREATE INDEX IF NOT EXISTS idx_titre_style ON "TitreStyle"("IdStyle");
CREATE INDEX IF NOT EXISTS idx_commentaire_titre ON "Commentaire"("IdTitre");

-- Indexes pour les tables conversationnelles
CREATE INDEX IF NOT EXISTS idx_conversation_history_user_id ON conversation_history(user_id);
CREATE INDEX IF NOT EXISTS idx_conversation_history_session_id ON conversation_history(session_id);
CREATE INDEX IF NOT EXISTS idx_conversation_history_created_at ON conversation_history(created_at);
CREATE INDEX IF NOT EXISTS idx_user_sessions_user_id ON user_sessions(user_id);
CREATE INDEX IF NOT EXISTS idx_user_sessions_last_activity ON user_sessions(last_activity);
CREATE INDEX IF NOT EXISTS idx_user_preferences_user_id ON user_preferences(user_id);
CREATE INDEX IF NOT EXISTS idx_conversation_feedback_conversation_id ON conversation_feedback(conversation_id);

-- ============================================================
-- VIEWS
-- ============================================================

-- View pour les statistiques de conversation
CREATE OR REPLACE VIEW conversation_stats AS
SELECT
    COUNT(DISTINCT ch.user_id) as total_users,
    COUNT(*) as total_messages,
    COUNT(DISTINCT ch.session_id) as total_sessions,
    ch.source,
    DATE(ch.created_at) as date
FROM conversation_history ch
GROUP BY ch.source, DATE(ch.created_at)
ORDER BY DATE(ch.created_at) DESC;

-- ============================================================
-- COMMENTS
-- ============================================================

COMMENT ON TABLE "Artiste" IS 'Artistes musicaux';
COMMENT ON TABLE "Titre" IS 'Titres musicaux avec références aux artistes';
COMMENT ON TABLE "Style" IS 'Styles musicaux';
COMMENT ON TABLE "TitreStyle" IS 'Relation many-to-many entre titres et styles';
COMMENT ON TABLE "Commentaire" IS 'Commentaires publiés sur les titres';
COMMENT ON TABLE conversation_history IS 'Historique complet de toutes les conversations utilisateur';
COMMENT ON TABLE user_sessions IS 'Sessions utilisateur pour maintenir le contexte conversationnel';
COMMENT ON TABLE user_preferences IS 'Préférences et paramètres utilisateur';
COMMENT ON TABLE conversation_feedback IS 'Feedback utilisateur sur la qualité des réponses';
COMMENT ON VIEW conversation_stats IS 'Statistiques agrégées des conversations';

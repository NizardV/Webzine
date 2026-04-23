# PostgreSQL Integration Guide

## Overview

La base de données PostgreSQL a été intégrée pour **mémoriser les conversations** et les **préférences utilisateurs**. Le schéma est automatiquement créé lors du démarrage du conteneur via `init-db.sql`.

---

## Schema Tables

### 1. `conversation_history`
Historique complet de toutes les conversations utilisateur.

```sql
CREATE TABLE conversation_history (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(255) NOT NULL,
    session_id UUID NOT NULL,
    user_message TEXT NOT NULL,
    assistant_response TEXT NOT NULL,
    source VARCHAR(50) DEFAULT 'wikipedia',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Colonnes:**
- `id` - Identifiant unique du message
- `user_id` - ID utilisateur (ex: IP ou session cookie)
- `session_id` - Identifiant unique de session
- `user_message` - Message envoyé par l'utilisateur
- `assistant_response` - Réponse du RAG
- `source` - Source RAG utilisée (wikipedia/deezer)
- `created_at` - Timestamp de création
- `updated_at` - Timestamp de dernière modification

---

### 2. `user_sessions`
Sessions utilisateur pour maintenir le contexte conversationnel.

```sql
CREATE TABLE user_sessions (
    id UUID PRIMARY KEY,
    user_id VARCHAR(255) NOT NULL,
    source VARCHAR(50) DEFAULT 'wikipedia',
    context TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_activity TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Colonnes:**
- `id` - Identifiant unique de session (UUID v4)
- `user_id` - ID utilisateur
- `source` - Source RAG actuellement sélectionnée
- `context` - Contexte JSON stocké pour la session (optionnel)
- `created_at` - Date de création
- `last_activity` - Dernière activité utilisateur

---

### 3. `user_preferences`
Préférences et paramètres utilisateur.

```sql
CREATE TABLE user_preferences (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(255) NOT NULL UNIQUE,
    preferred_source VARCHAR(50) DEFAULT 'wikipedia',
    enable_history BOOLEAN DEFAULT TRUE,
    enable_context BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Colonnes:**
- `id` - Identifiant unique
- `user_id` - ID utilisateur (clé unique)
- `preferred_source` - Source RAG préférée
- `enable_history` - Activer l'historique ?
- `enable_context` - Activer le contexte conversationnel ?
- `created_at` - Date de création
- `updated_at` - Date de modification

---

### 4. `conversation_feedback`
Feedback utilisateur sur la qualité des réponses.

```sql
CREATE TABLE conversation_feedback (
    id SERIAL PRIMARY KEY,
    conversation_id INTEGER NOT NULL,
    user_id VARCHAR(255) NOT NULL,
    rating INTEGER CHECK (rating BETWEEN 1 AND 5),
    feedback_text TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

**Colonnes:**
- `id` - Identifiant unique
- `conversation_id` - Référence à conversation_history
- `user_id` - ID utilisateur qui a fourni le feedback
- `rating` - Note 1-5 étoiles
- `feedback_text` - Commentaire utilisateur
- `created_at` - Date du feedback

---

## Index

Les index suivants sont créés pour optimiser les performances:

```sql
CREATE INDEX idx_conversation_history_user_id ON conversation_history(user_id);
CREATE INDEX idx_conversation_history_session_id ON conversation_history(session_id);
CREATE INDEX idx_conversation_history_created_at ON conversation_history(created_at);
CREATE INDEX idx_user_sessions_user_id ON user_sessions(user_id);
CREATE INDEX idx_user_sessions_last_activity ON user_sessions(last_activity);
CREATE INDEX idx_user_preferences_user_id ON user_preferences(user_id);
CREATE INDEX idx_conversation_feedback_conversation_id ON conversation_feedback(conversation_id);
```

---

## Views

### `conversation_stats`
Vue d'agrégation pour les statistiques de conversations.

```sql
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
```

**Utilisation:**
```sql
SELECT * FROM conversation_stats WHERE date = '2024-01-15';
```

---

## Integration .NET

### 1. DbContext Setup (EF Core)

```csharp
public class WebzineDbContext : DbContext
{
    public DbSet<ConversationHistory> ConversationHistories { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<ConversationFeedback> ConversationFeedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configuration...
        base.OnModelCreating(modelBuilder);
    }
}
```

### 2. Entities

```csharp
public class ConversationHistory
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public Guid SessionId { get; set; }
    public string UserMessage { get; set; }
    public string AssistantResponse { get; set; }
    public string Source { get; set; } = "wikipedia";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
```

### 3. Service Layer

```csharp
public class ConversationService : IConversationService
{
    private readonly WebzineDbContext _context;

    public async Task<ConversationHistory> SaveMessageAsync(
        string userId, Guid sessionId, string question, string answer, string source)
    {
        var history = new ConversationHistory
        {
            UserId = userId,
            SessionId = sessionId,
            UserMessage = question,
            AssistantResponse = answer,
            Source = source,
            CreatedAt = DateTime.UtcNow
        };

        _context.ConversationHistories.Add(history);
        await _context.SaveChangesAsync();
        return history;
    }

    public async Task<List<ConversationHistory>> GetConversationHistoryAsync(
        string userId, Guid sessionId, int take = 50)
    {
        return await _context.ConversationHistories
            .Where(c => c.UserId == userId && c.SessionId == sessionId)
            .OrderByDescending(c => c.CreatedAt)
            .Take(take)
            .ToListAsync();
    }
}
```

### 4. Controller Integration

```csharp
[HttpPost("query")]
public async Task<IActionResult> Query(
    [FromBody] QuestionRequest request,
    [FromServices] IConversationService conversationService)
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
    var sessionId = HttpContext.Session.GetString("SessionId");
    if (string.IsNullOrEmpty(sessionId))
    {
        sessionId = Guid.NewGuid().ToString();
        HttpContext.Session.SetString("SessionId", sessionId);
    }

    // Call RAG API...
    var answer = await _ragService.QueryAsync(request.Question);

    // Save to database
    await conversationService.SaveMessageAsync(
        userId, Guid.Parse(sessionId), request.Question, answer, currentSource);

    return Ok(new { answer, sources });
}
```

---

## Migration & Versioning

### Initial Migration

```bash
# Créer une migration initiale
dotnet ef migrations add InitialPostgresSetup

# Appliquer la migration
dotnet ef database update
```

### Schema Updates

Pour les mises à jour du schéma:

```bash
# Ajouter une colonne
dotnet ef migrations add AddFeedbackReason

# Réviser si erreur
dotnet ef migrations remove

# Appliquer
dotnet ef database update
```

---

## Queries Examples

### Récupérer l'historique d'un utilisateur

```sql
SELECT * FROM conversation_history
WHERE user_id = 'user_123'
ORDER BY created_at DESC
LIMIT 20;
```

### Statistiques par jour

```sql
SELECT 
    DATE(created_at) as date,
    COUNT(*) as message_count,
    COUNT(DISTINCT user_id) as unique_users,
    source
FROM conversation_history
GROUP BY DATE(created_at), source
ORDER BY date DESC;
```

### Feedback moyen par source

```sql
SELECT 
    ch.source,
    AVG(cf.rating) as avg_rating,
    COUNT(cf.rating) as feedback_count
FROM conversation_history ch
LEFT JOIN conversation_feedback cf ON ch.id = cf.conversation_id
WHERE cf.rating IS NOT NULL
GROUP BY ch.source;
```

### Sessions actives

```sql
SELECT 
    user_id,
    COUNT(*) as session_count,
    MAX(last_activity) as last_active
FROM user_sessions
WHERE last_activity > NOW() - INTERVAL '1 day'
GROUP BY user_id
ORDER BY last_active DESC;
```

---

## Backup & Recovery

### Backup manuel

```bash
# Full backup
docker-compose exec postgres pg_dump -U webzine webzine > backup.sql

# Compressed backup
docker-compose exec postgres pg_dump -U webzine webzine | gzip > backup.sql.gz

# Schema only
docker-compose exec postgres pg_dump -U webzine --schema-only webzine > schema.sql
```

### Restore

```bash
# From SQL file
docker-compose exec -T postgres psql -U webzine webzine < backup.sql

# From compressed
gunzip -c backup.sql.gz | docker-compose exec -T postgres psql -U webzine webzine

# Single table
docker-compose exec -T postgres psql -U webzine webzine < table_backup.sql
```

### Automated Backups

```bash
#!/bin/bash
# backup.sh
BACKUP_DIR="./backups"
mkdir -p $BACKUP_DIR
TIMESTAMP=$(date +%Y%m%d_%H%M%S)

docker-compose exec postgres pg_dump -U webzine webzine | gzip > $BACKUP_DIR/webzine_$TIMESTAMP.sql.gz

# Keep only last 7 days
find $BACKUP_DIR -name "*.sql.gz" -mtime +7 -delete
```

Run via cron:
```bash
0 2 * * * /path/to/backup.sh
```

---

## Performance Tuning

### Connection Pool (appsettings.json)

```json
{
  "ConnectionStrings": {
    "Postgres": "Server=postgres;Port=5432;Database=webzine;User Id=webzine;Password=***;Max Pool Size=20;Min Pool Size=5;"
  }
}
```

### PostgreSQL Optimization

```sql
-- Vacuum (cleanup & statistics)
VACUUM ANALYZE;

-- Index maintenance
REINDEX INDEX idx_conversation_history_user_id;

-- Check slow queries
SELECT query, calls, mean_exec_time, total_exec_time 
FROM pg_stat_statements 
ORDER BY total_exec_time DESC LIMIT 10;
```

---

## Troubleshooting

### Connection refused

```bash
# Vérifier si PostgreSQL tourne
docker-compose exec postgres psql -U webzine -c "SELECT 1;"

# Vérifier les logs
docker-compose logs postgres
```

### Slow queries

```bash
# Analyser une query
EXPLAIN ANALYZE SELECT * FROM conversation_history WHERE created_at > NOW() - INTERVAL '1 day';
```

### Data corruption

```bash
# Rebuild database
docker-compose down -v
docker-compose up -d postgres
# Tables seront recréées automatiquement via init-db.sql
```

---

## Resources

- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [EF Core PostgreSQL](https://www.npgsql.org/efcore/)
- [pg_stat_statements](https://www.postgresql.org/docs/current/pgstatstatements.html)

---

**Next:** Voir DOCKER_SETUP.md pour l'orchestration complète.

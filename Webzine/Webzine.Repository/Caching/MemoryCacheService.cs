using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Repository.Contracts.Caching;

namespace Webzine.Repository.Caching
{
    /// <summary>
    /// Service de cache en mémoire pour stocker les données récupérées depuis les APIs externes.
    /// Permet d'éviter les appels réseau répétés pendant le seeding des données.
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        // Classe interne pour représenter une entrée en cache avec sa date d'expiration
        private class CacheEntry
        {
            public object Value { get; set; }
            public DateTime ExpirationTime { get; set; }
        }

        // Dictionnaire stockant les données en cache : clé -> valeur + expiration
        private readonly Dictionary<string, CacheEntry> _cache = new Dictionary<string, CacheEntry>();

        // Objet de synchronisation pour assurer la thread-safety lors des accès au cache
        // Plusieurs threads peuvent accéder au cache simultanément pendant le seeding
        private readonly object _lockObject = new object();

        /// <summary>
        /// Récupère une valeur du cache si elle existe et n'a pas expiré.
        /// </summary>
        public bool TryGetValue<T>(string key, out T value)
        {
            lock (_lockObject)
            {
                if (_cache.TryGetValue(key, out var entry))
                {
                    // Vérifier si l'entrée a expiré
                    // Si la date/heure actuelle dépasse la date d'expiration, supprimer l'entrée
                    if (DateTime.UtcNow > entry.ExpirationTime)
                    {
                        _cache.Remove(key);
                        value = default;
                        return false;
                    }

                    // Retourner la valeur avec conversion du type
                    value = (T)entry.Value;
                    return true;
                }

                value = default;
                return false;
            }
        }

        /// <summary>
        /// Ajoute ou met à jour une valeur dans le cache avec une durée d'expiration.
        /// </summary>
        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            lock (_lockObject)
            {
                var entry = new CacheEntry
                {
                    Value = value,
                    // Calculer la date d'expiration = maintenant + durée d'expiration
                    ExpirationTime = DateTime.UtcNow.Add(expiration)
                };

                // Ajouter ou remplacer l'entrée dans le dictionnaire
                _cache[key] = entry;
            }
        }

        /// <summary>
        /// Supprime une valeur du cache.
        /// </summary>
        public void Remove(string key)
        {
            lock (_lockObject)
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// Vide complètement le cache. Utile lors du redémarrage ou de réinitialisations.
        /// </summary>
        public void Clear()
        {
            lock (_lockObject)
            {
                _cache.Clear();
            }
        }
    }
}

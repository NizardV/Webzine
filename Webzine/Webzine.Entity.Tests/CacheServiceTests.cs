using Webzine.Repository.Caching;
using Webzine.Repository.Contracts.Caching;

namespace Webzine.Entity.Tests
{
    /// <summary>
    /// Tests unitaires pour le service de cache en mémoire.
    /// Valide les opérations de mise en cache, expiration et suppression des données.
    /// </summary>
    [TestClass]
    public sealed class CacheServiceTests
    {
        private ICacheService _cacheService;

        [TestInitialize]
        public void Setup()
        {
            // Initialiser un nouveau service de cache avant chaque test
            _cacheService = new MemoryCacheService();
        }

        [TestMethod]
        public void Set_And_TryGetValue_Should_Return_Cached_Value()
        {
            // Arrange: préparer les données de test
            var key = "test_key";
            var value = "test_value";
            var expiration = TimeSpan.FromHours(1);

            // Act: ajouter au cache et récupérer
            _cacheService.Set(key, value, expiration);
            var success = _cacheService.TryGetValue<string>(key, out var retrievedValue);

            // Assert: vérifier que la valeur est bien en cache et accessible
            Assert.IsTrue(success);
            Assert.AreEqual(value, retrievedValue);
        }

        [TestMethod]
        public void TryGetValue_Should_Return_False_For_Non_Existent_Key()
        {
            // Arrange: chercher une clé qui n'existe pas
            var key = "non_existent_key";

            // Act: essayer de récupérer
            var success = _cacheService.TryGetValue<string>(key, out var retrievedValue);

            // Assert: doit retourner false car la clé n'existe pas
            Assert.IsFalse(success);
            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void TryGetValue_Should_Return_False_For_Expired_Entry()
        {
            // Arrange: ajouter une valeur avec une durée d'expiration très courte
            var key = "expiring_key";
            var value = "test_value";
            var expiration = TimeSpan.FromMilliseconds(100); // Expire très rapidement

            // Act: ajouter au cache, attendre que ça expire, puis essayer de récupérer
            _cacheService.Set(key, value, expiration);
            System.Threading.Thread.Sleep(200); // Attendre l'expiration
            var success = _cacheService.TryGetValue<string>(key, out var retrievedValue);

            // Assert: doit retourner false car la valeur a expiré
            Assert.IsFalse(success);
            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void Remove_Should_Delete_Cached_Value()
        {
            // Arrange: ajouter une valeur au cache
            var key = "removable_key";
            var value = "test_value";
            var expiration = TimeSpan.FromHours(1);

            _cacheService.Set(key, value, expiration);

            // Act: supprimer la valeur
            _cacheService.Remove(key);
            var success = _cacheService.TryGetValue<string>(key, out var retrievedValue);

            // Assert: la valeur ne doit plus être accessible
            Assert.IsFalse(success);
            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void Clear_Should_Remove_All_Cached_Values()
        {
            // Arrange: ajouter plusieurs valeurs en cache
            var key1 = "key1";
            var key2 = "key2";
            var value = "test_value";
            var expiration = TimeSpan.FromHours(1);

            _cacheService.Set(key1, value, expiration);
            _cacheService.Set(key2, value, expiration);

            // Act: vider tout le cache
            _cacheService.Clear();
            var success1 = _cacheService.TryGetValue<string>(key1, out var retrievedValue1);
            var success2 = _cacheService.TryGetValue<string>(key2, out var retrievedValue2);

            // Assert: aucune valeur ne doit être accessible
            Assert.IsFalse(success1);
            Assert.IsFalse(success2);
        }

        [TestMethod]
        public void Cache_Should_Support_Different_Types()
        {
            // Arrange: préparer différents types de données
            var stringKey = "string_key";
            var stringValue = "string_value";
            var intKey = "int_key";
            var intValue = 42;
            var expiration = TimeSpan.FromHours(1);

            // Act: mettre en cache des données de différents types
            _cacheService.Set(stringKey, stringValue, expiration);
            _cacheService.Set(intKey, intValue, expiration);

            var stringSuccess = _cacheService.TryGetValue<string>(stringKey, out var retrievedString);
            var intSuccess = _cacheService.TryGetValue<int>(intKey, out var retrievedInt);

            // Assert: vérifier que chaque type est bien récupéré
            Assert.IsTrue(stringSuccess);
            Assert.AreEqual(stringValue, retrievedString);
            Assert.IsTrue(intSuccess);
            Assert.AreEqual(intValue, retrievedInt);
        }
    }
}

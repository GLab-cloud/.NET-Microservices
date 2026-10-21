using MongoDB.Bson;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    public class ItemsRepository 
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<Item>(collectionName);      
        }
        public async Task<IReadOnlyCollection<Item>> GetAllItemsAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }
        public async Task<Item?> GetItemAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            return await dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateItemAsync(Item entity)
        { 
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }
        

        public async Task DeleteItemAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
       
        public async Task UpdateItemAsync(Item entity   )
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
        
    }
}
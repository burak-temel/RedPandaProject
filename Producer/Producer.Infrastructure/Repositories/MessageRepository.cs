using MongoDB.Driver;
using Producer.Domain.Models;
using Producer.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Producer.Infrastructure.Settings;

namespace Producer.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<ProducerModel> _abcCollection;

        public MessageRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings)
        {
            _abcCollection = database.GetCollection<ProducerModel>(settings.Value.CollectionName);
        }

        public async Task InsertAsync(ProducerModel model)
        {
            await _abcCollection.InsertOneAsync(model);
        }
    }
}

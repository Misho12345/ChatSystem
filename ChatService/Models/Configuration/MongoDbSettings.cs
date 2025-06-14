namespace ChatService.Models.Configuration;

public class MongoDbSettings
{
    public required string MongoConnection { get; set; }
    public required string MongoDatabaseName { get; set; }
}
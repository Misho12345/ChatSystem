namespace ChatService.Models.Configuration;

/// <summary>
/// Represents the settings required for configuring MongoDB.
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// Gets or sets the connection string for MongoDB.
    /// </summary>
    public required string MongoConnection { get; set; }

    /// <summary>
    /// Gets or sets the name of the MongoDB database.
    /// </summary>
    public required string MongoDatabaseName { get; set; }
}
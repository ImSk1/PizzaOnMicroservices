namespace Menu.API.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
        public string PizzaCollectionName { get; init; }
    }
}

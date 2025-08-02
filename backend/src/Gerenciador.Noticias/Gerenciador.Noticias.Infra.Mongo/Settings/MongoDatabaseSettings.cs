namespace Gerenciador.Noticias.Infra.Mongo.Settings;

public class MongoDatabaseSettings : IMongoDatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}

public interface IMongoDatabaseSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}

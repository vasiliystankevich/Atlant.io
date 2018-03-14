namespace Libraries.Core.Backend.WebApi.Repositories
{
    public interface IVersionRepository
    {
        string Version { get; }
    }

    public class VersionRepository: IVersionRepository
    {
        public string Version { get; } = "1.0.0.0";
    }
}

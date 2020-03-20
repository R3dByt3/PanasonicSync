namespace Configuration.Contracts
{
    public interface IConfigurator
    {
        void Save();
        void Set<T>(T Setting) where T : class;
        T Get<T>() where T : class;
    }
}

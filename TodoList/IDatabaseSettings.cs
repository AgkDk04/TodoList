namespace TodoList
{
    public interface IDatabaseSettings
    {
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
    }
}

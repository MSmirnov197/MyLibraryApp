//using Model;

//namespace DataAccessLayer
//{
//    public interface IRepositoryFactory
//    {
//        IRepository<Book> CreateRepository();
//        IRepository<Book> CreateCachedRepository();
//        IBookRepository CreateBookRepository();
//    }

//    public class RepositoryFactory : IRepositoryFactory
//    {
//        private readonly string _connectionString;

//        public RepositoryFactory(string connectionString)
//        {
//            _connectionString = connectionString;
//        }

//        public IRepository<Book> CreateRepository()
//        {
//            return new DapperRepository<Book>(_connectionString);
//        }

//        public IRepository<Book> CreateCachedRepository()
//        {
//            return new CachedDapperRepository<Book>(_connectionString);
//        }

//        public IBookRepository CreateBookRepository()
//        {

//            throw new NotImplementedException("IBookRepository не поддерживается для generic репозиториев");
//        }
//    }
//}
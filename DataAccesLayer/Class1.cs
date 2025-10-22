using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Определяет общий контракт для репозиториев, обеспечивающих базовые CRUD (Create, Read, Update, Delete) операции
    /// над сущностями, которые являются частью предметной области (реализуют IDomainObject).
    /// </summary>
    /// <typeparam name="T">Тип сущности, с которой работает репозиторий. Должен реализовывать <see cref="IDomainObject"/>.</typeparam>
    public interface IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Добавляет новую сущность в хранилище данных.
        /// </summary>
        /// <param name="entity">Экземпляр сущности, который необходимо добавить.</param>
        void Add(T entity);

        /// <summary>
        /// Удаляет сущность из хранилища данных по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности, которую нужно удалить.</param>
        /// <returns>Значение <c>true</c>, если сущность была успешно удалена; в противном случае <c>false</c>.</returns>
        bool Delete(int id);

        /// <summary>
        /// Извлекает одну сущность из хранилища данных по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности, которую нужно извлечь.</param>
        /// <returns>Экземпляр сущности, если она найдена; в противном случае <c>null</c>.</returns>
        T ReadById(int id);

        /// <summary>
        /// Извлекает все сущности данного типа из хранилища данных.
        /// </summary>
        /// <returns>Коллекция всех сущностей данного типа.</returns>
        IEnumerable<T> ReadAll();

        /// <summary>
        /// Обновляет существующую сущность в хранилище данных.
        /// </summary>
        /// <param name="entity">Экземпляр сущности с обновленными данными. Предполагается, что у него установлен корректный <see cref="IDomainObject.Id"/>.</param>
        /// <returns>Значение <c>true</c>, если сущность была успешно обновлена; в противном случае <c>false</c>.</returns>
        bool Update(T entity);
    }

    /// <summary>
    /// Контекст базы данных, использующий Entity Framework Core для управления доступом к данным.
    /// Определяет DbSet для различных сущностей и настраивает параметры подключения.
    /// </summary>
    public class LibraryDbContext : DbContext
    {
        /// <summary>
        /// Представляет набор сущностей типа <see cref="Book"/> в базе данных.
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Конфигурирует опции контекста базы данных.
        /// Указывает тип базы данных (SQL Server) и строку подключения.
        /// </summary>
        /// <param name="optionsBuilder">Строитель опций, позволяющий настроить контекст.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\РС\\source\\repos\\MyLibraryApp\\DataAccesLayer\\Database1.mdf;Integrated Security=True;");
        }
    }

    /// <summary>
    /// Реализация репозитория, использующая Entity Framework Core для выполнения CRUD-операций.
    /// Предоставляет стандартизированный доступ к данным для определенного типа сущности <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующей <see cref="IDomainObject"/>, для которого предназначен репозиторий.</typeparam>
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly LibraryDbContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityRepository{T}"/> с использованием предоставленного контекста.
        /// </summary>
        /// <param name="context">Экземпляр <see cref="LibraryDbContext"/>, используемый для взаимодействия с базой данных.</param>
        public EntityRepository(LibraryDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Добавляет новую сущность <typeparamref name="T"/> в контекст базы данных и сохраняет изменения.
        /// </summary>
        /// <param name="entity">Экземпляр сущности для добавления.</param>
        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Удаляет сущность <typeparamref name="T"/> из хранилища по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        /// <returns><c>true</c>, если сущность была найдена и удалена; <c>false</c> в противном случае.</returns>
        public bool Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Извлекает сущность <typeparamref name="T"/> по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для поиска.</param>
        /// <returns>Найденная сущность или <c>null</c>, если сущность не существует.</returns>
        public T ReadById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Извлекает все сущности <typeparamref name="T"/> из хранилища.
        /// </summary>
        /// <returns>Список всех сущностей данного типа.</returns>
        public IEnumerable<T> ReadAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>
        /// Обновляет существующую сущность <typeparamref name="T"/> в хранилище.
        /// </summary>
        /// <param name="entity">Экземпляр сущности с обновленными данными.</param>
        /// <returns><c>true</c>, если сущность была найдена и обновлена; <c>false</c> в противном случае.</returns>
        public bool Update(T entity)
        {
            var existingEntity = _dbSet.Find(entity.Id);
            if (existingEntity == null) return false;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            return true;
        }
    }

    /// <summary>
    /// Реализация репозитория, использующая библиотеку Dapper для прямого выполнения SQL-запросов.
    /// Взаимодействует с базой данных напрямую.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующей <see cref="IDomainObject"/>, для которой предназначен репозиторий.
    /// Ориентирован на таблицу Books.</typeparam>
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DapperRepository{T}"/> с строкой подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных SQL Server.</param>
        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет сущность в таблицу "Books" базы данных, используя Dapper.
        /// </summary>
        /// <param name="entity">Экземпляр сущности <typeparamref name="T"/> для добавления. Предполагается, что это <see cref="Book"/>.</param>
        /// <exception cref="SqlException">Вызывается при возникновении ошибки при выполнении SQL-запроса.</exception>
        public void Add(T entity)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute("INSERT INTO Books (Id, Title, Author, Genre, Year, Quantity) VALUES (@Id, @Title, @Author, @Genre, @Year, @Quantity)", entity);
            }
        }

        /// <summary>
        /// Удаляет сущность из таблицы "Books" по ее идентификатору, используя Dapper.
        /// </summary>
        /// <param name="id">Идентификатор сущности (книги) для удаления.</param>
        /// <returns><c>true</c>, если была удалена хотя бы одна строка; в противном случае <c>false</c>.</returns>
        /// <exception cref="SqlException">Вызывается при возникновении ошибки при выполнении SQL-запроса.</exception>
        public bool Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var affectedRows = db.Execute("DELETE FROM Books WHERE Id = @Id", new { Id = id });
                return affectedRows > 0;
            }
        }

        /// <summary>
        /// Извлекает одну сущность (книгу) из таблицы "Books" по ее идентификатору, используя Dapper.
        /// </summary>
        /// <param name="id">Идентификатор сущности (книги) для поиска.</param>
        /// <returns>Найденная сущность типа <typeparamref name="T"/>, или <c>null</c>, если сущность не найдена.</returns>
        /// <exception cref="SqlException">Вызывается при возникновении ошибки при выполнении SQL-запроса.</exception>
        public T ReadById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.QueryFirstOrDefault<T>("SELECT * FROM Books WHERE Id = @Id", new { Id = id });
            }
        }

        /// <summary>
        /// Извлекает все сущности (книги) из таблицы "Books", используя Dapper.
        /// </summary>
        /// <returns>Коллекция всех найденных сущностей типа <typeparamref name="T"/>.</returns>
        /// <exception cref="SqlException">Вызывается при возникновении ошибки при выполнении SQL-запроса.</exception>
        public IEnumerable<T> ReadAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<T>("SELECT * FROM Books");
            }
        }

        /// <summary>
        /// Обновляет существующую сущность (книгу) в таблице "Books", используя Dapper.
        /// </summary>
        /// <param name="entity">Экземпляр сущности с обновленными данными. Предполагается, что у него установлен корректный <see cref="IDomainObject.Id"/>.</param>
        /// <returns><c>true</c>, если была обновлена хотя бы одна строка; в противном случае <c>false</c>.</returns>
        /// <exception cref="SqlException">Вызывается при возникновении ошибки при выполнении SQL-запроса.</exception>
        public bool Update(T entity)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var affectedRows = db.Execute("UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre, Year = @Year, Quantity = @Quantity WHERE Id = @Id", entity);
                return affectedRows > 0;
            }
        }
    }
}
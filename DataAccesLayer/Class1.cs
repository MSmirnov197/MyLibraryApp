using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория, использующая библиотеку Dapper для прямого выполнения SQL-запросов.
    /// Взаимодействует с базой данных напрямую.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующей <see cref="IDomainObject"/>, для которой предназначен репозиторий.
    /// Ориентирован на таблицу Books.</typeparam>
    public class DapperRepository<T> : BaseRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DapperRepository{T}"/> с строкой подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных SQL Server.</param>
        public DapperRepository(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Добавляет сущность в таблицу "Books" базы данных, используя Dapper.
        /// </summary>
        /// <param name="entity">Экземпляр сущности <typeparamref name="T"/> для добавления. Предполагается, что это <see cref="Book"/>.</param>
        /// <exception cref="SqlException">Вызывается при возникновении ошибки при выполнении SQL-запроса.</exception>
        public override void Add(T entity)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
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
        public override bool Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
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
        public override T? ReadById(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                return db.QueryFirstOrDefault<T>("SELECT * FROM Books WHERE Id = @Id", new { Id = id });
            }
        }

        /// <summary>
        /// Извлекает все сущности (книги) из таблицы "Books", используя Dapper.
        /// </summary>
        /// <returns>Коллекция всех найденных сущностей типа <typeparamref name="T"/>.</returns>
        /// <exception cref="SqlException">Вызывается при возникновении ошибки при выполнении SQL-запроса.</exception>
        public override IEnumerable<T> ReadAll()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
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
        public override bool Update(T entity)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                var affectedRows = db.Execute("UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre, Year = @Year, Quantity = @Quantity WHERE Id = @Id", entity);
                return affectedRows > 0;
            }
        }
    }

    /// <summary>
    /// Кэширующая реализация репозитория Dapper, сохраняющая часто запрашиваемые данные в памяти.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующей <see cref="IDomainObject"/>.</typeparam>
    public class CachedDapperRepository<T> : DapperRepository<T> where T : class, IDomainObject
    {
        private readonly Dictionary<int, T> _cache = new Dictionary<int, T>();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CachedDapperRepository{T}"/> с строкой подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных SQL Server.</param>
        public CachedDapperRepository(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Извлекает сущность по идентификатору с использованием кэширования.
        /// </summary>
        /// <param name="id">Идентификатор сущности для поиска.</param>
        /// <returns>Найденная сущность или <c>null</c>, если сущность не найдена.</returns>
        public override T? ReadById(int id)
        {
            if (_cache.ContainsKey(id))
                return _cache[id];

            var entity = base.ReadById(id);
            if (entity != null)
                _cache[id] = entity;

            return entity;
        }

        /// <summary>
        /// Обновляет сущность в базе данных и кэше.
        /// </summary>
        /// <param name="entity">Экземпляр сущности с обновленными данными.</param>
        /// <returns><c>true</c>, если сущность была обновлена; <c>false</c> в противном случае.</returns>
        public override bool Update(T entity)
        {
            var result = base.Update(entity);
            if (result)
                _cache[entity.Id] = entity;

            return result;
        }

        /// <summary>
        /// Удаляет сущность из базы данных и кэша.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        /// <returns><c>true</c>, если сущность была удалена; <c>false</c> в противном случае.</returns>
        public override bool Delete(int id)
        {
            var result = base.Delete(id);
            if (result)
                _cache.Remove(id);

            return result;
        }
    }
}
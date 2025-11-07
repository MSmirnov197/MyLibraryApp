using System.Collections.Generic;
using System.Linq;
using Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Абстрактный базовый класс для репозиториев, предоставляющий общую функциональность.
    /// Реализует шаблон Template Method для определения общей структуры репозиториев.
    /// Демонстрирует принцип повторного использования кода (DRY) и принцип подстановки Лисков.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий интерфейс IDomainObject.</typeparam>
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Строка подключения к базе данных.
        /// </summary>
        protected readonly string ConnectionString;

        /// <summary>
        /// Инициализирует новый экземпляр базового репозитория.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        protected BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Добавляет новую сущность в хранилище данных.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        public abstract void Add(T entity);

        /// <summary>
        /// Удаляет сущность из хранилища данных по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        /// <returns>True, если удаление прошло успешно, иначе False.</returns>
        public abstract bool Delete(int id);

        /// <summary>
        /// Находит сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для поиска.</param>
        /// <returns>Найденная сущность или null, если сущность не найдена.</returns>
        public abstract T? ReadById(int id);

        /// <summary>
        /// Получает все сущности из хранилища данных.
        /// </summary>
        /// <returns>Коллекция всех сущностей.</returns>
        public abstract IEnumerable<T> ReadAll();

        /// <summary>
        /// Обновляет существующую сущность в хранилище данных.
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными.</param>
        /// <returns>True, если обновление прошло успешно, иначе False.</returns>
        public abstract bool Update(T entity);

        /// <summary>
        /// Проверяет существование сущности по идентификатору.
        /// Виртуальный метод с реализацией по умолчанию - демонстрирует принцип подстановки Лисков.
        /// </summary>
        /// <param name="id">Идентификатор для проверки.</param>
        /// <returns>True, если сущность существует, иначе False.</returns>
        public virtual bool Exists(int id)
        {
            return ReadById(id) != null;
        }

        /// <summary>
        /// Подсчитывает общее количество сущностей в хранилище.
        /// Виртуальный метод с реализацией по умолчанию.
        /// </summary>
        /// <returns>Количество сущностей.</returns>
        public virtual int Count()
        {
            return ReadAll().Count();
        }
    }
}
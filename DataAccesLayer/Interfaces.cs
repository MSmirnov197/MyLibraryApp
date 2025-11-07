using System.Collections.Generic;
using Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Определяет контракт для репозиториев, работающих с сущностями предметной области.
    /// Реализует принцип инверсии зависимостей - клиенты зависят от абстракций, а не от конкретных реализаций.
    /// </summary>
    /// <typeparam name="T">Тип сущности, реализующий интерфейс IDomainObject.</typeparam>
    public interface IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Добавляет новую сущность в хранилище данных.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        void Add(T entity);

        /// <summary>
        /// Удаляет сущность из хранилища данных по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        /// <returns>True, если удаление прошло успешно, иначе False.</returns>
        bool Delete(int id);

        /// <summary>
        /// Находит сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для поиска.</param>
        /// <returns>Найденная сущность или null, если сущность не найдена.</returns>
        T? ReadById(int id);

        /// <summary>
        /// Получает все сущности из хранилища данных.
        /// </summary>
        /// <returns>Коллекция всех сущностей.</returns>
        IEnumerable<T> ReadAll();

        /// <summary>
        /// Обновляет существующую сущность в хранилище данных.
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными.</param>
        /// <returns>True, если обновление прошло успешно, иначе False.</returns>
        bool Update(T entity);

        /// <summary>
        /// Проверяет существование сущности по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор для проверки.</param>
        /// <returns>True, если сущность существует, иначе False.</returns>
        bool Exists(int id);

        /// <summary>
        /// Подсчитывает общее количество сущностей в хранилище.
        /// </summary>
        /// <returns>Количество сущностей.</returns>
        int Count();
    }
}
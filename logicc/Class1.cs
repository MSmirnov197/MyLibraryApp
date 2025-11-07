using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using Model;

namespace logicc
{
    /// <summary>
    /// Основной класс бизнес-логики приложения, координирующий работу различных сервисов.
    /// Демонстрирует принцип единственной ответственности - делегирование задач специализированным компонентам.
    /// Реализует принцип инверсии зависимостей через внедрение зависимостей в конструктор.
    /// </summary>
    public class Logic
    {
        private readonly IRepository<Book> _repository;
        private readonly IBookValidator _validator;
        private readonly IBookMapper _mapper;
        private readonly IEnumerable<IBookGrouper> _groupers;
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса Logic с указанными зависимостями.
        /// Демонстрирует принцип инверсии зависимостей - зависимости внедряются извне.
        /// </summary>
        /// <param name="repository">Репозиторий для работы с данными книг.</param>
        /// <param name="validator">Валидатор для проверки данных книг.</param>
        /// <param name="mapper">Маппер для преобразования между Entity и DTO.</param>
        /// <param name="groupers">Коллекция стратегий для группировки книг.</param>
        /// <param name="logger">Логгер для записи информации о операциях.</param>
        public Logic(IRepository<Book> repository, IBookValidator validator,
                    IBookMapper mapper, IEnumerable<IBookGrouper> groupers, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _groupers = groupers ?? throw new ArgumentNullException(nameof(groupers));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Создает новую книгу после валидации данных.
        /// Демонстрирует принцип единственной ответственности - каждая операция делегируется специализированному компоненту.
        /// </summary>
        /// <param name="id">Уникальный идентификатор книги.</param>
        /// <param name="title">Название книги.</param>
        /// <param name="author">Автор книги.</param>
        /// <param name="genre">Жанр книги.</param>
        /// <param name="year">Год издания.</param>
        /// <param name="quantity">Количество экземпляров.</param>
        /// <returns>Результат операции с информацией об успехе или ошибке.</returns>
        public OperationResult CreateBook(int id, string title, string author, string genre, int year, int quantity)
        {
            try
            {
                _validator.ValidateBookData(id, title, author, genre, year, quantity);

                if (_repository.Exists(id))
                    return OperationResult.Fail($"Книга с ID {id} уже существует.");

                var book = _mapper.ToEntity(id, title, author, genre, year, quantity);
                _repository.Add(book);

                _logger.LogInfo($"Книга '{title}' успешно добавлена с ID {id}");
                return OperationResult.Ok("Книга успешно добавлена!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при создании книги: {ex.Message}", ex);
                return OperationResult.Fail(ex.Message, ex);
            }
        }

        /// <summary>
        /// Удаляет книгу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор книги для удаления.</param>
        /// <returns>True, если книга была удалена, иначе False.</returns>
        public bool DeleteBook(int id)
        {
            try
            {
                var result = _repository.Delete(id);
                if (result)
                    _logger.LogInfo($"Книга с ID {id} успешно удалена");
                else
                    _logger.LogWarning($"Попытка удаления несуществующей книги с ID {id}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении книги с ID {id}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Находит книгу по идентификатору и возвращает в формате DTO.
        /// </summary>
        /// <param name="id">Идентификатор книги для поиска.</param>
        /// <returns>Объект BookDTO или null, если книга не найдена.</returns>
        public BookDTO ReadBook(int id)
        {
            try
            {
                var book = _repository.ReadById(id);
                return book == null ? null : _mapper.ToDTO(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при чтении книги с ID {id}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Обновляет информацию о существующей книге.
        /// </summary>
        /// <param name="id">Идентификатор книги для обновления.</param>
        /// <param name="title">Новое название книги.</param>
        /// <param name="author">Новый автор книги.</param>
        /// <param name="genre">Новый жанр книги.</param>
        /// <param name="year">Новый год издания.</param>
        /// <param name="quantity">Новое количество экземпляров.</param>
        /// <returns>True, если книга была обновлена, иначе False.</returns>
        public bool UpdateBook(int id, string title, string author, string genre, int year, int quantity)
        {
            try
            {
                _validator.ValidateBookData(id, title, author, genre, year, quantity);

                var book = _mapper.ToEntity(id, title, author, genre, year, quantity);
                var result = _repository.Update(book);

                if (result)
                    _logger.LogInfo($"Книга с ID {id} успешно обновлена");
                else
                    _logger.LogWarning($"Попытка обновления несуществующей книги с ID {id}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при обновлении книги с ID {id}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Группирует книги по жанру используя соответствующую стратегию.
        /// Демонстрирует принцип открытости/закрытости - новые стратегии группировки можно добавлять без изменения этого метода.
        /// </summary>
        /// <returns>Словарь сгруппированных книг по жанрам.</returns>
        public Dictionary<string, List<BookDTO>> GroupBooksByGenre()
        {
            var grouper = _groupers.FirstOrDefault(g => g.GroupKey == "Genre");
            return grouper?.Group(_repository.ReadAll()) ?? new Dictionary<string, List<BookDTO>>();
        }

        /// <summary>
        /// Группирует книги по указанному критерию.
        /// Демонстрирует применение паттерна Стратегия для различных алгоритмов группировки.
        /// </summary>
        /// <param name="groupKey">Ключ группировки (Genre, Year, Author).</param>
        /// <returns>Словарь сгруппированных книг по указанному критерию.</returns>
        public Dictionary<string, List<BookDTO>> GroupBooksBy(string groupKey)
        {
            var grouper = _groupers.FirstOrDefault(g => g.GroupKey == groupKey);
            return grouper?.Group(_repository.ReadAll()) ?? new Dictionary<string, List<BookDTO>>();
        }

        /// <summary>
        /// Возвращает доступные стратегии группировки.
        /// Демонстрирует принцип открытости/закрытости - система легко расширяется новыми стратегиями.
        /// </summary>
        /// <returns>Коллекция доступных ключей группировки.</returns>
        public IEnumerable<string> GetAvailableGroupers()
        {
            return _groupers.Select(g => g.GroupKey);
        }

        /// <summary>
        /// Получает все книги из репозитория и преобразует их в DTO.
        /// </summary>
        /// <returns>Список всех книг в формате DTO.</returns>
        public List<BookDTO> GetAllBooks()
        {
            try
            {
                var books = _repository.ReadAll();
                return books.Select(b => _mapper.ToDTO(b)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при получении всех книг: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Возвращает массив допустимых жанров книг.
        /// </summary>
        /// <returns>Массив строк с названиями жанров.</returns>
        public string[] GetAvailableGenres()
        {
            return new[] { "drama", "science fiction", "adventure", "novel", "short story", "detective", "scientific literature" };
        }

        /// <summary>
        /// Выполняет поиск книг по названию, автору или жанру.
        /// Реализует поиск в памяти как fallback механизм.
        /// </summary>
        /// <param name="searchTerm">Поисковый запрос.</param>
        /// <returns>Список книг, соответствующих поисковому запросу.</returns>
        public List<BookDTO> SearchBooks(string searchTerm)
        {
            try
            {
                // Fallback: фильтрация в памяти
                var allBooks = _repository.ReadAll();
                return allBooks.Where(b =>
                    (b.Title?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                    (b.Author?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                    (b.Genre?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true))
                    .Select(b => _mapper.ToDTO(b))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при поиске книг по запросу '{searchTerm}': {ex.Message}", ex);
                throw;
            }
        }
    }
}
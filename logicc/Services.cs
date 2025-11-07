using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;

namespace logicc
{
    /// <summary>
    /// Результат операции, содержащий информацию об успешности выполнения и возможные ошибки.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Указывает, была ли операция успешной.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Сообщение о результате операции.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Исключение, возникшее при выполнении операции.
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// Создает успешный результат операции.
        /// </summary>
        /// <param name="message">Сообщение об успехе.</param>
        /// <returns>Экземпляр успешного результата.</returns>
        public static OperationResult Ok(string? message = null) => new OperationResult { Success = true, Message = message };

        /// <summary>
        /// Создает неуспешный результат операции.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="ex">Исключение, вызвавшее ошибку.</param>
        /// <returns>Экземпляр неуспешного результата.</returns>
        public static OperationResult Fail(string message, Exception? ex = null) => new OperationResult { Success = false, Message = message, Exception = ex };
    }

    /// <summary>
    /// Определяет контракт для валидации данных книги.
    /// </summary>
    public interface IBookValidator
    {
        /// <summary>
        /// Проверяет корректность данных книги.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="title">Название книги.</param>
        /// <param name="author">Автор книги.</param>
        /// <param name="genre">Жанр книги.</param>
        /// <param name="year">Год издания.</param>
        /// <param name="quantity">Количество экземпляров.</param>
        void ValidateBookData(int id, string title, string author, string genre, int year, int quantity);

        /// <summary>
        /// Проверяет корректность объекта книги.
        /// </summary>
        /// <param name="book">Объект книги для проверки.</param>
        void ValidateBook(Book book);
    }

    /// <summary>
    /// Реализация валидатора книг.
    /// </summary>
    public class BookValidator : IBookValidator
    {
        /// <summary>
        /// Проверяет корректность данных книги.
        /// </summary>
        public void ValidateBookData(int id, string title, string author, string genre, int year, int quantity)
        {
            if (id <= 0) throw new ArgumentException("ID должен быть положительным числом.");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Название не может быть пустым.");
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Автор не может быть пустым.");
            if (string.IsNullOrWhiteSpace(genre)) throw new ArgumentException("Жанр не может быть пустым.");
            if (year < 1000 || year > 2025) throw new ArgumentException("Год должен быть от 1000 до 2025.");
            if (quantity < 0) throw new ArgumentException("Количество не может быть отрицательным.");
        }

        /// <summary>
        /// Проверяет корректность объекта книги.
        /// </summary>
        public void ValidateBook(Book book)
        {
            ValidateBookData(book.Id, book.Title, book.Author, book.Genre, book.Year, book.Quantity);
        }
    }

    /// <summary>
    /// Определяет контракт для преобразования между Entity и DTO.
    /// </summary>
    public interface IBookMapper
    {
        /// <summary>
        /// Преобразует Entity в DTO.
        /// </summary>
        /// <param name="book">Объект книги Entity.</param>
        /// <returns>Объект книги DTO.</returns>
        BookDTO ToDTO(Book book);

        /// <summary>
        /// Преобразует DTO в Entity.
        /// </summary>
        /// <param name="dto">Объект книги DTO.</param>
        /// <returns>Объект книги Entity.</returns>
        Book ToEntity(BookDTO dto);

        /// <summary>
        /// Создает Entity из отдельных параметров.
        /// </summary>
        /// <param name="id">Идентификатор книги.</param>
        /// <param name="title">Название книги.</param>
        /// <param name="author">Автор книги.</param>
        /// <param name="genre">Жанр книги.</param>
        /// <param name="year">Год издания.</param>
        /// <param name="quantity">Количество экземпляров.</param>
        /// <returns>Объект книги Entity.</returns>
        Book ToEntity(int id, string title, string author, string genre, int year, int quantity);
    }

    /// <summary>
    /// Реализация маппера для преобразования между Entity и DTO.
    /// </summary>
    public class BookMapper : IBookMapper
    {
        /// <summary>
        /// Преобразует Entity в DTO.
        /// </summary>
        public BookDTO ToDTO(Book book)
        {
            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                Year = book.Year,
                Quantity = book.Quantity
            };
        }

        /// <summary>
        /// Преобразует DTO в Entity.
        /// </summary>
        public Book ToEntity(BookDTO dto)
        {
            return new Book
            {
                Id = dto.Id,
                Title = dto.Title ?? string.Empty,
                Author = dto.Author ?? string.Empty,
                Genre = dto.Genre ?? string.Empty,
                Year = dto.Year,
                Quantity = dto.Quantity
            };
        }

        /// <summary>
        /// Создает Entity из отдельных параметров.
        /// </summary>
        public Book ToEntity(int id, string title, string author, string genre, int year, int quantity)
        {
            return new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Genre = genre,
                Year = year,
                Quantity = quantity
            };
        }
    }

    /// <summary>
    /// Определяет контракт для стратегий группировки книг.
    /// </summary>
    public interface IBookGrouper
    {
        /// <summary>
        /// Получает ключ группировки.
        /// </summary>
        string GroupKey { get; }

        /// <summary>
        /// Группирует книги по заданному критерию.
        /// </summary>
        /// <param name="books">Коллекция книг для группировки.</param>
        /// <returns>Словарь сгруппированных книг.</returns>
        Dictionary<string, List<BookDTO>> Group(IEnumerable<Book> books);
    }

    /// <summary>
    /// Стратегия группировки книг по жанру.
    /// </summary>
    public class GenreGrouper : IBookGrouper
    {
        /// <summary>
        /// Получает ключ группировки - "Genre".
        /// </summary>
        public string GroupKey => "Genre";

        /// <summary>
        /// Группирует книги по жанру.
        /// </summary>
        public Dictionary<string, List<BookDTO>> Group(IEnumerable<Book> books)
        {
            return books.GroupBy(b => b.Genre)
                       .ToDictionary(g => g.Key, g => g.Select(b => new BookDTO
                       {
                           Id = b.Id,
                           Title = b.Title,
                           Author = b.Author,
                           Genre = b.Genre,
                           Year = b.Year,
                           Quantity = b.Quantity
                       }).ToList());
        }
    }

    /// <summary>
    /// Стратегия группировки книг по году издания.
    /// Годы сортируются в порядке возрастания, а внутри каждой группы книги сортируются по названию.
    /// </summary>
    public class YearGrouper : IBookGrouper
    {
        /// <summary>
        /// Получает ключ группировки - "Year".
        /// </summary>
        public string GroupKey => "Year";

        /// <summary>
        /// Группирует книги по году издания с комплексной сортировкой.
        /// Демонстрирует принцип улучшения пользовательского опыта через упорядочивание данных.
        /// </summary>
        public Dictionary<string, List<BookDTO>> Group(IEnumerable<Book> books)
        {
            var grouped = books
                .GroupBy(b => b.Year)
                .OrderBy(g => g.Key)  // Сортируем группы по году (от меньшего к большему)
                .ToDictionary(
                    g => $"Год: {g.Key}",
                    g => g.OrderBy(b => b.Title)  // Сортируем книги внутри группы по названию
                          .Select(b => new BookDTO
                          {
                              Id = b.Id,
                              Title = b.Title,
                              Author = b.Author,
                              Genre = b.Genre,
                              Year = b.Year,
                              Quantity = b.Quantity
                          }).ToList()
                );

            return grouped;
        }
    }

    /// <summary>
    /// Стратегия группировки книг по автору.
    /// </summary>
    public class AuthorGrouper : IBookGrouper
    {
        /// <summary>
        /// Получает ключ группировки - "Author".
        /// </summary>
        public string GroupKey => "Author";

        /// <summary>
        /// Группирует книги по автору.
        /// </summary>
        public Dictionary<string, List<BookDTO>> Group(IEnumerable<Book> books)
        {
            return books.GroupBy(b => b.Author)
                       .ToDictionary(g => g.Key, g => g.Select(b => new BookDTO
                       {
                           Id = b.Id,
                           Title = b.Title,
                           Author = b.Author,
                           Genre = b.Genre,
                           Year = b.Year,
                           Quantity = b.Quantity
                       }).ToList());
        }
    }

    /// <summary>
    /// Определяет контракт для системы логирования.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Записывает информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void LogInfo(string message);

        /// <summary>
        /// Записывает предупреждающее сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void LogWarning(string message);

        /// <summary>
        /// Записывает сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="ex">Исключение, вызвавшее ошибку.</param>
        void LogError(string message, Exception? ex = null);
    }

    /// <summary>
    /// Реализация логгера, записывающего сообщения в файл.
    /// </summary>
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        /// <summary>
        /// Инициализирует новый экземпляр файлового логгера.
        /// </summary>
        public FileLogger()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");
        }

        /// <summary>
        /// Записывает информационное сообщение.
        /// </summary>
        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        /// <summary>
        /// Записывает предупреждающее сообщение.
        /// </summary>
        public void LogWarning(string message)
        {
            Log("WARN", message);
        }

        /// <summary>
        /// Записывает сообщение об ошибке.
        /// </summary>
        public void LogError(string message, Exception? ex = null)
        {
            Log("ERROR", $"{message} {(ex != null ? $"- {ex}" : "")}");
        }

        /// <summary>
        /// Записывает сообщение в лог-файл.
        /// </summary>
        /// <param name="level">Уровень логирования.</param>
        /// <param name="message">Текст сообщения.</param>
        private void Log(string level, string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // Если не удалось записать в файл, игнорируем
            }
        }
    }
}
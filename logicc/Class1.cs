using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using Model;

namespace logicc
{
    /// <summary>
    /// Предоставляет бизнес-логику для работы с книгами, используя репозиторий.
    /// </summary>
    public class Logic
    {
        private readonly IRepository<Book> _repository;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Logic"/> с использованием указанного репозитория.
        /// </summary>
        /// <param name="repository">Репозиторий для работы с сущностями <see cref="Book"/>.</param>
        public Logic()
        {
            //var dbContext = new LibraryDbContext();
            //var bookRepository = new EntityRepository<Model.Book>(dbContext);
            //logic = new Logic(bookRepository);
            _repository = new DapperRepository<Model.Book>("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\РС\\source\\repos\\MyLibraryApp\\DataAccesLayer\\Database1.mdf;Integrated Security=True");
        }

        /// <summary>
        /// Создает новую книгу и добавляет ее в хранилище.
        /// </summary>
        /// <param name="id">Уникальный идентификатор книги.</param>
        /// <param name="title">Название книги.</param>
        /// <param name="author">Автор книги.</param>
        /// <param name="genre">Жанр книги.</param>
        /// <param name="year">Год издания.</param>
        /// <param name="quantity">Количество экземпляров.</param>
        public void CreateBook(int id, string title, string author, string genre, int year, int quantity)
        {
            var book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Genre = genre,
                Year = year,
                Quantity = quantity
            };
            _repository.Add(book);
        }

        /// <summary>
        /// Удаляет книгу из хранилища по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор книги для удаления.</param>
        /// <returns>True, если книга была успешно удалена; в противном случае False.</returns>
        public bool DeleteBook(int id)
        {
            return _repository.Delete(id);
        }

        /// <summary>
        /// Читает информацию о книге по ее идентификатору и возвращает ее в виде DTO.
        /// </summary>
        /// <param name="id">Идентификатор книги для чтения.</param>
        /// <returns>Объект <see cref="BookDTO"/>, содержащий информацию о книге, или null, если книга не найдена.</returns>
        public BookDTO ReadBook(int id)
        {
            var book = _repository.ReadById(id);
            return book == null ? null : new BookDTO
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
        /// Обновляет существующую книгу в хранилище.
        /// </summary>
        /// <param name="id">Идентификатор книги для обновления.</param>
        /// <param name="title">Новое название книги.</param>
        /// <param name="author">Новый автор книги.</param>
        /// <param name="genre">Новый жанр книги.</param>
        /// <param name="year">Новый год издания.</param>
        /// <param name="quantity">Новое количество экземпляров.</param>
        /// <returns>True, если книга была успешно обновлена; в противном случае False.</returns>
        public bool UpdateBook(int id, string title, string author, string genre, int year, int quantity)
        {
            var book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Genre = genre,
                Year = year,
                Quantity = quantity
            };
            return _repository.Update(book);
        }

        /// <summary>
        /// Группирует все книги в хранилище по их жанрам.
        /// </summary>
        /// <returns>Словарь, где ключ - жанр, а значение - список объектов <see cref="BookDTO"/>, относящихся к этому жанру.</returns>
        public Dictionary<string, List<BookDTO>> GroupBooksByGenre()
        {
            var books = _repository.ReadAll();
            return books.GroupBy(b => b.Genre)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(b => new BookDTO
                            {
                                Id = b.Id,
                                Title = b.Title,
                                Author = b.Author,
                                Genre = b.Genre,
                                Year = b.Year,
                                Quantity = b.Quantity
                            }).ToList()
                        );
        }

        /// <summary>
        /// Получает список всех книг из хранилища, преобразуя их в объекты <see cref="BookDTO"/>.
        /// </summary>
        /// <returns>Список всех книг.</returns>
        public List<BookDTO> GetAllBooks()
        {
            var books = _repository.ReadAll();
            return books.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                Year = b.Year,
                Quantity = b.Quantity
            }).ToList();
        }

        /// <summary>
        /// Предоставляет предопределенный массив доступных жанров.
        /// </summary>
        /// <returns>Массив строк, представляющих доступные жанры.</returns>
        public string[] GetAvailableGenres()
        {
            return new[] { "drama", "science fiction", "adventure", "novel", "short story", "detective", "scientific literature" };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace logicc
{
    /// <summary>
    /// Представляет логику приложения для управления книгами.
    /// </summary>
    public class Logic
    {
        private List<Book> books = new List<Book>();

        /// <summary>
        /// Создает новую книгу с указанным ID и добавляет ее в список.
        /// </summary>
        /// <param name="id">Уникальный идентификатор книги.</param>
        /// <param name="title">Название книги.</param>
        /// <param name="author">Автор книги.</param>
        /// <param name="genre">Жанр книги.</param>
        /// <param name="year">Год издания книги.</param>
        /// <param name="quantity">Количество книг.</param>
        public void CreateBook(int id, string title, string author, string genre, int year, int quantity)
        {
            if (books.Any(b => b.Id == id))
                throw new ArgumentException($"Книга с ID {id} уже существует.");

            var book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Genre = genre,
                Year = year,
                Quantity = quantity
            };
            books.Add(book);
        }

        /// <summary>
        /// Удаляет книгу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор книги для удаления.</param>
        /// <returns>Возвращает true, если книга была успешно удалена, и false в противном случае.</returns>
        public bool DeleteBook(int id)
        {
            return books.RemoveAll(b => b.Id == id) > 0;
        }

        /// <summary>
        /// Возвращает книгу по идентификатору в виде DTO.
        /// </summary>
        /// <param name="id">Идентификатор книги для поиска.</param>
        /// <returns>Объект BookDTO, если книга найдена, и null в противном случае.</returns>
        public BookDTO ReadBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
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
        /// Обновляет информацию о книге по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор книги для обновления.</param>
        /// <param name="title">Новое название книги.</param>
        /// <param name="author">Новый автор книги.</param>
        /// <param name="genre">Новый жанр книги.</param>
        /// <param name="year">Новый год издания книги.</param>
        /// <param name="quantity">Новое количество книг.</param>
        /// <returns>Возвращает true, если книга была успешно обновлена, и false в противном случае.</returns>
        public bool UpdateBook(int id, string title, string author, string genre, int year, int quantity)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return false;

            book.Title = title;
            book.Author = author;
            book.Genre = genre;
            book.Year = year;
            book.Quantity = quantity;
            return true;
        }

        /// <summary>
        /// Группирует книги по жанрам и возвращает в виде DTO.
        /// </summary>
        /// <returns>Словарь, где ключ - жанр, а значение - список BookDTO этого жанра.</returns>
        public Dictionary<string, List<BookDTO>> GroupBooksByGenre()
        {
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
        /// Возвращает список всех книг в виде BookDTO.
        /// </summary>
        /// <returns>Список всех книг в виде BookDTO.</returns>
        public List<BookDTO> GetAllBooks()
        {
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
        /// Возвращает список доступных жанров.
        /// </summary>
        /// <returns>Массив доступных жанров.</returns>
        public string[] GetAvailableGenres()
        {
            return new[] { "драма", "фантастика", "приключения", "роман", "повесть", "детектив", "научная литература" };
        }
    }
}
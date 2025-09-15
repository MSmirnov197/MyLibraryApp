using System;
using System.Text.RegularExpressions;

namespace Model
{
    /// <summary>
    /// Объект передачи данных для книги — используется для взаимодействия между Logic и View.
    /// Скрывает детали реализации модели Book.
    /// </summary>
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; } // Добавлено свойство Quantity

        public override string ToString()
        {
            return $"{Id}. {Title} ({Author}, {Genre}, {Year}, Количество: {Quantity})";
        }
    }

    /// <summary>
    /// Представляет модель данных для книги.
    /// </summary>
    public class Book
    {
        private string _author;
        private string _title;
        private string _genre;
        private int _year;
        private int _quantity; // Добавлено поле Quantity

        /// <summary>
        /// Уникальный идентификатор книги.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название книги.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название не может быть пустым");
                _title = value;
            }
        }

        /// <summary>
        /// Автор книги.
        /// </summary>
        public string Author
        {
            get => _author;
            set
            {
                if (!IsValidAuthor(value))
                    throw new ArgumentException("Автор должен содержать только буквы и пробелы");
                _author = value;
            }
        }

        /// <summary>
        /// Жанр книги.
        /// </summary>
        public string Genre
        {
            get => _genre;
            set
            {
                if (!IsValidGenre(value))
                    throw new ArgumentException("Неверный жанр. Доступные жанры: драма, фантастика, приключения, роман, повесть, детектив, научная литература");
                _genre = value;
            }
        }

        /// <summary>
        /// Год издания книги.
        /// </summary>
        public int Year
        {
            get => _year;
            set
            {
                if (value < 1000 || value > 2025)
                    throw new ArgumentException("Год должен быть в диапазоне от 1000 до 2025");
                _year = value;
            }
        }

        /// <summary>
        /// Количество книг.
        /// </summary>
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Количество книг не может быть отрицательным");
                _quantity = value;
            }
        }

        private bool IsValidAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                return false;

            return Regex.IsMatch(author, @"^[a-zA-Zа-яА-ЯёЁ\s\-]+$");
        }

        private bool IsValidGenre(string genre)
        {
            string[] validGenres = { "драма", "фантастика", "приключения", "роман", "повесть", "детектив", "научная литература" };
            return Array.Exists(validGenres, g => g.Equals(genre, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Возвращает строковое представление объекта Book.
        /// </summary>
        /// <returns>Строка, содержащая информацию о книге в формате: "Id. Title (Author, Genre, Year, Quantity)".</returns>
        public override string ToString()
        {
            return $"{Id}. {Title} ({Author}, {Genre}, {Year}, Количество: {Quantity})";
        }
    }
}
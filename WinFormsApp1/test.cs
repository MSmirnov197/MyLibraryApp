using logicc;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class test : Form
    {
        private Logic logic;

        public test(Logic logic)
        {
            this.logic = logic ?? throw new ArgumentNullException(nameof(logic));
            InitializeComponent();
            LoadBooks();
            InitializeGenreComboBox();
            InitializeGroupComboBox();
        }

        private void InitializeGenreComboBox()
        {
            cmbGenre.Items.AddRange(logic.GetAvailableGenres());
            cmbGenre.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void InitializeGroupComboBox()
        {
            var groupers = logic.GetAvailableGroupers();
            if (groupers != null && groupers.Any())
            {
                cmbGroupBy.Items.AddRange(groupers.ToArray());
                cmbGroupBy.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbGroupBy.SelectedIndex = 0;
            }
        }

        private void LoadBooks()
        {
            try
            {
                listBoxBooks.DataSource = null;
                listBoxBooks.DisplayMember = "ToString";
                listBoxBooks.DataSource = logic.GetAllBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка при загрузке книг: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text) || !int.TryParse(txtId.Text, out int id) || id <= 0)
                {
                    MessageBox.Show("❌ ID должен быть положительным числом.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var existingBook = logic.ReadBook(id);
                if (existingBook != null)
                {
                    MessageBox.Show($"❌ Книга с ID {id} уже существует.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ValidateInputs();

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("❌ Количество книг должно быть неотрицательным числом.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = logic.CreateBook(id, txtTitle.Text, txtAuthor.Text, cmbGenre.Text,
                    int.Parse(txtYear.Text), quantity);

                if (result.Success)
                {
                    MessageBox.Show("✅ Книга успешно добавлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadBooks();
                }
                else
                {
                    MessageBox.Show($"❌ Ошибка: {result.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedItem == null)
            {
                MessageBox.Show("❌ Выберите книгу для удаления.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var book = (dynamic)listBoxBooks.SelectedItem;
            if (MessageBox.Show($"Вы уверены, что хотите удалить книгу:\n{book.Title}?", "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (logic.DeleteBook(book.Id))
                {
                    MessageBox.Show("🗑️ Книга успешно удалена.", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBooks();
                }
                else
                {
                    MessageBox.Show("❌ Не удалось удалить книгу.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedItem == null)
            {
                MessageBox.Show("❌ Выберите книгу для просмотра.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var book = (dynamic)listBoxBooks.SelectedItem;
            txtId.Text = book.Id.ToString();
            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            cmbGenre.Text = book.Genre;
            txtYear.Text = book.Year.ToString();
            txtQuantity.Text = book.Quantity.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedItem == null)
            {
                MessageBox.Show("❌ Выберите книгу для обновления.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ValidateInputs();

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("❌ Количество книг должно быть неотрицательным числом.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var book = (dynamic)listBoxBooks.SelectedItem;

                if (logic.UpdateBook(book.Id, txtTitle.Text, txtAuthor.Text, cmbGenre.Text,
                    int.Parse(txtYear.Text), quantity))
                {
                    MessageBox.Show("✅ Книга успешно обновлена.", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadBooks();
                }
                else
                {
                    MessageBox.Show("❌ Не удалось обновить книгу.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, List<BookDTO>> grouped;

                if (cmbGroupBy.SelectedItem != null)
                {
                    grouped = logic.GroupBooksBy(cmbGroupBy.SelectedItem.ToString());
                }
                else
                {
                    grouped = logic.GroupBooksByGenre();
                }

                if (grouped.Count == 0)
                {
                    MessageBox.Show("📚 Нет книг для группировки.", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = $"📌 Группировка по {cmbGroupBy.SelectedItem ?? "жанрам"}:\n\n";
                foreach (var g in grouped)
                {
                    result += $"➤ {g.Key}:\n";
                    foreach (var b in g.Value)
                        result += $"   • {b.Title} ({b.Author}, {b.Year}, Количество: {b.Quantity})\n";
                    result += "\n";
                }
                MessageBox.Show(result, "Группировка книг",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка при группировке: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var searchTerm = txtSearch.Text.Trim();
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    LoadBooks();
                    return;
                }

                var results = logic.SearchBooks(searchTerm);
                listBoxBooks.DataSource = null;
                listBoxBooks.DisplayMember = "ToString";
                listBoxBooks.DataSource = results;

                MessageBox.Show($"🔍 Найдено {results.Count} книг по запросу: '{searchTerm}'",
                    "Результаты поиска", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка при поиске: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtId.Clear();
            txtTitle.Clear();
            txtAuthor.Clear();
            cmbGenre.SelectedIndex = -1;
            txtYear.Clear();
            txtQuantity.Clear();
            txtSearch.Clear();
        }

        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
                throw new ArgumentException("Название книги не может быть пустым.");

            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
                throw new ArgumentException("Автор книги не может быть пустым.");

            if (cmbGenre.SelectedItem == null)
                throw new ArgumentException("Жанр книги не выбран.");

            if (string.IsNullOrWhiteSpace(txtYear.Text) || !int.TryParse(txtYear.Text, out int year) || year < 1000 || year > 2025)
                throw new ArgumentException("Год издания должен быть в диапазоне от 1000 до 2025.");

            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
                throw new ArgumentException("Количество книг должно быть неотрицательным числом.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadBooks();
        }
    }
}
using System;
using System.Windows.Forms;
using System.Drawing;

namespace ManagingNotes
{
    public partial class NoteForm : Form
    {
        private NoteManager noteManager;
        private Note editingNote = null; // ← Режим редактирования

        public NoteForm()
        {
            InitializeComponent();
            noteManager = new NoteManager();
            UpdateNotesList();

            // ← Клик по списку
            listBoxNotes.SelectedIndexChanged += (s, e) =>
            {
                if (listBoxNotes.SelectedIndex == -1) return;

                string title = listBoxNotes.SelectedItem.ToString().Split('(')[0].Trim();
                var note = noteManager.Notes.Find(n => n.Title == title);

                if (note != null)
                {
                    textBoxTitle.Text = note.Title;
                    textBoxContent.Text = note.Content;
                    editingNote = note;
                    buttonAdd.Text = "Сохранить"; // ← Меняем текст кнопки
                }
            };
        }

        private void UpdateNotesList()
        {
            listBoxNotes.Items.Clear();
            foreach (var note in noteManager.Notes)
            {
                listBoxNotes.Items.Add($"{note.Title} ({note.Date:yyyy-MM-dd})");
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTitle.Text) || string.IsNullOrEmpty(textBoxContent.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (editingNote != null)
            {
                // ✏️ РЕДАКТИРУЕМ существующую
                editingNote.Title = textBoxTitle.Text;
                editingNote.Content = textBoxContent.Text;
                editingNote.Date = DateTime.Now;
                noteManager.SaveNotes();
            }
            else
            {
                // ➕ СОЗДАЁМ новую
                noteManager.AddNote(new Note(textBoxTitle.Text, textBoxContent.Text));
            }

            // ← Обновляем список и сбрасываем форму
            UpdateNotesList();
            ClearForm();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxNotes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите заметку!");
                return;
            }

            string title = listBoxNotes.SelectedItem.ToString().Split('(')[0].Trim();
            var note = noteManager.Notes.Find(n => n.Title == title);

            if (note != null)
            {
                noteManager.RemoveNote(note);
                UpdateNotesList();

                // Если удалили ту, что редактировали — сбрасываем форму
                if (editingNote == note)
                {
                    ClearForm();
                }
            }
        }

        // ← Очистка формы + сброс кнопки
        private void ClearForm()
        {
            textBoxTitle.Clear();
            textBoxContent.Clear();
            editingNote = null;
            listBoxNotes.ClearSelected();
            buttonAdd.Text = "Добавить"; // ← Возвращаем текст кнопки
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NoteForm());
        }
    }
}
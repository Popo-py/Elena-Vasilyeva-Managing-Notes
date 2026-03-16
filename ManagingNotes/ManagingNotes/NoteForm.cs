using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace ManagingNotes
{
    public partial class NoteForm : Form
    {
        private NoteManager noteManager;

        public NoteForm()
        {
            InitializeComponent(); // ← Вызывает Designer!
            noteManager = new NoteManager();
            UpdateNotesList();
        }

        private void UpdateNotesList()
        {
            listBoxNotes.Items.Clear();
            foreach (var note in noteManager.Notes)
            {
                listBoxNotes.Items.Add($"{note.Title} ({note.Date:yyyy-MM-dd})");
            }
        }

        // ← Используем имена ИЗ InitializeComponent()
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTitle.Text) || string.IsNullOrEmpty(textBoxContent.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Note newNote = new Note(textBoxTitle.Text, textBoxContent.Text);
            noteManager.AddNote(newNote);
            textBoxTitle.Clear();
            textBoxContent.Clear();
            UpdateNotesList();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxNotes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите заметку!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedItem = listBoxNotes.SelectedItem.ToString();
            string title = selectedItem.Split('(')[0].Trim();

            var noteToRemove = noteManager.Notes.Find(n => n.Title == title);
            if (noteToRemove != null)
            {
                noteManager.RemoveNote(noteToRemove);
                UpdateNotesList();
            }
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
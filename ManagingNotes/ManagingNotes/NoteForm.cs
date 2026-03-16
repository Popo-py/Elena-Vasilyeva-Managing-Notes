using System;
using System.Windows.Forms;
using System.Drawing;

namespace ManagingNotes
{
    public partial class NoteForm : Form
    {
        private NoteManager noteManager;

        public NoteForm()
        {
            InitializeComponent();
            noteManager = new NoteManager();
            UpdateNotesList();
        }

        private void UpdateNotesList()
        {
            listBoxNotes.Items.Clear();
            foreach (var note in noteManager.Notes)
            {
                listBoxNotes.Items.Add($"{note.Title} ({note.Date.ToString("yyyy-MM-dd")})");
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTitle.Text) || string.IsNullOrEmpty(textBoxContent.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            Note newNote = new Note(textBoxTitle.Text, textBoxContent.Text);
            try
            {
                noteManager.AddNote(newNote);
                textBoxTitle.Clear();
                textBoxContent.Clear();
                UpdateNotesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxNotes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите заметку для удаления!");
                return;
            }
            string selectedItem = listBoxNotes.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '(' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string title = parts[0].Trim();
                DateTime date;
                if (DateTime.TryParse(parts[1].Split(')')[0], out date))
                {
                    var noteToRemove = noteManager.Notes.Find(n =>
                        n.Title == title && n.Date.Date == date.Date);
                    if (noteToRemove != null)
                    {
                        try
                        {
                            noteManager.RemoveNote(noteToRemove);
                            UpdateNotesList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
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
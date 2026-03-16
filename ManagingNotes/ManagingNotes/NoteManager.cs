using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ManagingNotes
{
    public class NoteManager
    {
        public List<Note> Notes { get; private set; }
        private string NotesFilePath => Path.Combine(Application.StartupPath, "notes.txt");

        public NoteManager()
        {
            Notes = new List<Note>();
            LoadNotes();
        }

        public void AddNote(Note note)
        {
            Notes.Add(note);
            SaveNotes();
        }

        public void RemoveNote(Note note)
        {
            Notes.Remove(note);
            SaveNotes();
        }

        public void SaveNotes()
        {
            var lines = Notes.Select(n => $"{n.Title}|{n.Content}|{n.Date:yyyy-MM-dd HH:mm:ss}");
            File.WriteAllLines(NotesFilePath, lines);
        }

        private void LoadNotes()
        {
            if (!File.Exists(NotesFilePath)) return;
            foreach (var line in File.ReadAllLines(NotesFilePath))
            {
                var parts = line.Split('|');
                if (parts.Length >= 2)
                {
                    DateTime date = DateTime.Now;
                    if (parts.Length >= 3) DateTime.TryParse(parts[2], out date);
                    Notes.Add(new Note(parts[0], parts[1], date));
                }
            }
        }
    }
}
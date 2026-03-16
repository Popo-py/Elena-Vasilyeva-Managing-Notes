using System;

public class Note
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }

    // Конструктор для новых заметок (дата = сейчас)
    public Note(string title, string content)
    {
        Title = title;
        Content = content;
        Date = DateTime.Now;
    }

    // Конструктор для загрузки из файла (с указанной датой)
    public Note(string title, string content, DateTime date)
    {
        Title = title;
        Content = content;
        Date = date;
    }
}
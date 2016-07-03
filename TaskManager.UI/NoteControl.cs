using System;
using System.Configuration;
using System.Windows.Forms;
using TaskManager.Domain.Features.EraseNote;
using TaskManager.Domain.Features.SaveNote;
using TaskManager.Domain.Infrastructure;
using TaskManager.NoteEditorUI;
using Timer = System.Windows.Forms.Timer;

namespace TaskManager
{
    public partial class NoteControl : UserControl
    {
        private string _taskId;
        private int _interval;
        Timer _timer;
        private string _content;
        private CommandDispatcher _commandDispatcher;
        public event EventHandler<NoteSavedEventArgs> NoteSaved;
        public event EventHandler<NoteErasedEventArgs> NoteErased;

        internal NoteControl()
        {
            InitializeComponent();
        }

        public void Initialize(CommandDispatcher commandDispatcher)
        {
            if (commandDispatcher == null) throw new ArgumentNullException("commandDispatcher");
            _commandDispatcher = commandDispatcher;
            if (!int.TryParse(ConfigurationManager.AppSettings["notes.saveinterval.milliseconds"], out _interval))
            {
                _interval = 1000;
            }
        }
        
        public void RenderNote(string taskId, string content)
        {
            _taskId = taskId;
            _content = content;
            noteRichTextBox.Text = _content ?? "";
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            var noteContent = _timer.Tag.ToString();
            SaveNote(noteContent);
        }

        private void noteRichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                _timer = new Timer();
                _timer.Interval = _interval;
                _timer.Tick += _timer_Tick;
            }

            _timer.Stop();
            _timer.Tag = noteRichTextBox.Text;
            _timer.Start();
        }

        private void SaveNote(string newContent)
        {
            if (_content == newContent) return;

            if (newContent == "")
            {
                var eraseNote = new EraseNote(_taskId);
                _commandDispatcher.Send(eraseNote);
                _content = newContent;
                if (NoteErased != null)
                {
                    var eventArgs = new NoteErasedEventArgs(_taskId);
                    NoteErased(this, eventArgs);
                }
                return;
            }

            var saveNote = new SaveNote(_taskId, newContent);
            _commandDispatcher.Send(saveNote);
            _content = newContent;
            if (NoteSaved != null)
            {
                var eventArgs = new NoteSavedEventArgs(newContent, _taskId);
                NoteSaved(this, eventArgs);
            }
        }

        private void noteRichTextBox_Leave(object sender, EventArgs e)
        {
            _timer.Dispose();
            SaveNote(noteRichTextBox.Text);
        }

        public void Clear()
        {
            _content = "";
            noteRichTextBox.Text = "";
        }
    }
}

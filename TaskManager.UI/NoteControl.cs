using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using MediatR;
using TaskManager.Domain.Features.EraseNote;
using TaskManager.Domain.Features.SaveNote;
using TaskManager.NoteEditorUI;
using Timer = System.Windows.Forms.Timer;

namespace TaskManager
{
    public partial class NoteControl : UserControl
    {
        private IMediator _mediator;
        private string _taskId;
        private int _interval;
        Timer _timer;
        private string _initialContent;
        public event EventHandler<NoteSavedEventArgs> NoteSaved;
        public event EventHandler<NoteErasedEventArgs> NoteErased;


        internal NoteControl()
        {
            InitializeComponent();
        }

        public void Initialize(IMediator mediator)
        {
            _mediator = mediator;
            if (!int.TryParse(ConfigurationManager.AppSettings["notes.saveinterval.milliseconds"], out _interval))
            {
                _interval = 2000;
            }
        }
        
        public void RenderNote(string taskId, string initialContent)
        {
            _taskId = taskId;
            _initialContent = initialContent;
            noteRichTextBox.Text = _initialContent ?? "";
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

        private void SaveNote(string noteContent)
        {
            if (_initialContent == noteContent) return;

            if (noteContent == "")
            {
                var eraseNote = new EraseNote(_taskId);
                _mediator.Send(eraseNote);
                if (NoteErased != null)
                {
                    var eventArgs = new NoteErasedEventArgs(_taskId);
                    NoteErased(this, eventArgs);
                }
                return;
            }

            var saveNote = new SaveNote(_taskId, noteContent);
            _mediator.Send(saveNote);
            if (NoteSaved != null)
            {
                var eventArgs = new NoteSavedEventArgs(noteContent, _taskId);
                NoteSaved(this, eventArgs);
            }
        }

        private void noteRichTextBox_Leave(object sender, EventArgs e)
        {
            _timer.Dispose();
            SaveNote(noteRichTextBox.Text);
        }
    }
}

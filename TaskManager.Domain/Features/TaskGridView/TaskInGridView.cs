
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace TaskManager.Domain.Features.TaskGridView
{
    public class TaskInGridView : INotifyPropertyChanged
    {
        private string _id;
        private string _projectId;
        private string _deadline;
        private string _title;
        private bool _isDone;
        private string _priority;
        private string _note;

        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string ProjectId
        {
            get { return _projectId; }
            set
            {
                if (_projectId != value)
                {
                    _projectId = value;
                    OnPropertyChanged("ProjectId");
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        public string Deadline
        {
            get { return _deadline; }
            set
            {
                if (_deadline != value)
                {
                    _deadline = value;
                    OnPropertyChanged("Deadline");
                }
            }
        }

        public string Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged("Priority");
                }
            }
        }

        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                if (_isDone != value)
                {
                    _isDone = value;
                    OnPropertyChanged("IsDone");
                }
            }
        }

        public string Note
        {
            get { return _note; }
            set
            {
                if (_note != value)
                {
                    _note = value;
                    OnPropertyChanged("Note");
                }
            }
        }

        public TaskInGridView(string taskId, string projectId, string title, string deadline, string priority, bool isDone, string note = "")
        {
            _id = taskId;
            _projectId = projectId;
            _title = title;
            _deadline = deadline;
            _priority = priority;
            _isDone = isDone;
            _note = note;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Equals(TaskInGridView other)
        {
            return string.Equals(_id, other._id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TaskInGridView) obj);
        }

        public override int GetHashCode()
        {
            return (_id != null ? _id.GetHashCode() : 0);
        }
    }
}
using System.Windows.Forms;
using TaskManager.Domain.Common;
using TaskManager.Domain.Infrastructure;

namespace TaskManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var eventStoreConnectionBuilder = new EventStoreConnectionBuilder();
            var eventBus = new EventBus((@event, next) =>
                new ExceptionDecorator<Event>(next).Handle(@event));
            var commandDispatcher = new CommandDispatcher(eventStoreConnectionBuilder, eventBus,
                (command, next) => new ExceptionDecorator<Command>(next).Handle(command));

            projectTreeControl.Initialize(commandDispatcher);
            noteControl.Initialize(commandDispatcher, taskGridControl);
            taskGridControl.Initialize(commandDispatcher, projectTreeControl, noteControl);
        }
        
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            RavenDbStore.CleanUp();
        }
    }
}

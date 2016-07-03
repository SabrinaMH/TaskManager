using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TaskManager.TasksInGridViewUI
{
    public class GridUtils
    {
        private readonly DataGridView _grid;

        public GridUtils(DataGridView grid)
        {
            _grid = grid;
        }

        public void RemoveColumn(string columnName)
        {
            if (_grid.Columns.Contains(columnName))
            {
                _grid.Columns.Remove(columnName);
            }
        }

        public int IndexOfDataGridRowByColumnValue(string columnName, string columnValue)
        {
            DataGridViewRow row = _grid.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.Cells[columnName].Value.ToString().Equals(columnValue))
                .First();

            return row.Index;
        }

        public void SetHasNoteCheckBox(string noteContent, string taskTitle)
        {
            var indexOfTask = IndexOfDataGridRowByColumnValue("Title", taskTitle);
            var row = _grid.Rows[indexOfTask];

            if (string.IsNullOrWhiteSpace(noteContent))
            {
                row.Cells["HasNote"].Value = false;
            }
            else
            {
                row.Cells["HasNote"].Value = true;
            }
        }

        public void FadeOut(int rowIndex)
        {
            var selectedRow = _grid.Rows[rowIndex];
            var numberOfColumns = _grid.Columns.Count;
            for (int i = 0; i < numberOfColumns; i++)
            {
                selectedRow.Cells[i].Style.ForeColor = Color.DarkGray;
            }
        }

        public void FadeIn(int rowIndex)
        {
            for (int i = 0; i < _grid.Columns.Count; i++)
            {
                _grid.Rows[rowIndex].Cells[i].Style.ForeColor = Color.Black;
            }
        }


        public bool TouchesColumn(string columnName, int columnIndex, int rowIndex)
        {
            return columnIndex == _grid.Columns[columnName].Index && rowIndex != -1;
        }
    }
}
using Comparatist.View.Infrastructure;

namespace Comparatist.View.WordGrid
{
    internal class WordGridInputHandler : InputHandler<DataGridView>
    {
        public WordGridInputHandler(DataGridView control) : base(control)
        {
            Control.Dock = DockStyle.Fill;
            Control.AllowUserToAddRows = false;
            Control.RowHeadersVisible = false;
            Control.AutoGenerateColumns = false;
            Control.MultiSelect = false;
            Control.ReadOnly = true;
            Control.Visible = false;
        }

        public override void Dispose()
        {
            
        }
    }
}

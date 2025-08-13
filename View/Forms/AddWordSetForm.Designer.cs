using System.ComponentModel;

namespace Comparatist.View.Forms
{
    partial class AddWordSetForm
    {
        private DataGridView _grid;
        private Button _okButton;
        private Button _cancelButton;

        private void InitializeComponent()
        {
            _grid = new DataGridView();
            _okButton = new Button();
            _cancelButton = new Button();

            ((ISupportInitialize)(_grid)).BeginInit();
            SuspendLayout();

            _grid.AllowUserToAddRows = true;
            _grid.AllowUserToDeleteRows = true;
            _grid.Dock = DockStyle.Fill;
            _grid.Location = new Point(0, 0);
            _grid.RowHeadersVisible = false;
            _grid.AllowUserToAddRows = false;
            _grid.Size = new Size(400, 300);

            _grid.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Language",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 50,
                },

                new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Value",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 50,
                }
            });

            _okButton.Dock = DockStyle.Bottom;
            _okButton.Text = "OK";

            _cancelButton.Dock = DockStyle.Bottom;
            _cancelButton.Text = "Cancel";

            ClientSize = new Size(400, 340);

            Controls.Add(_okButton);
            Controls.Add(_cancelButton);
            Controls.Add(_grid);

            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Edit AutoReplace List";

            ((ISupportInitialize)(_grid)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

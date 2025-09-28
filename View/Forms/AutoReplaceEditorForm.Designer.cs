using Comparatist.View.Fonts;
using System.ComponentModel;

namespace Comparatist.View.Forms
{
    internal partial class AutoReplaceEditorForm
    {
        private DataGridView _grid;
        private Button _okButton;

        private void InitializeComponent()
        {
            _grid = new DataGridView();
            _okButton = new Button();

            ((ISupportInitialize)(_grid)).BeginInit();
            SuspendLayout();

            _grid.AllowUserToAddRows = true;
            _grid.AllowUserToDeleteRows = true;
            _grid.Dock = DockStyle.Fill;
            _grid.Location = new Point(0, 0);
            _grid.RowHeadersVisible = false;
            _grid.Size = new Size(400, 300);
			_grid.Font = FontManager.Instance.Font;

			_grid.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Shortcut",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 50,
                },

                new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Replacement",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 50,
                }
            });

            _okButton.Dock = DockStyle.Bottom;
            _okButton.Location = new Point(0, 310);
            _okButton.Size = new Size(400, 30);
            _okButton.Text = "OK";
            _okButton.Click += OnEndEdit;

            ClientSize = new Size(400, 340);
            Controls.Add(_grid);
            Controls.Add(_okButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Edit autoreplace list";

            ((ISupportInitialize)(_grid)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

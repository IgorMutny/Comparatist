using Comparatist.Data.Persistence;

namespace Comparatist
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _mainMenuStrip = new MenuStrip();
            _languageGridView = new DataGridView();
            _categoryTreeView = new TreeView();
            _wordGridView = new DataGridView();
            SuspendLayout();
 
            _mainMenuStrip.Location = new Point(0, 0);
            _mainMenuStrip.Size = new Size(800, 24);

            _languageGridView.Location = new Point(0, 24);
            _languageGridView.Size = new Size(800, 426);
            _languageGridView.Visible = false;

            _categoryTreeView.Location = new Point(0, 24);
            _categoryTreeView.Size = new Size(800, 426);
            
            _wordGridView.Location = new Point(0, 24);
            _wordGridView.Size = new Size(800, 426);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(_wordGridView);
            Controls.Add(_categoryTreeView);
            Controls.Add(_languageGridView);
            Controls.Add(_mainMenuStrip);
            MainMenuStrip = _mainMenuStrip;
            Name = "MainForm";
            Text = "Comparatist";
            ResumeLayout(false);
            PerformLayout();
        }

        private MenuStrip _mainMenuStrip;
        private DataGridView _languageGridView;
        private TreeView _categoryTreeView;
        private DataGridView _wordGridView;
    }
}

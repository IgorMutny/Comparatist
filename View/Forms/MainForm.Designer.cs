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

            _mainMenuStrip.Dock = DockStyle.Top;
            _languageGridView.Dock = DockStyle.Fill;
            _categoryTreeView.Dock = DockStyle.Fill;
            _wordGridView.Dock = DockStyle.Fill;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);

            var contentPanel = new Panel { Dock = DockStyle.Fill };
            contentPanel.Controls.Add(_languageGridView);
            contentPanel.Controls.Add(_categoryTreeView);
            contentPanel.Controls.Add(_wordGridView);
            Controls.Add(contentPanel);
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

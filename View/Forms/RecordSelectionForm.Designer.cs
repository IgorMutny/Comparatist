using Comparatist.Data.Entities;
using Comparatist.View.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparatist
{
    partial class RecordSelectionForm<T> where T: IRecord
    {
        private CheckedListBox _checkedListBox;
        private Button _okButton;
        private Button _cancelButton;

        private void InitializeComponent()
        {
			_checkedListBox = new CheckedListBox();
            _okButton = new Button();
            _cancelButton = new Button();

            // CheckedListBox
            _checkedListBox.Dock = DockStyle.Top;
            _checkedListBox.Height = 300;
			_checkedListBox.Font = FontManager.Instance.Font;

			// OK Button
			_okButton.Text = "OK";
            _okButton.DialogResult = DialogResult.OK;
            _okButton.Dock = DockStyle.Bottom;

            // Cancel Button
            _cancelButton.Text = "Cancel";
            _cancelButton.DialogResult = DialogResult.Cancel;
            _cancelButton.Dock = DockStyle.Bottom;

            // Form
            Controls.Add(_checkedListBox);
            Controls.Add(_okButton);
            Controls.Add(_cancelButton);
            Text = string.Empty;
            ClientSize = new Size(300, 350);
        }
    }
}

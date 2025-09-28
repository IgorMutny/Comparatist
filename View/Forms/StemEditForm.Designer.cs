using Comparatist.View.Fonts;
using System.Drawing;
using System.Windows.Forms;

namespace Comparatist
{
    public partial class StemEditForm
    {
        private Label _lblValue;
        private Label _lblTranslation;
        private Label _lblComment;
        private Label _lblRoots;

        private TextBox _valueTextBox;
        private TextBox _translationTextBox;
        private TextBox _commentTextBox;
        private TextBox _rootsTextBox;

        private CheckBox _checkedBox;

        private Button _rootSelectionButton;
        private Button _okButton;
        private Button _cancelButton;

        private void InitializeComponent()
        {
            _lblValue = new Label();
            _lblTranslation = new Label();
            _lblComment = new Label();
            _lblRoots = new Label();
            _valueTextBox = new TextBox();
            _translationTextBox = new TextBox();
            _commentTextBox = new TextBox();
            _rootsTextBox = new TextBox();
            _rootSelectionButton = new Button();
            _checkedBox = new CheckBox();
            _okButton = new Button();
            _cancelButton = new Button();
            SuspendLayout();
            // 
            // _lblValue
            // 
            _lblValue.Location = new Point(10, 15);
            _lblValue.Name = "_lblValue";
            _lblValue.Size = new Size(80, 15);
            _lblValue.TabIndex = 0;
            _lblValue.Text = "Value:";
            // 
            // _lblTranslation
            // 
            _lblTranslation.Location = new Point(10, 45);
            _lblTranslation.Name = "_lblTranslation";
            _lblTranslation.Size = new Size(80, 15);
            _lblTranslation.TabIndex = 2;
            _lblTranslation.Text = "Translation:";
            // 
            // _lblComment
            // 
            _lblComment.Location = new Point(10, 75);
            _lblComment.Name = "_lblComment";
            _lblComment.Size = new Size(80, 15);
            _lblComment.TabIndex = 4;
            _lblComment.Text = "Comment:";
            // 
            // _lblRoots
            // 
            _lblRoots.Location = new Point(10, 105);
            _lblRoots.Name = "_lblRoots";
            _lblRoots.Size = new Size(80, 15);
            _lblRoots.TabIndex = 6;
            _lblRoots.Text = "Roots:";
            // 
            // _valueTextBox
            // 
            _valueTextBox.Location = new Point(100, 12);
            _valueTextBox.Name = "_valueTextBox";
            _valueTextBox.Size = new Size(180, 23);
            _valueTextBox.TabIndex = 1;
			_valueTextBox.Font = FontManager.Instance.Font;
			// 
			// _translationTextBox
			// 
			_translationTextBox.Location = new Point(100, 42);
            _translationTextBox.Name = "_translationTextBox";
            _translationTextBox.Size = new Size(180, 23);
            _translationTextBox.TabIndex = 3;
			_translationTextBox.Font = FontManager.Instance.Font;
			// 
			// _commentTextBox
			// 
			_commentTextBox.Location = new Point(100, 72);
            _commentTextBox.Name = "_commentTextBox";
            _commentTextBox.Size = new Size(180, 23);
            _commentTextBox.TabIndex = 5;
			_commentTextBox.Font = FontManager.Instance.Font;
			// 
			// _rootsTextBox
			// 
			_rootsTextBox.Location = new Point(100, 102);
            _rootsTextBox.Name = "_rootsTextBox";
            _rootsTextBox.Size = new Size(150, 23);
            _rootsTextBox.TabIndex = 7;
            _rootsTextBox.ReadOnly = true;
			_rootsTextBox.Font = FontManager.Instance.Font;
			// 
			// _rootSelectionButton
			// 
			_rootSelectionButton.Location = new Point(255, 102);
            _rootSelectionButton.Name = "_rootSelectionButton";
            _rootSelectionButton.Size = new Size(25, 25);
            _rootSelectionButton.TabIndex = 11;
            _rootSelectionButton.Text = "...";
            // 
            // _checkedBox
            // 
            _checkedBox.Location = new Point(100, 162);
            _checkedBox.Name = "_checkedBox";
            _checkedBox.Size = new Size(100, 20);
            _checkedBox.TabIndex = 10;
            _checkedBox.Text = "Checked";
            // 
            // _okButton
            // 
            _okButton.DialogResult = DialogResult.OK;
            _okButton.Location = new Point(50, 202);
            _okButton.Name = "_okButton";
            _okButton.Size = new Size(80, 25);
            _okButton.TabIndex = 11;
            _okButton.Text = "OK";
            // 
            // _cancelButton
            // 
            _cancelButton.DialogResult = DialogResult.Cancel;
            _cancelButton.Location = new Point(150, 202);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new Size(80, 25);
            _cancelButton.TabIndex = 12;
            _cancelButton.Text = "Cancel";
            // 
            // StemEditForm
            // 
            AcceptButton = _okButton;
            CancelButton = _cancelButton;
            ClientSize = new Size(300, 240);
            Controls.Add(_lblValue);
            Controls.Add(_valueTextBox);
            Controls.Add(_lblTranslation);
            Controls.Add(_translationTextBox);
            Controls.Add(_lblComment);
            Controls.Add(_commentTextBox);
            Controls.Add(_lblRoots);
            Controls.Add(_rootsTextBox);
            Controls.Add(_rootSelectionButton);
            Controls.Add(_checkedBox);
            Controls.Add(_okButton);
            Controls.Add(_cancelButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StemEditForm";
            Text = "Stem Edit";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
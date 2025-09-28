using System.Drawing;
using System.Windows.Forms;

namespace Comparatist
{
    public partial class RootEditForm
    {
        private Label _lblValue;
        private Label _lblTranslation;
        private Label _lblComment;
        private Label _lblSemanticGroups;

        private TextBox _valueTextBox;
        private TextBox _translationTextBox;
        private TextBox _commentTextBox;
        private TextBox _groupsTextBox;

        private CheckBox _checkedBox;

        private Button _groupSelectionButton;
        private Button _okButton;
        private Button _cancelButton;

        private void InitializeComponent()
        {
            _lblValue = new Label();
            _lblTranslation = new Label();
            _lblComment = new Label();
            _lblSemanticGroups = new Label();
            _valueTextBox = new TextBox();
            _translationTextBox = new TextBox();
            _commentTextBox = new TextBox();
            _groupsTextBox = new TextBox();
            _groupSelectionButton = new Button();
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
            _lblSemanticGroups.Location = new Point(10, 105);
            _lblSemanticGroups.Name = "_lblSemanticGroups";
            _lblSemanticGroups.Size = new Size(80, 15);
            _lblSemanticGroups.TabIndex = 6;
            _lblSemanticGroups.Text = "Groups:";
            // 
            // _valueTextBox
            // 
            _valueTextBox.Location = new Point(100, 12);
            _valueTextBox.Name = "_valueTextBox";
            _valueTextBox.Size = new Size(180, 23);
            _valueTextBox.TabIndex = 1;
            // 
            // _translationTextBox
            // 
            _translationTextBox.Location = new Point(100, 42);
            _translationTextBox.Name = "_translationTextBox";
            _translationTextBox.Size = new Size(180, 23);
            _translationTextBox.TabIndex = 3;
            // 
            // _commentTextBox
            // 
            _commentTextBox.Location = new Point(100, 72);
            _commentTextBox.Name = "_commentTextBox";
            _commentTextBox.Size = new Size(180, 23);
            _commentTextBox.TabIndex = 5;
            // 
            // _rootsTextBox
            // 
            _groupsTextBox.Location = new Point(100, 102);
            _groupsTextBox.Name = "_groupsTextBox";
            _groupsTextBox.Size = new Size(150, 23);
            _groupsTextBox.TabIndex = 7;
            _groupsTextBox.ReadOnly = true;
            // 
            // _rootSelectionButton
            // 
            _groupSelectionButton.Location = new Point(255, 102);
            _groupSelectionButton.Name = "_groupSelectionButton";
            _groupSelectionButton.Size = new Size(25, 25);
            _groupSelectionButton.TabIndex = 11;
            _groupSelectionButton.Text = "...";
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
            Controls.Add(_lblSemanticGroups);
            Controls.Add(_groupsTextBox);
            Controls.Add(_groupSelectionButton);
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
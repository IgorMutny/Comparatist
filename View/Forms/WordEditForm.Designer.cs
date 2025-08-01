using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparatist
{
    partial class WordEditForm
    {
        private Label _lblValue;
        private Label _lblTranslation;
        private Label _lblComment;
        private Label _lblStem;
        private Label _lblLanguage;

        private TextBox _valueTextBox;
        private TextBox _translationTextBox;
        private TextBox _commentTextBox;
        private TextBox _stemTextBox;
        private TextBox _languageTextBox;

        private CheckBox _nativeBox;
        private CheckBox _checkedBox;

        private Button _okButton;
        private Button _cancelButton;

        private void InitializeComponent()
        {
            _lblValue = new Label();
            _lblTranslation = new Label();
            _lblComment = new Label();
            _lblStem = new Label();
            _lblLanguage = new Label();
            _valueTextBox = new TextBox();
            _translationTextBox = new TextBox();
            _commentTextBox = new TextBox();
            _stemTextBox = new TextBox();
            _languageTextBox = new TextBox();
            _nativeBox = new CheckBox();
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
            // _lblStem
            // 
            _lblStem.Location = new Point(10, 105);
            _lblStem.Name = "_lblStem";
            _lblStem.Size = new Size(80, 15);
            _lblStem.TabIndex = 6;
            _lblStem.Text = "Stem:";
            // 
            // _lblLanguage
            // 
            _lblLanguage.Location = new Point(10, 135);
            _lblLanguage.Name = "_lblLanguage";
            _lblLanguage.Size = new Size(80, 15);
            _lblLanguage.TabIndex = 6;
            _lblLanguage.Text = "Language:";
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
            // _stemTextBox
            // 
            _stemTextBox.Location = new Point(100, 102);
            _stemTextBox.Name = "_stemTextBox";
            _stemTextBox.ReadOnly = true;
            _stemTextBox.Size = new Size(180, 23);
            _stemTextBox.TabIndex = 7;
            // 
            // _languageTextBox
            // 
            _languageTextBox.Location = new Point(100, 133);
            _languageTextBox.Name = "_languageTextBox";
            _languageTextBox.ReadOnly = true;
            _languageTextBox.Size = new Size(180, 23);
            _languageTextBox.TabIndex = 8;
            // 
            // _nativeBox
            // 
            _nativeBox.AutoSize = true;
            _nativeBox.Location = new Point(100, 162);
            _nativeBox.Name = "_nativeBox";
            _nativeBox.Size = new Size(60, 19);
            _nativeBox.TabIndex = 6;
            _nativeBox.Text = "Native";
            _nativeBox.UseVisualStyleBackColor = true;
            // 
            // _checkedBox
            // 
            _checkedBox.Location = new Point(100, 182);
            _checkedBox.Name = "_checkedBox";
            _checkedBox.Size = new Size(100, 20);
            _checkedBox.TabIndex = 10;
            _checkedBox.Text = "Checked";
            // 
            // _okButton
            // 
            _okButton.DialogResult = DialogResult.OK;
            _okButton.Location = new Point(50, 222);
            _okButton.Name = "_okButton";
            _okButton.Size = new Size(80, 25);
            _okButton.TabIndex = 11;
            _okButton.Text = "OK";
            // 
            // _cancelButton
            // 
            _cancelButton.DialogResult = DialogResult.Cancel;
            _cancelButton.Location = new Point(150, 222);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new Size(80, 25);
            _cancelButton.TabIndex = 12;
            _cancelButton.Text = "Cancel";
            // 
            // WordEditForm
            // 
            AcceptButton = _okButton;
            CancelButton = _cancelButton;
            ClientSize = new Size(300, 260);
            Controls.Add(_lblValue);
            Controls.Add(_valueTextBox);
            Controls.Add(_lblTranslation);
            Controls.Add(_translationTextBox);
            Controls.Add(_lblComment);
            Controls.Add(_commentTextBox);
            Controls.Add(_lblStem);
            Controls.Add(_stemTextBox);
            Controls.Add(_lblLanguage);
            Controls.Add(_languageTextBox);
            Controls.Add(_nativeBox);
            Controls.Add(_checkedBox);
            Controls.Add(_okButton);
            Controls.Add(_cancelButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WordEditForm";
            Text = "Word Edit";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}

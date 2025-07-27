using System.Windows.Forms;

namespace Comparatist
{
    partial class RootEditForm
    {
        private Label _lblValue;
        private Label _lblTranslation;
        private Label _lblComment;

        private TextBox _valueTextBox;
        private TextBox _translationTextBox;
        private TextBox _commentTextBox;

        private CheckBox _checkedBox;
        private Button _okButton;
        private Button _cancelButton;

        private void InitializeComponent()
        {
            _lblValue = new Label();
            _lblTranslation = new Label();
            _lblComment = new Label();
            _valueTextBox = new TextBox();
            _translationTextBox = new TextBox();
            _commentTextBox = new TextBox();
            _checkedBox = new CheckBox();
            _okButton = new Button();
            _cancelButton = new Button();
            SuspendLayout();
            // 
            // _lblValue
            // 
            _lblValue.AutoSize = true;
            _lblValue.Location = new Point(10, 20);
            _lblValue.Name = "_lblValue";
            _lblValue.Size = new Size(38, 15);
            _lblValue.TabIndex = 0;
            _lblValue.Text = "Value:";
            // 
            // _lblTranslation
            // 
            _lblTranslation.AutoSize = true;
            _lblTranslation.Location = new Point(10, 50);
            _lblTranslation.Name = "_lblTranslation";
            _lblTranslation.Size = new Size(68, 15);
            _lblTranslation.TabIndex = 2;
            _lblTranslation.Text = "Translation:";
            // 
            // _lblComment
            // 
            _lblComment.AutoSize = true;
            _lblComment.Location = new Point(10, 80);
            _lblComment.Name = "_lblComment";
            _lblComment.Size = new Size(64, 15);
            _lblComment.TabIndex = 4;
            _lblComment.Text = "Comment:";
            // 
            // _valueTextBox
            // 
            _valueTextBox.Location = new Point(100, 18);
            _valueTextBox.Name = "_valueTextBox";
            _valueTextBox.Size = new Size(150, 23);
            _valueTextBox.TabIndex = 1;
            // 
            // _translationTextBox
            // 
            _translationTextBox.Location = new Point(100, 48);
            _translationTextBox.Name = "_translationTextBox";
            _translationTextBox.Size = new Size(150, 23);
            _translationTextBox.TabIndex = 3;
            // 
            // _commentTextBox
            // 
            _commentTextBox.Location = new Point(100, 78);
            _commentTextBox.Name = "_commentTextBox";
            _commentTextBox.Size = new Size(150, 23);
            _commentTextBox.TabIndex = 5;
            // 
            // _checkedBox
            // 
            _checkedBox.AutoSize = true;
            _checkedBox.Location = new Point(100, 110);
            _checkedBox.Name = "_checkedBox";
            _checkedBox.Size = new Size(72, 19);
            _checkedBox.TabIndex = 6;
            _checkedBox.Text = "Checked";
            _checkedBox.UseVisualStyleBackColor = true;
            // 
            // _okButton
            // 
            _okButton.DialogResult = DialogResult.OK;
            _okButton.Location = new Point(50, 150);
            _okButton.Name = "_okButton";
            _okButton.Size = new Size(80, 25);
            _okButton.TabIndex = 7;
            _okButton.Text = "OK";
            _okButton.UseVisualStyleBackColor = true;
            // 
            // _cancelButton
            // 
            _cancelButton.DialogResult = DialogResult.Cancel;
            _cancelButton.Location = new Point(150, 150);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new Size(80, 25);
            _cancelButton.TabIndex = 8;
            _cancelButton.Text = "Cancel";
            _cancelButton.UseVisualStyleBackColor = true;
            // 
            // RootEditForm
            // 
            AcceptButton = _okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = _cancelButton;
            ClientSize = new Size(280, 200);
            Controls.Add(_lblValue);
            Controls.Add(_valueTextBox);
            Controls.Add(_lblTranslation);
            Controls.Add(_translationTextBox);
            Controls.Add(_lblComment);
            Controls.Add(_commentTextBox);
            Controls.Add(_checkedBox);
            Controls.Add(_okButton);
            Controls.Add(_cancelButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RootEditForm";
            Text = "Root Edit";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
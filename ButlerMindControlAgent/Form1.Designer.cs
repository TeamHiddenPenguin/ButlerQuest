namespace ButlerMindControlAgent
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Title = new System.Windows.Forms.Label();
            this.LevelSelectLabel = new System.Windows.Forms.Label();
            this.FileLocBox = new System.Windows.Forms.TextBox();
            this.LoadButton = new System.Windows.Forms.Button();
            this.EnemySelectBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CommandList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StartXYZLabel = new System.Windows.Forms.Label();
            this.StartXPosBox = new System.Windows.Forms.TextBox();
            this.StartYPosBox = new System.Windows.Forms.TextBox();
            this.StartPosBox = new System.Windows.Forms.GroupBox();
            this.StartPosChange = new System.Windows.Forms.Button();
            this.MoveCommandBox = new System.Windows.Forms.GroupBox();
            this.MoveAddButton = new System.Windows.Forms.Button();
            this.MoveZPosBox = new System.Windows.Forms.TextBox();
            this.MoveXYZLabel = new System.Windows.Forms.Label();
            this.MoveYPosBox = new System.Windows.Forms.TextBox();
            this.MoveXPosBox = new System.Windows.Forms.TextBox();
            this.WaitCommandBox = new System.Windows.Forms.GroupBox();
            this.WaitCommandTextBox = new System.Windows.Forms.TextBox();
            this.WaitAddButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.FloorSelectLabel = new System.Windows.Forms.Label();
            this.FloorSelectBox = new System.Windows.Forms.ListBox();
            this.RemoveCommand = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.FixCommands = new System.Windows.Forms.Button();
            this.StartPosBox.SuspendLayout();
            this.MoveCommandBox.SuspendLayout();
            this.WaitCommandBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(13, 13);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(151, 13);
            this.Title.TabIndex = 0;
            this.Title.Text = "Butler Mind Control Agent v1.0";
            // 
            // LevelSelectLabel
            // 
            this.LevelSelectLabel.AutoSize = true;
            this.LevelSelectLabel.Location = new System.Drawing.Point(13, 41);
            this.LevelSelectLabel.Name = "LevelSelectLabel";
            this.LevelSelectLabel.Size = new System.Drawing.Size(157, 13);
            this.LevelSelectLabel.TabIndex = 1;
            this.LevelSelectLabel.Text = "Level file to edit (complete path)";
            // 
            // FileLocBox
            // 
            this.FileLocBox.Location = new System.Drawing.Point(16, 57);
            this.FileLocBox.Name = "FileLocBox";
            this.FileLocBox.Size = new System.Drawing.Size(191, 20);
            this.FileLocBox.TabIndex = 2;
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(214, 53);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 3;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // EnemySelectBox
            // 
            this.EnemySelectBox.FormattingEnabled = true;
            this.EnemySelectBox.Items.AddRange(new object[] {
            "Please select a floor first"});
            this.EnemySelectBox.Location = new System.Drawing.Point(12, 275);
            this.EnemySelectBox.Name = "EnemySelectBox";
            this.EnemySelectBox.Size = new System.Drawing.Size(183, 121);
            this.EnemySelectBox.TabIndex = 4;
            this.EnemySelectBox.SelectedIndexChanged += new System.EventHandler(this.EnemySelectChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 256);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Enemy to Edit";
            // 
            // CommandList
            // 
            this.CommandList.FormattingEnabled = true;
            this.CommandList.Items.AddRange(new object[] {
            "Please select an enemy to edit first"});
            this.CommandList.Location = new System.Drawing.Point(12, 438);
            this.CommandList.Name = "CommandList";
            this.CommandList.Size = new System.Drawing.Size(183, 134);
            this.CommandList.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 422);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Selected Enemy\'s Commands";
            // 
            // StartXYZLabel
            // 
            this.StartXYZLabel.AutoSize = true;
            this.StartXYZLabel.Location = new System.Drawing.Point(5, 19);
            this.StartXYZLabel.Name = "StartXYZLabel";
            this.StartXYZLabel.Size = new System.Drawing.Size(17, 52);
            this.StartXYZLabel.TabIndex = 9;
            this.StartXYZLabel.Text = "X:\r\n\r\nY:\r\n\r";
            // 
            // StartXPosBox
            // 
            this.StartXPosBox.Location = new System.Drawing.Point(28, 19);
            this.StartXPosBox.Name = "StartXPosBox";
            this.StartXPosBox.Size = new System.Drawing.Size(75, 20);
            this.StartXPosBox.TabIndex = 10;
            // 
            // StartYPosBox
            // 
            this.StartYPosBox.Location = new System.Drawing.Point(28, 45);
            this.StartYPosBox.Name = "StartYPosBox";
            this.StartYPosBox.Size = new System.Drawing.Size(75, 20);
            this.StartYPosBox.TabIndex = 11;
            // 
            // StartPosBox
            // 
            this.StartPosBox.Controls.Add(this.StartPosChange);
            this.StartPosBox.Controls.Add(this.StartXYZLabel);
            this.StartPosBox.Controls.Add(this.StartYPosBox);
            this.StartPosBox.Controls.Add(this.StartXPosBox);
            this.StartPosBox.Location = new System.Drawing.Point(237, 256);
            this.StartPosBox.Name = "StartPosBox";
            this.StartPosBox.Size = new System.Drawing.Size(197, 81);
            this.StartPosBox.TabIndex = 13;
            this.StartPosBox.TabStop = false;
            this.StartPosBox.Text = "Start Position (On this floor):";
            // 
            // StartPosChange
            // 
            this.StartPosChange.Location = new System.Drawing.Point(110, 29);
            this.StartPosChange.Name = "StartPosChange";
            this.StartPosChange.Size = new System.Drawing.Size(75, 23);
            this.StartPosChange.TabIndex = 15;
            this.StartPosChange.Text = "Change";
            this.StartPosChange.UseVisualStyleBackColor = true;
            this.StartPosChange.Click += new System.EventHandler(this.StartPosChange_Click);
            // 
            // MoveCommandBox
            // 
            this.MoveCommandBox.Controls.Add(this.MoveAddButton);
            this.MoveCommandBox.Controls.Add(this.MoveZPosBox);
            this.MoveCommandBox.Controls.Add(this.MoveXYZLabel);
            this.MoveCommandBox.Controls.Add(this.MoveYPosBox);
            this.MoveCommandBox.Controls.Add(this.MoveXPosBox);
            this.MoveCommandBox.Location = new System.Drawing.Point(237, 373);
            this.MoveCommandBox.Name = "MoveCommandBox";
            this.MoveCommandBox.Size = new System.Drawing.Size(197, 94);
            this.MoveCommandBox.TabIndex = 14;
            this.MoveCommandBox.TabStop = false;
            this.MoveCommandBox.Text = "Add New Move Command:";
            // 
            // MoveAddButton
            // 
            this.MoveAddButton.Location = new System.Drawing.Point(110, 43);
            this.MoveAddButton.Name = "MoveAddButton";
            this.MoveAddButton.Size = new System.Drawing.Size(75, 23);
            this.MoveAddButton.TabIndex = 14;
            this.MoveAddButton.Text = "Add";
            this.MoveAddButton.UseVisualStyleBackColor = true;
            this.MoveAddButton.Click += new System.EventHandler(this.MoveAddButton_Click);
            // 
            // MoveZPosBox
            // 
            this.MoveZPosBox.Location = new System.Drawing.Point(28, 64);
            this.MoveZPosBox.Name = "MoveZPosBox";
            this.MoveZPosBox.Size = new System.Drawing.Size(75, 20);
            this.MoveZPosBox.TabIndex = 12;
            // 
            // MoveXYZLabel
            // 
            this.MoveXYZLabel.AutoSize = true;
            this.MoveXYZLabel.Location = new System.Drawing.Point(5, 19);
            this.MoveXYZLabel.Name = "MoveXYZLabel";
            this.MoveXYZLabel.Size = new System.Drawing.Size(17, 65);
            this.MoveXYZLabel.TabIndex = 9;
            this.MoveXYZLabel.Text = "X:\r\n\r\nY:\r\n\r\nZ:";
            // 
            // MoveYPosBox
            // 
            this.MoveYPosBox.Location = new System.Drawing.Point(28, 45);
            this.MoveYPosBox.Name = "MoveYPosBox";
            this.MoveYPosBox.Size = new System.Drawing.Size(75, 20);
            this.MoveYPosBox.TabIndex = 11;
            // 
            // MoveXPosBox
            // 
            this.MoveXPosBox.Location = new System.Drawing.Point(28, 19);
            this.MoveXPosBox.Name = "MoveXPosBox";
            this.MoveXPosBox.Size = new System.Drawing.Size(75, 20);
            this.MoveXPosBox.TabIndex = 10;
            // 
            // WaitCommandBox
            // 
            this.WaitCommandBox.Controls.Add(this.WaitCommandTextBox);
            this.WaitCommandBox.Controls.Add(this.WaitAddButton);
            this.WaitCommandBox.Controls.Add(this.label3);
            this.WaitCommandBox.Location = new System.Drawing.Point(237, 487);
            this.WaitCommandBox.Name = "WaitCommandBox";
            this.WaitCommandBox.Size = new System.Drawing.Size(197, 74);
            this.WaitCommandBox.TabIndex = 15;
            this.WaitCommandBox.TabStop = false;
            this.WaitCommandBox.Text = "Add New Wait Command:";
            // 
            // WaitCommandTextBox
            // 
            this.WaitCommandTextBox.Location = new System.Drawing.Point(6, 43);
            this.WaitCommandTextBox.Name = "WaitCommandTextBox";
            this.WaitCommandTextBox.Size = new System.Drawing.Size(89, 20);
            this.WaitCommandTextBox.TabIndex = 15;
            // 
            // WaitAddButton
            // 
            this.WaitAddButton.Location = new System.Drawing.Point(110, 43);
            this.WaitAddButton.Name = "WaitAddButton";
            this.WaitAddButton.Size = new System.Drawing.Size(75, 23);
            this.WaitAddButton.TabIndex = 14;
            this.WaitAddButton.Text = "Add";
            this.WaitAddButton.UseVisualStyleBackColor = true;
            this.WaitAddButton.Click += new System.EventHandler(this.WaitAddButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Frames to Wait (60 fps):";
            // 
            // FloorSelectLabel
            // 
            this.FloorSelectLabel.AutoSize = true;
            this.FloorSelectLabel.Location = new System.Drawing.Point(13, 94);
            this.FloorSelectLabel.Name = "FloorSelectLabel";
            this.FloorSelectLabel.Size = new System.Drawing.Size(66, 13);
            this.FloorSelectLabel.TabIndex = 17;
            this.FloorSelectLabel.Text = "Floor to Edit:";
            // 
            // FloorSelectBox
            // 
            this.FloorSelectBox.FormattingEnabled = true;
            this.FloorSelectBox.Items.AddRange(new object[] {
            "Please Load a map first"});
            this.FloorSelectBox.Location = new System.Drawing.Point(12, 113);
            this.FloorSelectBox.Name = "FloorSelectBox";
            this.FloorSelectBox.Size = new System.Drawing.Size(183, 121);
            this.FloorSelectBox.TabIndex = 16;
            this.FloorSelectBox.SelectedIndexChanged += new System.EventHandler(this.FloorSelectChanged);
            // 
            // RemoveCommand
            // 
            this.RemoveCommand.Location = new System.Drawing.Point(12, 579);
            this.RemoveCommand.Name = "RemoveCommand";
            this.RemoveCommand.Size = new System.Drawing.Size(183, 23);
            this.RemoveCommand.TabIndex = 18;
            this.RemoveCommand.Text = "Remove Command";
            this.RemoveCommand.UseVisualStyleBackColor = true;
            this.RemoveCommand.Click += new System.EventHandler(this.RemoveCommand_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(335, 53);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 19;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // FixCommands
            // 
            this.FixCommands.Location = new System.Drawing.Point(12, 608);
            this.FixCommands.Name = "FixCommands";
            this.FixCommands.Size = new System.Drawing.Size(183, 23);
            this.FixCommands.TabIndex = 20;
            this.FixCommands.Text = "Reorder Commands from Tiled";
            this.FixCommands.UseVisualStyleBackColor = true;
            this.FixCommands.Click += new System.EventHandler(this.FixCommands_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 658);
            this.Controls.Add(this.FixCommands);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RemoveCommand);
            this.Controls.Add(this.FloorSelectLabel);
            this.Controls.Add(this.FloorSelectBox);
            this.Controls.Add(this.WaitCommandBox);
            this.Controls.Add(this.MoveCommandBox);
            this.Controls.Add(this.StartPosBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CommandList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EnemySelectBox);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.FileLocBox);
            this.Controls.Add(this.LevelSelectLabel);
            this.Controls.Add(this.Title);
            this.Name = "Form1";
            this.Text = "Form1";
            this.StartPosBox.ResumeLayout(false);
            this.StartPosBox.PerformLayout();
            this.MoveCommandBox.ResumeLayout(false);
            this.MoveCommandBox.PerformLayout();
            this.WaitCommandBox.ResumeLayout(false);
            this.WaitCommandBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label LevelSelectLabel;
        private System.Windows.Forms.TextBox FileLocBox;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.ListBox EnemySelectBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox CommandList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label StartXYZLabel;
        private System.Windows.Forms.TextBox StartXPosBox;
        private System.Windows.Forms.TextBox StartYPosBox;
        private System.Windows.Forms.GroupBox StartPosBox;
        private System.Windows.Forms.GroupBox MoveCommandBox;
        private System.Windows.Forms.TextBox MoveZPosBox;
        private System.Windows.Forms.Label MoveXYZLabel;
        private System.Windows.Forms.TextBox MoveYPosBox;
        private System.Windows.Forms.TextBox MoveXPosBox;
        private System.Windows.Forms.Button MoveAddButton;
        private System.Windows.Forms.GroupBox WaitCommandBox;
        private System.Windows.Forms.Button WaitAddButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox WaitCommandTextBox;
        private System.Windows.Forms.Label FloorSelectLabel;
        private System.Windows.Forms.ListBox FloorSelectBox;
        private System.Windows.Forms.Button RemoveCommand;
        private System.Windows.Forms.Button StartPosChange;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button FixCommands;
    }
}


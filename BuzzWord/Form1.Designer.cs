namespace BuzzWord
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
            this.btnReadLexique = new System.Windows.Forms.Button();
            this.txtBuzzWord = new System.Windows.Forms.TextBox();
            this.btnGetBuzzWordList = new System.Windows.Forms.Button();
            this.lbResults = new System.Windows.Forms.ListBox();
            this.txtWordMinLength = new System.Windows.Forms.TextBox();
            this.lblWordCount = new System.Windows.Forms.Label();
            this.lblBuzzWordCount = new System.Windows.Forms.Label();
            this.lblBuzzWordTop5 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCount = new System.Windows.Forms.Button();
            this.btnCrossword = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReadLexique
            // 
            this.btnReadLexique.Location = new System.Drawing.Point(12, 12);
            this.btnReadLexique.Name = "btnReadLexique";
            this.btnReadLexique.Size = new System.Drawing.Size(118, 23);
            this.btnReadLexique.TabIndex = 0;
            this.btnReadLexique.Text = "Read Lexique";
            this.btnReadLexique.UseVisualStyleBackColor = true;
            this.btnReadLexique.Click += new System.EventHandler(this.btnReadLexique_Click);
            // 
            // txtBuzzWord
            // 
            this.txtBuzzWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuzzWord.Location = new System.Drawing.Point(13, 42);
            this.txtBuzzWord.Name = "txtBuzzWord";
            this.txtBuzzWord.Size = new System.Drawing.Size(244, 31);
            this.txtBuzzWord.TabIndex = 1;
            // 
            // btnGetBuzzWordList
            // 
            this.btnGetBuzzWordList.Location = new System.Drawing.Point(13, 79);
            this.btnGetBuzzWordList.Name = "btnGetBuzzWordList";
            this.btnGetBuzzWordList.Size = new System.Drawing.Size(117, 23);
            this.btnGetBuzzWordList.TabIndex = 2;
            this.btnGetBuzzWordList.Text = "Get Buzz Word List";
            this.btnGetBuzzWordList.UseVisualStyleBackColor = true;
            this.btnGetBuzzWordList.Click += new System.EventHandler(this.btnGetBuzzWordList_Click);
            // 
            // lbResults
            // 
            this.lbResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbResults.FormattingEnabled = true;
            this.lbResults.ItemHeight = 16;
            this.lbResults.Location = new System.Drawing.Point(12, 140);
            this.lbResults.Name = "lbResults";
            this.lbResults.Size = new System.Drawing.Size(352, 436);
            this.lbResults.TabIndex = 3;
            // 
            // txtWordMinLength
            // 
            this.txtWordMinLength.Location = new System.Drawing.Point(264, 42);
            this.txtWordMinLength.Name = "txtWordMinLength";
            this.txtWordMinLength.Size = new System.Drawing.Size(100, 20);
            this.txtWordMinLength.TabIndex = 4;
            this.txtWordMinLength.Text = "5";
            // 
            // lblWordCount
            // 
            this.lblWordCount.AutoSize = true;
            this.lblWordCount.Location = new System.Drawing.Point(137, 12);
            this.lblWordCount.Name = "lblWordCount";
            this.lblWordCount.Size = new System.Drawing.Size(13, 13);
            this.lblWordCount.TabIndex = 5;
            this.lblWordCount.Text = "0";
            // 
            // lblBuzzWordCount
            // 
            this.lblBuzzWordCount.AutoSize = true;
            this.lblBuzzWordCount.Location = new System.Drawing.Point(140, 80);
            this.lblBuzzWordCount.Name = "lblBuzzWordCount";
            this.lblBuzzWordCount.Size = new System.Drawing.Size(13, 13);
            this.lblBuzzWordCount.TabIndex = 6;
            this.lblBuzzWordCount.Text = "0";
            // 
            // lblBuzzWordTop5
            // 
            this.lblBuzzWordTop5.AutoSize = true;
            this.lblBuzzWordTop5.Location = new System.Drawing.Point(197, 80);
            this.lblBuzzWordTop5.Name = "lblBuzzWordTop5";
            this.lblBuzzWordTop5.Size = new System.Drawing.Size(13, 13);
            this.lblBuzzWordTop5.TabIndex = 7;
            this.lblBuzzWordTop5.Text = "0";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(250, 75);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(55, 23);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "TEST";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnCount
            // 
            this.btnCount.Location = new System.Drawing.Point(311, 75);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(53, 23);
            this.btnCount.TabIndex = 9;
            this.btnCount.Text = "COUNT";
            this.btnCount.UseVisualStyleBackColor = true;
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);
            // 
            // btnCrossword
            // 
            this.btnCrossword.Location = new System.Drawing.Point(13, 111);
            this.btnCrossword.Name = "btnCrossword";
            this.btnCrossword.Size = new System.Drawing.Size(351, 23);
            this.btnCrossword.TabIndex = 10;
            this.btnCrossword.Text = "Crossword (?=any *=multiple any)";
            this.btnCrossword.UseVisualStyleBackColor = true;
            this.btnCrossword.Click += new System.EventHandler(this.btnCrossword_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 590);
            this.Controls.Add(this.btnCrossword);
            this.Controls.Add(this.btnCount);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.lblBuzzWordTop5);
            this.Controls.Add(this.lblBuzzWordCount);
            this.Controls.Add(this.lblWordCount);
            this.Controls.Add(this.txtWordMinLength);
            this.Controls.Add(this.lbResults);
            this.Controls.Add(this.btnGetBuzzWordList);
            this.Controls.Add(this.txtBuzzWord);
            this.Controls.Add(this.btnReadLexique);
            this.Name = "Form1";
            this.Text = "Lexique";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadLexique;
        private System.Windows.Forms.TextBox txtBuzzWord;
        private System.Windows.Forms.Button btnGetBuzzWordList;
        private System.Windows.Forms.ListBox lbResults;
        private System.Windows.Forms.TextBox txtWordMinLength;
        private System.Windows.Forms.Label lblWordCount;
        private System.Windows.Forms.Label lblBuzzWordCount;
        private System.Windows.Forms.Label lblBuzzWordTop5;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.Button btnCrossword;
    }
}


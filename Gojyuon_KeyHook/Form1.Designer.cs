namespace Gojyuon_KeyHook
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitleCaption = new System.Windows.Forms.Label();
            this.lblCondition = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(67, 74);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(273, 74);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Hook";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(69, 173);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(273, 74);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(69, 298);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(273, 74);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitleCaption
            // 
            this.lblTitleCaption.AutoSize = true;
            this.lblTitleCaption.Location = new System.Drawing.Point(21, 19);
            this.lblTitleCaption.Name = "lblTitleCaption";
            this.lblTitleCaption.Size = new System.Drawing.Size(232, 30);
            this.lblTitleCaption.TabIndex = 3;
            this.lblTitleCaption.Text = "五十音 KeyBoard";
            // 
            // lblCondition
            // 
            this.lblCondition.AutoSize = true;
            this.lblCondition.ForeColor = System.Drawing.Color.Blue;
            this.lblCondition.Location = new System.Drawing.Point(298, 19);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(0, 30);
            this.lblCondition.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 405);
            this.Controls.Add(this.lblCondition);
            this.Controls.Add(this.lblTitleCaption);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitleCaption;
        private System.Windows.Forms.Label lblCondition;
    }
}


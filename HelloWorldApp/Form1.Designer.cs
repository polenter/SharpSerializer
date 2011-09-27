namespace HelloWorldApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.serializeXmlButton = new System.Windows.Forms.Button();
            this.serializeSizeOptimizedBinary = new System.Windows.Forms.Button();
            this.serializeBurstBinary = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // serializeXmlButton
            // 
            this.serializeXmlButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serializeXmlButton.Location = new System.Drawing.Point(12, 34);
            this.serializeXmlButton.Name = "serializeXmlButton";
            this.serializeXmlButton.Size = new System.Drawing.Size(429, 23);
            this.serializeXmlButton.TabIndex = 0;
            this.serializeXmlButton.Text = "Serialize Xml";
            this.serializeXmlButton.UseVisualStyleBackColor = true;
            this.serializeXmlButton.Click += new System.EventHandler(this.serializeXmlButton_Click);
            // 
            // serializeSizeOptimizedBinary
            // 
            this.serializeSizeOptimizedBinary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serializeSizeOptimizedBinary.Location = new System.Drawing.Point(12, 86);
            this.serializeSizeOptimizedBinary.Name = "serializeSizeOptimizedBinary";
            this.serializeSizeOptimizedBinary.Size = new System.Drawing.Size(429, 23);
            this.serializeSizeOptimizedBinary.TabIndex = 1;
            this.serializeSizeOptimizedBinary.Text = "Serialize Binary (size optimized)";
            this.serializeSizeOptimizedBinary.UseVisualStyleBackColor = true;
            this.serializeSizeOptimizedBinary.Click += new System.EventHandler(this.serializeSizeOptimizedBinary_Click);
            // 
            // serializeBurstBinary
            // 
            this.serializeBurstBinary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serializeBurstBinary.Location = new System.Drawing.Point(12, 171);
            this.serializeBurstBinary.Name = "serializeBurstBinary";
            this.serializeBurstBinary.Size = new System.Drawing.Size(429, 23);
            this.serializeBurstBinary.TabIndex = 3;
            this.serializeBurstBinary.Text = "Serialize Binary (burst)";
            this.serializeBurstBinary.UseVisualStyleBackColor = true;
            this.serializeBurstBinary.Click += new System.EventHandler(this.serializeBurstBinary_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(429, 56);
            this.label1.TabIndex = 2;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(12, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(429, 52);
            this.label2.TabIndex = 4;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 338);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(429, 36);
            this.label3.TabIndex = 5;
            this.label3.Text = "Refer to Form1.cs for more details or visit the project page.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(9, 374);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(123, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "www.sharpserializer.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 396);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serializeBurstBinary);
            this.Controls.Add(this.serializeSizeOptimizedBinary);
            this.Controls.Add(this.serializeXmlButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button serializeXmlButton;
        private System.Windows.Forms.Button serializeSizeOptimizedBinary;
        private System.Windows.Forms.Button serializeBurstBinary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}


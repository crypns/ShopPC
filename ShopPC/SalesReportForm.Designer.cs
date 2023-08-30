namespace ShopPC
{
    partial class SalesReportForm
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
            this.dataGridViewSalesSummary = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSalesSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSalesSummary
            // 
            this.dataGridViewSalesSummary.AllowUserToAddRows = false;
            this.dataGridViewSalesSummary.AllowUserToDeleteRows = false;
            this.dataGridViewSalesSummary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSalesSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSalesSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSalesSummary.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSalesSummary.MultiSelect = false;
            this.dataGridViewSalesSummary.Name = "dataGridViewSalesSummary";
            this.dataGridViewSalesSummary.ReadOnly = true;
            this.dataGridViewSalesSummary.Size = new System.Drawing.Size(800, 450);
            this.dataGridViewSalesSummary.TabIndex = 1;
            // 
            // SalesReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewSalesSummary);
            this.Name = "SalesReportForm";
            this.Text = "Отчет по продажам";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSalesSummary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSalesSummary;
    }
}
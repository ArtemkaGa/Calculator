namespace CalculatorApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            // Устанавливаем точный размер окна под контент
            this.ClientSize = new System.Drawing.Size(412, 570);
            this.Text = "Калькулятор";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            // Запрещаем менять размер формы (чтобы не было пустых полей)
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
        }
    }
}
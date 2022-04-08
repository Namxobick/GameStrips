
namespace GameStripes
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.gameStrips1 = new GameStripes.GameStrips();
            this.SuspendLayout();
            // 
            // gameStrips1
            // 
            this.gameStrips1.Location = new System.Drawing.Point(207, 12);
            this.gameStrips1.MinimumSize = new System.Drawing.Size(500, 500);
            this.gameStrips1.Name = "gameStrips1";
            this.gameStrips1.Size = new System.Drawing.Size(785, 537);
            this.gameStrips1.TabIndex = 0;
            this.gameStrips1.Text = "gameStrips1";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1277, 625);
            this.Controls.Add(this.gameStrips1);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private GameStrips gameStrips1;
    }
}


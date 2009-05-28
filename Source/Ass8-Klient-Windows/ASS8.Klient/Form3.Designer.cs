namespace ASS8.Klient
{
    partial class Pobierane
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
            this.glPliki = new GlacialComponents.Controls.GlacialList();
            this.btnCzysc = new System.Windows.Forms.Button();
            this.btnZamknij = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // glPliki
            // 
            this.glPliki.AllowColumnResize = true;
            this.glPliki.AllowMultiselect = false;
            this.glPliki.AlternateBackground = System.Drawing.Color.DarkGreen;
            this.glPliki.AlternatingColors = false;
            this.glPliki.AutoHeight = true;
            this.glPliki.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.glPliki.BackgroundStretchToFit = true;
            this.glPliki.ControlStyle = GlacialComponents.Controls.GLControlStyles.Normal;
            this.glPliki.FullRowSelect = true;
            this.glPliki.GridColor = System.Drawing.Color.LightGray;
            this.glPliki.GridLines = GlacialComponents.Controls.GLGridLines.gridBoth;
            this.glPliki.GridLineStyle = GlacialComponents.Controls.GLGridLineStyles.gridSolid;
            this.glPliki.GridTypes = GlacialComponents.Controls.GLGridTypes.gridOnExists;
            this.glPliki.HeaderHeight = 22;
            this.glPliki.HeaderVisible = true;
            this.glPliki.HeaderWordWrap = false;
            this.glPliki.HotColumnTracking = false;
            this.glPliki.HotItemTracking = false;
            this.glPliki.HotTrackingColor = System.Drawing.Color.LightGray;
            this.glPliki.HoverEvents = false;
            this.glPliki.HoverTime = 1;
            this.glPliki.ImageList = null;
            this.glPliki.ItemHeight = 18;
            this.glPliki.ItemWordWrap = false;
            this.glPliki.Location = new System.Drawing.Point(12, 12);
            this.glPliki.Name = "glPliki";
            this.glPliki.Selectable = true;
            this.glPliki.SelectedTextColor = System.Drawing.Color.White;
            this.glPliki.SelectionColor = System.Drawing.Color.DarkBlue;
            this.glPliki.ShowBorder = true;
            this.glPliki.ShowFocusRect = false;
            this.glPliki.Size = new System.Drawing.Size(545, 145);
            this.glPliki.SortType = GlacialComponents.Controls.SortTypes.InsertionSort;
            this.glPliki.SuperFlatHeaderColor = System.Drawing.Color.White;
            this.glPliki.TabIndex = 0;
            this.glPliki.Text = "Pobieranie";
            // 
            // btnCzysc
            // 
            this.btnCzysc.Location = new System.Drawing.Point(86, 171);
            this.btnCzysc.Name = "btnCzysc";
            this.btnCzysc.Size = new System.Drawing.Size(75, 23);
            this.btnCzysc.TabIndex = 1;
            this.btnCzysc.Text = "Wyczyść";
            this.btnCzysc.UseVisualStyleBackColor = true;
            this.btnCzysc.Click += new System.EventHandler(this.btnCzysc_Click);
            // 
            // btnZamknij
            // 
            this.btnZamknij.Location = new System.Drawing.Point(409, 171);
            this.btnZamknij.Name = "btnZamknij";
            this.btnZamknij.Size = new System.Drawing.Size(75, 23);
            this.btnZamknij.TabIndex = 2;
            this.btnZamknij.Text = "Zamknij";
            this.btnZamknij.UseVisualStyleBackColor = true;
            this.btnZamknij.Click += new System.EventHandler(this.btnZamknij_Click);
            // 
            // Pobierane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 206);
            this.Controls.Add(this.btnZamknij);
            this.Controls.Add(this.btnCzysc);
            this.Controls.Add(this.glPliki);
            this.Name = "Pobierane";
            this.Text = "Pobierane";
            this.ResumeLayout(false);

        }

        #endregion

        private GlacialComponents.Controls.GlacialList glPliki;
        private System.Windows.Forms.Button btnCzysc;
        private System.Windows.Forms.Button btnZamknij;
    }
}
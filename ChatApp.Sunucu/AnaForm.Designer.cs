namespace ChatApp.Sunucu;

partial class AnaForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        pnlUst = new Panel();
        lblPort = new Label();
        nudPort = new NumericUpDown();
        btnBaslat = new Button();
        btnDurdur = new Button();
        chkYerelAg = new CheckBox();
        grpIstemciler = new GroupBox();
        lvIstemciler = new ListView();
        btnIstemciyiAt = new Button();
        colKullanici = new ColumnHeader();
        colAdres = new ColumnHeader();
        colZaman = new ColumnHeader();
        grpGunluk = new GroupBox();
        lstGunluk = new ListBox();
        durumCubugu = new StatusStrip();
        lblDurum = new ToolStripStatusLabel();
        lblIstemciSayisi = new ToolStripStatusLabel();
        lblYerelIp = new ToolStripStatusLabel();
        pnlUst.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
        grpIstemciler.SuspendLayout();
        grpGunluk.SuspendLayout();
        durumCubugu.SuspendLayout();
        SuspendLayout();
        //
        // pnlUst
        //
        pnlUst.Controls.Add(lblPort);
        pnlUst.Controls.Add(nudPort);
        pnlUst.Controls.Add(btnBaslat);
        pnlUst.Controls.Add(btnDurdur);
        pnlUst.Controls.Add(chkYerelAg);
        pnlUst.Dock = DockStyle.Top;
        pnlUst.Name = "pnlUst";
        pnlUst.Padding = new Padding(8);
        pnlUst.Size = new Size(760, 48);
        pnlUst.TabIndex = 0;
        //
        // lblPort
        //
        lblPort.AutoSize = true;
        lblPort.Location = new Point(12, 16);
        lblPort.Name = "lblPort";
        lblPort.Text = "Port:";
        //
        // nudPort
        //
        nudPort.Location = new Point(52, 12);
        nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
        nudPort.Name = "nudPort";
        nudPort.Size = new Size(80, 23);
        nudPort.TabIndex = 1;
        nudPort.Value = new decimal(new int[] { 5050, 0, 0, 0 });
        //
        // btnBaslat
        //
        btnBaslat.Location = new Point(146, 11);
        btnBaslat.Name = "btnBaslat";
        btnBaslat.Size = new Size(90, 26);
        btnBaslat.TabIndex = 2;
        btnBaslat.Text = "Başlat";
        btnBaslat.UseVisualStyleBackColor = true;
        btnBaslat.Click += btnBaslat_Click;
        //
        // btnDurdur
        //
        btnDurdur.Enabled = false;
        btnDurdur.Location = new Point(242, 11);
        btnDurdur.Name = "btnDurdur";
        btnDurdur.Size = new Size(90, 26);
        btnDurdur.TabIndex = 3;
        btnDurdur.Text = "Durdur";
        btnDurdur.UseVisualStyleBackColor = true;
        btnDurdur.Click += btnDurdur_Click;
        //
        // chkYerelAg
        //
        chkYerelAg.AutoSize = true;
        chkYerelAg.Checked = true;
        chkYerelAg.CheckState = CheckState.Checked;
        chkYerelAg.Location = new Point(348, 15);
        chkYerelAg.Name = "chkYerelAg";
        chkYerelAg.TabIndex = 4;
        chkYerelAg.Text = "Yerel ağa açık (diğer bilgisayarlar bağlanabilir)";
        chkYerelAg.UseVisualStyleBackColor = true;
        //
        // grpIstemciler
        //
        grpIstemciler.Controls.Add(lvIstemciler);
        grpIstemciler.Controls.Add(btnIstemciyiAt);
        grpIstemciler.Dock = DockStyle.Top;
        grpIstemciler.Name = "grpIstemciler";
        grpIstemciler.Padding = new Padding(8);
        grpIstemciler.Size = new Size(760, 220);
        grpIstemciler.TabIndex = 1;
        grpIstemciler.TabStop = false;
        grpIstemciler.Text = "Bağlı istemciler";
        //
        // lvIstemciler
        //
        lvIstemciler.Columns.AddRange(new ColumnHeader[] { colKullanici, colAdres, colZaman });
        lvIstemciler.Dock = DockStyle.Fill;
        lvIstemciler.FullRowSelect = true;
        lvIstemciler.GridLines = true;
        lvIstemciler.Name = "lvIstemciler";
        lvIstemciler.TabIndex = 0;
        lvIstemciler.UseCompatibleStateImageBehavior = false;
        lvIstemciler.View = View.Details;
        lvIstemciler.MultiSelect = false;
        lvIstemciler.SelectedIndexChanged += lvIstemciler_SelectedIndexChanged;
        //
        // btnIstemciyiAt
        //
        btnIstemciyiAt.Dock = DockStyle.Bottom;
        btnIstemciyiAt.Enabled = false;
        btnIstemciyiAt.Name = "btnIstemciyiAt";
        btnIstemciyiAt.Size = new Size(744, 28);
        btnIstemciyiAt.TabIndex = 1;
        btnIstemciyiAt.Text = "Seçili istemcinin bağlantısını kes";
        btnIstemciyiAt.UseVisualStyleBackColor = true;
        btnIstemciyiAt.Click += btnIstemciyiAt_Click;
        //
        // colKullanici
        //
        colKullanici.Text = "Kullanıcı adı";
        colKullanici.Width = 180;
        //
        // colAdres
        //
        colAdres.Text = "Adres (IP:port)";
        colAdres.Width = 220;
        //
        // colZaman
        //
        colZaman.Text = "Bağlanma zamanı";
        colZaman.Width = 200;
        //
        // grpGunluk
        //
        grpGunluk.Controls.Add(lstGunluk);
        grpGunluk.Dock = DockStyle.Fill;
        grpGunluk.Name = "grpGunluk";
        grpGunluk.Padding = new Padding(8);
        grpGunluk.TabIndex = 2;
        grpGunluk.TabStop = false;
        grpGunluk.Text = "Sunucu günlüğü";
        //
        // lstGunluk
        //
        lstGunluk.Dock = DockStyle.Fill;
        lstGunluk.HorizontalScrollbar = true;
        lstGunluk.IntegralHeight = false;
        lstGunluk.Name = "lstGunluk";
        lstGunluk.TabIndex = 0;
        //
        // durumCubugu
        //
        durumCubugu.Items.AddRange(new ToolStripItem[] { lblDurum, lblYerelIp, lblIstemciSayisi });
        durumCubugu.Name = "durumCubugu";
        durumCubugu.TabIndex = 3;
        //
        // lblDurum
        //
        lblDurum.Name = "lblDurum";
        lblDurum.Spring = true;
        lblDurum.Text = "Sunucu kapalı";
        lblDurum.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblIstemciSayisi
        //
        lblIstemciSayisi.Name = "lblIstemciSayisi";
        lblIstemciSayisi.Text = "Bağlı istemci: 0";
        //
        // lblYerelIp
        //
        lblYerelIp.ForeColor = SystemColors.GrayText;
        lblYerelIp.Name = "lblYerelIp";
        lblYerelIp.Text = "IP: -";
        //
        // AnaForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(760, 520);
        Controls.Add(grpGunluk);
        Controls.Add(grpIstemciler);
        Controls.Add(pnlUst);
        Controls.Add(durumCubugu);
        MinimumSize = new Size(640, 420);
        Name = "AnaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sohbet Sunucusu — YMH219";
        FormClosing += AnaForm_FormClosing;
        Load += AnaForm_Load;
        pnlUst.ResumeLayout(false);
        pnlUst.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
        grpIstemciler.ResumeLayout(false);
        grpGunluk.ResumeLayout(false);
        durumCubugu.ResumeLayout(false);
        durumCubugu.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlUst;
    private Label lblPort;
    private NumericUpDown nudPort;
    private Button btnBaslat;
    private Button btnDurdur;
    private CheckBox chkYerelAg;
    private GroupBox grpIstemciler;
    private ListView lvIstemciler;
    private Button btnIstemciyiAt;
    private ColumnHeader colKullanici;
    private ColumnHeader colAdres;
    private ColumnHeader colZaman;
    private GroupBox grpGunluk;
    private ListBox lstGunluk;
    private StatusStrip durumCubugu;
    private ToolStripStatusLabel lblDurum;
    private ToolStripStatusLabel lblIstemciSayisi;
    private ToolStripStatusLabel lblYerelIp;
}

namespace ChatApp.Istemci;

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
        pnlBaglanti = new Panel();
        lblSunucu = new Label();
        txtSunucu = new TextBox();
        lblPort = new Label();
        nudPort = new NumericUpDown();
        lblKullaniciAdi = new Label();
        txtKullaniciAdi = new TextBox();
        btnBaglan = new Button();
        btnKes = new Button();
        grpKullanicilar = new GroupBox();
        lstKullanicilar = new ListBox();
        grpMesajlar = new GroupBox();
        lstMesajlar = new ListBox();
        pnlGonder = new Panel();
        txtMesaj = new TextBox();
        btnGonder = new Button();
        durumCubugu = new StatusStrip();
        lblDurum = new ToolStripStatusLabel();
        pnlBaglanti.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
        grpKullanicilar.SuspendLayout();
        grpMesajlar.SuspendLayout();
        pnlGonder.SuspendLayout();
        durumCubugu.SuspendLayout();
        SuspendLayout();
        //
        // pnlBaglanti
        //
        pnlBaglanti.Controls.Add(lblSunucu);
        pnlBaglanti.Controls.Add(txtSunucu);
        pnlBaglanti.Controls.Add(lblPort);
        pnlBaglanti.Controls.Add(nudPort);
        pnlBaglanti.Controls.Add(lblKullaniciAdi);
        pnlBaglanti.Controls.Add(txtKullaniciAdi);
        pnlBaglanti.Controls.Add(btnBaglan);
        pnlBaglanti.Controls.Add(btnKes);
        pnlBaglanti.Dock = DockStyle.Top;
        pnlBaglanti.Name = "pnlBaglanti";
        pnlBaglanti.Size = new Size(720, 48);
        pnlBaglanti.TabIndex = 0;
        //
        // lblSunucu
        //
        lblSunucu.AutoSize = true;
        lblSunucu.Location = new Point(10, 16);
        lblSunucu.Name = "lblSunucu";
        lblSunucu.Text = "Sunucu:";
        //
        // txtSunucu
        //
        txtSunucu.Location = new Point(64, 12);
        txtSunucu.Name = "txtSunucu";
        txtSunucu.Size = new Size(120, 23);
        txtSunucu.TabIndex = 1;
        txtSunucu.Text = "127.0.0.1";
        //
        // lblPort
        //
        lblPort.AutoSize = true;
        lblPort.Location = new Point(194, 16);
        lblPort.Name = "lblPort";
        lblPort.Text = "Port:";
        //
        // nudPort
        //
        nudPort.Location = new Point(230, 12);
        nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudPort.Name = "nudPort";
        nudPort.Size = new Size(70, 23);
        nudPort.TabIndex = 2;
        nudPort.Value = new decimal(new int[] { 5050, 0, 0, 0 });
        //
        // lblKullaniciAdi
        //
        lblKullaniciAdi.AutoSize = true;
        lblKullaniciAdi.Location = new Point(312, 16);
        lblKullaniciAdi.Name = "lblKullaniciAdi";
        lblKullaniciAdi.Text = "Kullanıcı adı:";
        //
        // txtKullaniciAdi
        //
        txtKullaniciAdi.Location = new Point(394, 12);
        txtKullaniciAdi.MaxLength = 20;
        txtKullaniciAdi.Name = "txtKullaniciAdi";
        txtKullaniciAdi.Size = new Size(110, 23);
        txtKullaniciAdi.TabIndex = 3;
        //
        // btnBaglan
        //
        btnBaglan.Location = new Point(514, 11);
        btnBaglan.Name = "btnBaglan";
        btnBaglan.Size = new Size(85, 26);
        btnBaglan.TabIndex = 4;
        btnBaglan.Text = "Bağlan";
        btnBaglan.UseVisualStyleBackColor = true;
        btnBaglan.Click += btnBaglan_Click;
        //
        // btnKes
        //
        btnKes.Enabled = false;
        btnKes.Location = new Point(605, 11);
        btnKes.Name = "btnKes";
        btnKes.Size = new Size(105, 26);
        btnKes.TabIndex = 5;
        btnKes.Text = "Bağlantıyı Kes";
        btnKes.UseVisualStyleBackColor = true;
        btnKes.Click += btnKes_Click;
        //
        // grpKullanicilar
        //
        grpKullanicilar.Controls.Add(lstKullanicilar);
        grpKullanicilar.Dock = DockStyle.Right;
        grpKullanicilar.Name = "grpKullanicilar";
        grpKullanicilar.Padding = new Padding(6);
        grpKullanicilar.Size = new Size(170, 400);
        grpKullanicilar.TabIndex = 3;
        grpKullanicilar.TabStop = false;
        grpKullanicilar.Text = "Çevrimiçi";
        //
        // lstKullanicilar
        //
        lstKullanicilar.Dock = DockStyle.Fill;
        lstKullanicilar.IntegralHeight = false;
        lstKullanicilar.Name = "lstKullanicilar";
        lstKullanicilar.TabIndex = 0;
        //
        // grpMesajlar
        //
        grpMesajlar.Controls.Add(lstMesajlar);
        grpMesajlar.Dock = DockStyle.Fill;
        grpMesajlar.Name = "grpMesajlar";
        grpMesajlar.Padding = new Padding(6);
        grpMesajlar.TabIndex = 2;
        grpMesajlar.TabStop = false;
        grpMesajlar.Text = "Mesajlar";
        //
        // lstMesajlar
        //
        lstMesajlar.Dock = DockStyle.Fill;
        lstMesajlar.HorizontalScrollbar = true;
        lstMesajlar.IntegralHeight = false;
        lstMesajlar.Name = "lstMesajlar";
        lstMesajlar.TabIndex = 0;
        //
        // pnlGonder
        //
        pnlGonder.Controls.Add(txtMesaj);
        pnlGonder.Controls.Add(btnGonder);
        pnlGonder.Dock = DockStyle.Bottom;
        pnlGonder.Name = "pnlGonder";
        pnlGonder.Padding = new Padding(8, 6, 8, 6);
        pnlGonder.Size = new Size(720, 40);
        pnlGonder.TabIndex = 1;
        //
        // txtMesaj
        //
        txtMesaj.Dock = DockStyle.Fill;
        txtMesaj.Enabled = false;
        txtMesaj.MaxLength = 500;
        txtMesaj.Name = "txtMesaj";
        txtMesaj.PlaceholderText = "Mesajınızı yazın ve Enter'a basın";
        txtMesaj.TabIndex = 0;
        //
        // btnGonder
        //
        btnGonder.Dock = DockStyle.Right;
        btnGonder.Enabled = false;
        btnGonder.Name = "btnGonder";
        btnGonder.Size = new Size(110, 28);
        btnGonder.TabIndex = 1;
        btnGonder.Text = "Mesaj Gönder";
        btnGonder.UseVisualStyleBackColor = true;
        btnGonder.Click += btnGonder_Click;
        //
        // durumCubugu
        //
        durumCubugu.Items.AddRange(new ToolStripItem[] { lblDurum });
        durumCubugu.Name = "durumCubugu";
        durumCubugu.TabIndex = 4;
        //
        // lblDurum
        //
        lblDurum.Name = "lblDurum";
        lblDurum.Spring = true;
        lblDurum.Text = "Bağlı değil";
        lblDurum.TextAlign = ContentAlignment.MiddleLeft;
        //
        // AnaForm
        //
        AcceptButton = btnGonder;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(720, 480);
        Controls.Add(grpMesajlar);
        Controls.Add(grpKullanicilar);
        Controls.Add(pnlGonder);
        Controls.Add(pnlBaglanti);
        Controls.Add(durumCubugu);
        MinimumSize = new Size(736, 360);
        Name = "AnaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sohbet İstemcisi — YMH219";
        FormClosing += AnaForm_FormClosing;
        pnlBaglanti.ResumeLayout(false);
        pnlBaglanti.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
        grpKullanicilar.ResumeLayout(false);
        grpMesajlar.ResumeLayout(false);
        pnlGonder.ResumeLayout(false);
        pnlGonder.PerformLayout();
        durumCubugu.ResumeLayout(false);
        durumCubugu.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlBaglanti;
    private Label lblSunucu;
    private TextBox txtSunucu;
    private Label lblPort;
    private NumericUpDown nudPort;
    private Label lblKullaniciAdi;
    private TextBox txtKullaniciAdi;
    private Button btnBaglan;
    private Button btnKes;
    private GroupBox grpKullanicilar;
    private ListBox lstKullanicilar;
    private GroupBox grpMesajlar;
    private ListBox lstMesajlar;
    private Panel pnlGonder;
    private TextBox txtMesaj;
    private Button btnGonder;
    private StatusStrip durumCubugu;
    private ToolStripStatusLabel lblDurum;
}

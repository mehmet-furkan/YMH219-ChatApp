using ChatApp.Istemci.Models;
using ChatApp.Istemci.Services;

namespace ChatApp.Istemci;

/// <summary>
/// İstemci penceresi: sunucu adresi + kullanıcı adı ile bağlanma, mesaj listesi ve mesaj gönderme.
/// Ağ işleri SohbetIstemcisi'ndedir; form olaylara abone olup ekranı günceller.
/// </summary>
public partial class AnaForm : Form
{
    private readonly SohbetIstemcisi _istemci = new();

    public AnaForm()
    {
        InitializeComponent();

        _istemci.MesajAlindi += (_, mesaj) => ArayuzdeCalistir(() => MesajEkle(EkranSatiri.MesajdanOlustur(mesaj, _istemci.KullaniciAdi)));
        _istemci.KullaniciListesiGeldi += (_, liste) => ArayuzdeCalistir(() => KullanicilariGoster(liste));
        _istemci.BaglantiKesildi += (_, sebep) => ArayuzdeCalistir(() => BaglantiKesildi(sebep));

        txtKullaniciAdi.Text = "kullanici" + Random.Shared.Next(10, 100);
    }

    // ---------- Butonlar ----------

    private async void btnBaglan_Click(object? sender, EventArgs e)
    {
        btnBaglan.Enabled = false;
        lblDurum.Text = $"{txtSunucu.Text}:{nudPort.Value} adresine bağlanılıyor...";
        lblDurum.ForeColor = SystemColors.ControlText;

        try
        {
            await _istemci.BaglanAsync(txtSunucu.Text, (int)nudPort.Value, txtKullaniciAdi.Text);
        }
        catch (BaglantiHatasiException ex)
        {
            // Hata uygulamayı kapatmaz: kullanıcıya gösterilir, tekrar denenebilir.
            DurumYaz("Bağlanılamadı", hata: true);
            MesajEkle(EkranSatiri.HataSatiri(ex.Message));
            MessageBox.Show(ex.Message, "Bağlantı hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            btnBaglan.Enabled = true;
            return;
        }

        DurumYaz($"Bağlı: {_istemci.KullaniciAdi} @ {txtSunucu.Text}:{nudPort.Value}", hata: false);
        BaglantiAlanlariniAyarla(bagli: true);
        txtMesaj.Focus();
    }

    private async void btnKes_Click(object? sender, EventArgs e)
    {
        btnKes.Enabled = false;
        await _istemci.KesAsync(); // ekran BaglantiKesildi olayında güncellenir
    }

    private async void btnGonder_Click(object? sender, EventArgs e)
    {
        string metin = txtMesaj.Text;
        if (string.IsNullOrWhiteSpace(metin) || !_istemci.Bagli)
            return;

        try
        {
            await _istemci.GonderAsync(metin);
            txtMesaj.Clear();
        }
        catch (FormatException ex)
        {
            DurumYaz(ex.Message, hata: true); // ör. "/w" yanlış yazıldı: metin silinmez, düzeltilebilir
        }
        catch (BaglantiHatasiException ex)
        {
            DurumYaz(ex.Message, hata: true);
        }
        txtMesaj.Focus();
    }

    /// <summary>Çevrimiçi listesinde bir kullanıcıya çift tıklayınca ona özel mesaj komutu hazırlanır.</summary>
    private void lstKullanicilar_DoubleClick(object? sender, EventArgs e)
    {
        if (lstKullanicilar.SelectedItem is not string secili || !_istemci.Bagli)
            return;

        string ad = secili.Replace(" (sen)", "");
        txtMesaj.Text = $"/w {ad} ";
        txtMesaj.Focus();
        txtMesaj.SelectionStart = txtMesaj.TextLength;
    }

    private async void AnaForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_istemci.Bagli)
            await _istemci.KesAsync();
    }

    // ---------- Ekran güncelleme ----------

    /// <summary>İstemci olayları ağ görevinden gelebilir; kontrolleri UI thread'inde güncelle.</summary>
    private void ArayuzdeCalistir(Action islem)
    {
        if (IsDisposed || Disposing) return;
        if (InvokeRequired)
            BeginInvoke(islem);
        else
            islem();
    }

    private void BaglantiKesildi(string sebep)
    {
        MesajEkle(EkranSatiri.HataSatiri(sebep));
        DurumYaz(sebep, hata: sebep != "Bağlantı kesildi.");
        lstKullanicilar.Items.Clear();
        BaglantiAlanlariniAyarla(bagli: false);
    }

    private void MesajEkle(EkranSatiri satir)
    {
        lstMesajlar.Items.Add(satir);
        lstMesajlar.TopIndex = lstMesajlar.Items.Count - 1;
    }

    /// <summary>
    /// ListBox "OwnerDrawFixed" olduğu için her satırı biz çiziyoruz: satır türüne göre renk seçilir.
    /// </summary>
    private void lstMesajlar_DrawItem(object? sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index < 0 || lstMesajlar.Items[e.Index] is not EkranSatiri satir)
            return;

        Color renk = satir.Tur switch
        {
            SatirTuru.Kendi => Color.FromArgb(29, 78, 216),   // mavi
            SatirTuru.Ozel => Color.FromArgb(147, 51, 234),   // mor
            SatirTuru.Sistem => Color.Gray,
            SatirTuru.Hata => Color.Firebrick,
            _ => SystemColors.WindowText
        };
        if ((e.State & DrawItemState.Selected) != 0)
            renk = SystemColors.HighlightText;

        var yaziTipi = satir.Tur is SatirTuru.Sistem ? new Font(e.Font!, FontStyle.Italic) : e.Font!;
        TextRenderer.DrawText(e.Graphics, satir.Metin, yaziTipi, e.Bounds, renk, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        if (yaziTipi != e.Font) yaziTipi.Dispose();
    }

    private void KullanicilariGoster(IReadOnlyList<string> kullanicilar)
    {
        lstKullanicilar.BeginUpdate();
        lstKullanicilar.Items.Clear();
        foreach (var ad in kullanicilar)
            lstKullanicilar.Items.Add(ad == _istemci.KullaniciAdi ? ad + " (sen)" : ad);
        lstKullanicilar.EndUpdate();
        grpKullanicilar.Text = $"Çevrimiçi ({kullanicilar.Count})";
    }

    private void BaglantiAlanlariniAyarla(bool bagli)
    {
        txtSunucu.Enabled = nudPort.Enabled = txtKullaniciAdi.Enabled = !bagli;
        btnBaglan.Enabled = !bagli;
        btnKes.Enabled = bagli;
        txtMesaj.Enabled = btnGonder.Enabled = bagli;
        Text = bagli ? $"Sohbet İstemcisi — {_istemci.KullaniciAdi}" : "Sohbet İstemcisi — YMH219";
        if (!bagli) grpKullanicilar.Text = "Çevrimiçi";
    }

    private void DurumYaz(string metin, bool hata)
    {
        lblDurum.Text = metin;
        lblDurum.ForeColor = hata ? Color.Firebrick : Color.SeaGreen;
    }
}

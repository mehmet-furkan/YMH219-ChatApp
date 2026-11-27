using System.Net.Sockets;
using ChatApp.Sunucu.Models;
using ChatApp.Sunucu.Services;

namespace ChatApp.Sunucu;

/// <summary>
/// Sunucu penceresi: port seçimi, Başlat/Durdur, bağlı istemci listesi ve sunucu günlüğü.
/// Ağ işleri SohbetSunucusu'ndadır; form sadece olaylara abone olup ekranı günceller.
/// </summary>
public partial class AnaForm : Form
{
    private readonly SohbetSunucusu _sunucu = new(new MesajGecmisi());

    public AnaForm()
    {
        InitializeComponent();

        _sunucu.IstemciBaglandi += (_, istemci) => ArayuzdeCalistir(() => IstemciyiListeyeEkle(istemci));
        _sunucu.IstemciAyrildi += (_, istemci) => ArayuzdeCalistir(() => IstemciyiListedenCikar(istemci));
        _sunucu.Gunluk += (_, metin) => ArayuzdeCalistir(() => GunlugeEkle(metin));
    }

    private void AnaForm_Load(object? sender, EventArgs e)
    {
        var adresler = AgBilgisi.YerelIPv4Adresleri();
        lblYerelIp.Text = "Bu bilgisayarın IP'si: " + (adresler.Count > 0 ? string.Join(", ", adresler) : "bulunamadı");
    }

    // ---------- Butonlar ----------

    private void btnBaslat_Click(object? sender, EventArgs e)
    {
        int port = (int)nudPort.Value;
        try
        {
            _sunucu.Baslat(port, chkYerelAg.Checked);
        }
        catch (SocketException ex)
        {
            // En sık sebep: port başka bir program (veya açık kalmış başka bir sunucu) tarafından kullanılıyor.
            GunlugeEkle($"HATA: Port {port} dinlenemedi: {ex.Message}");
            MessageBox.Show($"Sunucu başlatılamadı.\n\nPort {port} kullanımda olabilir; başka bir port deneyin.\n\nAyrıntı: {ex.Message}",
                "Sunucu başlatılamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DurumuGuncelle();
    }

    private void btnDurdur_Click(object? sender, EventArgs e)
    {
        _sunucu.Durdur();
        DurumuGuncelle();
    }

    private void AnaForm_FormClosing(object? sender, FormClosingEventArgs e) => _sunucu.Durdur();

    // ---------- Ekran güncelleme ----------

    /// <summary>
    /// Sunucu olayları ağ görevlerinden gelebilir; kontroller sadece oluşturuldukları (UI) thread'den
    /// değiştirilebildiği için gerekirse BeginInvoke ile UI thread'ine aktarılır.
    /// </summary>
    private void ArayuzdeCalistir(Action islem)
    {
        if (IsDisposed) return;
        if (InvokeRequired)
            BeginInvoke(islem);
        else
            islem();
    }

    private void IstemciyiListeyeEkle(BagliIstemci istemci)
    {
        var satir = new ListViewItem(istemci.KullaniciAdi) { Tag = istemci };
        satir.SubItems.Add(istemci.UzakAdres);
        satir.SubItems.Add(istemci.BaglanmaZamani.ToString("dd.MM.yyyy HH:mm:ss"));
        lvIstemciler.Items.Add(satir);
        DurumuGuncelle();
    }

    private void IstemciyiListedenCikar(BagliIstemci istemci)
    {
        foreach (ListViewItem satir in lvIstemciler.Items)
        {
            if (satir.Tag == istemci)
            {
                lvIstemciler.Items.Remove(satir);
                break;
            }
        }
        DurumuGuncelle();
    }

    private void GunlugeEkle(string metin)
    {
        string satir = metin.StartsWith('[') ? metin : $"[{DateTime.Now:HH:mm:ss}] {metin}";
        lstGunluk.Items.Add(satir);
        lstGunluk.TopIndex = lstGunluk.Items.Count - 1; // en son satırı göster
    }

    private void DurumuGuncelle()
    {
        bool calisiyor = _sunucu.Calisiyor;
        btnBaslat.Enabled = !calisiyor;
        btnDurdur.Enabled = calisiyor;
        nudPort.Enabled = chkYerelAg.Enabled = !calisiyor;
        lblDurum.Text = calisiyor ? $"Sunucu çalışıyor — port {_sunucu.Port}" : "Sunucu kapalı";
        lblDurum.ForeColor = calisiyor ? Color.SeaGreen : SystemColors.ControlText;
        lblIstemciSayisi.Text = $"Bağlı istemci: {lvIstemciler.Items.Count}";
    }
}

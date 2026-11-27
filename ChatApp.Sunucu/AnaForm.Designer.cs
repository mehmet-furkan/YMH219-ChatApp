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
        SuspendLayout();
        //
        // AnaForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 500);
        Name = "AnaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sohbet Sunucusu — YMH219";
        ResumeLayout(false);
    }

    #endregion
}

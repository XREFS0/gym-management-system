using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace GymManagement
{
    public static class ScreenViewsFactory
    {
        public static UserControl CreateScreen(int screenIndex, MainDashboardForm parent)
        {
            switch (screenIndex)
            {
                case 0: return new DashboardOverviewScreen(parent);
                case 1: return new MembersScreen(parent);
                case 2: return new MemberDetailsScreen();
                case 3: return new MembershipsScreen();
                case 4: return new SubscriptionsScreen();
                case 5: return new PaymentsScreen();
                case 6: return new AttendanceScreen();
                case 7: return new TrainersScreen();
                case 8: return new WorkoutPlansScreen();
                case 9: return new ExpensesScreen();
                case 10: return new ReportsScreen();
                case 11: return new StaffUsersScreen();
                case 12: return new EquipmentScreen();
                case 13: return new NotificationsScreen();
                case 14: return new SettingsScreen();
                case 15: return new BackupRestoreScreen();
                default: return new DashboardOverviewScreen(parent);
            }
        }

        public static Guna2DataGridView CreateStyledGrid()
        {
            var grid = new Guna2DataGridView();
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.FromArgb(24, 27, 36);
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(32, 36, 48);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(200, 205, 220);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 44;

            grid.DefaultCellStyle.BackColor = Color.FromArgb(24, 27, 36);
            grid.DefaultCellStyle.ForeColor = Color.White;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 52, 68);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 140, 60);
            grid.RowTemplate.Height = 42;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark;
            grid.DataError += (s, e) => { e.ThrowException = false; };

            // Luxury SaaS status badges and styling
            grid.CellPainting += (s, pe) =>
            {
                if (pe.RowIndex >= 0 && pe.ColumnIndex >= 0 && pe.Value != null)
                {
                    string val = pe.Value.ToString() ?? "";
                    string header = grid.Columns[pe.ColumnIndex].HeaderText?.ToLowerInvariant() ?? "";

                    if (header.Contains("status") || header.Contains("حالة") || val == "Active" || val == "Expired" || val == "Maintenance" || val == "Paid")
                    {
                        pe.PaintBackground(pe.CellBounds, true);

                        Color bgBadge;
                        Color textBadge;

                        switch (val.ToLowerInvariant())
                        {
                            case "active":
                            case "paid":
                            case "working":
                                bgBadge = Color.FromArgb(40, 34, 197, 94); // Soft Emerald Green
                                textBadge = Color.FromArgb(74, 222, 128);
                                break;

                            case "expired":
                            case "unpaid":
                            case "inactive":
                                bgBadge = Color.FromArgb(40, 239, 68, 68); // Soft Red
                                textBadge = Color.FromArgb(248, 113, 113);
                                break;

                            case "maintenance":
                            case "pending":
                                bgBadge = Color.FromArgb(40, 234, 179, 8); // Soft Amber/Yellow
                                textBadge = Color.FromArgb(250, 204, 21);
                                break;

                            default:
                                bgBadge = Color.FromArgb(30, 255, 255, 255);
                                textBadge = Color.FromArgb(200, 205, 220);
                                break;
                        }

                        pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        using (Font f = new Font("Segoe UI", 8F, FontStyle.Bold))
                        {
                            SizeF sz = pe.Graphics.MeasureString(val, f);
                            int bw = (int)sz.Width + 18;
                            int bh = 22;
                            int bx = pe.CellBounds.X + (pe.CellBounds.Width - bw) / 2;
                            int by = pe.CellBounds.Y + (pe.CellBounds.Height - bh) / 2;

                            using (GraphicsPath path = new GraphicsPath())
                            {
                                int rad = 10;
                                path.AddArc(bx, by, rad, rad, 180, 90);
                                path.AddArc(bx + bw - rad, by, rad, rad, 270, 90);
                                path.AddArc(bx + bw - rad, by + bh - rad, rad, rad, 0, 90);
                                path.AddArc(bx, by + bh - rad, rad, rad, 90, 90);
                                path.CloseFigure();

                                using (Brush b = new SolidBrush(bgBadge))
                                    pe.Graphics.FillPath(b, path);

                                using (Pen p = new Pen(Color.FromArgb(80, textBadge), 1))
                                    pe.Graphics.DrawPath(p, path);

                                TextRenderer.DrawText(pe.Graphics, val, f, new Rectangle(bx, by, bw, bh), textBadge, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                            }
                        }
                        pe.Handled = true;
                    }
                }
            };

            return grid;
        }
    }

    public static class IconHelper
    {
        public static Bitmap CreateVectorIcon(string iconType, Color color, int size = 20)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (Pen pen = new Pen(color, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                using (Brush brush = new SolidBrush(color))
                {
                    float pad = 2f;
                    float w = size - pad * 2;
                    float h = size - pad * 2;

                    switch (iconType.ToLowerInvariant())
                    {
                        case "dashboard":
                            // Modern 4-quadrant rounded dashboard grid
                            float qw = (w - 3f) / 2f;
                            float qh = (h - 3f) / 2f;
                            g.DrawRectangle(pen, pad, pad, qw, qh);
                            g.DrawRectangle(pen, pad + qw + 3f, pad, qw, qh);
                            g.DrawRectangle(pen, pad, pad + qh + 3f, qw, qh);
                            g.DrawRectangle(pen, pad + qw + 3f, pad + qh + 3f, qw, qh);
                            break;

                        case "members":
                        case "users":
                            // Two users silhouette
                            g.DrawEllipse(pen, pad + 2, pad + 1, 6, 6);
                            g.DrawArc(pen, pad, pad + 9, 10, 8, 180, 180);
                            g.DrawEllipse(pen, pad + 10, pad + 3, 5, 5);
                            g.DrawArc(pen, pad + 8, pad + 10, 8, 7, 200, 140);
                            break;

                        case "member_details":
                        case "user":
                            // Single user profile with ID card / outline
                            g.DrawEllipse(pen, pad + w / 2 - 4, pad + 1, 8, 8);
                            g.DrawArc(pen, pad + 2, pad + 10, w - 4, 8, 180, 180);
                            break;

                        case "memberships":
                        case "plans":
                            // Modern Badge / Card
                            g.DrawRectangle(pen, pad + 1, pad + 2, w - 2, h - 4);
                            g.DrawLine(pen, pad + 4, pad + 6, pad + w - 4, pad + 6);
                            g.DrawLine(pen, pad + 4, pad + 10, pad + w - 7, pad + 10);
                            break;

                        case "subscriptions":
                        case "repeat":
                            // Modern circular sync/repeat arrows
                            g.DrawArc(pen, pad + 1, pad + 1, w - 2, h - 2, 45, 230);
                            g.DrawLine(pen, pad + w - 4, pad + 4, pad + w - 1, pad + 8);
                            g.DrawLine(pen, pad + w - 6, pad + 9, pad + w - 1, pad + 8);
                            break;

                        case "payments":
                        case "credit_card":
                            // Sleek Credit Card
                            g.DrawRectangle(pen, pad, pad + 2, w, h - 4);
                            g.DrawLine(pen, pad, pad + 6, pad + w, pad + 6);
                            g.DrawLine(pen, pad + 3, pad + 11, pad + 7, pad + 11);
                            break;

                        case "attendance":
                        case "clock":
                            // Modern Clock / Timer
                            g.DrawEllipse(pen, pad + 1, pad + 1, w - 2, h - 2);
                            g.DrawLine(pen, pad + w / 2, pad + 4, pad + w / 2, pad + h / 2);
                            g.DrawLine(pen, pad + w / 2, pad + h / 2, pad + w / 2 + 4, pad + h / 2);
                            break;

                        case "trainers":
                        case "dumbbell":
                            // Modern Dumbbell
                            g.DrawLine(pen, pad + 3, pad + h / 2, pad + w - 3, pad + h / 2);
                            g.DrawLine(pen, pad + 3, pad + 3, pad + 3, pad + h - 3);
                            g.DrawLine(pen, pad + 1, pad + 5, pad + 1, pad + h - 5);
                            g.DrawLine(pen, pad + w - 3, pad + 3, pad + w - 3, pad + h - 3);
                            g.DrawLine(pen, pad + w - 1, pad + 5, pad + w - 1, pad + h - 5);
                            break;

                        case "workout":
                        case "clipboard":
                            // Modern Clipboard / Workout List
                            g.DrawRectangle(pen, pad + 2, pad + 3, w - 4, h - 4);
                            g.DrawLine(pen, pad + 5, pad + 2, pad + w - 5, pad + 2);
                            g.DrawLine(pen, pad + 5, pad + 7, pad + w - 5, pad + 7);
                            g.DrawLine(pen, pad + 5, pad + 10, pad + w - 7, pad + 10);
                            break;

                        case "expenses":
                        case "wallet":
                            // Money / Wallet
                            g.DrawRectangle(pen, pad, pad + 3, w, h - 5);
                            g.DrawArc(pen, pad + w - 5, pad + 6, 4, 4, 0, 360);
                            break;

                        case "reports":
                        case "chart":
                            // Modern Trending Analytics Bar/Line
                            g.DrawLine(pen, pad + 1, pad + h - 1, pad + w - 1, pad + h - 1);
                            g.DrawLine(pen, pad + 3, pad + h - 5, pad + 3, pad + h - 1);
                            g.DrawLine(pen, pad + 7, pad + h - 9, pad + 7, pad + h - 1);
                            g.DrawLine(pen, pad + 11, pad + h - 13, pad + 11, pad + h - 1);
                            g.DrawLine(pen, pad + 14, pad + 3, pad + 9, pad + 7);
                            break;

                        case "staff":
                        case "shield":
                            // Modern Security Shield
                            PointF[] shieldPts = new PointF[] {
                                new PointF(pad + w / 2, pad + 1),
                                new PointF(pad + w - 2, pad + 3),
                                new PointF(pad + w - 3, pad + 10),
                                new PointF(pad + w / 2, pad + h - 1),
                                new PointF(pad + 3, pad + 10),
                                new PointF(pad + 2, pad + 3)
                            };
                            g.DrawPolygon(pen, shieldPts);
                            break;

                        case "equipment":
                        case "gear":
                            // Sleek Cog / Gear
                            g.DrawEllipse(pen, pad + 3, pad + 3, w - 6, h - 6);
                            g.DrawEllipse(pen, pad + w / 2 - 2, pad + h / 2 - 2, 4, 4);
                            g.DrawLine(pen, pad + w / 2, pad, pad + w / 2, pad + 3);
                            g.DrawLine(pen, pad + w / 2, pad + h - 3, pad + w / 2, pad + h);
                            g.DrawLine(pen, pad, pad + h / 2, pad + 3, pad + h / 2);
                            g.DrawLine(pen, pad + w - 3, pad + h / 2, pad + w, pad + h / 2);
                            break;

                        case "notifications":
                        case "bell":
                            // Modern Notification Bell
                            g.DrawArc(pen, pad + 3, pad + 3, w - 6, 9, 180, 180);
                            g.DrawLine(pen, pad + 2, pad + 11, pad + w - 2, pad + 11);
                            g.DrawArc(pen, pad + w / 2 - 2, pad + 12, 4, 3, 0, 180);
                            break;

                        case "settings":
                        case "sliders":
                            // Modern Tuning Sliders / Tool
                            g.DrawLine(pen, pad + 2, pad + 4, pad + w - 2, pad + 4);
                            g.DrawEllipse(pen, pad + 5, pad + 2, 4, 4);
                            g.DrawLine(pen, pad + 2, pad + 11, pad + w - 2, pad + 11);
                            g.DrawEllipse(pen, pad + w - 8, pad + 9, 4, 4);
                            break;

                        case "backup":
                        case "database":
                            // Modern Database / Disk Cylinders
                            g.DrawEllipse(pen, pad + 2, pad + 1, w - 4, 5);
                            g.DrawLine(pen, pad + 2, pad + 3, pad + 2, pad + 11);
                            g.DrawLine(pen, pad + w - 2, pad + 3, pad + w - 2, pad + 11);
                            g.DrawArc(pen, pad + 2, pad + 9, w - 4, 5, 0, 180);
                            g.DrawArc(pen, pad + 2, pad + 5, w - 4, 5, 0, 180);
                            break;

                        case "logout":
                            // Sleek Logout Door / Arrow
                            g.DrawRectangle(pen, pad + 1, pad + 1, w - 7, h - 2);
                            g.DrawLine(pen, pad + 6, pad + h / 2, pad + w - 1, pad + h / 2);
                            g.DrawLine(pen, pad + w - 4, pad + h / 2 - 3, pad + w - 1, pad + h / 2);
                            g.DrawLine(pen, pad + w - 4, pad + h / 2 + 3, pad + w - 1, pad + h / 2);
                            break;

                        default:
                            g.DrawEllipse(pen, pad + 2, pad + 2, w - 4, h - 4);
                            break;
                    }
                }
            }
            return bmp;
        }
    }

    public static class AppResources
    {
        private static readonly System.Reflection.Assembly Asm = typeof(AppResources).Assembly;

        public static Image? GetImage(string name)
        {
            try
            {
                // Try direct match or search among manifest names
                string[] names = Asm.GetManifestResourceNames();
                foreach (string res in names)
                {
                    if (res.EndsWith(name, StringComparison.OrdinalIgnoreCase))
                    {
                        using Stream? stream = Asm.GetManifestResourceStream(res);
                        if (stream != null)
                        {
                            return Image.FromStream(stream);
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        public static Icon? GetIcon(string name)
        {
            try
            {
                string[] names = Asm.GetManifestResourceNames();
                foreach (string res in names)
                {
                    if (res.EndsWith(name, StringComparison.OrdinalIgnoreCase))
                    {
                        using Stream? stream = Asm.GetManifestResourceStream(res);
                        if (stream != null)
                        {
                            return new Icon(stream);
                        }
                    }
                }
            }
            catch { }
            return null;
        }
    }

    // ==========================================
    // THERMAL RECEIPT PRINTER & PREVIEW
    // ==========================================
    public static class ReceiptPrinter
    {
        public static void PrintOrPreview(string receiptNo, string memberName, string membershipPlan, decimal amount, string payMethod, DateTime payDate, decimal remaining)
        {
            try
            {
                bool hasPrinters = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0;
                if (hasPrinters)
                {
                    using (var doc = new System.Drawing.Printing.PrintDocument())
                    {
                        doc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Receipt", 315, 600); // 80mm thermal width
                        doc.PrintPage += (s, e) =>
                        {
                            Graphics g = e.Graphics!;
                            g.SmoothingMode = SmoothingMode.AntiAlias;

                            int y = 20;
                            using (Font titleF = new Font("Segoe UI", 16F, FontStyle.Bold))
                            using (Font subF = new Font("Segoe UI", 8.5F))
                            using (Font boldF = new Font("Segoe UI", 9.5F, FontStyle.Bold))
                            using (Font regF = new Font("Segoe UI", 9F))
                            using (Brush textB = new SolidBrush(Color.Black))
                            using (Pen linePen = new Pen(Color.Gray, 1) { DashStyle = DashStyle.Dash })
                            {
                                // Header
                                StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center };
                                g.DrawString("⚡ FITFLOW GYM ⚡", titleF, textB, 150, y, centerFormat);
                                y += 30;
                                g.DrawString("Professional Fitness & Health Club", subF, textB, 150, y, centerFormat);
                                y += 18;
                                g.DrawString("Phone: +20 100 123 4567 | Cairo, Egypt", subF, textB, 150, y, centerFormat);
                                y += 25;

                                g.DrawLine(linePen, 15, y, 285, y);
                                y += 12;

                                // Receipt Details
                                g.DrawString($"RECEIPT: {receiptNo}", boldF, textB, 20, y);
                                y += 20;
                                g.DrawString($"DATE: {payDate:yyyy-MM-dd HH:mm}", regF, textB, 20, y);
                                y += 20;
                                g.DrawString($"MEMBER: {memberName}", boldF, textB, 20, y);
                                y += 20;
                                g.DrawString($"PLAN: {membershipPlan}", regF, textB, 20, y);
                                y += 24;

                                g.DrawLine(linePen, 15, y, 285, y);
                                y += 14;

                                // Amount Block
                                g.DrawString("PAYMENT METHOD:", regF, textB, 20, y);
                                g.DrawString(payMethod, boldF, textB, 280, y, new StringFormat { Alignment = StringAlignment.Far });
                                y += 24;

                                using (Font priceF = new Font("Segoe UI", 13F, FontStyle.Bold))
                                {
                                    g.DrawString("AMOUNT PAID:", boldF, textB, 20, y);
                                    g.DrawString($"{amount:N2} EGP", priceF, textB, 280, y - 2, new StringFormat { Alignment = StringAlignment.Far });
                                }
                                y += 28;

                                if (remaining > 0)
                                {
                                    g.DrawString("REMAINING DUE:", boldF, textB, 20, y);
                                    g.DrawString($"{remaining:N2} EGP", boldF, textB, 280, y, new StringFormat { Alignment = StringAlignment.Far });
                                    y += 24;
                                }

                                g.DrawLine(linePen, 15, y, 285, y);
                                y += 18;

                                // Barcode representation
                                g.DrawString($"* {receiptNo} *", new Font("Consolas", 11F, FontStyle.Bold), textB, 150, y, centerFormat);
                                y += 26;
                                g.DrawString("Thank you for training with us!", subF, textB, 150, y, centerFormat);
                                y += 16;
                                g.DrawString("No refund after 14 days of activation", new Font("Segoe UI", 7.5F, FontStyle.Italic), textB, 150, y, centerFormat);
                            }
                        };

                        using (var preview = new PrintPreviewDialog())
                        {
                            preview.Document = doc;
                            preview.Width = 460;
                            preview.Height = 650;
                            preview.StartPosition = FormStartPosition.CenterScreen;
                            preview.Text = $"إيصال الدفع - {receiptNo}";
                            preview.ShowDialog();
                            return;
                        }
                    }
                }
            }
            catch { }

            // Thermal receipt HTML fallback
            try
            {
                string html = $@"<!DOCTYPE html><html><head><meta charset='utf-8'><title>Receipt {receiptNo}</title>
<style>
body {{ font-family: 'Segoe UI', Tahoma, sans-serif; width: 300px; margin: 20px auto; padding: 15px; border: 1px solid #ccc; }}
.center {{ text-align: center; }}
.line {{ border-top: 1px dashed #777; margin: 10px 0; }}
.row {{ display: flex; justify-content: space-between; margin: 4px 0; font-size: 13px; }}
.bold {{ font-weight: bold; }}
.title {{ font-size: 18px; font-weight: bold; margin-bottom: 4px; }}
.barcode {{ font-family: monospace; font-size: 14px; margin-top: 10px; }}
@media print {{ body {{ border: none; margin: 0; width: 100%; }} .no-print {{ display: none; }} }}
</style></head><body>
<div class='no-print' style='text-align:center;margin-bottom:10px;'>
<button onclick='window.print()' style='background:#ff5f15;color:#fff;border:none;padding:8px 16px;border-radius:4px;cursor:pointer;font-weight:bold;'>🖨️ طباعة الإيصال</button>
</div>
<div class='center'>
<div class='title'>⚡ FITFLOW GYM ⚡</div>
<div style='font-size:11px;color:#555;'>Professional Fitness & Health Club</div>
<div style='font-size:11px;color:#555;'>Phone: +20 100 123 4567 | Cairo, Egypt</div>
</div>
<div class='line'></div>
<div class='row'><span class='bold'>RECEIPT:</span><span>{receiptNo}</span></div>
<div class='row'><span>DATE:</span><span>{payDate:yyyy-MM-dd HH:mm}</span></div>
<div class='row'><span class='bold'>MEMBER:</span><span>{memberName}</span></div>
<div class='row'><span>PLAN:</span><span>{membershipPlan}</span></div>
<div class='line'></div>
<div class='row'><span>METHOD:</span><span class='bold'>{payMethod}</span></div>
<div class='row' style='font-size:15px;'><span class='bold'>AMOUNT PAID:</span><span class='bold'>{amount:N2} EGP</span></div>
{(remaining > 0 ? $"<div class='row'><span class='bold'>REMAINING:</span><span>{remaining:N2} EGP</span></div>" : "")}
<div class='line'></div>
<div class='center barcode'>* {receiptNo} *</div>
<div class='center' style='font-size:11px;margin-top:6px;'>Thank you for training with us!</div>
<script>window.onload = function() {{ window.print(); }}</script>
</body></html>";

                string tempFile = Path.Combine(Path.GetTempPath(), $"Receipt_{receiptNo}.html");
                File.WriteAllText(tempFile, html, System.Text.Encoding.UTF8);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFile) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"تعذر إعداد الإيصال: {ex.Message}", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    // ==========================================
    // EXPORT HELPER (EXCEL / CSV / PDF)
    // ==========================================
    public static class ExportHelper
    {
        public static void ExportToExcel(DataTable dt, string defaultFileName)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات متاحة للتصدير حالياً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel CSV (*.csv)|*.csv|All Files (*.*)|*.*";
                sfd.FileName = $"{defaultFileName}_{DateTime.Now:yyyyMMdd_HHmm}.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sb = new System.Text.StringBuilder();

                        // Header with UTF8 BOM for Excel Arabic support
                        string[] columnNames = dt.Columns.Cast<DataColumn>().Select(c => $"\"{c.ColumnName.Replace("\"", "\"\"")}\"").ToArray();
                        sb.AppendLine(string.Join(",", columnNames));

                        foreach (DataRow row in dt.Rows)
                        {
                            string[] fields = row.ItemArray.Select(field =>
                                field == null ? "\"\"" : $"\"{field.ToString()?.Replace("\"", "\"\"")}\"").ToArray();
                            sb.AppendLine(string.Join(",", fields));
                        }

                        // Write with UTF-8 with BOM so Excel opens Arabic correctly
                        File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8);
                        var open = MessageBox.Show("تم تصدير البيانات إلى Excel بنجاح!\nهل ترغب في فتح الملف الآن؟", "تم التصدير", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (open == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"حدث خطأ أثناء التصدير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public static void ExportToPDF(DataTable dt, string title)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات متاحة للطباعة والتصدير.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Check if any printers exist before trying to create PrintDocument
                bool hasPrinters = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0;

                if (hasPrinters)
                {
                    using (var doc = new System.Drawing.Printing.PrintDocument())
                    {
                        doc.DefaultPageSettings.Landscape = true;
                        doc.PrintPage += (s, e) =>
                        {
                            Graphics g = e.Graphics!;
                            g.SmoothingMode = SmoothingMode.AntiAlias;

                            int y = 30;
                            using (Font hFont = new Font("Segoe UI", 16F, FontStyle.Bold))
                            using (Font dateF = new Font("Segoe UI", 9F))
                            using (Font thFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                            using (Font tdFont = new Font("Segoe UI", 8.5F))
                            using (Brush textB = new SolidBrush(Color.FromArgb(20, 25, 35)))
                            using (Brush headerBg = new SolidBrush(Color.FromArgb(245, 245, 250)))
                            using (Pen borderPen = new Pen(Color.FromArgb(220, 225, 235)))
                            {
                                // Header title
                                g.DrawString($"⚡ FitFlow Gym Management - {title}", hFont, textB, 30, y);
                                g.DrawString($"Export Date: {DateTime.Now:yyyy-MM-dd HH:mm} | Total Records: {dt.Rows.Count}", dateF, textB, 30, y + 30);
                                y += 65;

                                int colCount = dt.Columns.Count;
                                int tableWidth = e.MarginBounds.Width + 100;
                                int colWidth = tableWidth / Math.Max(1, colCount);

                                // Draw Header
                                g.FillRectangle(headerBg, 30, y, tableWidth, 32);
                                g.DrawRectangle(borderPen, 30, y, tableWidth, 32);

                                for (int c = 0; c < colCount; c++)
                                {
                                    g.DrawString(dt.Columns[c].ColumnName, thFont, textB, 35 + c * colWidth, y + 7);
                                }
                                y += 32;

                                // Draw Rows (capped to fit page)
                                int maxRows = Math.Min(dt.Rows.Count, 25);
                                for (int r = 0; r < maxRows; r++)
                                {
                                    if (r % 2 == 1)
                                        g.FillRectangle(new SolidBrush(Color.FromArgb(250, 250, 252)), 30, y, tableWidth, 26);

                                    g.DrawRectangle(borderPen, 30, y, tableWidth, 26);

                                    for (int c = 0; c < colCount; c++)
                                    {
                                        string cellVal = dt.Rows[r][c]?.ToString() ?? "";
                                        if (cellVal.Length > 25) cellVal = cellVal.Substring(0, 22) + "...";
                                        g.DrawString(cellVal, tdFont, textB, 35 + c * colWidth, y + 5);
                                    }
                                    y += 26;
                                }

                                if (dt.Rows.Count > maxRows)
                                {
                                    g.DrawString($"... And {dt.Rows.Count - maxRows} more records. Export to CSV/Excel for full dataset.", dateF, Brushes.Gray, 30, y + 10);
                                }
                            }
                        };

                        using (var preview = new PrintPreviewDialog())
                        {
                            preview.Document = doc;
                            preview.Width = 950;
                            preview.Height = 700;
                            preview.StartPosition = FormStartPosition.CenterScreen;
                            preview.Text = $"معاينة وتصدير PDF / طباعة - {title}";
                            preview.ShowDialog();
                            return;
                        }
                    }
                }
            }
            catch { }

            // Fallback for systems without printer drivers (e.g. saves beautiful printable HTML report that prints to PDF instantly in browser)
            ExportHtmlPrintableReport(dt, title);
        }

        public static void ExportHtmlPrintableReport(DataTable dt, string title)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "HTML Printable Report (*.html)|*.html|All Files (*.*)|*.*";
                    sfd.FileName = $"{title.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmm}.html";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        var sb = new System.Text.StringBuilder();
                        sb.AppendLine("<!DOCTYPE html><html dir='rtl' lang='ar'><head><meta charset='utf-8'>");
                        sb.AppendLine($"<title>{title}</title>");
                        sb.AppendLine("<style>");
                        sb.AppendLine("body { font-family: 'Segoe UI', Tahoma, Arial, sans-serif; margin: 30px; background: #fff; color: #222; }");
                        sb.AppendLine(".header { border-bottom: 2px solid #ff5f15; padding-bottom: 15px; margin-bottom: 20px; }");
                        sb.AppendLine(".title { font-size: 24px; font-weight: bold; color: #111; }");
                        sb.AppendLine(".sub { color: #666; font-size: 13px; margin-top: 5px; }");
                        sb.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 15px; font-size: 13px; }");
                        sb.AppendLine("th { background: #1e222d; color: #fff; text-align: right; padding: 10px; border: 1px solid #ddd; }");
                        sb.AppendLine("td { padding: 9px 10px; border: 1px solid #ddd; }");
                        sb.AppendLine("tr:nth-child(even) { background: #f9fafb; }");
                        sb.AppendLine("@media print { .no-print { display: none; } body { margin: 10px; } }");
                        sb.AppendLine("</style></head><body>");

                        sb.AppendLine("<div class='no-print' style='margin-bottom:15px;'>");
                        sb.AppendLine("<button onclick='window.print()' style='background:#ff5f15;color:#fff;border:none;padding:10px 22px;border-radius:6px;cursor:pointer;font-weight:bold;font-size:14px;'>🖨️ طباعة أو حفظ كـ PDF (Print / Save as PDF)</button>");
                        sb.AppendLine("</div>");

                        sb.AppendLine("<div class='header'>");
                        sb.AppendLine($"<div class='title'>⚡ FitFlow Gym - {title}</div>");
                        sb.AppendLine($"<div class='sub'>تاريخ التصدير: {DateTime.Now:yyyy-MM-dd HH:mm} | إجمالي السجلات: {dt.Rows.Count}</div>");
                        sb.AppendLine("</div>");

                        sb.AppendLine("<table><thead><tr>");
                        foreach (DataColumn col in dt.Columns)
                        {
                            sb.AppendLine($"<th>{col.ColumnName}</th>");
                        }
                        sb.AppendLine("</tr></thead><tbody>");

                        foreach (DataRow row in dt.Rows)
                        {
                            sb.AppendLine("<tr>");
                            foreach (var item in row.ItemArray)
                            {
                                sb.AppendLine($"<td>{item?.ToString()}</td>");
                            }
                            sb.AppendLine("</tr>");
                        }

                        sb.AppendLine("</tbody></table>");
                        sb.AppendLine("<script>window.onload = function() { window.print(); }</script>");
                        sb.AppendLine("</body></html>");

                        File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8);
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء إعداد التقرير: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // ==========================================
    // PRIVACY & PHONE NUMBER MASKING HELPER
    // ==========================================
    public static class PhoneMaskHelper
    {
        public static string Mask(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return "----";
            string clean = phone.Trim();
            if (clean.Length <= 4) return clean + "****";
            // e.g. "0100 123 4567" or "0101234567" -> Keep first 4-5 digits, mask middle with ****
            if (clean.Length >= 9)
            {
                // show prefix (first 4 digits) and suffix (last 2 digits), mask middle: e.g. 0101****89
                return clean.Substring(0, 4) + "****" + clean.Substring(clean.Length - 2);
            }
            return clean.Substring(0, Math.Min(3, clean.Length)) + "****";
        }
    }

    // ==========================================
    // 1. DASHBOARD OVERVIEW SCREEN (Matches the FitFlow UI image)
    // ==========================================
    public class DashboardOverviewScreen : UserControl
    {
        private MainDashboardForm mainForm;

        public DashboardOverviewScreen(MainDashboardForm form)
        {
            this.mainForm = form;
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.AutoScroll = true;
            InitializeDashboardUI();
        }

        private void InitializeDashboardUI()
        {
            // Gather stats from SQLite
            long totalMembers = 0;
            long activeSubs = 0;
            long expiredSubs = 0;
            decimal totalRevenue = 0;
            long todayAttendance = 0;

            try
            {
                totalMembers = Convert.ToInt64(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM Members") ?? 0);
                activeSubs = Convert.ToInt64(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM Subscriptions WHERE Status = 'Active'") ?? 0);
                expiredSubs = Convert.ToInt64(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM Subscriptions WHERE Status = 'Expired'") ?? 0);
                totalRevenue = Convert.ToDecimal(DatabaseHelper.ExecuteScalar("SELECT COALESCE(SUM(Amount), 0) FROM Payments") ?? 0);
                todayAttendance = Convert.ToInt64(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM Attendance WHERE date(CheckIn) = date('now')") ?? 0);
            }
            catch { }

            // 1. Top Hero Promo Banner (Like the hero shirtless fitness model in FitFlow)
            Guna2Panel heroCard = new Guna2Panel
            {
                Location = new Point(25, 20),
                Size = new Size(680, 220),
                BorderRadius = 20,
                FillColor = Color.FromArgb(28, 32, 44),
                BackColor = Color.Transparent
            };

            Label lblDate = new Label
            {
                Text = DateTime.Now.ToString("MMMM dd, yyyy").ToUpper(),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(140, 145, 160),
                Location = new Point(30, 25),
                AutoSize = true
            };

            Label lblGreet = new Label
            {
                Text = "Welcome Back, Captain!",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 60),
                Location = new Point(30, 48),
                AutoSize = true
            };

            Label lblPromoHeading = new Label
            {
                Text = "Ready to boost your\nclub's performance today?",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 75),
                Size = new Size(380, 70)
            };

            Guna2Button btnHeroAction = new Guna2Button
            {
                Text = "Add New Member",
                BorderRadius = 12,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 155),
                Size = new Size(165, 42),
                Cursor = Cursors.Hand
            };
            btnHeroAction.Click += (s, e) => mainForm.NavigateTo(1); // Go to Members

            PictureBox pbHeroModel = new PictureBox
            {
                Location = new Point(400, 5),
                Size = new Size(270, 210),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            pbHeroModel.Image = AppResources.GetImage("fit_hero.jpg");

            heroCard.Controls.AddRange(new Control[] { lblDate, lblGreet, lblPromoHeading, btnHeroAction, pbHeroModel });
            this.Controls.Add(heroCard);

            // 2. Member Activity Donut Card (Right side top)
            Guna2Panel pnlActivity = new Guna2Panel
            {
                Location = new Point(725, 20),
                Size = new Size(340, 220),
                BorderRadius = 20,
                FillColor = Color.FromArgb(28, 32, 44),
                BackColor = Color.Transparent
            };

            Label lblActTitle = new Label
            {
                Text = "Member Activity & Shifts",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 18),
                AutoSize = true
            };

            // Custom Painted Visual Chart representing Member Shifts
            PictureBox pbDonutChart = new PictureBox
            {
                Location = new Point(20, 50),
                Size = new Size(300, 150),
                BackColor = Color.Transparent
            };
            pbDonutChart.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Orange slice
                using (Brush b1 = new SolidBrush(Color.FromArgb(255, 95, 21)))
                    pe.Graphics.FillPie(b1, 20, 10, 110, 110, 0, 160);
                // Yellow slice
                using (Brush b2 = new SolidBrush(Color.FromArgb(245, 175, 40)))
                    pe.Graphics.FillPie(b2, 70, 15, 90, 90, 160, 110);
                // Teal slice
                using (Brush b3 = new SolidBrush(Color.FromArgb(40, 190, 140)))
                    pe.Graphics.FillPie(b3, 50, 45, 70, 70, 270, 90);

                // Legend
                using (Font f = new Font("Segoe UI", 8F))
                using (Brush textB = new SolidBrush(Color.FromArgb(180, 185, 200)))
                {
                    pe.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(255, 95, 21)), 180, 25, 8, 8);
                    pe.Graphics.DrawString("Morning (08:00 - 12:00)", f, textB, 195, 22);

                    pe.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(245, 175, 40)), 180, 55, 8, 8);
                    pe.Graphics.DrawString("Afternoon (12:00 - 17:00)", f, textB, 195, 52);

                    pe.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(40, 190, 140)), 180, 85, 8, 8);
                    pe.Graphics.DrawString("Evening (17:00 - 23:00)", f, textB, 195, 82);
                }
            };

            pnlActivity.Controls.AddRange(new Control[] { lblActTitle, pbDonutChart });
            this.Controls.Add(pnlActivity);

            // 3. Stat Cards Row (Current Members, Active Subs, Today Attendance, Total Revenue)
            int startX = 25;
            int cardW = 245;
            int gap = 20;

            CreateStatCard(startX, 260, cardW, "Total Members", totalMembers.ToString("N0"), "+12% this month", Color.FromArgb(40, 190, 140), "members");
            CreateStatCard(startX + (cardW + gap), 260, cardW, "Active Subscriptions", activeSubs.ToString("N0"), $"{expiredSubs} Expired", Color.FromArgb(255, 140, 60), "subscriptions");
            CreateStatCard(startX + (cardW + gap) * 2, 260, cardW, "Today Attendance", todayAttendance.ToString("N0"), "+5 in last hour", Color.FromArgb(60, 160, 255), "attendance");
            CreateStatCard(startX + (cardW + gap) * 3, 260, cardW, "Total Revenue", $"{totalRevenue:C0}".Replace("$", "EGP "), "+18.4% growth", Color.FromArgb(230, 80, 140), "payments");

            // 4. Membership Status Line Chart Preview Card (Bottom Left)
            Guna2Panel pnlChart = new Guna2Panel
            {
                Location = new Point(25, 395),
                Size = new Size(680, 290),
                BorderRadius = 20,
                FillColor = Color.FromArgb(28, 32, 44),
                BackColor = Color.Transparent
            };

            Label lblChartTitle = new Label
            {
                Text = "Membership & Attendance Analytics (Monthly Trend)",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(25, 20),
                AutoSize = true
            };

            PictureBox pbLineChart = new PictureBox
            {
                Location = new Point(25, 60),
                Size = new Size(630, 205),
                BackColor = Color.Transparent
            };
            pbLineChart.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // Draw Grid lines
                using (Pen gridPen = new Pen(Color.FromArgb(40, 45, 60), 1) { DashStyle = DashStyle.Dash })
                {
                    pe.Graphics.DrawLine(gridPen, 40, 30, 610, 30);
                    pe.Graphics.DrawLine(gridPen, 40, 80, 610, 80);
                    pe.Graphics.DrawLine(gridPen, 40, 130, 610, 130);
                    pe.Graphics.DrawLine(gridPen, 40, 170, 610, 170);
                }

                // Smooth Curves
                Point[] pts1 = new Point[] {
                    new Point(40, 140), new Point(110, 120), new Point(190, 70),
                    new Point(280, 110), new Point(370, 45), new Point(460, 90),
                    new Point(540, 40), new Point(610, 60)
                };
                Point[] pts2 = new Point[] {
                    new Point(40, 160), new Point(110, 140), new Point(190, 110),
                    new Point(280, 130), new Point(370, 90), new Point(460, 120),
                    new Point(540, 80), new Point(610, 110)
                };

                using (Pen p1 = new Pen(Color.FromArgb(255, 95, 21), 3))
                    pe.Graphics.DrawCurve(p1, pts1, 0.5f);

                using (Pen p2 = new Pen(Color.FromArgb(40, 190, 140), 2.5f))
                    pe.Graphics.DrawCurve(p2, pts2, 0.5f);

                // Months axis labels
                string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug" };
                using (Font f = new Font("Segoe UI", 8F))
                using (Brush b = new SolidBrush(Color.FromArgb(140, 145, 160)))
                {
                    for (int m = 0; m < months.Length; m++)
                    {
                        pe.Graphics.DrawString(months[m], f, b, 35 + m * 80, 185);
                    }
                }
            };

            pnlChart.Controls.AddRange(new Control[] { lblChartTitle, pbLineChart });
            this.Controls.Add(pnlChart);

            // 5. Membership Target Gauge Card (Bottom Right)
            Guna2Panel pnlGauge = new Guna2Panel
            {
                Location = new Point(725, 395),
                Size = new Size(340, 290),
                BorderRadius = 20,
                FillColor = Color.FromArgb(28, 32, 44),
                BackColor = Color.Transparent
            };

            Label lblTargetTitle = new Label
            {
                Text = "Monthly Gym Target",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            PictureBox pbGauge = new PictureBox
            {
                Location = new Point(20, 55),
                Size = new Size(300, 215),
                BackColor = Color.Transparent
            };
            pbGauge.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Arc Background
                using (Pen bgArc = new Pen(Color.FromArgb(42, 48, 64), 18))
                {
                    bgArc.StartCap = LineCap.Round;
                    bgArc.EndCap = LineCap.Round;
                    pe.Graphics.DrawArc(bgArc, 40, 20, 220, 220, 180, 180);
                }

                // Active Arc Gradient
                using (Pen valArc = new Pen(Color.FromArgb(255, 95, 21), 18))
                {
                    valArc.StartCap = LineCap.Round;
                    valArc.EndCap = LineCap.Round;
                    pe.Graphics.DrawArc(valArc, 40, 20, 220, 220, 180, 145);
                }

                // Target center percentage
                using (Font bigF = new Font("Segoe UI", 24F, FontStyle.Bold))
                using (Brush b = new SolidBrush(Color.White))
                {
                    pe.Graphics.DrawString("81.4%", bigF, b, 100, 80);
                }

                using (Font subF = new Font("Segoe UI", 8.5F))
                using (Brush sb = new SolidBrush(Color.FromArgb(40, 190, 140)))
                {
                    pe.Graphics.DrawString("▲ +8.2% higher than target", subF, sb, 80, 125);
                }

                using (Font lblF = new Font("Segoe UI", 9F))
                using (Brush lb = new SolidBrush(Color.FromArgb(160, 165, 180)))
                {
                    pe.Graphics.DrawString("Target: 100 Members", lblF, lb, 20, 165);
                    pe.Graphics.DrawString("Achieved: 82 Members", lblF, lb, 165, 165);
                }
            };

            pnlGauge.Controls.AddRange(new Control[] { lblTargetTitle, pbGauge });
            this.Controls.Add(pnlGauge);
        }

        private void CreateStatCard(int x, int y, int w, string title, string val, string sub, Color accent, string iconType)
        {
            Guna2Panel card = new Guna2Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, 115),
                BorderRadius = 16,
                FillColor = Color.FromArgb(28, 32, 44),
                BackColor = Color.Transparent
            };

            // Modern circular badge with vector icon
            Guna2Panel iconBadge = new Guna2Panel
            {
                Location = new Point(w - 52, 16),
                Size = new Size(36, 36),
                BorderRadius = 10,
                FillColor = Color.FromArgb(30, accent.R, accent.G, accent.B),
                BackColor = Color.Transparent
            };

            PictureBox pbIco = new PictureBox
            {
                Location = new Point(8, 8),
                Size = new Size(20, 20),
                Image = IconHelper.CreateVectorIcon(iconType, accent, 20),
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent
            };
            iconBadge.Controls.Add(pbIco);

            Label lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(145, 150, 165),
                Location = new Point(18, 16),
                AutoSize = true
            };

            Label lblV = new Label
            {
                Text = val,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(16, 40),
                AutoSize = true
            };

            Label lblS = new Label
            {
                Text = sub,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = accent,
                Location = new Point(18, 82),
                AutoSize = true
            };

            card.BorderThickness = 1;
            card.BorderColor = Color.FromArgb(40, 45, 60);

            // Micro-interaction on hover: glowing border matching card's accent color
            card.MouseEnter += (s, e) => {
                card.BorderColor = accent;
                card.FillColor = Color.FromArgb(34, 38, 52);
            };
            card.MouseLeave += (s, e) => {
                card.BorderColor = Color.FromArgb(40, 45, 60);
                card.FillColor = Color.FromArgb(28, 32, 44);
            };

            card.Controls.AddRange(new Control[] { lblT, lblV, lblS, iconBadge });
            this.Controls.Add(card);
        }
    }

    // ==========================================
    // 2. MEMBERS SCREEN (CRUD + Search + Card View)
    // ==========================================
    public class MembersScreen : UserControl, ISearchable
    {
        private MainDashboardForm mainForm;
        private Guna2DataGridView grid;
        private DataTable membersDt;

        public MembersScreen(MainDashboardForm form)
        {
            this.mainForm = form;
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "👥  Members Management",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnAdd = new Guna2Button
            {
                Text = "+ Add New Member",
                BorderRadius = 10,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(900, 20),
                Size = new Size(160, 40),
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => ShowAddMemberModal();

            Guna2Button btnEdit = new Guna2Button
            {
                Text = "✏️ Edit Member (تعديل)",
                BorderRadius = 10,
                FillColor = Color.FromArgb(40, 50, 70),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 160, 255),
                Location = new Point(715, 20),
                Size = new Size(165, 40),
                Cursor = Cursors.Hand
            };
            btnEdit.HoverState.FillColor = Color.FromArgb(60, 160, 255);
            btnEdit.HoverState.ForeColor = Color.White;
            btnEdit.Click += (s, e) => ShowEditMemberModal();

            Guna2Button btnDelete = new Guna2Button
            {
                Text = "🗑️ Delete",
                BorderRadius = 10,
                FillColor = Color.FromArgb(45, 30, 35),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(595, 20),
                Size = new Size(110, 40),
                Cursor = Cursors.Hand
            };
            btnDelete.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnDelete.HoverState.ForeColor = Color.White;
            btnDelete.Click += (s, e) => DeleteSelectedMember();
            Guna2Button btnExportExcel = new Guna2Button
            {
                Text = "📊 Excel",
                BorderRadius = 10,
                FillColor = Color.FromArgb(30, 48, 40),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 222, 128),
                Location = new Point(495, 20),
                Size = new Size(90, 40),
                Cursor = Cursors.Hand
            };
            btnExportExcel.HoverState.FillColor = Color.FromArgb(34, 197, 94);
            btnExportExcel.HoverState.ForeColor = Color.White;
            btnExportExcel.Click += (s, e) => ExportHelper.ExportToExcel(membersDt, "Gym_Members_Report");

            Guna2Button btnExportPDF = new Guna2Button
            {
                Text = "📑 PDF",
                BorderRadius = 10,
                FillColor = Color.FromArgb(48, 32, 42),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(248, 113, 113),
                Location = new Point(400, 20),
                Size = new Size(85, 40),
                Cursor = Cursors.Hand
            };
            btnExportPDF.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnExportPDF.HoverState.ForeColor = Color.White;
            btnExportPDF.Click += (s, e) => ExportHelper.ExportToPDF(membersDt, "Gym Members Directory");

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnAdd, btnEdit, btnDelete, btnExportExcel, btnExportPDF, gridContainer });
        }

        public void LoadData()
        {
            try
            {
                membersDt = DatabaseHelper.ExecuteQuery(@"
                    SELECT m.Id AS [ID], 
                           'MEM-' || printf('%04d', m.Id) AS [Member Code],
                           m.FullName AS [Full Name], 
                           SUBSTR(m.Phone, 1, 4) || '****' || SUBSTR(m.Phone, -2) AS [Phone Number], 
                           m.Email AS [Email], 
                           m.Gender AS [Gender], 
                           m.JoinDate AS [Joined Date],
                           COALESCE(s.Status, 'No Plan') AS [Status]
                    FROM Members m
                    LEFT JOIN Subscriptions s ON m.Id = s.MemberId
                    GROUP BY m.Id
                    ORDER BY m.Id DESC;
                ");
                grid.DataSource = membersDt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ApplySearch(string term)
        {
            if (membersDt == null) return;
            if (string.IsNullOrWhiteSpace(term))
            {
                membersDt.DefaultView.RowFilter = "";
            }
            else
            {
                string safe = term.Replace("'", "''").Trim();
                if (int.TryParse(safe, out int idVal))
                {
                    membersDt.DefaultView.RowFilter = $"[ID] = {idVal} OR [Member Code] LIKE '%{safe}%' OR [Full Name] LIKE '%{safe}%' OR [Phone Number] LIKE '%{safe}%'";
                }
                else
                {
                    membersDt.DefaultView.RowFilter = $"[Member Code] LIKE '%{safe}%' OR [Full Name] LIKE '%{safe}%' OR [Phone Number] LIKE '%{safe}%' OR [Email] LIKE '%{safe}%'";
                }
            }
        }

        private void ShowEditMemberModal()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد العضو المراد تعديل بياناته من الجدول.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string id = row.Cells["ID"].Value.ToString();
            string currName = row.Cells["Full Name"].Value.ToString();
            string currPhone = DatabaseHelper.ExecuteScalar("SELECT Phone FROM Members WHERE Id = @Id", new Microsoft.Data.Sqlite.SqliteParameter("@Id", id))?.ToString() ?? "";
            string currEmail = row.Cells["Email"].Value.ToString();
            string currGender = row.Cells["Gender"].Value.ToString();

            using (Form modal = new Form())
            {
                modal.Text = "تعديل بيانات العضو - Edit Member";
                modal.Size = new Size(460, 480);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                modal.MaximizeBox = false;

                Label lblN = new Label { Text = "الاسم الكامل / Full Name:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtN = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(380, 40), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currName };

                Label lblP = new Label { Text = "رقم الهاتف / Phone:", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtP = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(380, 40), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currPhone };

                Label lblE = new Label { Text = "البريد الإلكتروني / Email:", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtE = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(380, 40), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currEmail };

                Label lblG = new Label { Text = "النوع / Gender:", ForeColor = Color.White, Location = new Point(30, 245), AutoSize = true };
                Guna2ComboBox cbG = new Guna2ComboBox { Location = new Point(30, 270), Size = new Size(380, 40), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbG.Items.AddRange(new object[] { "Male", "Female" });
                cbG.SelectedItem = currGender;

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "💾 حفظ التعديلات (Save Changes)",
                    Location = new Point(30, 350),
                    Size = new Size(380, 46),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btnSave.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtN.Text))
                    {
                        MessageBox.Show("يرجى إدخال اسم العضو.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DatabaseHelper.ExecuteNonQuery(@"
                        UPDATE Members 
                        SET FullName = @N, Phone = @P, Email = @E, Gender = @G 
                        WHERE Id = @Id;",
                        new Microsoft.Data.Sqlite.SqliteParameter("@N", txtN.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@P", txtP.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@E", txtE.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@G", cbG.SelectedItem?.ToString() ?? "Male"),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Id", id));

                    MessageBox.Show("تم تحديث بيانات العضو بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblN, txtN, lblP, txtP, lblE, txtE, lblG, cbG, btnSave });
                modal.ShowDialog(this);
            }
        }

        private void ShowAddMemberModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "إضافة عضو جديد - New Gym Member";
                modal.Size = new Size(460, 480);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);
                modal.FormBorderStyle = FormBorderStyle.FixedDialog;
                modal.MaximizeBox = false;

                Label lblN = new Label { Text = "الاسم الكامل / Full Name:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtN = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(380, 40), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblP = new Label { Text = "رقم الهاتف / Phone:", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtP = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(380, 40), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblE = new Label { Text = "البريد الإلكتروني / Email:", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtE = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(380, 40), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblG = new Label { Text = "النوع / Gender:", ForeColor = Color.White, Location = new Point(30, 245), AutoSize = true };
                Guna2ComboBox cbG = new Guna2ComboBox { Location = new Point(30, 270), Size = new Size(380, 40), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbG.Items.AddRange(new object[] { "Male", "Female" });
                cbG.SelectedIndex = 0;

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "حفظ العضو (Save Member)",
                    Location = new Point(30, 350),
                    Size = new Size(380, 46),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnSave.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtN.Text))
                    {
                        MessageBox.Show("يرجى إدخال اسم العضو.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO Members (FullName, Phone, Email, Gender) VALUES (@N, @P, @E, @G)",
                        new Microsoft.Data.Sqlite.SqliteParameter("@N", txtN.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@P", txtP.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@E", txtE.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@G", cbG.SelectedItem.ToString()));

                    MessageBox.Show("تمت إضافة العضو بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblN, txtN, lblP, txtP, lblE, txtE, lblG, cbG, btnSave });
                modal.ShowDialog(this);
            }
        }

        private void DeleteSelectedMember()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد عضو من الجدول أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string id = row.Cells["ID"].Value.ToString();
            string name = row.Cells["Full Name"].Value.ToString();

            if (MessageBox.Show($"هل أنت متأكد من حذف العضو: {name}؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Subscriptions WHERE MemberId = @Id; DELETE FROM Attendance WHERE MemberId = @Id; DELETE FROM Payments WHERE MemberId = @Id; DELETE FROM WorkoutPlans WHERE MemberId = @Id; DELETE FROM Members WHERE Id = @Id;",
                    new Microsoft.Data.Sqlite.SqliteParameter("@Id", id));
                LoadData();
            }
        }
    }

    // ==========================================
    // 3. MEMBER DETAILS SCREEN
    // ==========================================
    public class MemberDetailsScreen : UserControl
    {
        private Guna2ComboBox cbMembers;
        private Label lblName, lblPhone, lblEmail, lblJoinDate, lblStatus, lblPlan;
        private Guna2DataGridView gridHistory;

        public MemberDetailsScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadMembersDropdown();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "👤  Member Profile & Full Details",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            cbMembers = new Guna2ComboBox
            {
                Location = new Point(700, 20),
                Size = new Size(360, 40),
                BorderRadius = 10,
                FillColor = Color.FromArgb(28, 32, 44),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            cbMembers.SelectedIndexChanged += (s, e) => LoadSelectedMemberProfile();

            // Profile Card (Left)
            Guna2Panel profileCard = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(340, 600),
                BorderRadius = 18,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent
            };

            Guna2CirclePictureBox pbAvatar = new Guna2CirclePictureBox
            {
                Location = new Point(110, 30),
                Size = new Size(110, 110),
                FillColor = Color.FromArgb(255, 95, 21)
            };

            lblName = new Label { Text = "Select Member", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 160), Size = new Size(300, 30), TextAlign = ContentAlignment.MiddleCenter };
            lblStatus = new Label { Text = "Status: --", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(40, 190, 140), Location = new Point(20, 195), Size = new Size(300, 20), TextAlign = ContentAlignment.MiddleCenter };

            lblPhone = new Label { Text = "📱 Phone: --", Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(170, 175, 190), Location = new Point(30, 250), AutoSize = true };
            lblEmail = new Label { Text = "✉️ Email: --", Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(170, 175, 190), Location = new Point(30, 290), AutoSize = true };
            lblPlan = new Label { Text = "📋 Membership: --", Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(170, 175, 190), Location = new Point(30, 330), AutoSize = true };
            lblJoinDate = new Label { Text = "📅 Join Date: --", Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(170, 175, 190), Location = new Point(30, 370), AutoSize = true };

            profileCard.Controls.AddRange(new Control[] { pbAvatar, lblName, lblStatus, lblPhone, lblEmail, lblPlan, lblJoinDate });

            // History Data Tab (Right)
            Guna2Panel historyCard = new Guna2Panel
            {
                Location = new Point(390, 80),
                Size = new Size(675, 600),
                BorderRadius = 18,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(12)
            };

            Label lblHist = new Label { Text = "Recent Attendance & Payment History", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(15, 15), AutoSize = true };
            gridHistory = ScreenViewsFactory.CreateStyledGrid();
            gridHistory.Location = new Point(15, 50);
            gridHistory.Size = new Size(645, 530);

            historyCard.Controls.AddRange(new Control[] { lblHist, gridHistory });

            this.Controls.AddRange(new Control[] { lblHeader, cbMembers, profileCard, historyCard });
        }

        private void LoadMembersDropdown()
        {
            var dt = DatabaseHelper.ExecuteQuery("SELECT Id, ('[MEM-' || printf('%04d', Id) || '] ' || FullName) AS MemberDisplay FROM Members ORDER BY Id ASC");
            cbMembers.DataSource = dt;
            cbMembers.DisplayMember = "MemberDisplay";
            cbMembers.ValueMember = "Id";
            if (dt.Rows.Count > 0) cbMembers.SelectedIndex = 0;
        }

        private void LoadSelectedMemberProfile()
        {
            if (cbMembers.SelectedValue == null) return;
            string memberId = cbMembers.SelectedValue.ToString();

            var dt = DatabaseHelper.ExecuteQuery(@"
                SELECT m.Id, m.FullName, m.Phone, m.Email, m.JoinDate,
                       COALESCE(s.Status, 'No Plan') AS Status,
                       COALESCE(p.Title, 'None') AS PlanTitle
                FROM Members m
                LEFT JOIN Subscriptions s ON m.Id = s.MemberId
                LEFT JOIN Memberships p ON s.MembershipId = p.Id
                WHERE m.Id = @Id LIMIT 1;
            ", new Microsoft.Data.Sqlite.SqliteParameter("@Id", memberId));

            if (dt.Rows.Count > 0)
            {
                var r = dt.Rows[0];
                lblName.Text = $"[MEM-{Convert.ToInt32(r["Id"]):D4}] {r["FullName"]}";
                lblPhone.Text = $"📱 Phone: {PhoneMaskHelper.Mask(r["Phone"]?.ToString())}";
                lblEmail.Text = $"✉️ Email: {r["Email"]}";
                lblPlan.Text = $"📋 Membership: {r["PlanTitle"]}";
                lblJoinDate.Text = $"📅 Join Date: {r["JoinDate"]}";
                lblStatus.Text = $"Status: {r["Status"]}";
            }

            // Load Attendance History
            gridHistory.Columns.Clear();
            var historyDt = DatabaseHelper.ExecuteQuery(@"
                SELECT CheckIn AS [Check-In Time], COALESCE(CheckOut, 'Still in gym') AS [Check-Out Time]
                FROM Attendance WHERE MemberId = @Id ORDER BY Id DESC LIMIT 15;
            ", new Microsoft.Data.Sqlite.SqliteParameter("@Id", memberId));
            gridHistory.DataSource = historyDt;
        }
    }

    // ==========================================
    // 4. MEMBERSHIPS PLANS SCREEN
    // ==========================================
    public class MembershipsScreen : UserControl
    {
        private Guna2DataGridView grid;

        public MembershipsScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "📋  Gym Membership Packages",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnAdd = new Guna2Button
            {
                Text = "+ Create New Package",
                BorderRadius = 10,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(880, 20),
                Size = new Size(185, 40),
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => AddMembershipPackage();

            Guna2Button btnEdit = new Guna2Button
            {
                Text = "✏️ Edit Package",
                BorderRadius = 10,
                FillColor = Color.FromArgb(40, 50, 70),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 160, 255),
                Location = new Point(720, 20),
                Size = new Size(150, 40),
                Cursor = Cursors.Hand
            };
            btnEdit.HoverState.FillColor = Color.FromArgb(60, 160, 255);
            btnEdit.HoverState.ForeColor = Color.White;
            btnEdit.Click += (s, e) => EditMembershipPackage();

            Guna2Button btnDelete = new Guna2Button
            {
                Text = "🗑️ Delete",
                BorderRadius = 10,
                FillColor = Color.FromArgb(45, 30, 35),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(600, 20),
                Size = new Size(110, 40),
                Cursor = Cursors.Hand
            };
            btnDelete.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnDelete.HoverState.ForeColor = Color.White;
            btnDelete.Click += (s, e) => DeleteMembershipPackage();

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnAdd, btnEdit, btnDelete, gridContainer });
        }

        private void LoadData()
        {
            grid.DataSource = DatabaseHelper.ExecuteQuery(@"
                SELECT Id AS [Plan ID], 
                       Title AS [Package Title], 
                       DurationDays AS [Duration (Days)], 
                       Price AS [Price (EGP)], 
                       Description AS [Features & Perks] 
                FROM Memberships;
            ");
        }

        private void EditMembershipPackage()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد الباقة المراد تعديلها من الجدول.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string id = row.Cells["Plan ID"].Value.ToString();
            string currTitle = row.Cells["Package Title"].Value.ToString();
            string currDays = row.Cells["Duration (Days)"].Value.ToString();
            string currPrice = row.Cells["Price (EGP)"].Value.ToString();
            string currDesc = row.Cells["Features & Perks"].Value.ToString();

            using (Form modal = new Form())
            {
                modal.Text = "تعديل باقة الاشتراك - Edit Package";
                modal.Size = new Size(420, 390);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblT = new Label { Text = "اسم الباقة:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtT = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currTitle };

                Label lblD = new Label { Text = "المدة بالأيام (Duration Days):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtD = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currDays };

                Label lblP = new Label { Text = "السعر (Price in EGP):", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtP = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currPrice };

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "💾 حفظ التعديلات",
                    Location = new Point(30, 260),
                    Size = new Size(340, 44),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery(@"
                        UPDATE Memberships 
                        SET Title = @T, DurationDays = @D, Price = @P 
                        WHERE Id = @Id;",
                        new Microsoft.Data.Sqlite.SqliteParameter("@T", txtT.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@D", txtD.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@P", txtP.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Id", id));

                    MessageBox.Show("تم تعديل باقة الاشتراك بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblT, txtT, lblD, txtD, lblP, txtP, btnSave });
                modal.ShowDialog(this);
            }
        }

        private void DeleteMembershipPackage()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد الباقة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string id = row.Cells["Plan ID"].Value.ToString();
            string title = row.Cells["Package Title"].Value.ToString();

            if (MessageBox.Show($"هل أنت متأكد من حذف الباقة: {title}؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Memberships WHERE Id = @Id", new Microsoft.Data.Sqlite.SqliteParameter("@Id", id));
                LoadData();
            }
        }

        private void AddMembershipPackage()
        {
            using (Form modal = new Form())
            {
                modal.Text = "باقة اشتراك جديدة";
                modal.Size = new Size(420, 360);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblT = new Label { Text = "اسم الباقة (مثلاً: 3 شهور VIP):", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtT = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblD = new Label { Text = "المدة بالأيام (Duration Days):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtD = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = "30" };

                Label lblP = new Label { Text = "السعر (Price in EGP):", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtP = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = "500" };

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "إضافة الباقة",
                    Location = new Point(30, 255),
                    Size = new Size(340, 42),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO Memberships (Title, DurationDays, Price, Description) VALUES (@T, @D, @P, 'باقة تدريب عامة')",
                        new Microsoft.Data.Sqlite.SqliteParameter("@T", txtT.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@D", txtD.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@P", txtP.Text.Trim()));
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblT, txtT, lblD, txtD, lblP, txtP, btnSave });
                modal.ShowDialog(this);
            }
        }
    }

    // ==========================================
    // 5. SUBSCRIPTIONS SCREEN
    // ==========================================
    public class SubscriptionsScreen : UserControl, ISearchable
    {
        private Guna2DataGridView grid;
        private DataTable dt;

        public SubscriptionsScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "🔄  Member Subscriptions & Renewals",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnRenew = new Guna2Button
            {
                Text = "⚡ Renew / New Subscription",
                BorderRadius = 10,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(840, 20),
                Size = new Size(225, 40),
                Cursor = Cursors.Hand
            };
            btnRenew.Click += (s, e) => ShowNewSubscriptionModal();

            Guna2Button btnEditSub = new Guna2Button
            {
                Text = "✏️ Edit Subscription",
                BorderRadius = 10,
                FillColor = Color.FromArgb(40, 50, 70),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 160, 255),
                Location = new Point(660, 20),
                Size = new Size(170, 40),
                Cursor = Cursors.Hand
            };
            btnEditSub.HoverState.FillColor = Color.FromArgb(60, 160, 255);
            btnEditSub.HoverState.ForeColor = Color.White;
            btnEditSub.Click += (s, e) => EditSubscriptionModal();

            Guna2Button btnDeleteSub = new Guna2Button
            {
                Text = "🗑️ Delete",
                BorderRadius = 10,
                FillColor = Color.FromArgb(45, 30, 35),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(540, 20),
                Size = new Size(110, 40),
                Cursor = Cursors.Hand
            };
            btnDeleteSub.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnDeleteSub.HoverState.ForeColor = Color.White;
            btnDeleteSub.Click += (s, e) => DeleteSubscription();

            Guna2Button btnExportExcel = new Guna2Button
            {
                Text = "📊 Excel",
                BorderRadius = 10,
                FillColor = Color.FromArgb(30, 48, 40),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 222, 128),
                Location = new Point(440, 20),
                Size = new Size(90, 40),
                Cursor = Cursors.Hand
            };
            btnExportExcel.HoverState.FillColor = Color.FromArgb(34, 197, 94);
            btnExportExcel.HoverState.ForeColor = Color.White;
            btnExportExcel.Click += (s, e) => ExportHelper.ExportToExcel(dt, "Subscriptions_Report");

            Guna2Button btnExportPDF = new Guna2Button
            {
                Text = "📑 PDF",
                BorderRadius = 10,
                FillColor = Color.FromArgb(48, 32, 42),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(248, 113, 113),
                Location = new Point(345, 20),
                Size = new Size(85, 40),
                Cursor = Cursors.Hand
            };
            btnExportPDF.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnExportPDF.HoverState.ForeColor = Color.White;
            btnExportPDF.Click += (s, e) => ExportHelper.ExportToPDF(dt, "Gym Subscriptions Directory");

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnRenew, btnEditSub, btnDeleteSub, btnExportExcel, btnExportPDF, gridContainer });
        }

        private void LoadData()
        {
            dt = DatabaseHelper.ExecuteQuery(@"
                SELECT s.Id AS [Sub ID], 
                       'MEM-' || printf('%04d', m.Id) AS [Member ID],
                       m.FullName AS [Member Name], 
                       p.Title AS [Package], 
                       s.StartDate AS [Start Date], 
                       s.EndDate AS [Expiry Date], 
                       s.Status AS [Status]
                FROM Subscriptions s
                JOIN Members m ON s.MemberId = m.Id
                JOIN Memberships p ON s.MembershipId = p.Id
                ORDER BY s.Id DESC;
            ");
            grid.DataSource = dt;
        }

        public void ApplySearch(string term)
        {
            if (dt == null) return;
            string safe = term.Replace("'", "''").Trim();
            if (string.IsNullOrWhiteSpace(safe))
            {
                dt.DefaultView.RowFilter = "";
            }
            else
            {
                dt.DefaultView.RowFilter = $"[Member ID] LIKE '%{safe}%' OR [Member Name] LIKE '%{safe}%' OR [Package] LIKE '%{safe}%'";
            }
        }

        private void EditSubscriptionModal()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد الاشتراك المراد تعديله من الجدول.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string subId = row.Cells["Sub ID"].Value.ToString();
            string currStatus = row.Cells["Status"].Value.ToString();
            string currEnd = row.Cells["Expiry Date"].Value.ToString();

            using (Form modal = new Form())
            {
                modal.Text = "تعديل تفاصيل الاشتراك - Edit Subscription";
                modal.Size = new Size(420, 360);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblE = new Label { Text = "تاريخ نهاية الاشتراك (YYYY-MM-DD):", ForeColor = Color.White, Location = new Point(30, 25), AutoSize = true };
                Guna2TextBox txtEnd = new Guna2TextBox { Location = new Point(30, 50), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currEnd };

                Label lblS = new Label { Text = "حالة الاشتراك (Status):", ForeColor = Color.White, Location = new Point(30, 105), AutoSize = true };
                Guna2ComboBox cbS = new Guna2ComboBox { Location = new Point(30, 130), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbS.Items.AddRange(new object[] { "Active", "Expired", "Suspended" });
                cbS.SelectedItem = currStatus;

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "💾 حفظ التعديلات",
                    Location = new Point(30, 210),
                    Size = new Size(340, 44),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery("UPDATE Subscriptions SET EndDate = @E, Status = @S WHERE Id = @Id;",
                        new Microsoft.Data.Sqlite.SqliteParameter("@E", txtEnd.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@S", cbS.SelectedItem?.ToString() ?? "Active"),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Id", subId));

                    MessageBox.Show("تم تحديث بيانات الاشتراك بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblE, txtEnd, lblS, cbS, btnSave });
                modal.ShowDialog(this);
            }
        }

        private void DeleteSubscription()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد اشتراك أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string subId = row.Cells["Sub ID"].Value.ToString();
            string member = row.Cells["Member Name"].Value.ToString();

            if (MessageBox.Show($"هل أنت متأكد من حذف اشتراك العضو: {member}؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Subscriptions WHERE Id = @Id", new Microsoft.Data.Sqlite.SqliteParameter("@Id", subId));
                LoadData();
            }
        }

        private void ShowNewSubscriptionModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "اشتراك جديد / تجديد";
                modal.Size = new Size(420, 350);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblM = new Label { Text = "اختر العضو:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2ComboBox cbM = new Guna2ComboBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbM.DataSource = DatabaseHelper.ExecuteQuery("SELECT Id, FullName FROM Members");
                cbM.DisplayMember = "FullName";
                cbM.ValueMember = "Id";

                Label lblP = new Label { Text = "اختر الباقة:", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2ComboBox cbP = new Guna2ComboBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbP.DataSource = DatabaseHelper.ExecuteQuery("SELECT Id, Title FROM Memberships");
                cbP.DisplayMember = "Title";
                cbP.ValueMember = "Id";

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "تأكيد الاشتراك وتفعيل الحساب",
                    Location = new Point(30, 200),
                    Size = new Size(340, 45),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnSave.Click += (s, e) =>
                {
                    if (cbM.SelectedValue == null || cbP.SelectedValue == null) return;
                    DatabaseHelper.ExecuteNonQuery(@"
                        INSERT INTO Subscriptions (MemberId, MembershipId, StartDate, EndDate, Status) 
                        VALUES (@M, @P, date('now'), date('now', '+30 days'), 'Active');",
                        new Microsoft.Data.Sqlite.SqliteParameter("@M", cbM.SelectedValue),
                        new Microsoft.Data.Sqlite.SqliteParameter("@P", cbP.SelectedValue));
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblM, cbM, lblP, cbP, btnSave });
                modal.ShowDialog(this);
            }
        }
    }

    // ==========================================
    // 6. PAYMENTS SCREEN
    // ==========================================
    public class PaymentsScreen : UserControl, ISearchable
    {
        private Guna2DataGridView grid;
        private DataTable dt;

        public PaymentsScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "💳  Payments & Invoices Management",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnPay = new Guna2Button
            {
                Text = "+ Record New Payment",
                BorderRadius = 10,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(860, 20),
                Size = new Size(205, 40),
                Cursor = Cursors.Hand
            };
            btnPay.Click += (s, e) => ShowPaymentModal();

            Guna2Button btnEditPay = new Guna2Button
            {
                Text = "✏️ Edit Payment (تعديل المبلغ)",
                BorderRadius = 10,
                FillColor = Color.FromArgb(40, 50, 70),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 160, 255),
                Location = new Point(640, 20),
                Size = new Size(210, 40),
                Cursor = Cursors.Hand
            };
            btnEditPay.HoverState.FillColor = Color.FromArgb(60, 160, 255);
            btnEditPay.HoverState.ForeColor = Color.White;
            btnEditPay.Click += (s, e) => EditPaymentModal();

            Guna2Button btnDeletePay = new Guna2Button
            {
                Text = "🗑️ Delete",
                BorderRadius = 10,
                FillColor = Color.FromArgb(45, 30, 35),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(520, 20),
                Size = new Size(110, 40),
                Cursor = Cursors.Hand
            };
            btnDeletePay.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnDeletePay.HoverState.ForeColor = Color.White;
            btnDeletePay.Click += (s, e) => DeletePayment();

            Guna2Button btnPrintReceipt = new Guna2Button
            {
                Text = "🖨️ Print Receipt",
                BorderRadius = 10,
                FillColor = Color.FromArgb(35, 45, 65),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 200, 255),
                Location = new Point(480, 20),
                Size = new Size(135, 40),
                Cursor = Cursors.Hand
            };
            btnPrintReceipt.HoverState.FillColor = Color.FromArgb(60, 160, 255);
            btnPrintReceipt.HoverState.ForeColor = Color.White;
            btnPrintReceipt.Click += (s, e) => PrintSelectedReceipt();

            Guna2Button btnExportExcel = new Guna2Button
            {
                Text = "📊 Excel",
                BorderRadius = 10,
                FillColor = Color.FromArgb(30, 48, 40),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 222, 128),
                Location = new Point(385, 20),
                Size = new Size(90, 40),
                Cursor = Cursors.Hand
            };
            btnExportExcel.HoverState.FillColor = Color.FromArgb(34, 197, 94);
            btnExportExcel.HoverState.ForeColor = Color.White;
            btnExportExcel.Click += (s, e) => ExportHelper.ExportToExcel(dt, "Payments_Report");

            Guna2Button btnExportPDF = new Guna2Button
            {
                Text = "📑 PDF",
                BorderRadius = 10,
                FillColor = Color.FromArgb(48, 32, 42),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(248, 113, 113),
                Location = new Point(295, 20),
                Size = new Size(85, 40),
                Cursor = Cursors.Hand
            };
            btnExportPDF.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnExportPDF.HoverState.ForeColor = Color.White;
            btnExportPDF.Click += (s, e) => ExportHelper.ExportToPDF(dt, "Payments & Invoices Report");

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnPay, btnEditPay, btnDeletePay, btnPrintReceipt, btnExportExcel, btnExportPDF, gridContainer });
        }

        private void PrintSelectedReceipt()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد الدفعة من الجدول أولاً لطباعة إيصال السداد.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var row = grid.SelectedRows[0];
                string receiptNo = row.Cells["Receipt #"]?.Value?.ToString() ?? "REC-000";
                string memberName = row.Cells["Member Name"]?.Value?.ToString() ?? "Valued Member";
                decimal amount = Convert.ToDecimal(row.Cells["Amount (EGP)"]?.Value ?? 0);
                string method = row.Cells["Method"]?.Value?.ToString() ?? "Cash";
                DateTime date = DateTime.TryParse(row.Cells["Date & Time"]?.Value?.ToString(), out var dtParsed) ? dtParsed : DateTime.Now;
                decimal remaining = Convert.ToDecimal(row.Cells["Remaining (EGP)"]?.Value ?? 0);

                ReceiptPrinter.PrintOrPreview(receiptNo, memberName, "Gym Access Subscription", amount, method, date, remaining);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ أثناء تحضير الإيصال: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            dt = DatabaseHelper.ExecuteQuery(@"
                SELECT p.Id AS [Receipt ID],
                       p.ReceiptNo AS [Receipt #],
                       'MEM-' || printf('%04d', m.Id) AS [Member ID],
                       m.FullName AS [Member Name],
                       p.Amount AS [Amount (EGP)],
                       p.PaymentMethod AS [Method],
                       p.PaymentDate AS [Date & Time],
                       p.Remaining AS [Remaining (EGP)]
                FROM Payments p
                JOIN Members m ON p.MemberId = m.Id
                ORDER BY p.Id DESC;
            ");
            grid.DataSource = dt;
        }

        public void ApplySearch(string term)
        {
            if (dt == null) return;
            string safe = term.Replace("'", "''").Trim();
            if (string.IsNullOrWhiteSpace(safe))
            {
                dt.DefaultView.RowFilter = "";
            }
            else
            {
                dt.DefaultView.RowFilter = $"[Member ID] LIKE '%{safe}%' OR [Member Name] LIKE '%{safe}%' OR [Receipt #] LIKE '%{safe}%'";
            }
        }

        private void EditPaymentModal()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد الدفعة / الإيصال المراد تعديلها من الجدول.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string payId = row.Cells["Receipt ID"].Value.ToString();
            string currAmount = row.Cells["Amount (EGP)"].Value.ToString();
            string currMethod = row.Cells["Method"].Value.ToString();
            string currRemaining = row.Cells["Remaining (EGP)"].Value.ToString();

            using (Form modal = new Form())
            {
                modal.Text = "تعديل بيانات الدفعة / الإيصال - Edit Payment";
                modal.Size = new Size(420, 390);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblA = new Label { Text = "المبلغ المدفوع (Amount EGP):", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtA = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currAmount };

                Label lblRem = new Label { Text = "المتبقي (Remaining EGP):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtRem = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currRemaining };

                Label lblMeth = new Label { Text = "طريقة الدفع:", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2ComboBox cbMeth = new Guna2ComboBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbMeth.Items.AddRange(new object[] { "Cash (كاش)", "Visa / Credit Card", "InstaPay", "Vodafone Cash" });
                cbMeth.SelectedItem = currMethod;

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "💾 حفظ التعديل على الإيصال",
                    Location = new Point(30, 265),
                    Size = new Size(340, 45),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery(@"
                        UPDATE Payments 
                        SET Amount = @A, Remaining = @Rem, PaymentMethod = @Met 
                        WHERE Id = @Id;",
                        new Microsoft.Data.Sqlite.SqliteParameter("@A", txtA.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Rem", txtRem.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Met", cbMeth.SelectedItem?.ToString() ?? "Cash"),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Id", payId));

                    MessageBox.Show("تم تعديل بيانات الدفعة بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblA, txtA, lblRem, txtRem, lblMeth, cbMeth, btnSave });
                modal.ShowDialog(this);
            }
        }

        private void DeletePayment()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد الإيصال أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string payId = row.Cells["Receipt ID"].Value.ToString();
            string recNo = row.Cells["Receipt #"].Value.ToString();

            if (MessageBox.Show($"هل أنت متأكد من حذف الإيصال: {recNo}؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Payments WHERE Id = @Id", new Microsoft.Data.Sqlite.SqliteParameter("@Id", payId));
                LoadData();
            }
        }

        private void ShowPaymentModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "تسجيل دفعة جديدة / إيصال";
                modal.Size = new Size(420, 390);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblM = new Label { Text = "العضو:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2ComboBox cbM = new Guna2ComboBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbM.DataSource = DatabaseHelper.ExecuteQuery("SELECT Id, FullName FROM Members");
                cbM.DisplayMember = "FullName";
                cbM.ValueMember = "Id";

                Label lblA = new Label { Text = "المبلغ المدفوع (EGP):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtA = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = "450" };

                Label lblMeth = new Label { Text = "طريقة الدفع:", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2ComboBox cbMeth = new Guna2ComboBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbMeth.Items.AddRange(new object[] { "Cash (كاش)", "Visa / Credit Card", "InstaPay", "Vodafone Cash" });
                cbMeth.SelectedIndex = 0;

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "طباعة وحفظ الإيصال",
                    Location = new Point(30, 265),
                    Size = new Size(340, 45),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnSave.Click += (s, e) =>
                {
                    string recNo = "REC-" + new Random().Next(10000, 99999);
                    DatabaseHelper.ExecuteNonQuery(@"
                        INSERT INTO Payments (MemberId, Amount, PaymentMethod, Remaining, ReceiptNo) 
                        VALUES (@M, @A, @Met, 0, @R);",
                        new Microsoft.Data.Sqlite.SqliteParameter("@M", cbM.SelectedValue),
                        new Microsoft.Data.Sqlite.SqliteParameter("@A", txtA.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Met", cbMeth.SelectedItem?.ToString() ?? "Cash"),
                        new Microsoft.Data.Sqlite.SqliteParameter("@R", recNo));
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblM, cbM, lblA, txtA, lblMeth, cbMeth, btnSave });
                modal.ShowDialog(this);
            }
        }
    }

    // ==========================================
    // 7. ATTENDANCE SCREEN
    // ==========================================
    public class AttendanceScreen : UserControl, ISearchable
    {
        private Guna2DataGridView grid;
        private DataTable dt;

        public AttendanceScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "⏱️  Attendance & Check-in / Check-out",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnCheckIn = new Guna2Button
            {
                Text = "⚡ Quick Check-In (تسجيل حضور)",
                BorderRadius = 10,
                FillColor = Color.FromArgb(40, 190, 140),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(830, 20),
                Size = new Size(235, 40),
                Cursor = Cursors.Hand
            };
            btnCheckIn.Click += (s, e) => QuickCheckInModal();

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnCheckIn, gridContainer });
        }

        private void LoadData()
        {
            dt = DatabaseHelper.ExecuteQuery(@"
                SELECT a.Id AS [Log ID],
                       'MEM-' || printf('%04d', m.Id) AS [Member ID],
                       m.FullName AS [Member Name],
                       SUBSTR(m.Phone, 1, 4) || '****' || SUBSTR(m.Phone, -2) AS [Phone],
                       a.CheckIn AS [Check-In Time],
                       COALESCE(a.CheckOut, 'Currently Active') AS [Check-Out]
                FROM Attendance a
                JOIN Members m ON a.MemberId = m.Id
                ORDER BY a.Id DESC;
            ");
            grid.DataSource = dt;
        }

        public void ApplySearch(string term)
        {
            if (dt == null) return;
            string safe = term.Replace("'", "''").Trim();
            if (string.IsNullOrWhiteSpace(safe))
            {
                dt.DefaultView.RowFilter = "";
            }
            else
            {
                dt.DefaultView.RowFilter = $"[Member ID] LIKE '%{safe}%' OR [Member Name] LIKE '%{safe}%' OR [Phone] LIKE '%{safe}%'";
            }
        }

        private void QuickCheckInModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "تسجيل دخول عضو";
                modal.Size = new Size(400, 240);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lbl = new Label { Text = "اختر العضو لتسجيل الحضور:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2ComboBox cb = new Guna2ComboBox { Location = new Point(30, 50), Size = new Size(320, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cb.DataSource = DatabaseHelper.ExecuteQuery("SELECT Id, ('[MEM-' || printf('%04d', Id) || '] ' || FullName) AS MemberDisplay FROM Members ORDER BY Id ASC");
                cb.DisplayMember = "MemberDisplay";
                cb.ValueMember = "Id";

                Guna2Button btnIn = new Guna2Button
                {
                    Text = "تسجيل دخول الآن",
                    Location = new Point(30, 115),
                    Size = new Size(320, 42),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(40, 190, 140),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnIn.Click += (s, e) =>
                {
                    if (cb.SelectedValue != null)
                    {
                        DatabaseHelper.ExecuteNonQuery("INSERT INTO Attendance (MemberId, CheckIn) VALUES (@M, datetime('now'))",
                            new Microsoft.Data.Sqlite.SqliteParameter("@M", cb.SelectedValue));
                        modal.Close();
                        LoadData();
                    }
                };

                modal.Controls.AddRange(new Control[] { lbl, cb, btnIn });
                modal.ShowDialog(this);
            }
        }
    }

    // ==========================================
    // 8. TRAINERS SCREEN
    // ==========================================
    public class TrainersScreen : UserControl, ISearchable
    {
        private Guna2DataGridView grid;
        private DataTable dt;

        public TrainersScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "🏋️  Gym Trainers & Coaches Management",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnAdd = new Guna2Button
            {
                Text = "+ Add New Coach",
                BorderRadius = 10,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(890, 20),
                Size = new Size(175, 40),
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => AddTrainerModal();

            Guna2Button btnEdit = new Guna2Button
            {
                Text = "✏️ Edit Coach (تعديل)",
                BorderRadius = 10,
                FillColor = Color.FromArgb(40, 50, 70),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 160, 255),
                Location = new Point(710, 20),
                Size = new Size(170, 40),
                Cursor = Cursors.Hand
            };
            btnEdit.HoverState.FillColor = Color.FromArgb(60, 160, 255);
            btnEdit.HoverState.ForeColor = Color.White;
            btnEdit.Click += (s, e) => EditTrainerModal();

            Guna2Button btnDelete = new Guna2Button
            {
                Text = "🗑️ Delete",
                BorderRadius = 10,
                FillColor = Color.FromArgb(45, 30, 35),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(590, 20),
                Size = new Size(110, 40),
                Cursor = Cursors.Hand
            };
            btnDelete.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnDelete.HoverState.ForeColor = Color.White;
            btnDelete.Click += (s, e) => DeleteTrainer();

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnAdd, btnEdit, btnDelete, gridContainer });
        }

        private void LoadData()
        {
            dt = DatabaseHelper.ExecuteQuery("SELECT Id AS [Coach ID], FullName AS [Coach Name], Specialty AS [Specialty], (SUBSTR(Phone, 1, 4) || '****' || SUBSTR(Phone, -2)) AS [Contact Phone], Salary AS [Salary (EGP)], HireDate AS [Hire Date] FROM Trainers;");
            grid.DataSource = dt;
        }

        public void ApplySearch(string term)
        {
            if (dt == null) return;
            string safe = term.Replace("'", "''").Trim();
            dt.DefaultView.RowFilter = string.IsNullOrWhiteSpace(safe) ? "" : $"[Coach Name] LIKE '%{safe}%' OR [Specialty] LIKE '%{safe}%' OR [Contact Phone] LIKE '%{safe}%'";
        }

        private void EditTrainerModal()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد المدرب أولاً من الجدول.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string id = row.Cells["Coach ID"].Value.ToString();
            string currName = row.Cells["Coach Name"].Value.ToString();
            string currSpec = row.Cells["Specialty"].Value.ToString();
            string currPhone = DatabaseHelper.ExecuteScalar("SELECT Phone FROM Trainers WHERE Id = @Id", new Microsoft.Data.Sqlite.SqliteParameter("@Id", id))?.ToString() ?? "";
            string currSal = row.Cells["Salary (EGP)"].Value.ToString();

            using (Form modal = new Form())
            {
                modal.Text = "تعديل بيانات المدرب - Edit Trainer";
                modal.Size = new Size(420, 390);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblN = new Label { Text = "اسم المدرب:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtN = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currName };

                Label lblS = new Label { Text = "التخصص:", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtS = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currSpec };

                Label lblP = new Label { Text = "رقم الهاتف:", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtP = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currPhone };

                Label lblSal = new Label { Text = "المرتب الشهري (EGP):", ForeColor = Color.White, Location = new Point(30, 245), AutoSize = true };
                Guna2TextBox txtSal = new Guna2TextBox { Location = new Point(30, 270), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currSal };

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "💾 حفظ تعديلات المدرب",
                    Location = new Point(30, 325),
                    Size = new Size(340, 42),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery(@"
                        UPDATE Trainers 
                        SET FullName = @N, Specialty = @S, Phone = @P, Salary = @Sal 
                        WHERE Id = @Id;",
                        new Microsoft.Data.Sqlite.SqliteParameter("@N", txtN.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@S", txtS.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@P", txtP.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Sal", txtSal.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Id", id));

                    MessageBox.Show("تم تحديث بيانات المدرب بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblN, txtN, lblS, txtS, lblP, txtP, lblSal, txtSal, btnSave });
                modal.ShowDialog(this);
            }
        }

        private void DeleteTrainer()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد المدرب أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string id = row.Cells["Coach ID"].Value.ToString();
            string name = row.Cells["Coach Name"].Value.ToString();

            if (MessageBox.Show($"هل أنت متأكد من حذف المدرب: {name}؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Trainers WHERE Id = @Id", new Microsoft.Data.Sqlite.SqliteParameter("@Id", id));
                LoadData();
            }
        }

        private void AddTrainerModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "إضافة مدرب جديد";
                modal.Size = new Size(420, 360);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblN = new Label { Text = "اسم المدرب:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtN = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblS = new Label { Text = "التخصص (مثلاً: لياقة بدنية، كمال أجسام):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtS = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblSal = new Label { Text = "المرتب الشهري (EGP):", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtSal = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = "8000" };

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "حفظ بيانات المدرب",
                    Location = new Point(30, 260),
                    Size = new Size(340, 42),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO Trainers (FullName, Specialty, Phone, Salary) VALUES (@N, @S, '0100000000', @Sal)",
                        new Microsoft.Data.Sqlite.SqliteParameter("@N", txtN.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@S", txtS.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Sal", txtSal.Text.Trim()));
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblN, txtN, lblS, txtS, lblSal, txtSal, btnSave });
                modal.ShowDialog(this);
            }
        }
    }

    // ==========================================
    // 9. WORKOUT PLANS SCREEN
    // ==========================================
    public class WorkoutPlansScreen : UserControl
    {
        private Guna2DataGridView grid;

        public WorkoutPlansScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "📝  Custom Workout Routines & Exercise Plans",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnAdd = new Guna2Button
            {
                Text = "+ Assign Workout Exercise",
                BorderRadius = 10,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(850, 20),
                Size = new Size(215, 40),
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => AddExerciseModal();

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnAdd, gridContainer });
        }

        private void LoadData()
        {
            grid.DataSource = DatabaseHelper.ExecuteQuery(@"
                SELECT w.Id AS [Plan ID],
                       m.FullName AS [Member],
                       w.DayOfWeek AS [Day],
                       w.ExerciseName AS [Exercise Name],
                       w.Sets || ' Sets' AS [Sets],
                       w.Reps || ' Reps' AS [Reps],
                       w.WeightKg || ' KG' AS [Target Weight]
                FROM WorkoutPlans w
                JOIN Members m ON w.MemberId = m.Id
                ORDER BY w.Id DESC;
            ");
        }

        private void AddExerciseModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "إضافة تمرين لجدول عضو";
                modal.Size = new Size(420, 420);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblM = new Label { Text = "العضو:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2ComboBox cbM = new Guna2ComboBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbM.DataSource = DatabaseHelper.ExecuteQuery("SELECT Id, FullName FROM Members");
                cbM.DisplayMember = "FullName";
                cbM.ValueMember = "Id";

                Label lblEx = new Label { Text = "اسم التمرين (مثال: Barbell Bench Press):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtEx = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblDay = new Label { Text = "يوم التمرين (Day of Week):", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2ComboBox cbDay = new Guna2ComboBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbDay.Items.AddRange(new object[] { "Saturday (السبت)", "Sunday (الأحد)", "Monday (الاثنين)", "Tuesday (الثلاثاء)", "Wednesday (الأربعاء)", "Thursday (الخميس)" });
                cbDay.SelectedIndex = 0;

                Label lblSets = new Label { Text = "Sets / Reps / Weight (KG):", ForeColor = Color.White, Location = new Point(30, 245), AutoSize = true };
                Guna2TextBox txtSets = new Guna2TextBox { Location = new Point(30, 270), Size = new Size(100, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = "4" };
                Guna2TextBox txtReps = new Guna2TextBox { Location = new Point(150, 270), Size = new Size(100, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = "12" };
                Guna2TextBox txtKg = new Guna2TextBox { Location = new Point(270, 270), Size = new Size(100, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = "60" };

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "حفظ التمرين للجدول",
                    Location = new Point(30, 325),
                    Size = new Size(340, 42),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery(@"
                        INSERT INTO WorkoutPlans (MemberId, ExerciseName, DayOfWeek, Sets, Reps, WeightKg)
                        VALUES (@M, @Ex, @D, @S, @R, @W);",
                        new Microsoft.Data.Sqlite.SqliteParameter("@M", cbM.SelectedValue),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Ex", txtEx.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@D", cbDay.SelectedItem.ToString()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@S", txtSets.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@R", txtReps.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@W", txtKg.Text.Trim()));
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblM, cbM, lblEx, txtEx, lblDay, cbDay, lblSets, txtSets, txtReps, txtKg, btnSave });
                modal.ShowDialog(this);
            }
        }
    }

    // ==========================================
    // 10. EXPENSES SCREEN
    // ==========================================
    public class ExpensesScreen : UserControl, ISearchable
    {
        private Guna2DataGridView grid;
        private DataTable dt;

        public ExpensesScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "💸  Gym Operational Expenses (مصروفات الجيم)",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnAdd = new Guna2Button
            {
                Text = "+ Add New Expense",
                BorderRadius = 10,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(880, 20),
                Size = new Size(185, 40),
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => AddExpenseModal();

            Guna2Button btnEditExp = new Guna2Button
            {
                Text = "✏️ Edit Expense (تعديل)",
                BorderRadius = 10,
                FillColor = Color.FromArgb(40, 50, 70),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 160, 255),
                Location = new Point(700, 20),
                Size = new Size(170, 40),
                Cursor = Cursors.Hand
            };
            btnEditExp.HoverState.FillColor = Color.FromArgb(60, 160, 255);
            btnEditExp.HoverState.ForeColor = Color.White;
            btnEditExp.Click += (s, e) => EditExpenseModal();

            Guna2Button btnDeleteExp = new Guna2Button
            {
                Text = "🗑️ Delete",
                BorderRadius = 10,
                FillColor = Color.FromArgb(45, 30, 35),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(580, 20),
                Size = new Size(110, 40),
                Cursor = Cursors.Hand
            };
            btnDeleteExp.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnDeleteExp.HoverState.ForeColor = Color.White;
            btnDeleteExp.Click += (s, e) => DeleteExpense();

            Guna2Button btnExportExcel = new Guna2Button
            {
                Text = "📊 Excel",
                BorderRadius = 10,
                FillColor = Color.FromArgb(30, 48, 40),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 222, 128),
                Location = new Point(480, 20),
                Size = new Size(90, 40),
                Cursor = Cursors.Hand
            };
            btnExportExcel.HoverState.FillColor = Color.FromArgb(34, 197, 94);
            btnExportExcel.HoverState.ForeColor = Color.White;
            btnExportExcel.Click += (s, e) => ExportHelper.ExportToExcel(dt, "Expenses_Report");

            Guna2Button btnExportPDF = new Guna2Button
            {
                Text = "📑 PDF",
                BorderRadius = 10,
                FillColor = Color.FromArgb(48, 32, 42),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(248, 113, 113),
                Location = new Point(385, 20),
                Size = new Size(85, 40),
                Cursor = Cursors.Hand
            };
            btnExportPDF.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnExportPDF.HoverState.ForeColor = Color.White;
            btnExportPDF.Click += (s, e) => ExportHelper.ExportToPDF(dt, "Gym Expenses Report");

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnAdd, btnEditExp, btnDeleteExp, btnExportExcel, btnExportPDF, gridContainer });
        }

        private void LoadData()
        {
            dt = DatabaseHelper.ExecuteQuery(@"
                SELECT Id AS [Expense ID],
                       Category AS [Expense Category],
                       Amount AS [Amount (EGP)],
                       ExpenseDate AS [Date],
                       Description AS [Description / Notes]
                FROM Expenses
                ORDER BY Id DESC;
            ");
            grid.DataSource = dt;
        }

        public void ApplySearch(string term)
        {
            if (dt == null) return;
            string safe = term.Replace("'", "''");
            dt.DefaultView.RowFilter = string.IsNullOrWhiteSpace(term) ? "" : $"[Expense Category] LIKE '%{safe}%' OR [Description / Notes] LIKE '%{safe}%'";
        }

        private void EditExpenseModal()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد المصروف المراد تعديله أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string id = row.Cells["Expense ID"].Value.ToString();
            string currCategory = row.Cells["Expense Category"].Value.ToString();
            string currAmount = row.Cells["Amount (EGP)"].Value.ToString();
            string currDesc = row.Cells["Description / Notes"].Value.ToString();

            using (Form modal = new Form())
            {
                modal.Text = "تعديل المصروف - Edit Expense";
                modal.Size = new Size(420, 360);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblC = new Label { Text = "نوع المصروف:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtC = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currCategory };

                Label lblA = new Label { Text = "المبلغ (Amount in EGP):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtA = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currAmount };

                Label lblD = new Label { Text = "ملاحظات وتفاصيل:", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtD = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = currDesc };

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "💾 حفظ التعديل",
                    Location = new Point(30, 255),
                    Size = new Size(340, 42),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery(@"
                        UPDATE Expenses 
                        SET Category = @C, Amount = @A, Description = @D 
                        WHERE Id = @Id;",
                        new Microsoft.Data.Sqlite.SqliteParameter("@C", txtC.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@A", txtA.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@D", txtD.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Id", id));

                    MessageBox.Show("تم تعديل المصروف بنجاح!", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblC, txtC, lblA, txtA, lblD, txtD, btnSave });
                modal.ShowDialog(this);
            }
        }

        private void DeleteExpense()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("يرجى تحديد المصروف أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = grid.SelectedRows[0];
            string id = row.Cells["Expense ID"].Value.ToString();
            string cat = row.Cells["Expense Category"].Value.ToString();

            if (MessageBox.Show($"هل أنت متأكد من حذف مصروف: {cat}؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Expenses WHERE Id = @Id", new Microsoft.Data.Sqlite.SqliteParameter("@Id", id));
                LoadData();
            }
        }

        private void AddExpenseModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "تسجيل مصروف جديد";
                modal.Size = new Size(420, 360);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblC = new Label { Text = "نوع المصروف (مثال: كهرباء، إيجار، صيانة):", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtC = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblA = new Label { Text = "المبلغ (Amount in EGP):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtA = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblD = new Label { Text = "ملاحظات وتفاصيل:", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtD = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "حفظ المصروف",
                    Location = new Point(30, 255),
                    Size = new Size(340, 42),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO Expenses (Category, Amount, Description) VALUES (@C, @A, @D)",
                        new Microsoft.Data.Sqlite.SqliteParameter("@C", txtC.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@A", txtA.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@D", txtD.Text.Trim()));
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblC, txtC, lblA, txtA, lblD, txtD, btnSave });
                modal.ShowDialog(this);
            }
        }
    }

    // ==========================================
    // 11. REPORTS SCREEN
    // ==========================================
    public class ReportsScreen : UserControl
    {
        private Label lblTotalRev, lblTotalExp, lblNetProfit, lblTotalMembers;

        public ReportsScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadReportData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "📈  Comprehensive Financial & Operational Reports",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            // 4 Big Report Summary Cards
            Guna2Panel cardRev = CreateMetricCard(30, 80, "إجمالي الإيرادات (Total Revenue)", "0 EGP", Color.FromArgb(40, 190, 140), out lblTotalRev);
            Guna2Panel cardExp = CreateMetricCard(295, 80, "إجمالي المصروفات (Expenses)", "0 EGP", Color.FromArgb(239, 68, 68), out lblTotalExp);
            Guna2Panel cardNet = CreateMetricCard(560, 80, "صافي الأرباح (Net Profit)", "0 EGP", Color.FromArgb(255, 95, 21), out lblNetProfit);
            Guna2Panel cardMem = CreateMetricCard(825, 80, "إجمالي الأعضاء (Members)", "0", Color.FromArgb(60, 160, 255), out lblTotalMembers);

            // Detailed Analytics List
            Guna2Panel detailedReport = new Guna2Panel
            {
                Location = new Point(30, 240),
                Size = new Size(1035, 440),
                BorderRadius = 18,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(15)
            };

            Label lblTable = new Label
            {
                Text = "Breakdown by Membership Plans & Revenue Share",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 18),
                AutoSize = true
            };

            DataTable dtBreakdown = null;
            try
            {
                dtBreakdown = DatabaseHelper.ExecuteQuery(@"
                    SELECT p.Title AS [Package Name], 
                           COUNT(s.Id) AS [Total Subscribers], 
                           COALESCE(SUM(pay.Amount), 0) || ' EGP' AS [Total Revenue Generated]
                    FROM Memberships p
                    LEFT JOIN Subscriptions s ON p.Id = s.MembershipId
                    LEFT JOIN Payments pay ON s.MemberId = pay.MemberId
                    GROUP BY p.Id;
                ");
            }
            catch { }

            Guna2Button btnExportExcel = new Guna2Button
            {
                Text = "📊 Excel",
                BorderRadius = 10,
                FillColor = Color.FromArgb(30, 48, 40),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(74, 222, 128),
                Location = new Point(915, 10),
                Size = new Size(95, 36),
                Cursor = Cursors.Hand
            };
            btnExportExcel.HoverState.FillColor = Color.FromArgb(34, 197, 94);
            btnExportExcel.HoverState.ForeColor = Color.White;
            btnExportExcel.Click += (s, e) => ExportHelper.ExportToExcel(dtBreakdown, "Gym_Revenue_Breakdown");

            Guna2Button btnExportPDF = new Guna2Button
            {
                Text = "📑 PDF",
                BorderRadius = 10,
                FillColor = Color.FromArgb(48, 32, 42),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(248, 113, 113),
                Location = new Point(815, 10),
                Size = new Size(90, 36),
                Cursor = Cursors.Hand
            };
            btnExportPDF.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnExportPDF.HoverState.ForeColor = Color.White;
            btnExportPDF.Click += (s, e) => ExportHelper.ExportToPDF(dtBreakdown, "Gym Revenue & Package Share Report");

            Guna2DataGridView gridBreakdown = ScreenViewsFactory.CreateStyledGrid();
            gridBreakdown.Location = new Point(15, 55);
            gridBreakdown.Size = new Size(1005, 360);
            if (dtBreakdown != null) gridBreakdown.DataSource = dtBreakdown;

            detailedReport.Controls.AddRange(new Control[] { lblTable, btnExportExcel, btnExportPDF, gridBreakdown });

            this.Controls.AddRange(new Control[] { lblHeader, cardRev, cardExp, cardNet, cardMem, detailedReport });
        }

        private Guna2Panel CreateMetricCard(int x, int y, string title, string val, Color accent, out Label valLabel)
        {
            Guna2Panel card = new Guna2Panel
            {
                Location = new Point(x, y),
                Size = new Size(245, 130),
                BorderRadius = 16,
                FillColor = Color.FromArgb(28, 32, 44),
                BackColor = Color.Transparent
            };

            Label lblT = new Label { Text = title, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.FromArgb(140, 145, 160), Location = new Point(15, 16), AutoSize = true };
            valLabel = new Label { Text = val, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = accent, Location = new Point(15, 50), AutoSize = true };

            card.Controls.AddRange(new Control[] { lblT, valLabel });
            return card;
        }

        private void LoadReportData()
        {
            try
            {
                decimal rev = Convert.ToDecimal(DatabaseHelper.ExecuteScalar("SELECT COALESCE(SUM(Amount), 0) FROM Payments") ?? 0);
                decimal exp = Convert.ToDecimal(DatabaseHelper.ExecuteScalar("SELECT COALESCE(SUM(Amount), 0) FROM Expenses") ?? 0);
                decimal net = rev - exp;
                long mem = Convert.ToInt64(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM Members") ?? 0);

                lblTotalRev.Text = $"{rev:N0} EGP";
                lblTotalExp.Text = $"{exp:N0} EGP";
                lblNetProfit.Text = $"{net:N0} EGP";
                lblTotalMembers.Text = $"{mem:N0} Members";
            }
            catch { }
        }
    }

    // ==========================================
    // 12. STAFF & USERS (المستخدمين والصلاحيات)
    // ==========================================
    public class StaffUsersScreen : UserControl, ISearchable
    {
        private Guna2DataGridView grid;
        private DataTable dt;

        public StaffUsersScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "🛡️  System Users & Staff Permissions (المستخدمين والصلاحيات)",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnAdd = new Guna2Button
            {
                Text = "+ Add System User",
                BorderRadius = 10,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(880, 20),
                Size = new Size(185, 40),
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => AddUserModal();

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnAdd, gridContainer });
        }

        private void LoadData()
        {
            dt = DatabaseHelper.ExecuteQuery(@"
                SELECT Id AS [User ID],
                       FullName AS [Full Name],
                       Email AS [Email (Login)],
                       Role AS [Permission / Role],
                       CreatedAt AS [Created Date]
                FROM Users
                ORDER BY Id ASC;
            ");
            grid.DataSource = dt;
        }

        public void ApplySearch(string term)
        {
            if (dt == null) return;
            string safe = term.Replace("'", "''");
            dt.DefaultView.RowFilter = string.IsNullOrWhiteSpace(term) ? "" : $"[Full Name] LIKE '%{safe}%' OR [Email (Login)] LIKE '%{safe}%' OR [Permission / Role] LIKE '%{safe}%'";
        }

        private void AddUserModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "إضافة مستخدم جديد للنظام";
                modal.Size = new Size(420, 390);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblN = new Label { Text = "اسم المستخدم:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtN = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblE = new Label { Text = "البريد الإلكتروني (Login Email):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2TextBox txtE = new Guna2TextBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblP = new Label { Text = "كلمة المرور:", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtP = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, PasswordChar = '●' };

                Label lblR = new Label { Text = "الصلاحية / Role:", ForeColor = Color.White, Location = new Point(30, 245), AutoSize = true };
                Guna2ComboBox cbR = new Guna2ComboBox { Location = new Point(30, 270), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbR.Items.AddRange(new object[] { "Super Admin (مدير عام)", "Receptionist (موظف استقبال)", "Accountant (محاسب)", "Trainer (كابتن)" });
                cbR.SelectedIndex = 0;

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "إنشاء حساب المستخدم",
                    Location = new Point(30, 320),
                    Size = new Size(340, 42),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnSave.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtN.Text) || string.IsNullOrWhiteSpace(txtE.Text) || string.IsNullOrWhiteSpace(txtP.Text))
                    {
                        MessageBox.Show("يرجى ملء جميع الحقول المطلوبة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    try
                    {
                        DatabaseHelper.ExecuteNonQuery(@"
                            INSERT INTO Users (FullName, Email, Password, Role) 
                            VALUES (@N, @E, @P, @R);",
                            new Microsoft.Data.Sqlite.SqliteParameter("@N", txtN.Text.Trim()),
                            new Microsoft.Data.Sqlite.SqliteParameter("@E", txtE.Text.Trim()),
                            new Microsoft.Data.Sqlite.SqliteParameter("@P", txtP.Text.Trim()),
                            new Microsoft.Data.Sqlite.SqliteParameter("@R", cbR.SelectedItem.ToString()));
                        modal.Close();
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                modal.Controls.AddRange(new Control[] { lblN, txtN, lblE, txtE, lblP, txtP, lblR, cbR, btnSave });
                modal.ShowDialog(this);
            }
        }
    }

    // ==========================================
    // 13. EQUIPMENT (أجهزة الجيم وحالتها والصيانة)
    // ==========================================
    public class EquipmentScreen : UserControl, ISearchable
    {
        private Guna2DataGridView grid;
        private DataTable dt;

        public EquipmentScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "⚙️  Gym Equipment & Maintenance (أجهزة الجيم والصيانة)",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Button btnAdd = new Guna2Button
            {
                Text = "+ Add New Equipment",
                BorderRadius = 10,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(880, 20),
                Size = new Size(185, 40),
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => AddEquipmentModal();

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(1035, 600),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, btnAdd, gridContainer });
        }

        private void LoadData()
        {
            dt = DatabaseHelper.ExecuteQuery(@"
                SELECT Id AS [ID],
                       Name AS [Equipment Name],
                       Category AS [Category],
                       Quantity AS [Quantity],
                       Status AS [Condition / Status],
                       LastMaintenance AS [Last Service],
                       NextMaintenance AS [Next Service Due],
                       Notes AS [Notes]
                FROM Equipment
                ORDER BY Id DESC;
            ");
            grid.DataSource = dt;
        }

        public void ApplySearch(string term)
        {
            if (dt == null) return;
            string safe = term.Replace("'", "''");
            dt.DefaultView.RowFilter = string.IsNullOrWhiteSpace(term) ? "" : $"[Equipment Name] LIKE '%{safe}%' OR [Category] LIKE '%{safe}%' OR [Condition / Status] LIKE '%{safe}%'";
        }

        private void AddEquipmentModal()
        {
            using (Form modal = new Form())
            {
                modal.Text = "إضافة جهاز جديد";
                modal.Size = new Size(420, 420);
                modal.StartPosition = FormStartPosition.CenterParent;
                modal.BackColor = Color.FromArgb(28, 32, 44);

                Label lblN = new Label { Text = "اسم الجهاز:", ForeColor = Color.White, Location = new Point(30, 20), AutoSize = true };
                Guna2TextBox txtN = new Guna2TextBox { Location = new Point(30, 45), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };

                Label lblC = new Label { Text = "القسم (Cardio / Strength / Free Weights):", ForeColor = Color.White, Location = new Point(30, 95), AutoSize = true };
                Guna2ComboBox cbC = new Guna2ComboBox { Location = new Point(30, 120), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbC.Items.AddRange(new object[] { "Cardio (كارديو)", "Strength (أجهزة عضلات)", "Free Weights (أوزان حرة)", "Crossfit (كروس فيت)" });
                cbC.SelectedIndex = 0;

                Label lblQ = new Label { Text = "العدد (Quantity):", ForeColor = Color.White, Location = new Point(30, 170), AutoSize = true };
                Guna2TextBox txtQ = new Guna2TextBox { Location = new Point(30, 195), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White, Text = "1" };

                Label lblS = new Label { Text = "الحالة التشغيلية:", ForeColor = Color.White, Location = new Point(30, 245), AutoSize = true };
                Guna2ComboBox cbS = new Guna2ComboBox { Location = new Point(30, 270), Size = new Size(340, 38), BorderRadius = 8, FillColor = Color.FromArgb(20, 23, 30), ForeColor = Color.White };
                cbS.Items.AddRange(new object[] { "Working (يعمل بحالة ممتازة)", "Needs Maintenance (يحتاج صيانة)", "Out of Service (خارج الخدمة)" });
                cbS.SelectedIndex = 0;

                Guna2Button btnSave = new Guna2Button
                {
                    Text = "حفظ الجهاز",
                    Location = new Point(30, 325),
                    Size = new Size(340, 42),
                    BorderRadius = 10,
                    FillColor = Color.FromArgb(255, 95, 21),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White
                };
                btnSave.Click += (s, e) =>
                {
                    DatabaseHelper.ExecuteNonQuery(@"
                        INSERT INTO Equipment (Name, Category, Quantity, Status, LastMaintenance, NextMaintenance, Notes)
                        VALUES (@N, @C, @Q, @S, date('now'), date('now', '+90 days'), 'فحص دوري');",
                        new Microsoft.Data.Sqlite.SqliteParameter("@N", txtN.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@C", cbC.SelectedItem.ToString()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@Q", txtQ.Text.Trim()),
                        new Microsoft.Data.Sqlite.SqliteParameter("@S", cbS.SelectedItem.ToString()));
                    modal.Close();
                    LoadData();
                };

                modal.Controls.AddRange(new Control[] { lblN, txtN, lblC, cbC, lblQ, txtQ, lblS, cbS, btnSave });
                modal.ShowDialog(this);
            }
        }
    }

    // ==========================================
    // 14. NOTIFICATIONS (تنبيه الاشتراكات اللي قربت تنتهي)
    // ==========================================
    public class NotificationsScreen : UserControl
    {
        private Guna2DataGridView grid;

        public NotificationsScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadExpiringSubscriptions();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "🔔  Subscription Expiry Alerts (تنبيهات الاشتراكات القريبة من الانتهاء)",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Label lblSub = new Label
            {
                Text = "قائمة بالأعضاء الذين تنتهي اشتراكاتهم خلال الأيام القليلة القادمة أو المنتهية حديثاً للتواصل وتجديد الاشتراك.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(160, 165, 180),
                Location = new Point(32, 52),
                AutoSize = true
            };

            Guna2Button btnWhatsApp = new Guna2Button
            {
                Text = "📲 Send Renewal Reminder (تذكير تجديد)",
                BorderRadius = 10,
                FillColor = Color.FromArgb(40, 190, 140),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(810, 20),
                Size = new Size(255, 42),
                Cursor = Cursors.Hand
            };
            btnWhatsApp.Click += (s, e) =>
            {
                if (grid.SelectedRows.Count > 0)
                {
                    string name = grid.SelectedRows[0].Cells["Member Name"].Value.ToString();
                    string phone = grid.SelectedRows[0].Cells["Phone"].Value.ToString();
                    MessageBox.Show($"تم تجهيز رسالة التذكير للعضو {name} ({phone}) عبر واتساب بنجاح!", "تذكير العضو", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("يرجى تحديد عضو من الجدول أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            Guna2Panel gridContainer = new Guna2Panel
            {
                Location = new Point(30, 95),
                Size = new Size(1035, 585),
                BorderRadius = 16,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(10)
            };

            grid = ScreenViewsFactory.CreateStyledGrid();
            grid.Dock = DockStyle.Fill;
            gridContainer.Controls.Add(grid);

            this.Controls.AddRange(new Control[] { lblHeader, lblSub, btnWhatsApp, gridContainer });
        }

        private void LoadExpiringSubscriptions()
        {
            grid.DataSource = DatabaseHelper.ExecuteQuery(@"
                SELECT s.Id AS [Sub ID],
                       'MEM-' || printf('%04d', m.Id) AS [Member ID],
                       m.FullName AS [Member Name],
                       SUBSTR(m.Phone, 1, 4) || '****' || SUBSTR(m.Phone, -2) AS [Phone],
                       p.Title AS [Current Package],
                       s.EndDate AS [Expiry Date],
                       CAST(julianday(s.EndDate) - julianday('now') AS INTEGER) AS [Days Left],
                       CASE 
                           WHEN julianday(s.EndDate) < julianday('now') THEN '⚠️ Expired (منتهي)'
                           WHEN julianday(s.EndDate) - julianday('now') <= 7 THEN '🔔 Expiring Soon (قريب جداً)'
                           ELSE 'Active (ساري)'
                       END AS [Alert Level]
                FROM Subscriptions s
                JOIN Members m ON s.MemberId = m.Id
                JOIN Memberships p ON s.MembershipId = p.Id
                WHERE julianday(s.EndDate) - julianday('now') <= 14
                ORDER BY s.EndDate ASC;
            ");
        }
    }

    // ==========================================
    // 15. SETTINGS SCREEN (إعدادات النظام والعملة والبيانات)
    // ==========================================
    public class SettingsScreen : UserControl
    {
        private Guna2TextBox txtGymName, txtGymPhone, txtGymAddress;
        private Guna2ComboBox cbCurrency, cbAlertDays;

        public SettingsScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            LoadCurrentSettings();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "🛠️  Gym System Settings (بيانات الجيم وإعدادات النظام)",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            Guna2Panel container = new Guna2Panel
            {
                Location = new Point(30, 75),
                Size = new Size(1035, 605),
                BorderRadius = 18,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(30)
            };

            Label lblSec1 = new Label { Text = "معلومات النادي الرياضي / Club Identity:", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(255, 140, 60), Location = new Point(40, 30), AutoSize = true };

            Label lblGN = new Label { Text = "اسم الجيم / Gym Name:", ForeColor = Color.White, Location = new Point(40, 70), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            txtGymName = new Guna2TextBox { Location = new Point(40, 95), Size = new Size(420, 42), BorderRadius = 10, FillColor = Color.FromArgb(28, 32, 44), ForeColor = Color.White };

            Label lblGP = new Label { Text = "رقم هاتف الجيم / Contact Phone:", ForeColor = Color.White, Location = new Point(500, 70), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            txtGymPhone = new Guna2TextBox { Location = new Point(500, 95), Size = new Size(420, 42), BorderRadius = 10, FillColor = Color.FromArgb(28, 32, 44), ForeColor = Color.White };

            Label lblGA = new Label { Text = "العنوان والمقر / Address:", ForeColor = Color.White, Location = new Point(40, 155), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            txtGymAddress = new Guna2TextBox { Location = new Point(40, 180), Size = new Size(880, 42), BorderRadius = 10, FillColor = Color.FromArgb(28, 32, 44), ForeColor = Color.White };

            Label lblSec2 = new Label { Text = "الخيارات المالية والتنبيهات / Preferences:", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.FromArgb(255, 140, 60), Location = new Point(40, 250), AutoSize = true };

            Label lblCurr = new Label { Text = "العملة المستخدمة / Currency:", ForeColor = Color.White, Location = new Point(40, 290), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cbCurrency = new Guna2ComboBox { Location = new Point(40, 315), Size = new Size(420, 42), BorderRadius = 10, FillColor = Color.FromArgb(28, 32, 44), ForeColor = Color.White };
            cbCurrency.Items.AddRange(new object[] { "EGP (جنيه مصري)", "USD ($)", "SAR (ريال سعودي)", "AED (درهم إماراتي)" });

            Label lblAlert = new Label { Text = "تنبيه قبل انتهاء الاشتراك بـ / Notify Before (Days):", ForeColor = Color.White, Location = new Point(500, 290), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cbAlertDays = new Guna2ComboBox { Location = new Point(500, 315), Size = new Size(420, 42), BorderRadius = 10, FillColor = Color.FromArgb(28, 32, 44), ForeColor = Color.White };
            cbAlertDays.Items.AddRange(new object[] { "3 Days", "5 Days", "7 Days", "14 Days" });

            Guna2Button btnSave = new Guna2Button
            {
                Text = "💾 Save All Settings (حفظ الإعدادات)",
                BorderRadius = 12,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(40, 400),
                Size = new Size(320, 48),
                Cursor = Cursors.Hand
            };
            btnSave.Click += (s, e) => SaveSettings();

            container.Controls.AddRange(new Control[] { lblSec1, lblGN, txtGymName, lblGP, txtGymPhone, lblGA, txtGymAddress, lblSec2, lblCurr, cbCurrency, lblAlert, cbAlertDays, btnSave });
            this.Controls.AddRange(new Control[] { lblHeader, container });
        }

        private void LoadCurrentSettings()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT Key, Value FROM Settings");
                foreach (DataRow r in dt.Rows)
                {
                    string k = r["Key"].ToString() ?? "";
                    string v = r["Value"].ToString() ?? "";
                    if (k == "GymName") txtGymName.Text = v;
                    else if (k == "Phone") txtGymPhone.Text = v;
                    else if (k == "Address") txtGymAddress.Text = v;
                    else if (k == "Currency") cbCurrency.SelectedItem = v;
                    else if (k == "NotifyDaysBeforeExpiry") cbAlertDays.SelectedItem = $"{v} Days";
                }
                if (cbCurrency.SelectedIndex < 0) cbCurrency.SelectedIndex = 0;
                if (cbAlertDays.SelectedIndex < 0) cbAlertDays.SelectedIndex = 2;
            }
            catch { }
        }

        private void SaveSettings()
        {
            try
            {
                SaveKey("GymName", txtGymName.Text.Trim());
                SaveKey("Phone", txtGymPhone.Text.Trim());
                SaveKey("Address", txtGymAddress.Text.Trim());
                SaveKey("Currency", cbCurrency.SelectedItem?.ToString() ?? "EGP");
                string days = cbAlertDays.SelectedItem?.ToString()?.Replace(" Days", "") ?? "7";
                SaveKey("NotifyDaysBeforeExpiry", days);

                MessageBox.Show("تم حفظ جميع إعدادات النظام بنجاح!", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveKey(string k, string v)
        {
            DatabaseHelper.ExecuteNonQuery("INSERT INTO Settings (Key, Value) VALUES (@K, @V) ON CONFLICT(Key) DO UPDATE SET Value = @V;",
                new Microsoft.Data.Sqlite.SqliteParameter("@K", k),
                new Microsoft.Data.Sqlite.SqliteParameter("@V", v));
        }
    }

    // ==========================================
    // 16. BACKUP & RESTORE (النسخ الاحتياطي والاستعادة)
    // ==========================================
    public class BackupRestoreScreen : UserControl
    {
        private Label lblLastBackup, lblDbSize;

        public BackupRestoreScreen()
        {
            this.BackColor = Color.FromArgb(19, 21, 28);
            this.Dock = DockStyle.Fill;
            InitializeUI();
            RefreshDbInfo();
        }

        private void InitializeUI()
        {
            Label lblHeader = new Label
            {
                Text = "💾  Database Backup & Restore (النسخ الاحتياطي واستعادة البيانات)",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };

            // Card 1: Backup
            Guna2Panel cardBackup = new Guna2Panel
            {
                Location = new Point(30, 80),
                Size = new Size(500, 320),
                BorderRadius = 18,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(25)
            };

            Label lblBTitle = new Label { Text = "📦 Create Backup (أخذ نسخة احتياطية)", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(25, 25), AutoSize = true };
            Label lblBDesc = new Label
            {
                Text = "قم بإنشاء نسخة احتياطية آمنة لقاعدة بيانات الجيم كاملة (الأعضاء، الاشتراكات، المدفوعات، التمارين) وحفظها في أي مكان على جهازك أو قرص خارجي.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(160, 165, 180),
                Location = new Point(25, 65),
                Size = new Size(450, 60)
            };

            lblDbSize = new Label { Text = "Database Size: -- KB", Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(255, 140, 60), Location = new Point(25, 140), AutoSize = true };
            lblLastBackup = new Label { Text = "Status: SQLite Local Database Active", Font = new Font("Segoe UI", 9F), ForeColor = Color.FromArgb(40, 190, 140), Location = new Point(25, 175), AutoSize = true };

            Guna2Button btnDoBackup = new Guna2Button
            {
                Text = "⚡ Export Backup Now (حفظ نسخة الآن)",
                BorderRadius = 12,
                FillColor = Color.FromArgb(255, 95, 21),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(25, 230),
                Size = new Size(450, 48),
                Cursor = Cursors.Hand
            };
            btnDoBackup.Click += (s, e) => ExecuteBackup();

            cardBackup.Controls.AddRange(new Control[] { lblBTitle, lblBDesc, lblDbSize, lblLastBackup, btnDoBackup });

            // Card 2: Restore
            Guna2Panel cardRestore = new Guna2Panel
            {
                Location = new Point(560, 80),
                Size = new Size(505, 320),
                BorderRadius = 18,
                FillColor = Color.FromArgb(24, 27, 36),
                BackColor = Color.Transparent,
                Padding = new Padding(25)
            };

            Label lblRTitle = new Label { Text = "🔄 Restore Database (استعادة نسخة سابقة)", Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(25, 25), AutoSize = true };
            Label lblRDesc = new Label
            {
                Text = "تنبيه هام: استعادة قاعدة بيانات سابقة ستقوم باستبدال كافة البيانات الحالية بالنسخة المختارة. يرجى التأكد قبل تنفيذ الاستعادة.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(25, 65),
                Size = new Size(450, 60)
            };

            Guna2Button btnDoRestore = new Guna2Button
            {
                Text = "📂 Select Backup File & Restore (استعادة)",
                BorderRadius = 12,
                FillColor = Color.FromArgb(45, 30, 35),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(239, 68, 68),
                Location = new Point(25, 230),
                Size = new Size(455, 48),
                Cursor = Cursors.Hand
            };
            btnDoRestore.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnDoRestore.HoverState.ForeColor = Color.White;
            btnDoRestore.Click += (s, e) => ExecuteRestore();

            cardRestore.Controls.AddRange(new Control[] { lblRTitle, lblRDesc, btnDoRestore });

            this.Controls.AddRange(new Control[] { lblHeader, cardBackup, cardRestore });
        }

        private void RefreshDbInfo()
        {
            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gym.db");
                if (File.Exists(dbPath))
                {
                    FileInfo fi = new FileInfo(dbPath);
                    lblDbSize.Text = $"Database Size: {(fi.Length / 1024.0):N1} KB";
                }
            }
            catch { }
        }

        private void ExecuteBackup()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "SQLite Database (*.db)|*.db|Backup File (*.bak)|*.bak";
                sfd.FileName = $"GymBackup_{DateTime.Now:yyyyMMdd_HHmmss}.db";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string src = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gym.db");
                        File.Copy(src, sfd.FileName, true);
                        MessageBox.Show($"تم إنشاء النسخة الاحتياطية بنجاح في المسار:\n{sfd.FileName}", "نجاح النسخ الاحتياطي", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"حدث خطأ أثناء النسخ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExecuteRestore()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SQLite Database (*.db;*.bak)|*.db;*.bak|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("تحذير: هل أنت متأكد من استعادة هذه النسخة؟ سيتم استبدال البيانات الحالية.", "تأكيد الاستعادة", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            string dest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gym.db");
                            File.Copy(ofd.FileName, dest, true);
                            MessageBox.Show("تم استعادة قاعدة البيانات بنجاح! يرجى إعادة تشغيل البرنامج لتطبيق التغييرات بالكامل.", "نجاح الاستعادة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshDbInfo();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"حدث خطأ أثناء الاستعادة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}


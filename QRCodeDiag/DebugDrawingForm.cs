using QRCodeDiag.DataBlocks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRCodeDiag
{
    public partial class DebugDrawingForm : Form
    {
        private static DebugDrawingForm[] debugDrawingForm = new DebugDrawingForm[2];
        private static bool[] debugFormOpen = { false, false };

        private int windowInstance;
        private ConcurrentQueue<RawCodeByte> highlightEventQueue;
        private QRCode qrCode;
        private bool drawNextEvent;
        private List<RawCodeByte> completeWordsList; // Contains the complete words

        [Conditional("DEBUG")]
        internal static void ResetDebugWindow(QRCode debuggedQRCode, int instance, int millisecondDelay = 1)
        {
            if (debugFormOpen[instance])
                debugDrawingForm[instance].RestartDebugging(debuggedQRCode);
            else
            {
                debugDrawingForm[instance] = new DebugDrawingForm(debuggedQRCode, instance, millisecondDelay);
                debugDrawingForm[instance].Show();
            }
        }
        [Conditional("DEBUG")]
        internal static void DebugHighlightCell(QRCode debuggedQRCode, RawCodeByte currentWord, int instance) //ToDo: currentWord gets changed/completed before timer ticks
        {
            if (!debugFormOpen[instance])
            {
                debugDrawingForm[instance] = new DebugDrawingForm(debuggedQRCode, instance);
                debugDrawingForm[instance].Show();
            }
            debugDrawingForm[instance].EnqueueDrawingEvent(currentWord.Clone() as RawCodeByte);
        }
        private DebugDrawingForm(QRCode debuggedQRCode, int instance, int millisecondDelay = 1)
        {
            InitializeComponent();
            this.windowInstance = instance;
            this.qrCode = debuggedQRCode;
            this.pictureBox1.Paint += this.PaintDebugEvents;
            this.drawNextEvent = false;
            this.timer1.Interval = millisecondDelay;
            this.highlightEventQueue = new ConcurrentQueue<RawCodeByte>();
            this.completeWordsList = new List<RawCodeByte>();
            debugFormOpen[this.windowInstance] = true;
        }
        public void RestartDebugging(QRCode debuggedQRCode)
        {
            this.timer1.Stop(); //ToDo maybe mutex for paint/restart required
            this.drawNextEvent = false;
            this.qrCode = debuggedQRCode;
            this.highlightEventQueue = new ConcurrentQueue<RawCodeByte>();
            this.completeWordsList = new List<RawCodeByte>();
            debugFormOpen[this.windowInstance] = true;
        }
        private void EnqueueDrawingEvent(RawCodeByte wd)
        {
            this.highlightEventQueue.Enqueue(wd);
            if (!this.timer1.Enabled)
                this.timer1.Start();
        }
        private void PaintDebugEvents(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (this.drawNextEvent && this.highlightEventQueue.TryDequeue(out RawCodeByte nextDraw))
            {
                // Draw current Bit box
                var x = nextDraw.GetPixelCoordinate(nextDraw.BitString.Length - 1).X;
                var y = nextDraw.GetPixelCoordinate(nextDraw.BitString.Length - 1).Y;
                var g = e.Graphics;
                this.qrCode.DrawCode(g);
                var pixelWidth = g.VisibleClipBounds.Size.Width / QRCode.SIZE;
                var pixelHeight = g.VisibleClipBounds.Size.Height / QRCode.SIZE;
                var pen = new Pen(Color.Red, Math.Max(pixelWidth / 10, 1));
                g.DrawRectangle(pen, pixelWidth * x, pixelHeight * y, pixelWidth, pixelHeight);

                // Draw Word boxes
                if (nextDraw.IsComplete)
                    this.completeWordsList.Add(nextDraw);

                var fontFamily = new FontFamily("Lucida Console");
                var smallFont = new Font(fontFamily, 0.5F * pixelHeight, FontStyle.Regular, GraphicsUnit.Pixel);
                var largeFont = new Font(fontFamily, pixelHeight, FontStyle.Regular, GraphicsUnit.Pixel);
                var redBrush = new SolidBrush(Color.Red);
                var orangeBrush = new SolidBrush(Color.Orange);

                for(int j = 0; j < completeWordsList.Count; j++)
                {
                    var wd = completeWordsList[j];
                    var p = new Pen(Color.Green, 2); //ToDo new color every word (or the correct one of 4 different ones)
                    foreach (var edge in wd.GetContour())
                    {
                        var startX = edge.Start.X;
                        var startY = edge.Start.Y;
                        var endX = edge.End.X;
                        var endY = edge.End.Y;
                        g.DrawLine(p, startX * pixelWidth, startY * pixelHeight, endX * pixelWidth, endY * pixelHeight);
                    }
                    for (int i = 0; i < wd.GetCurrentWordLength(); i++)
                    {
                        var pixCoord = wd.GetPixelCoordinate(i);
                        g.DrawString(i.ToString(), smallFont, redBrush, new Point((int)((pixCoord.X + 0.4F) * pixelWidth), (int)((pixCoord.Y+ 0.4F) * pixelHeight)));
                    }
                    var firstPixCoord = wd.GetPixelCoordinate(4);
                    string drawSymbol;
                    if (wd is ByteEncodingSymbol)
                    {
                        if (((ByteEncodingSymbol)wd).GetAsByte(out var symbolByte))
                            drawSymbol = Encoding.GetEncoding("iso-8859-1").GetString(new byte[] { symbolByte });
                        else
                            drawSymbol = "_";
                    }
                    else
                    {
                        drawSymbol = j.ToString();
                    }
                    g.DrawString(drawSymbol, largeFont, orangeBrush, new Point((int)(firstPixCoord.X * pixelWidth), (int)(firstPixCoord.Y * pixelHeight)));
                }

                // Write textbox
                this.textBox1.Text = this.GetCurrentSymbolInfo(nextDraw);
                this.timer1.Stop();
                if (this.highlightEventQueue.Count > 0)
                    this.timer1.Start();
            }
        }

        private string GetCurrentSymbolInfo(RawCodeByte wd)
        {
            var word = wd.BitString;
            if (word.Length > 0)
            {
                return String.Format(
                  "Current Symbol: {0}{4}Current Word: {1}{4}Current Position: {2}, {3}",
                  word[word.Length - 1],
                  word,
                  wd.GetPixelCoordinate(word.Length - 1).X,
                  wd.GetPixelCoordinate(word.Length - 1).Y,
                  Environment.NewLine);
            }
            else
            {
                return "No current Symbol";
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.drawNextEvent = true;
            this.pictureBox1.Invalidate();
        }

        private void DebugDrawingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            debugFormOpen[this.windowInstance] = false;
        }
    }
}

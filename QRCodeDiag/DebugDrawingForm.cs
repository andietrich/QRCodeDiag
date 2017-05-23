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
        private Type typeToShow;

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
        internal static void DebugHighlightCell(RawCodeByte currentWord, int instance) //ToDo: currentWord gets changed/completed before timer ticks
        {
            if (debugFormOpen[instance])
            {
                debugDrawingForm[instance].EnqueueDrawingEvent(currentWord.Clone() as RawCodeByte);
            }
            else
                Debug.WriteLine("Can't DebugHighlightCell: DebugDrawingForm instance " + instance + " is not open.");
        }
        private DebugDrawingForm(QRCode debuggedQRCode, int instance, int millisecondDelay)
        {
            InitializeComponent();
            this.windowInstance = instance;
            this.qrCode = debuggedQRCode;
            this.pictureBox1.Paint += this.PaintDebugEvents;
            this.drawNextEvent = false;
            this.timer1.Interval = millisecondDelay;
            this.highlightEventQueue = new ConcurrentQueue<RawCodeByte>();
            this.completeWordsList = new List<RawCodeByte>();
            this.typeToShow = typeof(RawCodeByte);
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
            var g = e.Graphics;
            var pixelWidth = g.VisibleClipBounds.Size.Width / QRCode.VERSIONSIZE;
            var pixelHeight = g.VisibleClipBounds.Size.Height / QRCode.VERSIONSIZE;
            var fontFamily = new FontFamily("Lucida Console");
            var smallFont = new Font(fontFamily, 0.5F * pixelHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var largeFont = new Font(fontFamily, pixelHeight, FontStyle.Regular, GraphicsUnit.Pixel);
            var redBrush = new SolidBrush(Color.Red);
            var orangeBrush = new SolidBrush(Color.Orange);
            this.qrCode.DrawCode(g, this.pictureBox1.Size);
            
            for(int j = 0; j < completeWordsList.Count; j++)
            {
                var wd = completeWordsList[j];
                if (wd.GetType() == this.typeToShow)
                {
                    // Draw symbol edges
                    var p = new Pen(Color.Green, 2); //ToDo new color every word (or the correct one of 4 different ones)
                    foreach (var edge in wd.GetContour())
                    {
                        var startX = edge.Start.X;
                        var startY = edge.Start.Y;
                        var endX = edge.End.X;
                        var endY = edge.End.Y;
                        g.DrawLine(p, startX * pixelWidth, startY * pixelHeight, endX * pixelWidth, endY * pixelHeight);
                    }

                    // Draw bit index
                    for (int i = 0; i < wd.CurrentSymbolLength; i++)
                    {
                        var pixCoord = wd.GetBitCoordinate(i);
                        g.DrawString(i.ToString(), smallFont, redBrush, new Point((int)((pixCoord.X + 0.4F) * pixelWidth), (int)((pixCoord.Y + 0.4F) * pixelHeight)));
                    }

                    
                    var firstPixCoord = wd.GetBitCoordinate(4);
                    if (wd is ByteEncodingSymbol && this.typeToShow == typeof(ByteEncodingSymbol)) // Draw symbol value
                    {
                        ((ByteEncodingSymbol)wd).GetAsByte(out var symbolByte);
                        string drawSymbol = Encoding.GetEncoding("iso-8859-1").GetString(new byte[] { symbolByte });
                        g.DrawString(drawSymbol, largeFont, orangeBrush, new Point((int)(firstPixCoord.X * pixelWidth), (int)(firstPixCoord.Y * pixelHeight)));
                    }
                    else if (this.typeToShow != typeof(ByteEncodingSymbol)) // Draw symbol index
                    {
                        g.DrawString(j.ToString(), largeFont, orangeBrush, new Point((int)(firstPixCoord.X * pixelWidth), (int)(firstPixCoord.Y * pixelHeight)));
                    }
                }
            }
            
            // Draw the bit iterations
            if (this.drawNextEvent && this.highlightEventQueue.TryDequeue(out RawCodeByte nextDraw) && nextDraw.GetType() == this.typeToShow)
            {
                // Draw current Bit box
                var x = nextDraw.GetBitCoordinate(nextDraw.BitString.Length - 1).X;
                var y = nextDraw.GetBitCoordinate(nextDraw.BitString.Length - 1).Y;

                var pen = new Pen(Color.Red, Math.Max(pixelWidth / 10, 1));
                g.DrawRectangle(pen, pixelWidth * x, pixelHeight * y, pixelWidth, pixelHeight);

                // Draw Word boxes
                if (nextDraw.IsComplete)
                    this.completeWordsList.Add(nextDraw);
                
                // Write textbox
                this.textBox1.Text = this.GetCurrentSymbolInfo(nextDraw);
                this.timer1.Stop();
                if (this.highlightEventQueue.Count > 0)
                {
                    this.timer1.Start();
                }
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
                  wd.GetBitCoordinate(word.Length - 1).X,
                  wd.GetBitCoordinate(word.Length - 1).Y,
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
            this.pictureBox1.Refresh();
        }

        private void DebugDrawingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            debugFormOpen[this.windowInstance] = false;
        }

        private void showEncodedButton_Click(object sender, EventArgs e)
        {
            this.typeToShow = typeof(ByteEncodingSymbol);
            var saveWordList = this.completeWordsList;
            this.RestartDebugging(this.qrCode);
            this.timer1.Interval = 1;
            foreach (var wd in saveWordList)
            {
                this.EnqueueDrawingEvent(wd);
            }
        }

        private void showRawButton_Click(object sender, EventArgs e)
        {
            this.typeToShow = typeof(RawCodeByte);
            var saveWordList = this.completeWordsList;
            this.RestartDebugging(this.qrCode);
            this.timer1.Interval = 1;
            foreach (var wd in saveWordList)
            {
                this.EnqueueDrawingEvent(wd);
            }
        }
    }
}

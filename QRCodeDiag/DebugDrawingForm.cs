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
        private static DebugDrawingForm debugDrawingForm;
        private static bool debugFormOpen = false;
        private ConcurrentQueue<WordDetails> highlightEventQueue;
        private QRCode qrCode;
        private bool drawNextEvent;
        private List<WordDetails> completeWordsList; // Contains the complete words

        [Conditional("DEBUG")]
        internal static void DebugHighlightCell(QRCode debuggedQRCode, WordDetails currentWord) //ToDo: currentWord gets changed/completed before timer ticks
        {
            if (!debugFormOpen)
            {
                debugDrawingForm = new DebugDrawingForm(debuggedQRCode);
                debugDrawingForm.Show();
            }
            debugDrawingForm.EnqueueDrawingEvent(currentWord.Clone() as WordDetails);
        }

        private DebugDrawingForm(QRCode debuggedQRCode, int millisecondDelay = 250)
        {
            InitializeComponent();
            this.qrCode = debuggedQRCode;
            this.pictureBox1.Paint += this.PaintDebugEvents;
            this.drawNextEvent = false;
            this.timer1.Interval = millisecondDelay;
            this.highlightEventQueue = new ConcurrentQueue<WordDetails>();
            this.completeWordsList = new List<WordDetails>();
            debugFormOpen = true;
        }

        private void EnqueueDrawingEvent(WordDetails wd)
        {
            this.highlightEventQueue.Enqueue(wd);
            if (!this.timer1.Enabled)
                this.timer1.Start();
        }
        private void PaintDebugEvents(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (this.drawNextEvent && this.highlightEventQueue.TryDequeue(out WordDetails nextDraw))
            {
                // Draw current Bit box
                var x = nextDraw.GetPixelCoordinate(nextDraw.DataWord.Length - 1).X;
                var y = nextDraw.GetPixelCoordinate(nextDraw.DataWord.Length - 1).Y;
                var g = e.Graphics;
                this.qrCode.DrawCode(g);
                var pixelWidth = g.VisibleClipBounds.Size.Width / QRCode.SIZE;
                var pixelHeight = g.VisibleClipBounds.Size.Height / QRCode.SIZE;
                var pen = new Pen(Color.Red, Math.Max(pixelWidth / 10, 1));
                g.DrawRectangle(pen, pixelWidth * x, pixelHeight * y, pixelWidth, pixelHeight);

                // Draw Word boxes
                if (nextDraw.IsComplete())
                    this.completeWordsList.Add(nextDraw);
                foreach(var wd in completeWordsList)
                {
                    var p = new Pen(Color.Green, 2); //ToDo new color every word (or the correct one of 4 different ones)
                    //ToDo draw the contours
                    foreach(var edge in wd.GetContour())
                    {
                        var startX = edge.Start.X;
                        var startY = edge.Start.Y;
                        var endX = edge.End.X;
                        var endY = edge.End.Y;
                        g.DrawLine(p, startX * pixelWidth, startY * pixelHeight, endX * pixelWidth, endY * pixelHeight);
                    }
                }

                // Write textbox
                this.textBox1.Text = this.GetCurrentSymbolInfo(nextDraw);
                this.timer1.Stop();
                if (this.highlightEventQueue.Count > 0)
                    this.timer1.Start();
            }
        }

        private string GetCurrentSymbolInfo(WordDetails wd)
        {
            if (wd.DataWord.Length > 0)
            {
                int len = wd.DataWord.Length;
                return String.Format(
                  "Current Symbol: {0}{4}Current Word: {1}{4}Current Position: {2}, {3}",
                  wd.DataWord[len - 1],
                  wd.DataWord,
                  wd.GetPixelCoordinate(len - 1).X,
                  wd.GetPixelCoordinate(len - 1).Y,
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
            debugFormOpen = false;
        }
    }
}

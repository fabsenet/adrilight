/* See the file "LICENSE" for the full license governing this code. */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Bambilight {

    public partial class Overlay : Form {

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        // Arrow Points
        private Point mTopEnd;
        private Point mTopHead;
        private Point mRightEnd;
        private Point mRightHead;
        private Point mBottomEnd;
        private Point mBottomHead;
        private Point mLeftEnd;
        private Point mLeftHead;

        private Graphics mGraphics;
        private readonly object mGraphicsLock = new object();

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;

        private BackgroundWorker mBackgroundWorker = new BackgroundWorker();

        public Overlay() {
            InitializeComponent();

            ShowInTaskbar = false;
            initArrowPoints();

            int exstyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            exstyle |= WS_EX_TRANSPARENT;
            SetWindowLong(this.Handle, GWL_EXSTYLE, exstyle);

            mBackgroundWorker.DoWork += mBackgroundWorker_DoWork;
            mBackgroundWorker.RunWorkerCompleted += mBackgroundWorker_Completed;
            mBackgroundWorker.WorkerSupportsCancellation = true;
        }

        private void initArrowPoints() {
            mTopEnd = new Point(1 * (Program.ScreenWidth / 4), 1 * (Program.ScreenHeight / 4));
            mTopHead = new Point(3 * (Program.ScreenWidth / 4), 1 * (Program.ScreenHeight / 4));

            mRightEnd = new Point(4 * (Program.ScreenWidth / 5), 1 * (Program.ScreenHeight / 4));
            mRightHead = new Point(4 * (Program.ScreenWidth / 5), 3 * (Program.ScreenHeight / 4));

            mBottomEnd = new Point(3 * (Program.ScreenWidth / 4), 3 * (Program.ScreenHeight / 4));
            mBottomHead = new Point(1 * (Program.ScreenWidth / 4), 3 * (Program.ScreenHeight / 4));

            mLeftEnd = new Point(1 * (Program.ScreenWidth / 5), 3 * (Program.ScreenHeight / 4));
            mLeftHead = new Point(1 * (Program.ScreenWidth / 5), 1 * (Program.ScreenHeight / 4));
        }

        public void Start() {
            Show();
            if (!mBackgroundWorker.IsBusy) {
                mBackgroundWorker.RunWorkerAsync();
            }
        }

        public void Stop() {
            if (mBackgroundWorker.IsBusy) {
                mBackgroundWorker.CancelAsync();
            }
        }

        private void mBackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(7, 7);
            Pen penArrow = new Pen(Color.Black, 7);
            penArrow.Alignment = PenAlignment.Inset;
            penArrow.DashStyle = DashStyle.Dash;
            penArrow.CustomEndCap = bigArrow;

            Pen penSpotBorder = new Pen(Color.Black, 2);

            Font font = new Font(FontFamily.GenericSansSerif, 16, FontStyle.Bold);

            SolidBrush solidBrushBlack = new SolidBrush(Color.Black);

            while (!mBackgroundWorker.CancellationPending) {
                
                lock (mGraphicsLock) {
                    try {
                        mGraphics = CreateGraphics();

                        if (Settings.SpotsX > 1 && Settings.SpotsY > 1) {
                            if ((Settings.MirrorX && Settings.MirrorY) || (!Settings.MirrorX && !Settings.MirrorY)) {
                                mGraphics.DrawLine(penArrow, mTopEnd, mTopHead);
                                mGraphics.DrawLine(penArrow, mBottomEnd, mBottomHead);
                                mGraphics.DrawLine(penArrow, mRightEnd, mRightHead);
                                mGraphics.DrawLine(penArrow, mLeftEnd, mLeftHead);
                            } else {
                                mGraphics.DrawLine(penArrow, mTopHead, mTopEnd);
                                mGraphics.DrawLine(penArrow, mBottomHead, mBottomEnd);
                                mGraphics.DrawLine(penArrow, mRightHead, mRightEnd);
                                mGraphics.DrawLine(penArrow, mLeftHead, mLeftEnd);
                            }
                        }

                        lock (SpotSet.Lock) {
                            foreach (Spot spot in SpotSet.Spots) {
                                mGraphics.DrawRectangle(penSpotBorder, spot.RectangleOverlayBorder);
                                mGraphics.FillRectangle(spot.Brush, spot.RectangleOverlayFilling);

                                if (spot == SpotSet.Spots[0]) {
                                    mGraphics.DrawString("1.", font, solidBrushBlack, spot.BottomRight.X + 3, spot.BottomRight.Y + 3);
                                }
                            }
                        }

                    } catch (Exception ex) {
                        Console.Write(ex);
                    }
                }
            }

            e.Cancel = true;
        }

        private void mBackgroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e) {
            Close();
        }

        public void RefreshGraphics() {
            try { // dirty but graphics clear is not stable to use
                lock (mGraphicsLock) {
                    if (null != mGraphics) mGraphics.Clear(BackColor);
                }
            } catch (Exception ex) {
                Console.Write(ex);
            }
        }
    }
}



using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace adrilight {

    public partial class Overlay : Form {

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        // Arrow Points
        private Point _mTopEnd;
        private Point _mTopHead;
        private Point _mRightEnd;
        private Point _mRightHead;
        private Point _mBottomEnd;
        private Point _mBottomHead;
        private Point _mLeftEnd;
        private Point _mLeftHead;

        private Graphics _mGraphics;
        private readonly object _mGraphicsLock = new object();

        private const int GwlExstyle = -20;
        private const int WsExTransparent = 0x20;

        private BackgroundWorker _mBackgroundWorker = new BackgroundWorker();

        public Overlay() {
            InitializeComponent();

            ShowInTaskbar = false;
            InitArrowPoints();

            int exstyle = GetWindowLong(this.Handle, GwlExstyle);
            exstyle |= WsExTransparent;
            SetWindowLong(this.Handle, GwlExstyle, exstyle);

            _mBackgroundWorker.DoWork += mBackgroundWorker_DoWork;
            _mBackgroundWorker.RunWorkerCompleted += mBackgroundWorker_Completed;
            _mBackgroundWorker.WorkerSupportsCancellation = true;
        }

        private void InitArrowPoints() {
            _mTopEnd = new Point(1 * (Program.ScreenWidth / 4), 1 * (Program.ScreenHeight / 4));
            _mTopHead = new Point(3 * (Program.ScreenWidth / 4), 1 * (Program.ScreenHeight / 4));

            _mRightEnd = new Point(4 * (Program.ScreenWidth / 5), 1 * (Program.ScreenHeight / 4));
            _mRightHead = new Point(4 * (Program.ScreenWidth / 5), 3 * (Program.ScreenHeight / 4));

            _mBottomEnd = new Point(3 * (Program.ScreenWidth / 4), 3 * (Program.ScreenHeight / 4));
            _mBottomHead = new Point(1 * (Program.ScreenWidth / 4), 3 * (Program.ScreenHeight / 4));

            _mLeftEnd = new Point(1 * (Program.ScreenWidth / 5), 3 * (Program.ScreenHeight / 4));
            _mLeftHead = new Point(1 * (Program.ScreenWidth / 5), 1 * (Program.ScreenHeight / 4));
        }

        public void Start() {
            Show();
            if (!_mBackgroundWorker.IsBusy) {
                _mBackgroundWorker.RunWorkerAsync();
            }
        }

        public void Stop() {
            if (_mBackgroundWorker.IsBusy) {
                _mBackgroundWorker.CancelAsync();
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

            while (!_mBackgroundWorker.CancellationPending) {
                
                lock (_mGraphicsLock) {
                    try {
                        _mGraphics = CreateGraphics();

                        if (Settings.SpotsX > 1 && Settings.SpotsY > 1) {
                            if ((Settings.MirrorX && Settings.MirrorY) || (!Settings.MirrorX && !Settings.MirrorY)) {
                                _mGraphics.DrawLine(penArrow, _mTopEnd, _mTopHead);
                                _mGraphics.DrawLine(penArrow, _mBottomEnd, _mBottomHead);
                                _mGraphics.DrawLine(penArrow, _mRightEnd, _mRightHead);
                                _mGraphics.DrawLine(penArrow, _mLeftEnd, _mLeftHead);
                            } else {
                                _mGraphics.DrawLine(penArrow, _mTopHead, _mTopEnd);
                                _mGraphics.DrawLine(penArrow, _mBottomHead, _mBottomEnd);
                                _mGraphics.DrawLine(penArrow, _mRightHead, _mRightEnd);
                                _mGraphics.DrawLine(penArrow, _mLeftHead, _mLeftEnd);
                            }
                        }

                        lock (SpotSet.Lock) {
                            foreach (Spot spot in SpotSet.Spots) {
                                _mGraphics.DrawRectangle(penSpotBorder, spot.RectangleOverlayBorder);
                                _mGraphics.FillRectangle(spot.OnDemandBrush, spot.RectangleOverlayFilling);

                                if (spot == SpotSet.Spots[0]) {
                                    _mGraphics.DrawString("1.", font, solidBrushBlack, spot.BottomRight.X + 3, spot.BottomRight.Y + 3);
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
                lock (_mGraphicsLock) {
                    if (null != _mGraphics) _mGraphics.Clear(BackColor);
                }
            } catch (Exception ex) {
                Console.Write(ex);
            }
        }
    }
}

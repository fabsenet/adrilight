using GalaSoft.MvvmLight;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Media;

using Color = System.Windows.Media.Color;

namespace adrilight {

    [DebuggerDisplay("Spot: Rectangle={Rectangle}, Color={Red},{Green},{Blue}")]
    sealed class Spot : ViewModelBase, IDisposable, ISpot
    {

        public Spot(int top, int left, int width, int height)
        {
            Rectangle = new Rectangle(top, left, width, height);
        }

        public Rectangle Rectangle { get; private set; }

        private bool _isFirst;
        public bool IsFirst {
            get => _isFirst;
            set { Set(() => IsFirst, ref _isFirst, value); }
        }

        private Color _color = Colors.Black;

        public Color OnDemandColor
        {
            get
            {
                _color = Color.FromRgb(Red, Green, Blue);
                return _color;
            }
        }
        private Color _colorT = Colors.Transparent;

        public Color OnDemandColorTransparent
        {
            get
            {
                _color = Color.FromArgb(0, Red, Green, Blue);
                return _color;
            }
        }

        public byte Red { get; private set; }
        public byte Green { get; private set; }
        public byte Blue { get; private set; }

        public void SetColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            _lastMissingValueIndication = null;

            RaisePropertyChanged(() => OnDemandColor);
            RaisePropertyChanged(() => OnDemandColorTransparent);
        }

        public void Dispose() {
        }

        private DateTime? _lastMissingValueIndication;
        private readonly double _dimToBlackIntervalInMs = TimeSpan.FromMilliseconds(10000).TotalMilliseconds;

        private float _dimR, _dimG, _dimB;

        public void IndicateMissingValue()
        {
            //this method might be called while another thread is calling setcolor() and we need the local copy to have a fixed value
            var localCopyLastMissingValueIndication = _lastMissingValueIndication;

            if (!localCopyLastMissingValueIndication.HasValue)
            {
                //a new period of missing values starts, copy last values
                _dimR = Red;
                _dimG = Green;
                _dimB = Blue;
                localCopyLastMissingValueIndication = _lastMissingValueIndication = DateTime.UtcNow;
            }

            var dimFactor =(float) (1 - (DateTime.UtcNow - localCopyLastMissingValueIndication.Value).TotalMilliseconds / _dimToBlackIntervalInMs);
            dimFactor = Math.Max(0, Math.Min(1, dimFactor));

            SetColor((byte)(dimFactor * _dimR), (byte)(dimFactor * _dimG), (byte)(dimFactor * _dimB));
        }
    }
}

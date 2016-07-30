using System;
using System.ComponentModel;
using DNWA.BHTCL;
using ArgumentException = DNWA.Exception.ArgumentException;
using ObjectDisposedException = DNWA.Exception.ObjectDisposedException;

// Created by James Swineson 2016-07-30
// Contact: dzics@public.swineson.me

namespace JSDenso
{
    public partial class CodeScanner : Component
    {
        #region private variables

        /// <summary>
        ///     DNWA library scanner object
        /// </summary>
        private readonly Scanner _scanner = new Scanner();

        /// <summary>
        ///     How many times have this scanned
        /// </summary>
        private int _readCount;

        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        #endregion

        #region constructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public CodeScanner()
        {
            InitializeComponent();
            _onLoad();
        }

        /// <summary>
        ///     Copy constructor
        /// </summary>
        /// <param name="container"></param>
        public CodeScanner(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            _onLoad();
        }

        private void _onLoad()
        {
            _scanner.OnDone += _scannerDone;
        }

        #endregion

        #region deconstructors

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region DNWA scanner invocation

        /// <summary>
        ///     Scanner got new result event handler
        /// </summary>
        private Code _readCode()
        {
            var c = new Code();
            // Get Barcode Type
            try
            {
                c.Text = new String(_scanner.InBufferType, 1);
            }
            catch (Exception ex)
            {
                throw new UnableToGetBarcodeTypeException("", ex);
            }
            // Get Barcode Count
            try
            {
                c.Length = _scanner.InBufferCount;
            }
            catch (Exception ex)
            {
                throw new UnableToGetBarcodeLengthException("", ex);
            }
            // Get Barcode Data
            try
            {
                c.Text = _scanner.Input(Scanner.ALL_BUFFER);
            }
            catch (ObjectDisposedException ex)
            {
                throw new UnableToGetBarcodeContentException("BarData Input Error", ex);
            }
            // Read Count +1, Display Read Count
            _readCount = ReadCount + 1;

            return c;
        }

        /// <summary>
        ///     Open scanner device port
        /// </summary>
        private void EnableScanner()
        {
            try
            {
                // Open Port
                _scanner.PortOpen = true;
            }
            catch (ArgumentException ex)
            {
                throw new PortOpenFailedException("Port Open Error", ex);
            }
        }

        /// <summary>
        ///     Close scanner device port
        /// </summary>
        private void DisableScanner()
        {
            try
            {
                // Close Port
                _scanner.PortOpen = false;
            }
            catch (ArgumentException ex)
            {
                throw new PortCloseFailedException("Port Close Error", ex);
            }
        }

        #endregion

        public bool Enabled
        {
            get { return _scanner.PortOpen; }
            set
            {
                if (value)
                {
                    if (Enabled) throw new PortAlreadyOpenException();
                    EnableScanner();
                }
                else
                {
                    if (!Enabled) throw new PortAlreadyClosedException();
                    DisableScanner();
                }
            }
        }

        /// <summary>
        ///     How many times have this scanned
        /// </summary>
        public int ReadCount
        {
            get { return _readCount; }
        }

        private void _scannerDone(Object o, EventArgs e)
        {
            OnCodeScanned(_readCode());
        }

        /// <summary>
        ///     Tragger event handler
        /// </summary>
        /// <param name="e">result code information</param>
        protected virtual void OnCodeScanned(Code e)
        {
            EventHandler<Code> handler = CodeScanned;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<Code> CodeScanned;
    }

    #region class Code: represents an barcode or 2Dcode content

    public class Code : EventArgs
    {
        public int Length { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
    }

    #endregion

    #region Scanner exceptions

    public class PortOpenFailedException : Exception
    {
        public PortOpenFailedException()
        {
        }

        public PortOpenFailedException(string message) : base(message)
        {
        }

        public PortOpenFailedException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class PortCloseFailedException : Exception
    {
        public PortCloseFailedException()
        {
        }

        public PortCloseFailedException(string message) : base(message)
        {
        }

        public PortCloseFailedException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class PortAlreadyOpenException : Exception
    {
        public PortAlreadyOpenException()
        {
        }

        public PortAlreadyOpenException(string message) : base(message)
        {
        }

        public PortAlreadyOpenException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class PortAlreadyClosedException : Exception
    {
        public PortAlreadyClosedException()
        {
        }

        public PortAlreadyClosedException(string message) : base(message)
        {
        }

        public PortAlreadyClosedException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class UnableToGetBarcodeTypeException : Exception
    {
        public UnableToGetBarcodeTypeException()
        {
        }

        public UnableToGetBarcodeTypeException(string message) : base(message)
        {
        }

        public UnableToGetBarcodeTypeException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class UnableToGetBarcodeLengthException : Exception
    {
        public UnableToGetBarcodeLengthException()
        {
        }

        public UnableToGetBarcodeLengthException(string message) : base(message)
        {
        }

        public UnableToGetBarcodeLengthException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class UnableToGetBarcodeContentException : Exception
    {
        public UnableToGetBarcodeContentException()
        {
        }

        public UnableToGetBarcodeContentException(string message) : base(message)
        {
        }

        public UnableToGetBarcodeContentException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    #endregion
}
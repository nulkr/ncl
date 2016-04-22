﻿using System;
using System.ComponentModel;
using System.Threading;

namespace ncl
{
    public enum SeqStatus { None = 0, Progress, Completed, Aborted, Error, Exception, Paused }

    public class Sequence : Component
    {
        #region constant

        private const int c_None = 0;
        private const int c_Completed = -4000;
        private const int c_ExceptErrorCode = 65535;

        #endregion constant

        #region field

        private BackgroundWorker _Worker = null;
        private int _ErrorCode = 0;
        private int _SeqNo = 0;

        #endregion field

        #region property

        [Browsable(false)]
        public int ErrorCode { get { return _ErrorCode; } }

        [Browsable(false)]
        public int SeqNo { get { return _SeqNo; } set { Interlocked.Exchange(ref _SeqNo, value); } }

        [Browsable(false)]
        public SeqStatus Status
        {
            get
            {
                if (_ErrorCode > 0)
                {
                    if (_ErrorCode == c_ExceptErrorCode)
                        return SeqStatus.Exception;
                    else
                        return SeqStatus.Error;
                }
                else
                {
                    switch (_SeqNo)
                    {
                        case c_None:
                            return SeqStatus.None;
                        case c_Completed:
                            return SeqStatus.Completed;
                    }

                    if (_SeqNo > 0)
                    {
                        if (_Worker.IsBusy)
                            return SeqStatus.Progress;
                        else
                            return SeqStatus.Paused;
                    }

                    return SeqStatus.Aborted;
                }
            }
        }

        public bool WorkerReportsProgress
        {
            get { return _Worker.WorkerReportsProgress; }
            set { _Worker.WorkerReportsProgress = value; }
        }

        public int ProgressInterval { get; set; }

        #endregion property

        #region constructor

        public Sequence()
        {
            _Worker = new BackgroundWorker();
            _Worker.WorkerReportsProgress = true; // 이거 없이 _Worker.ReportProgress 호출하면 오류 발생
            _Worker.WorkerSupportsCancellation = true;
            _Worker.DoWork += DoWork;
            _Worker.ProgressChanged += DoProgress;
            _Worker.RunWorkerCompleted += DoCompleted;

            ProgressInterval = 100;
        }

        #endregion constructor

        #region method

        public void Start(int nStartNo = 1)
        {
            if (!_Worker.IsBusy)
            {
                _ErrorCode = 0;
                if (OnCanStart != null)
                {
                    CancelEventArgs args = new CancelEventArgs();
                    OnCanStart(this, args);
                    if (args.Cancel)
                        return;
                }
                SeqNo = nStartNo;
                _Worker.RunWorkerAsync();
            }
        }

        public void Abort()
        {
            _Worker.CancelAsync();
            SeqNo = -Math.Abs(_SeqNo);
        }

        public void Next()
        {
            if (_SeqNo == 1)
                SeqNo = 10;
            else if (_SeqNo > 0)
                SeqNo += 10;
        }

        public void Jump(int No)
        {
            SeqNo = No;
        }

        public void Finish()
        {
            SeqNo = c_Completed;
        }

        public void Error(int nErrCode)
        {
            _Worker.CancelAsync();
            SeqNo = -Math.Abs(_SeqNo);
            _ErrorCode = nErrCode;
        }

        public void Pause()
        {
            if (_Worker.IsBusy)
                _Worker.CancelAsync();
        }

        public void Resume()
        {
            if (!_Worker.IsBusy)
                _Worker.RunWorkerAsync();
        }

        #endregion method

        #region event

        public delegate void CanStartEventHandler(object sender, CancelEventArgs e);

        public event CanStartEventHandler OnCanStart;

        public event DoWorkEventHandler OnWork;

        public event ProgressChangedEventHandler OnProgress;

        public event RunWorkerCompletedEventHandler OnCompleted;

        private void DoWork(object sender, DoWorkEventArgs args)
        {
            try
            {
                int tick = Environment.TickCount + ProgressInterval;

                while (_SeqNo > 0)
                {
                    if (OnWork != null) OnWork(this, args);

                    if (_Worker.WorkerReportsProgress && tick < Environment.TickCount)
                    {
                        _Worker.ReportProgress(_SeqNo);
                        tick = Environment.TickCount + ProgressInterval;
                    }

                    if (_Worker.CancellationPending) // request suspend
                    {
                        break;
                    }

                    System.Threading.Thread.Sleep(1);
                }
            }
            catch (Exception e)
            {
                App.Logger.Fatal(e);
                SeqNo = -Math.Abs(_SeqNo);
                _ErrorCode = c_ExceptErrorCode;
            }
        }

        private void DoProgress(object sender, ProgressChangedEventArgs e)
        {
            if (OnProgress != null) OnProgress(this, e);
        }

        private void DoCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (OnCompleted != null) OnCompleted(this, e);
        }

        #endregion event
    }
}
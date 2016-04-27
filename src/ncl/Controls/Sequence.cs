using System;
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

        public BackgroundWorker Worker = null;
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
                        if (Worker.IsBusy)
                            return SeqStatus.Progress;
                        else
                            return SeqStatus.Paused;
                    }

                    return SeqStatus.Aborted;
                }
            }
        }

        #endregion property

        #region constructor

        public Sequence()
        {
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true; // 이거 없이 Worker.ReportProgress 호출하면 오류 발생
            Worker.WorkerSupportsCancellation = true;
            Worker.DoWork += DoWork;
            Worker.ProgressChanged += DoProgress;
            Worker.RunWorkerCompleted += DoCompleted;
        }

        #endregion constructor

        #region method

        public void Start(int nStartNo = 1)
        {
            if (!Worker.IsBusy)
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
                Worker.RunWorkerAsync();
            }
        }

        public void Abort()
        {
            Worker.CancelAsync();
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
            Worker.CancelAsync();
            SeqNo = -Math.Abs(_SeqNo);
            _ErrorCode = nErrCode;
        }

        public void Pause()
        {
            if (Worker.IsBusy)
                Worker.CancelAsync();
        }

        public void Resume()
        {
            if (!Worker.IsBusy)
                Worker.RunWorkerAsync();
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
                while (_SeqNo > 0 && !Worker.CancellationPending)
                {
                    if (OnWork != null) OnWork(this, args);

                    if (Worker.CancellationPending) // request suspend
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
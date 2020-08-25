using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using log4net;

namespace RestAPI.Schedulers
{
    public abstract class AbstractScheduler
    {
        static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public abstract void Execute();
        private Timer _timer;
        private double _interval;
        private bool _isRunning;

        public AbstractScheduler (string interval)
        {
            _interval = Convert.ToDouble(interval);
            _isRunning = false;
        }

        public void Start()
        {   
            try
            {
                if (_timer == null)
                {
                    _timer = new Timer { AutoReset = true, Interval = _interval };
                    _timer.Elapsed += TimerElapsed;
                    _timer.Enabled = true;
                }
                else if (!_timer.Enabled)
                    _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                Log.Error("Start.:", ex);
            }
        }

        public void Stop()
        {
            try
            {
                if (_timer != null && _timer.Enabled)
                    _timer.Enabled = false;
            }
            catch (Exception ex)
            {
                Log.Error("Stop.:", ex);
            }
        }

        public void Dispose()
        {
            try
            {
                if (_timer != null)
                {
                    if (_timer.Enabled)
                        _timer.Enabled = false;

                    _timer.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Dispose.:", ex);
            }
        }

        private void TimerElapsed(object source, System.Timers.ElapsedEventArgs evt)
        {
            try
            {
                if (!_isRunning)
                {
                    _isRunning = !_isRunning;
                    try
                    {
                        Execute();
                    } catch (Exception e)
                    {
                        Log.Error("TimerElapsed.:", e);
                    }
                    _isRunning = !_isRunning;
                }
            }
            catch (Exception ex)
            {
                Log.Error("TimerElapsed.:", ex);
            }
        }
    }
}
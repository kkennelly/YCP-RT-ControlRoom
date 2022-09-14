using System;
using System.Threading;
using ControlRoomApplication.Constants;

namespace ControlRoomApplication.Entities
{
    public abstract class HeartbeatInterface
    {
        private Thread HeartbeatThread;
        private Mutex HeartbeatMutex;
        private DateTime LastCheckedIn;
        private bool KeepAlive;

        protected HeartbeatInterface()
        {
            HeartbeatThread = new Thread(HeartbeatThreadRoutine);
            HeartbeatMutex = new Mutex();
            LastCheckedIn = DateTime.UtcNow;

            HeartbeatTrackerContainer.StartTracking(this);

            KeepAlive = true;
            HeartbeatThread.Start();
        }

        ~HeartbeatInterface()
        {
            if (HeartbeatTrackerContainer.IsTracking(this))
            {
                HeartbeatTrackerContainer.StopTracking(this);
            }
        }

        public void AcquireControl()
        {
            HeartbeatMutex.WaitOne();
        }

        public void ReleaseControl()
        {
            HeartbeatMutex.ReleaseMutex();
        }

        public void BringUpHeartbeatThread()
        {
            KeepAlive = true;
            HeartbeatThread.Start();
        }

        public void BringDownHeartbeatThread()
        {
            AcquireControl();
            KeepAlive = false;
            ReleaseControl();

            HeartbeatThread.Join();
        }

        public bool IsConsideredAlive()
        {
            AcquireControl();
            double MillisecondsSinceLastCheckIn = (DateTime.UtcNow - LastCheckedIn).TotalMilliseconds;
            ReleaseControl();

            LastCheckedIn = DateTime.UtcNow;

            bool alive = MillisecondsSinceLastCheckIn <= HeartbeatConstants.MAXIMUM_ALLOWABLE_DIFFERENCE_IN_LAST_HEARD_MS;

            return alive;
        }

        public void BringDownDueToMiscommunication()
        {
            if (KeepAlive)
            {
                BringDownHeartbeatThread();
            }

            KillHeartbeatComponent();
        }

        protected void HeartbeatThreadRoutine()
        {
            AcquireControl();
            bool KeepRunningHeartbeat = KeepAlive;
            ReleaseControl();

            while (KeepRunningHeartbeat)
            {
                Thread.Sleep(HeartbeatConstants.INTERFACE_CHECK_IN_RATE_MS);

                // Keep this outside of the mutex's scope - in case the component test is blocking, this will still fail the heartbeat test
                KeepRunningHeartbeat = TestIfComponentIsAlive();

                AcquireControl();
                KeepRunningHeartbeat &= KeepAlive;
                LastCheckedIn = DateTime.UtcNow;
                ReleaseControl();
            }
        }

        // This is called to determine whether or not the component that this represents is responding to the system
        public abstract bool TestIfComponentIsAlive();

        // If this interface is failing to update appropriately or not hearing back as expected, this should be called
        protected abstract bool KillHeartbeatComponent();
    }
}
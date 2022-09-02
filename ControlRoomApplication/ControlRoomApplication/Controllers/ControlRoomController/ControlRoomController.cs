using System;
using System.Threading;
using ControlRoomApplication.Entities;
using ControlRoomApplication.Database;
using ControlRoomApplication.Controllers.Communications;
using ControlRoomApplication.Util;


namespace ControlRoomApplication.Controllers
{
    public class ControlRoomController
    {
        public ControlRoom ControlRoom { get; set; }
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ControlRoomController(ControlRoom controlRoom)
        {
            ControlRoom = controlRoom;
        }

        public bool AddRadioTelescopeController(RadioTelescopeController rtController)
        {
            if (ControlRoom.RadioTelescopes.Contains(rtController.RadioTelescope))
            {
                return false;
            }

            ControlRoom.RTControllerManagementThreads.Add(new RadioTelescopeControllerManagementThread(rtController));
            return true;
        }

        public bool AddRadioTelescopeControllerAndStart(RadioTelescopeController rtController)
        {
            if (AddRadioTelescopeController(rtController))
            {
                return ControlRoom.RTControllerManagementThreads[ControlRoom.RTControllerManagementThreads.Count - 1].Start();
            }
            else
            {
                return false;
            }
        }

        public bool RemoveRadioTelescopeControllerAt(int rtControllerIndex, bool waitForAnyTasks)
        {
            if ((rtControllerIndex < 0) || (rtControllerIndex >= ControlRoom.RTControllerManagementThreads.Count))
            {
                return false;
            }

            RadioTelescopeControllerManagementThread ToBeRemovedRTMT = ControlRoom.RTControllerManagementThreads[rtControllerIndex];

            if (!waitForAnyTasks)
            {
                ToBeRemovedRTMT.KillWithHardInterrupt();
            }
            else
            {
                ToBeRemovedRTMT.RequestToKill();
            }

            if (ToBeRemovedRTMT.WaitToJoin())
            {
                return ControlRoom.RTControllerManagementThreads.Remove(ToBeRemovedRTMT);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveRadioTelescopeController(RadioTelescopeController rtController, bool waitForAnyTasks)
        {
            return RemoveRadioTelescopeControllerAt(ControlRoom.RadioTelescopeControllers.IndexOf(rtController), waitForAnyTasks);
        }
    }
}
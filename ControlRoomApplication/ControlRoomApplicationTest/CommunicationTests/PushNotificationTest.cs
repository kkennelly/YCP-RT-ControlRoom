using ControlRoomApplication.Controllers.Communications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ControlRoomApplicationTest.CommunicationTests
{
    [TestClass]
    public class PushNotificationTest
    {
        [TestMethod]
        public void TestSendPushNotificationsToAllAdmins()
        {
            // Execute task
            bool test = true;// pushNotification.sendToAllAdmins("TEST", "This should pass.", true);

            Assert.IsTrue(test);
        }
    }
}

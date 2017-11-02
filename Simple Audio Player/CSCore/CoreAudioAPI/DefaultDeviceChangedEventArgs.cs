using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.CoreAudioAPI
{
    public class DefaultDeviceChangedEventArgs : DeviceNotificationEventArgs
    {
        public DataFlow DataFlow { get; set; }

        public Role Role { get; set; }

        public DefaultDeviceChangedEventArgs(string deviceID, DataFlow dataFlow, Role role)
            : base(deviceID)
        {
            DataFlow = dataFlow;
            Role = role;
        }
    }
}

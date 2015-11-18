namespace PSher.Services.Common
{
    using PSher.Common.Constants;
    using PSher.Services.Common.Contracts;
    using PubNubMessaging.Core;

    public class PubNubNotificationService : INotificationService
    {    
        private Pubnub pubnub;

        public PubNubNotificationService()
        {
            this.pubnub = new Pubnub(PubNubConstants.PublishKey, PubNubConstants.Subscribekey);
        }

        public static string LastMessage { get; private set; }

        public static string LastError { get; private set; }

        public void Notify(string notification)
        {
            this.pubnub.Publish(PubNubConstants.Channel, notification, (x) => LastMessage = x.ToString(), (e) => LastError = e.ToString());
        }
    }
}

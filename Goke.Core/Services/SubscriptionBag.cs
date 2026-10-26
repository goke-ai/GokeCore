namespace Goke.Core.Services
{
    public sealed class SubscriptionBag : IDisposable
    {
        private readonly List<IDisposable> subscriptions = [];

        public void Add(IDisposable subscription)
        {
            subscriptions.Add(subscription);
        }

        public void Dispose()
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Dispose();
            }

            subscriptions.Clear();
        }
    }
}

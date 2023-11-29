using Kuhpik;

public class SubscriptionSystem : GameSystem
{
    public override void OnInit()
    {
        GameEvents.Subscripe_E += Subscription;
    }

    private void Subscription(bool isSubscripe)
    {
        if (isSubscripe)
        {
            // IT IS NECESSARY TO ADD:: Complete disabling of advertising

            GameEvents.CoinsBoost_E?.Invoke(30, false);
            player.ChanceOfTreasure = 0.1f;
        }
    }
}

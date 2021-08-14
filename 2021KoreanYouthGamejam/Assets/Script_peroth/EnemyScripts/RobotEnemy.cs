namespace peroth
{
    public class RobotEnemy : HumanEnemy
    {
        public override void PlayerApproachNear()
        {
            var player = target.GetComponent<Player>();
            IsDetected(player);
        }
    }
}
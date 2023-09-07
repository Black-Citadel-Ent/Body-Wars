using UnityEngine;

namespace BaseItems
{
    public class BaseBehavior : MonoBehaviour
    {
        public static Vector2 RandomVector2()
        {
            var randAngle = UnityEngine.Random.Range(-180, 180) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle));
        }

        public static float TurnTowards(float currentDir, float turnTowards, float maxSpeed)
        {
            currentDir = currentDir.NormalizedAngle();
            turnTowards = turnTowards.NormalizedAngle();
            var diff = (turnTowards - currentDir).NormalizedAngle();
            return currentDir + Mathf.Min(Mathf.Abs(diff), maxSpeed * Time.fixedDeltaTime) *
                Mathf.Sign(diff);
        }
    }
}
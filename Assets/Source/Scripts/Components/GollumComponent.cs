using Spine.Unity;
using UnityEngine;

public class GollumComponent : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    private string[] _states = new string[] { "Angry", "Happy", "Sad"};

    public void SetAnimation(int animationIndex)
    {
        _skeletonAnimation.state.ClearTracks();

        _skeletonAnimation.state.AddAnimation(0, _states[animationIndex], false, 0f);
        _skeletonAnimation.state.AddAnimation(0, _states[animationIndex] + "_idle", true, 0f);
    }
}

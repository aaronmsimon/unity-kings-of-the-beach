using System.Collections;
using UnityEngine;

namespace KotB.Menus.Alt
{
    public class AthleteSelectionAnimator : MonoBehaviour
    {
        [SerializeField] private AnimationClip[] clips;
        [SerializeField] private float timeBeforClip = 2f;

        private Animator animator;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        private void Start() {
            StartCoroutine(PlayRandomClip());
        }

        private IEnumerator PlayRandomClip() {
            while (true) {
                float randomTime = Random.Range(0f, timeBeforClip);
                yield return new WaitForSeconds(randomTime);
                int clip = Random.Range(0, clips.Length);
                animator.Play(clips[clip].name, 0, 0f);
                yield return null; // wait one frame for the animator to transition
                float clipLength = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(clipLength);
            }
        }
    }
}

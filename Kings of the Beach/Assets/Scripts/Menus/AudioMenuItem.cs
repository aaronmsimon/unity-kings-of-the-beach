using UnityEngine;

namespace KotB.Menus
{
    public class AudioMenuItem : MenuItemBase
    {
        [Header("Audio Settings")]
        [SerializeField] private AudioClip selectSound;
        [SerializeField] private AudioClip navigationSound;
        [SerializeField] private AudioSource audioSource;

        public override void Initialize(MenuSystem system)
        {
            base.Initialize(system);
            
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
                
            if (audioSource == null)
                audioSource = Camera.main?.GetComponent<AudioSource>();
        }

        protected override void OnSelected()
        {
            base.OnSelected();
            PlaySound(navigationSound);
        }

        public override void OnSelect()
        {
            PlaySound(selectSound);
            base.OnSelect();
        }

        private void PlaySound(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}

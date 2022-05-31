namespace Salky.API.WebSocketRoutes.Models
{
    public class AudioState
    {
        public AudioState(bool microFoneMuted, bool headPhoneMuted)
        {
            MicroFoneMuted = microFoneMuted;
            HeadPhoneMuted = headPhoneMuted;
        }

        public bool MicroFoneMuted
        {
            get => HeadPhoneMuted ? true : _MicroFoneMuted;
            set => _MicroFoneMuted = HeadPhoneMuted ? true : value;
        }

        private bool _MicroFoneMuted;



        public bool HeadPhoneMuted { get; set; }

    }
}

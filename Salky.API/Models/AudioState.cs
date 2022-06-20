namespace Salky.API.Models
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

        [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        private bool _MicroFoneMuted;
        [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public bool CanTalk => !MicroFoneMuted && !HeadPhoneMuted;
        [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public bool CanHear => !HeadPhoneMuted;


        public bool HeadPhoneMuted { get; set; }

    }
}

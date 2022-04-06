using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wpf.Core
{
    public class AudioAdapter
    {
        public AudioAdapter()
        {
            waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveOut = new WaveOut();
            waveOut.DeviceNumber = 0;
            provider = new BufferedWaveProvider(waveIn.WaveFormat);
            waveOut.Init(provider);
        }
        private WaveIn waveIn;
        private WaveOut waveOut;
        private BufferedWaveProvider provider;
        public List<WaveInCapabilities> GetMicrofoneAvaibles => Enumerable.Range(0, WaveIn.DeviceCount).Select(index => WaveIn.GetCapabilities(index)).ToList();
        public List<WaveOutCapabilities> GetHeadPhoneAvailables => Enumerable.Range(0, WaveOut.DeviceCount).Select(index => WaveOut.GetCapabilities(index)).ToList();
        public void AddMicrofoneAudioHandler(Action<WaveInEventArgs> MicroFoneHandler) => waveIn.DataAvailable += (sender, e) => MicroFoneHandler(e);
        public void ChangeMicrofoneIndex(int index) => waveIn.DeviceNumber = index;
        public void ChangeHeadPhoneIndex(int index) => waveOut.DeviceNumber = index;
        public void StartMicrofoneListener() => waveIn.StartRecording();
        public void StopMicrofoneListener() => waveIn.StopRecording();
        public void ReproduceAudio(byte[] Buffer, int bytesRecorded) => provider.AddSamples(Buffer, 0, bytesRecorded);
        public void StartHeadPhoneListener() => waveOut.Play();
        public void StopHeadPhoneListener() => waveOut.Stop();

        internal void ReproduceAudio(object buffer, object lenght)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Android.Media;
using System.Threading.Tasks;

namespace Example_WorkingWithAudio
{
	//
	// This class shows how to use the low level class AudioTrack in order to play Audio.
	//
	class LowLevelPlayAudio : INotificationReceiver
	{
		static string filePath = "/data/data/Example_WorkingWithAudio.Example_WorkingWithAudio/files/testAudio.mp4";
		byte[] buffer = null;
		AudioTrack audioTrack = null;

		public async Task PlaybackAsync ()
		{
			FileStream fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new BinaryReader (fileStream);
			long totalBytes = new System.IO.FileInfo (filePath).Length;
			buffer = binaryReader.ReadBytes ((Int32)totalBytes);
			fileStream.Close ();
			fileStream.Dispose ();
			binaryReader.Close ();
            Analyse(buffer);
			await PlayAudioTrackAsync ();
		}

		protected async Task PlayAudioTrackAsync ()
		{
			audioTrack = new AudioTrack (
                // Stream type
				Android.Media.Stream.Music,
                // Frequency
				11025,
                // Mono or stereo
				ChannelOut.Mono,
                // Audio encoding
                Android.Media.Encoding.Pcm16bit,
                //Android.Media.Encoding.PcmFloat,

                // Length of the audio clip.
                buffer.Length,
                // Mode. Stream or static.
				AudioTrackMode.Stream);

			audioTrack.Play ();

			await audioTrack.WriteAsync (buffer, 0, buffer.Length);
		}

		public async Task StartAsync ()
		{
			await PlaybackAsync ();
		}

		public void Stop ()
		{
			if (audioTrack != null) {
				audioTrack.Stop ();
				audioTrack.Release ();
				audioTrack = null;
			}
		}

        public void Analyse(byte[] buffer)
        {
            var sampleList = new List<Int16>();
            for (int n = 0; n < buffer.Length; n += 2)
            {
                Int16 sample = BitConverter.ToInt16(buffer, n);
                sampleList.Add(sample);
            }

            SendToRecordingArray(sampleList);
        }

        private void SendToRecordingArray(List<Int16> toSet)
        {           
            RecordingSingleton.SetSamples(toSet);
        }

    }
}

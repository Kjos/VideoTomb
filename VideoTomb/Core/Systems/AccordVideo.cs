using Accord.Audio;
using Accord.Video.FFMPEG;
using VideoTomb.Core.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using Accord.Audio.Generators;

[assembly: InternalsVisibleTo("TestProject")]
namespace VideoTomb.Core.Systems
{
    class AccordVideo : Manager
    {
        private List<Thread> threads = new List<Thread>();

        public AccordVideo(string inputPath, Parameters descr)
            : base(inputPath, descr)
        {
        }

        public override void Process(System.Windows.Forms.ProgressBar progressBar = null)
        {
            ImageProcessor proc = ImageProcessor.None;
            this.Process(ref proc, progressBar);
        }


        public void Process(ref ImageProcessor imageProcessor, System.Windows.Forms.ProgressBar progressBar = null)
        {
            base.CheckPath();

            string outputPath = this.descr.OutputDir + "\\" + Path.GetFileName(inputPath);
            string wavPath = this.descr.OutputDir + "\\Audio_" + Path.GetFileNameWithoutExtension(inputPath) + ".wav";
            string wavNoisePath = this.descr.OutputDir + "\\Noise_" + Path.GetFileNameWithoutExtension(inputPath) + ".wav";
            string wavNoiseInvPath = this.descr.OutputDir + "\\NoiseInv_" + Path.GetFileNameWithoutExtension(inputPath) + ".wav";

            int parallel = Constants.Threads;
            int mid = parallel / 2;
            this.threads.Clear();

            VideoFileReader reader = new VideoFileReader();
            reader.Open(this.inputPath);

            bool hasAudio = reader.AudioCodec != AudioCodec.None;

            int start = 0;
            int end = this.descr.ShortSample ? Math.Min(300, (int)reader.FrameCount) : (int)reader.FrameCount;
            int length = end - start;

            long readCounter = start;// (int)reader.FrameCount / 4 * 3;

            int width = reader.Width;
            int height = reader.Height;

            if (progressBar != null)
            {
                progressBar.Invoke((MethodInvoker)delegate ()
                {
                    progressBar.Visible = true;
                    progressBar.Value = 0;
                    progressBar.Maximum = length;
                    progressBar.Step = 1;
                });
            }

            if (imageProcessor == ImageProcessor.None)
            {
                imageProcessor = new ImageProcessor(this.descr, width, height);
            } else if (imageProcessor.Width != width || imageProcessor.Height != height)
            {
                reader?.Close();
                throw new ArgumentException("Videos width and/or height do not match!");
            }

            long[][] timeRanges = Util.Util.ParseRanges(descr.TimeRanges, (float)reader.FrameRate);
            long cutFrame = (long)(descr.CutStart * (float)reader.FrameRate);

            ImageProcessor iproc = imageProcessor;

            VideoFileWriter writer = new VideoFileWriter();
            writer.Width = width;
            writer.Height = height;
            writer.FrameRate = reader.FrameRate;
            writer.VideoCodec = VideoCodec.H264;
            writer.PixelFormat = AVPixelFormat.FormatYuv420P;
            writer.BitRate = reader.BitRate * descr.BitrateMultiplier;
            writer.SampleRate = reader.SampleRate;
            writer.AudioBitRate = reader.BitRate;
            if (hasAudio)
            {
                writer.AudioCodec = descr.RemoveSound ? AudioCodec.None : AudioCodec.Aac;
                writer.AudioLayout = reader.AudioLayout;
                writer.FrameSize = 1;
                writer.SampleFormat = AVSampleFormat.Format32bitFloatPlanar;
            } else
            {
                writer.AudioCodec = AudioCodec.None;
            }
            writer.VideoOptions["preset"] = "veryfast";
            writer.Open(outputPath);

            long panoCounter = readCounter;
            long writeCounter = readCounter;

            DateTime startTime = DateTime.Now;

            object panoSync = new object();
            IList<byte> audio = new List<byte>();

            for (int t = 0; t < parallel; t++)
            {
                Thread thread = new Thread(() =>
                {
                    Bitmap srcBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                    BitmapData srcData = srcBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                    BitmapData dstData = new Bitmap(width, height, PixelFormat.Format24bppRgb).LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                    while (!this.aborted)
                    {
                        long frameId;
                        lock (reader)
                        {
                            frameId = Interlocked.Read(ref readCounter);

                            if (frameId == end)
                            {
                                return;
                            }

                            reader.ReadVideoFrame((int)frameId, srcData, audio);

                            Interlocked.Increment(ref readCounter);
                        }

                        bool doCut = cutFrame > frameId;

                        bool doEncode = Util.Util.InRange(timeRanges, frameId);

                        if (doEncode)
                        {
                            iproc.Encrypt(srcData, dstData, (int)frameId);
                        }

                        while (!this.aborted)
                        {
                            long wc = Interlocked.Read(ref writeCounter);
                            if (wc == frameId)
                            {
                                lock (writer)
                                {
                                    if (!doCut)
                                    {
                                        if (doEncode)
                                        {
                                            writer.WriteVideoFrame(dstData);
                                        }
                                        else
                                        {
                                            writer.WriteVideoFrame(srcData);
                                        }
                                    }
                                }

                                Interlocked.Increment(ref writeCounter);

                                if (progressBar != null)
                                {
                                    progressBar.Invoke((MethodInvoker)delegate ()
                                    {
                                        progressBar.Increment(1);
                                    });
                                }
                                break;
                            }
                            Thread.Sleep(10);
                        }
                    }
                });
                thread.Name = "Video Thread " + t;
                thread.Start();
                this.threads.Add(thread);
            }

            foreach (Thread thread in this.threads)
            {
                thread.Join();
            }

            if (this.aborted)
            {
                Console.WriteLine("Transcoding aborted");
            }

            if (hasAudio && !descr.RemoveSound)
            {
                if (audio.Count > 0)
                {
                    SampleFormat format;
                    int channels = writer.NumberOfChannels;
                    Console.WriteLine("AudioChannels: " + channels);
                    int ssize = 4;
                    switch (reader.AudioSampleFormat)
                    {
                        case AVSampleFormat.Format16bitSigned:
                            format = SampleFormat.Format16Bit;
                            ssize = 2;
                            break;
                        case AVSampleFormat.Format64bitDouble:
                        case AVSampleFormat.Format64bitDoublePlanar:
                            format = SampleFormat.Format64BitIeeeFloat;
                            ssize = 8;
                            break;
                        case AVSampleFormat.Format32bitSignedPlanar:
                        case AVSampleFormat.Format32bitSigned:
                            format = SampleFormat.Format32Bit;
                            break;
                        case AVSampleFormat.Format32bitFloat:
                        case AVSampleFormat.Format32bitFloatPlanar:
                            format = SampleFormat.Format32BitIeeeFloat;
                            break;
                        case AVSampleFormat.Format8bitUnsigned:
                        case AVSampleFormat.Format8bitUnsignedPlanar:
                            format = SampleFormat.Format8BitUnsigned;
                            ssize = 1;
                            break;
                        case AVSampleFormat.Format8bitSignedPlanar:
                            format = SampleFormat.Format8Bit;
                            ssize = 1;
                            break;
                        default:
                            throw new Exception("Audio not found");
                    }
                    Console.WriteLine("Audio Format: " + reader.AudioSampleFormat);
                    Console.WriteLine("Audio Sample Size: " + ssize);

                    // Cut audio from start
                    int cframe = (int)((float)cutFrame * (float)reader.SampleRate / (float)reader.FrameRate) * channels * ssize;
                    audio = audio.Skip(cframe).ToList();

                    byte[] audioArray = audio.ToArray();
                    int samples = audioArray.Length / ssize;

                    Signal s = Signal.FromArray(audioArray, samples, channels, reader.SampleRate, format);

                    if (this.descr.EncryptAudio)
                    {
                        float[] floats = s.ToFloat();

                        Random rand = new Random();
                        float[] randFloats = new float[floats.Length / channels];
                        float noiseFreq = 0.1f;
                        for (int i = 0; i < randFloats.Length; i++)
                        {
                            float sgn = rand.Next() % 2 == 0 ? 1 : -1;
                            float val = (float)rand.NextDouble();
                            randFloats[i] = (float)Math.Pow(val, noiseFreq) * sgn;
                        }

                        float noiseLp = 0.9f;
                        float noiseVal = randFloats[0];
                        for (int i = 0; i < randFloats.Length; i++)
                        {
                            noiseVal = noiseVal * noiseLp + randFloats[i] * (1f - noiseLp);
                            randFloats[i] = noiseVal;
                        }
                        for (int i = randFloats.Length-1; i > 0; i--)
                        {
                            noiseVal = noiseVal * noiseLp + randFloats[i] * (1f - noiseLp);
                            randFloats[i] = noiseVal;
                        }

                        for (int p = 0, f = 0; p < floats.Length; p+=channels, f++)
                        {
                            bool doEncode = Util.Util.InRange(timeRanges, (long)((float)f * (float)reader.FrameRate / (float)reader.SampleRate));
                            if (doEncode)
                            {
                                float val = (floats[p] + floats[p + 1]) / 2f;

                                float rem = 1f - Math.Abs(val);
                                float mix = randFloats[f] * rem;

                                floats[p] = val + mix;
                                floats[p + 1] = val - mix;
                            }
                        }

                        s = Signal.FromArray(floats, samples, channels, reader.SampleRate, SampleFormat.Format32BitIeeeFloat);
                    }

                    s.Save(wavPath);
                    writer.WriteAudioFrame(s);
                    s.Dispose();
                }
            }

            TimeSpan diff = DateTime.Now.Subtract(startTime);
            Console.WriteLine("Took: " + diff.TotalMinutes + " minutes");

            writer?.Close();
            reader?.Close();

            if (descr.Preview)
            {
                Util.Util.Preview(this.descr.OutputDir, outputPath, descr.IsNoise ? descr.NoiseHash : imageProcessor.MaskImage, descr.IsNoise);
            }
        }
    }
}

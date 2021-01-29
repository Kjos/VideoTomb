using VideoTomb.Core.Systems;
using VideoTomb.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using VideoTomb.Core.Processing;

namespace VideoTomb.Core.Systems
{
    class BatchVideo : Manager
    {
        private AccordVideo current = null;

        public BatchVideo(string inputPath, Parameters descr)
            : base(inputPath, descr)
        {
        }

        public override void Process(System.Windows.Forms.ProgressBar progressBar = null)
        {
            base.CheckPath();

            {
                var ext = new List<string>(FileUtil.SupportedVideos);
                IEnumerable<string> inputs = Directory.GetFiles(this.inputPath, "*.*", SearchOption.AllDirectories)
                        .Where(s => ext.Contains(Path.GetExtension(s)));

                ImageProcessor imageProcessor = ImageProcessor.None;

                foreach (string input in inputs)
                {
                    Thread t = new Thread(() =>
                    {
                        try
                        {
                            this.current = new AccordVideo(input, this.descr);
                            this.current.Process(ref imageProcessor, progressBar);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(input + ": " + ex.Message, "Error");
                        }
                    });
                    t.Start();
                    t.Join();

                    this.current = null;

                    if (this.aborted)
                    {
                        return;
                    }
                }
            }
        }

        public override void Abort()
        {
            base.Abort();
            this.current?.Abort();
        }
    }
}

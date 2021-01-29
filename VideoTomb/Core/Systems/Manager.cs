using System;
using System.IO;
using VideoTomb.Core.Processing;

namespace VideoTomb.Core.Systems
{
    abstract class Manager
    {
        protected string inputPath;
        protected bool aborted = false;
        protected Parameters descr;

        public bool IsAborted
        {
            get
            {
                return this.aborted;
            }
        }

        public Manager(string inputPath, Parameters descr)
        {
            this.inputPath = inputPath;
            this.descr = descr;
        }

        public abstract void Process(System.Windows.Forms.ProgressBar progressBar = null);

        public virtual void Abort()
        {
            this.aborted = true;
        }

        public void CheckPath()
        {
            string inputDir;
            if (Path.HasExtension(this.inputPath))
            {
                inputDir = this.inputPath.Substring(0, this.inputPath.LastIndexOf('\\') + 1);
            } else
            {
                inputDir = this.inputPath + "\\";
            }

            if (inputDir.Equals(this.descr.OutputDir))
            {
                throw new Exception("Input path is same as output path.");
            }
        }
    }
}

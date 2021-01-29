using VideoTomb.Core.Systems;
using VideoTomb.Core.Util;
using MaterialSkin.Controls;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using VideoTomb.Core.Processing;
using Windows.Services.Store;
using System.Drawing;
using MaterialSkin;
using System.Diagnostics;

namespace VideoTomb
{
    public partial class Form1 : MaterialForm
    {
        private Manager processor = null;
        private Thread thread = null;

        private OpenFileDialog inputDialog;
        private OpenFileDialog maskDialog;
        private FolderBrowserDialog directoryDialog;

        public Form1()
        {
            InitializeComponent();

            this.SkinManager.ColorScheme = new ColorScheme(Primary.Orange600, Primary.DeepOrange600, Primary.Orange600, Accent.Yellow100, TextShade.BLACK);

            this.outputPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            this.inputDialog.Filter = "Video Files (*.mp4;*.mov;*.webm)|*.MP4;*.MOV;*.WEBM";
            this.maskDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.bmp)|*.PNG;*.JPEG;*.JPG;*.BMP";
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.videotomb.com");
        }

        private void InputButton_Click(object sender, EventArgs e)
        {
            if (inputDialog.ShowDialog() == DialogResult.OK)
            {
                this.inputPath.Text = inputDialog.FileName;
            }
        }

        private void InputDirectoryButton_Click(object sender, EventArgs e)
        {
            if (directoryDialog.ShowDialog() == DialogResult.OK)
            {
                this.inputPath.Text = directoryDialog.SelectedPath;
            }
        }

        private void OutputDirectoryButton_Click(object sender, EventArgs e)
        {
            if (directoryDialog.ShowDialog() == DialogResult.OK)
            {
                this.outputPath.Text = directoryDialog.SelectedPath;
            }
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (processor != null)
            {
                this.processor.Abort();
            }
            else
            {
                string inputPath = this.inputPath.Text;
                if (!PathExists(inputPath, out bool isDir))
                {
                    return;
                }

                string outputPath = this.outputPath.Text;
                if (!PathExists(outputPath, out bool isOutputDir))
                {
                    return;
                }
                if (!isOutputDir)
                {
                    return;
                }
                     
                string outputDir = Util.CreateOutputDir(inputPath, outputPath, isDir);

                Parameters descr;
                if (this.noiseHashRadio.Checked)
                {
                    descr = new Parameters((int)this.resolutionInput.Value, (int)this.minHeight.Value, this.vertLines.Checked, this.shortSample.Checked,
                        this.normalizeMask.Checked, (int)this.blur.Value, this.removeSound.Checked, this.randomLines.Checked, (int)this.bitrateMult.Value,
                        this.preview.Checked, this.timeRanges.Text, this.encryptAudio.Checked, (float)this.cutStart.Value, outputDir);
                    descr.UpdateNoiseTextBox = this.noiseHashText;
                } else
                {
                    descr = new Parameters(this.imageMaskPath.Text, (int)this.resolutionInput.Value, (int)this.minHeight.Value, this.vertLines.Checked, this.shortSample.Checked,
                        this.normalizeMask.Checked, (int)this.blur.Value, this.removeSound.Checked, this.randomLines.Checked, (int)this.bitrateMult.Value,
                        this.preview.Checked, this.timeRanges.Text, this.encryptAudio.Checked, (float)this.cutStart.Value, outputDir);
                }

                if (isDir)
                {
                    this.processor = new BatchVideo(inputPath, descr);
                } else if (FileUtil.IsVideo(inputPath))
                {
                    this.processor = new AccordVideo(inputPath, descr);
                } else
                {
                    MessageBox.Show("Incompatible input.", "Error");
                    return;
                }

                this.startButton.Text = "Abort";

                this.thread = new Thread(() =>
                {
                    try
                    {
                        this.processor.Process(this.progressBar);

                        if (this.processor.IsAborted)
                        {
                            MessageBox.Show("Aborted.", "Success");
                        }
                        else
                        {
                            MessageBox.Show("Finished processing.", "Success");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }

                    this.startButton.Invoke((MethodInvoker)delegate ()
                    {
                        this.startButton.Text = "Start";
                    });
                    this.progressBar.Invoke((MethodInvoker)delegate ()
                    {
                        this.progressBar.Visible = false;
                    });

                    this.processor = null;
                });
                this.thread.Name = "Main Thread";
                this.thread.Start();
            }
        }

        public static bool PathExists(string path, out bool isDirectory)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path == string.Empty) throw new ArgumentException("Value cannot be empty.", nameof(path));

            isDirectory = Directory.Exists(path);

            return isDirectory || (File.Exists(path) && Path.HasExtension(path));
        }

        private void Input_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string file = files[0];

            try
            {
                if (PathExists(file, out bool isDirectory))
                {
                    if (isDirectory || FileUtil.IsVideo(file))
                    {
                        this.inputPath.Text = file;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Invalid input.", "Error");
            }
        }

        private void MaskButton_Click(object sender, EventArgs e)
        {
            if (maskDialog.ShowDialog() == DialogResult.OK)
            {
                this.imageMaskPath.Text = maskDialog.FileName;
                this.imageMaskRadio.Checked = true;
            }
        }

        private void UpdateUrl()
        {
            if (this.noiseHashLinkRadio.Checked)
            {
                this.linkResult.Text = Url.GenerateHashUrl(this.videoUrl.Text, this.noiseHashLink.Text, (int)this.adDelay.Value);
            }
            else
            {
                this.linkResult.Text = Url.GenerateImageUrl(this.videoUrl.Text, this.imageMaskUrl.Text, (int)this.adDelay.Value);
            }
        }

        private void UpdateUrl(object sender, EventArgs e)
        {
            this.UpdateUrl();
        }

        private void UpdateUrl(object sender, KeyEventArgs e)
        {
            this.UpdateUrl();
        }

        private void NoiseHashLink_KeyDown(object sender, KeyEventArgs e)
        {
            Keys k = e.KeyCode;
            e.SuppressKeyPress = !(k == Keys.Delete || k == Keys.Back || (k >= Keys.A && k <= Keys.Z) || (k >= Keys.D0 && k <= Keys.D9));

            this.UpdateUrl(sender, e);
        }

        private void NoiseHash_KeyDown(object sender, KeyEventArgs e)
        {
            Keys k = e.KeyCode;
            e.SuppressKeyPress = !(k == Keys.Delete || k == Keys.Back || (k >= Keys.A && k <= Keys.Z) || (k >= Keys.D0 && k <= Keys.D9));
        }

        private void TimeRanges_KeyDown(object sender, KeyEventArgs e)
        {
            Keys k = e.KeyCode;
            e.SuppressKeyPress = !(k == Keys.Delete || k == Keys.Back || k == Keys.OemMinus || k == Keys.Oemcomma || k == Keys.OemPeriod || (k >= Keys.D0 && k <= Keys.D9));
        }

        private void OpenLink_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.linkResult.Text))
            {
                Process.Start(this.linkResult.Text);
            }
        }
    }
}

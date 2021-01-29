using VideoTomb.Core.Systems;
using VideoTomb.Core.Util;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace VideoTomb
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.inputDialog = new System.Windows.Forms.OpenFileDialog();
            this.maskDialog = new System.Windows.Forms.OpenFileDialog();
            this.directoryDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.inputPath = new System.Windows.Forms.Label();
            this.inputButton = new System.Windows.Forms.Button();
            this.outputButton = new System.Windows.Forms.Button();
            this.outputPath = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.inputDirectoryButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.encryptAudio = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.timeRanges = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.bitrateMult = new System.Windows.Forms.NumericUpDown();
            this.preview = new System.Windows.Forms.CheckBox();
            this.randomLines = new System.Windows.Forms.CheckBox();
            this.removeSound = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.blur = new System.Windows.Forms.NumericUpDown();
            this.normalizeMask = new System.Windows.Forms.CheckBox();
            this.vertLines = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.minHeight = new System.Windows.Forms.NumericUpDown();
            this.resolutionInput = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.shortSample = new System.Windows.Forms.CheckBox();
            this.startButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.noiseHashText = new System.Windows.Forms.TextBox();
            this.noiseHashRadio = new System.Windows.Forms.RadioButton();
            this.imageMaskRadio = new System.Windows.Forms.RadioButton();
            this.maskButton = new System.Windows.Forms.Button();
            this.imageMaskPath = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.adDelay = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.noiseHashLinkRadio = new System.Windows.Forms.RadioButton();
            this.noiseHashLink = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.openLink = new System.Windows.Forms.Button();
            this.linkResult = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.imageMaskLinkRadio = new System.Windows.Forms.RadioButton();
            this.imageMaskUrl = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.videoUrl = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cutStart = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitrateMult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionInput)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adDelay)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cutStart)).BeginInit();
            this.SuspendLayout();
            // 
            // inputDialog
            // 
            resources.ApplyResources(this.inputDialog, "inputDialog");
            // 
            // inputPath
            // 
            resources.ApplyResources(this.inputPath, "inputPath");
            this.inputPath.Name = "inputPath";
            // 
            // inputButton
            // 
            resources.ApplyResources(this.inputButton, "inputButton");
            this.inputButton.Name = "inputButton";
            this.inputButton.UseVisualStyleBackColor = true;
            this.inputButton.Click += new System.EventHandler(this.InputButton_Click);
            // 
            // outputButton
            // 
            resources.ApplyResources(this.outputButton, "outputButton");
            this.outputButton.Name = "outputButton";
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.OutputDirectoryButton_Click);
            // 
            // outputPath
            // 
            resources.ApplyResources(this.outputPath, "outputPath");
            this.outputPath.Name = "outputPath";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.inputDirectoryButton);
            this.groupBox1.Controls.Add(this.inputButton);
            this.groupBox1.Controls.Add(this.inputPath);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // inputDirectoryButton
            // 
            resources.ApplyResources(this.inputDirectoryButton, "inputDirectoryButton");
            this.inputDirectoryButton.Name = "inputDirectoryButton";
            this.inputDirectoryButton.UseVisualStyleBackColor = true;
            this.inputDirectoryButton.Click += new System.EventHandler(this.InputDirectoryButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.outputPath);
            this.groupBox2.Controls.Add(this.outputButton);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.cutStart);
            this.groupBox3.Controls.Add(this.encryptAudio);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.timeRanges);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.bitrateMult);
            this.groupBox3.Controls.Add(this.preview);
            this.groupBox3.Controls.Add(this.randomLines);
            this.groupBox3.Controls.Add(this.removeSound);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.blur);
            this.groupBox3.Controls.Add(this.normalizeMask);
            this.groupBox3.Controls.Add(this.vertLines);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.minHeight);
            this.groupBox3.Controls.Add(this.resolutionInput);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.shortSample);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // encryptAudio
            // 
            resources.ApplyResources(this.encryptAudio, "encryptAudio");
            this.encryptAudio.Name = "encryptAudio";
            this.encryptAudio.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // timeRanges
            // 
            resources.ApplyResources(this.timeRanges, "timeRanges");
            this.timeRanges.Name = "timeRanges";
            this.timeRanges.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TimeRanges_KeyDown);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // bitrateMult
            // 
            resources.ApplyResources(this.bitrateMult, "bitrateMult");
            this.bitrateMult.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.bitrateMult.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bitrateMult.Name = "bitrateMult";
            this.bitrateMult.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // preview
            // 
            resources.ApplyResources(this.preview, "preview");
            this.preview.Checked = true;
            this.preview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.preview.Name = "preview";
            this.preview.UseVisualStyleBackColor = true;
            // 
            // randomLines
            // 
            resources.ApplyResources(this.randomLines, "randomLines");
            this.randomLines.Name = "randomLines";
            this.randomLines.UseVisualStyleBackColor = true;
            // 
            // removeSound
            // 
            resources.ApplyResources(this.removeSound, "removeSound");
            this.removeSound.Name = "removeSound";
            this.removeSound.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // blur
            // 
            resources.ApplyResources(this.blur, "blur");
            this.blur.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.blur.Name = "blur";
            this.blur.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // normalizeMask
            // 
            resources.ApplyResources(this.normalizeMask, "normalizeMask");
            this.normalizeMask.Checked = true;
            this.normalizeMask.CheckState = System.Windows.Forms.CheckState.Checked;
            this.normalizeMask.Name = "normalizeMask";
            this.normalizeMask.UseVisualStyleBackColor = true;
            // 
            // vertLines
            // 
            resources.ApplyResources(this.vertLines, "vertLines");
            this.vertLines.Checked = true;
            this.vertLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.vertLines.Name = "vertLines";
            this.vertLines.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // minHeight
            // 
            resources.ApplyResources(this.minHeight, "minHeight");
            this.minHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.minHeight.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.minHeight.Name = "minHeight";
            this.minHeight.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
            // 
            // resolutionInput
            // 
            resources.ApplyResources(this.resolutionInput, "resolutionInput");
            this.resolutionInput.Name = "resolutionInput";
            this.resolutionInput.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // shortSample
            // 
            resources.ApplyResources(this.shortSample, "shortSample");
            this.shortSample.Name = "shortSample";
            this.shortSample.UseVisualStyleBackColor = true;
            // 
            // startButton
            // 
            resources.ApplyResources(this.startButton, "startButton");
            this.startButton.Name = "startButton";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            // 
            // linkLabel
            // 
            this.linkLabel.ActiveLinkColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.linkLabel, "linkLabel");
            this.linkLabel.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel.LinkColor = System.Drawing.Color.Black;
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.TabStop = true;
            this.linkLabel.VisitedLinkColor = System.Drawing.Color.DarkOrange;
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.noiseHashText);
            this.groupBox4.Controls.Add(this.noiseHashRadio);
            this.groupBox4.Controls.Add(this.imageMaskRadio);
            this.groupBox4.Controls.Add(this.maskButton);
            this.groupBox4.Controls.Add(this.imageMaskPath);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // noiseHashText
            // 
            resources.ApplyResources(this.noiseHashText, "noiseHashText");
            this.noiseHashText.Name = "noiseHashText";
            this.noiseHashText.ReadOnly = true;
            this.noiseHashText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NoiseHash_KeyDown);
            // 
            // noiseHashRadio
            // 
            resources.ApplyResources(this.noiseHashRadio, "noiseHashRadio");
            this.noiseHashRadio.Checked = true;
            this.noiseHashRadio.Name = "noiseHashRadio";
            this.noiseHashRadio.TabStop = true;
            this.noiseHashRadio.UseVisualStyleBackColor = true;
            // 
            // imageMaskRadio
            // 
            resources.ApplyResources(this.imageMaskRadio, "imageMaskRadio");
            this.imageMaskRadio.Name = "imageMaskRadio";
            this.imageMaskRadio.UseVisualStyleBackColor = true;
            // 
            // maskButton
            // 
            resources.ApplyResources(this.maskButton, "maskButton");
            this.maskButton.Name = "maskButton";
            this.maskButton.UseVisualStyleBackColor = true;
            this.maskButton.Click += new System.EventHandler(this.MaskButton_Click);
            // 
            // imageMaskPath
            // 
            resources.ApplyResources(this.imageMaskPath, "imageMaskPath");
            this.imageMaskPath.Name = "imageMaskPath";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.progressBar);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.startButton);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox9);
            this.tabPage2.Controls.Add(this.groupBox8);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.adDelay);
            this.groupBox9.Controls.Add(this.label8);
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // adDelay
            // 
            resources.ApplyResources(this.adDelay, "adDelay");
            this.adDelay.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.adDelay.Name = "adDelay";
            this.adDelay.ValueChanged += new System.EventHandler(this.UpdateUrl);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label10);
            this.groupBox8.Controls.Add(this.noiseHashLinkRadio);
            this.groupBox8.Controls.Add(this.noiseHashLink);
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // noiseHashLinkRadio
            // 
            resources.ApplyResources(this.noiseHashLinkRadio, "noiseHashLinkRadio");
            this.noiseHashLinkRadio.Name = "noiseHashLinkRadio";
            this.noiseHashLinkRadio.UseVisualStyleBackColor = true;
            // 
            // noiseHashLink
            // 
            resources.ApplyResources(this.noiseHashLink, "noiseHashLink");
            this.noiseHashLink.Name = "noiseHashLink";
            this.noiseHashLink.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NoiseHashLink_KeyDown);
            this.noiseHashLink.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateUrl);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.openLink);
            this.groupBox7.Controls.Add(this.linkResult);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // openLink
            // 
            resources.ApplyResources(this.openLink, "openLink");
            this.openLink.Name = "openLink";
            this.openLink.UseVisualStyleBackColor = true;
            this.openLink.Click += new System.EventHandler(this.OpenLink_Click);
            // 
            // linkResult
            // 
            resources.ApplyResources(this.linkResult, "linkResult");
            this.linkResult.Name = "linkResult";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.imageMaskLinkRadio);
            this.groupBox6.Controls.Add(this.imageMaskUrl);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // imageMaskLinkRadio
            // 
            resources.ApplyResources(this.imageMaskLinkRadio, "imageMaskLinkRadio");
            this.imageMaskLinkRadio.Checked = true;
            this.imageMaskLinkRadio.Name = "imageMaskLinkRadio";
            this.imageMaskLinkRadio.TabStop = true;
            this.imageMaskLinkRadio.UseVisualStyleBackColor = true;
            // 
            // imageMaskUrl
            // 
            resources.ApplyResources(this.imageMaskUrl, "imageMaskUrl");
            this.imageMaskUrl.Name = "imageMaskUrl";
            this.imageMaskUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UpdateUrl);
            this.imageMaskUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateUrl);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.videoUrl);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // videoUrl
            // 
            resources.ApplyResources(this.videoUrl, "videoUrl");
            this.videoUrl.Name = "videoUrl";
            this.videoUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UpdateUrl);
            this.videoUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateUrl);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cutStart
            // 
            this.cutStart.DecimalPlaces = 1;
            this.cutStart.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.cutStart, "cutStart");
            this.cutStart.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.cutStart.Name = "cutStart";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.linkLabel);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Sizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Input_DragDrop);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitrateMult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionInput)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adDelay)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cutStart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label inputPath;
        private System.Windows.Forms.Button inputButton;
        private System.Windows.Forms.Button outputButton;
        private System.Windows.Forms.Label outputPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button startButton;

        private Button inputDirectoryButton;
        private ProgressBar progressBar;
        private LinkLabel linkLabel;
        private CheckBox shortSample;
        private GroupBox groupBox4;
        private TextBox noiseHashText;
        private RadioButton noiseHashRadio;
        private RadioButton imageMaskRadio;
        private Button maskButton;
        private Label imageMaskPath;
        private Label label2;
        private NumericUpDown resolutionInput;
        private CheckBox vertLines;
        private Label label4;
        private NumericUpDown minHeight;
        private CheckBox normalizeMask;
        private Label label1;
        private NumericUpDown blur;
        private CheckBox removeSound;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private GroupBox groupBox6;
        private TextBox noiseHashLink;
        private RadioButton noiseHashLinkRadio;
        private RadioButton imageMaskLinkRadio;
        private TextBox imageMaskUrl;
        private GroupBox groupBox5;
        private TextBox videoUrl;
        private TextBox linkResult;
        private GroupBox groupBox7;
        private CheckBox randomLines;
        private Label label5;
        private CheckBox preview;
        private Label label6;
        private NumericUpDown bitrateMult;
        private GroupBox groupBox8;
        private Label label10;
        private Label label12;
        private TextBox timeRanges;
        private GroupBox groupBox9;
        private NumericUpDown adDelay;
        private Label label8;
        private Button openLink;
        private CheckBox encryptAudio;
        private Label label7;
        private NumericUpDown cutStart;
    }
}


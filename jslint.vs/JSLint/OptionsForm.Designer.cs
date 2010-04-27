namespace JSLint
{
    partial class OptionsForm
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
            this.components = new System.ComponentModel.Container();
            this.chkIntegrateWithBuild = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.treeSolution = new System.Windows.Forms.TreeView();
            this.groupIntegrateWithBuild = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupCheckingScope = new System.Windows.Forms.GroupBox();
            this.chkScheckingScope = new System.Windows.Forms.CheckBox();
            this.chkStopBuildOnErrors = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numJSLintProcessWaitTime = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupJSLintOptions = new System.Windows.Forms.GroupBox();
            this.chkListJSLintOptions = new System.Windows.Forms.CheckedListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClearJSLintOptions = new System.Windows.Forms.Button();
            this.btnRecommendedJSLintOptions = new System.Windows.Forms.Button();
            this.btnGoodJavaScriptParts = new System.Windows.Forms.Button();
            this.groupIntegrateWithBuild.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupCheckingScope.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numJSLintProcessWaitTime)).BeginInit();
            this.groupJSLintOptions.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkIntegrateWithBuild
            // 
            this.chkIntegrateWithBuild.AutoSize = true;
            this.chkIntegrateWithBuild.Location = new System.Drawing.Point(9, 232);
            this.chkIntegrateWithBuild.Name = "chkIntegrateWithBuild";
            this.chkIntegrateWithBuild.Size = new System.Drawing.Size(116, 17);
            this.chkIntegrateWithBuild.TabIndex = 0;
            this.chkIntegrateWithBuild.Text = "Integrate with Build";
            this.chkIntegrateWithBuild.UseVisualStyleBackColor = true;
            // 
            // treeSolution
            // 
            this.treeSolution.CheckBoxes = true;
            this.treeSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeSolution.Location = new System.Drawing.Point(3, 16);
            this.treeSolution.Name = "treeSolution";
            this.treeSolution.Size = new System.Drawing.Size(347, 216);
            this.treeSolution.TabIndex = 1;
            // 
            // groupIntegrateWithBuild
            // 
            this.groupIntegrateWithBuild.Controls.Add(this.tableLayoutPanel1);
            this.groupIntegrateWithBuild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupIntegrateWithBuild.Location = new System.Drawing.Point(0, 233);
            this.groupIntegrateWithBuild.Name = "groupIntegrateWithBuild";
            this.groupIntegrateWithBuild.Size = new System.Drawing.Size(365, 286);
            this.groupIntegrateWithBuild.TabIndex = 2;
            this.groupIntegrateWithBuild.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.groupCheckingScope, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkStopBuildOnErrors, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.numJSLintProcessWaitTime, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(359, 267);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // groupCheckingScope
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupCheckingScope, 5);
            this.groupCheckingScope.Controls.Add(this.chkScheckingScope);
            this.groupCheckingScope.Controls.Add(this.treeSolution);
            this.groupCheckingScope.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupCheckingScope.Location = new System.Drawing.Point(3, 29);
            this.groupCheckingScope.Name = "groupCheckingScope";
            this.groupCheckingScope.Size = new System.Drawing.Size(353, 235);
            this.groupCheckingScope.TabIndex = 3;
            this.groupCheckingScope.TabStop = false;
            // 
            // chkScheckingScope
            // 
            this.chkScheckingScope.AutoSize = true;
            this.chkScheckingScope.Location = new System.Drawing.Point(11, 0);
            this.chkScheckingScope.Name = "chkScheckingScope";
            this.chkScheckingScope.Size = new System.Drawing.Size(160, 17);
            this.chkScheckingScope.TabIndex = 0;
            this.chkScheckingScope.Text = "Don\'t Check Whole Solution";
            this.chkScheckingScope.UseVisualStyleBackColor = true;
            // 
            // chkStopBuildOnErrors
            // 
            this.chkStopBuildOnErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chkStopBuildOnErrors.AutoSize = true;
            this.chkStopBuildOnErrors.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkStopBuildOnErrors.Location = new System.Drawing.Point(3, 4);
            this.chkStopBuildOnErrors.Name = "chkStopBuildOnErrors";
            this.chkStopBuildOnErrors.Size = new System.Drawing.Size(121, 17);
            this.chkStopBuildOnErrors.TabIndex = 1;
            this.chkStopBuildOnErrors.Text = "Stop Build On Errors";
            this.chkStopBuildOnErrors.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(150, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Kill process after";
            // 
            // numJSLintProcessWaitTime
            // 
            this.numJSLintProcessWaitTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numJSLintProcessWaitTime.Location = new System.Drawing.Point(240, 3);
            this.numJSLintProcessWaitTime.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numJSLintProcessWaitTime.Name = "numJSLintProcessWaitTime";
            this.numJSLintProcessWaitTime.Size = new System.Drawing.Size(38, 20);
            this.numJSLintProcessWaitTime.TabIndex = 3;
            this.numJSLintProcessWaitTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(284, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "seconds";
            // 
            // groupJSLintOptions
            // 
            this.groupJSLintOptions.Controls.Add(this.chkListJSLintOptions);
            this.groupJSLintOptions.Controls.Add(this.flowLayoutPanel1);
            this.groupJSLintOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupJSLintOptions.Location = new System.Drawing.Point(0, 0);
            this.groupJSLintOptions.Name = "groupJSLintOptions";
            this.groupJSLintOptions.Size = new System.Drawing.Size(365, 233);
            this.groupJSLintOptions.TabIndex = 2;
            this.groupJSLintOptions.TabStop = false;
            this.groupJSLintOptions.Text = "JSLint Options";
            // 
            // chkListJSLintOptions
            // 
            this.chkListJSLintOptions.CheckOnClick = true;
            this.chkListJSLintOptions.ColumnWidth = 175;
            this.chkListJSLintOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkListJSLintOptions.FormattingEnabled = true;
            this.chkListJSLintOptions.Items.AddRange(new object[] {
            "Stop on first error",
            "Strict whitespace",
            "Assume a browser",
            "Assume a Yahoo Widget",
            "Assume Rhino",
            "",
            "Tolerate debugger statements",
            "Tolerate eval",
            "Tolerate HTML case",
            "Tolearte HTML event handlers",
            "Tolerate sloppy line breaking",
            "Tolerate unfiltered for in",
            "",
            "Disallow undefined variables",
            "Disallow leading _ in identifiers",
            "Disallow == and !=",
            "Disallow ++ and --",
            "Disallow bitwise operators",
            "ADsafe"});
            this.chkListJSLintOptions.Location = new System.Drawing.Point(3, 16);
            this.chkListJSLintOptions.MultiColumn = true;
            this.chkListJSLintOptions.Name = "chkListJSLintOptions";
            this.chkListJSLintOptions.Size = new System.Drawing.Size(359, 184);
            this.chkListJSLintOptions.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnClearJSLintOptions);
            this.flowLayoutPanel1.Controls.Add(this.btnRecommendedJSLintOptions);
            this.flowLayoutPanel1.Controls.Add(this.btnGoodJavaScriptParts);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 201);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(359, 29);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // btnClearJSLintOptions
            // 
            this.btnClearJSLintOptions.AutoSize = true;
            this.btnClearJSLintOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClearJSLintOptions.Location = new System.Drawing.Point(3, 3);
            this.btnClearJSLintOptions.Name = "btnClearJSLintOptions";
            this.btnClearJSLintOptions.Size = new System.Drawing.Size(94, 23);
            this.btnClearJSLintOptions.TabIndex = 0;
            this.btnClearJSLintOptions.Text = "Clear All Options";
            this.btnClearJSLintOptions.UseVisualStyleBackColor = true;
            this.btnClearJSLintOptions.Click += new System.EventHandler(this.btnClearJSLintOptions_Click);
            // 
            // btnRecommendedJSLintOptions
            // 
            this.btnRecommendedJSLintOptions.AutoSize = true;
            this.btnRecommendedJSLintOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRecommendedJSLintOptions.Location = new System.Drawing.Point(103, 3);
            this.btnRecommendedJSLintOptions.Name = "btnRecommendedJSLintOptions";
            this.btnRecommendedJSLintOptions.Size = new System.Drawing.Size(128, 23);
            this.btnRecommendedJSLintOptions.TabIndex = 1;
            this.btnRecommendedJSLintOptions.Text = "Recommended Options";
            this.btnRecommendedJSLintOptions.UseVisualStyleBackColor = true;
            this.btnRecommendedJSLintOptions.Click += new System.EventHandler(this.btnRecommendedJSLintOptions_Click);
            // 
            // btnGoodJavaScriptParts
            // 
            this.btnGoodJavaScriptParts.AutoSize = true;
            this.btnGoodJavaScriptParts.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnGoodJavaScriptParts.Location = new System.Drawing.Point(237, 3);
            this.btnGoodJavaScriptParts.Name = "btnGoodJavaScriptParts";
            this.btnGoodJavaScriptParts.Size = new System.Drawing.Size(70, 23);
            this.btnGoodJavaScriptParts.TabIndex = 2;
            this.btnGoodJavaScriptParts.Text = "Good Parts";
            this.btnGoodJavaScriptParts.UseVisualStyleBackColor = true;
            this.btnGoodJavaScriptParts.Click += new System.EventHandler(this.btnGoodJavaScriptParts_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 519);
            this.Controls.Add(this.chkIntegrateWithBuild);
            this.Controls.Add(this.groupIntegrateWithBuild);
            this.Controls.Add(this.groupJSLintOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JSLint.VS Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.groupIntegrateWithBuild.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupCheckingScope.ResumeLayout(false);
            this.groupCheckingScope.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numJSLintProcessWaitTime)).EndInit();
            this.groupJSLintOptions.ResumeLayout(false);
            this.groupJSLintOptions.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIntegrateWithBuild;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TreeView treeSolution;
        private System.Windows.Forms.GroupBox groupIntegrateWithBuild;
        private System.Windows.Forms.CheckBox chkStopBuildOnErrors;
        private System.Windows.Forms.GroupBox groupCheckingScope;
        private System.Windows.Forms.CheckBox chkScheckingScope;
        private System.Windows.Forms.GroupBox groupJSLintOptions;
        private System.Windows.Forms.CheckedListBox chkListJSLintOptions;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnClearJSLintOptions;
        private System.Windows.Forms.Button btnRecommendedJSLintOptions;
        private System.Windows.Forms.Button btnGoodJavaScriptParts;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numJSLintProcessWaitTime;
        private System.Windows.Forms.Label label2;
    }
}
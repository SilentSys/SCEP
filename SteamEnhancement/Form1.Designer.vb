<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.VersionLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SCEPTabControl = New System.Windows.Forms.TabControl()
        Me.GeneralTab = New System.Windows.Forms.TabPage()
        Me.MinimizeCheck = New System.Windows.Forms.CheckBox()
        Me.StartupCheck = New System.Windows.Forms.CheckBox()
        Me.SCEPEnabledCheck = New System.Windows.Forms.CheckBox()
        Me.SRTab = New System.Windows.Forms.TabPage()
        Me.InvitesEnabledCheck = New System.Windows.Forms.CheckBox()
        Me.ProfilesEnabledCheck = New System.Windows.Forms.CheckBox()
        Me.TradesTab = New System.Windows.Forms.TabPage()
        Me.TradesEnabledCheck = New System.Windows.Forms.CheckBox()
        Me.SCEPNotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.StatusStrip1.SuspendLayout()
        Me.SCEPTabControl.SuspendLayout()
        Me.GeneralTab.SuspendLayout()
        Me.SRTab.SuspendLayout()
        Me.TradesTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VersionLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 76)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(220, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'VersionLabel
        '
        Me.VersionLabel.Name = "VersionLabel"
        Me.VersionLabel.Size = New System.Drawing.Size(0, 17)
        '
        'SCEPTabControl
        '
        Me.SCEPTabControl.Controls.Add(Me.GeneralTab)
        Me.SCEPTabControl.Controls.Add(Me.SRTab)
        Me.SCEPTabControl.Controls.Add(Me.TradesTab)
        Me.SCEPTabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SCEPTabControl.Location = New System.Drawing.Point(0, 0)
        Me.SCEPTabControl.Name = "SCEPTabControl"
        Me.SCEPTabControl.SelectedIndex = 0
        Me.SCEPTabControl.Size = New System.Drawing.Size(220, 98)
        Me.SCEPTabControl.TabIndex = 4
        '
        'GeneralTab
        '
        Me.GeneralTab.Controls.Add(Me.MinimizeCheck)
        Me.GeneralTab.Controls.Add(Me.StartupCheck)
        Me.GeneralTab.Controls.Add(Me.SCEPEnabledCheck)
        Me.GeneralTab.Location = New System.Drawing.Point(4, 22)
        Me.GeneralTab.Name = "GeneralTab"
        Me.GeneralTab.Padding = New System.Windows.Forms.Padding(3)
        Me.GeneralTab.Size = New System.Drawing.Size(212, 72)
        Me.GeneralTab.TabIndex = 0
        Me.GeneralTab.Text = "General"
        Me.GeneralTab.UseVisualStyleBackColor = True
        '
        'MinimizeCheck
        '
        Me.MinimizeCheck.AutoSize = True
        Me.MinimizeCheck.Location = New System.Drawing.Point(108, 6)
        Me.MinimizeCheck.Name = "MinimizeCheck"
        Me.MinimizeCheck.Size = New System.Drawing.Size(98, 17)
        Me.MinimizeCheck.TabIndex = 2
        Me.MinimizeCheck.Text = "Minimize to tray"
        Me.MinimizeCheck.UseVisualStyleBackColor = True
        '
        'StartupCheck
        '
        Me.StartupCheck.AutoSize = True
        Me.StartupCheck.Location = New System.Drawing.Point(6, 29)
        Me.StartupCheck.Name = "StartupCheck"
        Me.StartupCheck.Size = New System.Drawing.Size(60, 17)
        Me.StartupCheck.TabIndex = 1
        Me.StartupCheck.Text = "Startup"
        Me.StartupCheck.UseVisualStyleBackColor = True
        '
        'SCEPEnabledCheck
        '
        Me.SCEPEnabledCheck.AutoSize = True
        Me.SCEPEnabledCheck.Location = New System.Drawing.Point(6, 6)
        Me.SCEPEnabledCheck.Name = "SCEPEnabledCheck"
        Me.SCEPEnabledCheck.Size = New System.Drawing.Size(90, 17)
        Me.SCEPEnabledCheck.TabIndex = 0
        Me.SCEPEnabledCheck.Text = "Enable SCEP"
        Me.SCEPEnabledCheck.UseVisualStyleBackColor = True
        '
        'SRTab
        '
        Me.SRTab.Controls.Add(Me.InvitesEnabledCheck)
        Me.SRTab.Controls.Add(Me.ProfilesEnabledCheck)
        Me.SRTab.Location = New System.Drawing.Point(4, 22)
        Me.SRTab.Name = "SRTab"
        Me.SRTab.Padding = New System.Windows.Forms.Padding(3)
        Me.SRTab.Size = New System.Drawing.Size(212, 72)
        Me.SRTab.TabIndex = 1
        Me.SRTab.Text = "SteamRep"
        Me.SRTab.UseVisualStyleBackColor = True
        '
        'InvitesEnabledCheck
        '
        Me.InvitesEnabledCheck.AutoSize = True
        Me.InvitesEnabledCheck.Location = New System.Drawing.Point(6, 29)
        Me.InvitesEnabledCheck.Name = "InvitesEnabledCheck"
        Me.InvitesEnabledCheck.Size = New System.Drawing.Size(109, 17)
        Me.InvitesEnabledCheck.TabIndex = 2
        Me.InvitesEnabledCheck.Text = "Enabled in invites"
        Me.InvitesEnabledCheck.UseVisualStyleBackColor = True
        '
        'ProfilesEnabledCheck
        '
        Me.ProfilesEnabledCheck.AutoSize = True
        Me.ProfilesEnabledCheck.Location = New System.Drawing.Point(6, 6)
        Me.ProfilesEnabledCheck.Name = "ProfilesEnabledCheck"
        Me.ProfilesEnabledCheck.Size = New System.Drawing.Size(116, 17)
        Me.ProfilesEnabledCheck.TabIndex = 0
        Me.ProfilesEnabledCheck.Text = "Enabled on profiles"
        Me.ProfilesEnabledCheck.UseVisualStyleBackColor = True
        '
        'TradesTab
        '
        Me.TradesTab.Controls.Add(Me.TradesEnabledCheck)
        Me.TradesTab.Location = New System.Drawing.Point(4, 22)
        Me.TradesTab.Name = "TradesTab"
        Me.TradesTab.Padding = New System.Windows.Forms.Padding(3)
        Me.TradesTab.Size = New System.Drawing.Size(212, 72)
        Me.TradesTab.TabIndex = 3
        Me.TradesTab.Text = "Trades"
        Me.TradesTab.UseVisualStyleBackColor = True
        '
        'TradesEnabledCheck
        '
        Me.TradesEnabledCheck.AutoSize = True
        Me.TradesEnabledCheck.Location = New System.Drawing.Point(6, 6)
        Me.TradesEnabledCheck.Name = "TradesEnabledCheck"
        Me.TradesEnabledCheck.Size = New System.Drawing.Size(65, 17)
        Me.TradesEnabledCheck.TabIndex = 2
        Me.TradesEnabledCheck.Text = "Enabled"
        Me.TradesEnabledCheck.UseVisualStyleBackColor = True
        '
        'SCEPNotifyIcon
        '
        Me.SCEPNotifyIcon.Icon = CType(resources.GetObject("SCEPNotifyIcon.Icon"), System.Drawing.Icon)
        Me.SCEPNotifyIcon.Text = "SCEP"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(220, 98)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.SCEPTabControl)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SCEP Beta"
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.SCEPTabControl.ResumeLayout(False)
        Me.GeneralTab.ResumeLayout(False)
        Me.GeneralTab.PerformLayout()
        Me.SRTab.ResumeLayout(False)
        Me.SRTab.PerformLayout()
        Me.TradesTab.ResumeLayout(False)
        Me.TradesTab.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents SCEPTabControl As System.Windows.Forms.TabControl
    Friend WithEvents GeneralTab As System.Windows.Forms.TabPage
    Friend WithEvents VersionLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SRTab As System.Windows.Forms.TabPage
    Friend WithEvents ProfilesEnabledCheck As System.Windows.Forms.CheckBox
    Friend WithEvents TradesTab As System.Windows.Forms.TabPage
    Friend WithEvents TradesEnabledCheck As System.Windows.Forms.CheckBox
    Friend WithEvents InvitesEnabledCheck As System.Windows.Forms.CheckBox
    Friend WithEvents SCEPEnabledCheck As System.Windows.Forms.CheckBox
    Friend WithEvents StartupCheck As System.Windows.Forms.CheckBox
    Friend WithEvents MinimizeCheck As System.Windows.Forms.CheckBox
    Friend WithEvents SCEPNotifyIcon As System.Windows.Forms.NotifyIcon

End Class

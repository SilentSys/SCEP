'Copyright © 2014 SteamCEP.com
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program.  If not, see <http://www.gnu.org/licenses/>.
'
'----------
'Developer Info:
'Email:     andy@flankers.net
'Steam:     http://steamcommunity.com/profiles/76561197998958343
'Twitter:   https://twitter.com/SilentFL
'----------
Public Class Form1
    Dim SCEP As New SCEP
    Dim Loaded As Boolean = False
    Const Version As String = "1.1"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SCEPLoadSettings()
        Loaded = True

        VersionLabel.Text = Version

        If Not MinimizeCheck.Checked Then
            Me.WindowState = FormWindowState.Normal
            Me.ShowInTaskbar = True
        End If

        If SCEPEnabledCheck.Checked Then
            SCEP.StartProxy()
        End If

        CheckForUpdates()
    End Sub

    Private Sub SCEPEnabledCheck_CheckedChanged(sender As Object, e As EventArgs) Handles SCEPEnabledCheck.CheckedChanged
        If Loaded Then
            If SCEPEnabledCheck.Checked Then
                SCEP.StopProxy()
                SCEP.StartProxy()
            Else
                SCEP.StopProxy()
            End If
            SCEPSaveSettings()
        End If
    End Sub

    Private Sub ProfilesEnabledCheck_CheckedChanged(sender As Object, e As EventArgs) Handles ProfilesEnabledCheck.CheckedChanged
        SCEP.EnabledOnProfiles = ProfilesEnabledCheck.Checked
        SCEPSaveSettings()

    End Sub

    Private Sub InvitesEnabledCheck_CheckedChanged(sender As Object, e As EventArgs) Handles InvitesEnabledCheck.CheckedChanged
        SCEP.EnabledInInvites = InvitesEnabledCheck.Checked
        SCEPSaveSettings()
    End Sub

    Private Sub TradesEnabledCheck_CheckedChanged(sender As Object, e As EventArgs) Handles TradesEnabledCheck.CheckedChanged
        SCEP.EnableInTrades = TradesEnabledCheck.Checked
        SCEPSaveSettings()
    End Sub

    Private Sub MinimizeCheck_CheckedChanged(sender As Object, e As EventArgs) Handles MinimizeCheck.CheckedChanged
        SCEPNotifyIcon.Visible = MinimizeCheck.Checked
        SCEPSaveSettings()
    End Sub

    Private Sub SCEPNotifyIcon_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles SCEPNotifyIcon.MouseDoubleClick
        Me.ShowInTaskbar = True
        Me.Show()
    End Sub

    Private Sub StartupCheck_CheckedChanged(sender As Object, e As EventArgs) Handles StartupCheck.CheckedChanged
        If Loaded Then
            Dim principal = New System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())
            Dim Admin As Boolean = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator)

            If Admin Then
                If StartupCheck.Checked Then
                    My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).SetValue(Application.ProductName, Application.ExecutablePath)
                    SCEPSaveSettings()
                Else
                    My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).DeleteValue(Application.ProductName)
                    SCEPSaveSettings()
                End If

            Else
                StartupCheck.Checked = My.Settings.SCEPStartup
                MessageBox.Show("To enable this feature you must restart SCEP with Administrative privileges.", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub LockTradesCheck_CheckedChanged(sender As Object, e As EventArgs) Handles LockTradesCheck.CheckedChanged
        SCEP.LockDownTrades = LockTradesCheck.Checked
        SCEPSaveSettings()
    End Sub

    Private Sub TradeTfCheck_CheckedChanged(sender As Object, e As EventArgs) Handles TradeTfCheck.CheckedChanged
        SCEP.UseTradeTF = TradeTfCheck.Checked
        SCEPSaveSettings()
    End Sub

    Private Sub WebProtCheck_CheckedChanged(sender As Object, e As EventArgs) Handles WebProtCheck.CheckedChanged
        SCEP.WebsiteProtection = WebProtCheck.Checked
        If WebProtCheck.Checked Then
            SCEP.LoadOnStart()
        End If
        SCEPSaveSettings()
    End Sub

    Private Sub SteamOnlyCheck_CheckedChanged(sender As Object, e As EventArgs) Handles SteamOnlyCheck.CheckedChanged
        SCEP.SteamAppOnly = SteamOnlyCheck.Checked
        SCEPSaveSettings()
        SCEP.EnableSteamOnly()
    End Sub

    Private Sub SCEPSaveSettings()
        If Loaded Then
            My.Settings.SCEPEnabled = SCEPEnabledCheck.Checked
            My.Settings.SCEPStartup = StartupCheck.Checked
            My.Settings.SCEPProfiles = ProfilesEnabledCheck.Checked
            My.Settings.SCEPInvites = InvitesEnabledCheck.Checked
            My.Settings.SCEPTrades = TradesEnabledCheck.Checked
            My.Settings.SCEPMinimize = MinimizeCheck.Checked
            My.Settings.SCEPLockTrades = LockTradesCheck.Checked
            My.Settings.SCEPTradeTf = TradeTfCheck.Checked
            My.Settings.SCEPWebProtect = WebProtCheck.Checked
            My.Settings.SCEPSteamOnly = SteamOnlyCheck.Checked
            My.Settings.Save()
        End If
    End Sub

    Private Sub SCEPLoadSettings()
        SCEPEnabledCheck.Checked = My.Settings.SCEPEnabled
        StartupCheck.Checked = My.Settings.SCEPStartup
        ProfilesEnabledCheck.Checked = My.Settings.SCEPProfiles
        InvitesEnabledCheck.Checked = My.Settings.SCEPInvites
        TradesEnabledCheck.Checked = My.Settings.SCEPTrades
        MinimizeCheck.Checked = My.Settings.SCEPMinimize
        LockTradesCheck.Checked = My.Settings.SCEPLockTrades
        TradeTfCheck.Checked = My.Settings.SCEPTradeTf
        WebProtCheck.Checked = My.Settings.SCEPWebProtect
        SteamOnlyCheck.Checked = My.Settings.SCEPSteamOnly
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SCEP.StopProxy()
    End Sub

    Private Sub Form1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        If Me.WindowState = FormWindowState.Minimized And MinimizeCheck.Checked Then
            Me.Hide()
        End If
    End Sub

    Private Sub CheckForUpdates()
        Try
            Dim UpdatePage As String = SCEP.RequestPage("http://steamcep.com/updates.php", "", "")
            If Not String.IsNullOrEmpty(UpdatePage) Then
                Dim UpdateInfo() As String = UpdatePage.Split(" ")
                Dim NewVersion As Double = Convert.ToDouble(UpdateInfo(0))
                If NewVersion > Convert.ToDouble(Version) Then
                    MessageBox.Show("This version needs to be updated. Please visit " & UpdateInfo(1) & " to update.", "Update!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Process.Start(UpdateInfo(1))
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ReporBugLink_Click(sender As Object, e As EventArgs) Handles ReportLink.Click
        Process.Start("http://steamcep.com/report/")
    End Sub
End Class

Imports System.Net
Imports System.IO
Imports System.Threading
Imports System.Environment
Imports System.Text
Imports Microsoft.Win32
Public Class opsw
    Public Function String_Random(
    intMinLength As Integer,
    intMaxLength As Integer,
    strPrepend As String,
    strAppend As String,
    intCase As Integer,
    bIncludeDigits As Boolean) As String
        Dim s As String = String.Empty
        Select Case intCase

            Case 1

                ' Uppercase
                s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

            Case 2

                ' Lowercase
                s = "abcdefghijklmnopqrstuvwxyz"

            Case Else

                ' Case Insensitive + Numbers
                s = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"

        End Select

        ' Add numbers to the allowed characters if user chose so
        If bIncludeDigits = True Then s &= "0123456789"

        Static r As New Random

        Dim chactersInString As Integer = r.Next(intMinLength, intMaxLength)
        Dim sb As New StringBuilder

        ' Add the prepend string if one was passed
        If String.IsNullOrEmpty(strPrepend) = False Then sb.Append(strPrepend)

        For i As Integer = 1 To chactersInString

            Dim idx As Integer = r.Next(0, s.Length)

            sb.Append(s.Substring(idx, 1))

        Next

        ' Add the append string if one was passed
        If String.IsNullOrEmpty(strAppend) = False Then sb.Append(strAppend)

        Return sb.ToString()

    End Function
    Dim files As System.IO.StreamWriter
    Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
    Dim webClient As New System.Net.WebClient

    Dim p() As Process
    Dim CounterThread As New Thread(AddressOf CounterThreadMethod)
    Public Class GlobalVariables
        Public Shared val1 As String
        Public Shared val2 As String
    End Class
    'Declaring "Fake" globals due to Timer1 lag when making a webrequest. Hence why stats are within a background thread.'
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label4.Text = GlobalVariables.val1
        Label5.Text = GlobalVariables.val2
    End Sub
    Private Sub CounterThreadMethod()
        While True


            Try

                webClient.CachePolicy = New System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache)
                webClient.CachePolicy = New System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
                webClient.Headers.Add("Cache-Control", "no-cache")
                GlobalVariables.val2 = webClient.DownloadString("http://ip.g4z.co/raw.php?t=" + String_Random(30, 30, "Hey_Now_", String.Empty, 2, False))
            Catch ex As Exception

            End Try


            Thread.Sleep(2000)
            p = Process.GetProcessesByName("openvpn")
            If p.Count > 0 Then
                GlobalVariables.val1 = "True"
            Else
                GlobalVariables.val1 = "False"
            End If

        End While
    End Sub
    Private Sub opsw_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not System.IO.Directory.Exists(appData + "/OpenVPN Switcher") Then
            System.IO.Directory.CreateDirectory(appData + "/OpenVPN Switcher")
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF1.config", True)
            files.WriteLine("C:\Program Files\OpenVPN\bin\openvpn.exe")
            files.Close()
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF2.config", True)
            files.WriteLine("C:\Program Files\OpenVPN\bin\openvpn.exe")
            files.Close()
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF3.config", True)
            files.WriteLine("127.0.0.1")
            files.Close()
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF4.config", True)
            files.WriteLine("F")
            files.Close()
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF5.config", True)
            files.WriteLine("F")
            files.Close()
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF6.config", True)
            files.WriteLine("F")
            files.Close()
        End If
        Try
            TextBox1.Text = File.ReadAllText(appData + "/OpenVPN Switcher/CF1.config")


        Catch ex As Exception
            TextBox1.Text = "C:\Program Files\OpenVPN\bin\openvpn.exe"
        End Try
        Try
            TextBox2.Text = File.ReadAllText(appData + "/OpenVPN Switcher/CF2.config")
        Catch ex As Exception
            TextBox2.Text = "C:\Program Files\OpenVPN\bin\openvpn.exe"
        End Try
        Try
            TextBox3.Text = File.ReadAllText(appData + "/OpenVPN Switcher/CF3.config")
        Catch ex As Exception
            TextBox3.Text = "127.0.0.1"
        End Try
        Try

            Dim clean_str As String = File.ReadAllText(appData + "/OpenVPN Switcher/CF4.config").Replace(vbCr, "").Replace(vbLf, "")
            If clean_str = "T" Then
                Dim result As String = webClient.DownloadString("http://ip.g4z.co/raw.php")
                CheckBox1.Checked = True
                TextBox3.Text = result
            End If
        Catch ex As Exception

        End Try
        Try

            Dim clean_str As String = File.ReadAllText(appData + "/OpenVPN Switcher/CF6.config").Replace(vbCr, "").Replace(vbLf, "")
            If clean_str = "T" Then
                RadioButton1.Checked = True

            Else
                RadioButton2.Checked = True
            End If

        Catch ex As Exception

        End Try


        Try

            Dim clean_str As String = File.ReadAllText(appData + "/OpenVPN Switcher/CF5.config").Replace(vbCr, "").Replace(vbLf, "")
            If clean_str = "T" Then
                CheckBox2.Checked = True
                Timer2.Start()
                Button3.Text = "Stop"
            End If
        Catch ex As Exception

        End Try

        Label4.Text = "Unknown"
        Label5.Text = "Unknown"
        CounterThread.IsBackground = True
        CounterThread.Start()
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
        About.Show()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If CheckBox1.Checked = True Then
            System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/CF4.config", "")
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF4.config", True)
            files.WriteLine("T")
            files.Close()
        Else
            System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/CF4.config", "")
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF4.config", True)
            files.WriteLine("F")
            files.Close()
        End If
        If RadioButton1.Checked = True Then
            System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/CF6.config", "")
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF6.config", True)
            files.WriteLine("T")
            files.Close()
        Else
            System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/CF6.config", "")
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF6.config", True)
            files.WriteLine("F")
            files.Close()
        End If
        If CheckBox2.Checked = True Then
            System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/CF5.config", "")
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF5.config", True)
            files.WriteLine("T")
            files.Close()
        Else
            System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/CF5.config", "")
            files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF5.config", True)
            files.WriteLine("F")
            files.Close()
        End If

        System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/CF1.config", "")
        files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF1.config", True)
        files.WriteLine(TextBox1.Text)
        files.Close()
        System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/CF2.config", "")
        files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF2.config", True)
        files.WriteLine(TextBox2.Text)
        files.Close()
        System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/CF3.config", "")
        files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/CF3.config", True)
        files.WriteLine(TextBox3.Text)
        files.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Timer3.Interval = TextBox4.Text
        If Button3.Text = "Stop" Then
            Button3.Text = "Start"
            For Each prog As Process In Process.GetProcesses
                If prog.ProcessName = "openvpn" Then
                    prog.Kill()
                End If
            Next
            Timer2.Stop()
            Timer3.Stop()
        Else
            Timer2.Start()
            Timer3.Start()
            Button3.Text = "Stop"
        End If

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

        Dim myFiles As New List(Of String)


        Dim clean_str As String = File.ReadAllText(appData + "/OpenVPN Switcher/CF2.config").Replace(vbCr, "").Replace(vbLf, "")
        For Each file As String In My.Computer.FileSystem.GetFiles(clean_str)
            myFiles.Add(file)
        Next
        Dim rnd As New Random
        Dim clean_str_VPN As String = ControlChars.Quote & File.ReadAllText(appData + "/OpenVPN Switcher/CF1.config").Replace(vbCr, "").Replace(vbLf, "") & ControlChars.Quote
        Dim clean_vpn_string As String = ControlChars.Quote & myFiles(rnd.Next(0, myFiles.Count)) & ControlChars.Quote
        Dim total_cmd As String = clean_str_VPN + " " + clean_vpn_string
        System.IO.File.WriteAllText(appData + "/OpenVPN Switcher/launch.bat", "")
        files = My.Computer.FileSystem.OpenTextFileWriter(appData + "/OpenVPN Switcher/launch.bat", True, System.Text.Encoding.Default)
        files.WriteLine(total_cmd)
        files.Close()
        Timer2.Stop()
        Shell(appData + "/OpenVPN Switcher/launch.bat", AppWinStyle.NormalNoFocus)
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Try

            webClient.CachePolicy = New System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
            If webClient.DownloadString("http://ip.g4z.co/raw.php?t=" + String_Random(30, 30, "Hey_Now_", String.Empty, 2, False)) = TextBox3.Text Then
                For Each prog As Process In Process.GetProcesses
                    If prog.ProcessName = "openvpn" Then
                        prog.Kill()
                    End If
                Next
                Timer2.Start()
            Else


            End If

        Catch ex As Exception

        End Try
        If RadioButton1.Checked = True Then
            For Each prog As Process In Process.GetProcesses
                If prog.ProcessName = "openvpn" Then
                    prog.Kill()
                End If
            Next
            Timer2.Start()
        End If
    End Sub
End Class

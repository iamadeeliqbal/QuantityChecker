Imports System.IO
Imports System.Net
Imports System.Xml
Public Class Form1

    Dim acces_code As String
    Dim part_number As String
    Dim token As String
    Dim webaddress As String
    Dim xmldoc As New XmlDocument()
    Dim xmldoc_invalid As New XmlDocument()
    Dim xmldoc_part_num As New XmlDocument()
    Dim xmldoc_data As New XmlDocument()
    Dim xmldoc_data_pn As New XmlDocument()
    Dim xmlnode_tkn As XmlNodeList
    Dim xmlnode_invalid_tkn As XmlNodeList
    Dim xmlnode_qty As XmlNodeList
    Dim tkn As String
    Dim tkn_invalid As String
    Dim req_qty As String
    Dim triger As Integer
    Dim invalid_pn_list As XmlNodeList
    Dim invalid_pn As String

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles label1.Click

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBox_access_code_TextChanged(sender As Object, e As EventArgs) Handles TextBox_ac.TextChanged
        acces_code = TextBox_ac.Text
    End Sub

    Private Sub TextBox_pn_TextChanged(sender As Object, e As EventArgs) Handles TextBox_pn.TextChanged
        part_number = TextBox_pn.Text
    End Sub

    Private Sub Button_qty_Click(sender As Object, e As EventArgs) Handles Button_qty.Click
        If acces_code = "" And part_number = "" Then
            label1.ForeColor = Color.Red
            label2.ForeColor = Color.Red
            MessageBox.Show("Please Update Highlighted Values!", "Alert")
            'triger = 0
        End If
        If acces_code = "" And part_number <> "" Then
            label1.ForeColor = Color.Red
            MessageBox.Show("Enter Access Code!", "Alert")
        ElseIf acces_code <> "" Then
            label1.ForeColor = Color.Black
        End If
        If part_number = "" And acces_code <> "" Then
            label2.ForeColor = Color.Red
            MessageBox.Show("Enter Part Number!", "Alert")
        ElseIf part_number <> "" Then
            label2.ForeColor = Color.Black
        End If
        If acces_code <> "" And part_number <> "" Then
            triger = 1
        End If
        If triger = 1 Then
            label1.ForeColor = Color.Black
            label2.ForeColor = Color.Black
            xmldoc_invalid.Load("http://juki-pc:8081/?f=login&accesscode=" & acces_code)
            xmlnode_invalid_tkn = xmldoc_invalid.GetElementsByTagName("resdetail")
            For y = 0 To xmlnode_invalid_tkn.Count - 1
                tkn_invalid = xmlnode_invalid_tkn(y).InnerText
            Next
            If tkn_invalid = "invalid access code" Then
                label1.ForeColor = Color.Red
                MessageBox.Show("Enter Valid Access Code!!!", "Alert")
            End If
            xmldoc.Load("http://juki-pc:8081/?f=login&accesscode=" & acces_code)
            xmlnode_tkn = xmldoc.GetElementsByTagName("token")
            For x = 0 To xmlnode_tkn.Count - 1
                tkn = xmlnode_tkn(x).InnerText
            Next
            xmldoc_data_pn.Load("http://juki-pc:8081/?f=item_get&item_code=" & part_number & "&tkn=" & tkn)
            'webaddress = "http://juki-pc:8081/?f=item_get&item_code=" & part_number & "&tkn=" & tkn
            'Process.Start(webaddress)
            invalid_pn_list = xmldoc_data_pn.GetElementsByTagName("resdetail")
            For z = 0 To invalid_pn_list.Count - 1
                invalid_pn = invalid_pn_list(z).InnerText
                If invalid_pn = "item not found" Then
                    label2.ForeColor = Color.Red
                    MessageBox.Show("Part number is incorrect or not exists!!", "Error")
                End If
            Next

            'webaddress = "http://juki-pc:8081/?f=item_get&item_code=" & part_number & "&tkn=" & tkn
            'Process.Start(webaddress)
            Dim requ As WebRequest = WebRequest.Create("http://juki-pc:8081/?f=item_get&item_code=" & part_number & "&tkn=" & tkn)
            requ.Timeout = 10 * 60 * 1000   ' 10 minutes timeout and not 100s as the default.
            Dim response As WebResponse = requ.GetResponse()
            Console.WriteLine("Will download {0:N0}bytes", response.ContentLength)
            Dim stream As Stream = response.GetResponseStream()
            xmldoc_data.Load(stream)
            xmlnode_qty = xmldoc_data.GetElementsByTagName("stockquantity")
            For i = 0 To xmlnode_qty.Count - 1
                req_qty = xmlnode_qty(i).InnerText
                MessageBox.Show(req_qty & " items exist in the database.", part_number)
            Next
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub
End Class

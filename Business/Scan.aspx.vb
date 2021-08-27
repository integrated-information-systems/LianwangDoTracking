Imports System.Data.SqlClient
Imports System.Web.Configuration

Partial Class Business_Scan
    Inherits System.Web.UI.Page

    Protected Sub txtScancode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScancode.TextChanged
        If txtScancode.Text <> String.Empty And IsNumeric(txtScancode.Text) Then

            Dim DocNo As String = txtScancode.Text

            Dim SAPSqlConnection As New SqlConnection
            SAPSqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
            SAPSqlConnection.Open()

            Dim FindCommand As New SqlCommand
            FindCommand.CommandText = "SELECT DraftDo.DocNum,DraftDo.Delivered FROM ODRF LEFT JOIN " & WebConfigurationManager.AppSettings("CRM_DB") & ".dbo.DraftDo on DraftDo.DocNum=ODRF.DocNum WHERE ODRF.DocNum=@Docnum"
            FindCommand.Connection = SAPSqlConnection
            FindCommand.Parameters.AddWithValue("DocNum", DocNo)
            Dim DReader As SqlDataReader = FindCommand.ExecuteReader
            Dim DocNoAvailinSAP As Boolean = False
            Dim DocNoAvail As Boolean = False
            If DReader.Read Then
                DocNoAvailinSAP = True
                If Not IsDBNull(DReader("DocNum")) Then
                    DocNoAvail = True

                    If CBool(DReader("Delivered")) = True Then
                        AlertMsg("Duplicate found: Scanned this item already")
                    End If
                End If
        
            End If

            DReader.Close()
            FindCommand.Dispose()
            SAPSqlConnection.Close()

            Dim CustomSqlConnection As New SqlConnection
            CustomSqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_CRM_DB_ConnectionString").ConnectionString
            CustomSqlConnection.Open()
            If DocNoAvailinSAP = True Then
                If DocNoAvail = True Then
                    Dim UpdateCommand As New SqlCommand
                    UpdateCommand.CommandText = "UPDATE DraftDo SET Delivered=1, LastUpdatedOn=@LastUpdatedOn, UpdatedBy=@UpdatedBy WHERE DocNum=@DocNum "
                    UpdateCommand.Connection = CustomSqlConnection
                    UpdateCommand.Parameters.AddWithValue("DocNum", DocNo)
                    UpdateCommand.Parameters.AddWithValue("UpdatedBy", User.Identity.Name)
                    UpdateCommand.Parameters.AddWithValue("LastUpdatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))
                    UpdateCommand.ExecuteNonQuery()
                    UpdateCommand.Dispose()
                Else
                    Dim InsertCommand As New SqlCommand
                    InsertCommand.CommandText = "INSERT INTO DraftDo (DocNum,Delivered,UpdatedBy,LastUpdatedOn) Values(@DocNum, 1, @UpdatedBy, @LastUpdatedOn)"
                    InsertCommand.Connection = CustomSqlConnection
                    InsertCommand.Parameters.AddWithValue("DocNum", DocNo)
                    InsertCommand.Parameters.AddWithValue("UpdatedBy", User.Identity.Name)
                    InsertCommand.Parameters.AddWithValue("LastUpdatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))
                    InsertCommand.ExecuteNonQuery()
                    InsertCommand.Dispose()
                End If
                CustomSqlConnection.Close()
                AlertMsg("Scanned Successfully")
            Else
                txtScancode.Text = String.Empty
                AlertMsg("Doc No not valid ")
            End If
        Else
            txtScancode.Text = String.Empty
            AlertMsg("Please enter valid document no")
        End If
            txtScancode.Focus()
        txtScancode.Text = String.Empty
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtScancode.Focus()
    End Sub
    Public Sub AlertMsg(ByVal msg As String)
        Dim doubleQuotes As Char = """"
        msg = msg.Replace("'", doubleQuotes)
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Hy-Ray", "<script>alert('" & msg & "');</script>")
    End Sub
End Class

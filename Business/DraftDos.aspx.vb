Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Globalization
Imports System.Data
Imports System.Web.UI.Control
Partial Class Business_Fullwaitingdo
    Inherits System.Web.UI.Page
    
    Protected Sub btnFullLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFullLoad.Click, btnLooseLoad.Click, btnFullDelivered.Click, btnLooseDelivered.Click
        Dim FullorLoose As String = String.Empty
        Dim ActiveButton As Button = CType(sender, Button)
        Select Case ActiveButton.ID
            Case "btnFullLoad"
                btnFullLoad.CssClass = "tab_selected"
                btnLooseLoad.CssClass = "tab"
                btnFullDelivered.CssClass = "tab"
                btnLooseDelivered.CssClass = "tab"
                MultiView1.ActiveViewIndex = 0
                FullorLoose = "F"
            Case "btnLooseLoad"
                btnFullLoad.CssClass = "tab"
                btnLooseLoad.CssClass = "tab_selected"
                btnFullDelivered.CssClass = "tab"
                btnLooseDelivered.CssClass = "tab"
                MultiView1.ActiveViewIndex = 0
                FullorLoose = "L"
            Case "btnFullDelivered"
                btnFullLoad.CssClass = "tab"
                btnLooseLoad.CssClass = "tab"
                btnFullDelivered.CssClass = "tab_selected"
                btnLooseDelivered.CssClass = "tab"
                MultiView1.ActiveViewIndex = 1
                FullorLoose = "F"
            Case "btnLooseDelivered"
                btnFullLoad.CssClass = "tab"
                btnLooseLoad.CssClass = "tab"
                btnFullDelivered.CssClass = "tab"
                btnLooseDelivered.CssClass = "tab_selected"
                MultiView1.ActiveViewIndex = 1
                FullorLoose = "L"
        End Select
        UpdateGridCommand(FullorLoose)
    End Sub
    Private Sub UpdateGridCommand(ByVal FullOrLoose As String)
        Dim enUS As New CultureInfo("en-US")
        'FullLoadSqlDatasource.SelectCommand = "SELECT Putback,odrf.DocNum,CardName,DocDueDate,OPRJ.PrjName,DraftDo.Lorry,draftdo.Time,draftdo.SAPTime,cast(SAPTime as Time) as 'SAPTimeSort', cast(Time as Time) as 'TimeSort',draftdo.Reason FROM ODRF INNER JOIN " & WebConfigurationManager.AppSettings("CRM_DB") & ".dbo.DraftDo on draftdo.DocNum=odrf.DocNum LEFT JOIN DRF1 ON (DRF1.DocEntry=ODRF.DocEntry AND DRF1.LineNum=0) LEFT JOIN OPRJ ON OPRJ.PrjCode=DRF1.Project WHERE 1=1   "
        FullLoadSqlDatasource.SelectCommand = "SELECT Putback,odrf.DocNum,CardName,DocDueDate,OPRJ.PrjName,DraftDo.Lorry,draftdo.Time,draftdo.SAPTime,SAPTime as 'SAPTimeSort',Time as 'TimeSort',draftdo.Reason,draftdo.LorryGroup FROM ODRF INNER JOIN " & WebConfigurationManager.AppSettings("CRM_DB") & ".dbo.DraftDo on draftdo.DocNum=odrf.DocNum LEFT JOIN DRF1 ON (DRF1.DocEntry=ODRF.DocEntry AND DRF1.LineNum=0) LEFT JOIN OPRJ ON OPRJ.PrjCode=DRF1.Project WHERE 1=1   "
        FullLoadSqlDatasource.SelectParameters.Clear()
        If txtFromDate.Text <> String.Empty Or txtToDate.Text <> String.Empty Then
            Dim Conditions As New List(Of String)
            Dim FromDate As Date
            Dim ToDate As Date
            If Date.TryParseExact(txtFromDate.Text, "d/M/yyyy", enUS, DateTimeStyles.None, FromDate) Then
                Conditions.Add(" DocDueDate>=@FromDate ")
                FullLoadSqlDatasource.SelectParameters.Add("FromDate", TypeCode.DateTime, FromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
            End If
            If Date.TryParseExact(txtToDate.Text, "d/M/yyyy", enUS, DateTimeStyles.None, ToDate) Then
                Conditions.Add(" DocDueDate<=@ToDate ")
                FullLoadSqlDatasource.SelectParameters.Add("ToDate", TypeCode.DateTime, ToDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
            End If

            If Conditions.Count > 0 Then
                FullLoadSqlDatasource.SelectCommand = FullLoadSqlDatasource.SelectCommand & " AND "
                FullLoadSqlDatasource.SelectCommand = FullLoadSqlDatasource.SelectCommand & String.Join(" AND ", Conditions.ToArray)
            End If
        End If

        FullLoadSqlDatasource.SelectCommand = FullLoadSqlDatasource.SelectCommand & " AND "
        FullLoadSqlDatasource.SelectCommand = FullLoadSqlDatasource.SelectCommand & "U_FullLoose=@FullorLoose "

        FullLoadSqlDatasource.SelectParameters.Add("FullorLoose", FullOrLoose)

        FullLoadSqlDatasource.SelectCommand = FullLoadSqlDatasource.SelectCommand & " AND "
        FullLoadSqlDatasource.SelectCommand = FullLoadSqlDatasource.SelectCommand & WebConfigurationManager.AppSettings("CRM_DB") & ".dbo.DraftDo.Delivered=@Delivered "

        If MultiView1.ActiveViewIndex = 0 Then
            FullLoadSqlDatasource.SelectParameters.Add("Delivered", 0)
        ElseIf MultiView1.ActiveViewIndex = 1 Then
            FullLoadSqlDatasource.SelectParameters.Add("Delivered", 1)
        End If

        'Response.Write(FullLoadSqlDatasource.SelectCommand)

        DGFullLooseLoad.DataBind()
        DGDeliveredFullLooseLoad.DataBind()
    End Sub
    Private Function getPostBackControlName() As String
        Dim control As Control = Nothing
        'first we will check the "__EVENTTARGET" because if post back made by       the controls
        'which used "_doPostBack" function also available in Request.Form collection.

        Dim ctrlname As String = Page.Request.Params("__EVENTTARGET")
        If ctrlname IsNot Nothing AndAlso ctrlname <> [String].Empty Then
            control = Page.FindControl(ctrlname)
        Else

            ' if __EVENTTARGET is null, the control is a button type and we need to
            ' iterate over the form collection to find it
            Dim ctrlStr As String = [String].Empty
            Dim c As Control = Nothing
            For Each ctl As String In Page.Request.Form
                'handle ImageButton they having an additional "quasi-property" in their Id which identifies
                'mouse x and y coordinates
                If ctl.EndsWith(".x") OrElse ctl.EndsWith(".y") Then
                    ctrlStr = ctl.Substring(0, ctl.Length - 2)
                    c = Page.FindControl(ctrlStr)
                Else
                    c = Page.FindControl(ctl)
                End If
                If TypeOf c Is System.Web.UI.WebControls.Button OrElse TypeOf c Is System.Web.UI.WebControls.ImageButton Then
                    control = c
                    Exit For
                End If

            Next
        End If
        If IsNothing(control) Then
            Return String.Empty
        Else
            Return control.ID
        End If
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim POSTBackArgumentName As String = String.Empty

        If IsPostBack() Then
            POSTBackArgumentName = getPostBackControlName()
        End If
        If POSTBackArgumentName = String.Empty Then
            POSTBackArgumentName = "-"
        End If


        If IsPostBack() = True Then
            If ((POSTBackArgumentName.Equals("txtLorryGroup") Or POSTBackArgumentName.Equals("txtLorry") Or POSTBackArgumentName.Equals("txtTime") Or POSTBackArgumentName.Equals("txtReason"))) And (Not POSTBackArgumentName.Equals("chkPutBack") Or Not POSTBackArgumentName.Equals("btnPutBack")) Then

                UpdateGridValuesFullLoad()

            End If
        Else
            LoadDataFirstTime()
        End If

        If MultiView1.ActiveViewIndex <= -1 Then
            MultiView1.ActiveViewIndex = 0
            btnFullLoad.CssClass = "tab_selected"
        End If

        If Not POSTBackArgumentName.Equals("chkPutBack") And Not POSTBackArgumentName.Equals("btnPutBack") Then

            If btnFullLoad.CssClass = "tab_selected" Or btnFullDelivered.CssClass = "tab_selected" Then
                UpdateGridCommand("F")
            ElseIf btnLooseLoad.CssClass = "tab_selected" Or btnLooseDelivered.CssClass = "tab_selecte" Then
                UpdateGridCommand("L")
            End If

        End If


    End Sub
    Private Sub LoadDataFirstTime()
        Dim SAPSqlConnection As New SqlConnection
        SAPSqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
        SAPSqlConnection.Open()

        Dim SelectCommand As New SqlCommand
        SelectCommand.CommandText = "SELECT ODRF.DocNum,U_DTime FROM ODRF LEFT JOIN " & WebConfigurationManager.AppSettings("CRM_DB") & ".dbo.DraftDo on DraftDo.DocNum=ODRF.DocNum WHERE ODRF.U_FullLoose IS NOT NULL AND DATEDIFF(d,ODRF.docdate,getdate())<=7"

        SelectCommand.Connection = SAPSqlConnection
        Dim DReader As SqlDataReader = SelectCommand.ExecuteReader

     
        While DReader.Read
            Dim DocNo As String = String.Empty
            Dim SAPTime As String = String.Empty
            Dim MyTime As String = String.Empty

            DocNo = DReader("DocNum")
            If Not IsDBNull(DReader("U_DTime")) Then

                SAPTime = DReader("U_DTime").ToString
                MyTime = SAPTime

            End If


            Dim CustomSqlConnection As New SqlConnection
            CustomSqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_CRM_DB_ConnectionString").ConnectionString
            CustomSqlConnection.Open()

            Dim FindCommand As New SqlCommand
            FindCommand.CommandText = "SELECT * FROM  DraftDo  WHERE DraftDo.DocNum=@Docnum"

            FindCommand.Connection = CustomSqlConnection
            FindCommand.Parameters.AddWithValue("DocNum", DocNo)
            Dim FindReader As SqlDataReader = FindCommand.ExecuteReader
            Dim DocNoAvail As Boolean = False

            If FindReader.Read Then
                DocNoAvail = True
            End If

            FindReader.Close()
            FindCommand.Dispose()
            CustomSqlConnection.Close()

            If DocNoAvail = False Then
                Dim CustomSqlInsertConnection As New SqlConnection
                CustomSqlInsertConnection.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_CRM_DB_ConnectionString").ConnectionString
                CustomSqlInsertConnection.Open()


                Dim InsertCommand As New SqlCommand
                InsertCommand.CommandText = "INSERT INTO DraftDo (DocNum, SAPTime, Time, UpdatedBy,LastUpdatedOn) Values(@DocNum, @SAPTime, @Time, @UpdatedBy, @LastUpdatedOn)"
                InsertCommand.Connection = CustomSqlInsertConnection
                InsertCommand.Parameters.AddWithValue("DocNum", DocNo)
                InsertCommand.Parameters.AddWithValue("SAPTime", SAPTime)
                InsertCommand.Parameters.AddWithValue("Time", MyTime)
                InsertCommand.Parameters.AddWithValue("UpdatedBy", User.Identity.Name)
                InsertCommand.Parameters.AddWithValue("LastUpdatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))
                InsertCommand.ExecuteReader()
                InsertCommand.Dispose()
                CustomSqlInsertConnection.Close()
            End If

        End While

        DReader.Close()
        SelectCommand.Dispose()
        SAPSqlConnection.Close()
    End Sub
    Private Sub UpdateGridValuesFullLoad(Optional ByVal Firsttime As Boolean = False)

        Dim DG As GridView = New GridView

        If MultiView1.ActiveViewIndex = 0 Then
            DG = DGFullLooseLoad
        ElseIf MultiView1.ActiveViewIndex = 1 Then
            DG = DGDeliveredFullLooseLoad
        End If

        For Each DRow As GridViewRow In DG.Rows

            Dim DocNo As String = DRow.Cells(0).Text

            Dim Reason As String = String.Empty
            Dim LorryGroup As String = String.Empty
            Dim PutBack As Boolean = True
            If MultiView1.ActiveViewIndex = 1 Then
                Dim ReasonBox As TextBox = DRow.Cells(6).FindControl("txtReason")
                Dim PutBackbox As CheckBox = DRow.Cells(7).FindControl("chkPutBack")
                
                Reason = ReasonBox.Text

                If PutBackbox.Checked = True Then
                    PutBack = False
                End If
            Else
                Dim LorryGroupTxt As TextBox = DRow.Cells(8).FindControl("txtLorryGroup")
                LorryGroup = LorryGroupTxt.Text

                PutBack = False
            End If

            'Dim SAPSqlConnection As New SqlConnection
            'SAPSqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings("SAP_DB_ConnectionString").ConnectionString
            'SAPSqlConnection.Open()

            'Dim FindCommand As New SqlCommand
            'FindCommand.CommandText = "SELECT * FROM ODRF INNER JOIN " & WebConfigurationManager.AppSettings("CRM_DB") & ".dbo.DraftDo on DraftDo.DocNum=ODRF.DocNum WHERE ODRF.DocNum=@Docnum"

            'FindCommand.Connection = SAPSqlConnection
            'FindCommand.Parameters.AddWithValue("DocNum", DocNo)
            'Dim DReader As SqlDataReader = FindCommand.ExecuteReader
            'Dim DocNoAvail As Boolean = False

            'If DReader.Read Then
            '    DocNoAvail = True
            'End If

            'DReader.Close()
            'FindCommand.Dispose()
            'SAPSqlConnection.Close()

            Dim CustomSqlConnection As New SqlConnection
            CustomSqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_CRM_DB_ConnectionString").ConnectionString
            CustomSqlConnection.Open()

            'If DocNoAvail = True Then

            Dim UpdateCommand As New SqlCommand
            If MultiView1.ActiveViewIndex = 0 Then
                UpdateCommand.CommandText = "UPDATE DraftDo SET LorryGroup=@LorryGroup, Delivered=@Delivered, Lorry=@Lorry, Time=@Time, LastUpdatedOn=@LastUpdatedOn, UpdatedBy=@UpdatedBy WHERE DocNum=@DocNum "
                UpdateCommand.Connection = CustomSqlConnection
                UpdateCommand.Parameters.AddWithValue("DocNum", DocNo)
                Dim Lorry As TextBox = DRow.Cells(4).FindControl("txtLorry")
                UpdateCommand.Parameters.AddWithValue("Lorry", Lorry.Text)
                Dim Time As TextBox = DRow.Cells(5).FindControl("txtTime")
                UpdateCommand.Parameters.AddWithValue("Time", Time.Text)
                UpdateCommand.Parameters.AddWithValue("LorryGroup", LorryGroup)
                'UpdateCommand.Parameters.AddWithValue("Reason", Reason)
                UpdateCommand.Parameters.AddWithValue("UpdatedBy", User.Identity.Name)
                UpdateCommand.Parameters.AddWithValue("Delivered", CBool(PutBack))
                UpdateCommand.Parameters.AddWithValue("LastUpdatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))
                UpdateCommand.ExecuteNonQuery()
                UpdateCommand.Dispose()
            Else
                UpdateCommand.CommandText = "UPDATE DraftDo SET  Delivered=@Delivered,   Reason=@Reason,ReasonAppended=CAST(ReasonAppended as Varchar(5000)) +@Reason, LastUpdatedOn=@LastUpdatedOn, UpdatedBy=@UpdatedBy WHERE DocNum=@DocNum "
                UpdateCommand.Connection = CustomSqlConnection
                UpdateCommand.Parameters.AddWithValue("DocNum", DocNo)
                UpdateCommand.Parameters.AddWithValue("Reason", Reason)
                UpdateCommand.Parameters.AddWithValue("UpdatedBy", User.Identity.Name)
                UpdateCommand.Parameters.AddWithValue("Delivered", CBool(PutBack))
                UpdateCommand.Parameters.AddWithValue("LastUpdatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))
                UpdateCommand.ExecuteNonQuery()
                UpdateCommand.Dispose()
            End If


            'Else

            '    Dim InsertCommand As New SqlCommand
            '    InsertCommand.CommandText = "INSERT INTO DraftDo (DocNum,Lorry, Time, Reason,UpdatedBy,LastUpdatedOn) Values(@DocNum, @Lorry, @Time,@Reason, @UpdatedBy, @LastUpdatedOn)"
            '    InsertCommand.Connection = CustomSqlConnection
            '    InsertCommand.Parameters.AddWithValue("DocNum", DocNo)
            '    Dim Lorry As TextBox = DRow.Cells(4).FindControl("txtLorry")
            '    InsertCommand.Parameters.AddWithValue("Lorry", Lorry.Text)
            '    Dim Time As TextBox = DRow.Cells(5).FindControl("txtTime")
            '    InsertCommand.Parameters.AddWithValue("Time", Time.Text)
            '    InsertCommand.Parameters.AddWithValue("Reason", Reason)
            '    InsertCommand.Parameters.AddWithValue("UpdatedBy", User.Identity.Name)
            '    InsertCommand.Parameters.AddWithValue("LastUpdatedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))
            '    InsertCommand.ExecuteNonQuery()
            '    InsertCommand.Dispose()

            'End If
            CustomSqlConnection.Close()
        Next


    End Sub
    Protected Sub btnScan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnScan.Click
        Response.Redirect("~/Business/Scan.aspx", False)
    End Sub

    Protected Sub DGFullLooseLoad_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DGFullLooseLoad.RowCommand


        If e.CommandName.Equals("Lorry") Then
            If DGFullLooseLoad.SortDirection = SortDirection.Descending Then
                DGFullLooseLoad.Sort("Lorry", SortDirection.Ascending)
            Else
                DGFullLooseLoad.Sort("Lorry", SortDirection.Descending)
            End If

        ElseIf e.CommandName.Equals("Time") Then
            If DGFullLooseLoad.SortDirection = SortDirection.Descending Then
                DGFullLooseLoad.Sort("TimeSort", SortDirection.Ascending)
            Else
                DGFullLooseLoad.Sort("TimeSort", SortDirection.Descending)
            End If
        ElseIf e.CommandName.Equals("DOTime") Then
            If DGFullLooseLoad.SortDirection = SortDirection.Descending Then
                DGFullLooseLoad.Sort("SAPTimeSort", SortDirection.Ascending)
            Else
                DGFullLooseLoad.Sort("SAPTimeSort", SortDirection.Descending)
            End If
        End If
        DGFullLooseLoad.DataBind()
        DGDeliveredFullLooseLoad.DataBind()
    End Sub

    Protected Sub DGFullLooseLoad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DGFullLooseLoad.SelectedIndexChanged

    End Sub

    Protected Sub btnPutBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPutBack.Click
        'Response.Write("Btnputback")
        UpdateGridValuesFullLoad()
        If btnFullLoad.CssClass = "tab_selected" Or btnFullDelivered.CssClass = "tab_selected" Then
            UpdateGridCommand("F")
        ElseIf btnLooseLoad.CssClass = "tab_selected" Or btnLooseDelivered.CssClass = "tab_selected" Then
            UpdateGridCommand("L")
        End If
        'DGFullLooseLoad.DataBind()
        'DGDeliveredFullLooseLoad.DataBind()

    End Sub

    Protected Sub DGDeliveredFullLooseLoad_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DGDeliveredFullLooseLoad.RowCommand

    End Sub
End Class

Imports System.Globalization
Imports System.Web.Configuration

Partial Class Business_Report
    Inherits System.Web.UI.Page
    Public SerialNo As Integer = 0
    Public LorryGroup As String = "*"
    Public LorryName As String = "*"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim enUS As New CultureInfo("en-US")
        DGSqlSource.SelectCommand = "SELECT  ODRF.DocDate, Lorry, LorryGroup, OPRJ.PrjName, Time, cast(Time as Time) as 'TimeSort', DraftDo.DocNum, ODRF.U_fullloose FROM ODRF INNER JOIN " & WebConfigurationManager.AppSettings("CRM_DB") & ".dbo.DraftDo on draftdo.DocNum=odrf.DocNum LEFT JOIN DRF1 ON (DRF1.DocEntry=ODRF.DocEntry AND DRF1.LineNum=0) LEFT JOIN OPRJ ON OPRJ.PrjCode=DRF1.Project  WHERE Delivered=1  "

        DGSqlSource.SelectParameters.Clear()
        If txtFromDate.Text <> String.Empty Or txtToDate.Text <> String.Empty Then
            Dim Conditions As New List(Of String)
            Dim FromDate As Date
            Dim ToDate As Date
            If Date.TryParseExact(txtFromDate.Text, "d/M/yyyy", enUS, DateTimeStyles.None, FromDate) Then
                Conditions.Add(" DocDueDate>=@FromDate ")
                DGSqlSource.SelectParameters.Add("FromDate", TypeCode.DateTime, FromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
            End If
            If Date.TryParseExact(txtToDate.Text, "d/M/yyyy", enUS, DateTimeStyles.None, ToDate) Then
                Conditions.Add(" DocDueDate<=@ToDate ")
                DGSqlSource.SelectParameters.Add("ToDate", TypeCode.DateTime, ToDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
            End If

            If Conditions.Count > 0 Then
                DGSqlSource.SelectCommand = DGSqlSource.SelectCommand & " AND "
                DGSqlSource.SelectCommand = DGSqlSource.SelectCommand & String.Join(" AND ", Conditions.ToArray)
            End If

        End If
        DGSqlSource.SelectCommand = DGSqlSource.SelectCommand & " ORDER BY DraftDo.Lorry, DraftDo.LorryGroup "

        DGReport.DataBind()
    End Sub
    Protected Sub DGReport_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DGReport.RowCommand
        If e.CommandName.Equals("TimeSort") Then
            If DGReport.SortDirection = SortDirection.Descending Then
                DGReport.Sort("TimeSort", SortDirection.Ascending)
            Else
                DGReport.Sort("TimeSort", SortDirection.Descending)
            End If
        End If
        DGReport.DataBind()
    End Sub

    Protected Sub DGReport_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DGReport.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim SnoLbl As Label = e.Row.Cells(0).FindControl("SNo")
            Dim TempLorryName As String = e.Row.Cells(1).Text
            Dim TempLorryGrp As String = e.Row.Cells(2).Text
            If LorryName <> TempLorryName Then
                SerialNo = SerialNo + 1
            Else
                e.Row.Cells(1).Text = String.Empty
            End If
            LorryName = TempLorryName
            SnoLbl.Text = SerialNo
        End If
    End Sub
End Class

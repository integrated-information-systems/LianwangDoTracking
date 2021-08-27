Imports System.Web.Configuration
Imports System.Globalization

Partial Class Business_LorryReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim enUS As New CultureInfo("en-US")
        DGSqlSource.SelectCommand = "SELECT  ODRF.DocDate, Lorry, Time, cast(Time as Time) as 'TimeSort', DraftDo.DocNum, ODRF.U_fullloose FROM ODRF INNER JOIN " & WebConfigurationManager.AppSettings("CRM_DB") & ".dbo.DraftDo on draftdo.DocNum=odrf.DocNum  WHERE 1=1   "
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
End Class

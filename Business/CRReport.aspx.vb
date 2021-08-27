Imports CrystalDecisions.CrystalReports.Engine
Imports System.Globalization

Partial Class Business_CRReport
    Inherits System.Web.UI.Page
    Dim enUS As New CultureInfo("en-US")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Response.Write(Server.MapPath("Report.rpt").ToString)
        Dim crystalReport As New ReportDocument()
        crystalReport.Load(Server.MapPath("Report.rpt"))
        crystalReport.SetDatabaseLogon("sa", "B1Admin", "Sapserver", "Lianwang_LIVE")

        If txtFromDate.Text <> String.Empty Or txtToDate.Text <> String.Empty Then

            Dim FromDate As Date
            Dim ToDate As Date
            If Date.TryParseExact(txtFromDate.Text, "d/M/yyyy", enUS, DateTimeStyles.None, FromDate) Then

                crystalReport.SetParameterValue("FromDate", FromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))

            End If
            If Date.TryParseExact(txtToDate.Text, "d/M/yyyy", enUS, DateTimeStyles.None, ToDate) Then
                crystalReport.SetParameterValue("ToDate", ToDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
            End If

            

        End If
       
        CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
        CrystalReportViewer1.HasToggleGroupTreeButton = False
        CrystalReportViewer1.HasToggleParameterPanelButton = False
        CrystalReportViewer1.ReportSource = crystalReport

    End Sub
End Class

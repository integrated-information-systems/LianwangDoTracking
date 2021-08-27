<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="CRReport.aspx.vb" Inherits="Business_CRReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<table width="100%"><tr><td align="right">From Date<asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" ReadOnly="false" MaxLength="10" Width="70px"></asp:TextBox>&nbsp;To Date<asp:TextBox ID="txtToDate" runat="server"  Width="70px" ReadOnly="false" AutoPostBack="true" MaxLength="10"></asp:TextBox></td></tr>
        </table>
<center>
    
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"   ReportSourceID="CrystalReportSource1"   />
    <CR:CrystalReportSource ID="CrystalReportSource1" runat="server" Report-FileName="~/Business/Report.rpt">
    </CR:CrystalReportSource>
    </center>
</asp:Content>



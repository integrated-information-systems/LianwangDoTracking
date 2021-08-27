<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Scan.aspx.vb" Inherits="Business_Scan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Scan Delivery order</h1>
<div style="width:100%; min-height: 420px;padding: 120px 0px 0px 500px;" >
  Scan Do Number    
    <asp:TextBox ID="txtScancode" runat="server" MaxLength="8" BorderColor="Black" 
        AutoPostBack="true" Font-Size="XX-Large"></asp:TextBox>
</div>
</asp:Content>


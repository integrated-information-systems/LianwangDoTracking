<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="DailyReport.aspx.vb" Inherits="Business_LorryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table width="100%"><tr><td align="right">From Date<asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" ReadOnly="false" MaxLength="10" Width="70px"></asp:TextBox>&nbsp;To Date<asp:TextBox ID="txtToDate" runat="server"  Width="70px" ReadOnly="false" AutoPostBack="true" MaxLength="10"></asp:TextBox></td></tr>
        </table>
    <asp:GridView ID="DGReport" runat="server" CellPadding="4" AutoGenerateColumns="false" Width="100%" HeaderStyle-HorizontalAlign="Left" AllowSorting="true"
        DataSourceID="DGSqlSource" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
          <Columns>
                <asp:BoundField DataField="DocDate" HeaderText="Date"  DataFormatString="{0:dd/MM/yy}"
                    SortExpression="DocDate" />
                <asp:BoundField DataField="DocNum" HeaderText="Delivery order no" 
                    SortExpression="DocNum" />              
                <asp:BoundField DataField="Lorry" HeaderText="Lorry Details" 
                    SortExpression="Lorry" />   
                     <asp:BoundField DataField="PrjName" HeaderText="Location"   
                    SortExpression="PrjName" />            
            <asp:TemplateField HeaderText="Time">
                    <HeaderTemplate>
                    <asp:LinkButton ID="lnkTime" runat="server" Text="Time" CommandName="TimeSort" CommandArgument="TimeSort" />                
                    </HeaderTemplate>
                        <ItemTemplate> <asp:Label ID="lblTime" runat="server" Text='<%#Eval("Time") %>' Width="250px"  /> </ItemTemplate>
                </asp:TemplateField>                         
                                    
   <%--             <asp:BoundField DataField="U_Fullloose" HeaderText="Full/Loose" 
                    SortExpression="U_Fullloose" />--%>
           </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="DGSqlSource" runat="server"  ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
            SelectCommand="">
    </asp:SqlDataSource>
</asp:Content>


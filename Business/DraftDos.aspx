<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="DraftDos.aspx.vb" Inherits="Business_Fullwaitingdo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <table width="100%"><tr><td>
    <div style=" position:fixed; top:130px; left:30;"><asp:Button ID="btnFullLoad" runat="server" Text="Full Load" CssClass="tab" /><asp:Button ID="btnLooseLoad" CssClass="tab"
        runat="server" Text="Loose Load" /><asp:Button ID="btnFullDelivered" runat="server" Text="Full Delivered" CssClass="tab" /><asp:Button ID="btnLooseDelivered" CssClass="tab"
        runat="server" Text="Loose Delivered" />            <asp:Button ID="btnScan" runat="server" Text="Scan" CssClass="tab" /></div>
        </td><td align="right">From Date<asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" ReadOnly="false" MaxLength="10" Width="70px"></asp:TextBox>&nbsp;To Date<asp:TextBox ID="txtToDate" runat="server" AutoPostBack="true"  Width="70px" ReadOnly="false" MaxLength="10"></asp:TextBox></td></tr>
        </table>
        
    <asp:MultiView ID="MultiView1" runat="server">
    <asp:View runat="server" ID="FullLooseLoad">
    <table width="100%"><tr><td>
        <asp:GridView ID="DGFullLooseLoad" runat="server" AutoGenerateColumns="False" width="100%" HeaderStyle-HorizontalAlign="Left"
            CellPadding="4" DataSourceID="FullLoadSqlDatasource" ForeColor="#333333"  AllowSorting="true"
            GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="DocNum" HeaderText="DO No" 
                    SortExpression="DocNum" />
                <asp:BoundField DataField="CardName" HeaderText="Customer" 
                    SortExpression="CardName" />
                <asp:BoundField DataField="PrjName" HeaderText="Project" 
                    SortExpression="PrjName" />
                <asp:BoundField DataField="DocDueDate" HeaderText="Delivery Date"  DataFormatString="{0:dd/MM/yy}"
                    SortExpression="DocDueDate" />                    
                 <asp:TemplateField HeaderText="Lorry" SortExpression="txtLorry">
                     <HeaderTemplate>
                    <asp:LinkButton ID="lnkLorry" runat="server" Text="Lorry" CommandName="Lorry" CommandArgument="Lorry" />                
                    </HeaderTemplate>
                        <ItemTemplate> <asp:TextBox ID="txtLorry" AutoPostBack="true"   runat="server"  MaxLength="45" Text='<%#Eval("Lorry") %>'/> </ItemTemplate>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="DO Time">
                    <HeaderTemplate>
                    <asp:LinkButton ID="lnkDOTime" runat="server" Text="DO Time" CommandName="DOTime" CommandArgument="DOTime" />                
                    </HeaderTemplate>
                        <ItemTemplate>
                           <asp:TextBox ID="txtDOTime" ReadOnly="true" Width="80px"   runat="server"  MaxLength="45" Text='<%#Eval("SAPTime") %>'/>  </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Time">
                    <HeaderTemplate>
                    <asp:LinkButton ID="lnkTime" runat="server" Text="Time" CommandName="Time" CommandArgument="Time" />                
                    </HeaderTemplate>
                        <ItemTemplate> <asp:TextBox ID="txtTime" Width="80px" AutoPostBack="true" runat="server" MaxLength="7" Text='<%#Eval("Time") %>'/> </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Reason">
                        <ItemTemplate><asp:Label ID="lblReason" runat="server" Text='<%#Eval("Reason") %>' Width="250px"  /> </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Group">
                        <ItemTemplate>
                            <asp:TextBox ID="txtLorryGroup"  Width="35px" runat="server" Text='<%#Eval("LorryGroup") %>'  AutoPostBack="true" CausesValidation="true" MaxLength="3" ></asp:TextBox>                            
                        </ItemTemplate>
                </asp:TemplateField>             
            </Columns>
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
        </asp:GridView>
        <asp:SqlDataSource ID="FullLoadSqlDatasource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:SAP_DB_ConnectionString %>" 
            SelectCommand="">
        </asp:SqlDataSource></td><td align="right" valign="top">  &nbsp;</td></tr></table>
    </asp:View>
    <asp:View runat="server" ID="Delivered">
    <table width="100%"><tr><td>
        <asp:GridView ID="DGDeliveredFullLooseLoad" runat="server" AutoGenerateColumns="False" width="100%" HeaderStyle-HorizontalAlign="Left"
            CellPadding="4" DataSourceID="FullLoadSqlDatasource" ForeColor="#333333"  AllowSorting="true"
            GridLines="None">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="DocNum" HeaderText="DocNum" 
                    SortExpression="DocNum" />
                <asp:BoundField DataField="CardName" HeaderText="Customer" 
                    SortExpression="CardName" />
                <asp:BoundField DataField="PrjName" HeaderText="Project" 
                    SortExpression="PrjName" />
                <asp:BoundField DataField="DocDueDate" HeaderText="Delivery Date"  DataFormatString="{0:dd/MM/yy}"
                    SortExpression="DocDueDate" />                       
                  <asp:BoundField DataField="SAPTime" HeaderText="DO Time" 
                    SortExpression="SAPTime" />
                     <asp:BoundField DataField="Time" HeaderText="Time" 
                    SortExpression="Time" />
                       <asp:BoundField DataField="Lorry" HeaderText="Lorry" 
                    SortExpression="Lorry" />
                  <asp:TemplateField HeaderText="Reason" >
                  <HeaderStyle Width="100" />
<ItemStyle Width="100" />
                    <HeaderTemplate>
                    <asp:LinkButton ID="lnkReason" runat="server" Text="Reason" CommandName="Reason" CommandArgument="Reason" />                
                    </HeaderTemplate>
                        <ItemTemplate ><asp:TextBox ID="txtReason" AutoPostBack="true" runat="server" Width="450px" MaxLength="350" Text='<%#Eval("Reason") %>'/> </ItemTemplate>
                </asp:TemplateField>              
                <asp:TemplateField>
                         <ItemTemplate ><asp:CheckBox ID="chkPutBack" AutoPostBack="true" runat="server" Checked=<%#Eval("Putback") %> /></ItemTemplate>
                </asp:TemplateField>                 
                 
            </Columns>
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
        </asp:GridView></td><td align="right" valign="top">  <asp:Button ID="btnPutBack" runat="server" Text="Put Back" /></td></tr></table>
    </asp:View>
    </asp:MultiView>
</asp:Content>


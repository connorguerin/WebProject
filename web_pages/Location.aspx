<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPageW3.master" AutoEventWireup="false" CodeFile="Location.aspx.vb" Inherits="Location" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <header class="w3-container w3-blue">
      <h3>Location</h3>
    </header>
    
        <asp:Label ID="lblShift" runat="server"></asp:Label>
    <br />
    
        <asp:GridView ID="gvwEmployees" runat="server">
            <Columns>
                <asp:CommandField SelectText="View Details" ShowSelectButton="True" />
            </Columns>
        </asp:GridView>
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">
        <asp:ListItem></asp:ListItem>
        <asp:ListItem>Shift 1</asp:ListItem>
        <asp:ListItem>Shift 2</asp:ListItem>
        <asp:ListItem>Shift 3</asp:ListItem>
        <asp:ListItem>Shift 4</asp:ListItem>
    </asp:DropDownList>
    
    <p>
        <asp:DetailsView ID="dvwEmployee" runat="server" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" Height="50px" Width="125px" AutoGenerateRows="False" Visible="False">
        <EditRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <Fields>
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" />
            <asp:BoundField DataField="Cost to Hire" DataFormatString="{0:c}" HeaderText="Cost" />
            <asp:BoundField DataField="Shifts" HeaderText="Shifts" />
            <asp:ImageField DataImageUrlField="Picture"  HeaderText="Picture" DataImageUrlFormatString="">
            </asp:ImageField>
        </Fields>
        <FooterStyle BackColor="#CCCCCC" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
        <RowStyle BackColor="White" />
    </asp:DetailsView>
</asp:Content>


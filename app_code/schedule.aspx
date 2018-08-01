<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPageW3.master" AutoEventWireup="false" CodeFile="Schedule.aspx.vb" Inherits="Schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

         <header class="w3-container w3-blue">
      <h3>Employee Schedule</h3>
    </header>
  
         <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
         <asp:Button ID="btnOptimize" runat="server" Text="Optimize" />
  
    <p>
        <asp:GridView ID="gvwSchedule" runat="server" Visible="False">
            <Columns>
                <asp:CommandField SelectText="View Details" ShowSelectButton="True" />
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <asp:Label ID="lblCost" runat="server" Text="Label" Enabled="False" Visible="False"></asp:Label>
</asp:Content>

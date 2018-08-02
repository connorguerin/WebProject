<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPageW3.master" AutoEventWireup="false" CodeFile="Schedule.aspx.vb" Inherits="Schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

         <header class="w3-container w3-blue">
      <h3>Employee Schedule</h3>
    </header>
  
         <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
         <asp:Button ID="btnOptimize" runat="server" Text="Optimize" />
  
         <br />
         <asp:Label ID="lblShift1" runat="server"></asp:Label>
  
    <p>
        &nbsp;&nbsp;&nbsp;
        <asp:GridView ID="gvwSchedule" runat="server">
            <Columns>
                <asp:CommandField SelectText="View Details" ShowSelectButton="True" />
            </Columns>
        </asp:GridView>
    </p>
         <p>
             <asp:Label ID="lblShift2" runat="server"></asp:Label>
             <asp:GridView ID="gvwShift2" runat="server">
                 <Columns>
                     <asp:CommandField SelectText="View Details" ShowSelectButton="True" />
                 </Columns>
             </asp:GridView>
    </p>
         <p>
             <asp:Label ID="lblShift3" runat="server"></asp:Label>
             <asp:GridView ID="gvwShift3" runat="server">
                 <Columns>
                     <asp:CommandField SelectText="View Details" ShowSelectButton="True" />
                 </Columns>
             </asp:GridView>
    </p>
         <p>
             <asp:Label ID="lblShift4" runat="server"></asp:Label>
             <asp:GridView ID="gvwShift4" runat="server">
                 <Columns>
                     <asp:CommandField SelectText="View Details" ShowSelectButton="True" />
                 </Columns>
             </asp:GridView>
    </p>
    <p>
        <asp:Label ID="lblCost" runat="server" Text="Label" Enabled="False" Visible="False"></asp:Label>
</asp:Content>

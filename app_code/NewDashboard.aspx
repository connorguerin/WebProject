<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPageW3.master" AutoEventWireup="false" CodeFile="NewDashboard.aspx.vb" Inherits="NewDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="w3-container w3-blue-grey w3-card">
    <p><i class="fa fa-calendar" style="font-size:24px"></i> Viewing information for&nbsp;
        <asp:Button ID="bMonth" runat="server" Text="<<" Width="38px" />
&nbsp;
        <asp:Button ID="bDay" runat="server" Text="<" Width="25px" />
&nbsp; <asp:Label ID="lblDate" runat="server" Text="Label"></asp:Label>
        &nbsp;
        <asp:Button ID="fDay" runat="server" Text=">" Width="25px" />
&nbsp;
        <asp:Button ID="fMonth" runat="server" Text=">>" Width="37px" />
          </p>
  </div>
   <div class="w3-container w3-padding-16">
  <div class="w3-card-4" style="width:30%;">
    <header class="w3-container w3-blue">
      <h3>Key Metrics</h3>
    </header>

    <div class="w3-container">
      <p>Percent of Rooms Unoccupied: <asp:Label ID="Label1" runat="server" Text="Label" class="w3-right"></asp:Label></p>
        <p>Current Number of Guests: <asp:Label ID="lblGuestCount" runat="server" Text="Label" class="w3-right"></asp:Label></p>
        <p>Number of Departing Reservations: <asp:Label ID="lblDepartingRevs" runat="server" Text="Label" class="w3-right"></asp:Label></p>
        <p>Number of Arriving Reservations: <span class="w3-badge w3-right w3-margin-right"><asp:Label ID="lblArrivingRevs" runat="server" Text="Label" class="w3-right"></asp:Label></span></p>
    </div>

  </div>
</div>
</asp:Content>

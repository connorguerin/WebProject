<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPageW3.master" AutoEventWireup="false" CodeFile="Reservation.aspx.vb" Inherits="Reservation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <header class="w3-container w3-blue">
      <h3>Reservations</h3>
    </header>
        <asp:GridView ID="gvwReservation" runat="server">
           
        </asp:GridView>
    <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
    <asp:Button ID="btnOptimize" runat="server" Text="Optimize" />
</asp:Content>

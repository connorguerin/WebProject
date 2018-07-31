Imports System.Data
Partial Class Reservation
    Inherits System.Web.UI.Page
    Private myDataLoader As New DataLoader

    Private Sub btnOptimize_Click(sender As Object, e As EventArgs) Handles btnOptimize.Click
        Session("Day") = Calendar1.SelectedDate
        Dim planningHorizon As Integer = 2
        Dim CustomerList As List(Of Customer) = myDataLoader.LoadCustomerList
        Dim RoomList As List(Of Room) = myDataLoader.LoadRoomList
        Dim iterationTable As New DataTable

        Dim myRoomOptimizer As New RoomOptimizer(CustomerList, RoomList, Session("Day"))
        Dim fillGridview As New DataTable
        myRoomOptimizer.Solve()
        Dim roomOptimizationResults As OptimizationResults = myRoomOptimizer.Results
        fillGridview = roomOptimizationResults.RoomResultsToDataTable
        gvwReservation.DataSource = fillGridview
        gvwReservation.DataBind()



    End Sub

    Public Sub VacancyChange(ByVal roomNo As Double, taken As Boolean)

    End Sub
End Class

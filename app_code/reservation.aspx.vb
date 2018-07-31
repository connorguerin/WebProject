Imports System.Data
Partial Class Reservation
    Inherits System.Web.UI.Page
    Private myDataLoader As New DataLoader

    Private Sub btnOptimize_Click(sender As Object, e As EventArgs) Handles btnOptimize.Click
        'this code will probably be changed a bit since for the most part the things that are written are for test purposes. for now it runs roomoptimizer for the day you select and the following number of days specified in planning horizon variable
        Dim currentDay As DateTime = Calendar1.SelectedDate
        Dim planningHorizon As Integer = 3
        Dim OutputTable As New DataTable
        For i = 0 To planningHorizon
            'list of customers checking in
            Dim CustomerList As List(Of Customer) = myDataLoader.LoadCustomerList(currentDay.DayOfYear)
            'list of vacant rooms
            Dim RoomList As List(Of Room) = myDataLoader.LoadRoomList(currentDay)
            Dim myRoomOptimizer As New RoomOptimizer(CustomerList, RoomList, currentDay)
            Dim fillGridView As New DataTable
            myRoomOptimizer.Solve()
            Dim roomOptimizationResults As OptimizationResults = myRoomOptimizer.Results
            fillGridView = roomOptimizationResults.RoomResultsToDataTable


            'button click for approval, adds reservations to the access database
            For Each row In fillGridView.Rows
                myDataLoader.CreateReservation(CInt(row("Visit ID")), CDbl(Left(row("Room"), 3)))
            Next
            OutputTable.Merge(fillGridView)
            'update day
            currentDay = currentDay.AddDays(1)
            CustomerList.Clear()
            RoomList.Clear()
        Next
        gvwReservation.DataSource = OutputTable
        gvwReservation.DataBind()


    End Sub


End Class

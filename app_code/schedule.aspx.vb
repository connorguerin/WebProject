Imports System.Data
Partial Class Schedule
    Inherits System.Web.UI.Page
    Dim myDataLoader As New DataLoader

    Private Sub gvwSchedule_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvwSchedule.RowCommand

        If (e.CommandName = "View Details") Then
            Dim rowIndex As Integer = e.CommandArgument
            Dim Location As String = gvwSchedule.Rows.Item(rowIndex).Cells.Item(0).Text
            Session("Location") = myDataLoader.GetLocationInfo(Location)
            Response.Redirect("Location.aspx")
        End If
    End Sub

    Protected Sub btnOptimize_Click(sender As Object, e As EventArgs) Handles btnOptimize.Click
        Dim aDay As Integer = Calendar1.SelectedDate.DayOfYear
        Dim EmployeeList As List(Of Employee) = myDataLoader.LoadEmployeeList
        Dim myScheduleOptimizer As New ScheduleOptimizer(EmployeeList, aDay)
        myScheduleOptimizer.Solve()
        Dim scheduleOptimizationResults As OptimizationResults = myScheduleOptimizer.Results
        Dim fillGridview As DataTable = scheduleOptimizationResults.EmployeeResultsToDataTable
        gvwSchedule.DataSource = fillGridview
        gvwSchedule.DataBind()
    End Sub
End Class

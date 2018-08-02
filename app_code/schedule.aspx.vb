Imports System.Data
Partial Class Schedule
    Inherits System.Web.UI.Page
    Dim myDataLoader As New DataLoader

    Private Sub gvwSchedule_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvwSchedule.RowCommand

        If (e.CommandName = "Select") Then
            Dim rowIndex As Integer = e.CommandArgument
            Dim Location As String = gvwSchedule.Rows.Item(rowIndex).Cells.Item(1).Text
            Session("Location") = Location
            Response.Redirect("~/Location.aspx")
        End If

    End Sub

    Protected Sub btnOptimize_Click(sender As Object, e As EventArgs) Handles btnOptimize.Click
        Dim aDay As Integer = Calendar1.SelectedDate.DayOfYear
        Dim EmployeeList As List(Of Employee) = myDataLoader.LoadEmployeeList


        Dim myScheduleOptimizer As New ScheduleOptimizer2(EmployeeList, aDay)
        myScheduleOptimizer.Solve()
        Dim scheduleOptimizationResultsShift1 As OptimizationResults = myScheduleOptimizer.Results
        Session("Shift1") = scheduleOptimizationResultsShift1.GetEmployeeSchedule
        Dim fillgridview As DataTable = scheduleOptimizationResultsShift1.EmployeeResultsToDataTableTwo
        gvwSchedule.DataSource = fillgridview
        gvwSchedule.DataBind()


        myScheduleOptimizer.Update()
        myScheduleOptimizer.Solve()
        Dim scheduleOptimizationResultsShift2 As OptimizationResults = myScheduleOptimizer.Results
        Session("Shift2") = scheduleOptimizationResultsShift2.GetEmployeeSchedule
        fillgridview = scheduleOptimizationResultsShift2.EmployeeResultsToDataTableTwo
        gvwShift2.DataSource = fillgridview
        gvwShift2.DataBind()


        myScheduleOptimizer.Update()
        myScheduleOptimizer.Solve()
        Dim scheduleOptimizationResultsShift3 As OptimizationResults = myScheduleOptimizer.Results
        Session("Shift3") = scheduleOptimizationResultsShift3.GetEmployeeSchedule
        fillgridview = scheduleOptimizationResultsShift3.EmployeeResultsToDataTableTwo
        gvwShift3.DataSource = fillgridview
        gvwShift3.DataBind()


        myScheduleOptimizer.Update()
        myScheduleOptimizer.Solve()
        Dim scheduleOptimizationResultsShift4 As OptimizationResults = myScheduleOptimizer.Results
        Session("Shift4") = scheduleOptimizationResultsShift4.GetEmployeeSchedule
        fillgridview = scheduleOptimizationResultsShift4.EmployeeResultsToDataTableTwo
        gvwShift4.DataSource = fillgridview
        gvwShift4.DataBind()


        lblShift1.Text = "Shift 1: 12 AM - 6 AM"
        lblShift2.Text = "Shift 2: 6 AM - 12 PM"
        lblShift3.Text = "Shift 3: 12 PM - 6 PM"
        lblShift4.Text = "Shift 4: 6 PM - 12 AM"
    End Sub
End Class

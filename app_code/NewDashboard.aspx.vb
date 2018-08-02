
Partial Class NewDashboard
    Inherits System.Web.UI.Page
    Dim myDataLoader As New DataLoader

    Private Sub NewDashboard_Load(sender As Object, e As EventArgs) Handles Me.Load
        If lblDate.Text = "Label" Then
            Dim aDate As String = Today
            UpdateLabels(aDate)
        Else
            Exit Sub
        End If

    End Sub
    Protected Sub bDay_Click(sender As Object, e As EventArgs) Handles bDay.Click
        Dim curDate As Date = lblDate.Text
        curDate = curDate.AddDays(-1)
        UpdateLabels(curDate)
        lblDate.Text = curDate
    End Sub
    Protected Sub fDay_Click(sender As Object, e As EventArgs) Handles fDay.Click
        Dim curDate As Date = lblDate.Text
        curDate = curDate.AddDays(+1)
        UpdateLabels(curDate)
        lblDate.Text = curDate
    End Sub
    Protected Sub bMonth_Click(sender As Object, e As EventArgs) Handles bMonth.Click
        Dim curDate As Date = lblDate.Text
        curDate = curDate.AddMonths(-1)
        UpdateLabels(curDate)
        lblDate.Text = curDate
    End Sub
    Protected Sub fMonth_Click(sender As Object, e As EventArgs) Handles fMonth.Click
        Dim curDate As Date = lblDate.Text
        curDate = curDate.AddMonths(+1)
        UpdateLabels(curDate)
        lblDate.Text = curDate
    End Sub
    Private Sub UpdateLabels(newDate As Date)
        lblDate.Text = newDate
        If myDataLoader.OccupiedRoomNumbers(newDate).Count > 0 Then
            lblGuestCount.Text = myDataLoader.TotalCurrentGuests(newDate).ToString
            lblDepartingRevs.Text = myDataLoader.TotalDepartingRev(newDate).ToString
            lblArrivingRevs.Text = myDataLoader.TotalArrivingRev(newDate).ToString
        Else
            Exit Sub
        End If
    End Sub
End Class

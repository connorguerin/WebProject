Public Class Employee
    Private ReadOnly staffID As Integer
    Private ReadOnly positionID As Integer
    Private ReadOnly Name As String
    Private ReadOnly DateHired As DateTime

    Public Sub New(aStaffID As Integer, aPositionID As Integer, aName As String, aDateHired As DateTime)
        staffID = aStaffID
        positionID = aPositionID
        Name = aName
        DateHired = aDateHired
        SkillLevel = DateDiff(DateInterval.Year, Today(), DateHired)

    End Sub

    Public Function CanWorkJob(aJobID As Integer) As Integer
        If (aJobID * 2 = positionID Or aJobID * 2 - 1 = positionID) Then
            Return 1
        Else
            Return 0
        End If

    End Function

    Public Function HourlyRate() As Double
        'this is ugly as fuck hard coding all this stuff
        'TODO determine payrates
        If (staffID = 1) Then

        ElseIf (staffID = 2) Then

        ElseIf (staffID = 3) Then

        ElseIf (staffID = 4) Then

        ElseIf (staffID = 5) Then

        ElseIf (staffID = 6) Then

        ElseIf (staffID = 7) Then

        ElseIf (staffID = 8) Then

        ElseIf (staffID = 9) Then

        ElseIf (staffID = 10) Then

        ElseIf (staffID = 10) Then

        Else


        End If
    End Function
    Public Function SkillRating() As Integer

    End Function

    Public Function MaxHours() As Integer
        If (positionID Mod 2 = 0) Then
            Return 40
        Else
            Return 50
        End If
    End Function

    Public Function MinHours() As Integer
        If (positionID Mod 2 = 0) Then
            Return 0
        Else
            Return 40
        End If
    End Function
End Class

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
        Dim baseRate As Integer
        If (staffID = 1) Then
            baseRate = 20
        End If

        If (staffID = 2) Then
            baseRate = 10
        End If

        If (staffID = 3) Then
            baseRate = 22
        End If
        If (staffID = 4) Then
            baseRate = 12
        End If

        If (staffID = 5) Then
            baseRate = 24
        End If
        If (staffID = 6) Then
            baseRate = 14
        End If
        If (staffID = 7) Then
            baseRate = 22
        End If

        If (staffID = 8) Then
            baseRate = 12
        End If
        If (staffID = 9) Then
            baseRate = 23
        End If
        If (staffID = 10) Then
            baseRate = 13
        End If
        If (staffID = 11) Then
            baseRate = 24
        End If
        If (staffID = 12) Then
            baseRate = 14
        End If

        Return baseRate * (1 + (0.05 * SkillRating()))
    End Function
    Public Function SkillRating() As Integer
        Return math.abs(DateDiff(DateInterval.Year, Today(), DateHired))
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

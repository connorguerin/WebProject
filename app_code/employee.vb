Public Class Employee
    Private ReadOnly staffID As Integer
    Private ReadOnly positionID As Integer
    Private ReadOnly Name As String
    Private ReadOnly DateHired As DateTime
    Private ReadOnly SkillLevel As Integer

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

Public Class Employee
    Private ReadOnly staffID As Integer
    Private ReadOnly positionID As Integer
    Private ReadOnly Name As Integer
    Private ReadOnly DateHired As DateTime
    Private ReadOnly SkillLevel As Integer

    Public Sub New(aStaffID, aPositionID, aName, aDateHired)
        staffID = aStaffID
        positionID = aPositionID
        Name = aName
        DateHired = aDateHired
        SkillLevel = DateDiff(DateInterval.Year, Today(), aDateHired())
    End Sub
End Class

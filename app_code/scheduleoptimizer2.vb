Imports Microsoft.VisualBasic
Imports Microsoft.SolverFoundation.Services
Public Class ScheduleOptimizer2

    Private myEmployeeList As List(Of Employee)
    Private myModel As Model
    Private myDecisionMatrix(,) As Decision
    Private Day As Integer
    Private shift As Integer

    Public Sub New(anEmployeeList As List(Of Employee), aDay As Integer)
        myEmployeeList = anEmployeeList
        Dim openingDay As DateTime = "6/1/2018"
        Day = aDay - openingDay.DayOfYear
        Dim size(myEmployeeList.Count - 1, 5) As Decision
        myDecisionMatrix = size
        shift = 1
    End Sub

    Public Sub Solve()
        ' creates a Solver and a Model
        Dim mySolverContext As SolverContext = SolverContext.GetContext
        mySolverContext.ClearModel()
        myModel = mySolverContext.CreateModel
        ' Decision variables
        AddDecisions()
        ' Constraints
        AddConstraints()
        ' Objective Function
        AddGoal()
        ' Solve
        mySolverContext.Solve()   ' or specifically    mySolverContext.Solve(New SimplexDirective)
    End Sub

    Public Sub AddDecisions()
        Dim i, j As Integer
        'employees
        For i = 0 To myEmployeeList.Count - 1
            'jobs
            For j = 0 To 5
                'employee i works job j for shift k
                Dim employeeAssignment As New Decision(Domain.IntegerRange(0, 1), "Worker_" & i & "_job_" & j)
                myDecisionMatrix(i, j) = employeeAssignment
                myModel.AddDecision(employeeAssignment)
            Next
        Next
    End Sub

    Public Sub AddConstraints()
        'employee must be qualified to work the job assigned

        For i = 0 To myEmployeeList.Count - 1
            For j = 0 To 5
                Dim currentQualification As Term = myEmployeeList.Item(i).CanWorkJob(j)

                Dim currentDecision As Decision = myDecisionMatrix(i, j)
                myModel.AddConstraint("Employee_" & i & "_Qual_" & j & "_shift_", currentDecision <= currentQualification)

            Next
        Next


        'the number of employees scheduled to a job and shift must meet demand
        Dim numberAssigned As Term
        Dim demand() As Integer = laborNeeds(100, SeasonFactor(Day), shift)
        For j = 0 To 5
            numberAssigned = 0
            For i = 0 To myEmployeeList.Count - 1
                Dim currentDecision As Decision = myDecisionMatrix(i, j)
                numberAssigned += currentDecision
            Next
            myModel.AddConstraint("Meet_Demand_Job_" & j, numberAssigned >= demand(j))
        Next


        'minimum hours must be met for each employee
        Dim totalHours As Term
        For i = 0 To myEmployeeList.Count - 1
            totalHours = 0
            For j = 0 To 5
                Dim currentDecision As Decision = myDecisionMatrix(i, j)
                totalHours += currentDecision
            Next
            totalHours *= 4
            myModel.AddConstraint("Minimum_Hours_Employee_" & i, totalHours <= myEmployeeList.Item(i).GetHours)
        Next

        'minimum experience must be above a certain amount
        Dim experience As Term
        Dim totalExNeeded() As Integer = ExperienceNeeds(SeasonFactor(Day))
        For j = 0 To 5

            experience = 0
            For i = 0 To myEmployeeList.Count - 1
                Dim currentDecision As Decision = myDecisionMatrix(i, j)
                experience += currentDecision * myEmployeeList.Item(i).SkillRating()
            Next
            myModel.AddConstraint("Minimum_Experience_Job_" & j & "_Shift_", experience >= totalExNeeded(j))

        Next

    End Sub

    Public Sub AddGoal()
        Dim myGoal As Term = 0
        For i = 0 To myEmployeeList.Count - 1
            For j = 0 To 5
                Dim addTerm As Double = myEmployeeList.Item(i).HourlyRate * 4
                myGoal += myDecisionMatrix(i, j) * addTerm

            Next
        Next

        myModel.AddGoal("Minimize_Labor_Costs", GoalKind.Minimize, myGoal)


    End Sub
    Public Sub Update()
        For i = 0 To myEmployeeList.Count - 1
            For j = 0 To 5
                If myDecisionMatrix(i, j).ToDouble = 1 Then
                    myEmployeeList.Item(i).DecreaseHours()
                End If
            Next
        Next
        shift += 1
        Dim size(myEmployeeList.Count - 1, 5) As Decision
        myDecisionMatrix = size
    End Sub
    Public Function laborNeeds(n As Integer, seasonFactor As Integer, shift As Integer) As Integer()
        'n is the number of hotel reservations
        'season factor determines volume based on what season is selected. Default should be 1
        Dim returnVector(5) As Integer
        'raw numbers of expected customers in a given place at given shift

        'housekeeps needed
        Dim S2S3S4Housekeeps As Integer = Math.Ceiling(n * seasonFactor / 40)

        'front desk needed
        Dim S2S3S4FrontDesk As Integer = Math.Ceiling(n * seasonFactor / 40)
        If shift = 1 Then
            Dim S1Housekeeps As Integer = Math.Ceiling(n * seasonFactor / 30)
            Dim S1FrontDesk As Integer = Math.Ceiling(n * seasonFactor / 100)
            returnVector(0) = S1Housekeeps
            returnVector(1) = S1FrontDesk
        ElseIf shift = 2 Then
            Dim restS2 As Double = Math.Ceiling(0.5 * 0.25 * n) * seasonFactor
            Dim beachS2 As Double = Math.Ceiling(0.7 * 0.35 * n) * seasonFactor
            Dim spaS2 As Double = Math.Ceiling(0.7 * 0.2 * n) * seasonFactor
            Dim S2Lifeguards As Integer = Math.Ceiling(beachS2 / 40)
            Dim S2BeachAttendants As Integer = Math.Ceiling(beachS2 / 50)
            Dim S2RestaurantWorkers As Integer = Math.Ceiling(restS2 / 40)
            Dim S2SpaWorkers As Integer = Math.Ceiling(spaS2 / 30)
            returnVector(0) = S2S3S4Housekeeps
            returnVector(1) = S2S3S4FrontDesk
            returnVector(2) = S2Lifeguards
            returnVector(3) = S2BeachAttendants
            returnVector(4) = S2SpaWorkers
            returnVector(5) = S2RestaurantWorkers
        ElseIf shift = 3 Then
            Dim restS3 As Double = Math.Ceiling(0.5 * 0.25 * n) * seasonFactor
            Dim beachS3 As Double = Math.Ceiling(0.7 * 0.65 * n) * seasonFactor
            'spa
            Dim spaS3 As Double = Math.Ceiling(0.3 * 0.2 * n) * seasonFactor
            Dim S3Lifeguards As Integer = Math.Ceiling(beachS3 / 40)
            'beach attendant needed
            Dim S3BeachAttendants As Integer = Math.Ceiling(beachS3 / 30)
            Dim S3SpaWorkers As Integer = Math.Ceiling(spaS3 / 40)
            Dim S3RestaurantWorkers As Integer = Math.Ceiling(restS3 / 30)
            returnVector(0) = S2S3S4Housekeeps
            returnVector(1) = S2S3S4FrontDesk
            returnVector(2) = S3Lifeguards
            returnVector(3) = S3BeachAttendants
            returnVector(4) = S3SpaWorkers
            returnVector(5) = S3RestaurantWorkers
        ElseIf shift = 4 Then
            Dim restS4 As Double = Math.Ceiling(0.5 * 0.5 * n) * seasonFactor
            Dim S4RestaurantWorkers As Integer = Math.Ceiling(restS4 / 30)
            returnVector(0) = S2S3S4Housekeeps
            returnVector(1) = S2S3S4FrontDesk
            returnVector(5) = S4RestaurantWorkers
            'beach
        End If

        'matrix to be returned that says how many workers needed for each job for each shift
        'locations: 0-housekeep, 1-frontdesk, 2-lifeguard, 3-beach, 4-spa, 5-restaurant
        'shifts: 0-12am to 6am, 1-6am to noon, 2-noon to 6pm, 3-6pm to midnight
        Return returnVector

    End Function

    Public Function SeasonFactor(day As Integer) As Double
        'Assuming the resort is open for 3 months (93 days) and the peak season is the midpoint (day 47), where the default factor is 1 and the peak factor = 1.5 the season factor can be described as a function of the day, x where f(x) = -0.00023127 * x^2 + 0.021508 * x + 1
        Return -0.00023127 * day ^ 2 + 0.021508 * day + 1
    End Function

    Public Function ExperienceNeeds(seasonFactor As Double) As Integer()
        'this determines what the minimum average of employee experience levels should be for each shift of each job
        'as a function of the season

        'initialize matrix
        Dim experienceMatrix(5) As Integer
        'default values of 1
        For i = 0 To 5
            experienceMatrix(i) = 3
        Next

        'research shows lifeguard and restaurant  positions need higher experienced employees based on seasonal increases
        experienceMatrix(2) += Math.Ceiling(seasonFactor * 2)
        experienceMatrix(5) += Math.Ceiling(seasonFactor * 2)
        Return experienceMatrix


    End Function

    Public Function Results() As OptimizationResults
        Dim total As Integer = 0
        For i = 0 To myEmployeeList.Count - 1
            For j = 0 To 5
                If myDecisionMatrix(i, j).ToDouble = 1 Then
                    total += 1
                End If
            Next
        Next
        Return New OptimizationResults(myDecisionMatrix, myEmployeeList)
    End Function
End Class

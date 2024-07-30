Public Class SiteTailorFunctionAuthorization
    Public Description As String
    Public AccessLevel As Integer
    Public PathFromRoot As String
    Public Writable As Boolean
    Public FunctionCode As String
    Public HandledByCustCA As Boolean
    Public Disabled As Boolean

    Public Sub New(ByVal description As String, ByVal accessLevel As Integer, ByVal pathFromRoot As String, writable As Boolean, functionCode As String, handledByCustCA As Boolean)
        Me.Description = description
        Me.AccessLevel = accessLevel
        Me.PathFromRoot = pathFromRoot
        Me.Writable = Writable
        Me.FunctionCode = functionCode
        Me.HandledByCustCA = handledByCustCA
        Me.Disabled = False
    End Sub
End Class

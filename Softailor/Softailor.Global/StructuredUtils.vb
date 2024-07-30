Imports System.Web.UI.WebControls

Namespace StructuredUtils

    Public Class GenericIntList
        Inherits List(Of Integer)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(cbl As CheckBoxList)
            Me.New()
            For Each li As ListItem In cbl.Items
                If li.Selected Then
                    Me.Add(CInt(li.Value))
                End If
            Next
        End Sub

        Public Function GetTable() As DataTable
            Dim DT As New DataTable
            DT.Columns.Add("VALO_INT", Type.GetType("System.Int32"))
            Dim row As DataRow
            For Each integerValue In Me
                row = DT.NewRow
                row("VALO_INT") = integerValue
                DT.Rows.Add(row)
            Next
            Return DT
        End Function

        Public Function isEmpty() As Boolean
            Return Me.Count = 0
        End Function
    End Class

    Public Class GenericStringList
        Inherits List(Of String)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(cbl As CheckBoxList)
            Me.New()
            For Each li As ListItem In cbl.Items
                If li.Selected Then
                    Me.Add(li.Value)
                End If
            Next
        End Sub

        Public Function GetTable() As DataTable
            Dim DT As New DataTable
            DT.Columns.Add("VALO_STR", Type.GetType("System.String"))
            Dim row As DataRow
            For Each stringValue In Me
                row = DT.NewRow
                row("VALO_STR") = stringValue
                DT.Rows.Add(row)
            Next
            Return DT
        End Function

        Public Function isEmpty() As Boolean
            Return Me.Count = 0
        End Function

    End Class

    Public Class GenericDateList
        Inherits List(Of Date)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(cbl As CheckBoxList)
            Me.New()
            For Each li As ListItem In cbl.Items
                If li.Selected Then
                    Me.Add(Date.ParseExact(li.Value, "yyyyMMdd", Softailor.Global.Cultures.CulturaEnglish))
                End If
            Next
        End Sub

        Public Function GetTable() As DataTable
            Dim DT As New DataTable
            DT.Columns.Add("VALO_DTI", Type.GetType("System.DateTime"))
            Dim row As DataRow
            For Each dateTimeValue In Me
                row = DT.NewRow
                row("VALO_DTI") = dateTimeValue
                DT.Rows.Add(row)
            Next
            Return DT
        End Function

        Public Function isEmpty() As Boolean
            Return Me.Count = 0
        End Function

    End Class


    Public Class IntStringStringInt
        Public int1 As Integer = 0
        Public string1 As String = ""
        Public string2 As String = ""
        Public int2 As Integer = 0
    End Class

    Public Class GenericIntStringStringIntList
        Inherits List(Of IntStringStringInt)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Function GetTable() As DataTable

            Dim DT As New DataTable
            DT.Columns.Add("VALO_INT", Type.GetType("System.Int32"))
            DT.Columns.Add("VALO_ST1", Type.GetType("System.String"))
            DT.Columns.Add("VALO_ST2", Type.GetType("System.String"))
            DT.Columns.Add("VALO_IN2", Type.GetType("System.Int32"))

            Dim row As DataRow

            For Each myValue In Me
                row = DT.NewRow
                row("VALO_INT") = myValue.int1
                row("VALO_ST1") = myValue.string1
                row("VALO_ST2") = myValue.string2
                row("VALO_IN2") = myValue.int2
                DT.Rows.Add(row)
            Next
            Return DT

        End Function

        Public Function isEmpty() As Boolean
            Return Me.Count = 0
        End Function

    End Class

    Public Class SevenInt
        Public int1 As Integer = 0
        Public int2 As Integer = 0
        Public int3 As Integer = 0
        Public int4 As Integer = 0
        Public int5 As Integer = 0
        Public int6 As Integer = 0
        Public int7 As Integer = 0
    End Class

    Public Class Generic7IntList
        Inherits List(Of SevenInt)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Function GetTable() As DataTable
            Dim DT As New DataTable
            DT.Columns.Add("VALO_IN1", Type.GetType("System.Int32"))
            DT.Columns.Add("VALO_IN2", Type.GetType("System.Int32"))
            DT.Columns.Add("VALO_IN3", Type.GetType("System.Int32"))
            DT.Columns.Add("VALO_IN4", Type.GetType("System.Int32"))
            DT.Columns.Add("VALO_IN5", Type.GetType("System.Int32"))
            DT.Columns.Add("VALO_IN6", Type.GetType("System.Int32"))
            DT.Columns.Add("VALO_IN7", Type.GetType("System.Int32"))
            Dim row As DataRow
            For Each sevenInt In Me
                row = DT.NewRow
                row("VALO_IN1") = sevenInt.int1
                row("VALO_IN2") = sevenInt.int2
                row("VALO_IN3") = sevenInt.int3
                row("VALO_IN4") = sevenInt.int4
                row("VALO_IN5") = sevenInt.int5
                row("VALO_IN6") = sevenInt.int6
                row("VALO_IN7") = sevenInt.int7
                DT.Rows.Add(row)
            Next
            Return DT
        End Function

        Public Function isEmpty() As Boolean
            Return Me.Count = 0
        End Function
    End Class

    Public Class IntIntDateTime
        Public int1 As Integer = 0
        Public int2 As Integer = 0
        Public dateTime As DateTime
    End Class

    Public Class GenericIntIntDateTimeList
        Inherits List(Of IntIntDateTime)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Function GetTable() As DataTable

            Dim DT As New DataTable
            DT.Columns.Add("VALO_IN1", Type.GetType("System.Int32"))
            DT.Columns.Add("VALO_IN2", Type.GetType("System.Int32"))
            DT.Columns.Add("VALO_DTI", Type.GetType("System.DateTime"))

            Dim row As DataRow

            For Each myValue In Me
                row = DT.NewRow
                row("VALO_IN1") = myValue.int1
                row("VALO_IN2") = myValue.int2
                row("VALO_DTI") = myValue.dateTime
                DT.Rows.Add(row)
            Next
            Return DT

        End Function

        Public Function isEmpty() As Boolean
            Return Me.Count = 0
        End Function

    End Class


End Namespace

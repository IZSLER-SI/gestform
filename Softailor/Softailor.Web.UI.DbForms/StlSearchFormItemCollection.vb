Public Class StlSearchFormItemCollection
    Inherits List(Of IStlSearchFormItem)

    Private _searchForm As StlSearchForm

    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal searchForm As StlSearchForm)
        Me.new()
        _searchForm = searchForm
    End Sub

    Public Overloads Sub Add(ByVal value As IStlSearchFormItem)
        value.SearchForm = _searchForm
        MyBase.Add(value)
    End Sub

    Public Overloads Sub Sort()
        MyBase.Sort(New StlSearchFormItemComparer)
    End Sub

    Public Function FindByNOMECAMP(ByVal NOMECAMP As String) As IStlSearchFormItem
        For Each item As IStlSearchFormItem In Me
            If item.NOMECAMP = NOMECAMP Then Return item
        Next
        Return Nothing
    End Function
End Class

Friend Class StlSearchFormItemComparer
    Inherits Comparer(Of IStlSearchFormItem)

    Public Overloads Overrides Function Compare(ByVal x As IStlSearchFormItem, ByVal y As IStlSearchFormItem) As Integer
        Return CType(x, IStlSearchFormItem).OrderIndex.CompareTo(CType(y, IStlSearchFormItem).OrderIndex)
    End Function

End Class

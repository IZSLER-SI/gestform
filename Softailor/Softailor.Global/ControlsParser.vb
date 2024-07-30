Imports System.Web.UI.WebControls
Imports System.Data.SqlTypes

Public Class ControlsParser

    Public Shared Function DropDownSqlInt32(d As DropDownList) As SqlInt32
        If d.SelectedValue = "" Then
            Return SqlInt32.Null
        Else
            Return New SqlInt32(CInt(d.SelectedValue))
        End If
    End Function

    Public Shared Function DropDownSqlString(d As DropDownList) As SqlString
        If d.SelectedValue = "" Then
            Return SqlString.Null
        Else
            Return New SqlString(d.SelectedValue)
        End If
    End Function

    Public Shared Function TextBoxSqlString(t As TextBox, Optional trim As Boolean = True) As SqlString
        If trim Then
            If t.Text.Trim = "" Then
                Return SqlString.Null
            Else
                Return New SqlString(t.Text.Trim)
            End If
        Else
            If t.Text = "" Then
                Return SqlString.Null
            Else
                Return New SqlString(t.Text)
            End If
        End If
    End Function

    Public Shared Function TextBoxSqlInt32(t As TextBox) As SqlInt32
        If t.Text.Trim = "" Then
            Return SqlInt32.Null
        Else
            Return New SqlInt32(CInt(t.Text.Trim))
        End If
    End Function

    Public Shared Function TextBoxSqlDateTimeDDMMYYYY(t As TextBox) As SqlDateTime

        If t.Text = "" Or t.Text = "__/__/____" Then
            Return SqlDateTime.Null
        Else
            Return New SqlDateTime(ValidationUtils.ParseItalianDate(t.Text))
        End If

    End Function


End Class

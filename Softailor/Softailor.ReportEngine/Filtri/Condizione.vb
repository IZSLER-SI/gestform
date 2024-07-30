Public Class Condizione

    <XmlAttribute>
    Public Property NomeCampoDb As String

    <XmlAttribute>
    Public Property Comparazione As Comparazioni

    Public Property Valori As List(Of Valore)

    Public Sub New()
        Valori = New List(Of Valore)
    End Sub

    Public Function GetFilter(myCampo As Campo) As String

        Dim filtro As String

        filtro = vbCrLf & vbTab & "("

        filtro &= GetFilterInt(myCampo)    'vbTab & vbTab & myCampo.NomeDb

        filtro &= vbCrLf & vbTab & ")"

        Return filtro

    End Function
    Private Function GetFilterInt(myCampo As Campo) As String

        Dim filtro As String = ""

        Select Case Comparazione
            Case Comparazioni.Vuoto
                'NULLA
                filtro = vbCrLf & vbTab & vbTab & myCampo.NomeDb & " is null"

            Case Comparazioni.NonVuoto
                'NULLA
                filtro = vbCrLf & vbTab & vbTab & myCampo.NomeDb & " is not null"

            Case Comparazioni.Uguale
                'lista o IN (MULTIPLO)
                filtro = GetFilterInt_Equal(myCampo)

            Case Comparazioni.Diverso
                'lista o IN (MULTIPLO)
                filtro = GetFilterInt_Different(myCampo)

            Case Comparazioni.Maggiore, Comparazioni.Minore, Comparazioni.MaggioreUguale, Comparazioni.MinoreUguale
                'singolo valore (SINGOLO)
                filtro = GetFilterInt_MajMin(myCampo, Comparazione)
            
            Case Comparazioni.Compreso
                'compreso, con eventuali OR (MULTIPLO)
                filtro = GetFilterInt_Between(myCampo)

            Case Comparazioni.IniziaPer, Comparazioni.FiniscePer, Comparazioni.Contiene
                'stringhe, con LIKE e OR (MULTIPLO)
                filtro = GetFilterInt_Like(myCampo, Comparazione)
        End Select

        Return filtro




    End Function

    Private Function GetFilterInt_Equal(myCampo As Campo) As String

        Dim filtro As String
        Dim cValore As Integer

        Select Case Valori.Count
            Case 0
                Throw New ArgumentOutOfRangeException
            Case 1
                'singolo
                filtro = vbCrLf & vbTab & vbTab & myCampo.NomeDb & " = " & Valori(0).SqlValue(myCampo.Tipo)
            Case Else
                'lista multipla
                filtro = vbCrLf & vbTab & vbTab & myCampo.NomeDb & " IN ("
                For cValore = 0 To Valori.Count - 1
                    filtro &= If(cValore = 0, "", ",") & Valori(cValore).SqlValue(myCampo.Tipo)
                Next
                filtro &= ")"
        End Select

        Return filtro

    End Function

    Private Function GetFilterInt_Different(myCampo As Campo) As String

        Dim filtro As String
        Dim cValore As Integer

        Select Case Valori.Count
            Case 0
                Throw New ArgumentOutOfRangeException
            Case 1
                'singolo
                filtro = vbCrLf & vbTab & vbTab & myCampo.NomeDb & " <> " & Valori(0).SqlValue(myCampo.Tipo)
            Case Else
                'lista multipla
                filtro = vbCrLf & vbTab & vbTab & myCampo.NomeDb & " NOT IN ("
                For cValore = 0 To Valori.Count - 1
                    filtro &= If(cValore = 0, "", ",") & Valori(cValore).SqlValue(myCampo.Tipo)
                Next
                filtro &= ")"
        End Select

        Return filtro

    End Function

    Private Function GetFilterInt_MajMin(myCampo As Campo, myComparazione As Comparazioni) As String

        Dim filtro As String

        If Valori.Count <> 1 Then
            Throw New ArgumentOutOfRangeException
        End If
        'singolo
        Dim cString As String = ""
        Select Case Comparazione
            Case Comparazioni.Maggiore
                cString = ">"
            Case Comparazioni.MaggioreUguale
                cString = ">="
            Case Comparazioni.Minore
                cString = "<"
            Case Comparazioni.MinoreUguale
                cString = "<="
        End Select
        filtro = vbCrLf & vbTab & vbTab & myCampo.NomeDb & " " & cString & " " & Valori(0).SqlValue(myCampo.Tipo)

        Return filtro

    End Function

    Private Function GetFilterInt_Between(myCampo As Campo) As String

        Dim filtro As String = ""
        Dim cValore As Integer

        Select Case Valori.Count
            Case 0
                Throw New ArgumentOutOfRangeException
            Case Else
                'lista multipla
                For cValore = 0 To Valori.Count - 1
                    filtro &= vbCrLf & vbTab & vbTab
                    If cValore > 0 Then filtro &= "OR "
                    filtro &= "(" & myCampo.NomeDb & " BETWEEN " & Valori(cValore).SqlValue(myCampo.Tipo) & " AND " & Valori(cValore).SqlValue2(myCampo.Tipo) & ")"
                Next
        End Select

        Return filtro

    End Function

    Private Function GetFilterInt_Like(myCampo As Campo, myComparazione As Comparazioni) As String

        If myCampo.Tipo <> TipoDato.Stringa Then
            Throw New ArgumentOutOfRangeException
        End If

        Dim filtro As String = ""
        Dim cValore As Integer

        Select Case Valori.Count
            Case 0
                Throw New ArgumentOutOfRangeException
            Case Else
                'lista multipla
                For cValore = 0 To Valori.Count - 1
                    filtro &= vbCrLf & vbTab & vbTab
                    If cValore > 0 Then filtro &= "OR "
                    filtro &= "(" & myCampo.NomeDb & " LIKE "

                    Dim v1 = Valori(cValore).v1

                    Select Case myComparazione
                        Case Comparazioni.IniziaPer
                            filtro &= "'" & Replace(v1, "'", "''") & "%'"
                        Case Comparazioni.FiniscePer
                            filtro &= "'%" & Replace(v1, "'", "''") & "'"
                        Case Comparazioni.Contiene
                            filtro &= "'%" & Replace(v1, "'", "''") & "%'"
                    End Select

                    filtro &= ")"
                Next
        End Select

        Return filtro

    End Function
End Class

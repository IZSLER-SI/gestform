Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Softailor.Global.SqlUtils

Public Class VatCodeValidator

    Public ac_NAZIONEISO As String = ""
    Public ac_VALIDAZIONEPARTITAIVA As String = ""
    Public ac_LINGUA As String = ""
    Public fl_EU As Boolean = False

    Public Sub New(dbConn As SqlConnection, ac_NAZIONEISO As String, ac_LINGUA As String)

        Me.ac_LINGUA = ac_LINGUA

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = dbConn.CreateCommand
        dbCmd.CommandType = CommandType.StoredProcedure
        dbCmd.CommandText = "sp_est_GetDatiNazione"
        dbCmd.Parameters.Add("@ac_nazioneiso", SqlDbType.NVarChar, 2).Value = ac_NAZIONEISO
        dbRdr = dbCmd.ExecuteReader
        If dbRdr.Read Then
            Me.ac_NAZIONEISO = dbRdr.GetString(0)
            Me.ac_VALIDAZIONEPARTITAIVA = Nz(dbRdr.GetSqlString(1), "")
            Me.fl_EU = dbRdr.GetBoolean(2)
        End If
        dbRdr.Close()
        dbCmd.Dispose()

    End Sub

    Public Function ValidateVATCode(ByVal vatCode As String, ByVal emptyIsOk As Boolean) As String

        'restituisce "" in caso di OK, oppure l'errore localizzato

        Dim vc As String = vatCode.Trim.ToUpper

        If vc = "" Then
            'partita IVA vuota
            If emptyIsOk Then
                Return ""
            Else
                Select Case ac_LINGUA
                    Case "it"
                        Return "Partita IVA obbligatoria."
                    Case Else
                        Return "VAT code / Tax ID is required."
                End Select
            End If

        End If

        'OK, non è vuota.
        If ac_VALIDAZIONEPARTITAIVA = "ITALY" Then
            'Italia
            If ValidationUtils.ValidatePartitaIVAItaliana(vc) Then
                Return ""
            Else
                Select Case ac_LINGUA
                    Case "it"
                        Return "Partita IVA scorretta."
                    Case Else
                        Return "Incorrect VAT code."
                End Select
            End If
        ElseIf ac_VALIDAZIONEPARTITAIVA = "" Then
            'resto del mondo
            Return ""
        Else
            'unione europea, tranne Italia
            Return ValidateVATCodeEU(vc)
        End If

    End Function

    Private Function ValidateVATCodeEU(ByVal vc As String) As String

        'calcoli stato per stato
        Dim result As String = ""
        Dim regex As Regex

        Select Case ac_VALIDAZIONEPARTITAIVA
            Case "AUSTRIA"
                regex = New Regex("^U\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato Uxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: Uxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "BELGIUM"
                regex = New Regex("^0\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato 0xxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: 0xxxxxxxx (x=digit)."
                    End Select
                End If
            Case "BULGARIA"
                regex = New Regex("^\d{9,10}$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxx oppure xxxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxx or xxxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "CYPRUS"
                regex = New Regex("^\d\d\d\d\d\d\d\d[A-Z]$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxA (x=cifra, A=lettera)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxA (x=digit, A=letter)."
                    End Select
                End If
            Case "DENMARK"
                regex = New Regex("^\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxx (x=digit)."
                    End Select
                End If
            Case "ESTONIA"
                regex = New Regex("^\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "FINLAND"
                regex = New Regex("^\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxx (x=digit)."
                    End Select
                End If
            Case "FRANCE"
                regex = New Regex("^\w\w\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato XXxxxxxxxxx (X=cifra o lettera, x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: XXxxxxxxxxx (X=letter or digit, x=digit)."
                    End Select
                End If
            Case "GERMANY"
                regex = New Regex("^\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "UNITED KINGDOM"
                regex = New Regex("^$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc, "^\d{9,9}$") And Not regex.IsMatch(vc, "^\d{12,12}$") And Not regex.IsMatch(vc, "^[Gg][Dd]\d\d\d$") And Not regex.IsMatch(vc, "^[Hh][Aa]\d\d\d$") Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxx, xxxxxxxxxxxx, GDxxx o HAxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxx, xxxxxxxxxxxx, GDxxx or HAxxx (x=digit)."
                    End Select
                End If
            Case "GREECE"
                regex = New Regex("^\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "IRELAND"
                regex = New Regex("^\d\w\d\d\d\d\d[A-Z]$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xXxxxxxA (x=cifra, X=cifra o lettera, A=lettera)."
                        Case Else
                            Return "Invalid VAT code. Required format: xXxxxxxA (x=digit, X=digit or letter, A=letter)."
                    End Select
                End If
            Case "LATVIA"
                regex = New Regex("^\d\d\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "LITHUANIA"
                regex = New Regex("", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc, "^\d{9,9}$") And Not regex.IsMatch(vc, "^\d{12,12}$") Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxx o xxxxxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxx or xxxxxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "LUXEMBOURG"
                regex = New Regex("^\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxx (x=digit)."
                    End Select
                End If
            Case "MALTA"
                regex = New Regex("^\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxx (x=digit)."
                    End Select
                End If
            Case "NETHERLANDS"
                regex = New Regex("^\d\d\d\d\d\d\d\d\dB\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxxBxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxxBxx (x=digit)."
                    End Select
                End If
            Case "POLAND"
                regex = New Regex("^\d\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "PORTUGAL"
                regex = New Regex("^\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "CZECH REPUBLIC"
                regex = New Regex("^\d{8,10}$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxx o xxxxxxxxx o xxxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxx or xxxxxxxxx or xxxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "ROMANIA"
                regex = New Regex("^\d{2,10}$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere composta da un minimo di 2 a un massimo di 10 cifre."
                        Case Else
                            Return "Invalid VAT code. Required format: 2-10 digits."
                    End Select
                End If
            Case "SLOVAKIA"
                regex = New Regex("^\d\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "SLOVENIA"
                regex = New Regex("^\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxx (x=digit)."
                    End Select
                End If
            Case "SPAIN"
                regex = New Regex("^\w\d\d\d\d\d\d\d\w$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato XxxxxxxxX (X=cifra o lettera, x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: XxxxxxxxX (X=letter or digit, x=digit)."
                    End Select
                End If
            Case "SWEDEN"
                regex = New Regex("^\d\d\d\d\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxxxxxx (x=digit)."
                    End Select
                End If
            Case "HUNGARY"
                regex = New Regex("^\d\d\d\d\d\d\d\d$", RegexOptions.IgnoreCase)
                If Not regex.IsMatch(vc) Then
                    Select Case ac_LINGUA
                        Case "it"
                            Return "Formato non valido. La partita IVA deve essere nel formato xxxxxxxx (x=cifra)."
                        Case Else
                            Return "Invalid VAT code. Required format: xxxxxxxx (x=digit)."
                    End Select
                End If
        End Select


        Return result


    End Function
End Class

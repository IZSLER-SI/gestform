Imports System.Data.SqlTypes
Imports System.Text

Public Class ValidationUtils
		Public Shared Function ValidateHourMinute(ByVal s As String, Optional ByVal culture As System.Globalization.CultureInfo = Nothing) As Boolean

				'se non passo la cultura, prendo quella dell'UI

				If culture Is Nothing Then culture = System.Globalization.CultureInfo.CurrentUICulture

				'accetta ore nel formato HH:mm o H:mm o HH.mm o H.mm

				Dim sOra As String = s.Trim

				'cambio i : in . o viceversa in base a quanto richiesto dalla cultura vigente
				If culture.DateTimeFormat.TimeSeparator = "." Then
						sOra = sOra.Replace(":", ".")
				ElseIf culture.DateTimeFormat.TimeSeparator = ":" Then
						sOra = sOra.Replace(".", ":")
				End If

				'ok ora provo a fare il parse
				Try
						Dim ora As Date = Date.ParseExact(sOra, "H:mm", culture)
						Return True
				Catch ex As Exception
						Return False
				End Try

		End Function

		Public Shared Function ParseHourMinute(ByVal s As String, Optional ByVal culture As System.Globalization.CultureInfo = Nothing) As Date

				'se non passo la cultura, prendo quella dell'UI

				If culture Is Nothing Then culture = System.Globalization.CultureInfo.CurrentUICulture

				'accetta ore nel formato HH:mm o H:mm o HH.mm o H.mm

				Dim sOra As String = s.Trim

				'cambio i : in . o viceversa in base a quanto richiesto dalla cultura vigente
				If culture.DateTimeFormat.TimeSeparator = "." Then
						sOra = sOra.Replace(":", ".")
				ElseIf culture.DateTimeFormat.TimeSeparator = ":" Then
						sOra = sOra.Replace(".", ":")
				End If

				'faccio il parse (mi aspetto che i dati siano validi)
				Return Date.ParseExact(sOra, "H:mm", culture)

		End Function

		Public Shared Function ValidateEmail(ByVal mail As String) As Boolean
				If mail = "" Then
						Return False
				Else
						If mail Like "*softailor.local" Then
								Return True
						Else
								Dim strRegex As String = "^([a-zA-Z0-9'_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,6}|[0-9]{1,3})(\]?)$"
								Dim re As New System.Text.RegularExpressions.Regex(strRegex)
								Return re.IsMatch(mail)
						End If
				End If
		End Function

		Public Shared Function ValidateItalianDate(ByVal sDate As String) As Boolean
				Dim sMyDate As String = sDate.Trim
				Dim myDate As Date
				Dim formats(1) As String
				formats(0) = "d/M/yyyy"
				formats(1) = "d/M/yy"
				Try
						myDate = Date.ParseExact(sMyDate, formats, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None)
						ValidateItalianDate = True
				Catch ex As Exception
						ValidateItalianDate = False
				End Try
		End Function

		Public Shared Function ParseItalianDate(ByVal sDate As String) As Date
				Dim sMyDate As String = sDate.Trim
				Dim formats(1) As String
				formats(0) = "d/M/yyyy"
				formats(1) = "d/M/yy"
				ParseItalianDate = Date.ParseExact(sMyDate, formats, Softailor.Global.Cultures.CulturaItalian, Globalization.DateTimeStyles.None)
		End Function

		Public Shared Function FormatItalianDateY2(ByVal d As Date) As String
				Return d.ToString("dd/MM/yy", Cultures.CulturaItalian)
		End Function

		Public Shared Function FormatItalianDateY4(ByVal d As Date) As String
				Return d.ToString("dd/MM/yyyy", Cultures.CulturaItalian)
		End Function

		Public Shared Function ValidateItalianDecimal(ByVal s As String) As Boolean
				If s.Trim = "" Then Return False
				Try
						Dim d As Decimal = Decimal.Parse(s.Trim, Cultures.CulturaItalian)
						Return True
				Catch ex As Exception
						Return False
				End Try
		End Function

		Public Shared Function ValidateInteger(ByVal s As String) As Boolean
				If s.Trim = "" Then Return False
				Try
						Dim i As Integer = Integer.Parse(s.Trim)
						Return True
				Catch ex As Exception
						Return False
				End Try
		End Function

		Public Shared Function ParseInteger(ByVal s As String) As Integer

				Return Integer.Parse(s.Trim)

		End Function

		Public Shared Function ValidateItalianDecimalConSeparatoreMigliaia(ByVal s As String) As Boolean
				If s.Trim = "" Then Return False
				Try
						Dim d As Decimal = Decimal.Parse(s.Trim, Cultures.CulturaItalianConSeparatoreMigliaia)
						Return True
				Catch ex As Exception
						Return False
				End Try
		End Function

		Public Shared Function FormatItalianCurrency(ByVal d As Decimal) As String
				Return d.ToString("#0.00", Cultures.CulturaItalian)
		End Function

		Public Shared Function FormatItalianCurrencyConSeparatoreMigliaia(ByVal d As Decimal) As String
				Return d.ToString("#,##0.00", Cultures.CulturaItalianConSeparatoreMigliaia)
		End Function

		Public Shared Function ParseItalianDecimal(ByVal d As String) As Decimal
				Return Decimal.Parse(d.Trim, Cultures.CulturaItalian)
		End Function

		Public Shared Function ParseItalianDecimalConSeparatoreMigliaia(ByVal d As String) As Decimal
				Return Decimal.Parse(d.Trim, Cultures.CulturaItalianConSeparatoreMigliaia)
		End Function

		Public Shared Function FormatItalianDecimal(ByVal d As Decimal) As String
				Return d.ToString("#0.####", Cultures.CulturaItalian)
		End Function

		Public Shared Function ValidateCodiceFiscaleItaliano(ByVal sorgen As String) As Boolean

				'   sorgen  IN  stringa contenente il C.F. / P.I.

				Dim i As Integer, j As Integer, k As Integer
				Dim A As Integer, B As String, pari As Boolean
				Dim espres As String

				If sorgen = "" Then
						ValidateCodiceFiscaleItaliano = False
				Else
						espres = Left(sorgen & Space(16), 16)
						A = 0
						pari = False
						B = "A01B00C05D07E09F13G15H17I19J21K02L04M18N20O11P03Q06R08S12T14U16V10W22X25Y24Z23"
						For i = 1 To 15
								k = Asc(Mid(espres, i, 1)) - 48
								k = If(k < 0, 0, k)
								j = InStr(B, Mid(espres, i, 1))
								If pari Then
										k = If(k <= 9, k, CInt(j / 3))
								Else
										k = CInt(Mid(B, If(k <= 9, k * 3 + 2, j + 1), 2))
								End If
								A = A + k
								pari = Not pari
						Next i
						pari = (Mid(espres, 16, 1) = Mid(B, (A Mod 26) * 3 + 1, 1))
						ValidateCodiceFiscaleItaliano = pari
				End If
		End Function

		Public Class DatiCodiceFiscale
				Public dataNascita As SqlDateTime = SqlDateTime.Null
				Public belfioreNascita As SqlString = SqlString.Null
				Public genereMF As SqlString = SqlString.Null
		End Class

		Public Shared Function DatiDaCodiceFiscaleValido(cf As String) As DatiCodiceFiscale


				Dim sCod As String
				Dim sAnno As String
				Dim sMese As String
				Dim sGiorno As String
				Dim iAnno As Integer
				Dim iMese As Integer
				Dim iGiorno As Integer
				Dim sData As String

				Dim output As New DatiCodiceFiscale

				sCod = UCase(cf)
				sAnno = Mid(sCod, 7, 2)
				sMese = Mid(sCod, 9, 1)
				sGiorno = Mid(sCod, 10, 2)

				'DATA E GENERE
				If (Not IsNumeric(sAnno)) Or (Not IsNumeric(sGiorno)) Or (InStr("ABCDEHLMPRST", sMese) = 0) Then
						'rimane tutto nullo
				Else
						'verifichiamo se la data funziona...
						iAnno = CInt(sAnno)

						iMese = InStr("ABCDEHLMPRST", sMese)
						iGiorno = CInt(sGiorno)
						If iGiorno > 40 Then
								output.genereMF = New SqlString("F")
								iGiorno = iGiorno - 40
						Else
								output.genereMF = New SqlString("M")
						End If

						sData = Format(iAnno, "00") & Format(iMese, "00") & Format(iGiorno, "00")
						Try
								output.dataNascita = Date.ParseExact(sData, "yyMMdd", Softailor.Global.Cultures.CulturaItalian)
						Catch ex As Exception
								output.dataNascita = SqlDateTime.Null
						End Try
				End If

				'BELFIORE NASCITA
				output.belfioreNascita = Mid(cf, 12, 4)

				Return output

		End Function

		Public Shared Function ValidateCodiceFiscaleEnte(ByVal CF As String) As Boolean

				'codice fiscale ente ha 11 caratteri numerici, non é un codice fiscale normale a meno che non si tratta di ditta individuale. A volte coincide con la P.IVA

				Dim result As Boolean = False
				Dim X As Integer = 0, Y As Integer = 0, Z As Integer = 0

				If CF = "" Then
						Return True
				End If

				If Len(CF) <> 11 Then
						Return False
				Else
						'verifico che siano tutte cifre
						Dim strRegex As String = "^([0-9]+)$"
						Dim objER As New System.Text.RegularExpressions.Regex(strRegex)
						' verifica la corrispondenza con il pattern
						result = objER.IsMatch(CF)
						If result <> True Then
								objER = Nothing
								Return False
						End If
						Return True
				End If
		End Function

		Public Shared Function ValidatePartitaIVAItaliana(ByVal PI As String) As Boolean

				'nuova versione 15 settembre 2010 da gazzetta ufficiale

				Dim i As Integer
				Dim cifra As Integer, cifraControllo As Integer
				Dim pari As Boolean
				Dim result As Boolean = False
				Dim X As Integer = 0, Y As Integer = 0, Z As Integer = 0

				If PI = "" Then
						Return True
				End If

				If Len(PI) <> 11 Then
						Return False
				Else
						'verifico che siano tutte cifre
						Dim strRegex As String = "^([0-9]+)$"
						Dim objER As New System.Text.RegularExpressions.Regex(strRegex)
						' verifica la corrispondenza con il pattern
						result = objER.IsMatch(PI)
						If result <> True Then
								objER = Nothing
								Return False
						Else

								'OK sono tutte cifre
								'per caratteri da 1 a 10:
								'x = somma cifre in posizione dispari
								'y = somma cifre in posizione pari * 2
								'z = numero cifre in posizione pari maggiori o uguali a 5
								'caratt controllo = 10 - ((x + y + z) mod 10)

								pari = False
								For i = 1 To 10
										cifra = CInt(Mid(PI, i, 1))
										If pari Then
												If cifra > 4 Then
														Dim doppio As Integer = cifra * 2
														Select Case doppio
																Case 10 : Z = Z + 1
																Case 12 : Z = Z + 3
																Case 14 : Z = Z + 5
																Case 16 : Z = Z + 7
																Case 18 : Z = Z + 9
														End Select
												Else
														Y = Y + cifra * 2
												End If
										Else
												X = X + cifra
										End If
										pari = Not pari
								Next i
								cifraControllo = (10 - ((X + Y + Z) Mod 10)) Mod 10
								Return cifraControllo = CInt(Mid(PI, 11, 1))
						End If
				End If
		End Function

		Public Shared Function ValidateIBANItaliano(ByVal pIBAN As String) As Boolean
				Const MOD_NUM As Integer = 97
				Const L_IBAN As Integer = 27
				Dim retValue As Boolean = False
				' trasformiamo in maiuscolo il codice depurato degli spazi
				Dim codiceIBAN As String = pIBAN.Trim().ToUpper()
				' se si ottiene una stringa vuota sono dolori! :)
				If codiceIBAN = String.Empty Then
						Return False
				End If

				' test codice paese
				If Not CheckPaese(codiceIBAN.Substring(0, 2)) Then
						Return True
				End If

				' Controllo lungehzza
				If codiceIBAN.Length <> L_IBAN Then
						Return False
				End If

				Try
						' trasformazione in stringa numerica
						Dim codiceNumerico As String = CalcolaCodiceNumerico(codiceIBAN)
						Dim i As Integer
						' applichiamo il modulo 97 su 6 caratteri alla volta
						While codiceNumerico.Length > 6
								i = Integer.Parse(codiceNumerico.Substring(0, 6)) Mod MOD_NUM
								codiceNumerico = i.ToString() + codiceNumerico.Substring(6)
						End While
						' la stringa restante con lunghezza <= 6 può essere
						' convertita agevolmente
						i = Integer.Parse(codiceNumerico) Mod MOD_NUM
						' se il valore modulo 97 non è uno il check digit
						' dell'IBAN è errato
						If i <> 1 Then
								retValue = True
						Else
								retValue = True
						End If
				Catch ex As Exception
						retValue = False
				End Try
				Return retValue
		End Function

		Private Shared Function CalcolaCodiceNumerico(ByVal codiceIBAN As String) As String
				Dim sb As New StringBuilder()
				Const lettere As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
				' per ogni carattere contenuto nella stringa riformattata per il calcolo
				' troviamo la posizione relativa nella stringa lettere e
				' aggiungiamo il valore ottenuto sull'oggetto stringbuilder
				For Each c As Char In codiceIBAN.Substring(4) + codiceIBAN.Substring(0, 4)
						Dim x As Integer = lettere.IndexOf(c)
						If x = -1 Then
								Throw (New Exception("Caratteri non validi nella stringa"))
						End If
						sb.Append(x)
				Next
				Return sb.ToString()
		End Function

		Private Shared Function CheckPaese(ByVal codicePaese As String) As Boolean
				Const STATI As String = "IT,SM"
				Return (STATI.IndexOf(codicePaese) <> -1)
		End Function

End Class

Public Class EpsonItfBarcode

    Public Shared Function CongressBC2of5(NumeIn As Integer, CodCng As String) As String
        'formazione di una stringa per codice a barre
        'di tipo Interleaved 2 of 5
        'studiata per utilizzare il carattere
        '   EPSON ITF
        '   Nome del file: EPITF.TTF

        'nume: numero da codificare, POSITIVO
        'aggiunge anche il CODICE CONVEGNO e il CHECKSUM

        Dim StrNum As String
        Dim StrTot As String
        Dim i As Integer
        Dim N As Integer
        Dim cm As Integer
        Dim StrOut As String
        Dim CHK As Integer


        StrNum = Trim(Str(NumeIn))                    'sempre dispari
        If Len(StrNum) < 7 Then                     'lunghezza minima 10 char
            StrNum = Right("00000000" & StrNum, 7)
        End If
        If Len(StrNum) Mod 2 = 0 Then StrNum = "0" & StrNum
        StrTot = CodCng & StrNum

        'bene,ora calcoliamo il checksum

        CHK = 0
        For i = 1 To Len(StrTot)
            If i Mod 2 = 1 Then
                CHK = CHK + 3 * CInt(Mid(StrTot, i, 1))
            Else
                CHK = CHK + CInt(Mid(StrTot, i, 1))
            End If
        Next i
        CHK = CHK Mod 10
        CHK = 10 - CHK
        CHK = CHK Mod 10

        StrTot = StrTot & Chr(48 + CHK)

        StrOut = ""

        'Apertura codice:        ASCII 253
        StrOut = Chr(253)
        For i = 1 To Len(StrTot) \ 2
            N = CInt(Mid(StrTot, (i - 1) * 2 + 1, 2))
            Select Case N
                Case 0 To 10
                    '0   10  130 140
                    cm = N - 0 + 130
                Case 11 To 18
                    '11  18  149 156
                    cm = N - 11 + 149
                Case 19
                    '19  19  159 159
                    cm = 159
                Case 20 To 23
                    '20  23  161 164
                    cm = N - 20 + 161
                Case 24 To 40
                    '24  40  166 182
                    cm = N - 24 + 166
                Case 41 To 96
                    '41  96  184 239
                    cm = N - 41 + 184
                Case 97 To 99
                    '97  99  250 252
                    cm = N - 97 + 250
            End Select
            StrOut = StrOut & Chr(cm)
        Next i

        'Chiusura codice:        ASCII 254
        StrOut = StrOut & Chr(254)
        CongressBC2of5 = StrOut

        'Cifre:      ASCII   240 249

    End Function

    Public Shared Function CongressBC2of5_Number(NumeIn As Integer, CodCng As String) As String
        'formazione di una stringa per codice a barre
        'di tipo Interleaved 2 of 5
        'studiata per utilizzare il carattere
        '   EPSON ITF
        '   Nome del file: EPITF.TTF

        'nume: numero da codificare, POSITIVO
        'aggiunge anche il CODICE CONVEGNO e il CHECKSUM

        Dim StrNum As String
        Dim StrTot As String
        Dim i As Integer
        Dim CHK As Integer


        StrNum = Trim(Str(NumeIn))                    'sempre dispari
        If Len(StrNum) < 7 Then                     'lunghezza minima 10 char
            StrNum = Right("00000000" & StrNum, 7)
        End If
        If Len(StrNum) Mod 2 = 0 Then StrNum = "0" & StrNum
        StrTot = CodCng & StrNum

        'bene,ora calcoliamo il checksum

        CHK = 0
        For i = 1 To Len(StrTot)
            If i Mod 2 = 1 Then
                CHK = CHK + 3 * CInt(Mid(StrTot, i, 1))
            Else
                CHK = CHK + CInt(Mid(StrTot, i, 1))
            End If
        Next i
        CHK = CHK Mod 10
        CHK = 10 - CHK
        CHK = CHK Mod 10

        StrTot = StrTot & Chr(48 + CHK)

        Return StrTot

    End Function

    Public Shared Function IdFromBarcode(BC As String, CodCon As String) As Integer

        'lettura di un barcode
        'formato del barcode: xxxxn*C
        'dove: xxxx   = codice convegno
        '      n*     = ID del partecipante
        '      C      = checksum
        'restituisce:
        '   -1 se c'è un errore di lettura (barcode vuoto, carattere di controllo non valido)
        '   -2 se il barcode è di un altro congresso
        '   altrimenti l'ID del personaggio in questione, AMMESSO CHE ESISTA

        Dim BcExam As String
        Dim Caratt As String
        Dim i As Integer
        Dim LunCod As Integer
        Dim CHK As Integer
        Dim sId As String

        'pulizia
        BcExam = ""
        For i = 1 To Len(BC)
            Caratt = Mid(BC, i, 1)
            If (Caratt >= "0") And (Caratt <= "9") Then
                BcExam = BcExam & Caratt
            End If
        Next i

        LunCod = Len(BcExam)

        If (LunCod < 6) Or (LunCod Mod 2 <> 0) Then 'barcode troppo corto o non pari
            IdFromBarcode = -1
        Else    'lunghezza sufficiente e barcode pari
            'calcoliamo il checksum
            CHK = 0
            For i = 1 To LunCod - 1
                If i Mod 2 = 1 Then
                    CHK = CHK + 3 * CInt(Mid(BcExam, i, 1))
                Else
                    CHK = CHK + CInt(Mid(BcExam, i, 1))
                End If
            Next i
            CHK = CHK Mod 10
            CHK = 10 - CHK
            CHK = CHK Mod 10
            If CInt(Mid(BcExam, LunCod, 1)) <> CHK Then
                IdFromBarcode = -1
            Else

                If Left(BcExam, 4) <> CodCon Then   'codice convegno sbagliato
                    IdFromBarcode = -2
                Else    'convegno giusto
                    sId = Mid(BcExam, 5, Len(BcExam) - 5)
                    If Len(sId) <= 9 Then
                        IdFromBarcode = CInt(sId)
                    Else
                        IdFromBarcode = -1
                    End If
                End If
            End If
        End If
    End Function

End Class

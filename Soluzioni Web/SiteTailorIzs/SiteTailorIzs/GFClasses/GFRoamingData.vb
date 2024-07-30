Imports System.Web.HttpContext

Public Class GFRoamingData

    Public originiIscrizione As New List(Of String)
    Public originiIscrizioneSet As Boolean = False
    Private Const originiIscrizioneKey As String = "RDOI"

    Public categorieEcm As New List(Of String)
    Public categorieEcmSet As Boolean = False
    Private Const categorieEcmKey As String = "RDCE"

    Public statiIscrizione As New List(Of String)
    Public statiIscrizioneSet As Boolean = False
    Private Const statiIscrizioneKey As String = "RDSI"

    Public statiEcm As New List(Of String)
    Public statiEcmSet As Boolean = False
    Private Const statiEcmKey As String = "RDSE"

    Public statiQuestionario As New List(Of String)
    Public statiQuestionarioSet As Boolean = False
    Private Const statiQuestionarioKey As String = "RDSQ"

    Public statiPresenza As New List(Of String)
    Public statiPresenzaSet As Boolean = False
    Private Const statiPresenzaKey As String = "RDSP"

    Public categoriePersone As New List(Of String)
    Public categoriePersoneSet As Boolean = False
    Private Const categoriePersoneKey As String = "RDDE"

    Public cognomeSet As Boolean = False
    Public cognome As String = ""
    Private Const cognomeKey As String = "CP"

    Public nomeSet As Boolean = False
    Public nome As String = ""
    Private Const nomeKey As String = "NP"

    Public ordinamento As String = "CN"
    Private Const ordinamentoKey As String = "OB"

    Public Function GetRequestData() As String

        Dim s As String = ""

        s &= ordinamentoKey & "=" & ordinamento
        If cognomeSet Then s &= cognomeKey & Current.Server.UrlEncode(cognome)
        If nomeSet Then s &= cognomeKey & Current.Server.UrlEncode(nome)
        If originiIscrizioneSet Or originiIscrizione.Count > 0 Then s = Enqueue(s, originiIscrizione, originiIscrizioneKey)
        If categorieEcmSet Or categorieEcm.Count > 0 Then s = Enqueue(s, categorieEcm, categorieEcmKey)
        If statiIscrizioneSet Or statiIscrizione.Count > 0 Then s = Enqueue(s, statiIscrizione, statiIscrizioneKey)
        If statiEcmSet Or statiEcm.Count > 0 Then s = Enqueue(s, statiEcm, statiEcmKey)
        If statiQuestionarioSet Or statiQuestionario.Count > 0 Then s = Enqueue(s, statiQuestionario, statiQuestionarioKey)
        If statiPresenzaSet Or statiPresenza.Count > 0 Then s = Enqueue(s, statiPresenza, statiPresenzaKey)
        If categoriePersoneSet Or categoriePersone.Count > 0 Then s = Enqueue(s, categoriePersone, categoriePersoneKey)
        Return s

    End Function

    Public Sub New()

    End Sub

    Public Sub New(R As HttpRequest)

        Me.New()

        With R.QueryString
            Me.ordinamento = .Item(ordinamentoKey)

            If .AllKeys.Contains(cognomeKey) Then
                Me.cognomeSet = True
                Me.cognome = .Item(cognomeKey)
            End If

            If .AllKeys.Contains(nomeKey) Then
                Me.nomeSet = True
                Me.nome = .Item(nomeKey)
            End If

            If .AllKeys.Contains(originiIscrizioneKey) Then
                Me.originiIscrizioneSet = True
                Me.originiIscrizione = Dequeue(.Item(originiIscrizioneKey))
            End If

            If .AllKeys.Contains(categorieEcmKey) Then
                Me.categorieEcmSet = True
                Me.categorieEcm = Dequeue(.Item(categorieEcmKey))
            End If

            If .AllKeys.Contains(statiIscrizioneKey) Then
                Me.statiIscrizioneSet = True
                Me.statiIscrizione = Dequeue(.Item(statiIscrizioneKey))
            End If

            If .AllKeys.Contains(statiEcmKey) Then
                Me.statiEcmSet = True
                Me.statiEcm = Dequeue(.Item(statiEcmKey))
            End If

            If .AllKeys.Contains(statiQuestionarioKey) Then
                Me.statiQuestionarioSet = True
                Me.statiQuestionario = Dequeue(.Item(statiQuestionarioKey))
            End If

            If .AllKeys.Contains(statiPresenzaKey) Then
                Me.statiPresenzaSet = True
                Me.statiPresenza = Dequeue(.Item(statiPresenzaKey))
            End If

            If .AllKeys.Contains(categoriePersoneKey) Then
                Me.categoriePersoneSet = True
                Me.categoriePersone = Dequeue(.Item(categoriePersoneKey))
            End If

        End With

    End Sub

    Private Function Enqueue(sIn As String, l As List(Of String), key As String) As String

        Dim sOut As String = sIn

        sOut &= "&"
        sOut &= key & "="
        For Each value In l
            sOut &= value & "\"
        Next
        Return sOut

    End Function

    Private Function Dequeue(sIn As String) As List(Of String)

        Dim lOut As New List(Of String)

        For Each s As String In sIn.Split("\"c)
            If s <> String.Empty Then lOut.Add(s)
        Next

        Return lOut

    End Function

End Class

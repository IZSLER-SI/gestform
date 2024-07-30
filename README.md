# Gestionale della Formazione

## About
Il Gestionale della Formazione è un software che consente la gestione di tutti i dati relativi agli eventi progettati gestiti ed erogati dall’Ufficio Formazione e la rendicontazione dei dati di fruizione all’Organismo di Accreditamento ECM AGENAS. Si integra con le altre soluzioni per le diverse specifiche funzioni attraverso Web-Service e Middleware appositamente realizzati.

Il gestionale può essere interfacciato con il sistema di gestione delle risorse umane da cui attingerà quotidianamente i dati anagrafici del personale dipendente mantenendo aggiornata l’anagrafica di base del personale interno per l’ufficio della formazione. Lo scambio delle informazioni avviene a mezzo routine di allineamento dei Database. 

I dati anagrafici delle persone che accedono alla formazione come esterni, invece sono inseriti e aggiornati direttamente dagli utenti che si registrano sul portale della formazione. Questo portale è il front-end del gestionale e costituisce una vera e propria vetrina dei corsi con funzionalità dedicate agli utenti registrati e ai dipendenti.
Si tratta di un CMS accessibile via WEB in cui vengono pubblicati i dati degli eventi formativi e che consente la raccolta delle iscrizioni, la rendicontazione da remoto delle presenze da parte dei tutor, l’invio di promemoria ai partecipanti e l’accesso in SSO al software Valutazione-Web, alla Piattaforma di Formazione a Distanza e agli altri applicativi integrati con il gestionale.

Gli utenti esterni si registrano con i propri dati da questo sito, i dipendenti e gli esterni si iscrivono ai corsi e, sempre dallo stesso portale, accedono agli altri applicativi della formazione. Oltre al catalogo dell’offerta formativa, gli utenti trovano i propri attestati, il proprio portfolio formativo aggiornato e strumenti di interazione che consentono di candidarsi come docenti, di rendicontare le presenze dei corsisti per gli eventi per cui svolgono la funzione di Tutor, di richiedere l’autorizzazione a svolgere formazione esterna finanziata dall’Istituto(solo per i dipendenti) e di autocertificare la formazione individuale svolta presso altri enti senza la partecipazione economica dell’Istituto(solo per i dipendenti). 

Sul portale sono pubblicati documenti di natura generale, news e allegati che concernono il sistema della formazione oltre al materiale didattico specifico di ogni evento formativo gestito.

## Prerequisiti
1. È necessario disporre di un account Microsoft Office 365, altrimenti l'applicativo web non funzionerà
2. Installare LibreOffice sul server, anche in versione portable, necessario per convertire alcuni documenti in PDF
3. Installare i font **Epson ITF** (EPITF.TTF) e **Softailor ITF** (STLITF.TTF) per la stampa corretta dei report barcode. I file si trovano nella cartella Others.
    Per installare i font, TASTO DESTRO > INSTALLA PER TUTTI GLI UTENTI
4. Installare SAP Crystal Reports runtime engine for .NET Framework (32-bit): il file è nella cartella Others e si chiama _CRRuntime_32bit_13_0_21.msi_
5. Installare IIS URL Rewrite Module 2: il file è nella cartella Others e si chiama rewrite_x64_it-IT.msi
6. Installare SharePoint Client Components: il file è nella cartella Others e si chiama sharepointclientcomponents_x64.msi

## Ambiente di sviluppo
**IDE**: Visual Studio (Community) 2015 (ad esempio Version 14.0.254431.01 Update 3)

**.NET Framework**: Version 4.5

## Impostazione dei permessi IIS
### Ambiente di sviluppo
Viene utilizzatto IIS Express, eseguito direttamente dall'IDE di sviluppo

### Ambiente di test/produzione
1. Clonare il repository. Tramite l'IDE, creare i **publish** dei progetti
2. Creare la struttura delle cartelle server. Questa la configurazione
    ```
    ├── inetpub
        └── GestioneFormazione
            └── BackOffice
            └── BinaryData
                └── AZI_1
                └── DEFTHUMBS
            └── FrontOffice
    ```
3. Impostare IIS, in particolare *Application pool/Pool di applicazioni* e *Site/Siti*
    1. Nell'*Application pool/Pool di applicazioni* in Impostazioni Avanzate, abilitare le applicazioni a 32-bit 
4. Impostare i permessi alle cartelle, in particolare i permessi di scrittura per l'utente IIS sui BinaryData

### Database
Creare il database dell'applicativo, in questo caso chiamato GestioneFormazione
```sql
CREATE DATABASE [GestioneFormazione]
```
Una volta creato il database creare
- gli utenti ed i ruoli, definendo anche le associazioni
```sql
CREATE USER [GestioneFormazioneService] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [GestioneFormazioneReporter] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [GestioneFormazionePublisher] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE USER [GestioneFormazionePublic] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
CREATE ROLE [WebService]
GO
CREATE ROLE [WebReporter]
GO
CREATE ROLE [WebPublisher]
GO
CREATE ROLE [WebPublic]
GO
ALTER ROLE [WebService] ADD MEMBER [GestioneFormazioneService]
GO
ALTER ROLE [WebReporter] ADD MEMBER [GestioneFormazioneReporter]
GO
ALTER ROLE [WebPublisher] ADD MEMBER [GestioneFormazionePublisher]
GO
ALTER ROLE [WebPublic] ADD MEMBER [GestioneFormazionePublic]
GO
```
- i datatype personalizzati
```sql
CREATE TYPE [dbo].[Generic4IntList] AS TABLE(
	[Int1] [int] NOT NULL,
	[Int2] [int] NOT NULL,
	[Int3] [int] NOT NULL,
	[Int4] [int] NOT NULL
)
GO
CREATE TYPE [dbo].[GenericIntList] AS TABLE(
	[VALO_INT] [int] NULL
)
GO
CREATE TYPE [dbo].[GenericIntStringStringIntList] AS TABLE(
	[VALO_INT] [int] NOT NULL,
	[VALO_ST1] [nvarchar](50) NOT NULL,
	[VALO_ST2] [nvarchar](50) NOT NULL,
	[VALO_IN2] [int] NOT NULL
)
GO
CREATE TYPE [dbo].[GenericStringList] AS TABLE(
	[VALO_STR] [nvarchar](200) NULL
)
GO
CREATE TYPE [dbo].[gf_ParticipantsEntryExitList_TableType] AS TABLE(
	[id_ISCRITTO] [int] NOT NULL,
	[tx_COGNOME] [nvarchar](100) NOT NULL,
	[tx_NOME] [nvarchar](100) NOT NULL,
	[dt_DATA] [date] NOT NULL,
	[tm_INIZIO] [datetime] NOT NULL,
	[tm_FINE] [datetime] NOT NULL
)
GO
CREATE TYPE [dbo].[RapportoLavorativoTableType] AS TABLE(
	[ac_MATRICOLA] [nvarchar](16) NULL,
	[ac_UNITAOPERATIVA] [nvarchar](16) NULL,
	[ac_CATEGORIALAVORATIVA] [nvarchar](8) NULL,
	[ac_PROFESSIONE] [nvarchar](10) NULL,
	[id_DISCIPLINA] [int] NULL,
	[ac_PROFILO] [nvarchar](20) NULL,
	[ac_RUOLO] [nvarchar](4) NULL,
	[ac_TIPOCONTRATTO] [nvarchar](8) NULL,
	[ac_CATEGORIACONTRATTO] [nvarchar](8) NULL,
	[ac_FASCIACONTRATTO] [nvarchar](8) NULL,
	[tx_NOTEGESTPERSONALE] [nvarchar](400) NULL
)
GO
CREATE TYPE [dbo].[RecapitoTableType] AS TABLE(
	[ac_TIPOINDIRIZZO] [nvarchar](4) NULL,
	[tx_INDIRIZZO] [nvarchar](300) NULL,
	[ac_CAP] [nvarchar](5) NULL,
	[tx_POSTALCODE] [nvarchar](20) NULL,
	[ac_COMUNE] [nvarchar](4) NULL,
	[tx_LOCALITA] [nvarchar](100) NULL,
	[tx_CITY] [nvarchar](150) NULL,
	[tx_PROVINCIA] [nvarchar](50) NULL,
	[tx_STATO] [nvarchar](50) NULL,
	[ac_NAZIONE] [nvarchar](4) NULL
)
GO
```
Il file .sql, contenente lo scheletro dell'applicativo, si trova nella cartella Others.

### Impostazione dei web.config
#### GestioneFormazione.FrontOffice
1. Modificare questi parametri direttamente nel Web.config
- Configurare la sezione `<mailSettings>`
    - Impostare _host_ (ad esempio: smtp.office365.com)
    - Impostare _port_ (ad esempio: 587)
    - Impostare, con gli stessi valori, _userName_ e _from_
    - Impostare _password_
- Configurare la sezione `<appSettings>` con i dati di accesso a OneDrive
    - Impostare _sharepoint_site_root_
    - Impostare _onedrive_site_relativebase_ (solitamente inizia con /personal/)
    - Impostare _onedrive_site_root_ (solitamente è la combinazione delle chiavi precedenti)
    - Impostare _onedrive_username_
    - Impostare _onedrive_password_
    - Impostare _onedrive_skipbasefolderscreation_
        - **Primo accesso al sito** = 0: verrà creata automaticamente la struttura in Onedrive
        - **Accessi successivi** = 1: dopo il primo accesso, il processo può essere saltato
- Configurare le `<connectionStrings>` (utilizzare lo stesso utente)
    - Impostare _ConnectionString_
    - Impostare _WsConnectionString_
2. Modificare questi valori da IDE nel file My Project > Settings.settings . Saranno automaticamente aggiornati i valori nel Web.config
- Configurare la sezione `<applicationSettings>` con alcuni dati generici dell'applicativo
    - Impostare _ErrorReportMailFrom_ con il valore desiderato
    - Impostare _ErrorReportMailTo_ con il valore desiderato
    - Impostare _PageTitle_ : è il title della pagina, visualizzato nel browser
    - Impostare _CompanyKey_ : valore fondamentale, è utilizzato per i report e per accedere a cartelle specifiche (es: /Content/IZSLER, template Templates/IZSLER)
        - Una volta definito il parametro, modificare la regola di rewrite url `<rule name="Riscrittura immagini aziendali"` specificando, come URL, url="/Content/**valore di CompanyKey**/CImg/{R:1}"
    - Impostare _CompanyName_Short_ 
    - Impostare _PasswordResetKey_
    - Impostare _FrontOfficeUrl_ : URL del portale FrontOffice
    - Impostare _GenericMail_RagioneSociale_ : ragione sociale che comparirà nell'email
    - Impostare _GenericMail_IndirizzoCompleto_ : indirizzo email che comparirà nell'email
    - Impostare _GenericMail_Tel_ : impostare telefono che comparirà nell'email
    - Impostare _GenericMail_Fax_ : impostare fax che comparirà nell'email
    - Impostare _GenericMail_MailFrom_: impostare mail che invia le comunicazioni
    - Impostare _ValutazioneWeb_Key_:
    - Impostare _SofficeExePath_: impostare il path all'eseguibile Libre Office, installato nei prerequisiti (ad esempio: C:\Program Files\LibreOffice\program\soffice.exe)
    - Impostare _AppDataTempPath_: (ad esempio: "C:\inetpub\temp\libreoffice". NB: i doppi apici sono obbligatori)
    - Impostare _BinariesTmpBasePath_: (ad esempio : C:\emailsent\izsler)
    - Impostare _DEBUG_FLG_: utile per testare l'invio dell'email ad indirizzi di sviluppo per la formazione esterna. Valori accettati: 0 (disattivato) - 1 (attivato)
        - Impostare _DEBUG_EMAIL_FROM_: mail di test che invia le informazioni sulla formazione esterna
        - Impostare _DEBUG_EMAIL_TO_: mail di test che riceve le informazioni sulla formazione esterna
    - Impostare _formazione_ext_path_56B_002_: template **DOCX** per formazione esterna (il nome del file potrebbe essere 56B.002_rev02.docx). 
        Il file dovrà essere caricato su Microsoft Office 365 nel path Documents/GestioneFormazione/Modelli/Richieste_PartecipazioneExt/
    - Impostare _formazione_ext_path_56C_002_: template **DOCX** per formazione esterna (il nome del file potrebbe essere 56C.002_rev02.docx)
        Il file dovrà essere caricato su Microsoft Office 365 nel path Documents/GestioneFormazione/Modelli/Richieste_PartecipazioneExt/
    - Impostare _formazione_ext_path_56D_002_: template **DOCX** per formazione esterna (il nome del file potrebbe essere 56D.002_rev02.docx)
        Il file dovrà essere caricato su Microsoft Office 365 nel path Documents/GestioneFormazione/Modelli/Richieste_PartecipazioneExt/
    - Impostare _ErrorReportMailSubject_ : è l'oggerro con cui saranno spedite le mail di errore
    - Impostare _BinariesBasePath_ : path assoluto alla cartella \BinaryData\AZI_1\
    - Impostare _BinariesBasePath_WoAzi_ : path assoluto alla cartella \BinaryData
    - Impostare _ReportsBasePath_ : path assoluto alla cartella \Reports
    - Impostare _ValutazioneWeb_Url_ : URL dell'applicativo web Valutazione Web

#### SiteTailorIzs (BackOffice)
Per modificare il file Web.config accedere da IDE a My Project > Settings.settings
- Configurare la sezione `<mailSettings>`
    - Impostare _host_ (ad esempio: smtp.office365.com)
    - Impostare _port_ (ad esempio: 587)
    - Impostare, con gli stessi valori, _userName_ e _from_
    - Impostare _password_
- Configurare la sezione `<appSettings>` con i dati di accesso a OneDrive
    - NON MODIFICARE
        - aspnet:MaxHttpCollectionKeys
        - sitetailor_SGLAPPLI
        - sitetailor_DESAPPLI
        - sitetailor_CustomContextHandler1
    - Impostare _sharepoint_site_root_
    - Impostare _onedrive_site_relativebase_ (solitamente inizia con /personal/)
    - Impostare _onedrive_site_root_ (solitamente è la combinazione delle chiavi precedenti)
    - Impostare _onedrive_username_
    - Impostare _onedrive_password_
    - Impostare _onedrive_skipbasefolderscreation_
        - **Primo accesso al sito** = 0: verrà creata automaticamente la struttura in Onedrive
        - **Accessi successivi** = 1: dopo il primo accesso, il processo può essere saltato
    - Impostare _sitetailor_ErrorReportMailFrom_ : indirizzo mail da cui vengono inviati gli errori dell'applicativo
    - Impostare _sitetailor_ErrorReportMailTo_ : indirizzo mail a cui vengono inviati gli errori dell'applicativo
    - Impostare _sitetailor_ErrorReportMailSubject_ : oggetto delle mail per gli errori dell'applicativo (ad esempio: "Errore GestForm **nome significativo del portale**")
    - Impostare _sitetailor_BinariesBasePath_ : è il path assoluto alla cartella BinaryData
    - Impostare _GF_MailFrom_ : indirizzo mail da cui vengono inviate le comunicazioni all'esterno
    - Impostare _GF_FrontofficeBasePath_FromWeb_ : DNS dell'applicativo FrontOffice
- Configurare la sezione `<applicationSettings> -> <Softailor.SiteTailorIzs.My.MySettings>`
    - Impostare _CompanyName_ : parametro utilizzato per accedere ai report e ad alcune cartelle contenente CSS ed immagini
    - Impostare _BccMail_ : indirizzo email che riceverà, silentemente, delle notifiche
    - Impostare _GenericMail_MailFrom_ : impostare mail che invia le comunicazioni
    - Impostare _GenericMail_MailTo_ : impostare mail che riceve le comunicazione

### Impostare i progetti
Il procedimento di seguito serve per configurare i progetti **GestioneFormazione.FrontOffice** e **SiteTailorIzs (BackOffice)** a seguito delle impostazioni di Settings.settings (ad esempio: _CompanyName_) o, più in generale, per configurare i progetti
#### GestioneFormazione.FrontOffice
```
├── FrontOffice
    └── ...
    └── Content
        └── Settings.settings > valore di CompanyName
            └── CImg
            └── FavIcon
            └── Styles
    └── Files
    └── Img
        └── Settings.settings > valore di CompanyName
    └── ...
    └── Templates
        └── Settings.settings > valore di CompanyName
```
- Template/Settings.settings > valore di CompanyName/Mail/NotificaModuloFormazioneEsterna.xslt: modificare l'indirizzo email (placeholder MY-MAIL)
- Template/Settings.settings > valore di CompanyName/Footer.xslt: 
- Content/Settings.settings > valore di CompanyName/CImg/Mail/LogoHdr.png: aggiungere, mantenendo la stessa forma, il logo desiderato
- Content/Settings.settings > valore di CompanyName/CImg/Header.png: aggiungere, mantenendo la stessa forma, il logo desiderato

#### SiteTailorIzs (BackOffice)
```
├── BackOffice
    └── ...
    └── GFTemplates
        └── Mail
            └── Settings.settings > valore di CompanyName_AcceptanceConfirmation.xslt
            └── Settings.settings > valore di CompanyName_NotAccepted.xslt
            └── Settings.settings > valore di CompanyName_NotificaModProfiloUfficio.xslt
            └── Settings.settings > valore di CompanyName_ParticipationReminder.xslt
            └── Settings.settings > valore di CompanyName_ParticipationReminderDRM.xslt
    └── ...
    └── Reports
        └── rptAttestatoPartecipazione_Settings.settings > valore di CompanyName.rpt